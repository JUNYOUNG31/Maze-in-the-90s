using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace FirebaseWebGL.Examples.Auth
{
    public class LobbyHandler : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        //1. ����������
        private GameObject profileUI;
        [SerializeField]
        //2. ���� ����������
        private GameObject changePfpUI;
        [SerializeField]
        //3. ��й�ȣ ����
        private GameObject changePasswordUI;
        [SerializeField]
        //4. ȸ�� Ż��
        private GameObject deleteUserConfirmUI;
        [SerializeField]
        //5. Ȯ��������
        private GameObject actionSuccessPanelUI;
        [Space(5f)]

        [Header("Basic Info References")]
        [SerializeField]
        private TMP_Text lobbyUsernameText;
        [SerializeField]
        private Text myPageUsernameText;
        [Space(5f)]

        [Header("Profile Picture References")]
        [SerializeField]
        private Image lobbyProfilePicture;
        [SerializeField]
        private Image myPageProfilePicture;
        [SerializeField]
        private TMP_InputField profilePictureLink;
        [SerializeField]
        private TMP_Text outputText;

        [Header("Change Password References")]
        [SerializeField]
        private TMP_InputField changePasswordInputField;
        [SerializeField]
        private TMP_InputField changePasswordConfirmInputField;
        [SerializeField]
        private TMP_Text pwErrorText;
        [Space(5f)]

        [Header("Action Success Panel References")]
        [SerializeField]
        private TMP_Text actionSuccessText;

        //�г���, ����
        string userName = null;
        string photoURL = null;


        private void Start()
        {
            //���� ���� �� ������ �ε�
            CheckAuthState();
        }

        private void GetUsername(string _username)
        {
            userName = _username;

            //�� �� ���̸� �ε������� ����
            if (userName != null && photoURL != null)
            {
                LoadProfile();
            }
        }

        private void GetPhotoURL(string _photoURL)
        {
            photoURL = _photoURL;

            //�� �� ���̸� �ε������� ����
            if (userName != null && photoURL != null)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            //Set UI
            StartCoroutine(LoadImage(photoURL.ToString()));
            lobbyUsernameText.text = userName.ToString();
            myPageUsernameText.text = userName.ToString();
        }

        private IEnumerator LoadImage(string _photoUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(_photoUrl);

            request.SetRequestHeader("Access-Control-Allow-Origin", "*");

            request.SetRequestHeader("Access-Control-Allow-Credentials", "true");
            request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, Content-Type, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
            request.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS");

            yield return request.SendWebRequest();

            if (request.error != null)
            {
                string output = "�� �� ���� ������ �߻��Ͽ����ϴ� �ٽ� �õ����ּ���";
                Debug.Log("�ε��̹��� ����");
                Debug.Log(request.error);

                if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    output = "�������� �ʴ� �̹��� ���� �����Դϴ� �ٸ� �̹����� ����ϼ���";
                }

                Output(output);
            }
            else
            {
                Texture2D image = ((DownloadHandlerTexture)request.downloadHandler).texture;

                lobbyProfilePicture.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);

                myPageProfilePicture.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);
            }
        }

        public void Output(string _output)
        {
            outputText.text = _output;
        }

        public void ClearUI()
        {
            profileUI.SetActive(false);
            changePfpUI.SetActive(false);
            changePasswordUI.SetActive(false);
            actionSuccessPanelUI.SetActive(false);
            deleteUserConfirmUI.SetActive(false);
            actionSuccessText.text = "";
        }

        //1. ����������
        public void ProfileUI()
        {
            ClearUI();
            profileUI.SetActive(true);
            //������ �ٲ��� �� �ٽ� ȣ��
            CheckAuthState();
        }

        //2. ���� ����
        public void ChangePfpUI()
        {
            ClearUI();
            changePfpUI.SetActive(true);
        }

        public void ChangePfpSuccess()
        {
            ClearUI();
            actionSuccessPanelUI.SetActive(true);
            actionSuccessText.text = "������ ������ ���������� ����Ǿ����ϴ�";
        }

        public void SubmitProfileImageButton()
        {
            UpdateProfilePicture(profilePictureLink.text);
        }

        //2. ��� ����
        public void ChangePwUI()
        {
            ClearUI();
            changePasswordUI.SetActive(true);
        }

        public void ChangePwSuccess()
        {
            ClearUI();
            actionSuccessPanelUI.SetActive(true);
            actionSuccessText.text = "��й�ȣ�� ���������� ����Ǿ����ϴ�";
        }

        public void SubmitNewPwButton()
        {
            if ((changePasswordInputField.text == changePasswordConfirmInputField.text) == true)
            {
                UpdatePw(changePasswordInputField.text);
            }
            else
            {
                pwErrorText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�";
            }
        }

        //4. ȸ��Ż��
        public void DeleteUserConfirmUI()
        {
            ClearUI();
            deleteUserConfirmUI.SetActive(true);
        }

        public void DeleteUserSuccess()
        {
            ClearUI();
            actionSuccessPanelUI.SetActive(true);
            actionSuccessText.text = "ȸ��Ż�� �Ϸ�Ǿ����ϴ�";
        }

        public void DeleteUserButton()
        {
            DeleteUser();
        }

        //�α׾ƿ�
        public void SignOutButton()
        {
            SignOut();
        }




        public void CheckAuthState() =>
           FirebaseAuth.CheckAuthState();

        public void UpdateProfilePicture(string newProfile) =>
           FirebaseAuth.UpdateProfilePicture(newProfile);

        public void UpdatePw(string newPw) =>
           FirebaseAuth.UpdatePw(newPw);

        public void DeleteUser() =>
           FirebaseAuth.DeleteUser();

        public void SignOut() =>
           FirebaseAuth.SignOut();


        public void LoginScreen()
        {
            GameManager1.instance.ChangeScene("Login");
        }

    }

}