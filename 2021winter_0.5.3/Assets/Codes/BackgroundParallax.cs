using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundParallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    void Start() {
        startpos = transform.position.x;
        length = GetComponent<Tilemap>().size.x;
    }

    private void LateUpdate() {

        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }

    void FixedUpdate() {
    }
}
