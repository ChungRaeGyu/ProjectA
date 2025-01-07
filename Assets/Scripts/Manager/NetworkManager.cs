using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using WebSocketSharp;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;
    public TMP_Text StatusText;
    public TMP_InputField NickNameInput;
    public TMP_InputField RoomInputText;
    public GameObject RoomNameList_Txt;
    public GameObject RoomListContent;
    string RoomName;
    List<RoomInfo> myList = new List<RoomInfo>();
    // Start is called before the first frame update
    private void Awake() {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
        Screen.SetResolution(1080,600, false);
        PhotonNetwork.SendRate = 60; //값을 보내는 빈도이다.
        PhotonNetwork.SerializationRate = 30; //동기화 빈도
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }
    public void Connect(){
        PhotonNetwork.ConnectUsingSettings();   
    }
    public override void OnConnectedToMaster()
    {
        print("서버접속 완료");
        if(PhotonNetwork.LocalPlayer.NickName.IsNullOrEmpty()){
            PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        }
        JoinLobby();
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void JoinLobby()=>PhotonNetwork.JoinLobby();
    
    public override void OnJoinedLobby(){   
        print("입장");
        myList.Clear();
    }

    public void CreateRoom() {
        PhotonNetwork.CreateRoom(RoomInputText.text);
    }
    public void ButtonJoinRoom()
    {
        print("이름 : " + EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        PhotonNetwork.JoinRoom(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
    }
    public override void OnCreatedRoom()
    {    
        RoomName=RoomInputText.text;
        print("방생성 완료" + RoomName);
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방만들기 실패");
    }
    public void JoinRandomRoom(){
        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.LoadLevel("Room");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("방 랜덤참가 실패");
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomInputText.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Room");
        print("방 입장");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("message : " + message);
        print("방참가 실패");
    }
    public void RoomOut(){
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
        print("방 나옴");
    }
}
