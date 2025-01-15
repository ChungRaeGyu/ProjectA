using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Content;
    Dictionary<Player,GameObject> nickName = new Dictionary<Player,GameObject>();
    [SerializeField] GameObject NickNamePrefab;
    [SerializeField] TMP_Text roomNameTxt;
    [SerializeField] TMP_Text playerCountTxt;
    [SerializeField] GameObject gameStartBtn;
    PhotonView pv;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        pv.RPC("CreateNickNameObj", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
        roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
        PlayerTextSet();

        if(PhotonNetwork.IsMasterClient)
        {
            //������ Ŭ���̾�Ʈ�� ������
            gameStartBtn.SetActive(true);
        }
    }
    [PunRPC]
    private void CreateNickNameObj(Player newPlayer)
    {
        //Start���� ���
        GameObject temp = Instantiate(NickNamePrefab, Content.transform);
        temp.GetComponent<TMP_Text>().text = newPlayer.NickName;
        nickName.Add(newPlayer, temp);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("���� " + newPlayer.NickName);
        PlayerTextSet();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("���� " + otherPlayer.NickName);
        Destroy(nickName[otherPlayer].gameObject);
        Debug.Log(nickName.Count());
        nickName.Remove(otherPlayer);
        PlayerTextSet();
    }

    private void PlayerTextSet()
    {
        playerCountTxt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "��";
    }

    public void GameStartBtn()
    {
        //���ӽ��۹�ư
        PhotonNetwork.LoadLevel("Room");
    }

    public void ExitRoomBtn()
    {
        //�泪����
        PhotonNetwork.LeaveRoom(this);
    }
    
    /*
    [PunRPC]
    private void SetParentNickName(GameObject obj)
    {
        //Awake���� ��� , NickName������Ʈ�� �θ� ����
        obj.transform.SetParent(Content.transform);
        obj.SetActive(false);
        NickNamePrefab.Add(obj.GetComponent<NickNameControl>());
    }
    IEnumerator UpdateUI()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5f);
            UpdatePlayerList();
        }
    }

    //�ƿ� �ݺ��ؼ� ���� �ȵ� �� GetComponent
    void UpdatePlayerList()
    {
        foreach(NickNameControl temp in NickNamePrefab)
        {
            temp.gameObject.SetActive(false);
        }

        var player = PhotonNetwork.PlayerList;
        for(int i=0; i < player.Length; i++)
        {
            NickNamePrefab[i].gameObject.SetActive(true);
            NickNamePrefab[i].NickNameSet(player[i].NickName);
        }
    }
    */

}
