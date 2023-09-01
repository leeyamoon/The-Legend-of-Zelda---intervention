using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FairyItem : MonoBehaviour
{
    [SerializeField] private Vector3 middleOfCyclicPath;
    [SerializeField, Range(0, 3)] private float radius;
    [SerializeField] private AudioClip pickedUpAudio;

    private void Update()
    {
        transform.position =
            middleOfCyclicPath + radius * new Vector3(Mathf.Cos(Time.fixedTime), Mathf.Sin(Time.fixedTime),0);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.Shared.FillLives();
            AudioManager.Shared().MakeSoundOnce(pickedUpAudio);
            Destroy(gameObject);
        }
    }
}
