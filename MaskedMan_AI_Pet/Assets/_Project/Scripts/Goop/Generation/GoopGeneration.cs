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
 
 [SerializeField] RectTransform canvasRect;

 void Start() {
     // Start spawning goop
     StartCoroutine(SpawnGoopWithRandomDelay());
 }

 IEnumerator SpawnGoopWithRandomDelay() {
     while (true) {
         SpawnGoop(Random.Range(minGoopPerClusterInclusive, maxGoopPerClusterExclusive)); // Spawn goop
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
         
         
         goopPrefab.transform.localScale = new Vector3(_scale, _scale, 1f);

         // Generate a random x-coordinate within the canvas width
         float _randomX = Random.Range(0, canvasRect.rect.width);

         // Calculate the position for the goopPrefab at the top of the canvas with the random x-coordinate
         Vector3 _desiredPosition = new Vector3(_randomX, canvasRect.rect.height, 0f);

         // Adjust the position to keep the goopPrefab within canvas bounds
         float _canvasWidth = canvasRect.rect.width;
         float _elementWidth = goopPrefab.GetComponent<RectTransform>().rect.width * _scale;

         float _adjustedX = Mathf.Clamp(_desiredPosition.x, _elementWidth / 2, _canvasWidth - _elementWidth / 2);

         Vector3 _adjustedPosition = new Vector3(_adjustedX, _desiredPosition.y, 0f);

         // Instantiate the goopPrefab at the adjusted position
         Goop_Instance _spawnedGoop = Instantiate (goopPrefab, _adjustedPosition, Quaternion.identity,UI_Manager.instance.transform).GetComponent<Goop_Instance>();
         _spawnedGoop.size = _randomizedSize;
     }
 }
}
