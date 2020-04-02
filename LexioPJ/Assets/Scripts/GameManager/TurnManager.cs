using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;

public class TurnManager : MonoBehaviourPunCallbacks
{
    public static TurnManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<TurnManager>();

            return instance;
        }
    }
    private static TurnManager instance;
    public Player startPlayer;
    public Player turnPlayer;
    public Player winPlayer;
    public int turnIndex;
    public int passCount;
    public bool isNewTurn = true;
    public bool isGameOver = false;
    public int HavingCardCount = 0;
    public List<Card> HavingCards = new List<Card>();   
    public List<Card> selectedCards = new List<Card>();
    public List<Card> originCards = new List<Card>();
    public Transform[] playerPos;
    public int[] havingCardCount = new int[6];
    public int[] sortedCardCount = new int[6];

    Card tempCard;
    PokerManager pokerManager;
    public bool CanRegister = false;
    public string lastTurnStr;
    int lastTurnInt;
    public int PlayerIndex { get; set; }

    private void Awake()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.LocalPlayer.NickName == PhotonNetwork.PlayerList[i].NickName)
                PlayerIndex = i;
        }
    }
    private void Start()
    {
        pokerManager = GetComponent<PokerManager>();
        StartCoroutine(WaitGame());
    }
    public int FindIndex(string NickName)
    {
        int PlayerIndex = 0;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (NickName == PhotonNetwork.PlayerList[i].NickName)
                PlayerIndex = i;
        }
        return PlayerIndex;
    }
    IEnumerator WaitGame()
    {
        while (true)
        {
            yield return null;
            if(turnPlayer == PhotonNetwork.LocalPlayer)
            {
                BeginTurn();
                CanRegister = true;
            }
            else
            {
                EndTurn();
                CanRegister = false;
            }

            if (isGameOver)
            {
                Game_MainCanvasManager.Instance.PassButton.interactable = false;
                Game_MainCanvasManager.Instance.RegisterButton.interactable = false;
                Game_MainCanvasManager.Instance.GameResult.SetActive(true);
                if(HavingCardCount == 0)
                {
                    Game_MainCanvasManager.Instance.GameResultText.text = "승리";
                }
                else
                {
                    Game_MainCanvasManager.Instance.GameResultText.text = "패배";
                }
               yield return new WaitForSeconds(3.0f);
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
                Game_MainCanvasManager.Instance.GameOverObj.SetActive(true);
                photonView.RPC("RPC_Return", RpcTarget.All, PlayerIndex, HavingCardCount, PhotonNetwork.LocalPlayer.NickName);                
                
                break;
            }
            else
            {
                Game_MainCanvasManager.Instance.LastTurn.SetActive(true);
                Game_MainCanvasManager.Instance.GameOverObj.SetActive(false);
            }

        }
    }

    [PunRPC]
    private void RPC_GameOver()
    {
        isGameOver = true; 
        
    }
    [PunRPC]
    private void RPC_Return(int index, int n, string name)
    {
        Game_MainCanvasManager.Instance.GameOverObj.SetActive(true);
        GameOverManager.Instance.GameOver(index, n, name);

    }
    public void SelectCard(Card card)
    {        
        selectedCards.Add(card);
    }
    public void UnSelectCard(Card card)
    {
        selectedCards.Remove(card);
    }
    
    //턴 시작
    private void BeginTurn()
    {
        Game_MainCanvasManager.Instance.PassButton.interactable = true;
        Game_MainCanvasManager.Instance.RegisterButton.interactable = true;
    }
    private void EndTurn()
    {
        Game_MainCanvasManager.Instance.PassButton.interactable = false;
        Game_MainCanvasManager.Instance.RegisterButton.interactable = false;
    }

    public void Pass()
    {
        photonView.RPC("RPC_Pass", RpcTarget.All);
        Card[] cards = GameObject.FindObjectsOfType<Card>();
        foreach(Card card in cards)
        {
            card.UnselectCard();
        }
        photonView.RPC("RPC_PlayPassSound", RpcTarget.All);
    }

    public void Register()
    {
        //처음 등록
        if(isNewTurn)
        {
            if (!CanRegister_New())
                return;
            Register_new();
            photonView.RPC("RPC_PlayCallSound", RpcTarget.All);
        }
        else
        {
            if (!CanRegister_ComPare())
                return;
            Register_Compare();
            photonView.RPC("RPC_PlayCallSound", RpcTarget.All);
        }
       
    }
    [PunRPC]
    private void RPC_PlayCallSound()
    {
        SoundManager.Instance.PlayCallSound();
    }

    [PunRPC]
    private void RPC_PlayPassSound()
    {
        SoundManager.Instance.PlayPassSound();
    }
    [PunRPC]
    private void RPC_Pass()
    {
        PlayerScript[] playerScripts = GameObject.FindObjectsOfType<PlayerScript>();
        for(int i = 0;i< playerScripts.Length; i++)
        {
            playerScripts[i].NoticeTurnState(turnPlayer, "패스");
        }

        passCount++;
        if (passCount == PhotonNetwork.PlayerList.Length - 1)
        {
            isNewTurn = true;
            
            Game_MainCanvasManager.Instance.AllPassObj.SetActive(true);
            SoundManager.Instance.PlayAllPassSound();
        }

        turnIndex++;
        turnPlayer = PhotonNetwork.PlayerList[turnIndex % PhotonNetwork.PlayerList.Length];
    }
    [PunRPC]
    private void RPC_Register()
    {
        PlayerScript[] playerScripts = GameObject.FindObjectsOfType<PlayerScript>();
        for (int i = 0; i < playerScripts.Length; i++)
        {
            playerScripts[i].NoticeTurnState(turnPlayer, "등록");
            if (playerScripts[i].photonView.Owner == turnPlayer)
            {
                playerScripts[i].havingCards = HavingCards;
            }
        }
        selectedCards.Clear();
        passCount = 0;
        isNewTurn = false;

        turnIndex++;
        turnPlayer = PhotonNetwork.PlayerList[turnIndex % PhotonNetwork.PlayerList.Length];

    }

    private bool CanRegister_New()
    {
        if (selectedCards.Count == 1)
        {
            lastTurnStr = "싱글";
            return pokerManager.isOne(selectedCards);
        }
        else if (selectedCards.Count == 2)
        {
            lastTurnStr = "페어";
            return pokerManager.isPair(selectedCards);
        }
        else if (selectedCards.Count == 3)
        {
            lastTurnStr = "트리플";
            return pokerManager.isTriple(selectedCards);
        }
        else if (selectedCards.Count == 5)
        {
            if (pokerManager.isStraightFlush(selectedCards))
            {
                lastTurnStr = "스트레이트 플러시";
                return true;
            }
            if (pokerManager.isFourCard(selectedCards))
            {
                lastTurnStr = "포카드";
                return true;
            }
            if (pokerManager.isFullHouse(selectedCards))
            {
                lastTurnStr = "풀하우스";
                return true;
            }
            if (pokerManager.isFlush(selectedCards))
            {
                lastTurnStr = "플러시";
                return true;
            }
            if (pokerManager.isStraight(selectedCards))
            {
                lastTurnStr = "스트레이트";
                return true;
            }
            else
            {
                lastTurnStr = "";
                return false;
            }
        }
        else
        {
            lastTurnStr = "";
            return false;
        }
    }

    public void SetLastTurnInt(int n)
    {
        if(n == 0)
            n = CardManager.Instance.maxUseCard;
        lastTurnInt = n;
    }
    private void Register_new()
    {
        
        Game_MainCanvasManager.Instance.RegisterCardsOnCanvas(selectedCards);
        Game_MainCanvasManager.Instance.Register_DestroyCard(selectedCards);
        Game_MainCanvasManager.Instance.SetLastTurnText(lastTurnStr, lastTurnInt);
        photonView.RPC("RPC_InitializeOrizinCards", RpcTarget.All);

        for (int i = 0; i < selectedCards.Count; i++)
        {
            photonView.RPC("RPC_ChangeOrizinCards", RpcTarget.All, selectedCards[i].GetStyle(), selectedCards[i].GetNumber());
            HavingCardCount--;
        }
        if (HavingCardCount == 0)
        {
            winPlayer = turnPlayer;
            photonView.RPC("RPC_GameOver", RpcTarget.All);
        }
        photonView.RPC("RPC_Register", RpcTarget.All);
        selectedCards.Clear();
    }

    private bool CanRegister_ComPare()
    {
        if (originCards.Count != selectedCards.Count)
            return false;

        if (selectedCards.Count == 1)
        {
            print(1);
            lastTurnStr = "싱글";
            return pokerManager.CanOne(originCards, selectedCards);
        }
        else if (selectedCards.Count == 2)
        {
            lastTurnStr = "페어";
            return pokerManager.CanPair(originCards, selectedCards);
        }
        else if (selectedCards.Count == 3)
        {
            lastTurnStr = "트리플";
            return pokerManager.CanTriple(originCards, selectedCards);
        }
        else if (selectedCards.Count == 5)
        {
            if (pokerManager.isStraightFlush(selectedCards))
            {
                if (!pokerManager.isStraightFlush(originCards))
                {
                    lastTurnStr = "스트레이트 플러시";
                    lastTurnInt = pokerManager.MaxNumInCards(selectedCards);
                    return true;
                }
                else
                {
                    lastTurnStr = "스트레이트 플러시";
                    return pokerManager.CanStraightFlush(originCards, selectedCards);
                }
            }
            else if (pokerManager.isFourCard(selectedCards))
            {
                if (pokerManager.isStraightFlush(originCards))
                {
                    return false;
                }
                else if (pokerManager.isFullHouse(originCards) || pokerManager.isFlush(originCards) || pokerManager.isStraight(originCards))
                {
                    lastTurnStr = "포카드";
                    lastTurnInt = pokerManager.MaxNumInFourCard(selectedCards);
                    return true;
                }
                else
                {
                    lastTurnStr = "포카드";
                    return pokerManager.CanFourCard(originCards, selectedCards);
                }
            }
            else if (pokerManager.isFullHouse(selectedCards))
            {
                if (pokerManager.isStraightFlush(originCards) || pokerManager.isFourCard(originCards))
                {
                    return false;
                }
                else if (pokerManager.isFlush(originCards) || pokerManager.isStraight(originCards))
                {
                    lastTurnStr = "풀하우스";
                    lastTurnInt = pokerManager.MaxNumInFullHouse(selectedCards);
                    return true;
                }
                else
                {
                    lastTurnStr = "풀하우스";
                    return pokerManager.CanFullHouse(originCards, selectedCards);
                }
            }
            else if (pokerManager.isFlush(selectedCards))
            {
                if (pokerManager.isStraightFlush(originCards) || pokerManager.isFourCard(originCards) || pokerManager.isFullHouse(originCards))
                {
                    return false;
                }
                else if (pokerManager.isStraight(originCards))
                {
                    lastTurnStr = "플러시";
                    lastTurnInt = pokerManager.MaxNumInCards(selectedCards);
                    return true;
                }
                else
                {
                    lastTurnStr = "플러시";
                    return pokerManager.CanFlush(originCards, selectedCards);
                }
            }
            else if (pokerManager.isStraight(selectedCards))
            {
                if (!pokerManager.isStraight(originCards))
                {
                    return false;
                }
                else
                {
                    lastTurnStr = "스트레이트";
                    return pokerManager.CanStraight(originCards, selectedCards);
                }
            }
            else
            {
                lastTurnStr = "";
                return false;
            }
        }
        else
        {
            lastTurnStr = "";
            return false;
        }
    }

    private void Register_Compare()
    {
        Game_MainCanvasManager.Instance.RegisterCardsOnCanvas(selectedCards);
        Game_MainCanvasManager.Instance.Register_DestroyCard(selectedCards);
        Game_MainCanvasManager.Instance.SetLastTurnText(lastTurnStr, lastTurnInt);
        photonView.RPC("RPC_InitializeOrizinCards", RpcTarget.All);

        for (int i = 0; i < selectedCards.Count; i++)
        {
            photonView.RPC("RPC_ChangeOrizinCards", RpcTarget.All, selectedCards[i].GetStyle(), selectedCards[i].GetNumber());
            HavingCardCount--;
        }
        if (HavingCardCount == 0)
        {
            winPlayer = turnPlayer;
            photonView.RPC("RPC_GameOver", RpcTarget.All);
        }
        photonView.RPC("RPC_Register", RpcTarget.All);
        selectedCards.Clear();
    }

    [PunRPC]
    private void RPC_ChangeOrizinCards(string style, int num)
    {
        originCards.Add(new Card(style, num));

    }
    [PunRPC]
    private void RPC_InitializeOrizinCards()
    {
        originCards.Clear();
    }

}
