using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    private bool isFlipped = false;
    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private GameObject col;

    private float wait;
    private float timer;
    private float speed = 4f;

    //Color
    private SpriteRenderer spr;
    void OnEnable()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();

        spr.material.color = Color.white;

        col.SetActive(false);
        anim.SetBool("isDie", false);
        anim.SetBool("isRun", true);
        wait = 3f;
        timer = 1f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer <= 0)
        {
            LookAtPlayer();
            Vector3 target = new Vector3(player.position.x, rb.position.y);
            Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            
            if (!anim.GetBool("isDie"))
            {
                float change = 0.00075f * speed;
                spr.material.color = Color.Lerp(spr.material.color, Color.red, change);             
            }
            else
            {
                col.SetActive(true);
                spr.material.color = Color.white;         
            }
        }
        else
            timer -= Time.deltaTime;

        wait -= Time.deltaTime;
        if (wait <= 0)
        {
            anim.SetBool("isDie", true);
        }
    }

    public void Inactivated()
    {
        gameObject.SetActive(false);
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

    void BoomStart() {
        col.SetActive(true);
    }

    void BoomEnd() {
        col.SetActive(false);
    }
}
