using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*[System.Serializable]
public class TalkDataDic : SerializableDictionary<int, string[]> { }*/
public class TalkManager : MonoBehaviour {
    
    private Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        /*
        //normal talk
        talkData.Add(1000, new string[] { "난 남자란다.", "자꾸 뭘 꼬라보는거지?" });     //key : npcId, value : 대사
        talkData.Add(2000, new string[] { "난 여자란다.", "눈 깔어;;" });

        //quest talk
        talkData.Add(10 + 1000, new string[] { "옆에 여자한테 가봐." });
        talkData.Add(11 + 2000, new string[] { "남자한테 다시 가봐." });
        talkData.Add(12 + 1000, new string[] { "끝났어." });
    */

        //talkData.Add()
    }

    public string GetTalk(int id, int talkIndex)
    {
       /* if (!talkData.ContainsKey(id)) {
            if (talkIndex == talkData[id - id % 10].Length)
                return null;
            else
                return talkData[id - id % 10][talkIndex];
        }*/
       
        if(talkIndex == talkData[id].Length)    //대화끝나면 대화창 닫음
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];     //아니면 계속진행
        }
    }
}
