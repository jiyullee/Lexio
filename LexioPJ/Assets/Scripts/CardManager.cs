using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class CardManager : MonoBehaviourPun
{
    public static CardManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<CardManager>();

            return instance;
        }
    }
    private static CardManager instance;
    Random r = new Random(Guid.NewGuid().GetHashCode());
    List<Card> cards = new List<Card>();
    public List<Card> shuffledCards = new List<Card>();
    GameObject[] playerObjects = new GameObject[6];
    public int maxCardCount = 12;
    public int maxUseCard = 6;
    List<List<Card>> divideCardSets = new List<List<Card>>();
    List<Card> dividedCardSet = new List<Card>();
    int seed = 0;
    void Start()
    {

        switch (PhotonNetwork.PlayerList.Length)
        {
            case 3:
                maxCardCount = 12;
                maxUseCard = 9;
                break;
            case 4:
                maxCardCount = 13;
                maxUseCard = 13;
                break;
            case 5:
                maxCardCount = 12;
                maxUseCard = 15;
                break;
            case 6:
                maxCardCount = 10;
                maxUseCard = 15;
                break;
            default:
                maxCardCount = 12;
                maxUseCard = 6;
                break;

        }
        for (int i = 0; i < maxUseCard; i++)
        {
            Card card = new Card("Sun", i + 1);
            cards.Add(card);
        }
        for (int i = 0; i < maxUseCard; i++)
        {
            Card card = new Card("Moon", i + 1);
            cards.Add(card);
        }
        for (int i = 0; i < maxUseCard; i++)
        {
            Card card = new Card("Star", i + 1);
            cards.Add(card);
        }
        for (int i = 0; i < maxUseCard; i++)
        {
            Card card = new Card("Wind", i + 1);
            cards.Add(card);
        }
        GameStart();
    }

    public void GameStart()
    {
        photonView.RPC("RPC_InitializeShuffledCards", RpcTarget.All);
        if (PhotonNetwork.IsMasterClient)
            Shuffle();
    }
    private void Shuffle()
    {
        
        for (int i = 0; i < cards.Count; i++)
        {
            Card temp = cards[i];
            int randomIndex = r.Next(0, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }     
       
        for(int i = 0;i< cards.Count; i++)
        {
            photonView.RPC("RPC_SendShuffledCard", RpcTarget.All, cards[i].GetNumber(), cards[i].GetStyle());
        }
        photonView.RPC("RPC_Shuffle", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_SendShuffledCard(int num, string style)
    {
        shuffledCards.Add(new Card(style, num));
    }

    [PunRPC]
    private void RPC_InitializeShuffledCards()
    {
        shuffledCards.Clear();
    }
    [PunRPC]
    private void RPC_Shuffle()
    {
        divideCardSets = Chunk.ChunkBy(shuffledCards, maxCardCount);
        PlayerScript[] playerScripts = GameObject.FindObjectsOfType<PlayerScript>();

        for (int i = 0; i < playerScripts.Length; i++)
        {
            for (int j = 0; j < PhotonNetwork.PlayerList.Length; j++)
            {
                if (playerScripts[i].photonView.Owner == PhotonNetwork.PlayerList[j])
                {
                    playerScripts[i].InitializeCardSet(divideCardSets[j]);
                }
            }
        }
        dividedCardSet = divideCardSets[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        Game_MainCanvasManager.Instance.ShowCards(dividedCardSet);
    }   
}

public static class Chunk
{
    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }
}