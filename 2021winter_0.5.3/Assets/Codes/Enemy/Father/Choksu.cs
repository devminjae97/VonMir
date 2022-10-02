using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choksu : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Father boss;
    private const float yOfMap = -1.0475f;
    [SerializeField] private GameObject[] hitPoints;

    private void OnEnable()
    {
        if (boss.Flipped())
        {
            transform.position = new Vector3(boss.transform.position.x + 5.5f, yOfMap);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        else
        {
            transform.position = new Vector3(boss.transform.position.x - 5.5f, yOfMap);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
    }
    public void Activated()
    {
        gameObject.SetActive(true);
    }

    public void Inactivated()
    {
        hitPoints[3].SetActive(false);
        gameObject.SetActive(false);  
    }
    void Active1()
    {
        hitPoints[0].SetActive(true);
    }
    void Active2()
    {
        hitPoints[0].SetActive(false);
        hitPoints[1].SetActive(true);
    }
    void Active3()
    {
        hitPoints[1].SetActive(false);
        hitPoints[2].SetActive(true);
    }
    void Active4()
    {
        hitPoints[2].SetActive(false);
        hitPoints[3].SetActive(true);
    }
}
