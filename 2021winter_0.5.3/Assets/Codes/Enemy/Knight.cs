using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour {

    [SerializeField] GameObject Player;
    [SerializeField] GameObject Spear;
    [SerializeField] QuestManager qm;

    Animator anim;
    Rigidbody2D rigid;
    SpriteRenderer sr;

    float speed = -2;
    bool isWalk;
    bool isAlive = false;
    bool isFollow = false;
    bool isFacingRight = false;

    private int hp;

    [Header("HPBar")]
    [SerializeField] Image healthBar;


    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(idle());

        Color color = sr.color;
        color.a = 1;
        sr.color = color;
        
        speed = -2;
        isAlive = true;
        isFollow = false;

        hp = 250;

        anim.SetBool("die", false);
    }

    IEnumerator idle()
    {
        anim.SetBool("walk", false);
        isWalk = false;
        anim.SetBool("idle", true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(walk());

        int ranAction = Random.Range(0, 2);

        switch (ranAction)
        {
            case 0:
                speed = -2;
                Flip(speed);
                break;
            case 1:
                speed = 2;
                Flip(speed);
                break;
        }
    }

    IEnumerator walk()
    {
        StingAttackOff();
        isWalk = true;
        anim.SetBool("walk", true);
        anim.SetBool("idle", false);
        anim.SetBool("attack", false);

        if (!isFollow)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(idle());
        }
        else
        {
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (isAlive) {
            if (isWalk) {
                rigid.velocity = new Vector2(speed, rigid.velocity.y);
                if (isFollow) {
                    if (gameObject.transform.position.x < Player.transform.position.x) {
                        Flip(2);
                    }
                    else {
                        Flip(-2);
                    }
                }
            }


            if (Mathf.Abs(gameObject.transform.position.x - Player.transform.position.x) <= 2.5f)
            {

                isWalk = false;     //attack()
                anim.SetBool("attack", true);
                anim.SetBool("idle", false);
                anim.SetBool("walk", false);

            }
            else if (((Mathf.Abs(gameObject.transform.position.x - Player.transform.position.x) <= 5f) && (Mathf.Abs(gameObject.transform.position.x - Player.transform.position.x) > 2.5f)))
            {
                isFollow = true;
                StartCoroutine("walk");
            }
        }
    }

    void Flip(float spd)
    {
        speed = spd;
        if ((isFacingRight && speed < 0) || (!isFacingRight && speed > 0))
        {
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = !isFacingRight;
        }
    }

    void turnAtk()
    {
        if (gameObject.transform.position.x < Player.transform.position.x)
        {
            if (!isFacingRight)
            {
                Flip(2);
            }
        }
        else
        {
            if (isFacingRight)
            {
                Flip(-2);
            }
        }
    }

    void StingAttackOn()
    {
        Spear.SetActive(true);
    }

    void StingAttackOff()
    {
        Spear.SetActive(false);
    }

    public void TakeDamage(int d) {
        hp -= d;
        //add
        healthBar.fillAmount = hp / 250f;

        if(hp <= 0) {
            StopAllCoroutines();
            StartCoroutine(CoDie());
        }
    }

    IEnumerator CoDie() {
        isAlive = false;

        Color color = sr.color;

        anim.SetBool("die", true);
        anim.SetBool("walk", false);
        anim.SetBool("attack", false);
        anim.SetBool("idle", false);

        yield return new WaitForSeconds(0.5f);

        while (true) {
            color.a -= Time.deltaTime;
            if(color.a <= 0) {
                color.a = 0;
                sr.color = color;
                break;
            }


            sr.color = color;

            yield return null;
        }

        qm.KillEnemy(-999);
        this.gameObject.SetActive(false);
    }
}
