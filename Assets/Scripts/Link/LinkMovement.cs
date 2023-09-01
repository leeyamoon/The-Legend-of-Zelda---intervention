using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class LinkMovement : MonoBehaviour
{
    #region Regular Movement

        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private const int RIGHT = 0;
        private const int LEFT = 1;
        private const int UP = 2;
        private const int DOWN = 3;
        [FormerlySerializedAs("SPEED")] [SerializeField] private float speed;
        private int _animationSpeed;
    
        private Vector2 _directionalVector = Vector2.zero;
        private Vector2 _facingDirection;
    
        private bool _upClick = false;
        private bool _downClick = false;
        private bool _leftClick = false;
        private bool _rightClick = false;
        private static readonly int DIRECTION = Animator.StringToHash("Direction");

    #endregion

    #region Got Hitted

    private bool _hittable = true;
    [SerializeField] private float timeToBeInvincible = 1f;
    [SerializeField] private AudioClip hittedAudio;
    [SerializeField] private Color hittedColor = Color.red;
    #endregion

    public static LinkMovement Shared { get; private set; }
    
    private void Awake()
    {
        Shared = this;
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        _facingDirection = Vector2.down;
        _animationSpeed = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
        if((!_upClick && !_downClick && !_leftClick && !_rightClick) || GameManager.Shared.isWorldActionActive
           || LinkAttack.Shared.InAttack)
            _directionalVector = Vector2.zero;
        else if(_upClick && _downClick && _rightClick) //specific case in the game, a lot of moving mechanics aren't trivial
            _directionalVector = Vector2.right;
        else if((_upClick && _downClick) || (_rightClick && _leftClick))
            _directionalVector = Vector2.zero;
        else if(_upClick)
            _directionalVector = Vector2.up;
        else if(_downClick)
            _directionalVector = Vector2.down;
        else if(_rightClick)
            _directionalVector = Vector2.right;
        else if(_leftClick)
            _directionalVector = Vector2.left;
        StandingAndMoving();
    }

    private void StandingAndMoving()
    {
        if (_directionalVector != Vector2.zero)
        {
            _facingDirection = _directionalVector;
            _animationSpeed = 1;
            SetAnimationDirection();
        }
        else if(LinkAttack.Shared.InAttack ||GameManager.Shared.isScreenMoves)
            _animationSpeed = 1;
        else
            _animationSpeed = 0;
        _animator.speed = _animationSpeed;
    }

    private void SetAnimationDirection()
    {
        if (Mathf.Approximately(_facingDirection.x, 1))
            _animator.SetInteger(DIRECTION,RIGHT);
        else if(Mathf.Approximately(_facingDirection.x, -1))
            _animator.SetInteger(DIRECTION,LEFT);
        else if(Mathf.Approximately(_facingDirection.y, 1))
            _animator.SetInteger(DIRECTION,UP);
        else if (Mathf.Approximately(_facingDirection.y, -1))
            _animator.SetInteger(DIRECTION,DOWN);
    }
    
    private void FixedUpdate()
    {
        _rigidbody.velocity = _directionalVector * speed;
    }

    private void CheckInput()
    {
        _upClick = Input.GetKey(KeyCode.UpArrow);
        _downClick = Input.GetKey(KeyCode.DownArrow);
        _rightClick = Input.GetKey(KeyCode.RightArrow);
        _leftClick = Input.GetKey(KeyCode.LeftArrow);
    }

    public Vector2 GetFacingDirection()
    {
        return _facingDirection;
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && _hittable)
        {
            GameManager.Shared.DecreaseLives();
            StartCoroutine(CanGetHitForSeconds(timeToBeInvincible));
        }
    }

    private IEnumerator CanGetHitForSeconds(float seconds)
    {
        _hittable = false;
        gameObject.GetComponent<SpriteRenderer>().color = hittedColor;
        AudioManager.Shared().MakeSoundOnce(hittedAudio);
        yield return new WaitForSeconds(seconds);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        _hittable = true;
    }
}
