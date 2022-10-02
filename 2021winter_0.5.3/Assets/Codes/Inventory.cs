using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject card_blade;
    [SerializeField] private GameObject card_sword;
    [SerializeField] private GameObject card_wand;
    [SerializeField] private GameObject card_xbow;

    private const float LIMIT_BOARD_X = 5.5f;
    private const float LIMIT_BOARD_Y = 1.75f;
    private const float LIMIT_SLOT_X = 0.5f;
    private const float SNAP_X = 1f;
    private const float SNAP_Y = 1.75f;


    //[SerializeField] private GameObject[] list_cards;
    //private GameObject[] list_cardsOnBoard;
    private ArrayList arrlist_cards = new ArrayList();


    private static Vector3 position_pool = new Vector3(-9999, -9999, 0);
    private static Vector3 position_main = new Vector3(-5, 0.75f, 0);
    private static Vector3 position_sub = new Vector3(-2.25f, -1, 0);
    private static Vector3 position_mouse;
    private static Vector3 position_card_init;

    private static Transform tr_selectedCard;

    private static GameObject card_main;
    private static GameObject card_sub;

    private static bool isDrag;

    void Start()
    {
        Initialize();

        test();
    }
    
    void Update()
    {
        MouseButton();
    }

    private void Initialize() {
        isDrag = false;

        card_main = null;
        card_sub = null;
        card_blade.SetActive(false);
        card_sword.SetActive(false);
        card_wand.SetActive(false);
        card_xbow.SetActive(false);
    }

    int Snap() {
        // on slot
        //Debug.Log("Up " + Mathf.Abs(selectedCard.position.x - position_main.x) + ", " + Mathf.Abs(selectedCard.position.y - position_main.y));
        if (Mathf.Abs(tr_selectedCard.position.x - position_main.x) <= SNAP_X &&
           Mathf.Abs(tr_selectedCard.position.y - position_main.y) <= SNAP_Y) {
            tr_selectedCard.position = position_main;

            if(card_main != null) {
                card_main.transform.position = position_card_init;
            }
            card_main = tr_selectedCard.gameObject;
            return 0;
        } else if (Mathf.Abs(tr_selectedCard.position.x - position_sub.x) <= SNAP_X &&
            Mathf.Abs(tr_selectedCard.position.y - position_sub.y) <= SNAP_Y) {
            tr_selectedCard.position = position_sub;

            if (card_sub != null) {
                card_sub.transform.position = position_card_init;
            }
            card_sub = tr_selectedCard.gameObject;
            return 0;
        }

        // cover slot
        if (tr_selectedCard.position.x < LIMIT_SLOT_X) {
            tr_selectedCard.position = position_card_init;
            return 1;
        }

        // out of board 
        if (tr_selectedCard.position.x > LIMIT_BOARD_X)
            tr_selectedCard.position = new Vector3(LIMIT_BOARD_X, tr_selectedCard.position.y, tr_selectedCard.position.z);
        else if (tr_selectedCard.position.x < -LIMIT_BOARD_X)
            tr_selectedCard.position = new Vector3(-LIMIT_BOARD_X, tr_selectedCard.position.y, tr_selectedCard.position.z);

        if (tr_selectedCard.position.y > LIMIT_BOARD_Y)
            tr_selectedCard.position = new Vector3(tr_selectedCard.position.x, LIMIT_BOARD_Y, tr_selectedCard.position.z);
        else if (tr_selectedCard.position.y < -LIMIT_BOARD_Y)  // need it?
            tr_selectedCard.position = new Vector3(tr_selectedCard.position.x, -LIMIT_BOARD_Y, tr_selectedCard.position.z);

        return 2;
    }

    void MouseButton() {
        if (Input.GetMouseButtonDown(0)) {
            position_mouse = Input.mousePosition;
            Debug.Log("click ");
            RaycastHit2D ray = Physics2D.Raycast(ScreenPosConvert(position_mouse, -10), Vector3.forward, 10);
            if (ray.transform != null)
                if (ray.transform.tag.Equals("Card")) {
                    tr_selectedCard = ray.transform;
                    //Debug.Log("onClick " + selectedCard.name);
                    isDrag = true;
                    SetOrder();
                    position_card_init = tr_selectedCard.position;
                    tr_selectedCard.position = ScreenPosConvert(position_mouse, tr_selectedCard.position.z);
                }
                else if (ray.transform.tag.Equals("X")) {
                    Close();
                }
        }
        if (Input.GetMouseButton(0)) {
            if (isDrag) {
                position_mouse = Input.mousePosition;
                tr_selectedCard.position = ScreenPosConvert(position_mouse, tr_selectedCard.position.z);
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            if (isDrag) {
                position_mouse = Input.mousePosition;
                isDrag = false;
                //Debug.Log(ScreenPosConvert(position_mouse, 0) + "/" + selectedCard.position);
                int rt = Snap();
                

                tr_selectedCard = null;
            }
        }
    }

    Vector3 ScreenPosConvert(Vector3 prev, float z) {
        return new Vector3(prev.x/108 - 8.889f, prev.y/108 - 5, z);
    }

    void AddCard(GameObject g) {
        arrlist_cards.Insert(0, g);
        
        g.SetActive(true);

        RenewOrder();
    }

    /*
    void DeleteCard() {

    }
    */

    void SetOrder() {
        Debug.Log("SetOrder_1");
        for(int i = 0; i < arrlist_cards.Count; i++) {
            Debug.Log("SetOrder_2");
            if ((GameObject)arrlist_cards[i] == tr_selectedCard.gameObject) {
                Debug.Log("SetOrder_3");
                if (i != 0) {
                    Debug.Log("SetOrder_4");
                    arrlist_cards.RemoveAt(i);
                    arrlist_cards.Insert(0, tr_selectedCard.gameObject);

                    RenewOrder();
                }
                else
                    Debug.Log("SetOrder_iii " + i);
                break;
            }
        }
    }

    void RenewOrder() {
        Debug.Log("renew");
        for (int i = 0; i < arrlist_cards.Count; i++) {
            ((GameObject)arrlist_cards[i]).GetComponent<Card>().SetPosition(i);
        }
    }

    void Close() {
        this.gameObject.SetActive(false);
    }

    void test() {
        AddCard(card_blade);
        AddCard(card_sword);
        AddCard(card_wand);
        AddCard(card_xbow);
    }
}
