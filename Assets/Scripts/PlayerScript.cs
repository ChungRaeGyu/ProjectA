using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PhotonView pv;
    [SerializeField] TextMesh nickName;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        Debug.Log("»ý¼º");
        nickName.text = (string)pv.InstantiationData[0];
        //nickName.text = PhotonNetwork.LocalPlayer.NickName;
    }

    public void NickNameSet(string name)
    {
        nickName.text = name;
    }
}
