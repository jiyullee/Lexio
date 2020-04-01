using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using Photon.Realtime;

public class Auth_Playfab : MonoBehaviour
{
    public InputField id_Input;
    public InputField PW_Input;
    public InputField NickName_Input;
    public InputField PWChecker;
    public Text PWCheckerText;
    private string nickname;
    private string id;
    private string password;
    public Text ErrorText;
    public Toggle autoLogin;

    public Text id_check;
    public Text pw_check;
    private string _playFabPlayerIdCache;
    private AudioSource buttonSound;
    private void Awake()
    {
        Screen.SetResolution(1280, 720, true);
        if (PlayerPrefs.HasKey("Audio"))
            PlayerPrefs.DeleteKey("Audio");
        buttonSound = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if (!(SceneManager.GetActiveScene().name == "SignIn"))
        {
            return;
        }
        if (PlayerPrefs.HasKey("AutoLogin"))
        {
            if (PlayerPrefs.GetInt("AutoLogin") != 1)
            {
                id_Input.text = "";
                PW_Input.text = "";
                autoLogin.isOn = false; return;
            }

            autoLogin.isOn = true;
            if (autoLogin.isOn)
            {
                if (PlayerPrefs.HasKey("ID"))
                    id_Input.text = PlayerPrefs.GetString("ID");
                if (PlayerPrefs.HasKey("Password"))
                    PW_Input.text = PlayerPrefs.GetString("Password");
                Login();
            }
            
        }
    }
    
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "SignIn")
        {
            if (id_Input.text.Length == 0)
            {
                id_check.text = "아이디를 입력하세요.";
            }
            else if (!(3 <= id_Input.text.Length && id_Input.text.Length <= 10))
            {
                id_check.text = "3글자 이상의 아이디를 입력하세요.";
            }
            else if (!(6 <= PW_Input.text.Length && PW_Input.text.Length <= 100))
            {
                pw_check.text = "6글자 이상의 비밀번호를 입력하세요.";
            }
            else
            {
                id_check.text = "";
                pw_check.text = "";
            }
        }
       
    }
    public void Login()
    {
        var request = new LoginWithPlayFabRequest { Username = id_Input.text, Password = PW_Input.text };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _playFabPlayerIdCache = result.PlayFabId;
        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
        }, AuthenticateWithPhoton, OnPlayFabError);


        Debug.Log("로그인 성공");
        ErrorText.text = "로그인 성공";
        PlayerPrefs.SetString("ID", id_Input.text);
        PlayerPrefs.SetString("Password", PW_Input.text);

        if (autoLogin.isOn)
        {        
            PlayerPrefs.SetInt("AutoLogin", 1);
        }
        else
        {
            PlayerPrefs.SetInt("AutoLogin", 0);
        }

        var request = new GetAccountInfoRequest { Username = id_Input.text };
        PlayFabClientAPI.GetAccountInfo(request, GetAccountSuccess, GetAccountFailure);
    }

    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache); 

        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        PhotonNetwork.AuthValues = customAuth;
    }
    private void OnPlayFabError(PlayFabError obj)
    {
        //LogMessage(obj.GenerateErrorReport());
    }

    private void GetAccountFailure(PlayFabError obj)
    {
        throw new NotImplementedException();
    }

    private void GetAccountSuccess(GetAccountInfoResult obj)
    {
        string nickname = obj.AccountInfo.TitleInfo.DisplayName;
        if (nickname == null)
        {
            PhotonNetwork.LoadLevel("SetNickName");
        }
        else
        {
            PlayerPrefs.SetString("Nickname", nickname);
            
            PhotonNetwork.LoadLevel("Lobby");
        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("로그인 실패");
        Debug.LogWarning(error.GenerateErrorReport());
        ErrorText.text = "로그인 실패";
    }

    public void Register()
    {
        buttonSound.Play();
        if (PWChecker.text != PW_Input.text)
        {
            PWCheckerText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }
        else
        {
            PWCheckerText.text = "비밀번호가 일치합니다.";
        }
        var request = new RegisterPlayFabUserRequest { Username = id_Input.text, Password = PW_Input.text};
        request.RequireBothUsernameAndEmail = false;
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);

    }

    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("가입 성공");
        ErrorText.text = "회원가입 성공!";
    }

    private void RegisterFailure(PlayFabError error)
    {
        if (error.GenerateErrorReport() == "/Client/RegisterPlayFabUser: Username not available" + "\nUsername: User name already exists.")
            ErrorText.text = "회원가입 실패 : 중복된 ID입니다.";
        else
        {
            ErrorText.text = "회원가입 실패";
        }
    }

    public void SetNickName()
    {
        buttonSound.Play();
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = NickName_Input.text };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, DisplayNameUpdateSuccess, DisplayNameUpdateFailure);
    }

    private void DisplayNameUpdateFailure(PlayFabError obj)
    {
        ErrorText.text = "닉네임 생성 실패 : 중복된 이름이거나 글자 수 오류입니다.";
    }

    private void DisplayNameUpdateSuccess(UpdateUserTitleDisplayNameResult obj)
    {
        ErrorText.text = "닉네임 생성 성공";
        PhotonNetwork.LoadLevel("Lobby");
    }

}
