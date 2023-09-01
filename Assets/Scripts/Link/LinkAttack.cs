using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkAttack : MonoBehaviour
{
    public static LinkAttack Shared { get; private set; }
    private bool _haveSword;
    public bool InAttack { get; private set; }
    private Animator _animator;
    private static readonly int FIGHT = Animator.StringToHash("Fight");
    [SerializeField] private GameObject attackArea;
    [SerializeField] private AudioClip attackAudio;

    private void Awake()
    {
        Shared = this;
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _haveSword = false;
        InAttack = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && _haveSword && !InAttack && !GameManager.Shared.isWorldActionActive)
        {
            StartCoroutine(AttackSword());
        }
    }

    private IEnumerator AttackSword()
    {
        GameObject attack = CreateAttackArea();
        AudioManager.Shared().MakeSoundOnce(attackAudio);
        InAttack = true;
        _animator.SetBool(FIGHT, true);
        _animator.speed = 1;
        yield return new WaitForSeconds(0.35f);
        _animator.SetBool(FIGHT, false);
        _animator.speed = 0;
        InAttack = false;
        Destroy(attack);
    }

    private GameObject CreateAttackArea()
    {
        Vector2 pos = LinkMovement.Shared.GetFacingDirection();
        if (Mathf.Approximately(pos.x, 1))
            return Instantiate(attackArea, transform.position, Quaternion.Euler(0, 0, 90));
        if(Mathf.Approximately(pos.x, -1))
            return Instantiate(attackArea, transform.position, Quaternion.Euler(0, 0, 270));
        if(Mathf.Approximately(pos.y, 1))
            return Instantiate(attackArea, transform.position, Quaternion.Euler(0, 0, 180));
        return Instantiate(attackArea, transform.position, Quaternion.identity);
    }

    public void ActiveSword()
    {
        _haveSword = true;
    }
}
