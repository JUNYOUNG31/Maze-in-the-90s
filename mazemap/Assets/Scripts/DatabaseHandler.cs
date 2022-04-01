using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;


public class DatabaseHandler : MonoBehaviour
{
    public TMP_Text statusText;
    public TMP_InputField pathInputField;
    public TMP_InputField valueInputField;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
    }

    // ������ ���� : �ش� ��ο� ���� �ִ� ���, ������
    public void PostJSON() => FirebaseDatabase.PostJSON(pathInputField.text, valueInputField.text, gameObject.name,
            "DisplayInfo", "DisplayErrorObject");

    // ������ �о����
    public void GetJSON() => FirebaseDatabase.GetJSON(pathInputField.text, gameObject.name,
            "DisplayData", "DisplayErrorObject");

    // ������ ���� : �ش� ��ο� ���� �ִ� ���, UID�� Ű�� ���� �߰���
    public void PushJSON() => FirebaseDatabase.PushJSON(pathInputField.text, valueInputField.text, gameObject.name,
        "DisplayInfo", "DisplayErrorObject");

    // ������ ���� : �ش� ����� ���� ��� �� 1���� ���� �ٲ�
    public void UpdateJSON() => FirebaseDatabase.UpdateJSON(pathInputField.text, valueInputField.text,
        gameObject.name, "DisplayInfo", "DisplayErrorObject");

    // ������ ����
    public void DeleteJSON() => FirebaseDatabase.DeleteJSON(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");


    public void DisplayData(string data)
    {
        statusText.text = data;
    }

    public void DisplayInfo(string info)
    {
        statusText.text = info;
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = JsonUtility.FromJson<FirebaseError>(error);
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        statusText.text = error;
    }
}