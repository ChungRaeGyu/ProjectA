using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] TMP_Text peopleNumTxt;
    //PlayerObject
    GameObject character;
    //Ready
    [SerializeField] Camera TopViewCamera; //choice seat
    [SerializeField] Transform[] seats;
    Dictionary<Player, int> seatNum = new Dictionary<Player, int>();

    [SerializeField] TMP_Text test;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        roomNameTxt.text = $"방 이름 : {PhotonNetwork.CurrentRoom.Name}";
        character = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); //Resourse 풀더안에 있어야한다.
        pv.RPC("RandomPlacement", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, RandomNum());
        UpdatePlayerList();
    }
    void Update()
    {
        Test();
    }    
    private void Test()
    {
        string a="";
        foreach (var player in seatNum) {
            a += player;
        }
        test.text = a;
        a = "";
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //데이터 베이스가 있다면 이걸 받아오면 딱인데 나중에 넣을 건데
        UpdatePlayerList();
        //자 잠깐만 이거봐봐 들어오면 공유해야지
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    #region UI

    private void UpdatePlayerList()
    {
        peopleNumTxt.text = $"{PhotonNetwork.PlayerList.Length}명";
    }
    #endregion

    #region Ready
    [PunRPC]
    private void RandomPlacement(Player player,int num)
    {
        //Start에서 사용 됌 자리 랜덤 설정 후 Dictionary에 추가 
        seatNum.Add(player, num);
        character.transform.position = seats[seatNum[player]].transform.position;

    }

    private int RandomNum()
    {
        int rand;
        do
        {
            rand = Random.Range(0, seats.Length);

        } while (seatNum.ContainsValue(rand));

        return rand;
        //Todo: seatNum에 넣어준다. 그리고 저 정보들을 끝나고 한대 모은다?
    }
    #endregion
}