using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//���� ��� ���
using Photon.Realtime;


public class LeaveGame : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveRoom() // ���� ����
    {
        PhotonNetwork.LeaveRoom();//�涰���� ���� ��Ʈ��ũ ���
    }

    public override void OnLeftRoom()//���� ������ ȣ��
    {
        PhotonNetwork.LoadLevel("Lobby");// Lobby �� �ҷ�����
    }
}
