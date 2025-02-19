using Photon.Realtime;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceCrewPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform[] choicePos;
    GameManager gameManager;
    GameObject togglePrefab;
    GameObject toggleBox;

    private void Awake()
    {
        gameManager = GameManager.instance;
        gameManager.choiceCrewPanel = this;
    }
    void Start()
    {
        for (int i = 0; i < gameManager.seatNum.Count; i++)
        {
            GameObject temp = Instantiate(togglePrefab, toggleBox.transform);
            temp.transform.position = choicePos[i].position;
            temp.GetComponentInChildren<Text>().text = gameManager.seatNum[i].NickName;
        }
    }

    public void CompleteBtn()
    {
        //누굴 선택했는지 확인하기
        //
    }
}
