using UnityEngine;
using System.Collections;

public class KittyCreator : MonoBehaviour {

  public float minSpawnTime = 0.75f; 
  public float maxSpawnTime = 2f;
  public GameObject catPrefab;


  void Start () {
    Invoke ("SpawnCat", minSpawnTime);
  }

  void SpawnCat () {
    Camera camera = Camera.main;
    Vector3 cameraPos = camera.transform.position;
    float xMax = camera.aspect * camera.orthographicSize;
    float xRange = camera.aspect * camera.orthographicSize * 1.75f;
    float yMax = camera.orthographicSize - 0.5f;

    Vector3 catPos = 
      new Vector3 (cameraPos.x + Random.Range (xMax - xRange, xMax),
                  Random.Range (-yMax, yMax),
                  catPrefab.transform.position.z);

    Instantiate (catPrefab, catPos, Quaternion.identity);

    Invoke ("SpawnCat", Random.Range (minSpawnTime, maxSpawnTime));
  }
}
