using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordItemFunctionality : MonoBehaviour
{
    [SerializeField] private OldManInCave oldMan;
    [SerializeField] private AudioClip pickedUpAudio;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.Shared.ActiveSword();
            oldMan.DestroyOldMan();
            AudioManager.Shared().MakeSoundOnce(pickedUpAudio);
            Destroy(gameObject);
        }
    }
}
