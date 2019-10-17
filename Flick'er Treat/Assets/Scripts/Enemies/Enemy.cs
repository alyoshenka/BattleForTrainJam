using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    Vector3 vecToTarget;

    void Start()
    {
        
    }

    void Update()
    {
        vecToTarget = (target.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, vecToTarget * 3, Color.red);
    }
}
