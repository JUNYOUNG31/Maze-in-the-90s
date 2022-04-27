using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//���� ��� ���
using Photon.Realtime;
using System.Runtime.InteropServices;
using UnityEngine.Networking;

public class LeaveGame : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;

    [DllImport("__Internal")]
    private static extern void RemoveCam();
    private string uid;
    [System.Serializable]
    public class FormData
    {
        public string data;
    }

    IEnumerator Exit_in()
    {
        IEnumerator DelData()
        {
            uid = FirebaseWebGL.Examples.Auth.LoginHandler.UserUid;
            FormData data1 = new FormData();
            data1.data = uid;
            string data2 = JsonUtility.ToJson(data1);
            // string GetDataUrl = $"https://j6e101.p.ssafy.io/recog/detect/{uid}/delete";
            string GetDataUrl = $"http://127.0.0.1:8000/recog/detect/{uid}/delete";
            using (UnityWebRequest request = UnityWebRequest.Post(GetDataUrl, data2))
            {
                yield return request.Send();
                if (request.isNetworkError || request.isHttpError) //�ҷ����� ���� ��
                {
                    Debug.Log(request.error);
                }
                else
                {
                    if (request.isDone)
                    {
                        Debug.Log(data1.data + "�����Ϸ�");
                    }
                }
            }
        }
        yield return StartCoroutine(DelData());
        RemoveCam();
    }
    public void Exit_to()
    {
        StartCoroutine(Exit_in());

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LeaveTutorial()
    {
        Exit_to();
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void LeaveRoom() // ���� ����
    {
        PhotonNetwork.LeaveRoom();//�涰���� ���� ��Ʈ��ũ ���
        Exit_to();        
    }

    public override void OnLeftRoom()//���� ������ ȣ��
    {
        PhotonNetwork.LoadLevel("Lobby");// Lobby �� �ҷ�����
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//������ �� ����Ʈ ���
    {
        foreach (Transform trans in roomListContent)//�����ϴ� ��� roomListContent
        {
            Destroy(trans.gameObject);//�븮��Ʈ ������Ʈ�� �ɶ����� �������
        }
        for (int i = 0; i < roomList.Count; i++)//�氹����ŭ �ݺ�
        {
            if (roomList[i].RemovedFromList)//����� ���� ��� ���Ѵ�. 
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            //instantiate�� prefab�� roomListContent��ġ�� ������ְ� �� �������� i��° �븮��Ʈ�� �ȴ�. 
        }
    }
}
