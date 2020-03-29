using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using System;

public class Player_UserInfo : MonoBehaviourPun
{
    public Text NickNameText;
    public Text WinLoseText;
    public Text MoneyText;

    private string NickName;
    private int win;
    private int lose;
    private int money;

    public string myID;

    public void AccessInfo(string name)
    {
        var request = new GetAccountInfoRequest
        {
            TitleDisplayName = name
        };
        PlayFabClientAPI.GetAccountInfo(request, OnGetUsernameResult, OnGetUsernameError);
    }
    private void OnGetUsernameError(PlayFabError obj)
    {
        print(obj.GenerateErrorReport());
    }

    private void OnGetUsernameResult(GetAccountInfoResult obj)
    {
        myID = obj.AccountInfo.PlayFabId;
        GetData();
    }

    private string RewriteMoneyText(long n)
    {
        string s = "";
        string auk = "억";
        string man = "만";
        string won = "원";

        long div1 = n / 100000000;
        long rem1 = n % 100000000;
        long div2 = rem1 / 10000;
        long rem2 = rem1 % 10000;
        if (div1 > 0)
        {
            s += div1.ToString() + auk;
        }
        if (div2 > 0)
        {
            s += div2.ToString() + man;
        }
        if (rem2 == 0)
            s += won;
        else
            s += rem2.ToString() + won;
        return s;
    }

    public void SetData()
    {
        
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
                
                if (result.Data.ContainsKey("Win"))  win = int.Parse(result.Data["Win"].Value);
                if (result.Data.ContainsKey("Lose")) lose = int.Parse(result.Data["Lose"].Value);
                if (result.Data.ContainsKey("Money")) money = int.Parse(result.Data["Money"].Value);
               
                WinLoseText.text = string.Format("{0}승 {1}패", win, lose);
                MoneyText.text = RewriteMoneyText(money);
                NickNameText.text = photonView.Owner.NickName;

            }
        }, (error) => print("데이터 불러오기 실패"));
    }

}
