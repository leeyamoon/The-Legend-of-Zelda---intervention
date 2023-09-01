using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBush : MonoBehaviour
{
    [SerializeField] private GameObject destroyAnimation;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("AttackArea"))
        {
            Destroy(gameObject);
            Instantiate(destroyAnimation, transform.position, Quaternion.identity);
        }
    }
}
