using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;//�ؽ�Ʈ �޽� ���� ��� ���
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    public RoomInfo info;
    public void SetUp(RoomInfo _info)// ������ �޾ƿ���
    {
        info = _info;
        text.text = _info.Name;
    }

    // Update is called once per frame
    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);//��ó��ũ��Ʈ �޼���� JoinRoom ����
    }
}
