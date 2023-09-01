using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManInCave : MonoBehaviour
{
    [SerializeField, Min(0)] private float timeToWait = 1;
    [SerializeField, Min(0.01f)] private float timeBetweenFlips = 0.05f;
    
    private bool _alreadyUsed;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _alreadyUsed = false;
    }

    public void DestroyOldMan()
    {
        if (_alreadyUsed) return;
        _alreadyUsed = true;
        StartCoroutine(DestroyOldManCoroutine());
    }

    private IEnumerator DestroyOldManCoroutine()
    {
        bool flip = false;
        Sprite sprite = _spriteRenderer.sprite;
        GameManager.Shared.isWorldActionActive = true;
        for (float i = 0; i < timeToWait; i += timeBetweenFlips)
        {
            _spriteRenderer.sprite = flip ? null : sprite;
            flip = !flip;
            yield return new WaitForSeconds(timeBetweenFlips);
        }
        GameManager.Shared.isWorldActionActive = false;
        Destroy(gameObject);
    }
}
