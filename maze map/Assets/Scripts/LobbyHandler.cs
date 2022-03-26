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
        //[SerializeField]
        //3. ��й�ȣ ����������
        //private GameObject changePasswordUI;
        [SerializeField]
        //Ȯ��������
        private GameObject actionSuccessPanelUI;
        [SerializeField]
        //4. ȸ�� Ż��
        private GameObject deleteUserConfirmUI;
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

        private void getUsername(string _username)
        {
            userName = _username;

            //�� �� ���̸� �ε������� ����
            if (userName != null && photoURL != null)
            {
                LoadProfile();
            }
        }

        private void getPhotoURL(string _photoURL)
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
            yield return request.SendWebRequest();

            if (request.error != null)
            {
                string output = "�� �� ���� ������ �߻��Ͽ����ϴ� �ٽ� �õ����ּ���";

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
            //changePasswordUI.SetActive(false);
            actionSuccessPanelUI.SetActive(false);
            deleteUserConfirmUI.SetActive(false);
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


        //3. ��� ����
        //public void ChangePwUI()
        //{
        //    ClearUI();
        //    changePasswordUI.SetActive(true);
        //}

        //public void ChangePwSuccess()
        //{
        //    ClearUI();
        //    actionSuccessPanelUI.SetActive(true);
        //    actionSuccessText.text = "��й�ȣ�� ���������� ����Ǿ����ϴ�";
        //}

        //public void SubmitNewPwButton()
        //{
        //    UpdateProfilePicture(profilePictureLink.text);
        //}

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

        public void DeleteUser() =>
           FirebaseAuth.DeleteUser();

        public void SignOut() =>
           FirebaseAuth.SignOut();

        public void LoginScreen()
        {
            GameManager.instance.ChangeScene("Login");
        }

    }

}