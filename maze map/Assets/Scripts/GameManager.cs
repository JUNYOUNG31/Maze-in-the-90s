using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    public static Dictionary<string, string> records = new Dictionary<string, string>();
    public bool isConnect = false;
    void Start()
    {
        isConnect = true;
        StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        yield return new WaitUntil(() => isConnect);
        Vector3 pos = new Vector3(-1030 + Random.Range(-150, 150) * 1.0f, 800 + Random.Range(-80, 80) * 1.0f, 0.0f);
        GameObject playerTemp = PhotonNetwork.Instantiate("Witch3", pos, Quaternion.identity, 0);
    }

    void Update()
    {
        
    }
}
