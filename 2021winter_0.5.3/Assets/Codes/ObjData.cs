using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjData : MonoBehaviour
{
    public int id;
    public bool isNpc;
    public GameObject questOrder;
    public QuestManager questManager;

    void Update()   //퀘스트 마지막 클리어시 퀘스트 번호 증가하면서 비어있는 배열 건드림. 그로인한 어쩔 수 없는 오류 막고자
    {               //만든 코드. 그런데 오류가 한번은 나는 것도 문제고 업데이트에 들어있
        /*
        try         //퀘스트가 해당 Npc 차례일 때 머리 위에 느낌표 뜨는 코드
        {
            if (isNpc)
            {
                if (id == questManager.dxnry_quest[questManager.questId].npcId[questManager.questActionIndex])
                {
                    questOrder.SetActive(true);
                }
                else
                {
                    questOrder.SetActive(false);
                }
            }
        } catch(KeyNotFoundException e)
        {

        }
        */
    }   
}
