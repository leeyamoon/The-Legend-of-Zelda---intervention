using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Octorock : MonoBehaviour
{
    public enum Side
    { 
        Right, Left, Up, Down
    }
    
    #region Walking Fields
    
    [SerializeField, Min(0.1f)]
    private float rayLength = 1.5f;
    
    [SerializeField]
    private LayerMask rayLayer = default;
    
    [SerializeField]
    private float rayRadius = 0.5f;

    [SerializeField, Min(0.05f)] 
    private float timePerUnit = 0.2f;
    
    public static readonly Dictionary<Side, Vector2> SIDE_TO_DIRECTION = new Dictionary<Side, Vector2>() {{Side.Up , Vector2.up},
        { Side.Down , Vector2.down} ,{Side.Left, Vector2.left}, { Side.Right , Vector2.right}};

    private bool _canGoLeft;
    private bool _canGoRight;
    private bool _canGoUp;
    private bool _canGoDown;
    private Vector2 _curDirection;
    private Side _curSide;
    private bool _canTurnedAround;
    private bool _isStanding;
    private static readonly int DIRECTION = Animator.StringToHash("Direction");
    
    #endregion

    #region Shooting Fields

    [SerializeField] private float shootingRadius = 0.5f;
    [SerializeField] private GameObject rock;
    [SerializeField] private float rockSpeed = 1.5f;
    private GameObject _curRock;

    #endregion

    #region Death Fields
    [SerializeField] private GameObject deathAnimation;
    [SerializeField] private GameObject[] lootList;
    #endregion
    
    
    private bool _isAlive;
    private Random _random;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _canTurnedAround = true;
        _canGoDown = false;
        _canGoUp = false;
        _canGoLeft = false;
        _canGoRight = false;
        _random = new Random();
        _isAlive = true;
        _isStanding = true;
        _curSide = Side.Down;
        _curDirection = Vector2.down;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.Shared.isWorldActionActive)
            GameManager.Shared.FreezeGameObject(gameObject);
    }

    private void FixedUpdate()
    {
        if (!_canTurnedAround  || !_isAlive)
            return;
        StartCoroutine(CooldownUntilTurn());
        if(!ShootRandomly())
            WalkingAround();
    }
    
    private IEnumerator CooldownUntilTurn()
    {
        _canTurnedAround = false;
        yield return new WaitForSeconds(timePerUnit);
        _canTurnedAround = true;
    }
    
    #region Walking Functionality

    private void WalkingAround()  //todo check about the rock
    {
        CheckSides();
        _curSide = ChooseRandomSideToWalk();
        WalkToDirectionOrStay();
        ChangeAnimationDirection();
    }
    
    private void CheckSides()
    {
        _canGoLeft = CanWalkToSide(Side.Left, rayRadius);
        _canGoRight = CanWalkToSide(Side.Right, rayRadius);
        _canGoUp = CanWalkToSide(Side.Up, rayRadius);
        _canGoDown = CanWalkToSide(Side.Down, rayRadius);
    }

    private Side ChooseRandomSideToWalk()
    {
        bool[] isSidesAvailable = {_canGoRight, _canGoLeft, _canGoUp, _canGoDown};
        Side[] side = { Side.Right, Side.Left, Side.Up, Side.Down};
        List<Side> availableSides = new List<Side>();
        for(int i = 0; i < side.Length ; i++)
            if(isSidesAvailable[i])
                availableSides.Add(side[i]);
        if (availableSides.Contains(_curSide)) //make sure higher probability to stay in the same position;
            availableSides.Add(_curSide);
        int index = _random.Next(availableSides.Count);
        return availableSides[index];
    }

    private void WalkToDirectionOrStay()
    {
        _isStanding = _random.Next(4) == 0; //25% to stay & 75% to walk
        if (_isStanding)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        _curDirection = SIDE_TO_DIRECTION[_curSide];
        _rigidbody.velocity = _curDirection * (1f / timePerUnit); // Time * Speed = Dist => SpeedPerUnit = 1/TimePerUnit
    }

    private bool CanWalkToSide(Side side , float radius) //todo merge two functions
    {
        Vector2 origin = transform.position;
        Vector2 rayDirection = SIDE_TO_DIRECTION[side];
        Vector2 fromOrigin = rayDirection.x == 0 ? Vector2.left : Vector2.up;
        RaycastHit2D hitUpper = Physics2D.Raycast(origin + fromOrigin*radius, rayDirection, rayLength, rayLayer);
        RaycastHit2D hitMiddle = Physics2D.Raycast(origin, rayDirection, rayLength, rayLayer);
        RaycastHit2D hitLower = Physics2D.Raycast(origin - fromOrigin*radius, rayDirection, rayLength, rayLayer);
        Debug.DrawRay(origin + fromOrigin*radius, rayDirection*rayLength, Color.magenta);
        Debug.DrawRay(origin, rayDirection*rayLength, Color.magenta);
        Debug.DrawRay(origin - fromOrigin*radius, rayDirection*rayLength, Color.magenta);
        return hitUpper.collider == null && hitLower.collider == null && hitMiddle.collider == null;
    }
    
    private void ChangeAnimationDirection() //todo change to different animations
    {
        int sideDirectionValueInAnimator = 
            _curSide switch
            {
                Side.Down => 0,
                Side.Right => 1,
                Side.Up => 2,
                _ => 3
            };
        _animator.SetInteger(DIRECTION, sideDirectionValueInAnimator);
    }
    
    #endregion
    
    private void OnEnable()
    {
        _canTurnedAround = true;
    }
    

    private bool ShootRandomly()
    {
        if(!_isStanding || !_isAlive || _curRock != null)
            return false;
        var forShooting = _random.Next(3);  // An 1/3 probability to shoot
        if (forShooting != 0)
            return false;
        Vector3 directionIn3D = SIDE_TO_DIRECTION[_curSide];
        _curRock = Instantiate(rock, transform.position + directionIn3D * shootingRadius, Quaternion.identity);
        _curRock.GetComponent<Rigidbody2D>().velocity = SIDE_TO_DIRECTION[_curSide]* rockSpeed;
        return true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("AttackArea"))
        {
            Destroy(gameObject);
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
            if (_random.NextDouble() < 0.5f && lootList.Length > 0) // 1/2 chance to have a drop
                Instantiate(lootList[_random.Next(lootList.Length)], transform.position, Quaternion.identity);
            
        }
    }

}
