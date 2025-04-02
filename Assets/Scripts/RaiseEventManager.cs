using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon; // RaiseEvent 사용을 위해 필요
using UnityEngine;

public class RaiseEventManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        GameManager.instance.RaiseEventManager = this;
    }
    public void SendAllUpdate(EGameState currentState)
    {
        //object[] content = new object[] { newHP }; // 보낼 데이터
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // 모든 클라이언트에게 보냄
        //TargetActor = new int[] {ActorNumber로 가능}
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장 (패킷 손실 방지)

        PhotonNetwork.RaiseEvent((byte)currentState, null,options, sendOptions);
    }

    public void SendOtherUpdate(EGameState currentState)
    {
        //object[] content = new object[] { newHP }; // 보낼 데이터
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // 모든 클라이언트에게 보냄
        //TargetActor = new int[] {ActorNumber로 가능}
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장 (패킷 손실 방지)

        PhotonNetwork.RaiseEvent((byte)currentState, null, options, sendOptions);
    }
    public void SendPlayersUpdate(EGameState currentState, int[] Acts)
    {
        //object[] content = new object[] { newHP }; // 보낼 데이터
        RaiseEventOptions options = new RaiseEventOptions { TargetActors = Acts}; // 모든 클라이언트에게 보냄
        //TargetActor = new int[] {ActorNumber로 가능}
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장 (패킷 손실 방지)

        PhotonNetwork.RaiseEvent((byte)currentState, null, options, sendOptions);
    }



    public void AddHandler()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent; // 이벤트 리스너 등록
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent; // 이벤트 리스너 해제
    }

    private void OnEvent(EventData photonEvent)
    {
        /*원래 각 스크립트에 OnEvent를 필요할때 Handler를 재등록하는 방식으로 switch를 사용하지 않으려고 했으나
         * 이 게임의 진행방식이 마스터가 전체 진행을 맡고 필요할때마다 다른사람에게 호출하는 방식으로 진행이 되고 있어서
         * 
        */
        switch ((EGameState)photonEvent.Code)
        {
            case EGameState.READY:
                break;

        }
        //object[] data = (object[])photonEvent.CustomData; null자리에 data를 입력해주면 받아올 수 있다. 
        //여기서 사용할 메소드를 정하면 됌 
    }
}
