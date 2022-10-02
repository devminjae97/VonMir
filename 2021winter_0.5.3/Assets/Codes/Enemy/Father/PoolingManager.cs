using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    [SerializeField]private GameObject fireTurtle_JUSPrefab;
    [SerializeField]private GameObject fireTurtle_FirePrefab;
    [SerializeField]private GameObject father_SpawnMobPrefab;

    GameObject[] fireTurtle_JUS;
    GameObject[] fireTurtle_Fire;
    GameObject[] targetPool;

    GameObject[] father_SpawnMob;

    private void Awake()
    {
        fireTurtle_JUS = new GameObject[16];
        fireTurtle_Fire = new GameObject[6];

        father_SpawnMob = new GameObject[24];

        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < fireTurtle_JUS.Length; i++)
        {
            fireTurtle_JUS[i] = Instantiate(fireTurtle_JUSPrefab);
            fireTurtle_JUS[i].SetActive(false);
        }
        
        for (int i = 0; i < fireTurtle_Fire.Length; i++)
        {
            fireTurtle_Fire[i] = Instantiate(fireTurtle_FirePrefab);
            fireTurtle_Fire[i].SetActive(false);
        }

        for (int i = 0; i < father_SpawnMob.Length; i++)
        {
            father_SpawnMob[i] = Instantiate(father_SpawnMobPrefab);
            father_SpawnMob[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "FireTurtle_JUS":
                targetPool = fireTurtle_JUS;
                break;
            case "FireTurtle_Fire":
                targetPool = fireTurtle_Fire;
                break;
            case "Father_SpawnMob":
                targetPool = father_SpawnMob;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }
}
