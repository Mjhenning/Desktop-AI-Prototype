using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum GoopSize {
    Small,
    Medium,
    Big
}

public class GoopGeneration : MonoBehaviour {
 [SerializeField]GameObject goopPrefab;
 [SerializeField] float minSpawnDelay = 1f; // Minimum time between spawns
 [SerializeField] float maxSpawnDelay = 3f; // Maximum time between spawns
 [SerializeField] int maxGoopPerClusterExclusive;
 [SerializeField] int minGoopPerClusterInclusive;

 void Start() {
     // Start spawning goop
     StartCoroutine(SpawnGoopWithRandomDelay());
 }

 IEnumerator SpawnGoopWithRandomDelay() {
     while (true) {
         SpawnGoop(/*Random.Range(minGoopPerClusterInclusive, maxGoopPerClusterExclusive)*/ 3); // Spawn goop
         float _delay = Random.Range(minSpawnDelay, maxSpawnDelay);
         yield return new WaitForSeconds(_delay); // Wait for a random delay before spawning again
     }
 }

 void SpawnGoop (int spawnamount) {
     for (int i = 0; i < spawnamount; i++) {
         GoopSize _randomizedSize = (GoopSize) Random.Range (0, 3);

         // Resize the goopPrefab based on its size
         float _scale = 1f;
         switch (_randomizedSize) {
             case GoopSize.Small:
                 _scale = 0.8f;
                 break;
             case GoopSize.Medium:
                 _scale = 1f;
                 break;
             case GoopSize.Big:
                 _scale = 1.4f;
                 break;
         }
         
         goopPrefab.transform.localScale = new Vector3 (_scale, _scale, 1f);

         // Generate a random x-coordinate within the screen width
         float _randomX = Random.Range (0, Screen.width);

         // Calculate the position for the goopPrefab at the top of the screen with the random x-coordinate
         Vector3 _desiredPosition = new Vector3 (_randomX, Screen.height, 0f);
         Vector3 _screenPosition = Camera.main.ScreenToWorldPoint (_desiredPosition);

         // Adjust the position to keep the goopPrefab within screen bounds
         float _screenWidth = Screen.width;
         float _elementWidth = goopPrefab.GetComponent<SpriteRenderer> ().bounds.size.x * _scale;

         float _adjustedX = Mathf.Clamp (_screenPosition.x, _elementWidth / 2, _screenWidth - _elementWidth / 2);

         Vector3 _adjustedPosition = new Vector3 (_adjustedX, _screenPosition.y, 0f);

         // Instantiate the goopPrefab at the adjusted position
         Goop_Instance _spawnedGoop = Instantiate (goopPrefab, _adjustedPosition, Quaternion.identity,UI_Manager.instance.transform).GetComponent<Goop_Instance>();
         _spawnedGoop.size = _randomizedSize;
     }
 }
}
