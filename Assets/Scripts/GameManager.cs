using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-999)]
public class GameManager : MonoBehaviour
{
    #region general
    public static GameManager Shared { get; private set; }
    [HideInInspector] public bool isWorldActionActive;
    [HideInInspector] public bool isScreenMoves;
    private Vector2 _locationInMapsGrid;
    #endregion
    
    #region inventory variables
    private const float MAP_FACTOR = 0.25f;
    private const string X_CHAR = "X";
    private bool _isInventoryActive;
    private int _gemsCounter;
    private int _keysCounter;
    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private GameObject link;
    [SerializeField] private Canvas canvasUI;
    [SerializeField] private GameObject swordInInventory;
    [SerializeField] private Image minimapRect;
    #endregion

    #region Lives
    private int _maxLives;
    private int _livesCounter;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;
    #endregion

    #region Transitions Fields

    [SerializeField] private GameObject leftTransition;
    [SerializeField] private GameObject rightTransition;
    [SerializeField, Range(0.01f, 2)] private float transitionTime;
    [SerializeField, Min(3)] private int movesInTransition = 10;
    
    #endregion
    
    private void Awake()
    {
        Shared = this;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(ScreenTransition(false));
        _isInventoryActive = false;
        isScreenMoves = false;
        _gemsCounter = 0;
        _keysCounter = 0;
        _maxLives = 6;
        _livesCounter = _maxLives;
        _locationInMapsGrid = new Vector2(8,8);
    }

    // Update is called once per frame
    private void Update()
    {
        InventoryOpeningAndClosing();
    }

    public void ChangeLocationInMapGrid(int xChange, int yChange)
    {
        _locationInMapsGrid.x += xChange;
        _locationInMapsGrid.y += yChange;
        minimapRect.GetComponent<RectTransform>().position += new Vector3(xChange*MAP_FACTOR, -yChange*MAP_FACTOR);
    }

    public Vector2 GetLinkLocationInGrid()
    {
        return _locationInMapsGrid;
    }

    private void InventoryOpeningAndClosing()
    {
        if (Input.GetKeyDown(KeyCode.Return) && (!isWorldActionActive || _isInventoryActive) && !isScreenMoves) //to open and close inventory
        {
            if (!_isInventoryActive)
            {
                link.SetActive(false);
                canvasUI.renderMode = RenderMode.WorldSpace;
                CinemachineFunctionality.Shared.MoveCameraToSide(EdgeOfMap.Direction.Up);
                _isInventoryActive = true;
                isWorldActionActive = true;
            }
            else
            {
                _isInventoryActive = false;
                isWorldActionActive = false;
                CinemachineFunctionality.Shared.MoveCameraToSide(EdgeOfMap.Direction.Down);
                StartCoroutine(WaitUntilInventoryClose());
            } 
        }

        if (_isInventoryActive)
        {
            isWorldActionActive = true;
        }
    }

    private IEnumerator WaitUntilInventoryClose()
    {
        yield return new WaitUntil(() => !isWorldActionActive);
        canvasUI.renderMode = RenderMode.ScreenSpaceCamera;
        link.SetActive(true);
    }

    public void FreezeGameObject(GameObject obj)
    {
        StartCoroutine(FreezeGameObjectIE(obj));
    }
    
    private static IEnumerator FreezeGameObjectIE(GameObject obj)
    {
        obj.SetActive(false);
        yield return new WaitWhile(() => Shared.isWorldActionActive);
        obj.SetActive(true);
    }

    public bool AddOrRemoveGems(int amount)
    {
        if (amount >= 0)
        {
            _gemsCounter += amount;
            gemsText.text = X_CHAR + _gemsCounter;
            return true;
        }
        if (-amount > _gemsCounter)
            return false;
        _gemsCounter += amount;
        gemsText.text = X_CHAR + _gemsCounter;
        return true;
    } //made the option to remove gems in order to give the option to use the functionality if extending the game
    
    public void AddKey()
    {
        _keysCounter++;
        keysText.text = X_CHAR + _keysCounter;
    }
    
    public bool RemoveKey()
    {
        if (_keysCounter == 0)
            return false;
        _keysCounter--;
        keysText.text = X_CHAR + _keysCounter;
        return true;
    }
    
    public void DecreaseLives()
    {
        _livesCounter--;
        if (_livesCounter % 2 == 1)
            hearts[_livesCounter / 2].GetComponent<Image>().sprite = halfHeartSprite;
        else
        {
            hearts[_livesCounter/2].GetComponent<Image>().sprite = emptyHeartSprite;
        }

        if (_livesCounter == 0)
        {
            SceneManager.LoadScene("AfterDeadScreen");
        }
    }

    public void FillLives()
    {
        _livesCounter = _maxLives;
        foreach (var heart in hearts)
        {
            heart.GetComponent<Image>().sprite = fullHeartSprite;
        }
    }

    public bool GetInventoryIsActive()
    {
        return _isInventoryActive;
    }

    public void ActiveSword()
    {
        LinkAttack.Shared.ActiveSword();
        swordInInventory.SetActive(true);
    }

    public IEnumerator ScreenTransition(bool isClosing)
    {
        isWorldActionActive = true;
        float factorForClosing = isClosing ? -1 : 1;
        factorForClosing *= (10f / movesInTransition);
        for (int i = 0; i < movesInTransition; i++)
        {
            yield return new WaitForSeconds(transitionTime);
            leftTransition.transform.position += Vector3.left * factorForClosing;
            rightTransition.transform.position += Vector3.right * factorForClosing;
        }
        isWorldActionActive = false;
    }
}
