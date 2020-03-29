using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using Photon.Realtime;

public class Lobby_UserInfo : MonoBehaviour
{
    public LobbyNetwork LobbyNetwork;
    public Text NickNameText;
    public Text WinLoseText;
    public Text MoneyText;
    public Lobby_ChatService Lobby_ChatService;
    private string NickName;
    private int win;
    private int lose;
    public int chargeCount { get; set; }
    public int Day { get; set; }
    public int money { get; set; }

    public string myID;
    public Lobby_Charge Lobby_Charge;
    public Lobby_RatingSystem Lobby_RatingSystem;
    private void Start()
    {     
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetUsernameResult, OnGetUsernameError);              
    }
    private string RewriteMoneyText(int n)
    {
        string s = "";
        string auk = "억";
        string man = "만";
        string won = "원";

        if (n == 100000000)
            return "1" + auk + won;
        if (n == 10000)
            return "1" + man + won;
        if (n == 0)
            return "0" + won;
        int div1 = n / 100000000;
        int rem1 = n % 100000000;
        int div2 = rem1 / 10000;
        int rem2 = rem1 % 10000;
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
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "Win", win.ToString() }, { "Lose", lose.ToString() }, { "Money", money.ToString() }, { "Day", System.DateTime.Now.Day.ToString() } }, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request, (result) => {
            
        }, (error) => print("데이터 저장 실패"));
    }

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = myID };
        PlayFabClientAPI.GetUserData(request, (result) =>
        {
            if (result.Data.Count != 0)
            {
                if (result.Data.ContainsKey("Win")) { win = int.Parse(result.Data["Win"].Value); }
                if (result.Data.ContainsKey("Lose")) { lose = int.Parse(result.Data["Lose"].Value); }
                if (result.Data.ContainsKey("Money")) { money = int.Parse(result.Data["Money"].Value); }
                if (result.Data.ContainsKey("Day")) { Day = int.Parse(result.Data["Day"].Value); }
                if (result.Data.ContainsKey("ChargeCount")) { chargeCount = int.Parse(result.Data["ChargeCount"].Value); }
                int nowDay = System.DateTime.Now.Day;
                if (nowDay != Day)
                {
                    chargeCount = 10;
                    var request1 = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "ChargeCount", "10" } } };
                    PlayFabClientAPI.UpdateUserData(request1, (result1) => {
                        Lobby_Charge.remainCountText.text = "남은 횟수 : 10회";
                    }, (error) => print("데이터 저장 실패"));
                }

            }
            else
            {
                money = 10000;
                chargeCount = 10;
                SetData();
                SetChargeCount();
            }
            WinLoseText.text = string.Format("{0}승 {1}패", win, lose);
            MoneyText.text = RewriteMoneyText(money);
            Lobby_Charge.remainCountText.text = "남은 횟수 : " + chargeCount + "회";
            Lobby_RatingSystem.SetWins(win, lose);
            Lobby_RatingSystem.SetMoney(money);
            Lobby_RatingSystem.GetLeaderBoard_Win();
        }, (error) => print("데이터 불러오기 실패"));
    }

    private void OnGetUsernameResult(GetAccountInfoResult obj)
    {
        NickName = obj.AccountInfo.TitleInfo.DisplayName;
        NickNameText.text = NickName;
        PhotonNetwork.LocalPlayer.NickName = NickName;
        myID = obj.AccountInfo.PlayFabId;
        GetData();
        Lobby_ChatService.SetName(NickName);
        
    }

    private void OnGetUsernameError(PlayFabError obj)
    {
        
    }

    public void SetChargeCount()
    {
        if (chargeCount <= 0)
            chargeCount = 0;
        var request1 = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "ChargeCount", chargeCount.ToString() } } };
        PlayFabClientAPI.UpdateUserData(request1, (result1) => {

        }, (error) => print("데이터 저장 실패"));
    }

    public void Charge(int n)
    {
        money += n;
        chargeCount--;
        SetChargeCount();
        SetData();
        GetData();
        Lobby_Charge.remainCountText.text = "남은 횟수 : " + chargeCount + "회";
    }
}
