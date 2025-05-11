using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

[Serializable]
public class UI_InputFields
{
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordComfirmInputField;
    public TextMeshProUGUI ResultText;
    public Button ConfirmButton;
}
public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject RegisterPanel;
    [Header("로그인")]
    public UI_InputFields LoginInputField;
    [Header("회원가입")]
    public UI_InputFields RegisterInputField;

    private const string PREFIX = "ID_";
    private const string SALT = "1004000";
    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
        
        LoginCheck();
    }
    //회원가입하기
    public void OnClickGoToRegisterButton()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
        RefreshRegister();
    }

    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }

    public void Register()
    {
        //1. 아이디 입력을 확인한다.
        string id = RegisterInputField.IDInputField.text;
        string password = RegisterInputField.PasswordInputField.text;
        string passwordComfirm = RegisterInputField.PasswordComfirmInputField.text;
        
        if (string.IsNullOrEmpty(id))
        {
            RegisterInputField.ResultText.text = "아이디를 입력해주세요";
            Feedback();
            return;
        }
        //2. 비밀번호 입력을 확인한다.
        if (string.IsNullOrEmpty(password))
        {
            RegisterInputField.ResultText.text = "비밀번호를 입력해주세요";
            Feedback();
            return;
        }
        //3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다.
        if (string.IsNullOrEmpty(passwordComfirm))
        {
            RegisterInputField.ResultText.text = "2차 비밀번호를 입력해주세요";
            Feedback();
            return;
        }
        else
        {
            bool result = password == passwordComfirm;
            if (!result)
            {
                RegisterInputField.ResultText.text = "입력한 비밀번호가 다릅니다.";
                Feedback();
                return;
            }
        }
        // 4. playerprefs를 이용해서 아이디와 비밀번호를 저장한다.
        PlayerPrefs.SetString(PREFIX + id, Encryption(password + SALT));
        LoginInputField.IDInputField.text = id;
        // 5. 로그인 창으로 돌아간다. (이때 아이디는 자동으로 입력되어 있다)
        OnClickGoToLoginButton();
    }

    private void RefreshRegister()
    {
        RegisterInputField.IDInputField.text = "";
        RegisterInputField.PasswordInputField.text = "";
        RegisterInputField.PasswordComfirmInputField.text = "";
        RegisterInputField.ResultText.text = "";
    }

    public string Encryption(string text)
    {
        SHA256 sha256 = SHA256.Create();
        // 운영체제 혹은 프로그래밍 언어별로 string 표현하는 방식이 다 다르므로
        // utf8 버전으로 바꿔주어야한다.
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = String.Empty;
        foreach (byte b in hash)
        {
            resultText += b.ToString("x2");
        }
        
        return resultText;
    }
    private void Feedback()
    {
        RegisterInputField.ResultText.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).OnComplete(() =>
        {
            RegisterInputField.ResultText.rectTransform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        });
    }

    public void LogIn()
    {
        //1. 아이디 입력을 확인한다.
        string id = LoginInputField.IDInputField.text;
        string password = LoginInputField.PasswordInputField.text;
        
        if (string.IsNullOrEmpty(id))
        {
            LoginInputField.ResultText.text = "아이디를 입력해주세요";
            Feedback();
            return;
        }
        //2. 비밀번호 입력을 확인한다.
        if (string.IsNullOrEmpty(password))
        {
            LoginInputField.ResultText.text = "비밀번호를 입력해주세요";
            Feedback();
            return;
        }
        //3. playerprefs.Get을 이용해서 아이디와 비밀번호가 맞는지 확인한다.
        if (!PlayerPrefs.HasKey(PREFIX + id))
        {
            LoginInputField.ResultText.text = "아이디와 비밀번호를 확인해주세요";
            return;
        }
        
        string hashedPassword = PlayerPrefs.GetString(PREFIX + id);
        if (hashedPassword != Encryption(password + SALT))
        {
            LoginInputField.ResultText.text = "아이디와 비밀번호를 확인해주세요";
            return;
        }
        SceneManager.LoadScene(1);
        Debug.Log("로그인 성공");
        // 맞다면 로그인
    }
    public void LoginCheck()
    {
        string id = LoginInputField.IDInputField.text;
        string password = LoginInputField.PasswordInputField.text;
        
        LoginInputField.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password);
    }
}
