using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanner : MonoBehaviour {


    //private const int DAMAGE_SWORD = 50;
    //private const int DAMAGE_BLADE = 50;

    /*
    private const float SCANNER_SPEED_SWORD = 4f;
    private const float SCANNER_SPEED_BLADE = 4f;
    */
    private const float SCANNER_SPEED = 4f;

    [SerializeField] private Transform goAtkPoint;
    [SerializeField] private BossManager boss;
    [SerializeField] private bool isWandType;

    private float initPosX;
    private float curPosX;

    private float t;
    
    // SetActive(true)
    void OnEnable() {

        // initial
        GetComponent<CircleCollider2D>().enabled = true;
        this.transform.position = goAtkPoint.position;
        initPosX = this.transform.position.x;

        if (isWandType)
            t = 0.7f;
        else
            t = PlayerControl.GetAttackSpeed() * 0.4f;

        if (PlayerControl.GetIsFacingRight()) {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        else {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        // 크기설정
    }

    void Update() {
        Scan();


        if (t > 0)
            t -= Time.deltaTime;
    }

    void SetSize(float size) {
        this.GetComponent<CapsuleCollider2D>().size = new Vector2(0.5f, size);
    }

    void Scan() {
        if (isWandType)
            transform.Translate(Vector3.left * 14 * Time.deltaTime);
        else
        transform.Translate(Vector3.left * 10 * Time.deltaTime);

        if (t <= 0)
            EndScan();
    }

    void EndScan() {
        //Debug.Log("EndScan");
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            boss.TakeDamage(PlayerControl.GetAttackDamage());
            GetComponent<CircleCollider2D>().enabled = false;
            if (isWandType) {
                EndScan();
            }
        } else if(collision.CompareTag("Knight")){
            collision.GetComponent<Knight>().TakeDamage(PlayerControl.GetAttackDamage());
            GetComponent<CircleCollider2D>().enabled = false;
            if (isWandType) {
                EndScan();
            }
        }
    }
}
