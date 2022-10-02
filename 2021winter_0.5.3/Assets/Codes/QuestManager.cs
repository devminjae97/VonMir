using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    private const string CODE_ERROR = "-11111";
    private const string CODE_NULL = "-99999";

    // character id : 0mmii => m : map, i: index
    private const int ID_CHARACTER_KASTONIA_PLAYER = 00001;
    private const int ID_CHARACTER_KASTONIA_KING = 00002;
    private const int ID_CHARACTER_KASTONIA_WILSON = 00003;     // 조력자
    private const int ID_CHARACTER_ARDILL_MAN = 00101;
    private const int ID_CHARACTER_ARDILL_WOMAN = 00102;
    private const int ID_CHARACTER_BOOLCITY_MAN = 00201;
    private const int ID_CHARACTER_BOOLCITY_WOMAN = 00202;
    private const int ID_CHARACTER_REVCASTLE_KING = 00301;
    private const int ID_CHARACTER_REVCASTLE_SON = 00302;

    // boss id : 1mmii => m : map, i: index
    private const int ID_BOSS_KASTONIA_DUMMY = 10001;
    private const int ID_BOSS_ARDILL_ARMY = 10101;
    private const int ID_BOSS_ARDILL_SWAMP = 10102;
    private const int ID_BOSS_BOOLCITY_WISTONNE = 10201;
    private const int ID_BOSS_REVCASTLE_KING = 10301;

    // 잡몹 id : 2mmii => m : map, i: index
    private const int ID_ENEMY_CASTLE_KNIGHT = 20401;

    // special id : 9xxxx
    private const int ID_CAMERA = 90001;
    private const int ID_SCENE = 90002;
    private const int ID_CURTAIN = 90003;

    [SerializeField] private GameObject[] scenes;
    [SerializeField] private GameObject cWindow;
    [SerializeField] private Text txt;
    //[SerializeField] private Text txtName;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private CameraMove cam;
    //[SerializeField] private CameraMove cam;
    
    private static int questIndex;

    private GameManager gm;
    private BossManager bm;
    private EnemyManager em;
    private int questEnemyID;
    private int questEnemyNum;
    private bool isEnable;
    private bool isActionQuestOn;
    private bool isSequenceEnd;


    //private int questActionIndex = 0;


    private ArrayList arrlist_quest = new ArrayList();
    
    void Awake()
    {
        gm = this.GetComponent<GameManager>();
        bm = this.GetComponent<BossManager>();
        em = this.GetComponent<EnemyManager>();

        questIndex = 0;

        isActionQuestOn = false;

        SetCWindow(false);

        GenerateData();
    }

    void Update() {

        ////////////////////////////// TEST
        if (Input.GetKeyDown(KeyCode.K) && isActionQuestOn) {
            bm.TakeDamage(999999);
        }

        if (Input.GetKeyDown(KeyCode.A) && isEnable) {
            /*
            questIndex++;
            txt.text = "index : " + questIndex.ToString();
            */
            if(!isActionQuestOn)
                SetScript();
        }

        if (isActionQuestOn && questEnemyNum == 0) {
            isActionQuestOn = false; 
            gm.SetGuide(true);
            RunQuest(questIndex);
            playerControl.StopMove();
        }


    }

    void GenerateData()
    {
        //dxnry_quest.Add(10, new QuestData("남자 퀘스트 첫 번째", new int[] { 1000, 2000, 1000 }));   

        //Dictionary<int, string> dxnry_tmp = new Dictionary<int, string>();
        
        QuestData qd0 = new QuestData("튜토리얼", 00);
        QuestData qd1 = new QuestData("발단", 01);
        QuestData qd2 = new QuestData("죽음", 02);
        QuestData qd3 = new QuestData("장례식", 03);
        QuestData qd4 = new QuestData("아르미", 04);
        QuestData qd5 = new QuestData("아딜마을", 05);
        QuestData qd6 = new QuestData("스웜프", 06);
        QuestData qd7 = new QuestData("아딜마을", 07);
        QuestData qd8 = new QuestData("불마을 가는 길", 08);
        QuestData qd9 = new QuestData("불마을", 09);
        QuestData qd10 = new QuestData("불거북",10);
        QuestData qd11 = new QuestData("불마을", 11);
        QuestData qd12 = new QuestData("왕국앞마당", 12);
        QuestData qd13 = new QuestData("왕국복도", 13);
        QuestData qd14 = new QuestData("왕국", 14);
        QuestData qd15 = new QuestData("처치", 15);
        QuestData qd16 = new QuestData("엔딩1", 16);
        QuestData qd17 = new QuestData("엔딩2", 17);

        //qd1.SetQuestDictionary(dxnry_tmp);
        qd0.AddQuestScene(ID_SCENE, 0);
        qd0.AddQuestWait(1f);
        qd0.AddQuestAction(ID_CAMERA, -10f, false);
        qd0.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 5f, false);
        qd0.AddQuestAction(ID_CAMERA, 5f, 2f, true);
        qd0.AddQuestWait(0.5f);
        qd0.AddQuestScene(ID_CURTAIN, 0, 3f, false);
        qd0.AddQuestWait(8f);
        qd0.AddQuestEnemy(ID_BOSS_KASTONIA_DUMMY, true);
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "왕자님, 오늘의 수업은 검술입니다.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "A키를 이용해 기본공격 할 수 있습니다.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "앞에 있는 따끈따끈한 허수아비를 공격해보세요.");
        qd0.AddQuestBranch();
        qd0.AddQuestEnemy(ID_BOSS_KASTONIA_DUMMY, 1, "허수아비를 잡아보자");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "어렵지 않지요?");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "키보드 2를 누르면 완드로 바꾸어 사용할 수 있습니다.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "오, 진짜?");
        qd0.AddQuestEnemy(ID_BOSS_KASTONIA_DUMMY, true);
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "완드로 다시 한번 허수아비를 잡아보시죠.");
        qd0.AddQuestBranch();
        qd0.AddQuestEnemy(ID_BOSS_KASTONIA_DUMMY, 1, "허수아비를 잡아보자.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "나쁘지 않네.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "방금 완드로 잡으신거 맞죠?");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "어 왜?");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "이 게임 시스템 상 어떤 무기로 잡았는지 제가 알 수 없어서요. (귀찮아서)");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그게 무슨 말이야?");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "아무것도 아닙니다.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "눈치 채셨겠지만, 키보드 1을 누르면 다시 검을 사용하실 수 있어요.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "오키오키.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "다음은 강화된 스킬공격이지만, 개발이 더뎌져서 이 버전에서 수업은 여기까지만 하도록 하겠습니다. 참고로 대시는 D입니다.");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "?????");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "이제 아버지께 오늘 배운것을 보고드리러 가시지요");
        qd0.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그래 그래.");
        qd0.AddQuestBranch();

        qd1.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "이게 머선129?");
        qd1.AddQuestBranch();

        qd2.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, "!", 1f);
        qd2.AddQuestScene(ID_CURTAIN, 0, 0f, true);
        qd2.AddQuestScene(ID_SCENE, 1);
        qd2.AddQuestScene(ID_SCENE, 2);
        qd2.AddQuestScene(ID_CURTAIN, 0, 0f, false);
        qd2.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, -3f, false);
        qd2.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, "lie", true); // lie down
        qd2.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "아버지!");
        qd2.AddQuestScript(ID_CHARACTER_KASTONIA_KING, "ㄷ...쳐...");
        qd2.AddQuestScript(ID_CHARACTER_KASTONIA_KING, "도망쳐라 아들아...");
        qd2.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, "lie", false); // lie up
        qd2.AddQuestBranch();

        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "...");
        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "복수할거야.");
        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "어떡하시게요. 우리는 범인이 누군지도 모르고, 식별할 만한 흔적이 하나도 없었습니다.");
        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "하나 있어.");
        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "바닥과 기둥에 불로 그을린 자국이 있었어. 불속성을 다룰수 있는 매우 훈련된 자였겠지. 그런 자가 흔치는 않을거야.");
        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "...좋습니다.");
        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "그런데, 만약 찾더라도 어떻게 복수하시게요? 아직 왕자님은 힘도 능력도 부족하십니다.");
        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그래서 자네를 데려가겠단거 아니겠는가.");
        qd3.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "예?");
        qd3.AddQuestBranch();

        qd4.AddQuestScene(ID_SCENE, 3);
        qd4.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd4.AddQuestEnemy(ID_BOSS_ARDILL_ARMY, true);
        qd4.AddQuestWait(0.5f);
        qd4.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, -2f, true);
        qd4.AddQuestWait(1f);
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "저게 뭐지?");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "아르마딜로 같아 보이는데요.");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "여기에 서서 이 길을 지나다니는 사람들을 괴롭히는 모양입니다.");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "우리도 이길을 쭉 따라 가야하지?");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "네. 이 참에 수련도 할 겸 잡고 지나가실까요?");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "쟤도 누군가의 부모님이고 자식이고 일텐데...");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "?");
        qd4.AddQuestBranch();
        qd4.AddQuestEnemy(ID_BOSS_ARDILL_ARMY, 1, "아르마딜로를 잡아보자");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "잘 잡으셨네요.");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "근데 넌 왜 구경만 하는 거야?");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "수련이잖아요~.");
        qd4.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "가던 길 계속 가시지요.");
        qd4.AddQuestBranch();
        qd4.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 14f, true);
        qd4.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd4.AddQuestWait(1.5f);

        
        qd5.AddQuestScene(ID_SCENE, 4);
        qd5.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd5.AddQuestWait(0.5f);
        qd5.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, -2f, true);
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "말 좀 묻겠습니다.");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_MAN, "...");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "혹시 이 근처에 불을 다루는 자나 마물이 있습니까?");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_MAN, "...");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "그 자한텐 말을 기대하지 않는 것이 좋을거야.");
        qd5.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 2f, true);
        qd5.AddQuestWait(0.5f);
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "무슨 사연이라도 있습니까?");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "묵언수행중이래.");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "아, 예...");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "불을 잘 다루는 자는 내가 잘 알고 있지.");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "제게 좀 알려주시겠습니까?");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "일단 이 근처에는 없고, 다른 마을로 넘어가야하는데...");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그 마을이 어디입니까?");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "음...");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "그 정보를 알려주면 나는 얻는게 뭐지?");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "사례말입니까? 돈이라면 얼마든지 드릴 수 있습니다.");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "돈은 말고.");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "이 마을 끝자락에 가보면 이 숲의 정기를 빨아먹는 기분 나쁜 놈이 하나있어.");
        qd5.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "그 놈좀 처리해줘. 그러면 알려주도록 하지.");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "좋습니다. 지금 당장 다녀오도록 하죠.");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "바로 다녀오시는 겁니까.");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "응. 금방 잡을 수 있겠지..?");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "제가 예전에 들은 소문에는 이곳의 마물은 몸이 약하다고 하니 충분하실겁니다.");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "몸 조심히 다녀오십시오.");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "너는 안가?");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "몸 조심히 다녀오십시오.");
        qd5.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "???");
        qd5.AddQuestBranch();
        qd5.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 2f, true);
        qd5.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd5.AddQuestWait(1.5f);
        
        qd6.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd6.AddQuestEnemy(ID_BOSS_ARDILL_SWAMP, true);
        qd6.AddQuestWait(0.5f);
        qd6.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 0f, true);
        qd6.AddQuestWait(1f);
        qd6.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "어서 잡고 돌아가자.");
        qd6.AddQuestBranch();
        qd6.AddQuestEnemy(ID_BOSS_ARDILL_SWAMP, 1, "스웜프를 잡아보자");
        qd6.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "검술이 점점 익숙해져가는게 나쁘진않네...");
        qd6.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "돌아가야겠다.");
        qd6.AddQuestBranch();
        qd6.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, -14f, true);
        qd6.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd6.AddQuestWait(1.5f);

        
        qd7.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 2f, false);
        qd7.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd7.AddQuestWait(1f);
        qd7.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "처치했습니다.");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "고생했네.");
        qd7.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그럼 이제 그 정보를...");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "그래 그래.");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "이 마을 오른쪽으로 2마일 정도 가면 블도시라고 있어.");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "그 근처 산 안에 동굴이 있는데, 그 안에서 지내는 모양이야.");
        qd7.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "아, 감사합니다.");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "근데 걔를 찾는 이유가 뭔가?");
        qd7.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "... 제가 되갚아줘야 할 것이 있습니다.");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "복수같은건가? 그런건 좋지 않아.");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "무슨 일이 있었는지는 모르지만, 그 어떤 일이였든지 그게 운명이였다 생각하고 지나가는게...");
        qd7.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그 녀석이 저한테 당해준다면 그것 또한 그의 운명이겠죠.");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "그래... 몸 조심하게.");
        qd7.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "네.");
        qd7.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 14f, true);
        qd7.AddQuestWait(2f);
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "...");
        qd7.AddQuestScript(ID_CHARACTER_ARDILL_WOMAN, "그 겁쟁이 녀석이 무슨 짓을 했다는 거지...");
        qd7.AddQuestBranch();
        qd7.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd7.AddQuestWait(1.5f);

        qd8.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd8.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 0, true);
        qd8.AddQuestWait(1.5f);
        qd8.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "날씨가 참 좋네.");
        qd8.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "그러게 말입니다.");
        qd8.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "만약, 우리가 찾아가는 이 녀석이 범인이 맞겠지?");
        qd8.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "그러길 바라야죠.");
        qd8.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "후련하게지?");
        qd8.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "후련할거야.");
        qd8.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "어서 가시죠.");
        qd8.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그러자.");
        qd8.AddQuestBranch();
        qd8.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 14, true);
        qd8.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd8.AddQuestWait(1.5f);
        
        qd9.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd9.AddQuestWait(1.5f);
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "안녕하세요.");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_MAN, "점심 식사하시려고요?");
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "아뇨, 저희는...");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_MAN, "아, 모텔 방이 필요해요?");
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "이 시대에 모텔이 있어요?");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_MAN, "그러게요?");
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "???");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_MAN, "???");
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "저희는 불을 잘 다루는 자나 마물을 찾으러 다니는데요, 아딜(마을)에서 이곳으로 가보라 해서요.");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "아딜에서 오셨다고요?");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "혹시 제 남동생을 보셨나요? 노란 꽁지머리를 하고 수염을 기르는 사람인데...");
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "아, 묵언수행중이던?");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "아... 아직도 그런... ");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "저희 남매는 원래 같이 살았었는데, 이렇게 찢어지게 된 일엔 깊은 사연이 있었어요...");
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "안 궁금합니다.");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "예.");
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "불을 잘 다루는 자가 있는 동굴이 어딘가요?");
        qd9.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "저쪽 산 중턱에 있어요.");
        qd9.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "감사합니다.");
        qd9.AddQuestBranch();
        qd9.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 14, true);
        qd9.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd9.AddQuestWait(1.5f);

        qd10.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd10.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, -2, true);
        qd10.AddQuestWait(1.5f);
        qd10.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "공모전 기간이 끝나가니까 뭘 생각해서 쓰기에 너무 힘들고 귀찮다.");
        qd10.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "ㄹㅇㅋㅋ. 그냥 빨리 잡고 끝내죠.");
        qd10.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "ㄱㄱ");
        qd10.AddQuestBranch();
        qd10.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 14, true);
        qd10.AddQuestWait(1f);
        qd10.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd10.AddQuestWait(1.5f);

        qd11.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd11.AddQuestEnemy(ID_BOSS_BOOLCITY_WISTONNE, true);
        qd11.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, -5, true);
        qd11.AddQuestWait(1.5f);
        qd11.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "엥? 내가 생각했던 비주얼이 아닌데?");
        qd11.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "일단 잡고 보자.");
        qd11.AddQuestBranch();
        qd11.AddQuestEnemy(ID_BOSS_BOOLCITY_WISTONNE, 1, "위스턴을 잡아보자.");
        qd11.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "아, 도망가 버렸네...");
        qd11.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "일단 돌아가자.");
        qd11.AddQuestBranch();
        qd11.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd11.AddQuestWait(1.5f);
        
        qd12.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd12.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, -5, false);
        qd12.AddQuestWait(1.5f);
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "그래서, 못 잡으신건가요..?");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "응. 근데 내 느낌에 걔는 아닌거 같아.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그러기엔 너무 소심하고 귀여웠어.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "걔가요...?");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "봤어?");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "그럼 다른 불을 다루는 자가 있는지 찾아봐야 할텐데말이죠...");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "사실 불을 다루는 것도 잘못 짚은거 아닐까..?");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "아버지랑 싸운 도중에 촛불같은게 떨어져서 그을린거라던가...");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "어찌 되었든 상당한 실력자인건 사실일겁니다. 폐하를 힘으로 제압하신 분이라면...");
        qd12.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "저런... 당신네 가족에도 슬픈 일이 있었군요... 저희 남매도 원래 같이 살았었는데...");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "안 궁금합니다.");
        qd12.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "불을 잘 다루는지는 모르지만, 무예가 뛰어난 자에 대해 들은적이 있습니다.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "누구죠 그게?");
        qd12.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "이 길을 따라 한 시간정도 가다보면 한 왕이 있습니다.");
        qd12.AddQuestScript(ID_CHARACTER_BOOLCITY_WOMAN, "그 곳의 왕이 무예에 재능이 있다고 소문이 나있죠.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "그자일 수도...");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "왜 다른 성의 왕이..? 직접 그럴 이유가...");
        qd12.AddQuestScript(ID_CHARACTER_BOOLCITY_MAN, "없어졌어요.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "네?");
        qd12.AddQuestScript(ID_CHARACTER_BOOLCITY_MAN, "그 성의 왕이 사라졌다나? 밤사이에. 왕가 사람도 백성들도 모두 어디로 갔는지 모르는 모양인가 보오.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그렇군요...");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "그 없어진 때가 언제입니까?");
        qd12.AddQuestScript(ID_CHARACTER_BOOLCITY_MAN, "이틀전인가? 그쯤이였지 아마.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "!");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "폐하의 서거 전날 밤...");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "일단 그 성에 가서 확인 해보는것이 어떻습니까?");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "사라졌대잖아. 아무도 어디로 갔는지 모른다고.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "혹시 압니까? 폐하을 죽이고 숨어 있으려고 그런 소문을 꾸민것인지.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "아... 이게 맞는건가.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "확실합니다. 가서 왕이 있으면 확신이 맞지 않겠습니까?");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "좋아... 일단 가서 숨어있는지 진짜 없는지 보면 알게되겠지.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "있으면, 시원하게 복수하고 오시지요.");
        qd12.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그러자.");
        qd12.AddQuestBranch();
        qd12.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 14, true);
        qd12.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd12.AddQuestWait(1.5f);

        qd13.AddQuestScene(ID_SCENE, 6);
        qd13.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd13.AddQuestWait(2f);
        qd13.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "곧 성입니다.");
        qd13.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "있을까..?");
        qd13.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "긴장되십니까?");
        qd13.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "약간... 만약 숨어있는거면 확실한거겠지?");
        qd13.AddQuestScript(ID_CHARACTER_KASTONIA_WILSON, "예. 무술에 조예가 깊은자가 폐하 서거 전날 밤에 사라졌다? 확실합니다.");
        qd13.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "그래. 자, 드가자.");
        qd13.AddQuestBranch();
        qd13.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 14f, true);
        qd13.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd13.AddQuestWait(1.5f);

        qd14.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd14.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, -12f, true);
        qd14.AddQuestWait(1f);
        qd14.AddQuestEnemy(ID_ENEMY_CASTLE_KNIGHT, true);
        qd14.AddQuestBranch();
        qd14.AddQuestEnemy(ID_ENEMY_CASTLE_KNIGHT, 6, "기사를 모두 처치하자.");
        qd14.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "후...");
        qd14.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "빈 왕의 자리를 이렇게 지킬리는 없지.");
        qd14.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "딱대라.");
        qd14.AddQuestBranch();
        qd14.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 14f, true);       //위치 조정
        qd14.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd14.AddQuestWait(1.5f);

        qd15.AddQuestWait(1f);
        qd15.AddQuestScene(ID_CURTAIN, 0, 2f, false);
        qd15.AddQuestEnemy(ID_BOSS_REVCASTLE_KING, true);
        qd15.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 4f, false);      //위치 조정
        qd15.AddQuestWait(1.5f);
        qd15.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "드디어 조우하게 됐네.");
        qd15.AddQuestScript(ID_CHARACTER_REVCASTLE_KING, "...");
        qd15.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "내가 누군진 알지?");
        qd15.AddQuestScript(ID_CHARACTER_REVCASTLE_KING, "...");
        qd15.AddQuestScript(ID_CHARACTER_KASTONIA_PLAYER, "놀라지도 않는거 보면 확실하네.");
        qd15.AddQuestBranch();
        qd15.AddQuestEnemy(ID_BOSS_REVCASTLE_KING, 1, "마지막 적을 처치하자.");
        qd15.AddQuestBranch();
        qd15.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd15.AddQuestWait(1.5f);

        qd16.AddQuestWait(1f);
        qd16.AddQuestScene(ID_CURTAIN, 0, 2f, false);
        qd16.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 6f, true);
        qd16.AddQuestWait(1f);
        qd16.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, "!", 2f);

        qd16.AddQuestScript(ID_CHARACTER_REVCASTLE_SON, "이게 머선129?");
        qd16.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 6f, true);
        qd16.AddQuestWait(1f);
        qd16.AddQuestScript(ID_CHARACTER_REVCASTLE_SON, "아버지!");
        qd16.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 12f, true);
        qd16.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd16.AddQuestWait(1f);


        qd17.AddQuestScene(ID_SCENE, 7);
        qd17.AddQuestScene(ID_SCENE, 8);
        qd17.AddQuestScene(ID_CURTAIN, 0, 1f, false);
        qd17.AddQuestScript(ID_CHARACTER_KASTONIA_KING, "ㄷ...쳐...");
        qd17.AddQuestScript(ID_CHARACTER_KASTONIA_KING, "도망쳐라 아들아...");
        qd17.AddQuestBranch();
        qd17.AddQuestAction(ID_CHARACTER_KASTONIA_PLAYER, 20f, true);
        qd17.AddQuestWait(0.5f);
        qd17.AddQuestScene(ID_CURTAIN, 0, 1f, true);
        qd17.AddQuestWait(1.5f);
        qd17.AddQuestScene(ID_SCENE, 9);
        qd17.AddQuestWait(5f);




        arrlist_quest.Add(qd0);
        arrlist_quest.Add(qd1);
        arrlist_quest.Add(qd2);
        arrlist_quest.Add(qd3);
        arrlist_quest.Add(qd4);
        arrlist_quest.Add(qd5);
        arrlist_quest.Add(qd6);
        arrlist_quest.Add(qd7);
        arrlist_quest.Add(qd8);
        arrlist_quest.Add(qd9);
        arrlist_quest.Add(qd10);
        arrlist_quest.Add(qd11);
        arrlist_quest.Add(qd12);
        arrlist_quest.Add(qd13);
        arrlist_quest.Add(qd14);
        arrlist_quest.Add(qd15);
        arrlist_quest.Add(qd16);
        arrlist_quest.Add(qd17);

    }
    /*
    public void SetEnabled() {
        isEnable = true;
    }
    */

    void SetSceneGuide(bool b) {

    }


    void SetCWindow(bool b) {
        

        cWindow.gameObject.SetActive(b);
        txt.gameObject.SetActive(b);
        isEnable = b;
    }
    
    public void RunQuest(int index) {
        questIndex = index;

        gm.SetPlayerControlDisabled();

        // ui
        gm.SetUI(false);

        /// tutorial
        //StartCoroutine(ShowCWindow(1f));
        StartCoroutine(CoSetScript(1f));
    }

    int SetScript() {
        QuestScript script = ((QuestData)arrlist_quest[questIndex]).GetQuestScript();
        

        if (script is null) {
            SetCWindow(false);

            Debug.Log("SetScript >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 2-1 : Quest Finished.");

            isSequenceEnd = true;
            playerControl.StopMove();
            /*
            gm.SetGuide(false);
            StartCoroutine(ResumePlayer(.5f));
            */

            return 1;
        }
        else {
            /*
            // action script
            if (script.GetQuestType()) {
                SetCWindow(false);
                questEnemy = script.GetID();
                questEnemyNum = script.GetNum();
                isActionQuestOn = true;

                // if boss일 경우
                bm.SetBoss(questEnemy);

                gm.SetGuide(false);
                StartCoroutine(ResumePlayer(.5f));
            }
            // not action script
            else {
                txt.text = script.GetContent();
            }
            */

            switch (script.GetScriptType()) {
                case 0: // conv
                    txt.text = "[" + ConvertIDtoName(script.GetID()) + "] " + script.GetContent();
                    if (!isEnable)
                        SetCWindow(true);
                    break;
                case 1: // enemy
                    questEnemyID = script.GetID();
                    int type = questEnemyID / 10000;

                    if (script.GetBool()) {
                        SetCWindow(false);
                        questEnemyNum = script.GetNum();
                        isActionQuestOn = true;

                        if (type == 1)
                            bm.SetAlive();
                        //else if (type == 2)
                        //em.SetEnemy(ID_ENEMY_CASTLE_KNIGHT, 6);

                        playerControl.StopMove();

                        gm.SetGuide(false);
                        StartCoroutine(CoResumePlayer(.5f));

                    } else {
                        if (type == 1)
                            bm.SetBoss(questEnemyID);
                        else if (type == 2)
                            em.SetEnemy(ID_ENEMY_CASTLE_KNIGHT, 6);

                        SetScript();
                    }



                    break;
                case 2: // move
                    SetCWindow(false);
                    if (script.GetID() == ID_CHARACTER_KASTONIA_PLAYER) {    // player
                        if (script.GetBool()) {
                            playerControl.MovePlayerToX(script.GetFloat());
                        } else
                            playerControl.SetPlayerPosition(script.GetFloat());
                    }
                    else if (script.GetID() == ID_CAMERA){
                        if (script.GetBool()) {
                            cam.Travel(script.GetFloat(), script.GetSpeed());
                        } else
                            cam.SetPositionX(script.GetFloat());
                    }
                    SetScript();
                    break;
                case 3: // action
                    if (script.GetContent().Equals("lie")) {
                        if (script.GetID() == ID_CHARACTER_KASTONIA_PLAYER) {    // player
                            playerControl.SetPlayerLie(script.GetBool());
                            SetScript();
                        }
                    } else if (script.GetContent().Equals("!")) {
                        if (script.GetID() == ID_CHARACTER_KASTONIA_PLAYER) {    // player
                            StartCoroutine(CoSetScreamer(script.GetFloat()));
                        }
                    }
                    break;
                case 4: // scene
                    if (script.GetID() == ID_SCENE)
                        StartCoroutine(CoSetScenePic(script.GetSceneNumber()));
                    else if(script.GetID() == ID_CURTAIN) {
                        gm.SetCurtain(script.GetSceneNumber(), script.GetBool(), script.GetFloat());
                        //Debug.Log(">>>>>>>>> curtain out!");
                        SetScript();
                    }
                    break;
                case 5: // wait
                    //Debug.Log("!! wait !! " + script.GetFloat());
                    StartCoroutine(CoWait(script.GetFloat()));
                    break;
                case -1:
                    SetCWindow(false);
                    SetScript();
                    break;
            }

            return 0;
        }
    }
    /*
    IEnumerator ShowCWindow(float t) {
        yield return new WaitForSeconds(t);

        SetCWindow(true);
        SetScript();
    }*/

    IEnumerator CoSetScript(float t) {
        yield return new WaitForSeconds(t);
        
        SetScript();
    }

    IEnumerator CoResumePlayer(float t) {
        yield return new WaitForSeconds(t);

        cam.SetIsSnapping(true);
        gm.SetPlayerControlEnabled();
        gm.SetUI(true);
    }

    IEnumerator CoSetScenePic(int n) {
        SetCWindow(false);
        CloseScenePics();

        //gm.SetCurtain(0, true, 0);

        GameObject pic = scenes[n];
        StartCoroutine(CoFadeOut(pic, 0));
        scenes[n].SetActive(true);

        StartCoroutine(CoFadeIn(pic, 1));

        yield return new WaitForSeconds(2f);


        StartCoroutine(CoFadeOut(pic, 1));
        yield return new WaitForSeconds(1f);
        Debug.Log(">>>>>>>> Scene end");

        CloseScenePics();
        //gm.SetCurtain(0, false, 0);
        //SetCWindow(true);
        SetScript();
    }

    IEnumerator CoWait(float t) {
        yield return new WaitForSeconds(t);
        SetScript();
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

    IEnumerator CoSetScreamer(float t) {

        SetCWindow(false);
        playerControl.SetScreamer(true);

        yield return new WaitForSeconds(t);


        playerControl.SetScreamer(false);
        SetCWindow(true);
        SetScript();
    }

    private void CloseScenePics() {
        for (int i = 0; i < scenes.Length; i++)
            scenes[i].SetActive(false);
    }

    public void KillEnemy(int id) {
        if (isActionQuestOn && (questEnemyID == id || id == -999))
            questEnemyNum -= 1;
    }

    public bool GetIsSequenceEnd() {
        if (isSequenceEnd) {
            Debug.Log("QM :: SequenceEnd!");
            isSequenceEnd = false;
            return true;
        }
        return false;
    }

    private string ConvertIDtoName(int id) {
        switch (id) {
            case ID_CHARACTER_KASTONIA_KING:
                return "왕";
            case ID_CHARACTER_KASTONIA_PLAYER:
                return "왕자";
            case ID_CHARACTER_KASTONIA_WILSON:
                return "호위대장";
            case ID_CHARACTER_ARDILL_MAN:
                return "임호수";
            case ID_CHARACTER_ARDILL_WOMAN:
                return "할머니";
            case ID_CHARACTER_BOOLCITY_MAN:
                return "상가주인";
            case ID_CHARACTER_BOOLCITY_WOMAN:
                return "임연수";
            case ID_CHARACTER_REVCASTLE_KING:
                return "어느 성의 왕";
            case ID_CHARACTER_REVCASTLE_SON:
                return "???";
            default:
                return null;
        }
    }


}

class QuestData {

    private const string CODE_ERROR = "-11111";
    private const string CODE_NULL = "-99999";

    private string questName;
    private int questID;    // mmqqccc => m: mapID, q: quest index, c : conversation index(000);
    //public int[] npcId;
    //private Dictionary<int, string> dxnry_conversation;
    private ArrayList qScripts;

    private static int index;

    public QuestData(string name, int id /*, int[] npc*/) {
        questName = name;       //퀘스트 이름. 아이디아님. 나중에 퀘스트 창 열면 쓰일 이름정도나 코딩하면서 확인용
        questID = id;
        qScripts = new ArrayList();
        //npcId = npc;        //퀘스트 진행시 만나야 할 npc 순서.
        index = 0;
    }

    public void AddQuestBranch() {
        qScripts.Add(new QuestScript());
    }

    public void AddQuestAction(int id, string c, float t) {
        qScripts.Add(new QuestScript(id, c, t));
    }

    public void AddQuestAction(int id, string c, bool b) {
        qScripts.Add(new QuestScript(id, c, b));
    }

    public void AddQuestAction(int id, float x, bool b) {
        qScripts.Add(new QuestScript(id, x, b));
    }

    public void AddQuestAction(int id, float x, float v, bool b) {
        qScripts.Add(new QuestScript(id, x, v, b));
    }

    public void AddQuestEnemy(int id, int num, string c) {
        qScripts.Add(new QuestScript(id, num, c));
    }

    public void AddQuestEnemy(int id, bool fake) {
        qScripts.Add(new QuestScript(id, fake));
    }

    public void AddQuestScript(int id, string c) {
        qScripts.Add(new QuestScript(id, c));
    }

    public void AddQuestScene(int id, int n) {
        qScripts.Add(new QuestScript(id, n));
    }

    public void AddQuestScene(int id, int n, float t, bool b) {  // curtain
        qScripts.Add(new QuestScript(id, n, t, b));
    }

    public void AddQuestWait(float t) {    // wait
        qScripts.Add(new QuestScript(t));
    }

    public QuestScript GetQuestScript() {
        if (index < qScripts.Count) {
            QuestScript qScript = (QuestScript)qScripts[index];
            index++;
            return qScript;
        }
        else {
            index = 0;
            return null;
        }
    }
}

class QuestScript {
    private int id;
    private int num;
    private string content;
    private int type;   // 0: conv, 1: enemy, 2: move, 3: action, 4: scene, 5: wait
    private int sceneNum;
    private float f;
    private float v;
    private bool b;

    public QuestScript(int id, string c) {  // conversation
        type = 0;
        this.id = id;
        content = c;
    }

    public QuestScript(int id, int n, string c) {   // enemy
        type = 1;
        this.id = id;
        num = n;
        content = c;
        b = true;
    }
    
    public QuestScript(int id, bool fake) {   // enemy showUp
        type = 1;
        this.id = id;
        b = false;
    }

    public QuestScript(int id, float x, bool b) {   // move
        type = 2;
        this.id = id;
        f = x;
        this.b = b;
    }

    public QuestScript(int id, float x, float v, bool b) {   // move
        type = 2;
        this.id = id;
        f = x;
        this.v = v;
        this.b = b;
    }

    public QuestScript(int id, string c, bool b) {   // lie
        type = 3;
        this.id = id;
        content = c;
        this.b = b;
    }

    public QuestScript(int id, string c, float f) {   //  screamer, ...
        type = 3;
        this.id = id;
        content = c;
        this.f = f;
    }

    public QuestScript(int id, int n) {   // scene
        type = 4;
        this.id = id;
        sceneNum = n;
    }
    public QuestScript(int id, int n, float t, bool b) {   // curtain
        type = 4;
        this.id = id;
        sceneNum = n;
        f = t;
        this.b = b;
    }

    public QuestScript(float t) {   // wait
        type = 5;
        f = t;
    }

    public QuestScript() {   // branch
        type = -1;
    }


    public int GetScriptType() {
        return type;
    }

    public int GetID() {
        return id;
    }

    public int GetNum() {
        return num;
    }

    public string GetContent() {
        return content;
    }

    public int GetSceneNumber() {
        return sceneNum;
    }

    public float GetFloat() {
        return f;
    }

    public float GetSpeed() {
        return v;
    }

    public bool GetBool() {
        return b;
    }
}
