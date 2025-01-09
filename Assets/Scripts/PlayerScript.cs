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
        if(pv.IsMine)
            nickName.text = PhotonNetwork.LocalPlayer.NickName;
    }
}
