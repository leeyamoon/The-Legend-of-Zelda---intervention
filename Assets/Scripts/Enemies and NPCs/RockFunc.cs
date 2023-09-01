using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFunc : MonoBehaviour
{
    private void Update()
    {
        if(GameManager.Shared.isScreenMoves)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
            return;
        Destroy(gameObject);
        
    }
}
