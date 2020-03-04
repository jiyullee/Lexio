using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPun
{
    public GameObject myTurn;
    public GameObject WinStateObj;
    public Text playerNameText;
    public GameObject turnStateObj;
    [SerializeField]
    private string playerName;
    public List<Card> havingCards = new List<Card>();
    Image image;
    Color myTurnColor = Color.blue;
    bool isTurn;
    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(MyTurn());
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        while (true)
        {
            yield return null;
            if (TurnManager.Instance.isGameOver)
            {
                WinStateObj.SetActive(true);
                if (photonView.Owner == TurnManager.Instance.winPlayer)
                {
                    WinStateObj.GetComponentInChildren<Text>().text = "승리";
                }
                else
                {                   
                    WinStateObj.GetComponentInChildren<Text>().text = "패배";
                }
            }
            else
            {
                WinStateObj.SetActive(false);
                
            }
        }
    }

    IEnumerator MyTurn()
    {
        while (true)
        {
            myTurn.SetActive(false);
            yield return null;
            if(photonView.Owner == TurnManager.Instance.turnPlayer)
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
                yield return new WaitForSeconds(1.0f);
            }
            

        }
    }

    public void ClearRegisterdCards()
    {
        Card[] cards = GetComponentsInChildren<Card>();
        for (int j = 0; j < cards.Length; j++)
        {
            Destroy(cards[j].gameObject);
        }
    }
    public void SetPlayerName(string str)
    {
        playerName = str;
        playerNameText.text = playerName;
    }

    public void InitializeCardSet(List<Card> cardSet)
    {
        havingCards = cardSet;
        for(int i = 0; i < havingCards.Count; i++)
        {
            if(havingCards[i].GetNumber() == 3 && havingCards[i].GetStyle() == "Wind")
            {
                TurnManager.Instance.turnPlayer = photonView.Owner;
                photonView.RPC("RPC_NoticeStartPlayer", RpcTarget.All);
                TurnManager.Instance.isNewTurn = true;
            }
        }        
    }
     
    [PunRPC]
    private void RPC_NoticeStartPlayer()
    {
        TurnManager.Instance.turnIndex = TurnManager.Instance.turnPlayer.ActorNumber - 1;
    }

    public void NoticeTurnState(Player player, string str)
    {
        if (str != "패스" && str != "등록")
        {
            turnStateObj.SetActive(false);
            return;
        }
        if (photonView.Owner == player)
        {

            turnStateObj.SetActive(true);
            turnStateObj.GetComponent<TurnState>().ChangeText(str);
        }
    }
    
}
