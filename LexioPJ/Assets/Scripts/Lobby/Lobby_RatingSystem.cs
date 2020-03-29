using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Lobby_RatingSystem : MonoBehaviour
{
    public GameObject ratingResult;
    public GameObject scroll;
    public Text now;
    public List<PlayerLeaderboardEntry> GetWins()
    {
        GetLeaderboardRequest request = new GetLeaderboardRequest();
        request.MaxResultsCount = 10;
        request.StatisticName = "Win";

        List<PlayerLeaderboardEntry> temp = new List<PlayerLeaderboardEntry>();
        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            temp = result.Leaderboard;
            print(temp.Count);
        }, error => { });

        if (temp.Count < 1)
            return null;
        return temp;
    }
    public List<PlayerLeaderboardEntry> GetMoneys()
    {
        GetLeaderboardRequest request = new GetLeaderboardRequest();
        request.MaxResultsCount = 10;
        request.StatisticName = "Money";

        List<PlayerLeaderboardEntry> temp = new List<PlayerLeaderboardEntry>();
        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            temp = result.Leaderboard;
        }, error => { });

        if (temp.Count < 1)
            return null;
        return temp;
    }

    public void SetWins(int win, int lose)
    {

        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest();
        List<StatisticUpdate> listUpdate = new List<StatisticUpdate>();
        StatisticUpdate su = new StatisticUpdate();
        su.StatisticName = "WinPercent";
        su.Value = (int)(((double)win / (win + lose)) * 100);
        listUpdate.Add(su);
        request.Statistics = listUpdate;
        PlayFabClientAPI.UpdatePlayerStatistics(request, result =>
        {
            Debug.Log("win have been set!");
        }, error => { });
    }

    public void SetMoney(int money)
    {

        UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest();
        List<StatisticUpdate> listUpdate = new List<StatisticUpdate>();
        StatisticUpdate su = new StatisticUpdate();
        su.StatisticName = "Money";
        su.Value = money;
        listUpdate.Add(su);
        request.Statistics = listUpdate;
        PlayFabClientAPI.UpdatePlayerStatistics(request, result =>
        {
            Debug.Log("win have been set!");
        }, error => { });
    }

    public void OnClick_GetLeaderboard()
    {
        if(now.text == "승률")
        {
            GetLeaderBoard_Money();
            now.text = "머니";
        }else if(now.text == "머니")
        {
            GetLeaderBoard_Win();
            now.text = "승률";
        }
    }

    public void OnClick_RefreshLeaderboard()
    {
        if (now.text == "승률")
        {
            GetLeaderBoard_Win();
        }
        else if (now.text == "머니")
        {
            GetLeaderBoard_Money();          
        }
    }

    public void GetLeaderBoard_Win()
    {
        if(scroll.transform.childCount != 0)
        {
            Lobby_Rating[] ratings = FindObjectsOfType<Lobby_Rating>();
            foreach (Lobby_Rating rating in ratings)
                Destroy(rating.gameObject);
        }
        GetLeaderboardRequest request = new GetLeaderboardRequest
        {
            MaxResultsCount = 10, StartPosition = 0, StatisticName = "WinPercent"
        };
        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            List<PlayerLeaderboardEntry> temp = result.Leaderboard;
            for (int i = 0; i < temp.Count; i++)
            {
                GameObject obj = Instantiate(ratingResult);
                obj.transform.SetParent(scroll.transform);
                obj.GetComponent<Lobby_Rating>().SetText((temp[i].Position + 1).ToString(), temp[i].DisplayName, temp[i].StatValue + "%");
            }
        }, error => { });

    }
    public void GetLeaderBoard_Money()
    {
        if (scroll.transform.childCount != 0)
        {
            Lobby_Rating[] ratings = FindObjectsOfType<Lobby_Rating>();
            foreach (Lobby_Rating rating in ratings)
                Destroy(rating.gameObject);
        }
        GetLeaderboardRequest request = new GetLeaderboardRequest
        {
            MaxResultsCount = 10,
            StartPosition = 0,
            StatisticName = "Money"
        };
        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            List<PlayerLeaderboardEntry> temp = result.Leaderboard;
            for (int i = 0; i < temp.Count; i++)
            {
                GameObject obj = Instantiate(ratingResult);
                obj.transform.SetParent(scroll.transform);
                obj.GetComponent<Lobby_Rating>().SetText((temp[i].Position + 1).ToString(), temp[i].DisplayName, RewriteMoneyText(temp[i].StatValue));
            }
        }, error => { });

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
}
