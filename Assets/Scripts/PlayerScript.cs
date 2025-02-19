using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //닉네임
    //캐릭터 머리 돌아가는거

    PhotonView pv;
    [SerializeField] TextMesh nickName;
    public RoleInfo roleInfo;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        nickName.text = (string)pv.InstantiationData[0];
    }

    public void NickNameSet(string name)
    {
        nickName.text = name;
    }
}
