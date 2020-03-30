using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviourPun
{
    public static GameOverManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameOverManager>();

            return instance;
        }
    }
    private static GameOverManager instance;
    public LevelLoader LevelLoader;
    public Game_UserInfo Game_UserInfo;
    public int[] havingCardCount = new int[6];
    public int[] sortedCardCount = new int[6];
    private int[] rate = new int[6];
    private int[] sortedByRate = new int[6];
    public GameObject Result;
    public GameObject ResultRayoutGroup;
    public Transform[] spawnPositions;

    Color[] resultColor = { new Color(1, 0.8431373f, 0, 1), new Color(0.7529412f, 0.7529412f, 0.7529412f, 1), new Color(8039216f, 0.4980392f, 0.1960784f, 1), Color.white, Color.white, Color.white };
    string betting;
    int bettingAmount;
    float lifeTime = 5;
    public Text lifeTimeText;
    public bool isWin;
    int count = 0;
    private void Start()
    {
        betting = (string)PhotonNetwork.CurrentRoom.CustomProperties["Betting"];
        if (betting == "백원")
            bettingAmount = 100;
        else if (betting == "천원")
            bettingAmount = 1000;
        else if (betting == "만원")
            bettingAmount = 10000;
        else if (betting == "십만원")
            bettingAmount = 100000;
        else if (betting == "백만원")
            bettingAmount = 1000000;
    }

    private void OnEnable()
    {
        RoomInfo room = PhotonNetwork.CurrentRoom;
        if (room.CustomProperties.ContainsKey("Restart"))
            room.CustomProperties.Remove("Restart");
        room.CustomProperties.Add("Restart", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(room.CustomProperties);
        transform.SetAsLastSibling();
    }

    IEnumerator ReturnToRoom()
    {
        while (true)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime < 0 && PhotonNetwork.IsMasterClient)
            {
                lifeTime = 0;
                PhotonNetwork.AutomaticallySyncScene = true;
                LevelLoader.LoadNextLevel("Room");
            }
            else if (lifeTime < 0)
                lifeTime = 0;
            lifeTimeText.text = (int)lifeTime + "초 뒤 대기실로 이동";
            yield return null;
        }
    }

    public void GameOver(int index ,int n, string name)
    {
        count++;
        havingCardCount[index] = n;
        sortedCardCount[index] = n;
        if(count < PhotonNetwork.PlayerList.Length)
        {
            return;
        }
        else
        {
            StartCoroutine(ReturnToRoom());
            Sort(name);
        }      
    }
    private void Sort(string name)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            for(int j = i;j< PhotonNetwork.PlayerList.Length; j++)
            {
                if(sortedCardCount[i] >= sortedCardCount[j])
                {
                    int temp = sortedCardCount[i];
                    sortedCardCount[i] = sortedCardCount[j];
                    sortedCardCount[j] = temp;
                }
            }
        }

        SortRate();
    }

    private void SortRate()
    {
        int temp = 1;
        rate[0] = 1;
        for(int i = 1; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(sortedCardCount[i] == sortedCardCount[i - 1])
            {
                rate[i] = rate[i - 1];
                ++temp;
            }
            else
            {
                rate[i] = ++temp;
            }
            
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            for (int j = 0; j < PhotonNetwork.PlayerList.Length; j++)
            {
                if (havingCardCount[i] == sortedCardCount[j])
                {
                    sortedByRate[i] = rate[j];
                    break;
                }
                
            }
                
        }
        
        int money = CreateResult();
        //photonView.RPC("RPC_CreateResult", RpcTarget.All, TurnManager.Instance.PlayerIndex, PhotonNetwork.LocalPlayer.NickName);
        
        //1등
        if (sortedByRate[TurnManager.Instance.PlayerIndex] == 1) 
        {
            Game_UserInfo.win++;
        }
        else
        {
            Game_UserInfo.lose++;
        }

        Game_UserInfo.money += money;

        Game_UserInfo.SetData();
        

    }

    private int CreateResult()
    {
        int money = 0;
        for(int index = 0;index < PhotonNetwork.PlayerList.Length; index++)
        {
            var spawnPosition = spawnPositions[sortedByRate[index] - 1];
            for(int i = sortedByRate[index] - 1; i < PhotonNetwork.PlayerList.Length; i++)
            {
                spawnPosition = spawnPositions[i];
                if (spawnPosition.childCount == 0)
                {
                    spawnPosition = spawnPositions[i];
                    break;
                }
            }
            GameObject obj = Instantiate(Result);
            obj.GetComponent<Image>().color = resultColor[sortedByRate[index] - 1];
            obj.transform.SetParent(spawnPosition);
            obj.GetComponent<RectTransform>().localPosition = new Vector3(-50,50,0);
            print(obj.GetComponent<RectTransform>().localPosition);
            Text[] texts = obj.GetComponentsInChildren<Text>();
            texts[0].text = sortedByRate[index].ToString() + "위";
            texts[1].text = PhotonNetwork.PlayerList[index].NickName;

            
            int temp = Sum(index);
            if (index == TurnManager.Instance.PlayerIndex)
                money = temp;
            if (temp >= 0)
                texts[2].text = "+" + NormalizeBettingText(temp);
            else
                texts[2].text = "-" + NormalizeBettingText(temp);           
        }
        return money = money * bettingAmount;
        
    }
  
    private string NormalizeBettingText(int temp)
    {
        string s = "";
        string up = "";
        if (betting == "백원") { up = "천"; }
        else if (betting == "천원") { up = "만"; }
        else if (betting == "만원") { up = "십"; }
        else if (betting == "십만원") { up = "백"; }
        else if (betting == "백만원") { up = "천"; }

        if (temp < 0)
            temp *= -1;

        if (temp >= 10)
        {
            if (temp % 10 == 0)
            {
                s = (temp / 10) + up + "원";
            }
            else
                s = (temp / 10) + up + (temp % 10) + betting;
        }
      

        else
        {
            if(temp < 0) temp *= -1;          
            s = temp.ToString() + betting;
            if (temp == 0)
                s = "0원";
        }
        return s;
    }

    private int Sum(int index)
    {
        int sum = 0;
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int a = havingCardCount[i];
            int b = havingCardCount[index];

            if (b < a)
            {
                sum += (a - b);
            }
            else if(b > a)
            {
                sum -= (b - a);
            }

        }
        return sum;
    }
}
