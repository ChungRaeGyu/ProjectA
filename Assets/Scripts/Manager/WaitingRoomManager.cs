using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingRoomManager : MonoBehaviour
{
    [SerializeField] GameObject Content;
    public List<NickNameControl> NickNamePrefab = new List<NickNameControl>();
    PhotonView pv;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            for(int i=0; i < 10; i++)
            {
                GameObject temp = PhotonNetwork.Instantiate("NickName", Vector3.zero, Quaternion.identity);
                pv.RPC("SetParentNickName", RpcTarget.AllBuffered, temp);
            }
        }
    }
    void Start()
    {
        StartCoroutine(UpdateUI());
    }

    [PunRPC]
    private void SetParentNickName(GameObject obj)
    {
        //Awake에서 사용 , NickName오브젝트의 부모 지정
        obj.transform.SetParent(Content.transform);
        obj.SetActive(false);
        NickNamePrefab.Add(obj.GetComponent<NickNameControl>());
    }
    IEnumerator UpdateUI()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5f);
            UpdatePlayerList();
        }
    }

    //아오 반복해서 쓰면 안될 것 GetComponent
    void UpdatePlayerList()
    {
        foreach(NickNameControl temp in NickNamePrefab)
        {
            temp.gameObject.SetActive(false);
        }

        var player = PhotonNetwork.PlayerList;
        for(int i=0; i < player.Length; i++)
        {
            NickNamePrefab[i].gameObject.SetActive(true);
            NickNamePrefab[i].NickNameSet(player[i].NickName);
        }
    }
    

}
