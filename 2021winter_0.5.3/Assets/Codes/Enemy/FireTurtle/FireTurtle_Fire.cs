using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurtle_Fire : MonoBehaviour
{
    private Animator anim;
    BoxCollider2D bc;
    Vector2 pos;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        Invoke("ActiveFalse", 4);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector3.left * Time.deltaTime * 5);
    }

    void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
