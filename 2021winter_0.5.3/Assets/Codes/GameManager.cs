using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //private const float LIMIT_R_KASTONIA = asdf; 

    [SerializeField] private GameObject[] backgrounds;  // 0: tutorial, 1: forest, 2: castle, 3: funeral, 4: army, 5: ardill, 6: swamp, 7: boolyard, 8: boolcity, 9: cave
    [SerializeField] private GameObject[] tiles;        // 0: grass, 1: aisle, 2: chamber, 3: funeral, 4: army, 5: ardill, 6: swamp, 7: boolyard, 8: boolcity, 9: toTheCave, 10: cave, 11: revAisle, 12: revChamber
    [SerializeField] private GameObject[] go_curtains;  // 0: black, 1: white
    [SerializeField] private GameObject go_player;
    [SerializeField] private GameObject go_enemy;
    [SerializeField] private GameObject go_camera;
    [SerializeField] private GameObject ui_parent;
    [SerializeField] private GameObject ui_boss;
    [SerializeField] private GameObject ui_player;
    [SerializeField] private Transform upper;
    [SerializeField] private Transform lower;

    private bool isGuiding;
    private bool isSequenceEnd;
    private int currentSequence;

    // Scripts Component
    private BossManager enemy;
    private PlayerControl playerControl;
    private CameraMove cam;
    private QuestManager questManager;

    // Start is called before the first frame update
    void Start() {
        playerControl = go_player.GetComponent<PlayerControl>();
        enemy = this.GetComponent<BossManager>();
        cam = go_camera.GetComponent<CameraMove>();
        questManager = this.GetComponent<QuestManager>();

        isGuiding = false;
        isSequenceEnd = false;
        upper.position = new Vector3(0, 7, 0);
        lower.position = new Vector3(0, -7, 0);

        ///// start point   init = 0
        currentSequence = 0;
        /////
        

        Color transColor = new Color(1, 1, 1, 0);
        go_curtains[0].GetComponent<SpriteRenderer>().color = transColor;
        go_curtains[1].GetComponent<SpriteRenderer>().color = transColor;

        Debug.Log("Scene : IngameScene" + Time.time);

        StartCoroutine(ManageSequence());
    }


    // Update is called once per frame
    void Update()
    {
        /*
        /////////// TEST
        if (Input.GetKeyDown(KeyCode.R)) {
            //enemy.Reset(1000);
            playerControl.Reset();
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            playerControl.TakeDamage(1);
        }*/
    }

    public void SetPlayerControlDisabled() {
        playerControl.SetControlDisabled();
    }

    public void SetPlayerControlEnabled() {
        playerControl.SetControlEnabled();
    }
    
    public void SetUI(bool b) {
        ui_parent.SetActive(b);
        /*
        ui_boss.SetActive(b);
        ui_player.SetActive(b);
        */
    }

    public void SetGuide(bool b) {
        if (b && !isGuiding) {
            StartCoroutine(GuideOn(10f));
        }
        else if (!b && isGuiding) {
            StartCoroutine(GuideOff(10f));
        }
    }

    public void SetCurtain(int n, bool b, float t) {
        if(b)
            StartCoroutine(CoFadeIn(go_curtains[n], t));
        else
            StartCoroutine(CoFadeOut(go_curtains[n], t));
    }

    private void SetMap(int b, int t) {
        for (int i = 0; i < backgrounds.Length; i++) {
            if(backgrounds[i] != null)
                backgrounds[i].SetActive(false);
        }

        for (int i = 0; i < tiles.Length; i++) {
            if (tiles[i] != null)
                tiles[i].SetActive(false);
        }

        backgrounds[b].SetActive(true);
        tiles[t].SetActive(true);
    }

    IEnumerator ManageSequence() {

        while (true) {
            if (currentSequence == -9999) {
                SetUI(false);
                SetCurtain(0, false, 0f);
                SetCurtain(1, false, 0f);
                break;
            }

            StartCoroutine(PlaySequence(currentSequence));
            yield return new WaitUntil(() => isSequenceEnd);
            currentSequence++;
            isSequenceEnd = false;
        }
    }

    IEnumerator PlaySequence(int index) {

        SetPlayerControlDisabled();
        //화면비 2:1로
        if (index == 0) {
            Debug.Log("Sequence :: Tutorial");

            SetMap(0, 0);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(5, -2, 0));
            cam.SetIsSnapping(false);
            cam.SetLimit(-2, 10);

            StartCoroutine(CoFadeIn(go_curtains[0], 0f));

            /*
            cam.Travel(new Vector3(-10, 0, -10), new Vector3(5, 0, -10), 1.5f);

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(CoFadeOut(go_curtains[0], 3f));

            yield return new WaitUntil(() => !cam.GetIsTraveling());
            */

            questManager.RunQuest(index);
            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            yield return new WaitForSeconds(1f);

            isSequenceEnd = true;
        } else if (index == 1) {
            Debug.Log("Sequence :: Beginning");

            SetMap(2, 1);
            SetUI(false);
            SetGuide(true);
            cam.SetLimit(0, 0);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            playerControl.SetPlayerPosition(new Vector3(10, -2, 0));

            //...
            yield return null;
            //...

            playerControl.MovePlayerToX(8f);
            StartCoroutine(CoFadeOut(go_curtains[0], 1f));
            yield return new WaitForSeconds(0.5f);

            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            playerControl.MovePlayerToX(-10f);

            //yield return new WaitUntil(() => playerControl.GetIsMoveEnd());

            yield return new WaitForSeconds(1f);

            StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            yield return new WaitForSeconds(1.5f);
            isSequenceEnd = true;
            
        } else if (index == 2) {
            Debug.Log("Sequence :: Death");


            SetMap(2, 2);
            SetUI(false);
            SetGuide(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            playerControl.SetPlayerPosition(new Vector3(10, -2, 0));

            StartCoroutine(CoFadeOut(go_curtains[0], 1f));
            playerControl.MovePlayerToX(6f);
            yield return new WaitForSeconds(0.5f);

            //...
            yield return null;
            //...
            questManager.RunQuest(index);
            
            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;
        } else if (index == 3) {
            Debug.Log("Sequence :: Funeral");


            SetMap(3, 3);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-1, -2, 0));
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            playerControl.SetFaceRight(true);

            StartCoroutine(CoFadeOut(go_curtains[0], 1.5f));
            yield return new WaitForSeconds(1f);

            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;

        } else if (index == 4) {
            Debug.Log("Sequence :: Boss-Army");


            SetMap(4, 4);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-10, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(0, 3);

            //StartCoroutine(CoFadeOut(go_curtains[0], 1f));
            //yield return new WaitForSeconds(0.5f);

            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            //StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            //yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;

        } else if (index == 5) {
            Debug.Log("Sequence :: Ardill");

            SetMap(5, 5);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-11, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            //StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            //yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;

        } else if (index == 6) {
            Debug.Log("Sequence :: Swamp");

            SetMap(6, 6);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-11, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(-1, 1);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());
            
            isSequenceEnd = true;

        } else if (index == 7) {
            Debug.Log("Sequence :: Back to Ardill");

            SetMap(5, 5);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-11, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());
            
            isSequenceEnd = true;

        } else if (index == 8) {
            Debug.Log("Sequence :: To the BoolCity");

            SetMap(7, 7);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-11, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());
            
            isSequenceEnd = true;

        } else if (index == 9) {
            Debug.Log("Sequence :: BoolCity");

            SetMap(8, 8);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-2, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());
            
            isSequenceEnd = true;

        } else if (index == 10) {
            Debug.Log("Sequence :: To the cave");

            SetMap(7, 9);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-2, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());
            
            isSequenceEnd = true;

        } else if (index == 11) {
            Debug.Log("Sequence :: Cave");

            SetMap(9, 10);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-9, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(0, 3);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());
            
            isSequenceEnd = true;

        } else if (index == 12) {
            Debug.Log("Sequence :: Back to BoolCity");

            SetMap(8, 8);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-2, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(0);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            //StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            //yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;

        }
        ///******************************************************************************************************************
        else if (index == 13) { // 막판 // 중간에 추가하면 숫자 밀기
            Debug.Log("Sequence :: Front yard");


            SetMap(0, 0);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(0, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(8);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            //StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            //yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;

        } else if(index == 14) {   // 숫자 밀기
            Debug.Log("Sequence :: Knights");


            SetMap(2, 11);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(-15, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(-4);
            cam.SetLimit(-4, 2);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());

            //StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            //yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;

        } else if (index == 15) {   // 숫자 밀기
            Debug.Log("Sequence :: King");


            SetMap(2, 12);
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(4, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(4);
            cam.SetLimit(0, 6);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());



            StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;

        } else if (index == 16) {
            Debug.Log("Sequence :: Ending1");


            SetMap(2, 13);  // 맵 추가
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(3, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(6);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());



            StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;
        }else if(index == 17) {
            Debug.Log("Sequence :: Ending2");


            SetMap(2, 14);  // 맵 추가
            SetUI(false);
            SetGuide(true);
            playerControl.SetPlayerPosition(new Vector3(12.5f, -2, 0));
            playerControl.SetFaceRight(true);
            cam.SetIsSnapping(false);
            cam.SetPositionX(12.5f);
            cam.SetLimit(0, 0);


            questManager.RunQuest(index);

            yield return new WaitUntil(() => questManager.GetIsSequenceEnd());



            StartCoroutine(CoFadeIn(go_curtains[0], 1f));
            yield return new WaitForSeconds(1.5f);

            isSequenceEnd = true;
        }
        else {
            Debug.Log("Done");
            Application.Quit();
        }


        yield return null;
    }
    
    IEnumerator GuideOn(float v) {

        float yUpper = upper.localPosition.y;
        float yLower = lower.localPosition.y;

        //isGuiding = true;

        while (upper.localPosition.y - lower.localPosition.y > 10f) {
            yUpper -= Time.deltaTime * v;
            yLower += Time.deltaTime * v;
            upper.localPosition = new Vector3(0, yUpper, 0);
            lower.localPosition = new Vector3(0, yLower, 0);

            yield return null;
        }

        upper.localPosition = new Vector3(0, 5, 0);
        lower.localPosition = new Vector3(0, -5, 0);
        isGuiding = true;
    }

    IEnumerator GuideOff(float v) {

        float yUpper = upper.localPosition.y;
        float yLower = lower.localPosition.y;

        //isGuiding = true;

        while (upper.localPosition.y - lower.localPosition.y < 14f) {
            yUpper += Time.deltaTime * v;
            yLower -= Time.deltaTime * v;
            upper.localPosition = new Vector3(0, yUpper, 0);
            lower.localPosition = new Vector3(0, yLower, 0);

            yield return null;
        }

        upper.localPosition = new Vector3(0, 7, 0);
        lower.localPosition = new Vector3(0, -7, 0);
        isGuiding = false;
    }

    IEnumerator CoFadeIn(GameObject g, float t) {

        SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
        Color clr = sr.color;

        while (clr.a < 1f) {
            if (clr.a >= 1f || t == 0) {
                clr.a = 1f;
                break;
            }
            clr.a += Time.deltaTime / t;
            sr.color = clr;

            yield return null;
        }

        sr.color = clr;
    }

    IEnumerator CoFadeOut(GameObject g, float t) {

        SpriteRenderer sr = g.GetComponent<SpriteRenderer>();
        Color clr = sr.color;

        while (clr.a > 0f) {
            if (clr.a <= 0f || t == 0) {
                clr.a = 0f;
                break;
            }

            clr.a -= Time.deltaTime / t;
            sr.color = clr;

            yield return null;
        }

        sr.color = clr;
    }
}
