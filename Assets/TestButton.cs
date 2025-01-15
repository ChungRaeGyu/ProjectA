using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{

    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button.GetComponent<Button>();
        button.onClick.AddListener(NetworkManager.Instance.RoomOut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
