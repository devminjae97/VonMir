using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampControl : MonoBehaviour {

    [SerializeField] private BossManager bm;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject damage;
    [SerializeField] private Vine Vine;
    [SerializeField] private Vomit Vomit;

    private Animator anim;
    private Rigidbody2D rb;
    private bool isFlipped;
    private bool isReady;
    private bool isAlive;

    //Animation
    private float speed;
    private const float ATTACKRANGE = 2f;
    [SerializeField] private float invokeTime; //스킬 발동
    [SerializeField] private float wait;

    //Effect
    [SerializeField] private bool isActivated;

    //Selection
    [SerializeField] private float rand;
    [SerializeField] private float timer;
    [SerializeField] private float count;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = 0;
        count = 0;
        wait = 5f;
        timer = 2f;
        isFlipped = false;
        isReady = true;
        isAlive = false;
        //anim.SetBool("isIdle", true);
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (bm.GetIsAlive() && isReady) {
            Activate();
        }

        if (isAlive) {
            invokeTime += Time.deltaTime;

            if (anim.GetBool("isIdle")&& !anim.GetBool("isAttack3")) {
                if (timer <= 0 && !anim.GetBool("isAttack") && !anim.GetBool("isAttack2"))
                {
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                        rand = Random.Range(0, 2);
                    }
                    if (rand == 0) {
                        anim.SetBool("isWalk", true);
                    }
                    if((rand == 1 || rand == 2))
                    {
                        if ((rand == 1 || rand == 2) && invokeTime > 7.5f) //내뿜기
                        {
                            anim.SetBool("isAttack", true);
                            anim.SetBool("isWalk", false);
                            invokeTime = 0;
                        }
                        else
                        {
                            anim.SetBool("isAttack2", true);
                            anim.SetBool("isWalk", false);
                        }
                    }                
                    anim.SetBool("isIdle", false);
                    LookAtPlayer();
                    timer = 2f;
                }
                else
                    timer -= Time.deltaTime;
            }

            if (anim.GetBool("isWalk") && !anim.GetBool("isIdle")) {
                speed = 2.5f;
                Follow();
            }
            //넝쿨
            if (anim.GetBool("isAttack2") && !anim.GetBool("isIdle")) //넝쿨 애니메이션
            {
                wait -= Time.deltaTime;
                if (wait <= 3) {
                    anim.SetBool("isAttack2", false);
                    anim.SetBool("isIdle", true);
                    wait = 5f;
                }
            }

            Vector3 offset = player.position - transform.position;
            float distance = offset.sqrMagnitude;
            if (!anim.GetBool("isAttack2") &&distance <= ATTACKRANGE * ATTACKRANGE) //근접공격
            {
                anim.SetBool("isWalk", false);
                if (!anim.GetBool("isWalk") && (!anim.GetBool("isAttack3") || anim.GetBool("isAttack3")) && count < 2)
                {
                    anim.SetBool("isAttack3", true);
                    
                    wait -= Time.deltaTime;
                    if (anim.GetBool("isAttack3") && wait <= 4)
                    {
                        count++;
                        wait = 5f;
                    }
                }else if (count==2)
                {
                    anim.SetBool("isAttack3", false);
                    anim.SetBool("isAttack", true);                
                }
                Debug.Log(count);
            }
            if ((anim.GetBool("isAttack") || anim.GetBool("isAttack3")) && distance > ATTACKRANGE * ATTACKRANGE) //Attack To Follow
            {
                wait -= Time.deltaTime;
                if (anim.GetBool("isAttack") &&!Vomit.gameObject.activeSelf && wait <= 2)
                {
                    anim.SetBool("isAttack", false);
                    count = 0;
                }
                anim.SetBool("isAttack3", false);
                anim.SetBool("isIdle", true);              
            }
        }
    }
    
    private void Activate() {
        Debug.Log("::ACASET!!!!!!!!!!!!!!!");
        isAlive = true;
        isReady = false;
        anim.SetBool("isIdle", true);
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    public void NoVomit()
    {
        if (anim.GetBool("isAttack") && !anim.GetBool("isIdle")) //내뿜기
        {
            anim.SetBool("isAttack", false);
            anim.SetBool("isIdle", true);
        }
    }
    public void Skill_Vine_Activated()
    {
        Vine.Activated();
    }

    public void Skill_Vomit_Activated()
    {
        Vomit.Activated();
    }
    public void Skill_Vomit_Inactivated()
    {
        Vomit.Inactivated();
    }

    private void Follow()
    {      
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
     }

    public bool Flipped()
    {
        return isFlipped;
    }

    void Active_col()
    {
        damage.SetActive(true);
    }
    void InActive_col()
    {
        damage.SetActive(false);
    }
}
