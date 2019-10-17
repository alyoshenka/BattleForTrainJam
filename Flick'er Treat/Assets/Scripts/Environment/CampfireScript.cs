using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    
    [SerializeField]
    float health = 100;
    float maxHealth;
    float cachedHealth;
    float damageTime = 0;
    [SerializeField]
    float damageInterval = 1;

    [SerializeField]
    SpaceController spaceController = default;
    [SerializeField]
    List<GameObject> penetratingEnemies;

    void Start()
    {
        maxHealth = health;
        cachedHealth = health;
        penetratingEnemies = new List<GameObject>();
    }

    void Update()
    {
        // Take damage if enemy near
        damageTime += Time.deltaTime;
        if (damageTime >= damageInterval)
        {
            health -= penetratingEnemies.Count;

            damageTime = 0;
        }

        // Detect change in health
        if (cachedHealth != health)
        {
            spaceController.SetBorderScale(health / maxHealth);
            cachedHealth = health;

            // Lose
            if (health <= 0)
            {
                Debug.Log("You have lost");
            }
        }
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            penetratingEnemies.Add(other.gameObject);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < penetratingEnemies.Count; i++)
        {
            if (penetratingEnemies[i] == other.gameObject)
            {
                penetratingEnemies.RemoveAt(i);
            }
        }
    }
}
