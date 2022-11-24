using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
	CharacterController cc;
	Animator animator;
	new AudioSource audio;
	
	Transform gunBarrelEnd;
	
	public float moveSpeed = 2f;
	
	float startY;
	
	public GameObject bulletEffect;
	public float fireRate = 2f;
	float lastFire;
	public float fireRange = 32f;
	
	void Awake() {
		cc = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
		
		startY = transform.localPosition.y;
		
		gunBarrelEnd = transform.Find("GunBarrelEnd");
		
		lastFire = Time.time;
	}
	
	void Update() {
		// Move around..
		bool isWalking = Move();
		animator.SetBool("IsWalking", isWalking);
		LookAtMouse();
		
		// Shoot at a fixed interval...
		float fireTime = Time.time - lastFire;
		if (fireTime >= fireRate) {
			if (Input.GetButton("Fire1")) {
				Shoot();
				lastFire += fireRate;
			} else {
				lastFire = Time.time - fireRate * 1.1f;
			}
		}
		
		// Scare enemies..
		if (Input.GetButtonDown("Scare") || Input.GetButtonUp("Scare")) {
			GameObject.Find("Enemies")
				.SendMessage("Scare", SendMessageOptions.DontRequireReceiver);
		}
		
		// Constant Y coordinate ( this is a 2D game now )
		Vector3 a = this.transform.position;
		a.y = startY; this.transform.position = a;
	}
	
	// Moves the character controller in response to WASD stuff.
	// Returns if you moved a non-zero amount.
	bool Move() {
		Vector2 wasd = new Vector2(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")
		).normalized;
		
		Vector3 move = new Vector3(wasd.x, 0f, wasd.y) * moveSpeed;
		
		cc.Move(move * Time.deltaTime);
		
		return !Mathf.Approximately(move.magnitude, 0f);
	}
	
	// Rotates the character towards the place the mouse is over.
	void LookAtMouse() {
		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane xzPlane = new Plane(Vector3.up, 0f);
		Vector3 direction = IntersectRayWithPlane(cameraRay, xzPlane).GetValueOrDefault();
		direction -= transform.position; direction.y = 0;
		transform.localRotation = Quaternion.LookRotation(direction);
	}
	
	// Shoots a bullet at enemies.
	void Shoot() {
		RaycastHit hit;
		Vector3 target;
		if (Physics.Raycast(
			gunBarrelEnd.transform.position,
			gunBarrelEnd.transform.forward,
			out hit, fireRange,
			1 << LayerMask.NameToLayer("Shoot"),
			QueryTriggerInteraction.Collide
		)) {
			hit.collider.BroadcastMessage(
				"Hurt", Random.Range(0.9f, 1.2f),
				SendMessageOptions.DontRequireReceiver
			);
			target = hit.point;
		} else {
			target = gunBarrelEnd.transform.position +
				gunBarrelEnd.transform.forward * fireRange;
		}
		
		// Make a "bullet effect", which is the line that appears when you shoot.
		GameObject bullet = Instantiate(bulletEffect);
		LineRenderer line = bullet.GetComponent<LineRenderer>();
		Vector3[] positions = { gunBarrelEnd.transform.position, target };
		line.SetPositions(positions);
		
		// Pew pew sound
		audio.pitch = Random.Range(0.9f, 1.1f);
		audio.Play();
	}
	
	// Don't want to bother with collision layers, so I'm just gonna
	// steal from this: https://stackoverflow.com/a/58819973/
	//
	// Returns the point at which the ray intersects the plane.
	Vector3? IntersectRayWithPlane(Ray ray, Plane plane) {
		// How similar is the ray's direction to the plane's normal vector?
		// (We have to flip the normal result the ray points from the origin
		//  down to the plane, and the plane points up from its surface.)
		float directionSimilarity = -Vector3.Dot(plane.normal, ray.direction);
		
		// Prevent Divide-by-Zero when ray is perpendicular to plane.
		if (Mathf.Approximately(Mathf.Abs(directionSimilarity), 0f))
			return null;
		
		// I hate mathematicians and their single-letter variables.
		float rayIntersectLength =
			(Vector3.Dot(plane.normal, ray.origin) + plane.distance) / directionSimilarity;
		// == The "Do you understand the code you copy" Corner ==
		// - Flips sign because ray points against plane normal.
		// - Gets similarity between the plane's normal and the ray's origin,
		//   (but note that ray's origin isn't necessarily normalized! it's
		//    a position, not a direction...)
		
		// Oh I see, it's dividing "the similarity between the ray's direction
		// and the plane's normal" against "the similarity between the ray's
		// position and the plane's normal", and offsetting it by the plane's
		// distance from the origin.
		// Can't quite follow it all the way, but it makes a bit more sense.
		
		// Ignore intersection if ray starts and ends below the plane.
		if (rayIntersectLength < 0f) return null;
		
		return ray.origin + rayIntersectLength * ray.direction;
	}
	
	// Message from WarpZone, will correctly warp the player to the new position.
	void WarpTo(Vector3 warpPosition) {
		Collider c = this.GetComponent<Collider>();
		cc.enabled = false;
		transform.position = warpPosition;
		cc.enabled = true;
	}
}
