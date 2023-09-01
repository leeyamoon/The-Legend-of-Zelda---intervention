using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] private float deathAnimationTime = 0.5f;
    
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(DeathCoroutine(deathAnimationTime));
    }

    private IEnumerator DeathCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
