using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {

    private const float SPEED = 8f;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private BossManager bm;

    private PlayerControl playerControl;
    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    
    private float dir;

    private bool isReady;
    private bool isAlive;
    private bool isRolling;

    private void OnEnable() {

        playerControl = player.GetComponent<PlayerControl>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        Initialize();

        //Activate();
    }

    private void OnDisable() {
        StopAllCoroutines();

        Initialize();
    }

    private void Update() {
        if (bm.GetIsAlive() && isReady) {
            Activate();
        }

        if (isAlive && isRolling) {
            rigid.velocity = new Vector3(dir * SPEED, rigid.velocity.y);
        }
    }

    private void Initialize() {
        isReady = true;
        isAlive = false;
        isRolling = false;
        sprite.flipX = false;
        anim.SetBool("isRoll", false);
    }

    private void Activate() {
        isAlive = true;
        isReady = false;
        StartCoroutine(Idle());
    }

    IEnumerator Idle() {
        
        isRolling = false;      
        
        anim.SetBool("isRoll", false);
        yield return new WaitForSeconds(3f);

        StartCoroutine(Roll());
    }

    IEnumerator Roll() {
        SetDirection();
        
        isRolling = true;
        hitBox.SetActive(true);

        anim.SetBool("isRoll", true);
        yield return new WaitForSeconds(2f);

        hitBox.SetActive(false);

        StartCoroutine(Idle());
    }

    private void SetDirection() {
        dir = (player.transform.position.x - transform.position.x) > 0 ? 1 : -1;
        
        if (dir == 1)
            sprite.flipX = true;
        else if (dir == -1)
            sprite.flipX = false;
    }
    
}