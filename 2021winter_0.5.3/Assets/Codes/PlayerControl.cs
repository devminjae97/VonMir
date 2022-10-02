using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    private const float COOL_DASH = 2f;
    private const float COOL_SWITCH = 1f;
    private const float DURATION_DASH = 0.25f;
    private const float UNIT_PIXEL = 0.0625f;
    private const float UNIT_HP_HALF = 0.3125f; 
    private const int PIXEL_SKILL = 14;
    private const int PIXEL_SWITCH = 22;
    private const int HP_INIT = 10;

    private static readonly float[] list_attackSpeed = { 0.45f, 0.27f, 0.8f};
    private static readonly int[] list_attackDamage = { 50, 25, 15};
    //private static readonly int[] list_attackDamage = { 150, 75, 40};   // power test

    //public SpriteRenderer spriteRenderer;
    public Animator anim;
    public Animator animAura;

    [SerializeField] private BossManager bm;
    [SerializeField] private GameObject hitScanner;
    [SerializeField] private GameObject hitScannerWand;
    [SerializeField] private GameObject aura;
    [SerializeField] private GameObject screamer;
    [SerializeField] private GameObject frame_weapon1;
    [SerializeField] private GameObject frame_weapon2;
    [SerializeField] private SpriteRenderer spriteRenderer_mainWeapon;
    [SerializeField] private SpriteRenderer spriteRenderer_subWeapon;
    [SerializeField] private Sprite[] arr_sprite_mainWeapon;
    [SerializeField] private Sprite[] arr_sprite_subWeapon;
    [SerializeField] private Transform cool_a;
    [SerializeField] private Transform cool_d;
    [SerializeField] private Transform cool_f;
    [SerializeField] private Transform cool_s1;
    [SerializeField] private Transform cool_s2;
    [SerializeField] private Transform mask_hp;
    [SerializeField] private GameObject hitEffectColor;
    private SpriteRenderer hitColor;

    private GameObject[] pool_hitScanner;
    private Transform cool_switch;
    private Rigidbody2D rigid;
    private SpriteRenderer sr;
    private CapsuleCollider2D col;

    private float moveAccel = 0.75f;
    private float maxSpeed = 5f;
    private float resistSpeed = 0.9f;
    private float jumpPower = 12f;
    private float dashPower = 10f;


    private float tDash;
    private float tAttack;
    private float tSwitch;
    private float tOnAction;

    private float h;
    private static bool isFacingRight;
    private bool isGround;
    private bool isAttack;
    private bool isLie;
    private bool isDash;
    private bool isCharged;
    private bool isControlEnable;
    private bool isJumpEnable;
    private bool isMainWeapon;
    private bool isSwitchEnable;
    private bool isOnAction;
    private bool isDead;
    private bool isAutoControl;
    private bool isMoveEnd;

    private int numWeapon;   // 1 : main, 2 : sub
    private int hp;

    private static int currentWeapon;  // 0: sword, 1: blade, ...
    private static int mainWeapon;
    private static int subWeapon;

    /*
    private static int mainStone;
    private static int subStone;
    */

    private int jumpTime;


    private void Awake() {
        Initialize();
    }

    private void Update() {
        Timer();
        // animation
        if (h == 0 || (!isControlEnable && !isAutoControl) || !isGround)
            anim.SetBool("isRun", false);
        else
            anim.SetBool("isRun", true);


        if (isControlEnable) {
            if (!isOnAction && !isDead) {
                if (!isLie /*&& !isDash*/) {
                    // jump
                    if (Input.GetButtonDown("Jump") && isGround) {
                        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                        anim.SetBool("isJump", true);
                        isGround = false;
                    }
                    else if (Input.GetButtonDown("Jump") && !isGround && jumpTime > 0) {
                        rigid.velocity = new Vector3(rigid.velocity.x, 0, 0);
                        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                        anim.SetBool("isJump", true);
                        jumpTime--;
                    }
                    
                    // attack
                    if (Input.GetKey(KeyCode.A) && !isAttack) {
                        Attack();
                    }

                    // sprite direction
                    /*
                    if (Input.GetKey(KeyCode.LeftArrow)) {
                        spriteRenderer.flipX = false;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                        spriteRenderer.flipX = true;

                    if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                        if (Input.GetKey(KeyCode.RightArrow))
                            spriteRenderer.flipX = true;
                    }
                    else if (Input.GetKeyUp(KeyCode.RightArrow)) {
                        if (Input.GetKey(KeyCode.LeftArrow))
                            spriteRenderer.flipX = false;
                    }
                    */

                    if ((Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && isFacingRight) ||
                        (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !isFacingRight) ||
                        (Input.GetKeyUp(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) && !isFacingRight) ||
                        (Input.GetKeyUp(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) && isFacingRight)) {
                        Flip();
                    }



                    // dash
                    if (h != 0 && Input.GetKey(KeyCode.D) && isGround && tDash <= 0) {
                        rigid.AddForce(Vector2.right * dashPower * h, ForceMode2D.Impulse);
                        isDash = true;
                        isOnAction = true;
                        cool_d.localPosition = Vector3.zero;
                        anim.SetBool("isDash", true);
                        tDash = COOL_DASH;
                    }
                }


                // Lie
                if (Input.GetKey(KeyCode.DownArrow) && isGround && h == 0) {
                    SetPlayerLie(true);
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow) || !isGround || h != 0) {
                    SetPlayerLie(false);
                }

                // Switch Weapon
                if (Input.GetKeyDown(KeyCode.Alpha1) && isSwitchEnable) {
                    SwitchWeapon(1);
                    // 임시 코드
                    /*
                    isSwitchEnable = false;

                    if (numWeapon == 0)
                        numWeapon = 1;
                    else
                        numWeapon = 0;
                    anim.SetInteger("numWeapon", numWeapon);
                    tSwitch = COOL_SWITCH;
                    */
                }
                if(Input.GetKeyDown(KeyCode.Alpha2) && isSwitchEnable) {
                    SwitchWeapon(2);
                }
                
                // Fall
                if (rigid.velocity.y < 0f && /*!isAttack*/ !isOnAction) {
                    if (!anim.GetBool("isFall")) {
                        //Debug.Log(":: FALL");
                    }
                    if (isLie) {
                        isLie = false;
                        anim.SetBool("isLie", false);
                    }
                    anim.SetBool("isJump", false);
                    anim.SetBool("isFall", true);
                }
            }
        }

        // land
        if (isGround && anim.GetBool("isFall")) {
            anim.SetBool("isFall", false);
        }



        // accel
        if (!isGround)
            moveAccel = 0.5f;
        else
            moveAccel = 5.0f;


        if(hp == 0 && isGround && !isDead) {
            isDead = true;
            anim.SetTrigger("trgDead");
            Debug.Log("PC :: DEADDDDDDDDDDDDDDDDDDDDDDDD");
        }


        if(isDead && Input.GetKeyDown(KeyCode.F)) {
            Resurrection();
        }

    }

    private void FixedUpdate() {
        if (isControlEnable)
            h = Input.GetAxisRaw("Horizontal");
        else if (!isAutoControl) {
            h = 0;
        }

        // run
        if ((isControlEnable && !isOnAction) || isAutoControl) {
            rigid.AddForce(Vector2.right * moveAccel * h, ForceMode2D.Impulse);
            if (tDash < COOL_DASH - 0.15f)
                rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -maxSpeed, maxSpeed), rigid.velocity.y);
            else
                rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -maxSpeed - 12, maxSpeed + 12), rigid.velocity.y);
        }

        // stop - 지면에서만
        if (h == 0 && rigid.velocity.x != 0f && isGround) {
            rigid.velocity = new Vector2(rigid.velocity.x * resistSpeed, rigid.velocity.y);
        }


        // dash

    }

    private void Initialize() {

        // init start
        isControlEnable = false;

        hitColor = hitEffectColor.GetComponent<SpriteRenderer>();
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        sr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        isJumpEnable = false;
        isSwitchEnable = true;
        isGround = false;
        isFacingRight = false;
        isAttack = false;
        isLie = false;
        isDash = false;
        isCharged = true;
        isOnAction = false;
        isDead = false;
        isAutoControl = false;
        isMoveEnd = false;
        //isMainWeapon = true;
        tDash = 0f;
        tAttack = 0f;
        tOnAction = 0f;
        tSwitch = 0f;
        hp = HP_INIT;

        SetPlayerLie(false);

        pool_hitScanner = new GameObject[12];
        for(int i = 0; i < 12; i++) {
            GameObject hs = Instantiate(hitScannerWand);
            hs.SetActive(false);
            pool_hitScanner[i] = hs;
        }

        // 임시-----
        mainWeapon = 0;
        subWeapon = 2;
        // ---------

        spriteRenderer_mainWeapon.sprite = arr_sprite_mainWeapon[mainWeapon];
        spriteRenderer_subWeapon.sprite = arr_sprite_subWeapon[subWeapon];

        cool_switch = cool_s1;
        frame_weapon1.SetActive(true);
        frame_weapon2.SetActive(false);

        numWeapon = 1;
        currentWeapon = 0;
        anim.SetInteger("currentWeapon", currentWeapon);

        CheckHP();

        if (currentWeapon == 2) 
            aura.SetActive(true);
        else
            aura.SetActive(false);
        
        // init end
        isControlEnable = true;
    }

    public void SetPlayerPosition(Vector3 vec) {
        transform.position = vec;
    }

    public void SetPlayerPosition(float x) {
        Vector3 vec = transform.position;
        transform.position = new Vector3(x, vec.y, vec.z);
    }

    private void SwitchWeapon(int num) {

        if (num != numWeapon) {
            //isMainWeapon = !isMainWeapon;
            numWeapon = num;
            isSwitchEnable = false;

            if (num == 1) {
                currentWeapon = mainWeapon;
                cool_switch = cool_s1;
                frame_weapon1.SetActive(true);
                frame_weapon2.SetActive(false);
            }
            else if (num == 2){
                currentWeapon = subWeapon;
                cool_switch = cool_s2;
                frame_weapon2.SetActive(true);
                frame_weapon1.SetActive(false);
            }

            if (currentWeapon == 2) {  // Wand
                isCharged = true;
                aura.SetActive(true);
            }
            else
                aura.SetActive(false);

            anim.SetInteger("currentWeapon", currentWeapon);
            tSwitch = COOL_SWITCH;
        }
    }

    public void SetFaceRight(bool b) {
        if (b == !isFacingRight) {
            Flip();
        } 
    }

    private void Flip() {
        isFacingRight = !isFacingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    private void Timer() {
        // KeyCode.A
        // attack
        if (tAttack > 0) {
            tAttack -= Time.deltaTime;
            cool_a.localPosition = new Vector3(0f, -(float)((int)(PIXEL_SKILL * (list_attackSpeed[currentWeapon] - tAttack) / list_attackSpeed[currentWeapon])) * UNIT_PIXEL, 0f);
        } else if (tAttack < 0) {
            
            if (currentWeapon == 2) { // wand
                animAura.SetBool("isAtk", false);
            }

            AtkEnd();
        }


        // KeyCode.1,2
        if (tSwitch > 0) {
            tSwitch -= Time.deltaTime;
            cool_switch.localPosition = new Vector3(0f, -(float)((int)(PIXEL_SWITCH * (COOL_SWITCH - tSwitch) / COOL_SWITCH)) * UNIT_PIXEL, 0f);
        } else if(tSwitch < 0) {
            isSwitchEnable = true;
            tSwitch = 0;
        }


        // KeyCode.D
        // dash
        if (tDash > 0) {
            tDash -= Time.deltaTime;

            //- PIXEL_SKILL * tDash/TIME_DASH

            cool_d.localPosition = new Vector3(0f, -(float)((int)(PIXEL_SKILL * (COOL_DASH - tDash) / COOL_DASH)) * UNIT_PIXEL, 0f);
            //Debug.Log(">> COOL DOWN : " + (float)((int)(PIXEL_SKILL * (TIME_DASH - tDash) / TIME_DASH)) * PixelUnit);
            //Debug.Log(">> " + cool_d.position);
        }
        if (/*isDash*/isOnAction && tDash < COOL_DASH - 0.25f && tDash > 0) { //<<<<<<<<<<<< 여기 어케ㅎ지
            isDash = false;
            CheckOnAction();
            anim.SetBool("isDash", false);
            Debug.Log(":: stop dash");
        } else if(tDash < 0) {
            tDash = 0;
        }

    
        // KeyCode.F
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Platform") {
            isGround = true;
            jumpTime = 1;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Platform")
            isGround = false;
    }

    private void Attack() {
        if (currentWeapon == 2 && !isCharged)
            return;

        if (anim.GetBool("isJump"))
            anim.SetBool("isJump", false);
        anim.SetBool("isAtk", true);
        anim.SetTrigger("trgAtk");
        tAttack = list_attackSpeed[currentWeapon];
        isAttack = true;
        isOnAction = true;
        //isAbleToControl = false;


        //hitScanner.SetActive(true);

        if(currentWeapon == 2) { // wand
            animAura.SetBool("isAtk", true);
        }

    }

    private void SetHitScanner() {
        if (currentWeapon == 2) {
            StartCoroutine(SetHitScannerWand());
        }
        else
            hitScanner.SetActive(true);
    }

    IEnumerator SetHitScannerWand() {
        int n = 0;
        int i = 0;
        while(n < 4) {
            if (!pool_hitScanner[i].activeSelf) {
                pool_hitScanner[i].SetActive(true);
                n++;
                yield return new WaitForSeconds(0.05f);
            }
            i++;
        }
    }

    private void AtkEnd() {
        Debug.Log(":: END ATK");
        anim.SetBool("isAtk", false);
        //anim.SetTrigger("trgEndAtk");
        //isAttack = false;
        //isAbleToControl = true;
        isAttack = false;
        CheckOnAction();
        tAttack = 0;
        

    }

    private void ActionEnable() {
        isOnAction = false;
    }

    private void ChargeMana() {
        isCharged = true;
    }

    /*
    private void Testest() {
        Debug.Log(":::::::::::: END ATK");
    }*/

    public static bool GetIsFacingRight() {
        return isFacingRight;
    }

    public static int GetAttackDamage() {
        return list_attackDamage[currentWeapon];
    }

    public static float GetAttackSpeed() {
        return list_attackSpeed[currentWeapon];
    }

    private void CheckOnAction() {
        if (isAttack || isDash)
            isOnAction = true;
        else
            isOnAction = false;
    }

    public void SetScreamer(bool b) {
        screamer.SetActive(b);
    }

    public void SetPlayerLie(bool b) {
        // collider 크기 줄이기 추가
        if (b) {
            col.size = new Vector2(0.5f, 0.5f);
            col.offset = new Vector2(0f, -0.75f);
        }
        else {
            col.size = new Vector2(0.5f, 1f);
            col.offset = new Vector2(0f, -0.5f);
        }

        isLie = b;
        anim.SetBool("isLie", b);
    }

    public void TakeDamage(int i) {
        // if isAlive || isControlEnable
        hp -= i;
        CheckHP();
    }

    private void CheckHP() {

        if (hp <= 0 && !isDead) {
            hp = 0;
            isControlEnable = false;
            aura.SetActive(false);
            // position 제자리(가속도 없애기)
            rigid.AddForce(Vector3.up, ForceMode2D.Impulse);
            anim.SetBool("isJump", false);
            anim.SetTrigger("trgDeadF");
            Debug.Log("PC :: DEADFFFFFFFFFFFFFFFFFFFFFFFF");
        }
        RenewHPBar();
    }

    private void RenewHPBar() {

        mask_hp.localPosition = new Vector3(-(HP_INIT - hp) * UNIT_HP_HALF, 0f, 0f);
    }

    public void MovePlayerToX(float x) {
        StartCoroutine(MovePlayer(x));
    }

    IEnumerator MovePlayer(float x) {
        Vector3 dir = new Vector3(x - transform.position.x, 0, 0).normalized;

        isAutoControl = true;
        if ((dir.x == 1 && !isFacingRight) || (dir.x == -1 && isFacingRight))
            Flip();

        do {
            h = dir.x;
            yield return null;
            if (!((x - transform.position.x) * dir.x > 0.01f * dir.x))
                isAutoControl = false;
        } while (isAutoControl);

        isAutoControl = false;
        isMoveEnd = true;


        h = 0;
    }

    public void StopMove() {
        Debug.Log("PC :: Stop Move");
        anim.SetBool("isJump", false);
        anim.SetBool("isFall", false);
        anim.SetBool("isDash", false);
        anim.SetBool("isLie", false);
        isAutoControl = false;
    }
    
    public bool GetIsMoveEnd() {
        if (isMoveEnd) {
            Debug.Log("PC :: Move End!");
            isMoveEnd = false;
            return true;
        }
        return false;
    }

    public bool GetIsGround() {
        return isGround;
    }

    public void TakeGroundDamage(int d) {
        if (!isDead && this.tag == "Player") {
            int dir = isFacingRight ? -1 : 1;
            /*
            rigid.velocity = Vector3.zero;
            rigid.AddForce(Vector2.up * jumpPower * 0.5f + Vector2.right * 8 * dir, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
            isGround = false;
            */

            OnDamaged(new Vector3(dir * -100, 0, 0));

            TakeDamage(d);
        }
    }

    public void OnDamaged() {
        OnDamaged(bm.GetBossPos());
    }

    public void OnDamaged(Vector3 targetPos) {
        gameObject.tag = "Untagged";

        sr.color = new Color(1, 1, 1, 0.4f);

        int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.velocity = Vector3.zero;
        rigid.AddForce(new Vector3(dir, 1, 0) * 8, ForceMode2D.Impulse);

        StartCoroutine(HitEffect(1, 0));

        Invoke("OffDamage", 3);
    }

    IEnumerator HitEffect(float start, float end)
    {
        hitEffectColor.SetActive(true);
        for (int i = 10; i >= 0; i--)
        {
            float f = i * 0.1f;
            Color c = hitColor.color;
            c.a = f;
            hitColor.color = c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OffDamage() {
        gameObject.tag = "Player";
        sr.color = new Color(1, 1, 1, 1);
    }

    private void Resurrection() {
        anim.SetTrigger("trgAlive");
        Initialize();
    }

    /// TEST
    public void Reset() {
        Initialize();
    }

    public void SetControlEnabled() {
        isControlEnable = true;
    }
    
    public void SetControlDisabled() {
        isControlEnable = false;
    }
}
