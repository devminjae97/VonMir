using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    [SerializeField] private SpriteRenderer spriteRenderer_new;
    [SerializeField] private SpriteRenderer spriteRenderer_load;
    [SerializeField] private SpriteRenderer spriteRenderer_menu;
    [SerializeField] private SpriteRenderer spriteRenderer_quit;
    [SerializeField] private Sprite sprite_new_up;
    [SerializeField] private Sprite sprite_new_down;
    [SerializeField] private Sprite sprite_load_up;
    [SerializeField] private Sprite sprite_load_down;
    [SerializeField] private Sprite sprite_load_ban;
    [SerializeField] private Sprite sprite_menu_up;
    [SerializeField] private Sprite sprite_menu_down;
    [SerializeField] private Sprite sprite_menu_ban;
    [SerializeField] private Sprite sprite_quit_up;
    [SerializeField] private Sprite sprite_quit_down;

    private Transform tr_in;
    private Transform tr_out;

    // 0(null), 1, 2, 3, 4
    private int num_in;
    private int num_out;

    //private bool isNewEnable;
    private bool isLoadEnable;
    private bool isMenuEnable;
    //private bool isQuitEnable;

    void Start() {
        Initialize();
    }

    void Update() {
        GetMouse();
    }

    void Initialize() {
        num_in = 0;
        num_out = 0;

        isLoadEnable = false;
        isMenuEnable = false;

        RenewButtons();
    }

    void RenewButtons() {
        if (isLoadEnable) {
            spriteRenderer_load.sprite = sprite_load_up;
        } else {
            spriteRenderer_load.sprite = sprite_load_ban;
        }
        
        if (isMenuEnable) {
            spriteRenderer_menu.sprite = sprite_menu_up;
        }
        else {
            spriteRenderer_menu.sprite = sprite_menu_ban;
        }
        spriteRenderer_new.sprite = sprite_new_up;
        spriteRenderer_quit.sprite = sprite_quit_up;
    }

    void GetMouse() {


        if (Input.GetMouseButtonDown(0)) {
            Vector3 pos = Input.mousePosition;
            RaycastHit2D ray = Physics2D.Raycast(ScreenPosConvert(pos, -10), Vector3.forward, 10);

            if (ray.transform != null) {
                if (ray.transform.name.Equals("btn_new")) {
                    num_in = 1;
                    spriteRenderer_new.sprite = sprite_new_down;
                } else if (ray.transform.name.Equals("btn_load")) {
                    if (isLoadEnable) {
                        num_in = 2;
                        spriteRenderer_load.sprite = sprite_load_down;
                    }
                } else if (ray.transform.name.Equals("btn_menu")) {
                    if (isMenuEnable) {
                        num_in = 3;
                        spriteRenderer_menu.sprite = sprite_menu_down;
                    }
                } else if (ray.transform.name.Equals("btn_quit")) {
                    num_in = 4;
                    spriteRenderer_quit.sprite = sprite_quit_down;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            Vector3 pos = Input.mousePosition;
            RaycastHit2D ray = Physics2D.Raycast(ScreenPosConvert(pos, -10), Vector3.forward, 10);

            if (ray.transform != null) {
                if (ray.transform.name.Equals("btn_new")) {
                    if(num_in == 1) {
                        // scene
                        Debug.Log("new game");
                        SceneManager.LoadScene("IngameScene");
                    }
                }else if (ray.transform.name.Equals("btn_load")) {
                    if (num_in == 2 && isLoadEnable) {
                        // scene
                        Debug.Log("load game");
                    }
                }
                else if (ray.transform.name.Equals("btn_menu")) {
                    if (num_in == 3 && isMenuEnable) {
                        // scene
                        Debug.Log("menu");
                    }
                }
                else if (ray.transform.name.Equals("btn_quit")) {
                    if (num_in == 4) {
                        // scene
                        Debug.Log("quit");
                    }
                }
            }

            Initialize();
        }
    }

    Vector3 ScreenPosConvert(Vector3 prev, float z) {
        return new Vector3(prev.x / 108 - 8.889f, prev.y / 108 - 5, z);
    }

    void NewGame() {
        Debug.Log("new game");
    }

    void LoadGame() {
        Debug.Log("new game");
    }

    void Menu() {
        Debug.Log("new game");
    }

    void Quit() {
        Debug.Log("new game");
    }
}
