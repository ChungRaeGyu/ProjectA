using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    private EGameState currentState;
    PhotonView pv;
    //UI
    [SerializeField] TMP_Text roomNameTxt;
    //PlayerObject
    GameObject character; //게임오브젝트와 닉네임 관리를 위한
    PlayerScript playerScript;
    //Ready
    [SerializeField] Transform[] seats; //자리의 위치를 가지고 있다.
    Dictionary<int, int> seatNum = new Dictionary<int, int>();  //자리 위치, 플레이어actnum
    //Role
    Dictionary<RoleInfo,int> roleAllocation = new Dictionary<RoleInfo, int>();
    [SerializeField] List<int> turnList = new List<int>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }


        pv = GetComponent<PhotonView>();
        currentState = EGameState.READY;

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
            RoleAllocationing();
        }
    }
    void Start()
    {
        roomNameTxt.text = $"방 이름 : {PhotonNetwork.CurrentRoom.Name}";
        //state으로 처리하자
        StartCoroutine(CGameProgress());
    }


    #region 자리지정및역할배정
    private void RoleAllocationing()
    {
        //얘는 마스터클라이언트만진행
        Role.SuffleRole();
        var playerList = PhotonNetwork.PlayerList;
        for(int i=0; i < playerList.Count(); i++)
        {
            roleAllocation.Add(Role.roleList[i], playerList[i].ActorNumber);
            pv.RPC("RoleSharing", playerList[i], Role.roleList[i].camp, Role.roleList[i].name);
        }

    }
    [PunRPC]
    private void RoleSharing(bool camp, string name)
    {
        //해당되는 플레이어만
        Debug.Log(name);
        playerScript.roleInfo.SetRole(camp, name);
        Debug.Log("직업 보여 주기");

    }
    [PunRPC]
    private void SeatTurnList(int[] tempList, int[] random, int[] actorNum)
    {
        //모두

        //Awake에 사용되는 중
        turnList = tempList.ToList();
        for(int i=0; i<random.Length; i++)
            seatNum.Add(random[i],actorNum[i]);
        Seating();
    }
    private void Seating()
    {
        //모두

        object[] customData = new object[] { PhotonNetwork.LocalPlayer.NickName};
        character = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0, customData);
        int num = seatNum.FirstOrDefault(x => x.Value == PhotonNetwork.LocalPlayer.ActorNumber).Key;
        character.transform.position = seats[num].position;
        playerScript = character.GetComponent<PlayerScript>();

        // 여기서 부터 게임 시작
        StartCoroutine(CGameProgress());
    }
    private int RandomNum(List<int> tempList)
    {
        int rand;
        do
        {
            rand = Random.Range(0, seats.Length);
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

    IEnumerator CGameProgress()
    {
        while (true)
        {
            Debug.Log("게임 진행 중 ");
            switch (currentState)
            {
                case EGameState.READY:
                    
                    break;

            }
        }
    }
}