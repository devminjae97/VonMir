using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    private const float LIMIT_BOARD_X = 5.5f;
    private const float LIMIT_BOARD_Y = 1.75f;
    private const float LIMIT_SLOT_X = 0.5f;
    private const int OFFSET_Z = 4;


    void OnEnable() {

        float rand_x = UnityEngine.Random.Range(LIMIT_SLOT_X, LIMIT_BOARD_X);
        float rand_y = UnityEngine.Random.Range(-LIMIT_BOARD_Y, LIMIT_BOARD_Y);

        this.SetPosition(rand_x, rand_y);
    }

    public void SetPosition(int z) {
        //Debug.Log(this.name + " :: SetPosition " + (z - OFFSET_Z));
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, z - OFFSET_Z);
    }

    void SetPosition(float x, float y) {
        this.transform.position = new Vector3(x, y, 0);
    }


}
