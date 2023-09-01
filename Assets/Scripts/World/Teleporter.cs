using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class Teleporter : MonoBehaviour
{
    [SerializeField] private TeleportInfo info;
    [SerializeField] private AudioClip teleportationAudio;
    [SerializeField] private bool activeWorldSound;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        StartCoroutine(CinemachineFunctionality.Shared.MoveCameraToPlace(col.gameObject, info));
        if(!activeWorldSound)
            AudioManager.Shared().SetActiveOverWorldAudio(false);
        else
        {
            StartCoroutine(OutToWorldAudioCoroutine());
        }
        AudioManager.Shared().MakeSoundOnce(teleportationAudio);
    }
    
    private IEnumerator OutToWorldAudioCoroutine()
    {
        yield return new WaitForSeconds(CinemachineFunctionality.Shared.GetTimeToMoveTeleport());
        AudioManager.Shared().SetActiveOverWorldAudio(true);
    }
}
