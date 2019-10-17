using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    float spawnRadius;

    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    SpaceController spaceController;

    void Start()
    {
        spawnRadius = spaceController.GetLerpedValue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spawnEnemies(1, spaceController.GetLerpedValue() / 2);
            Debug.Log(spaceController.GetLerpedValue());
        }
    }

    void spawnEnemies(float numToBeSpawned, float radius)
    {
        for (int i = 0; i < numToBeSpawned; i++)
        {
            float randomRotation = Random.Range(1, 360);

            float rotationInRadians = ((randomRotation) * (Mathf.PI / 180)); // Convert to radians
            float rotatedX = Mathf.Cos(rotationInRadians) - Mathf.Sin(rotationInRadians);
            float rotatedZ = Mathf.Sin(rotationInRadians) + Mathf.Cos(rotationInRadians);

            Vector3 enemyPosition = new Vector3(rotatedX, 0, rotatedZ).normalized * radius;
            enemyPosition.y = 5;
            Debug.Log(enemyPosition);
            Instantiate<GameObject>(enemyPrefab, enemyPosition, Quaternion.identity);
        }
        
    }
}
