using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private const float TIME_HIT = 0.15f;
    private const float UNIT = 0.0625f;
    private const int PIXEL_HP = 100;
    private const float SPEED_LOSE_HP = 100f;

    // boss id : 1mmii => m : map, i: index
    private const int ID_BOSS_KASTONIA_DUMMY = 10001;
    private const int ID_BOSS_ARDILL_ARMY = 10101;
    private const int ID_BOSS_ARDILL_SWAMP = 10102;
    private const int ID_BOSS_BOOLCITY_WISTONNE = 10201;
    private const int ID_BOSS_REVCASTLE_KING = 10301;


    [SerializeField] private Sprite[] pool_faces;
    [SerializeField] private Sprite[] pool_names;
    [SerializeField] private GameObject[] pool_boss;
    [SerializeField] private GameObject ui_boss;
    [SerializeField] private SpriteRenderer ui_face;
    [SerializeField] private SpriteRenderer ui_name;
    [SerializeField] private Transform mask_hp;
    [SerializeField] private Transform mask_hp_red;
    [SerializeField] private Animator anim;
    [SerializeField] private int currentHP;
    [SerializeField] private float indicatedHP;

    
    private static Dictionary<int, Boss> dxnry_boss;
    private static string currentName;
    private static int currentID;
    private static GameObject currentBoss;

    //private SpriteRenderer mask;
    private QuestManager qm;
    private float t;
    private int initHP;
    private bool isAlive;
    private bool isEnabled;
    private bool isHit;


    // Start is called before the first frame update
    void Start()
    {
        GenerateBoss();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // effect
        if (t > 0)
            t -= Time.deltaTime;
        else {
            //mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0);
            anim.SetBool("isHit", false);
        }

        // slide down
        if(indicatedHP > currentHP) {
            indicatedHP -= Time.deltaTime * SPEED_LOSE_HP;
            RenewHPRedBar();
        } else if(indicatedHP < currentHP) {    // adjust
            indicatedHP = currentHP;
        }

        if(isAlive && indicatedHP <= 0) {
            if (currentBoss.name != "Wistonne") {
                Debug.Log("daed1");
                qm.KillEnemy(currentID);
            }
            Die();
        }
    }

    private void Initialize() {
        //mask = transform.Find("SpriteMask").GetComponent<SpriteRenderer>();
        //mask_hp.localPosition = Vector3.zero;
        //mask_hp_red.localPosition = Vector3.zero;
        qm = this.GetComponent<QuestManager>();
        SetHP(0);
        RenewHPBar();
        RenewHPRedBar();
        isAlive = false;
        isEnabled = false;
    }
    
    void GenerateBoss() {
        dxnry_boss = new Dictionary<int, Boss>();

        dxnry_boss.Add(ID_BOSS_KASTONIA_DUMMY, new Boss(0, "Dummy", 200));
        dxnry_boss.Add(ID_BOSS_ARDILL_ARMY, new Boss(1, "Army", 400));
        dxnry_boss.Add(ID_BOSS_ARDILL_SWAMP, new Boss(2, "Swamp", 800));
        dxnry_boss.Add(ID_BOSS_BOOLCITY_WISTONNE, new Boss(3, "Wistonne", 1000));
        dxnry_boss.Add(ID_BOSS_REVCASTLE_KING, new Boss(4, "King", 1200));
    }

    public void SetBoss(int id) {
        //name = dxnry_enemy[id].GetName();

        Debug.Log("Set Boss");

        Boss boss = dxnry_boss[id];
        int index = boss.GetIndex();

        currentBoss = pool_boss[index];
        currentID = id;
        currentName = boss.GetName();
        ui_face.sprite = pool_faces[index];
        ui_name.sprite = pool_names[index];
        SetHP(boss.GetHP());
        RenewHPBar();
        RenewHPRedBar();

        ui_boss.SetActive(true);
        currentBoss.SetActive(true);
    }

    public void SetAlive() {
        isAlive = true;
        isEnabled = true;
    }

    public bool GetIsAlive() {
        return isAlive;
    }

    private void SetHP(int hp) {
        initHP = hp;
        currentHP = hp;
        indicatedHP = hp;
    }

    public void TakeDamage(int damage) {

        if (isEnabled) {
            currentHP -= damage;
            Debug.Log("Get Damage " + damage);
            CheckHP();

            // 피격 효과
            /*
            mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 0.75f);
            t = TIME_HIT;
            */
            t = TIME_HIT;
            anim.SetBool("isHit", true);
        }
    }

    private void CheckHP() {
        if(currentHP <= 0 && isEnabled) {

            // 행동 멈추는 코드. 실직적으로 이때부터 죽은상태
            //
            
            if (currentBoss.name == "Wistonne") {
                Debug.Log("wistonne");
                StartCoroutine(CoTurtleDead(currentBoss));
            }
            else {
                Debug.Log("norm");
                currentBoss.SetActive(false);
            }


            //isAlive = false;
            currentHP = 0;
        }
        RenewHPBar();
    }

    private void RenewHPBar() {
        if(initHP == 0)
            mask_hp.localPosition = new Vector3(100 * UNIT, 0f, 0f);
        else
            mask_hp.localPosition = new Vector3((float)((int)(PIXEL_HP * (initHP - currentHP) / initHP)) * UNIT, 0f, 0f);
    }

    private void RenewHPRedBar() {
        if (initHP == 0)
            mask_hp_red.localPosition = new Vector3(100 * UNIT, 0f, 0f);
        else
            mask_hp_red.localPosition = new Vector3((float)((int)(PIXEL_HP * (initHP - indicatedHP) / initHP)) * UNIT, 0f, 0f);
    }

    public Vector3 GetBossPos() {
        return currentBoss.transform.position;
    }

    //////////////////////// TEST
    /*
    public void Reset(int i) {
        SetHP(i);
        isAlive = true;
        RenewHPBar();
    }*/

    private void Die() {
        isAlive = false;
        //currentBoss.SetActive(false);

        ui_boss.SetActive(false);

        currentBoss = null;
    }

    IEnumerator CoTurtleDead(GameObject boss) {

        boss.GetComponent<FireTurtle>().Die();

        yield return new WaitForSeconds(8f);

        Debug.Log("daed2");
        qm.KillEnemy(currentID);
    }
}

class Boss {

    private string name;
    private int index;
    private int hp;
    //private int element;

    public Boss(int index, string name, int hp) {
        this.index = index;
        this.name = name;
        this.hp = hp;
    }

    public int GetIndex() {
        return index;
    }

    public string GetName() {
        return name;
    }

    public int GetHP() {
        return hp;
    }
}
