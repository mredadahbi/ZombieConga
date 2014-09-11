using UnityEngine;
using System.Collections;

public class BackgroundRepeater : MonoBehaviour {

  private Transform cameraTransform;
  private float spriteWidth;

  void Start () {
    cameraTransform = Camera.main.transform;
    SpriteRenderer spriteRenderer = renderer as SpriteRenderer;
    spriteWidth = spriteRenderer.sprite.bounds.size.x;
  }
  
  void Update () {
    if ((transform.position.x + spriteWidth) < cameraTransform.position.x) {
      Vector3 newPos = transform.position;
      newPos.x += 2.0f * spriteWidth; 
      transform.position = newPos;
    }
  }
}
