using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour {
	CharacterController cc;
	Animator animator;
	new AudioSource audio;
	
	Transform gunBarrelEnd;
	
	bool alive = true;
	
	public float moveSpeed = 2f;
	
	float startY;
	
	public GameObject bulletEffect;
	public float fireRate = 2f;
	float lastFire;
	public float fireRange = 32f;
	
	int killCount_ = 0;
	public int killCount { get => killCount_; }
	
	public float strongTimeLength = 7f;
	
	bool strong = false;
	public bool isStrong { get => strong; }
	float strongStartTime = 0f;
	
	void Awake() {
		cc = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
		
		startY = transform.localPosition.y;
		
		gunBarrelEnd = transform.Find("GunBarrelEnd");
		
		lastFire = Time.time - fireRate;
	}
	
	void Update() {
		if (!alive) return;
		
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
		
		// Constant Y coordinate ( this is a 2D game now )
		Vector3 a = this.transform.position;
		a.y = startY; this.transform.position = a;
		
		// Limit powerup time.
		if (strong && (Time.time - strongStartTime) >= strongTimeLength)
			strong = false;
	}
	
	// Moves the character controller in response to WASD stuff.
	// Returns true if you moved a non-zero amount.
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
			float strongMultiplier = strong ? 2f : 1f;
			hit.collider.BroadcastMessage(
				"Hurt", (this.gameObject, Random.Range(0.9f, 1.2f) * strongMultiplier),
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
	
	// Message from Powerup if you should become strong!
	void Strong() {
		strong = true;
		strongStartTime = Time.time;
	}
	
	// Message from enemies when you kill them!
	void Killer() {
		killCount_++;
	}
	
	// Message from Health, if player runs out of HP...
	void Die() {
		animator.Play("Death");
		alive = false;
		
		if (killCount > PlayerPrefs.GetInt("BestKills", 0)) {
			PlayerPrefs.SetInt("BestKills", killCount);
			// and then burst of confetti or whatever
		}
	}
	
	// Built-in animation event when dying...
	void RestartLevel() {
		Debug.Log("your'e daead.");
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
