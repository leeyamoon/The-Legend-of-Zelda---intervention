using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneralNPCFunc : MonoBehaviour
{
    [SerializeField, Range(0.6f, 5)] private float interactionRadius = 1;
    [SerializeField] private List<string> firstChat;
    [SerializeField] private TextMeshProUGUI _textMesh; 
    [SerializeField] private string helperText;
    [SerializeField, Min(0.02f)] private float waitingBetweenChars;
    [SerializeField, Min(1)] private float waitingAfterHelperText;
    [SerializeField] private Transform linkPos; 
    private bool _firstTalk = true;
    private bool _linkInRange;
    public bool AlreadyGaveHint { get; private set; }
    
    private void Start()
    {
        _firstTalk = true;
        _linkInRange = false;
        AlreadyGaveHint = false;
    }
    
    private void Update()
    { 
        _linkInRange = Vector2.Distance(linkPos.position, transform.position) <= interactionRadius;
        if (Input.GetKeyDown(KeyCode.Z) && _linkInRange)
        {
            StartConversation();
        }
    }
    
    private void StartConversation()
    {
        if (!_firstTalk || !Input.GetKeyDown(KeyCode.Z)) return;
        _firstTalk = false;
        StartCoroutine(SpeakingCoroutine());
    }
    
    private IEnumerator SpeakingCoroutine()
    {
        foreach (var chat in firstChat)
        {
            yield return GenerateOneSentence(chat);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z) && _linkInRange);
        }
        _textMesh.text = "";
        while (true)
        {
            yield return GenerateOneSentence(helperText);
            AlreadyGaveHint = true;
            yield return new WaitForSeconds(waitingAfterHelperText);
            _textMesh.text = "";
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z) && _linkInRange);
        }
    }
    
    private IEnumerator GenerateOneSentence(string chat)
    {
        _textMesh.text = "";
        AudioManager.Shared().SetSpeakingAudio(true);
        foreach (var chr in chat)
        { 
            yield return new WaitForSeconds(waitingBetweenChars);
            _textMesh.text += chr;
        }
        AudioManager.Shared().SetSpeakingAudio(false);
    }
}
