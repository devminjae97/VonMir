using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurtle : MonoBehaviour {

    [SerializeField] private BossManager bm;
    [SerializeField] private CameraMove cam;
    [SerializeField] private PlayerControl player;

    public PoolingManager poolingManager;
    public string[] skills;
    public Transform[] spawnPoints;
    public Transform[] fireSpawnPoint;
    public GameObject FireTurtleBeam;
    //public bool isSKilling = false;
    
    private Animator anim;
    private bool isReady;
    private bool isAlive;

    private List<string> ranList = new List<string>() { "GroundSlam", "Fire", "Beam" };
    private string stringStore;

    private void Awake()
    {
        skills = new string[] { "FireTurtle_JUS", "FireTurtle_Fire" };
        anim = GetComponent<Animator>();
        isReady = true;
        isAlive = false;
    }

    private void Update() {
        if (bm.GetIsAlive() && isReady) {
            Activate();
        }
    }


    IEnumerator CoIdle()
    {
        anim.SetTrigger("idle");
        yield return new WaitForSeconds(2f);

        int rand = Random.Range(0, ranList.Count);
        switch (ranList[rand])
        {
            case "GroundSlam":
                StartCoroutine(CoGroundSlam());
                break;
            case "Fire":
                StartCoroutine(CoFire());
                break;
            case "Beam":
                StartCoroutine(CoBeam());
                break;
        }
    }

    IEnumerator CoGroundSlam()
    {
        anim.SetTrigger("groundSlam");
        if (ranList.Count != 3)
            ranList.Add(stringStore);
        yield return new WaitForSeconds(2f);
        StartCoroutine(CoIdle());
    }

    IEnumerator CoFire()
    {
        anim.SetTrigger("fire");
        if (ranList.Count != 3)
            ranList.Add(stringStore);
        ranList.Remove("Fire");
        stringStore = "Fire";

        yield return new WaitForSeconds(2f);
        StartCoroutine(CoIdle());
    }

    IEnumerator CoBeam()
    {
        anim.SetTrigger("beam");
        if (ranList.Count != 3)
            ranList.Add(stringStore);
        ranList.Remove("Beam");
        stringStore = "Beam";

        yield return new WaitForSeconds(3f);
        StartCoroutine(CoIdle());
    }

    IEnumerator CoSpawnJUS() {
        int ranNum = Random.Range(0, 3);
        switch (ranNum) {
            case 0:
                for (int i = 0; i < 5; i++) {
                    int ranPoint = Random.Range(0, 9);
                    GameObject turtleSkill1 = poolingManager.MakeObj(skills[0]);
                    turtleSkill1.transform.position = spawnPoints[ranPoint].position;
                    yield return new WaitForSeconds(0.5f);
                }
                break;
            case 1:
                for (int i = 0; i < 10; i += 2) {
                    GameObject turtleSkill2 = poolingManager.MakeObj(skills[0]);
                    turtleSkill2.transform.position = spawnPoints[i].position;
                }
                break;
            case 2:
                for (int i = 1; i < 10; i += 2) {
                    GameObject turtleSkill3 = poolingManager.MakeObj(skills[0]);
                    turtleSkill3.transform.position = spawnPoints[i].position;
                }
                break;
        }
        yield return null;
    }

    private void Activate() {
        isAlive = true;
        isReady = false;
        StartCoroutine(CoIdle());
    }

    void ShakeGround() {
        cam.Shake(0.35f, 0.25f);
    }

    void SpawnJUS()
    {
        StartCoroutine(CoSpawnJUS());
    }

    void SpawnFire()
    {
        GameObject turtleFireSkill = poolingManager.MakeObj(skills[1]);
        turtleFireSkill.transform.position = fireSpawnPoint[0].position;
    }

    void SpawnBeam()
    {
        FireTurtleBeam.SetActive(true);
    }

    void DisableBeam()
    {
        FireTurtleBeam.SetActive(false);
    }

    void GroundDamage() {
        if (player.GetIsGround()) {
            player.TakeGroundDamage(1);
        }
    }

    public void Die() {
        DisableBeam();
        Debug.Log("daed");
        anim.SetTrigger("die");
    }

    void Dead() {
        gameObject.SetActive(false);
    }
}
