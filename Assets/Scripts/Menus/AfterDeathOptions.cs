using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterDeathOptions : MonoBehaviour
{
    [SerializeField] private GameObject[] options;
    private int _indexOfOption = 0;
    
    // Start is called before the first frame update
    private void Start()
    {
        foreach(var option in options)
            option.SetActive(false);
        options[_indexOfOption].SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && options.Length > 0)
        {
            options[_indexOfOption].SetActive(false);
            ++_indexOfOption;
            _indexOfOption = _indexOfOption < options.Length ? _indexOfOption : 0;
            options[_indexOfOption].SetActive(true);
        } else if(Input.GetKeyDown(KeyCode.UpArrow) && options.Length > 0)
        {
            options[_indexOfOption].SetActive(false);
            --_indexOfOption;
            _indexOfOption = _indexOfOption < 0 ? options.Length-1 : _indexOfOption;
            options[_indexOfOption].SetActive(true);
        }

        if (!Input.GetKeyDown(KeyCode.X) && !Input.GetKeyDown(KeyCode.Return)) return;
        switch (_indexOfOption)
        {
            case 0:
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                break;
            case 1:
                Application.Quit();
                break;
        }
    }
}
