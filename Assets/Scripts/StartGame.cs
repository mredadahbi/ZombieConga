using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

  void Start () {
    Invoke ("LoadLevel", 3f);
  }

  void LoadLevel () {
    Application.LoadLevel ("CongaScene");
  }
}
