using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestsManager : MonoBehaviour
{
    #region Quest Bushes Fields
    [SerializeField] private GameObject[] bushes;
    [SerializeField] private Vector2 placeToPutNewBush;
    [SerializeField] private GameObject effectOfCloud;
    [SerializeField] private GameObject bushPrefab;
    [SerializeField] private GeneralNPCFunc lirit;
    [SerializeField] private GameObject keyItem;
    #endregion

    #region Quest Waterfall
    [SerializeField] private GameObject obstacleToWaterfall;
    [SerializeField] private GeneralNPCFunc smamit;
    #endregion
    
    #region Quest Dessert
    [SerializeField] private GameObject obstacleDessert;
    [SerializeField] private GeneralNPCFunc valvarart;
    #endregion

    #region Quest Last Gate
    [SerializeField] private GameObject theGate;
    [SerializeField] private GeneralNPCFunc shraga;
    #endregion

    #region Quest Endgame
    [SerializeField] private GeneralNPCFunc morfius;
    #endregion
    
    
    private void Start()
    {
        StartCoroutine(BushQuest());
        StartCoroutine(EarAndRemoveBarrierQuest(smamit, obstacleToWaterfall));
        StartCoroutine(EarAndRemoveBarrierQuest(valvarart, obstacleDessert));
        StartCoroutine(EarAndRemoveBarrierQuest(shraga, theGate));
        StartCoroutine(EndgameQuest());
    }

    private IEnumerator BushQuest()
    {
        yield return new WaitUntil(() => lirit.AlreadyGaveHint && AllBushesAreGone());
        Instantiate(effectOfCloud, placeToPutNewBush, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(bushPrefab, placeToPutNewBush, Quaternion.identity);
        Instantiate(keyItem, placeToPutNewBush, Quaternion.identity);
    }

    private IEnumerator EarAndRemoveBarrierQuest(GeneralNPCFunc npc, GameObject obstacle)
    {
        yield return new WaitUntil(() => npc.AlreadyGaveHint);
        Destroy(obstacle);
    }

    private IEnumerator EndgameQuest()
    {
        yield return new WaitUntil(() => morfius.AlreadyGaveHint);
        yield return GameManager.Shared.ScreenTransition(true);
        SceneManager.LoadScene("Scenes/EndGameScene");
    }
    
    private bool AllBushesAreGone()
    {
        foreach (var bush in bushes)
        {
            if (bush != null)
                return false;
        }
        return true;
    }
}
