using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _manager;

    private AudioSource _audioSource;

    private AudioSource _speakingAudioSource;

    private AudioClip _overWorldAudio;

    [SerializeField] private AudioClip speakingAudio;
    
    private void Awake()
    {
        _manager = this;
        _audioSource = GetComponent<AudioSource>();
        _speakingAudioSource = gameObject.AddComponent<AudioSource>();
        _speakingAudioSource.loop = true;
        _overWorldAudio = _audioSource.clip;
        _speakingAudioSource.clip = speakingAudio;
    }

    public static AudioManager Shared()
    {
        return _manager;
    }

    public void MakeSoundOnce(AudioClip audio)
    {
        _audioSource.PlayOneShot(audio);
    }

    public void SetActiveOverWorldAudio(bool toActive)
    {
        _audioSource.clip = toActive ? _overWorldAudio : null;
        _audioSource.Play();
    }

    public void SetSpeakingAudio(bool toSpeak)
    {
        if (toSpeak)
        {
            _speakingAudioSource.Play();
            _audioSource.mute = true;
            return;
        }
        _speakingAudioSource.Stop();
        _audioSource.mute = false;
    }
}
