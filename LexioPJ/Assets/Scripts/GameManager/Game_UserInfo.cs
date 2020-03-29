using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;

public class Game_UserInfo : MonoBehaviourPun
{
    public Text NickNameText;
    public Text WinLoseText;
    public Text MoneyText;

    private string NickName;
    public int win { get; set; }
    public int lose { get; set; }
    public int money { get; set; }

    public string myID;
    int nextWin;
    int nextLose;
    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetUsernameResult, OnGetUsernameError);
    }

    public void SetData()
    {
        if (money <= 0)
            money = 0;
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "Win", win.ToString() }, { "Lose", lose.ToString() }, { "Money", money.ToString() } }, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("데이터 저장 성공"), (error) => print("데이터 저장 실패"));
    }

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = myID };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            if (result.Data != null)
            {
                if (result.Data.ContainsKey("Win")) { win = int.Parse(result.Data["Win"].Value); nextWin = win + 1; } ;
                if (result.Data.ContainsKey("Lose")) { lose = int.Parse(result.Data["Lose"].Value); nextLose = lose + 1; };
                if (result.Data.ContainsKey("Money")) money = int.Parse(result.Data["Money"].Value);
            }

        }, (error) => print("데이터 불러오기 실패"));
    }

    private void OnGetUsernameResult(GetAccountInfoResult obj)
    {
        NickName = obj.AccountInfo.TitleInfo.DisplayName;
        myID = obj.AccountInfo.PlayFabId;
        GetData();
    }

    private void OnGetUsernameError(PlayFabError obj)
    {

    }
}
