using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField, Min(0.01f)] private float timeBetweenChars;
    [SerializeField, Min(1)] private float timeUntilPressAnyKey;
    [SerializeField] private string textToWrite;
    [SerializeField] private GameObject pressAnyKey;
    
    private void Start()
    {
        StartCoroutine(TextAppearingCoroutine());
    }

    private IEnumerator TextAppearingCoroutine()
    {
        foreach (var chr in textToWrite)
        {
            yield return new WaitForSeconds(timeBetweenChars);
            textBox.text += chr;
        }
        yield return new WaitForSeconds(timeUntilPressAnyKey);
        pressAnyKey.SetActive(true);
        yield return new WaitUntil(() => Input.anyKey);
        SceneManager.LoadScene("Scenes/Opening");
    }
}
