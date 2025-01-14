using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
public enum Stage{
    READY,
    START,
    EXPEDITIONCHOICE,
    VOTING,
    EXPEDITIONRESULT,
    FINDKING,
    ENDGIN
}

public class GameManager : MonoBehaviourPunCallbacks
{

    PhotonView pv;
    //UI
    [SerializeField] TMP_Text roomNameTxt;
    //PlayerObject
    GameObject character;
    //Ready
    [SerializeField] Transform[] seats; //자리의 위치를 가지고 있다.
    Dictionary<int, Player> seatNum = new Dictionary<int, Player>();
    [SerializeField] List<int> turnList = new List<int>();
    [SerializeField] TMP_Text test;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                int random = RandomNum();
                turnList.Add(random);
                seatNum.Add(random, player);
            }
            ListSort(turnList);
            pv.RPC("SeetTurnList", RpcTarget.AllBuffered, turnList.ToArray());
        }

    }
    void Start()
    {
        roomNameTxt.text = $"방 이름 : {PhotonNetwork.CurrentRoom.Name}";
        //character = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); //Resourse 풀더안에 있어야한다.
        //pv.RPC("RandomPlacement", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, RandomNum());
    }
    void Update()
    {
        Test();
    }
    private void Test()
    {
        string a = "";
        foreach (var player in seatNum) {
            a += player;
        }
        test.text = a;
        a = "";
    }


    #region Ready
    [PunRPC]
    private void SeetTurnList(int[] tempList)
    {
        turnList = tempList.ToList();
    }

    private int RandomNum()
    {
        int rand;
        do
        {
            rand = Random.Range(0, seats.Length);
            Debug.Log("랜덤 뽑기");
        } while (seatNum.ContainsKey(rand));

        return rand;
        //Todo: seatNum에 넣어준다. 그리고 저 정보들을 끝나고 한대 모은다?
    }

    private void ListSort(List<int> tempList)
    {
        for (int i = 0; i < tempList.Count; i++)
        {
            Sorting(tempList, i, i-1);
        }
        Debug.Log("TempList");

    }

    void Sorting(List<int> tempList, int a, int b)
    {

        if (a < 0 || b < 0)
        {
            return;
        }
        string num = "";
        foreach (int i in tempList)
        {
            num += i + ",";
        }
        Debug.Log(num);
        if (tempList[b] > tempList[a])
        {
            int temp = tempList[b];
            tempList[b] = tempList[a];
            tempList[a] = temp;
            Sorting(tempList, a-1, b-1);
        }
        else
        {
            return;
        }

    }
    #endregion
}