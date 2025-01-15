using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
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
    GameObject character;
    //Ready
    [SerializeField] Transform[] seats; //�ڸ��� ��ġ�� ������ �ִ�.
    Dictionary<int, int> seatNum = new Dictionary<int, int>();  //�ڸ� ��ġ, �÷��̾�actnum
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
        roomNameTxt.text = $"�� �̸� : {PhotonNetwork.CurrentRoom.Name}";


    }
    #region Ready
    [PunRPC]
    private void SeatTurnList(int[] tempList, int[] random, int[] actorNum)
    {
        //Awake�� ���Ǵ� ��
        turnList = tempList.ToList();
        for(int i=0; i<random.Length; i++)
            seatNum.Add(random[i],actorNum[i]);
        Seating();

    }
    private void Seating()
    {
        character = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); //Resourse Ǯ���ȿ� �־���Ѵ�.
        //���� �� �� ��ġ �����ֱ�
        int num = seatNum.FirstOrDefault(x => x.Value == PhotonNetwork.LocalPlayer.ActorNumber).Key;
        foreach (KeyValuePair<int, int> temp in seatNum)
        {
            Debug.Log($"Ű : {temp.Key}, Value : {temp.Value}");
        }
        Debug.Log(num);
        character.transform.position = seats[num].position;
    }
    private int RandomNum(List<int> tempList)
    {
        int rand;
        do
        {
            rand = Random.Range(0, seats.Length);
            Debug.Log("���� �̱�");
        } while (tempList.Contains(rand));

        return rand;
        //Todo: seatNum�� �־��ش�. �׸��� �� �������� ������ �Ѵ� ������?
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