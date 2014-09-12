using UnityEngine;
using System.Collections;

public class CatUpdater : MonoBehaviour {

  private CatController catController;
  
  // Use this for initialization
  void Start () {
    catController = transform.parent.GetComponent<CatController> ();  
  }
  
  void UpdateTargetPosition () {
    catController.UpdateTargetPosition ();
  }

  void OnBecameInvisible () {
    catController.OnBecameInvisible ();
  }

  void GrantCatTheSweetReleaseOfDeath () {
    DestroyObject (gameObject);
  }
}
