using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class EndGame : MonoBehaviour
{
    public GameObject StartBtn;
    public GameObject TimeRecord;
    public Text text_Timer;
    [SerializeField] Transform recordListContent;
    [SerializeField] GameObject recordListItemPrefab;

    private int playercnt = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.name == "Witch3(Clone)")
        {
            if (GameManager.records.Count==PhotonNetwork.CurrentRoom.PlayerCount)
            {
                StartBtn.GetComponent<StartGame>().timeActive = false; //Ÿ�̸� ����

                foreach (KeyValuePair<string, string> record in GameManager.records)//�����ϴ� ��� roomListContent
                {
                    playercnt += 1;
                    Instantiate(recordListItemPrefab, recordListContent).GetComponent<RecordListItem>().SetUp(playercnt,record);
                }
                gameObject.SetActive(false);
                TimeRecord.SetActive(true);
            }
            
        }
    }

    private void Update()
    {
        /*
        if (playercnt == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            TimeRecord.SetActive(true);
            EndBtn.SetActive(false);
        }
        */
    }
}
