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
    [SerializeField] GameObject guidePanel;
    PhotonView pv;

    [SerializeField] RoomSettingManager roomSettingManager;
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
            //마스터 클라이언트가 방장임
            gameStartBtn.SetActive(true);
        }
    }
    [PunRPC]
    private void CreateNickNameObj(Player newPlayer)
    {
        //Start에서 사용
        GameObject temp = Instantiate(NickNamePrefab, Content.transform);
        temp.GetComponent<TMP_Text>().text = newPlayer.NickName;
        nickName.Add(newPlayer, temp);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("들어옴 " + newPlayer.NickName);
        PlayerTextSet();
        if(PhotonNetwork.IsMasterClient)
            roomSettingManager.UpdateCount(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("나감 " + otherPlayer.NickName);
        Destroy(nickName[otherPlayer].gameObject);
        Debug.Log(nickName.Count());
        nickName.Remove(otherPlayer);
        PlayerTextSet();
        if (PhotonNetwork.IsMasterClient)
            roomSettingManager.UpdateCount(PhotonNetwork.CurrentRoom.PlayerCount);

    }

    private void PlayerTextSet()
    {
        playerCountTxt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "명";
    }

    public void GameStartBtn()
    {
        //게임시작버튼
        if (roomSettingManager.JobCount())
            PhotonNetwork.LoadLevel("Room");
        else
            guidePanel.SetActive(true);
    }
    public void CloseGuideBtn()
    {
        guidePanel.SetActive(false);
    }
    public void ExitRoomBtn()
    {
        //방나가기
        PhotonNetwork.LeaveRoom(this);
    }

}
