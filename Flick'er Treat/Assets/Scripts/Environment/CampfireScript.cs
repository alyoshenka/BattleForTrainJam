using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    Slider UISlider;
    AudioSource audioSource;
    [SerializeField]
    AudioClip damageClip;

    void Start()
    {
        maxHealth = health;
        cachedHealth = health;
        penetratingEnemies = new List<GameObject>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Take damage if enemy near
        damageTime += Time.deltaTime;
        if (damageTime >= damageInterval)
        {
            health -= penetratingEnemies.Count;

            damageTime = 0;

            if (health < maxHealth && penetratingEnemies.Count == 0)
            {
                health += 0.5f;
            }
        }

        // Detect change in health
        if (cachedHealth != health)
        {
            if (health < cachedHealth)
            {
                audioSource.PlayOneShot(damageClip, 0.5f);
            }

            spaceController.SetBorderScale(health / maxHealth);
            cachedHealth = health;
            UISlider.value = Mathf.Abs( (health / maxHealth) -1 );
            

            // Lose
            if (health <= 0)
            {
                Debug.Log("You have lost");
                GameManager.Instance.Lose();
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

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
