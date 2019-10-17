using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireScript : MonoBehaviour
{
    
    [SerializeField]
    float health = 100;
    float maxHealth;
    float cachedHealth;

    [SerializeField]
    SpaceController spaceController;

    void Start()
    {
        maxHealth = health;
        cachedHealth = health;
    }

    void Update()
    {
        // Detect change in health
        if (cachedHealth != health)
        {
            spaceController.SetBorderScale(health / maxHealth);
            cachedHealth = health;
        }
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
