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

        [Header("Post Game Data References")]
        [SerializeField]
        private TMP_InputField gameMode;
        [SerializeField]
        private TMP_InputField gameMap;
        [SerializeField]
        private TMP_InputField gamerName;
        [SerializeField]
        private TMP_InputField timeSpent;

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
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                DisplayError("Webgl �÷����� �ƴϸ� Javascript ����� �νĵ��� �ʽ��ϴ�.");
                return;
            }

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

        public void ClearUI()
        {
            loginUI.SetActive(false);
            resetPwUI.SetActive(false);
            emailSentUI.SetActive(false);
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


        public void RegisterScreen()
        {
            GameManager.instance.ChangeScene("SignUp");
        }

        public void LobbyScreen()
        {
            GameManager.instance.ChangeScene("Lobby");
        }

        public void RankingScreen()
        {
            GameManager.instance.ChangeScene("Ranking");
        }
    }

}