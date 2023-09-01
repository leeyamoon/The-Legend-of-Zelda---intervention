using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject enemyType;

    [SerializeField] private Octorock.Side animationSideToGo;

    [SerializeField] private float animationTime;

    [SerializeField] private GameObject animationOfEnemyFromSide;

    private float _rotationAngleOfAnimation;

    private GameObject _enemy;

    private bool _onScreenNow;

    private bool _isAlive;
    
    [SerializeField] private float spawnAfterSeconds; //Under 0 means random time between 1 to 5 seconds

    private const float START_COUNT_AFTER = 1.5f; //time to the camera to move;

    [SerializeField] private Vector2 mapCoordinatesInWorld;
    
    // Start is called before the first frame update
    private void Start()
    {
        _isAlive = true;
        _onScreenNow = false;
        _enemy = Instantiate(enemyType, transform.position, Quaternion.identity);
        _enemy.SetActive(false);
        if (spawnAfterSeconds < 0)
            spawnAfterSeconds = (float) new Random().NextDouble() * 4 + 1;
        _rotationAngleOfAnimation = 
            animationSideToGo switch
                {
                    Octorock.Side.Down => 0,
                    Octorock.Side.Right => 90,
                    Octorock.Side.Up => 180,
                    _ => 270
                };
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isAlive && _enemy == null)
            _isAlive = false;
        if (mapCoordinatesInWorld == GameManager.Shared.GetLinkLocationInGrid() && _isAlive && !_onScreenNow)
        {
            _onScreenNow = true;
            StartCoroutine(ActiveEnemyAfterTime());
        }
        else if(mapCoordinatesInWorld != GameManager.Shared.GetLinkLocationInGrid() && _isAlive && _enemy.activeSelf && 
                !GameManager.Shared.GetInventoryIsActive())
        {
            SetBackToPlaceAndHide();
        }
    }

    private IEnumerator ActiveEnemyAfterTime()
    {
        yield return new WaitForSeconds(START_COUNT_AFTER + spawnAfterSeconds - animationTime);
        var animationOf = AnimationCreator();
        yield return new WaitForSeconds(animationTime);
        yield return new WaitWhile(() => GameManager.Shared.isWorldActionActive);
        Destroy(animationOf);
        _enemy.SetActive(true);
    }

    private void SetBackToPlaceAndHide()
    {
        if(_enemy == null)
            return;
        _enemy.transform.position = transform.position;
        _enemy.SetActive(false);
        _onScreenNow = false;
    }

    private GameObject AnimationCreator()
    {
        Vector2 sideToGo = Octorock.SIDE_TO_DIRECTION[animationSideToGo];
        GameObject instanceOfEnemyAnimation = Instantiate(animationOfEnemyFromSide, transform.position - (Vector3)sideToGo,
            Quaternion.Euler(0, 0, _rotationAngleOfAnimation));
        instanceOfEnemyAnimation.GetComponent<Rigidbody2D>().velocity = sideToGo *(1f/animationTime); //to keep one check between one tile the octorock moves
        return instanceOfEnemyAnimation;
    }
}
