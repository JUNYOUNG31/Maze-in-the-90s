using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MiniJSON;


public class RankingHandler : MonoBehaviour
{
        
    public static RankingHandler instance;
    //�ؿ� ������ ���� ������Ʈ
    [SerializeField] Transform recordListContent;
    //������
    [SerializeField] GameObject gameRecordPrefab;

    //JSON���� ��ȯ�� ���ӱ�ϵ�����(���̾�̽��� ���� ��)
    [Serializable]
    public class gameRecord
    {
        public int gameMode;       
        public string gameMap;
        public string nickName;
        public float time;
    }

    //������Ʈ�� ��ȯ�� ���ӱ�ϵ�����(���̾�̽����� �޾��� ��)
    [Serializable]
    public class saveRecord
    {
        public string nickName;
        public string time;
    }

    int startIdx = 0;

    public void Update()
    {
        
    }

    //������ ������ �� EndGamd���� ���� ���ӵ�����
    public void GetGameData(KeyValuePair<string, string> _data)
    {
        //{name: ��¼��, time: ��¼��} �̷��� ����
        //string���� �޾Ƽ� float �� ��ȯ
        float recordToFlaot = (float.Parse(_data.Value));
        SendGameRecord(_data.Key, recordToFlaot);
    }

    //���ӵ����� ����, JSON ��ȯ, ����
    public void SendGameRecord(string username, float time)
    {
        gameRecord gameRecordObject = new gameRecord();

        gameRecordObject.gameMode = 1;
        gameRecordObject.gameMap = "forest1";
        gameRecordObject.nickName = username;
        gameRecordObject.time = time;

        string json = JsonUtility.ToJson(gameRecordObject);

        //���̾�̽��� ����
        FirebaseDatabase.PostGameRecord(json);
    }

    //TOP10 �����͸� �޾Ƽ� ��ŷ ���� ����(�� ������ ��)
    public void SetUp(string record)
    {
        var text = "";

        //JSON ���ڿ� ���¿��� �ٽ� Deserialize
        Dictionary<string, object> response = Json.Deserialize(record) as Dictionary<string, object>;

        //�ε����� ����Ͽ� �ð� ���� ����
        string time = response["time"].ToString();

        //�ð��� 12:11 �̷� �������� ��ȯ
        //string time1 = time.Substring(0, 4);

        for (int i=0; i<10; i++)
        {
            if(time[i] == '.')
            {
                text += ':';

                for (int j = i + 1; j < i + 3; j++)
                {
                    text += time[j];
                }
                break;
            }
            else
            {
                text += time[i];
            }
        }

        response["time"] = text;

        //������ �߰�
        response["idx"] = (startIdx + 1).ToString();
        startIdx += 1;

        //Debug
        Debug.Log(response["name"]);
        Debug.Log(response["time"]);
        Debug.Log(response["idx"]);

        Debug.Log(response["name"].GetType().Name);
        Debug.Log(response["time"].GetType().Name);
        Debug.Log(response["idx"].GetType().Name);

        //RecordListItem.cs�� ������
        Instantiate(gameRecordPrefab, recordListContent).GetComponent<RankListItem>().SetUp(response);
    }

    //�� Ŭ�� (���, �ʸ��� �ٸ� ��û)
    public void ChangeTab(int mode, string map)
    {
        //��ŷ �ε��� �ʱ�ȭ
        startIdx = 0;
        FirebaseDatabase.SetByInfo(mode, map);
    }


    public void LobbyorLoginScreen()
    {
        //�α��� �������� Ȯ��
        FirebaseAuth.IsLoggedIn();
    }

    public void BackBtn(int status)
    {
        if (status == 1)
        {
            GameManager.instance.ChangeScene("Lobby");
        }

        else
        {
            GameManager.instance.ChangeScene("Login");
        }
    }
}
