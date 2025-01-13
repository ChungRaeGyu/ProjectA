using TMPro;
using UnityEngine;

public class NickNameControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private TMP_Text nickName;    
    void Awake()
    {
        nickName = GetComponent<TMP_Text>();
    }
    public void NickNameSet(string name)
    {
        nickName.text = name;
    }
}
