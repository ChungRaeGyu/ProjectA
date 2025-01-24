using NUnit.Framework;
using Photon.Pun;
using System;
using System.Collections;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public enum ERole
{
    MERIN,
    PERCIVAL,
    MORGANA,
    MORDRED,
    ASSASSIN,
    OBERON,
    NORMAL
}
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
    public static int[] list;

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
        object[] tempobj= new object[2]; //이름, 진영, 
        foreach(Toggle tempToggle in Job)
        {
            if (tempToggle.isOn)
            {
                Role.roleList.Add(tempToggle.GetComponent<RoleInfo>());
            }
        }
        for(int i=0; i <maxGood - good; i++)
        {
            RoleInfo roleInfo = new RoleInfo();
            roleInfo.SetRole(true, ERole.NORMAL);
            Role.roleList.Add(roleInfo);
        }
        for (int i = 0; i < maxEvil - evil; i++)
        {
            RoleInfo roleInfo = new RoleInfo();
            roleInfo.SetRole(false, ERole.NORMAL);
            Role.roleList.Add(roleInfo);
        }
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
