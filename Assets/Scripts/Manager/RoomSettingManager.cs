using Photon.Pun;
using System.Collections;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class RoomSettingManager : MonoBehaviour
{
    [SerializeField] TMP_Text goodTxt;
    [SerializeField] TMP_Text evilTxt;

    [SerializeField]
    Toggle[] Job;

    int good=1; //Toggle을 체크할때마다 올려주기?
    int maxGood;
    int evil=1;
    int maxEvil;
    public void UpdateCount(int count)
    {
        switch (count)
        {
            case 5:
                UpdateMaxCount(3, 2);
                break;
            case 6:
                UpdateMaxCount(4, 2);
                break;
            case 7:
                UpdateMaxCount(4, 3);
                break;
            case 8:
                UpdateMaxCount(5, 3);
                break;
            case 9:
                UpdateMaxCount(6, 3);
                break;
            case 10:
                UpdateMaxCount(6, 4);
                break;
            default:
                UpdateMaxCount(3, 2);
                break;
        }
    }
    void UpdateMaxCount(int _maxGood, int _maxEvil)
    {
        this.maxGood  = _maxGood;
        this.maxEvil = _maxEvil;
        UpdateCount();
    }
    void UpdateCount()
    {
        SetColor();
        goodTxt.text = $"선 {good}/{maxGood}";
        evilTxt.text = $"악 {evil}/{maxEvil}";
        
    }

    void SetColor()
    {
        if (good != maxGood)
            goodTxt.color = Color.red;
        else 
            goodTxt.color = Color.black;
        if (evil != maxEvil)
            evilTxt.color = Color.red;
        else
            evilTxt.color = Color.black;
    }
    public void ToggleUpdate(Toggle toggle)
    {
        if (toggle.isOn)
        {
            if (toggle.CompareTag("Good"))
                good++;
            else
                evil++;
        }
        else
        {
            if (toggle.CompareTag("Good"))
                good--;
            else 
                evil--;
        }
        UpdateCount();

    }
    public bool JobCount()
    {
        return good==maxGood&&evil==maxEvil;
    }
    public void SetJob()
    {
        Hashtable jobs = new Hashtable();
        foreach(Toggle tempToggle in Job)
        {
            if (tempToggle.isOn)
            {
                jobs.Add(tempToggle.name, tempToggle.name);
            }
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(jobs);
    }
    #region 버튼
    public void NormalUpBtn(TMP_Text text)
    {
        int temp = int.Parse(text.text);
        if(text.CompareTag("Good"))
            good++;
        else
            evil++;
        text.text = (temp+1).ToString();
        UpdateCount();
    }
    public void NormalDownBtn(TMP_Text text)
    {
        int temp = int.Parse(text.text);
        if (temp == 0) return;
        if (text.CompareTag("Good"))
            good--;
        else
            evil--;
        text.text = (temp-1).ToString();
        UpdateCount();
    }

    public void OpenPanelBtn()
    {
        gameObject.SetActive(true);
    }
    public void ClosePanelBtn()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
