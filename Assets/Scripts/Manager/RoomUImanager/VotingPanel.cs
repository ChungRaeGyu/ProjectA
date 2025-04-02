using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class VotingPanel : MonoBehaviour
{
    private int allReady = 0;
    private int agree = 0;
    private int oppo = 0;

    private void Awake()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent; // 이벤트 리스너 등록

    }
    public void AgreeBtn()
    {
        GameManager.instance.AllReady();
        SendUpdate(true);
        GameManager.instance.roomUImanager.PanelControlBtn(this.gameObject);
    }
    public void OppoBtn()
    {
        GameManager.instance.AllReady();
        SendUpdate(false);
        GameManager.instance.roomUImanager.PanelControlBtn(this.gameObject);
    }

    public void SendUpdate(bool agree)
    {
        object[] content = new object[] { agree }; // 보낼 데이터
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; // 모든 클라이언트에게 보냄
        //TargetActor = new int[] {ActorNumber로 가능}
        SendOptions sendOptions = new SendOptions { Reliability = true }; // 신뢰성 보장 (패킷 손실 방지)

        PhotonNetwork.RaiseEvent(1, content, options, sendOptions);
    }
    public void OnEvent(EventData eventData)
    {
        object data = eventData.CustomData;
        
        //Code를 해주지 않으면 data값을 못찾는다.
        if(eventData.Code == 1)
        {
            object[] datas = data as object[];
            allReady++;
            if ((bool)datas[0])
                this.agree++;
            else
                oppo++;
            Debug.Log("수신");
        }
    }

    public int GetAllReady()
    {
        return allReady;
    }

    public bool GetResult()
    {
        Debug.Log(agree);
        return agree > oppo ? true : false;
    }
    public bool GetExpeditionResult()
    {
        //투표 결과
        return oppo >= 1 ? false : true;
    }
    public void Reset()
    {
        allReady = 0;
        agree = 0;
        oppo = 0;
    }
}
