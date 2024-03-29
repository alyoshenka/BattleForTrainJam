﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float health = 5;
    float maxHealth;
    [SerializeField]
    int lightsNeeded = 1;
    [SerializeField]
    GameObject target;
    [SerializeField]
    List<GameObject> penetratingLights;

    Vector3 vecToTarget;
    float sidestepDir;
    Vector3 sidestepVec = Vector3.zero;
    float sidestepTime = 0;
    float sidestepDecisionInterval = 1;
    bool fleeing = false;
    [SerializeField]
    float approachSpeed = 1;
    [SerializeField]
    float fleeSpeed = 1.5f;
    [SerializeField]
    AudioClip[] ambientClips;
    [SerializeField]
    AudioClip[] damageClips;
    AudioSource audioSource;
    float ambientTime = 0;
    [SerializeField]
    float ambientInterval;

    void Start()
    {
        maxHealth = health;
        target = GameObject.Find("Campfire");
        penetratingLights = new List<GameObject>();
        audioSource = GetComponent<AudioSource>();
        ambientInterval = Random.Range(20, 60);
    }

    void Update()
    {
        if (GameManager.isPaused) { return; }

        vecToTarget = (target.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, vecToTarget * 3, Color.red);

        // Take damage
        if (penetratingLights.Count >= lightsNeeded)
        {
            if (health > 0)
            {
                health -= Time.deltaTime;
                if (health < 0) { health = 0; }
            }
            //Debug.Log(health);
        } //Regain Health
        else if (health < maxHealth && penetratingLights.Count == 0)
        {
            health += Time.deltaTime / 5;
        }

        // Decide if approaching or fleeing
        if (fleeing && health >= maxHealth)
        {
            fleeing = false;
        } 
        else if (!fleeing && health <= 0)
        {
            fleeing = true;
            audioSource.PlayOneShot(damageClips[(int)Mathf.Round(Random.Range(0, damageClips.Length))]);
        }

        // Movement
        if (!fleeing)
        {
            approach();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((target.transform.position - transform.position).normalized), 0.05f );
            
        }
        else
        {
            flee();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-(target.transform.position - transform.position).normalized), 0.05f);

        }

        // Ambient sounds
        ambientTime += Time.deltaTime;
        if (ambientTime >= ambientInterval)
        {
            audioSource.PlayOneShot(ambientClips[(int)Mathf.Round(Random.Range(0, ambientClips.Length))]);
            ambientTime = 0;
        }
    }

    private void approach()
    {
        // Decide on direction of sidestep
        sidestepTime += Time.deltaTime;
        if (sidestepTime >= sidestepDecisionInterval)
        {
            sidestepDir = (Mathf.Round(Random.Range(-1, 2)));
            
            sidestepTime = 0;
        }
        sidestepVec = new Vector3(-vecToTarget.z, vecToTarget.y, vecToTarget.x) * sidestepDir;
        Vector3 direction = (vecToTarget + sidestepVec).normalized;

        transform.Translate(direction * Time.deltaTime * approachSpeed, Space.World);
        Debug.DrawRay(transform.position, sidestepVec * 3, Color.green);
        Debug.DrawRay(transform.position, direction * 3, Color.blue);
    }

    private void flee()
    {
        transform.Translate(-vecToTarget * Time.deltaTime * fleeSpeed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Light"))
        {
            penetratingLights.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < penetratingLights.Count; i++)
        {
            if (penetratingLights[i] == other.gameObject)
            {
                penetratingLights.RemoveAt(i);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (fleeing && collision.gameObject.CompareTag("Void"))
        {
            health = maxHealth;
            penetratingLights.Clear();
        }
    }
}
