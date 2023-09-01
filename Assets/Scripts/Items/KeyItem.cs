using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [SerializeField] private AudioClip pickedUpAudio;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        GameManager.Shared.AddKey();
        AudioManager.Shared().MakeSoundOnce(pickedUpAudio);
        Destroy(gameObject);
    }
}
