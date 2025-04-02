using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUImanager : MonoBehaviour
{

    [SerializeField] GameObject guidePanel;
    [SerializeField] TMP_Text guideText;
    [SerializeField] TMP_Text guideSubjectText;
    [SerializeField] GameObject agreeBtn;
    [SerializeField] GameObject disAgreeBtn;
    [SerializeField] GameObject aceptBtn;


    [SerializeField] TMP_Text roomNameTxt;
    //rolecheck
    [SerializeField] RolePanel rolePanel;

    //Expedition
    [SerializeField] GameObject choiceBtn;

    public ChoiceCrewPanel choiceCrewPanel;
    public VotingPanel VotingPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        GameManager.instance.roomUImanager = this;
    }
    void Start()
    {
        roomNameTxt.text = $"방 이름 : {PhotonNetwork.CurrentRoom.Name}";
    }

    public void RoleSharing(RoleInfo roleInfo)
    {
        rolePanel.SetRolePanel(roleInfo);
        rolePanel.OpenBtn();
    }

    public void ChoiceBtnActiveControl(bool bol)
    {
        choiceBtn.SetActive(bol);
    }

    public void PanelControlBtn(GameObject obj)
    {
        bool bol = obj.activeInHierarchy ? false : true;
        obj.SetActive(bol);
    }

    public void SetGuideText(string text,string subject="안내")
    {
        guideSubjectText.text = subject;
        guideText.text = text;
    }
    public string GetGuideText()
    {
        return guideText.text;
    }
    public void GuideControl(bool bol,int num = -1)
    {
        ButtonReset();
        if (bol) 
        {
            //평소에는 확인만 있으면 되고 특수한 상황에서만 확인, 취소가 나와야한다.
            if (num == -1)
            {
                //평소
                aceptBtn.SetActive(true);
            }
            else
            {
                agreeBtn.SetActive(true);
                disAgreeBtn.SetActive(true);
            }
        }
        guidePanel.SetActive(bol);
    }
    public void GuideCloseBtn()
    {
        guidePanel.SetActive(false);
    }
    private void ButtonReset()
    {
        agreeBtn.SetActive(false);
        disAgreeBtn.SetActive(false);
        aceptBtn.SetActive(false);
    }
}
