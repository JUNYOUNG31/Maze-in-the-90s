using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks//�ٸ� ���� ���� �޾Ƶ��̱�
{
    [SerializeField] TMP_Text text;
    Player player;//���� ����Ÿ���� Player�� ���� �� �� �ְ� ���ش�.
    public GameObject dropbox;
    public static int char_index;
    public void OnDropdownEvent(int index)
    {
        char_index = index;
        Debug.Log(char_index);
    }

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;//�÷��̾� �̸� �޾Ƽ� �׻�� �̸��� ��Ͽ� �߰� ������ش�. 

        if (_player.NickName != PhotonNetwork.NickName)
        {
            TMP_Dropdown tMP_Dropdown = dropbox.GetComponent<TMP_Dropdown>();
            tMP_Dropdown.enabled = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)//�÷��̾ �涰������ ȣ��
    {
        if (player == otherPlayer)//���� �÷��̾ ����?
        {
            Destroy(gameObject);//�̸�ǥ ����
        }
    }

    public override void OnLeftRoom()//�� ������ ȣ��
    {
        Destroy(gameObject);//�̸�ǥ ȣ��
    }
}