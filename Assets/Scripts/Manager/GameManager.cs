using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    GameObject character; //게임오브젝트와 닉네임 관리를 위한
    //Ready
    [SerializeField] Transform[] seats; //자리의 위치를 가지고 있다.
    Dictionary<int, int> seatNum = new Dictionary<int, int>();  //자리 위치, 플레이어actnum
    [SerializeField] List<int> turnList = new List<int>();
    [SerializeField] TMP_Text test;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            List<int> randomList = new List<int>();
            List<int> actNumList = new List<int>();
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                int random = RandomNum(randomList);
                turnList.Add(random);
                randomList.Add(random);
                actNumList.Add(player.ActorNumber);
            }
            ListSort(turnList);
            pv.RPC("SeatTurnList", RpcTarget.AllBuffered, turnList.ToArray(),randomList.ToArray(),actNumList.ToArray());
        }
    }
    void Start()
    {
        roomNameTxt.text = $"방 이름 : {PhotonNetwork.CurrentRoom.Name}";


    }
    #region Ready
    [PunRPC]
    private void SeatTurnList(int[] tempList, int[] random, int[] actorNum)
    {
        Debug.Log("punRPC");
        //Awake에 사용되는 중
        turnList = tempList.ToList();
        for(int i=0; i<random.Length; i++)
            seatNum.Add(random[i],actorNum[i]);
        Seating();
    }
    private void Seating()
    {
        object[] customData = new object[] { PhotonNetwork.LocalPlayer.NickName};
        Debug.Log("Seating()");
        GameObject tempObj = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0, customData);
        int num = seatNum.FirstOrDefault(x => x.Value == PhotonNetwork.LocalPlayer.ActorNumber).Key;
        tempObj.transform.position = seats[num].position;
    }
    private int RandomNum(List<int> tempList)
    {
        int rand;
        do
        {
            rand = Random.Range(0, seats.Length);
            Debug.Log("랜덤 뽑기");
        } while (tempList.Contains(rand));

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