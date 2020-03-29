using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VolumetricLines;
using Random = System.Random;
public class PlayerScript : MonoBehaviourPunCallbacks
{
    public GameObject myTurn;
    public GameObject turnStateObj;
    public Sprite[] planetSprites;
    
    Color[] planetColors = new Color[] {Color.red, new Color(1, 0.4980392f, 1,1), Color.yellow, Color.green, Color.blue, new Color(0,0, 0.50196081f,1), new Color(0.5450981f,0,1,1) };
    [SerializeField]
    private string playerName;
    public List<Card> havingCards = new List<Card>();
    Image image;
    Color myTurnColor = Color.blue;
    bool isTurn;
    public GameObject infoPanel;
    public GameObject cardPanel;

    public GameObject[] Players_2;
    public GameObject[] Players_3;
    public GameObject[] Players_4;
    public GameObject[] Players_5;
    public GameObject[] Players_6;
    public int PlayerIndex { get; set; }
    public Player owner;

    public GameObject Sleepy_Emoji;
    public GameObject Mad_Emoji;
    public GameObject Disappoint_Emoji;
    public GameObject Love_Emoji;
    private void Start()
    {
        image = GetComponent<Image>();
        
        StartCoroutine(MyTurn());
    }
    
    IEnumerator MyTurn()
    {
        while (true)
        {            
            yield return null;
            if(owner == TurnManager.Instance.turnPlayer)
            {
                if (!isTurn)
                {
                    myTurn.SetActive(true);
                    isTurn = true;
                }
                else
                {
                    myTurn.SetActive(false);
                    isTurn = false;
                }
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                myTurn.SetActive(true);
            }
            
        }
    }
  
    public void OnRegisterPanel(int PlayerCount, int playerIndex)
    {
        switch (PlayerCount)
        {
            
            case 2:
                Players_2[playerIndex].SetActive(true);
                cardPanel = Players_2[playerIndex]; break;
            case 3:
                Players_3[playerIndex].SetActive(true);
                cardPanel = Players_3[playerIndex]; break;
            case 4:
                Players_4[playerIndex].SetActive(true);
                cardPanel = Players_4[playerIndex]; break;
            case 5:
                Players_5[playerIndex].SetActive(true);
                cardPanel = Players_5[playerIndex]; break;
            case 6:
                Players_6[playerIndex].SetActive(true);
                cardPanel = Players_6[playerIndex]; break;
        }
    }
    public void SetColor()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (photonView.Owner.NickName == PhotonNetwork.PlayerList[i].NickName)
                PlayerIndex = i;
        }
        photonView.RPC("RPC_SetColor", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_SetColor()
    {
        int rand = (int)photonView.Owner.CustomProperties["ImageRand"];
        myTurn.GetComponent<Image>().sprite = planetSprites[rand];
        infoPanel.GetComponent<Image>().color = planetColors[PlayerIndex];
        cardPanel.GetComponent<Image>().color = planetColors[PlayerIndex];
    }

    public void ClearRegisterdCards()
    {
        Card[] cards = GetComponentsInChildren<Card>();
        for (int j = 0; j < cards.Length; j++)
        {
            Destroy(cards[j].gameObject);
        }
    }

    public void InitializeCardSet(List<Card> cardSet)
    {
        havingCards = cardSet;
        for(int i = 0; i < havingCards.Count; i++)
        {
            if(havingCards[i].GetNumber() == 3 && havingCards[i].GetStyle() == "Wind")
            {
                TurnManager.Instance.turnPlayer = owner;
                photonView.RPC("RPC_NoticeStartPlayer", RpcTarget.All, PlayerIndex);
                TurnManager.Instance.isNewTurn = true;
                break;
            }
        }        
    }
     
    [PunRPC]
    private void RPC_NoticeStartPlayer(int index)
    {

        TurnManager.Instance.turnIndex = index;
    }

    public void NoticeTurnState(Player player, string str)
    {
        if (str != "패스" && str != "등록")
        {
            turnStateObj.SetActive(false);
            return;
        }
        if (owner == player)
        {

            turnStateObj.SetActive(true);
            turnStateObj.GetComponent<TurnState>().ChangeText(str);
        }
    }
    
    public void OnEmoticon(string msg)
    {
        StartCoroutine(Emoji(msg));
    }
    IEnumerator Emoji(string msg)
    {
        GameObject obj = null;
        if (msg == "빨리")
            obj = Sleepy_Emoji;
        else if (msg == "실망")
            obj = Disappoint_Emoji;
        else if (msg == "화남")
            obj = Mad_Emoji;
        else if (msg == "행복")
            obj = Love_Emoji;

        obj.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        obj.SetActive(false);
    }
}
