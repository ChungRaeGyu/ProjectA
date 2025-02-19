using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomUImanager : MonoBehaviour
{

    [SerializeField] TMP_Text roomNameTxt;
    //rolecheck
    [SerializeField] RolePanel rolePanel;

    //Expedition
    [SerializeField] GameObject choiceBtn;
    [SerializeField] ChoiceCrewPanel choiceCrewPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomNameTxt.text = $"πÊ ¿Ã∏ß : {PhotonNetwork.CurrentRoom.Name}";
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

    public void ChoicePanelBtn()
    {
        bool bol = choiceCrewPanel.gameObject.activeInHierarchy ? false : true;
        choiceCrewPanel.gameObject.SetActive(bol);
    }

}
