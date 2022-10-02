using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    // 잡몹 id : 2mmii => m : map, i: index
    private const int ID_ENEMY_CASTLE_KNIGHT = 20401;


    private static Dictionary<int, Enemy> dxnry_enemy;

    //[SerializeField] private GameObject go_knight;
    [SerializeField] private GameObject[] pool_knights;
    private static Dictionary<int, GameObject[]> dxnry_pool;

    // Start is called before the first frame update
    void Start() {
        GenerateEnemies();
        Initialize();
    }

    // Update is called once per frame
    void Update() {

    }

    private void Initialize() {

    }

    private void GenerateEnemies() {
        dxnry_pool = new Dictionary<int, GameObject[]>();

        dxnry_enemy = new Dictionary<int, Enemy>();

        dxnry_enemy.Add(ID_ENEMY_CASTLE_KNIGHT, new Enemy(0, "knight", 100));




        // instantiate
        /*
        pool_knights = new GameObject[6];
        for (int i = 0; i < 6; i++) {
            GameObject knight = Instantiate(go_knight);
            knight.SetActive(false);
            pool_knights[i] = knight;
        }*/


        // organization
        dxnry_pool.Add(ID_ENEMY_CASTLE_KNIGHT, pool_knights);
    }

    public void SetEnemy(int id, int num) {
        GameObject[] pool = dxnry_pool[id];

        if (num > pool.Length)
            num = pool.Length;

        for(int i = 0; i < num; i++) {
            pool[i].SetActive(true);
        }

        Debug.Log("em :: enemy!");
    }
}

class Enemy {

    private string name;
    private int index;
    private int hp;
    //private int element;

    public Enemy(int index, string name, int hp) {
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
