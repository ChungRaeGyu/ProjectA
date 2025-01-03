using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButtonManager : MonoBehaviour
{
    private NetworkManager networkManager;
    public Button roomMakeBtn;
    public Button roomEnterBtn;
    public Button randomRoomEnterBtn;
    public TMP_InputField roomNameInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        networkManager = NetworkManager.Instance;
        roomMakeBtn.onClick.AddListener(networkManager.CreateRoom);
        roomEnterBtn.onClick.AddListener(networkManager.JoinRoom);
        randomRoomEnterBtn.onClick.AddListener(networkManager.JoinRandomRoom);
        networkManager.RoomInputText = roomNameInput;
    }
}
