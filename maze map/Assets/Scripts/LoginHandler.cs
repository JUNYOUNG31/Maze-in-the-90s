using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FirebaseWebGL.Examples.Auth
{
    public class LoginHandler : MonoBehaviour
    {
        public static LoginHandler instance;
        public static string UserUid;

        [Header("UI References")]
        [SerializeField]
        //1. �α���
        private GameObject loginUI;
        [SerializeField]
        //2. ��й�ȣ �缳��
        private GameObject resetPwUI;
        [SerializeField]
        //3. �̸��� ���� Ȯ��
        private GameObject emailSentUI;
        [SerializeField]
        //4. �г�üũ
        private GameObject checkNicknameUI;
        [Space(5f)]

        [Header("Login References")]
        [SerializeField]
        private TMP_InputField loginEmail;
        [SerializeField]
        private TMP_InputField loginPassword;
        [SerializeField]
        private TMP_Text emailErrorText;
        [SerializeField]
        private TMP_Text passwordErrorText;
        [Space(5f)]

        [Header("Reset Password References")]
        [SerializeField]
        private TMP_InputField resetPwEmail;
        [SerializeField]
        private TMP_Text resetEmailErrorText;
        [Space(5f)]

        [Header("Email Sent References")]
        [SerializeField]
        private TMP_Text usedEmail;
        public TMP_Text statusText;
        [Space(5f)]

        [Header("Nickname References")]
        [SerializeField]
        private TMP_InputField checkUsernameText;
        [SerializeField]
        private TMP_Text outputText;
        [Space(5f)]

        public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        //�̸��� ��� ��ȿ�� �˻�
        public static bool ValidateEmail(string email)
        {
            if (email != null)
                return Regex.IsMatch(email, MatchEmailPattern);
            else
                return false;
        }

        private void Start()
        {
            CheckAutoLogin();
        }

        private void DisplayError(string errortext)
        {
            statusText.text = errortext;
        }

        private void DisPlayInfo(string Infotext)
        {
            statusText.text = Infotext;
        }

        public void SignUpNicknameCheck()
        {
            CheckNickUI();
        }

        public void ClearUI()
        {
            loginUI.SetActive(false);
            resetPwUI.SetActive(false);
            emailSentUI.SetActive(false);
            checkNicknameUI.SetActive(false);
            checkUsernameText.text = "";
        }

        public void CheckNickUI()
        {
            ClearUI();
            checkNicknameUI.SetActive(true);
        }

        public void LoginScreen()
        {
            ClearUI();
            loginUI.SetActive(true);
        }

        public void ResetPwScreen()
        {
            ClearUI();
            resetPwUI.SetActive(true);
        }

        public void EmailSentScreen(string email)
        {
            ClearUI();
            emailSentUI.SetActive(true);
            usedEmail.text = email;

        }

        public void ResetPw()
        {
            if (ValidateEmail(resetPwEmail.text) != false)
            {
                ResetPassword(resetPwEmail.text);
            }
            else
            {
                resetEmailErrorText.text = "��ȿ�� �̸����� �ƴϰų� ��ġ�ϴ� ����ڰ� �����ϴ�!";
            }
        }

        private void CheckedNameForSocial(int result)
        {

            if (result == 0)
            {
                outputText.text = "����� �� ���� �г����Դϴ�";
            }
            else if (result == 1)
            {
                outputText.text = "��� ������ �г����Դϴ�";
            }
            else if (result == 3)
            {
                outputText.text = "�г����� �Է����ּ���";
            }
        }


        public void SignWithEmailAndPassword() =>
            FirebaseAuth.SignInWithEmailAndPassword(loginEmail.text, loginPassword.text, gameObject.name, "DisPlayInfo", "DisplayError");

        public void LoginWithGoogle() =>
            FirebaseAuth.LoginWithGoogle(gameObject.name, "DisPlayInfo", "DisplayError");

        public void LoginWithGithub() =>
            FirebaseAuth.LoginWithGithub(gameObject.name, "DisPlayInfo", "DisplayError");

        public void CheckAutoLogin() =>
            FirebaseAuth.CheckAutoLogin();

        public void ResetPassword(string email) =>
            FirebaseAuth.ResetPassword(email);

        public void CheckNicknameForSocial() =>
           FirebaseDatabase.CheckNicknameForSocial(checkUsernameText.text);


        public void CheckComplete()
        {
            if (outputText.text == "��� ������ �г����Դϴ�")
            {
                FirebaseAuth.UpdateInfoWithGoogleOrGithub(checkUsernameText.text);
            }

        }


        public void RegisterScreen()
        {
            GameManager.instance.ChangeScene("SignUp");
        }

        public void LobbyScreen(string uid)
        {
            UserUid = uid;
            Debug.Log("from login to lobby");
            Debug.Log(uid);
            GameManager.instance.ChangeScene("Lobby");
        }

        public void RankingScreen()
        {
            GameManager.instance.ChangeScene("Ranking");
        }
    }

}