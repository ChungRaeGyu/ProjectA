using Photon.Realtime;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceCrewPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform[] choicePos;
    GameManager gameManager;
    [SerializeField] GameObject togglePrefab;
    [SerializeField] Transform toggleBox;
    List<Toggle> toggleList = new List<Toggle>();
    List<Player> players = new List<Player>();
    Dictionary<Toggle,Player> toggleDic = new Dictionary<Toggle,Player>();

    [SerializeField] TMP_Text text;

    StringBuilder sb = new StringBuilder();

    private void Awake()
    {
        gameManager = GameManager.instance;
    }
    private void OnEnable()
    {
        text.text = $"{gameManager.expeditionCount[gameManager.round]}만큼 골라주세요.";
    }
    void Start()
    {
        for (int i = 0; i < gameManager.seatNum.Count; i++)
        {
            GameObject temp = Instantiate(togglePrefab, toggleBox);
            temp.transform.position = choicePos[gameManager.turnList[i]].position;
            temp.GetComponentInChildren<Text>().text = gameManager.seatNum[gameManager.turnList[i]].NickName;
            Toggle tempToggle = temp.GetComponent<Toggle>();
            toggleList.Add(tempToggle);
            toggleDic.Add(tempToggle, gameManager.seatNum[gameManager.turnList[i]]);
        }
    }

    public void DecideBtn()
    {
        //모든 토글을 가지고 선택되어 있는게 뭔지 확인하고 싶음
        //그 후 그 토글에 해당하는 플레이어를 담아놓고 허가가 되면 그 사람에게 투표권한을 준다.
        ClearPlayers();
        foreach (Toggle toggle in toggleList)
        {
            if (toggle.isOn)
            {
                players.Add(toggleDic[toggle]);
            }
        }
        //expeditionCount값이 동일 하지 않는다.
        if (players.Count == gameManager.expeditionCount[gameManager.round])
        {
            foreach(Player player in players)
            {
                if (player.GetNext()==null)
                {
                    sb.Append(player.NickName);

                }
                else
                {
                    sb.Append(player.NickName+", ");
                }
            }
            gameManager.roomUImanager.GuideControl(true,0);
            gameManager.roomUImanager.SetGuideText(sb.ToString(), "원정대원확인");
            //이 숫자는 인원수 보드 판에 따라 달라져야 한다.
            //누굴 찍었는지 확인하는 메세지 보여주기
            //맞다고 확인하면 그 사람들에게 투표하라고 쏴주기 ㅇㅇ
        }
        else
        {
            
            gameManager.roomUImanager.GuideControl(true);
            gameManager.roomUImanager.SetGuideText($"{gameManager.expeditionCount[gameManager.round]}명만 골라주세요");
        }
    }

    public void AceptBtn()
    {
        //투표로 넘어감
        gameManager.crewText = gameManager.roomUImanager.GetGuideText();
        gameManager.WaitExpeditionBool();
        gameManager.roomUImanager.GuideControl(false);
        this.gameObject.SetActive(false);
    }
    public void ClearPlayers()
    {
        players.Clear();
        sb.Clear();
    }
    public List<Player> GetPlayers()
    {
        return players;
    }

}
