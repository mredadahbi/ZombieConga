using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ZombieController : MonoBehaviour {

  public float moveSpeed;
  public float turnSpeed;
  private int lives = 3;

  private Vector3 moveDirection;
  private List<Transform> congaLine = new List<Transform> ();

  [SerializeField]
  private PolygonCollider2D[]
    colliders;
  private int currentColliderIndex = 0;

  private bool isInvincible = false;
  private float timeSpentInvincible;

  public AudioClip enemyContactSound;
  public AudioClip catContactSound;

  // Use this for initialization
  void Start () {
    moveDirection = Vector3.right;
  }
	
  // Update is called once per frame
  void Update () {
    Vector3 currentPosition = transform.position;
    // 2
    if (Input.GetButton ("Fire1")) {
      // 3
      Vector3 moveToward = Camera.main.ScreenToWorldPoint (Input.mousePosition);
      // 4
      moveDirection = moveToward - currentPosition;
      moveDirection.z = 0; 
      moveDirection.Normalize ();
    }

    Vector3 target = moveDirection * moveSpeed + currentPosition;
    transform.position = Vector3.Lerp (currentPosition, target, Time.deltaTime);

    float targetAngle = Mathf.Atan2 (moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
    transform.rotation = 
			Quaternion.Slerp (transform.rotation, 
			                 Quaternion.Euler (0, 0, targetAngle), 
			                 turnSpeed * Time.deltaTime);

    EnforceBounds ();

    if (isInvincible) {
      timeSpentInvincible += Time.deltaTime;     
      if (timeSpentInvincible < 3f) {
        float remainder = timeSpentInvincible % .3f;
        renderer.enabled = remainder > .15f; 
      } else {
        renderer.enabled = true;
        isInvincible = false;
      }
    }
  }

  public void SetColliderForSprite (int spriteNum) {
    colliders [currentColliderIndex].enabled = false;
    currentColliderIndex = spriteNum;
    colliders [currentColliderIndex].enabled = true;
  }

  void OnTriggerEnter2D (Collider2D other) {
    if (other.CompareTag ("cat")) {
      audio.PlayOneShot (catContactSound);
      Transform followTarget = congaLine.Count == 0 ? transform : congaLine.Last ();
      congaLine.Add (other.transform);
      other.transform.parent.GetComponent<CatController> ().JoinConga (followTarget, this.moveSpeed, this.turnSpeed);
      if (congaLine.Count >= 5) {
        Debug.Log ("You won!");
        Application.LoadLevel ("WinScene");
      }
    } else if (!isInvincible && other.CompareTag ("enemy")) {
      audio.PlayOneShot (enemyContactSound);
      isInvincible = true;
      timeSpentInvincible = 0;
      for (int i = 0; i < 2 && congaLine.Count > 0; i++) {
        int lastIdx = congaLine.Count - 1;
        Transform cat = congaLine [lastIdx];
        congaLine.RemoveAt (lastIdx);
        cat.parent.GetComponent<CatController> ().ExitConga ();
      }
      if (--lives <= 0) {
        Debug.Log ("You lost!");
        Application.LoadLevel ("LoseScene");
      }
    }
  }

  private void EnforceBounds () {
    Vector3 newPosition = transform.position; 
    Camera mainCamera = Camera.main;
    Vector3 cameraPosition = mainCamera.transform.position;

    float xDist = mainCamera.aspect * mainCamera.orthographicSize; 
    float xMax = cameraPosition.x + xDist;
    float xMin = cameraPosition.x - xDist;

    if (newPosition.x < xMin || newPosition.x > xMax) {
      newPosition.x = Mathf.Clamp (newPosition.x, xMin, xMax);
      moveDirection.x = -moveDirection.x;
    }

    float yDist = mainCamera.orthographicSize;
    float yMax = cameraPosition.y + yDist;
    float yMin = cameraPosition.y - yDist;

    if (newPosition.y < yMin || newPosition.y > yMax) {
      newPosition.y = Mathf.Clamp (newPosition.y, yMin, yMax);
      moveDirection.y = -moveDirection.y;
    }

    transform.position = newPosition;
  }
}
