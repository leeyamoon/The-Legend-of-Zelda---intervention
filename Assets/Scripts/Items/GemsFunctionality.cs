using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsFunctionality : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private float lifeTime;
    [SerializeField] private AudioClip pickedUpAudio;
    
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(LifeTime(lifeTime));
    }

    private void Update()
    {
        if(GameManager.Shared.isScreenMoves)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        GameManager.Shared.AddOrRemoveGems(value);
        AudioManager.Shared().MakeSoundOnce(pickedUpAudio);
        Destroy(gameObject);
    }
    
    private IEnumerator LifeTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
