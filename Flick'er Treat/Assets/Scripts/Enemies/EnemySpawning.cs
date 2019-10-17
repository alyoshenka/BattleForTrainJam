using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{

    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    GameObject bigEnemyPrefab;
    [SerializeField]
    SpaceController spaceController = default;

    float spawnTime = 0;
    [SerializeField]
    float spawnInterval = 10;

    void Start()
    {
        //spawnRadius = spaceController.GetLerpedValue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            spawnEnemies(1, spaceController.GetLerpedValue() / 2);
        }

        spawnTime += Time.deltaTime;
        if (spawnTime >= spawnInterval)
        {
            spawnEnemies(1, spaceController.GetLerpedValue() / 2);

            spawnTime = 0;
        }
    }

    public void spawnEnemies(float numToBeSpawned, float radius)
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
            Instantiate<GameObject>(Mathf.Round(Random.Range(1,5)) == 1 ? bigEnemyPrefab : enemyPrefab , enemyPosition, Quaternion.identity);
        }
        
    }
}
