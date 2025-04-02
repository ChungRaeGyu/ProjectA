using TMPro;
using UnityEngine;

public class RolePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text roleText;
    [SerializeField] private TMP_Text teamText;
    private bool check = true;


    public void SetRolePanel(RoleInfo roleInfo)
    {
        roleText.text = roleInfo.name;
        teamText.text = roleInfo.camp ? "선 세력" : "악 세력";
    }
    public void CloseBtn()
    {
        if (check)
        {
            GameManager.instance.AllReady();
            check = false;
        }
        gameObject.SetActive(false);
    }
    public void OpenBtn()
    {
        gameObject.SetActive(true);
    }
}
