using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    [Header("Firebase")]
    public FirebaseAuth auth;
    public FirebaseUser user;
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

    [Header("Register References")]
    [SerializeField]
    private TMP_InputField registerUsername;
    [SerializeField]
    private TMP_InputField registerEmail;
    [SerializeField]
    private TMP_InputField registerPassword;
    [SerializeField]
    private TMP_InputField registerConfirmPassword;
    [SerializeField]
    private TMP_Text registerNameErrorText;
    [SerializeField]
    private TMP_Text registerEmailErrorText;
    [SerializeField]
    private TMP_Text registerPasswordErrorText;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(CheckAndFixDependencies());
    }

    private IEnumerator CheckAndFixDependencies()
    {
        var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

        yield return new WaitUntil(predicate: () => checkAndFixDependenciesTask.IsCompleted);

        var dependencyResult = checkAndFixDependenciesTask.Result;

        if (dependencyResult == DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyResult}");
        }
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        StartCoroutine(CheckAutoLogin());

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

    }

    private IEnumerator CheckAutoLogin()
    {
        yield return new WaitForEndOfFrame();
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(predicate: () => reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    private void AutoLogin()
    {
        if (user != null)
        {
            if (user.IsEmailVerified)
            {
                GameManager.instance.ChangeScene("MyPage");
            }
            else
            {
                StartCoroutine(SendVerificationEmail());
            }
        }
        else
        {
            AuthUIManager.instance.LoginScreen();
        }
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("�α׾ƿ���!!");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log($"�α��ε�: {user.DisplayName}");
            }
        }
    }

    public void ClearOutPuts()
    {
        emailErrorText.text = "";
        passwordErrorText.text = "";
        registerNameErrorText.text = "";
        registerEmailErrorText.text = "";
        registerPasswordErrorText.text = "";
    }

    public void LoginButton()
    {
        StartCoroutine(LoginLogic(loginEmail.text, loginPassword.text));
    }

    public void RegisterButton()
    {
        StartCoroutine(RegisterLogic(registerUsername.text, registerEmail.text, registerPassword.text, registerConfirmPassword.text));
    }

    private IEnumerator LoginLogic(string _email, string _password)
    {
        Credential credential = EmailAuthProvider.GetCredential(_email, _password);

        var loginTask = auth.SignInWithCredentialAsync(credential);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseException = (FirebaseException)loginTask.Exception.GetBaseException();
            AuthError error = (AuthError)firebaseException.ErrorCode;
            string output = "�� �� ���� ������ �߻��Ͽ����ϴ� �ٽ� �õ����ּ���";
            string pwoutput = "";

            switch (error)
            {
                case AuthError.MissingEmail:
                    output = "�̸����� �Է����ּ���";
                    break;
                case AuthError.InvalidEmail:
                    output = "�߸��� �̸��� �����Դϴ�";
                    break;
                case AuthError.MissingPassword:
                    output = "";
                    pwoutput = "��й�ȣ�� �Է����ּ���";
                    break;
                case AuthError.WrongPassword:
                    output = "";
                    pwoutput = "��й�ȣ�� Ʋ���ϴ�";
                    break;
                case AuthError.UserNotFound:
                    output = "�������� �ʴ� �����Դϴ�";
                    break;
            }
           emailErrorText.text = output;
           passwordErrorText.text = pwoutput;
        }
        else
        {
            if (user.IsEmailVerified)
            {
                yield return new WaitForSeconds(1f);
                GameManager.instance.ChangeScene("MyPage");
            }
            else
            {
                StartCoroutine(SendVerificationEmail());
            }
        }
    }

    private IEnumerator RegisterLogic(string _username, string _email, string _password, string _confirmPassword)
    {
        if (_username == "")
        {
            registerNameErrorText.text = "�г����� �Է����ּ���";
        }
        else if (_password != _confirmPassword)
        {
            registerPasswordErrorText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�";
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)registerTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;
                string output = "�� �� ���� ������ �߻��Ͽ����ϴ� �ٽ� �õ����ּ���";
                string pwoutput = "";
                string emailoutput = "";

                switch (error)
                {
                    case AuthError.InvalidEmail:
                        emailoutput = "�߸��� �̸��� �����Դϴ�";
                        output = "";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        emailoutput = "�̹� ��� ���� �̸����Դϴ�";
                        output = "";
                        break;
                    case AuthError.WeakPassword:
                        pwoutput = "��й�ȣ�� �ּ� 6�ڸ��� ������ּ���";
                        output = "";
                        break;
                    case AuthError.MissingEmail:
                        emailoutput = "�̸����� �Է����ּ���";
                        output = "";
                        break;
                    case AuthError.MissingPassword:
                        pwoutput = "��й�ȣ�� �Է����ּ���";
                        output = "";
                        break;
                }
                registerNameErrorText.text = output;
                registerEmailErrorText.text = emailoutput;
                registerPasswordErrorText.text = pwoutput;
            }
            else
            {
                UserProfile profile = new UserProfile
                {
                    DisplayName = _username,

                    //TODO: Give Profile Default Photo
                    PhotoUrl = new System.Uri("https://pbs.twimg.com/media/EFKdt0bWsAIfcj9.jpg"),
                };

                var defaultUserTask = user.UpdateUserProfileAsync(profile);

                yield return new WaitUntil(predicate: () => defaultUserTask.IsCompleted);

                if (defaultUserTask.Exception != null)
                {
                    user.DeleteAsync();
                    FirebaseException firebaseException = (FirebaseException)defaultUserTask.Exception.GetBaseException();
                    AuthError error = (AuthError)firebaseException.ErrorCode;
                    string output = "�� �� ���� ������ �߻��Ͽ����ϴ� �ٽ� �õ����ּ���";

                    switch (error)
                    {
                        case AuthError.Cancelled:
                            output = "������Ʈ ��ҵ�";
                            break;
                        case AuthError.SessionExpired:
                            output = "������ �����";
                            break;
                    }
                    registerNameErrorText.text = output;
                }
                else
                {
                    Debug.Log($"���� ���� ����: {user.DisplayName} ({user.UserId})");

                    StartCoroutine(SendVerificationEmail());
                }
            }
        }
    }

    private IEnumerator SendVerificationEmail()
    {
        if (user != null)
        {
            var emailTask = user.SendEmailVerificationAsync();

            yield return new WaitUntil(predicate: () => emailTask.IsCompleted);

            if (emailTask.Exception != null)
            {
                FirebaseException firebaseException = (FirebaseException)emailTask.Exception.GetBaseException();
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string output = "�� �� ���� ������ �߻��Ͽ����ϴ� �ٽ� �õ����ּ���";

                switch (error)
                {
                    case AuthError.Cancelled:
                        output = "������ ��ҵǾ����ϴ�";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        output = "�߸��� �̸��� �����Դϴ�";
                        break;
                    case AuthError.TooManyRequests:
                        output = "��û�� ����ġ�� �����ϴ�";
                        break;
                }

                AuthUIManager.instance.AwaitVerification(false, user.Email, output);
            }
            else
            {
                AuthUIManager.instance.AwaitVerification(true, user.Email, null);
                Debug.Log("�̸����� ���������� ���۵Ǿ����ϴ�");
            }
        }
    }

    public void UpdateProfilePicture(string _newPfpURL)
    {
        StartCoroutine(UpdateProfilePictureLogic(_newPfpURL));
    }

    private IEnumerator UpdateProfilePictureLogic(string _newPfpURL)
    {
        if (user != null)
        {
            UserProfile profile = new UserProfile();

            try
            {
                UserProfile _profile = new UserProfile
                {
                    PhotoUrl = new System.Uri(_newPfpURL),
                };

                profile = _profile;
            }
            catch
            {
                MyPageManager.instance.Output("�̹����� �ҷ��� �� �����ϴ�. ��ȿ�� ��ũ���� Ȯ�����ּ���");
                yield break;
            }

            var pfpTask = user.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(predicate: () => pfpTask.IsCompleted);

            if (pfpTask.Exception != null)
            {
                Debug.LogError($"������ ���� ���濡 �����߽��ϴ�: {pfpTask.Exception}");
            }
            else
            {
                MyPageManager.instance.ChangePfpSuccess();
                Debug.Log("������ ������ ���������� ����Ǿ����ϴ�");
            }
        }
    }

    public void SignOut()
    {
        //Signs out of Firebase
        auth.SignOut();
        //Go back to Login UI
        GameManager.instance.ChangeScene("Auth");
    }
}
