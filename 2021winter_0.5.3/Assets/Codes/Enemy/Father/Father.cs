using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Father : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private BossManager bm;
    [SerializeField] private CameraMove cam;
    [SerializeField] private Transform player;
    [SerializeField] private bool isFlipped = false;
    [SerializeField] private GameObject damage;
    [SerializeField] private Choksu choksu;

    //Jump
    [SerializeField] private float jumpHeight =15f;
    private bool isGrounded;
    private const float xOfMap_r = 16f;
    private const float xOfMap_l = -10f;
    private float yOfMap;
    private float jumpTarget;


    //Related with Skills
    private const float ATTACKRANGE = 3f;
    private float speed;

    [SerializeField] private float invokeTime; //스킬 발동
    [SerializeField] private float wait;
    private Vector3 target;

    //Selection
    [SerializeField] private float rand;
    [SerializeField] private float timer;

    //Spawn
    [SerializeField] private GameObject spawnMob;
    [SerializeField] private PoolingManager poolingManager;
    [SerializeField] private string[] skills;
    [SerializeField] private Transform[] mobSpawn;

    
    private bool isReady;
    private bool isAlive;
    

    private void OnEnable()
    {
        skills = new string[] {"Father_SpawnMob"};
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        isReady = true;
        isAlive = false;

        wait = 5f;
        timer = 1f;
        yOfMap = transform.position.y;
    }
    
    private void FixedUpdate()
    {
        if (bm.GetIsAlive() && isReady) {
            Activate();
        }

        if (isAlive) {
            invokeTime += Time.deltaTime;

            //Selection
            if (anim.GetBool("isIdle")) {
                if (timer <= 0) {
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                        rand = Random.Range(0, 6);
                    }
                    if (rand == 0) {
                        anim.SetBool("isJump", true);
                    }
                    else if ((rand == 1 || rand == 2)) {
                        if (rand == 1 && invokeTime > 5f) {
                            anim.SetBool("isSkill1", true);
                            invokeTime = 0;
                        }
                        else if (rand == 2 && invokeTime <= 30f && invokeTime >= 5f) {
                            anim.SetBool("isSkill2", true);
                            invokeTime = 0;
                        }
                        else {
                            anim.SetBool("isReady", true);
                        }
                    }
                    else if (rand == 3) {
                        speed = 7.5f;
                        anim.SetBool("isFollow", true);
                    }
                    else if (rand == 4)
                        anim.SetBool("isTel", true);
                    else if (rand == 5 || rand == 6) {
                        anim.SetBool("isReady", true);
                    }

                    anim.SetBool("isIdle", false);
                    timer = 1f;
                    LookAtPlayer();
                }
                else
                    timer -= Time.deltaTime;
            }

            //Follow
            if (anim.GetBool("isFollow")) {

                Vector3 target = new Vector3(player.position.x, rb.position.y);
                Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);

                Vector3 offset = player.position - transform.position;
                float distance = offset.sqrMagnitude;
                if (distance <= ATTACKRANGE * ATTACKRANGE) {
                    int pick = Random.Range(0, 5);

                    if (pick == 0)
                    {
                        anim.SetBool("isSkill1", true);
                    }
                    else if (pick == 1)
                    {
                        anim.SetBool("isSkill2", true);
                    }
                    else if (pick == 2|| pick == 3)
                    {
                        anim.SetBool("isReady", true);
                    }
                    else if (pick == 4)
                    {
                        anim.SetBool("isTel", true);
                    }
                    else if (pick ==  5)
                    {
                        anim.SetBool("isJump", true);
                    }
                    anim.SetBool("isFollow", false);
                }
            }

            //Teleportation
            if (anim.GetBool("isTel")) {
                if (player.position.x > 0) {
                    if (transform.position.x > player.position.x) {
                        target = new Vector3(xOfMap_l, yOfMap);
                    }
                    else {
                        target = new Vector3(xOfMap_r, yOfMap);
                    }
                }
                else {
                    if (transform.position.x > player.position.x) {
                        target = new Vector3(xOfMap_l, yOfMap);
                    }
                    else {
                        target = new Vector3(xOfMap_r, yOfMap);
                    }
                }

            }

            //Attack
            if (anim.GetBool("isReady")) {
                damage.SetActive(true);
                wait -= Time.deltaTime;
                if (wait <= 4f) {
                    anim.SetBool("isReady", false);
                    anim.SetBool("isAttack", true);

                    if (!isFlipped)
                        target = new Vector3(xOfMap_l, yOfMap);
                    else
                        target = new Vector3(xOfMap_r, yOfMap);

                    wait = 5f;
                }
            }
            if (!anim.GetBool("isReady") && anim.GetBool("isAttack")) {
                speed = 50f;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target) {
                    wait -= Time.deltaTime;
                    if (wait <= 3) {
                        anim.SetBool("isAttack", false);
                        anim.SetBool("isIdle", true);
                        damage.SetActive(false);
                        wait = 5f;
                    }
                }
            }
            //Skill1 - Spawn
            if (anim.GetBool("isSkill1")) {
                wait -= Time.deltaTime;
                if (wait <= 3) {
                    anim.SetBool("isSkill1", false);
                    anim.SetBool("isIdle", true);
                    wait = 5f;
                }
            }
            //Skill2 - Choksu
            if (anim.GetBool("isSkill2")) {
                wait -= Time.deltaTime;
                if (wait <= 3) {
                    anim.SetBool("isSkill2", false);
                    anim.SetBool("isIdle", true);
                    wait = 5f;
                }
            }

            //Jump
            if (anim.GetBool("isJump") && isGrounded) {
                wait -= Time.deltaTime;
                if (wait <= 3.5) {
                    anim.SetBool("isJump", false);
                    anim.SetBool("isStart", true);
                    wait = 5f;
                }
            }
             if (!anim.GetBool("isJump") && anim.GetBool("isStart") && isGrounded) {
                if (player.position.x >= 0) {
                    if (transform.position.x > player.position.x) {
                        jumpTarget = xOfMap_l;
                    }
                    else {
                        jumpTarget = xOfMap_r;
                    }
                }
                else {
                    if (transform.position.x > player.position.x) {
                        jumpTarget = xOfMap_l;
                    }
                    else {
                        jumpTarget = xOfMap_r;
                    }
                }
                rb.AddForce(new Vector2(jumpTarget, jumpHeight), ForceMode2D.Impulse);
                isGrounded = false;
            }
            if (rb.velocity.y < 0f && !isGrounded) {
                anim.SetBool("isFall", true);
                anim.SetBool("isStart", false);
            }

            // land
            if (isGrounded && anim.GetBool("isFall")) {
                anim.SetBool("isFall", false);
                anim.SetBool("isLand", true);
            }
            if (isGrounded && !anim.GetBool("isFall") && anim.GetBool("isLand")) {
                wait -= Time.deltaTime;
                if (wait <= 3.5) {
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    {
                        int pick = Random.Range(0, 5);
                        if (pick == 0)
                        {
                            anim.SetBool("isSkill1", true);
                        }
                        else if (pick == 1)
                        {
                            anim.SetBool("isSkill2", true);
                        }
                        else if (pick == 2 || pick == 3)
                        {
                            anim.SetBool("isReady", true);
                        }
                        else if (pick == 4)
                        {
                            anim.SetBool("isFollow", true);
                        }
                        else if (pick == 5)
                        {
                            anim.SetBool("isTel", true);
                        }
                        anim.SetBool("isLand", false);
                        LookAtPlayer();
                    }
                  
                    wait = 5f;
                }
            }
        }
    }
    private void Activate() {
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

    void SpawnMob()
    {
        int ranPoint = Random.Range(0, 4);
        GameObject fatherSkill_Mob = poolingManager.MakeObj(skills[0]);
        fatherSkill_Mob.transform.position = mobSpawn[ranPoint].position;
    }

     void Teleportation()
    {
        transform.position = target;
        if(transform.position == target)
        {
            LookAtPlayer();
            int pick = Random.Range(0, 5);
            if (pick == 0)
            {
                anim.SetBool("isSkill1", true);
            }
            else if (pick == 1)
            {
                anim.SetBool("isSkill2", true);
            }
            else if (pick == 2 || pick == 3)
            {
                anim.SetBool("isReady", true);
            }
            else if (pick == 4)
            {
                anim.SetBool("isFollow", true);
            }
            else if (pick == 5)
            {
                anim.SetBool("isJump", true);
            }
            anim.SetBool("isTel", false);
        }
    }

    public void Choksu_Activated()
    {
        choksu.Activated();
    }

    public void Choksu_Inactivated()
    {
        choksu.Inactivated();
    }

    public bool Flipped()
    {
        return isFlipped;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform"))
            isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform"))
            isGrounded = false;
    }

    void ShakeCam() {
        cam.Shake();
    }
}
