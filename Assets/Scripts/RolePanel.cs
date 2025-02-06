using TMPro;
using UnityEngine;

public class RolePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text roleText;
    [SerializeField] private TMP_Text teamText;
   


    public void SetRolePanel(RoleInfo roleInfo)
    {
        roleText.text = roleInfo.name;
        teamText.text = roleInfo.camp ? "악 세력" : "선 세력";
    }
    public void CloseBtn()
    {
        gameObject.SetActive(false);
    }
    public void OpenBtn()
    {
        gameObject.SetActive(true);
    }
}
