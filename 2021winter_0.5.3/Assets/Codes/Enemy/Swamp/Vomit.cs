using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vomit : MonoBehaviour
{
    [SerializeField] private SwampControl swamp;
    private float yOfMap;
    [SerializeField] private GameObject[] hitPoints;

    private void OnEnable()
    {
        yOfMap = transform.position.y;
        if (swamp.Flipped())
        {
            transform.position = new Vector3(swamp.transform.position.x + 11f, yOfMap);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
        else
        {
            transform.position = new Vector3(swamp.transform.position.x - 11f, yOfMap);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }     
    }

    public void Activated()
    {
        gameObject.SetActive(true);
    }

    public void Inactivated()
    {
        gameObject.SetActive(false);
    }

    void Active1()
    {
        hitPoints[0].SetActive(true);
    }
    void Active2()
    {
        hitPoints[1].SetActive(true);
    }
    void Active3()
    {
        hitPoints[2].SetActive(true);
    }
    void Active4()
    {
        hitPoints[2].SetActive(false);
        hitPoints[3].SetActive(true);
    }
    void Active5()
    {
        hitPoints[3].SetActive(false);
        hitPoints[4].SetActive(true);
    }
    void Active6()
    {
        hitPoints[4].SetActive(false);
        hitPoints[5].SetActive(true);
    }
    void Active7()
    {
        hitPoints[5].SetActive(false);
        hitPoints[6].SetActive(true);
    }
    void Active8()
    {
        hitPoints[6].SetActive(false);
        hitPoints[7].SetActive(true);
    }
    void Active9()
    {
        hitPoints[7].SetActive(false);
        hitPoints[8].SetActive(true);
    }
    void Active10()
    {
        hitPoints[1].SetActive(false);
        hitPoints[8].SetActive(false);
    }
    void Active11()
    {
        hitPoints[0].SetActive(false);
    }
}
