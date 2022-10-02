using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JUS : MonoBehaviour
{
    private Animator anim;
    Rigidbody2D rb;
    BoxCollider2D bc;

    private void OnEnable() {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        rb.constraints = RigidbodyConstraints2D.None;
        bc.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag( "Platform"))
        {
            bc.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            anim.SetBool("JUSBreaking", true);
        } 
    }

    void JUSDisable()
    {
        gameObject.SetActive(false);
    }
}
