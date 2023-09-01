using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOfEnemyFunc : MonoBehaviour
{
    private bool _isActive;

    private void Start()
    {
        _isActive = true;
    }

    private void Update()
    {
        if (_isActive && GameManager.Shared.isWorldActionActive)
        {
            _isActive = false;
            gameObject.SetActive(false);
        } else if (!_isActive && !GameManager.Shared.isWorldActionActive)
        {
            _isActive = true;
            gameObject.SetActive(true);
        }
    }
}
