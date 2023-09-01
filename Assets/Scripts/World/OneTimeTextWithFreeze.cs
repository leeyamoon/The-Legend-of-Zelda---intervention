using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class OneTimeTextWithFreeze : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private string textToShow;
    [SerializeField, Min(0.01f)] private float timeBetweenChars = 0.25f;
    private string _curText = "";
    private bool _appeared = false;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !_appeared)
        {
            StartCoroutine(MakeTextOnScreen());
            _appeared = true;
        }
    }

    private IEnumerator MakeTextOnScreen()
    {
        GameManager.Shared.isWorldActionActive = true;
        yield return new WaitForSeconds(1);
        GameManager.Shared.isWorldActionActive = true;
        AudioManager.Shared().SetSpeakingAudio(true);
        foreach (var letter in textToShow)
        {
            yield return new WaitForSeconds(timeBetweenChars);
            _curText += letter;
            textMeshProUGUI.text = _curText;
        }
        AudioManager.Shared().SetSpeakingAudio(false);
        GameManager.Shared.isWorldActionActive = false;
    }
}
