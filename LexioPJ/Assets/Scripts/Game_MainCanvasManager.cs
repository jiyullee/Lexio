using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_MainCanvasManager : MonoBehaviourPun
{
    
    public static Game_MainCanvasManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Game_MainCanvasManager>();

            return instance;
        }
    }
    private static Game_MainCanvasManager instance;

    public GameObject cardPrefab;
    public Game_UI_CardContainer cardContainer;
    public Button GameStartButton;
    public Button[] passOrRegisterBtns;

    public Button PassButton;
    public Button RegisterButton;
    public GameObject LastTurn;
    public GameObject AllPassObj;
    public GameObject[] RegisterPanels;

    public List<Card> registerdCards = new List<Card>();
    public List<GameObject> havingCardObj = new List<GameObject>();

    public Sprite[] SunImages;
    public Sprite[] MoonImages;
    public Sprite[] StarImages;
    public Sprite[] WindImages;
    public Text LastTurnText;

    public void SetLastTurnText(string s, int n)
    {
        photonView.RPC("RPC_SetLastTurnText", RpcTarget.All, s, n);
    }
    [PunRPC]
    private void RPC_SetLastTurnText(string s, int n)
    {
        LastTurnText.text = s + " " + n.ToString();
    }
    [PunRPC]
    private void RPC_SetLastTurnText(string s)
    {
        LastTurnText.text = s;
    }

    public void OnClick_GameStart()
    {
        photonView.RPC("RPC_GameStart", RpcTarget.All);
        photonView.RPC("RPC_InitializeGamePanel", RpcTarget.All);
        photonView.RPC("RPC_SetLastTurnText", RpcTarget.All, "게임이 시작되었습니다.");
        CardManager.Instance.GameStart();
        GameStartButton.gameObject.SetActive(false);
        RegisterButton.interactable = false;
    }

    [PunRPC]
    private void RPC_GameStart()
    {
        TurnManager.Instance.isGameOver = false;
        foreach (Card card in cardContainer.Slots)
            card.SetNullImage();
        PlayerScript[] playerScripts = FindObjectsOfType<PlayerScript>();
        for (int i = 0; i < playerScripts.Length; i++)
        {
            playerScripts[i].turnStateObj.SetActive(false);
            playerScripts[i].ClearRegisterdCards();
        }
        
        LastTurn.SetActive(true);
        RegisterButton.interactable = true;
        PassButton.interactable = true;
    }
    public void ShowCards(List<Card> cards)
    {
        Card[] slots = cardContainer.Slots;
        TurnManager.Instance.HavingCards = cards;
        TurnManager.Instance.HavingCardCount = CardManager.Instance.maxCardCount;
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject obj = slots[i].gameObject;
            havingCardObj.Add(obj);
            obj.GetComponent<Card>().SetNumber(cards[i].GetNumber());
            obj.GetComponent<Card>().SetStyle(cards[i].GetStyle());
            obj.GetComponent<Card>().SetImage();
        }
    }

    public void Register_DestroyCard(List<Card> cards)
    {
        for(int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < havingCardObj.Count; j++)
            {
                Card tempCard = havingCardObj[j].GetComponent<Card>();
                if (tempCard.GetNumber() == cards[i].GetNumber() && tempCard.GetStyle() == cards[i].GetStyle())
                {
                    GameObject obj = havingCardObj[j];
                    havingCardObj.RemoveAt(j);
                    obj.GetComponent<Card>().SetBgImage();
                    obj.GetComponent<Button>().interactable = false;
                }
            }
        }
        
    }

    public void RegisterCardsOnCanvas(List<Card> cards)
    {
        photonView.RPC("RPC_InitializeGamePanel", RpcTarget.All);
        for (int i = 0; i < cards.Count; i++)
            photonView.RPC("RPC_AddRegisterCard", RpcTarget.All, cards[i].GetNumber(), cards[i].GetStyle());
        for (int i = 0; i < cards.Count; i++)
            photonView.RPC("RPC_RegisterCardsOnCanvas", RpcTarget.All, i, cards.Count, cards[i].GetNumber(), cards[i].GetStyle());
    }

    [PunRPC]
    private void RPC_InitializeGamePanel()
    {
        for (int i = 0; i < RegisterPanels.Length; i++)
        {
            RegisterPanels[i].SetActive(false);
            RegisterPanels[i].GetComponent<Image>().sprite = null;
        }
    }
    [PunRPC]
    private void RPC_RegisterCardsOnCanvas(int n, int cardsCount, int num, string style)
    {
        Card card = new Card(style, num);
        RegisterPanels[n].SetActive(true);
        RegisterPanels[n].GetComponent<Image>().sprite = card.GetImage();

        GameObject[] registers = GameObject.FindGameObjectsWithTag("Register");
        for (int i = 0; i < registers.Length; i++)
        {
            if (registers[i].GetComponentInParent<PlayerScript>().photonView.Owner == TurnManager.Instance.turnPlayer)
            {
                GameObject obj = Instantiate(cardPrefab);
                obj.GetComponent<Card>().SetNumber(card.GetNumber());
                obj.GetComponent<Card>().SetStyle(card.GetStyle());
                obj.GetComponent<Card>().SetImage();
                obj.transform.SetParent(registers[i].transform);
            }
        }
    }
    [PunRPC]
    private void RPC_AddRegisterCard(int num, string style)
    {
        registerdCards.Add(new Card(style, num));
    }

}
