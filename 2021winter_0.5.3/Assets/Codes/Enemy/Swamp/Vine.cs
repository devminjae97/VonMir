using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    [SerializeField] private SwampControl swamp;
    [SerializeField] private Transform player;
    private const float EndOfMap_r = 17f;
    private const float EndOfMap_l = -11f;
    private float yOfMap;
    private Vector3 target;
    private float speed =5f;

    private void OnEnable()
    {
        yOfMap = transform.position.y;

        if (swamp.Flipped())
            transform.position = new Vector3(swamp.transform.position.x + 3f,yOfMap);
        else
            transform.position = new Vector3(swamp.transform.position.x - 3f, yOfMap);
        
    }
    private void FixedUpdate()
    {
        if (swamp.Flipped())
        {
            target = new Vector3(EndOfMap_r, yOfMap);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        else if (!swamp.Flipped())
        {
            target = new Vector3(EndOfMap_l, yOfMap);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

        if (gameObject.activeSelf&& (transform.position.x == EndOfMap_r || transform.position.x == EndOfMap_l))
        {
            gameObject.SetActive(false);
        }
    }

    public void Activated()
    {
        gameObject.SetActive(true);
    }
}
