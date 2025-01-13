using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Content;
    Dictionary<Player,GameObject> nickName = new Dictionary<Player,GameObject>();
    public GameObject NickNamePrefab;
    PhotonView pv;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        pv.RPC("CreateNickNameObj", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
        //CreateNickNameObj(PhotonNetwork.LocalPlayer);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("들어옴 " + newPlayer.NickName);
    }
    [PunRPC]
    private void CreateNickNameObj(Player newPlayer)
    {
        //Start에서 사용
        GameObject temp = Instantiate(NickNamePrefab, Content.transform);
        temp.GetComponent<TMP_Text>().text = newPlayer.NickName;
        nickName.Add(newPlayer, temp);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("나감 " + otherPlayer.NickName);
        Destroy(nickName[otherPlayer].gameObject);
        Debug.Log(nickName.Count());
        nickName.Remove(otherPlayer);
    }
    /*
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
    */

}
