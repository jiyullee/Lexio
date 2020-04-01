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
    public GameObject GameOverObj;
    public Button[] passOrRegisterBtns;

    public Button PassButton;
    public Button RegisterButton;
    public Button SortNumBtn;
    public Button SortShapeBtn;
    public GameObject LastTurn;
    public GameObject AllPassObj;
    public GameObject[] RegisterPanels;
    public GameObject GameResult;
    public List<Card> registerdCards = new List<Card>();
    public List<GameObject> havingCardObj = new List<GameObject>();

    public Sprite[] SunImages;
    public Sprite[] MoonImages;
    public Sprite[] StarImages;
    public Sprite[] WindImages;
    public Sprite BackGround;
    public Text LastTurnText;
    GameObject[] sortedCards;

    public AudioSource buttonSound;

    private void Awake()
    {
        Screen.SetResolution(1280, 720, true);
        buttonSound = GetComponent<AudioSource>();
        
    }
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

    public void ShowCards(List<Card> cards)
    {
        StartCoroutine(ShowCardWithDelay(cards));
    }

    IEnumerator ShowCardWithDelay(List<Card> cards)
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
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void Register_DestroyCard(List<Card> cards)
    {
        GameObject[] cardObjs = GameObject.FindGameObjectsWithTag("Slot");
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cardObjs.Length; j++)
            {
                Card tempCard = cardObjs[j].GetComponent<Card>();
                if (tempCard.GetNumber() == cards[i].GetNumber() && tempCard.GetStyle() == cards[i].GetStyle())
                {
                    cardObjs[j].GetComponent<Card>().SetBgImage();
                }
            }
        }
        
    }

    public void RegisterCardsOnCanvas(List<Card> cards)
    {
        List<Card> SortedCards = cards;
        if (TurnManager.Instance.lastTurnStr == "스트레이트" || TurnManager.Instance.lastTurnStr == "플러시" || TurnManager.Instance.lastTurnStr == "스트레이트 플러시")
            SortedCards = Ascendingorder(cards);
        else if(TurnManager.Instance.lastTurnStr == "풀하우스")
            SortedCards = FullHouseOrder(cards);
        else if(TurnManager.Instance.lastTurnStr == "포카드")
            SortedCards = FourCardOrder(cards);


        photonView.RPC("RPC_InitializeGamePanel", RpcTarget.All);
        for (int i = 0; i < SortedCards.Count; i++)
            photonView.RPC("RPC_AddRegisterCard", RpcTarget.All, SortedCards[i].GetNumber(), SortedCards[i].GetStyle());
        for (int i = 0; i < SortedCards.Count; i++)
            photonView.RPC("RPC_RegisterCardsOnCanvas", RpcTarget.All, i, SortedCards.Count, SortedCards[i].GetNumber(), SortedCards[i].GetStyle());
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
        RegisterPanels[n].GetComponent<Image>().sprite = card.GetImage(style, num);

        GameObject[] registers = GameObject.FindGameObjectsWithTag("Register");
        for (int i = 0; i < registers.Length; i++)
        {
            if (registers[i].GetComponentInParent<PlayerScript>().photonView.Owner == TurnManager.Instance.turnPlayer)
            {
                GameObject obj = Instantiate(cardPrefab);
                Card _card = obj.GetComponent<Card>();
                obj.GetComponent<Image>().sprite = _card.GetImage(style, num);
                obj.transform.SetParent(registers[i].transform);
            }
        }
    }
    [PunRPC]
    private void RPC_AddRegisterCard(int num, string style)
    {
        registerdCards.Add(new Card(style, num));
    }

    //오름차순
    private List<Card> Ascendingorder(List<Card> cards)
    {

        for(int i = 0; i < cards.Count; i++)
        {
            for(int j = i; j < cards.Count; j++)
            {
                if(cards[i].GetNumber() > cards[j].GetNumber())
                {
                    Card tempCard = cards[i];
                    cards[i] = cards[j];
                    cards[j] = tempCard;

                }
            }
        }
        return cards;
    }

    private List<Card> FullHouseOrder(List<Card> cards)
    {

        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = i; j < cards.Count; j++)
            {
                if (cards[i].GetNumber() > cards[j].GetNumber())
                {
                    Card tempCard = cards[i];
                    cards[i] = cards[j];
                    cards[j] = tempCard;

                }
            }
        }
        if(cards[1].GetNumber() != cards[2].GetNumber())
        {
            Card tempCard = cards[0];
            cards[0] = cards[4];
            cards[4] = tempCard;
            Card tempCard2 = cards[1];
            cards[1] = cards[3];
            cards[3] = tempCard;
        }
        return cards;
    }

    private List<Card> FourCardOrder(List<Card> cards)
    {

        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = i; j < cards.Count; j++)
            {
                if (cards[i].GetNumber() > cards[j].GetNumber())
                {
                    Card tempCard = cards[i];
                    cards[i] = cards[j];
                    cards[j] = tempCard;

                }
            }
        }
        if (cards[0].GetNumber() != cards[1].GetNumber())
        {
            Card tempCard = cards[0];
            cards[0] = cards[4];
            cards[4] = tempCard;
        }
        return cards;
    }

    public void OnClick_SortedByNum()
    {
        buttonSound.Play();
        Card[] sortedCards = cardContainer.Slots;

        foreach (Card card  in sortedCards)
        {
            if(card.GetImage() == BackGround)
                card.PosNum = 20;
            else
                card.PosNum = card.GetNumber();
        }

        SwapInfo(sortedCards);
        List<List<Card>> list = new List<List<Card>>();
        for (int i = 1; i <= CardManager.Instance.maxUseCard; i++)
        {
            List<Card> _list = new List<Card>();
            for (int j = 0; j < sortedCards.Length; j++)
            {

                if (sortedCards[j].PosNum == i)
                {
                    _list.Add(sortedCards[j]);
                }
            }
            list.Add(_list);
        }
        for (int i = 0; i < list.Count; i++)
        {
            SortedByShape(list[i]);
        }
    }

    public void OnClick_SortedByShape()
    {
        buttonSound.Play();
        Card[] sortedCards = cardContainer.Slots;

        foreach (Card card in sortedCards)
        {
            if (card.GetImage() == BackGround)
                card.PosNum = 20;
            else
                card.PosNum = card.GetStyleClass();
        }

        SwapInfo(sortedCards);
        List<List<Card>> list = new List<List<Card>>();
        for (int i = 1; i <= 4; i++)
        {
            List<Card> _list = new List<Card>();
            for (int j = 0; j < sortedCards.Length; j++)
            {

                if (sortedCards[j].PosNum == i)
                {
                    _list.Add(sortedCards[j]);
                }
            }
            list.Add(_list);
        }
        for (int i = 0; i < list.Count; i++)
        {
            SortedByNum(list[i]);
        }
    }
    public void SortedByNum(List<Card> sortedCards)
    {
        foreach (Card card in sortedCards)
        {
            if (card.GetImage() == BackGround)
                card.PosNum = 20;
            else
                card.PosNum = card.GetNumber();
        }

        SwapInfo(sortedCards.ToArray());

    }
    public void SortedByShape(List<Card> sortedCards)
    {
        foreach (Card card in sortedCards)
        {
            if (card.GetImage() == BackGround)
                card.PosNum = 20;
            else
                card.PosNum = card.GetStyleClass();
        }

        SwapInfo(sortedCards.ToArray());

    }
    private void SwapInfo(Card[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            for (int j = i; j < cards.Length; j++)
            {
                if (cards[i].PosNum > cards[j].PosNum)
                {
                    int tempNum = cards[i].PosNum;
                    cards[i].PosNum = cards[j].PosNum;
                    cards[j].PosNum = tempNum;

                    int no1 = cards[i].GetNumber();
                    string style1 = cards[i].GetStyle();
                    int no2 = cards[j].GetNumber();
                    string style2 = cards[j].GetStyle();
                    Sprite sprite1 = cards[i].GetImage();
                    Sprite sprite2 = cards[j].GetImage();
                    cards[i].SetCardInfo(style2, no2);
                    cards[j].SetCardInfo(style1, no1);
                    cards[i].SetImage(sprite2);
                    cards[j].SetImage(sprite1);
                }
            }
        }
    }
}
