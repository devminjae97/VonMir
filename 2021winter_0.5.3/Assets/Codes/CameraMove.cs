using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private  float limit_l;
    private  float limit_r;

    public Transform tf_player;
    public Transform tf_holder;

    private bool isSnapToPlayer;
    private bool isShaking;
    private bool isTraveling;
    private bool isManualMode;

    void Awake(){
        isSnapToPlayer = true;
        isShaking = false;
        isTraveling = false;
        isManualMode = false;
        limit_l = -100f;
        limit_r = 100f;
    }

    void Update() {
        if (isSnapToPlayer && !isTraveling && !isManualMode) {
            tf_holder.position = new Vector3(tf_player.position.x, 0f, -10f);
        }

        /*
        //Test
        if (Input.GetKeyDown(KeyCode.S) && !isShaking) {
            StartCoroutine(IEShake(0.35f, 0.15f));
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            StartCoroutine(IETravel(new Vector3(-10, 0, -10), new Vector3(20, 0, -10), 2f));
        }
        */
        if(!isManualMode)
            CheckLimit();
    }

    public void SetLimit(float l, float r) {
        limit_l = l;
        limit_r = r;
    }
    
    private void CheckLimit() {
        if (tf_player.position.x < limit_l || tf_player.position.x > limit_r)
            isSnapToPlayer = false;
        else
            isSnapToPlayer = true;

        if (tf_holder.position.x < limit_l)
            tf_holder.position = new Vector3(limit_l, 0f, -10f);
        if (tf_holder.position.x > limit_r)
            tf_holder.position = new Vector3(limit_r, 0f, -10f);
    }

    public void SetIsSnapping(bool b) {
        isManualMode = !b;
    }

    public void SetPositionX(float x) {
        Vector3 v = tf_holder.position;
        tf_holder.position = new Vector3(x, v.y, v.z);
    }

    public void Shake(float duration, float magnitude) {
        StartCoroutine(IEShake(duration, magnitude));
    }

    public void Shake() {
        StartCoroutine(IEShake(0.35f, 0.15f));
    }

    public void Travel(Vector3 start, Vector3 end, float speed) {
        StartCoroutine(IETravel(start, end, speed));
    }

    public void Travel(float f, float speed) {
        Vector3 vecFinal = new Vector3(f, 0, -10);
        StartCoroutine(IETravel(tf_holder.position, vecFinal, speed));
    }

    IEnumerator IEShake(float duration, float magnitude) {
        Vector3 pos_init = transform.localPosition;
        float elapsed = 0.0f;

        isShaking = true;

        while(elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos_init.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = pos_init;
        isShaking = false;
    }
    
    IEnumerator IETravel(Vector3 start, Vector3 end, float speed) {
        Vector3 dir = (end - start).normalized;

        isTraveling = true;
        isSnapToPlayer = false;

        tf_holder.position = start;
        
        while ((tf_holder.position - end).sqrMagnitude > 0.01f) {

            tf_holder.position += dir * Time.deltaTime * speed;

            yield return null;
        }
        
        isSnapToPlayer = true;
        isTraveling = false;
    }

    public bool GetIsTraveling() {
        return isTraveling;
    }
}
