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
        roomNameTxt.text = $"�� �̸� : {PhotonNetwork.CurrentRoom.Name}";
        character = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); //Resourse Ǯ���ȿ� �־���Ѵ�.
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
        //������ ���̽��� �ִٸ� �̰� �޾ƿ��� ���ε� ���߿� ���� �ǵ�
        UpdatePlayerList();
        //�� ��� �̰ź��� ������ �����ؾ���
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }
    #region UI

    private void UpdatePlayerList()
    {
        peopleNumTxt.text = $"{PhotonNetwork.PlayerList.Length}��";
    }
    #endregion

    #region Ready
    [PunRPC]
    private void RandomPlacement(Player player,int num)
    {
        //Start���� ��� �� �ڸ� ���� ���� �� Dictionary�� �߰� 
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
        //Todo: seatNum�� �־��ش�. �׸��� �� �������� ������ �Ѵ� ������?
    }
    #endregion
}