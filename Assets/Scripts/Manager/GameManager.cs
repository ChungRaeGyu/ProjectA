using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public EGameState currentState;
    [HideInInspector] public RaiseEventManager RaiseEventManager;
    PhotonView pv;
    //UI
    [HideInInspector] public RoomUImanager roomUImanager;
    //PlayerObject
    GameObject character; //게임오브젝트와 닉네임 관리를 위한
    PlayerScript playerScript;
    //Ready
    public int playerCount;
    [SerializeField] Transform[] seats; //자리의 위치를 가지고 있다.
    public Dictionary<int, Player> seatNum = new Dictionary<int, Player>();  //자리 위치, 플레이어actnum 마스터만
    int allReady = 0; //역할 확인 대기
    //Role
    Dictionary<RoleInfo, int> roleAllocation = new Dictionary<RoleInfo, int>(); //마스터만 역할에 따른 플레이어 넘버를 가지고 있다.
    public List<int> turnList = new List<int>(); //이 턴리스트와 seatNum을 조합하여서 순서대로 플레이어가 나온다.
    //Expedition
    public bool waitExpedition = false;
    int turnCount = -1;
    public int[] expeditionCount = new int[5];
    public int round = 0;

    public string crewText;  //임시
    public string[] crews = new string[5]; //영구 보관, 나중에 원정대 결과 확인 하는 script로 이동가능

    //[Header("Voting")]
    

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
        PlayerCounting();
    }

    private void PlayerCounting()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        switch (playerCount)
        {
            case 5:
                expeditionCount = new int[] { 2, 3, 2, 3, 3 };
                break;
            case 6:
                expeditionCount = new int[] { 2, 3, 4, 3, 4 };
                break;
            case 7:
                expeditionCount = new int[] { 2, 3, 3, 3, 4 };
                break;
            case 8:
            case 9:
            case 10:
                expeditionCount = new int[] { 3, 4, 4, 5, 5 };
                break;
        }
    }

    void Start()
    {
        //state으로 처리하자
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(CGameProgress());
    }

    public void RoundUpdate(int num)
    {
        Hashtable newPro = new Hashtable();
        newPro["Round"] = num;
        PhotonNetwork.CurrentRoom.SetCustomProperties(newPro);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        //customproperties가 없다 아니 근데 이걸 왜 호출 되냐고 ㅋㅋㅋㅋㅋ
        if (propertiesThatChanged.ContainsKey("Round"))
            round = (int)PhotonNetwork.CurrentRoom.CustomProperties["Round"];
        if (propertiesThatChanged.ContainsKey("State"))
            currentState = (EGameState)PhotonNetwork.CurrentRoom.CustomProperties["State"];

    }
    #region 자리정리 및 랜덤값 기능
    private int RandomNum(Dictionary<int, Player> tempDic)
    {
        int rand;
        do
        {
            rand = Random.Range(0, seats.Length);
        } while (tempDic.ContainsKey(rand));

        return rand;
        //Todo: seatNum에 넣어준다. 그리고 저 정보들을 끝나고 한대 모은다?
    }

    private void ListSort(List<int> tempList)
    {
        for (int i = 0; i < tempList.Count; i++)
        {
            Sorting(tempList, i, i - 1);
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
            Sorting(tempList, a - 1, b - 1);
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
                    yield return StartCoroutine(CReady());
                    break;
                case EGameState.ChoiceEXPEDITION:
                    yield return StartCoroutine(CExpedition());
                    break;
                case EGameState.VOTING:
                    yield return StartCoroutine(CVoting());
                    break;
                case EGameState.EXPEDITIONVOTING:
                    yield return StartCoroutine(CExPeditionVoting());
                    break;
                case EGameState.EXPEDITIONRESULT:
                    yield return StartCoroutine(CExPeditionVoting());
                    break;
            }
        }
    }
    #region CExpeditionResult
/*    IEnumerator CExPeditionResult()
    {
        //여기다가 할 일 결과를 보여주고 panel이 사라지게 만든다. 결과 공유가 필요함
    }*/

    #endregion

    #region CExpeditionVoting
    IEnumerator CExPeditionVoting()
    {
        Log("원정대의 투표");
        Voting(roomUImanager.choiceCrewPanel.GetPlayers());
        yield return new WaitUntil(() => roomUImanager.VotingPanel.GetAllReady() == expeditionCount[round]);
        roomUImanager.VotingPanel.Reset();
        ChangeState(EGameState.EXPEDITIONRESULT);
        Log("원정대 투표 종료");

    }
    public void Voting(List<Player> players)
    {
        foreach (Player player in players)
        {
            pv.RPC("VotingAction", player);
        }
    }
    #endregion
    #region CVoting
    IEnumerator CVoting()
    {
        Log("원정대원에 대한 투표");
        pv.RPC("VotingAction", RpcTarget.All);
        //원정대원에 대한 찬성반대
        //모두가 투표를 완료했는지 기다려야한다.
        yield return new WaitUntil(()=>roomUImanager.VotingPanel.GetAllReady() == playerCount);
        if (roomUImanager.VotingPanel.GetResult())
            ChangeState(EGameState.EXPEDITIONVOTING);
        else
            ChangeState(EGameState.ChoiceEXPEDITION);
        roomUImanager.VotingPanel.Reset();

        Log("원정대원 투표 종료");
    }
    [PunRPC]
    private void VotingAction()
    {
        roomUImanager.VotingPanel.gameObject.SetActive(true);
        //원정대원에 대한 찬성 반대 고르기
    }
    #endregion
    #region CExepedition
    IEnumerator CExpedition()
    {
        Log("원정대원 뽑기");
        //여기서 그러면 첫번쨰 할일 그 첫번째 원정대장 정하기
        if (turnCount == -1)
            turnCount = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
        else
        {
            turnCount++;
            if (turnCount >= PhotonNetwork.CurrentRoom.PlayerCount)
                turnCount = 0;
        }

        pv.RPC("ChoiceCrew", seatNum[turnList[turnCount]]);
        //그리고 원정대원 뽑기 
        //얘가 매 프레임마다 확인을 한다 이말이지;;
        yield return new WaitUntil(()=> waitExpedition==true);
        ChangeState(EGameState.VOTING);
        waitExpedition = false;
        Log("원정대원 뽑기 종료");
    }
    [PunRPC]
    private void ChoiceCrew()
    {
        //원정대장에게 대원 선택할 수 있는 창을 준다. 버튼 만들어 주기
        roomUImanager.ChoiceBtnActiveControl(true);
    }

    public void WaitExpeditionBool()
    {
        pv.RPC("SetBool", RpcTarget.MasterClient);
    }
    [PunRPC]
    private void SetBool()
    {
        waitExpedition = true;
    }

    #endregion
    #region CReady
    IEnumerator CReady()
    {
        Log("준비시작");
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                int random = RandomNum(seatNum);
                turnList.Add(random); // 9 1 0 8 4
                seatNum.Add(random, player);
                pv.RPC("SeatTurnList", player, random);
            }
            int[] tempTurn = turnList.ToArray();
            pv.RPC("SeatShare", RpcTarget.OthersBuffered, tempTurn);
            ListSort(turnList);
            RoleAllocationing();
        }
        yield return new WaitUntil(() => CheckAllReady());
        Log("준비 끝");
    }
    private bool CheckAllReady()
    {
        if (allReady == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            ChangeState(EGameState.ChoiceEXPEDITION);
            allReady = 0;
            return true;
        }
        else
            return false;
    }
    #region 자리배정
    [PunRPC]
    private void SeatShare(int[] rand)
    {
        turnList = rand.ToList();
        for (int i = 0; i < rand.Length; i++)
        {
            seatNum.Add(rand[i], PhotonNetwork.PlayerList[i]);
        }
        ListSort(turnList);
    }
    [PunRPC]
    private void SeatTurnList(int rand)
    {
        //모두
        Seating(rand);
    }
    private void Seating(int rand)
    {
        //모두
        object[] customData = new object[] { PhotonNetwork.LocalPlayer.NickName };
        character = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0, customData);
        character.transform.position = seats[rand].position;
        playerScript = character.GetComponent<PlayerScript>();
    }
    #endregion

    #region 역할배정
    private void RoleAllocationing()
    {
        //얘는 마스터클라이언트만진행
        Role.SuffleRole();
        var playerList = PhotonNetwork.PlayerList;
        for (int i = 0; i < playerList.Count(); i++)
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

        roomUImanager.RoleSharing(playerScript.roleInfo);
    }

    public void AllReady()
    {
        pv.RPC("PAllReady", PhotonNetwork.MasterClient);
    }
  
    [PunRPC]
    private void PAllReady()
    {
        allReady++;
    }
    #endregion
    #endregion
    public void ChangeState(EGameState state) 
    {
        currentState = state;
    }

    public void Log(string msg)
    {
        Debug.Log(msg);
    }
}