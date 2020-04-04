using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerManager : MonoBehaviour
{
    //카드 승부 비교 함수
    //원
    public bool CanOne(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isOne(comparingCards))
            return false;

        int maxUseCard = CardManager.Instance.maxUseCard;
        int originNum = originCards[0].GetNumber();
        int comPareNum = comparingCards[0].GetNumber();

        if (originNum == 1)
            originNum = maxUseCard + 1;
        else if(originNum == 2)
            originNum = maxUseCard + 2;

        if (comPareNum == 1)
            comPareNum = maxUseCard + 1;
        else if (comPareNum == 2)
            comPareNum = maxUseCard + 2;
        if (comPareNum > originNum)
        {
            if(comPareNum == maxUseCard)
                TurnManager.Instance.SetLastTurnInt(comPareNum);
            else
                TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }
        else if (comPareNum == originNum)
        {
            if (comPareNum == maxUseCard)
                TurnManager.Instance.SetLastTurnInt(comPareNum);
            else
                TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return comparingCards[0].GetStyleClass() > originCards[0].GetStyleClass();
        }
        else
        {
            return false;
        }
    }
    //페어
    public bool CanPair(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isPair(comparingCards))
            return false;

        int maxUseCard = CardManager.Instance.maxUseCard;
        int originNum = originCards[0].GetNumber();
        int comPareNum = comparingCards[0].GetNumber();
        if (originNum == 1)
            originNum = maxUseCard + 1;
        else if (originNum == 2)
            originNum = maxUseCard + 2;

        if (comPareNum == 1)
            comPareNum = maxUseCard + 1;
        else if (comPareNum == 2)
            comPareNum = maxUseCard + 2;

        if(comPareNum > originNum)
        {
            if (comPareNum == maxUseCard)
                TurnManager.Instance.SetLastTurnInt(comPareNum);
            else
                TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }else if(comPareNum == originNum)
        {
            if (ComPareStyle_Pair(originCards, comparingCards) == comparingCards)
            {
                if (comPareNum == maxUseCard)
                    TurnManager.Instance.SetLastTurnInt(comPareNum);
                else
                    TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
                return true;
            }
            else return false;
        }
        else
        {
            return false;
        }        
    }

    private int MaxStyleClass(int i, int j)
    {
        return i >= j ? i : j;
    }
    private int MaxStyleClass(int i, int j, int k)
    {
        return MaxStyleClass(MaxStyleClass(i, j), k);
    }
  
    private List<Card> ComPareStyle_Pair(List<Card> cards1, List<Card> cards2)
    {
        int maxClass_in_Cards1 = MaxStyleClass(cards1[0].GetStyleClass(), cards1[1].GetStyleClass());
        int maxClass_in_Cards2 = MaxStyleClass(cards2[0].GetStyleClass(), cards2[1].GetStyleClass());

        return maxClass_in_Cards1 > maxClass_in_Cards2 ? cards1 : cards2;
    }

    private List<Card> ComPareStyle_Triple(List<Card> cards1, List<Card> cards2)
    {
        int maxClass_in_Cards1 = MaxStyleClass(cards1[0].GetStyleClass(), cards1[1].GetStyleClass(), cards1[2].GetStyleClass());
        int maxClass_in_Cards2 = MaxStyleClass(cards2[0].GetStyleClass(), cards2[1].GetStyleClass(), cards2[2].GetStyleClass());

        return maxClass_in_Cards1 > maxClass_in_Cards2 ? cards1 : cards2;
    }

    private List<Card> ComPareStyle_StraightFlush(List<Card> cards1, List<Card> cards2)
    {
        int maxUseCard = CardManager.Instance.maxUseCard;
        int maxStyleCards1 = cards1[0].GetStyleClass();
        int maxStyleCards2 = cards2[0].GetStyleClass();


        return MaxStyleClass(maxStyleCards1, maxStyleCards2) == maxStyleCards1 ? cards1 : cards2;
    }
    //트리플
    public bool CanTriple(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isTriple(comparingCards))
            return false;
        int maxUseCard = CardManager.Instance.maxUseCard;
        int originNum = originCards[0].GetNumber();
        int comPareNum = comparingCards[0].GetNumber();
        if (originNum == 1)
            originNum = maxUseCard + 1;
        else if (originNum == 2)
            originNum = maxUseCard + 2;

        if (comPareNum == 1)
            comPareNum = maxUseCard + 1;
        else if (comPareNum == 2)
            comPareNum = maxUseCard + 2;

        if (comPareNum > originNum)
        {
            if (comPareNum == maxUseCard)
                TurnManager.Instance.SetLastTurnInt(comPareNum);
            else
                TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }
        else if (comPareNum == originNum)
        {
            if (ComPareStyle_Triple(originCards, comparingCards) == comparingCards)
            {
                if (comPareNum == maxUseCard)
                    TurnManager.Instance.SetLastTurnInt(comPareNum);
                else
                    TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
                return true;
            }
            else return false;
        }
        else
        {
            return false;
        }
    }

    //스트레이트 1
    public bool CanStraight(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isStraight(comparingCards))
            return false;
        else
        {
            int maxUseCard = CardManager.Instance.maxUseCard;
            int compareRank = StraightRank(comparingCards);
            int originRank = StraightRank(originCards);

            if (compareRank < originRank)
            {
                int max = MaxNumInCards(comparingCards);
                if (max == maxUseCard)
                    TurnManager.Instance.SetLastTurnInt(max);
                else
                    TurnManager.Instance.SetLastTurnInt(max % maxUseCard);
                return true;
            }
            else if (compareRank == originRank)
            {
                return CompareStraight_SameRank(originCards, comparingCards, compareRank);
            }
            else return false;
        }
        
           
    }
    private bool CompareStraight_SameRank(List<Card> originCards, List<Card> comparingCards, int rank)
    {
        if (rank == 1 || rank == 2)
        {
            int originClass = 0;
            int compareClass = 0;
            for (int i = 0; i < originCards.Count; i++)
            {
                if (originCards[i].GetNumber() == 2)
                {
                    originClass = originCards[i].GetStyleClass();
                }
                if (comparingCards[i].GetNumber() == 2)
                {
                    compareClass = comparingCards[i].GetStyleClass();
                }
            }
            if (compareClass > originClass)
            {
                TurnManager.Instance.SetLastTurnInt(2);
                return true;
            }
            else return false;
        }
        else if (rank == 3)
        {
            int originClass = 0;
            int compareClass = 0;
            for (int i = 0; i < originCards.Count; i++)
            {
                if (originCards[i].GetNumber() == 1)
                {
                    originClass = originCards[i].GetStyleClass();
                }
                if (comparingCards[i].GetNumber() == 1)
                {
                    compareClass = comparingCards[i].GetStyleClass();
                }
            }
            if (compareClass > originClass)
            {
                TurnManager.Instance.SetLastTurnInt(1);
                return true;
            }
            else return false;
        }
        else if (rank == 4)
        {
            int maxInOrigin = MaxNumInCards(originCards);
            int maxInComPare = MaxNumInCards(comparingCards);
            if (maxInComPare > maxInOrigin)
            {
                TurnManager.Instance.SetLastTurnInt(maxInComPare);
                return true;
            }
            else if (maxInComPare == maxInOrigin)
            {
                int originClass = 0;
                int compareClass = 0;
                for (int i = 0; i < originCards.Count; i++)
                {
                    if (originCards[i].GetNumber() == maxInComPare)
                    {
                        originClass = originCards[i].GetStyleClass();
                    }
                    if (comparingCards[i].GetNumber() == maxInComPare)
                    {
                        compareClass = comparingCards[i].GetStyleClass();
                    }
                }
                if (compareClass > originClass)
                {
                    TurnManager.Instance.SetLastTurnInt(maxInComPare);
                    return true;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }

    public int MaxNumInCards(List<Card> cards)
    {
        int maxUseCard = CardManager.Instance.maxUseCard;
        int[] sortedByNum = Ascendingorder(cards);
                 
        for(int i= 0; i < sortedByNum.Length; i++)
        {
            if (sortedByNum[i] == 1)
                sortedByNum[i] += maxUseCard;
            else if (sortedByNum[i] == 2)
                sortedByNum[i] += maxUseCard;
        }

        int max = sortedByNum[0];
        for(int i = 1; i < sortedByNum.Length; i++)
        {
            if (max < sortedByNum[i])
                max = sortedByNum[i];
        }
        
        return max;
    }

    public int MaxNumInFourCard(List<Card> cards)
    {
        int[] sortedByNum = Ascendingorder(cards);
        return sortedByNum[2];

    }

    public int MaxNumInFullHouse(List<Card> cards)
    {
        int[] sortedByNum = Ascendingorder(cards);
        return sortedByNum[2];

    }
    //플러쉬  2
    public bool CanFlush(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isFlush(comparingCards))
            return false;
        
        int maxUseCard = CardManager.Instance.maxUseCard;

        int maxOrigin = MaxNumInCards(originCards);
        int maxCompare = MaxNumInCards(comparingCards);
       
        if (maxCompare > maxOrigin)
        {
            if (maxCompare != maxUseCard)
                maxCompare = maxCompare % maxUseCard;
            TurnManager.Instance.SetLastTurnInt(maxCompare);
            return true;
        }
        else if (maxCompare == maxOrigin)
        {
            int originClass = originCards[0].GetStyleClass();
            int compareClass = comparingCards[0].GetStyleClass();
            if (compareClass > originClass)
            {
                if (maxCompare != maxUseCard)
                    maxCompare = maxCompare % maxUseCard;
                TurnManager.Instance.SetLastTurnInt(maxCompare);
                return true;
            }
            else return false;
        }
        else return false;
    }

    //풀하우스 3
    public bool CanFullHouse(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isFullHouse(comparingCards))
            return false;
        int maxUseCard = CardManager.Instance.maxUseCard;
        int[] sortedByNum_Origin = Ascendingorder(originCards);
        int[] sortedByNum_Compare = Ascendingorder(comparingCards);
        if (sortedByNum_Compare[2] == 1)
            sortedByNum_Compare[2] += maxUseCard;
        else if (sortedByNum_Compare[2] == 2)
            sortedByNum_Compare[2] += maxUseCard;

        if (sortedByNum_Origin[2] == 1)
            sortedByNum_Origin[2] += maxUseCard;
        else if (sortedByNum_Origin[2] == 2)
            sortedByNum_Origin[2] += maxUseCard;
      
        if (sortedByNum_Compare[2] > sortedByNum_Origin[2])
        {
            if (sortedByNum_Compare[2] == maxUseCard)
                TurnManager.Instance.SetLastTurnInt(maxUseCard);
            else
                TurnManager.Instance.SetLastTurnInt(sortedByNum_Compare[2] % maxUseCard);
            return true;
        }
        else return false;
    }

    //포카드 4
    public bool CanFourCard(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isFourCard(comparingCards))
            return false;
        int maxUseCard = CardManager.Instance.maxUseCard;
        int[] sortedByNum_Origin = Ascendingorder(originCards);
        int[] sortedByNum_Compare = Ascendingorder(comparingCards);
        if (sortedByNum_Compare[2] == 1)
            sortedByNum_Compare[2] += maxUseCard;
        else if (sortedByNum_Compare[2] == 2)
            sortedByNum_Compare[2] += maxUseCard;

        if (sortedByNum_Origin[2] == 1)
            sortedByNum_Origin[2] += maxUseCard;
        else if (sortedByNum_Origin[2] == 2)
            sortedByNum_Origin[2] += maxUseCard;
      
        if (sortedByNum_Compare[2] > sortedByNum_Origin[2])
        {
            if (sortedByNum_Compare[2] == maxUseCard)
                TurnManager.Instance.SetLastTurnInt(maxUseCard);
            else
                TurnManager.Instance.SetLastTurnInt(sortedByNum_Compare[2] % maxUseCard);
            return true;
        }
        else return false;
    }

    //스트레이트플러쉬 5
    public bool CanStraightFlush(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isStraightFlush(comparingCards))
            return false;
        int maxUseCard = CardManager.Instance.maxUseCard;
        int compareRank = StraightRank(comparingCards);
        int originRank = StraightRank(originCards);
        if (compareRank < originRank)
        {
            int max = MaxNumInCards(comparingCards);
            if (max == maxUseCard)
                TurnManager.Instance.SetLastTurnInt(max);
            else
                TurnManager.Instance.SetLastTurnInt(max % maxUseCard);
            return true;
        }
        else if (compareRank == originRank)
        {
            return CompareStraight_SameRank(originCards, comparingCards, compareRank);
        }
        else return false;

    }

    public bool isOne(List<Card> cards)
    {
        if (cards.Count == 1)
        {
            TurnManager.Instance.SetLastTurnInt(cards[0].GetNumber());
            return true;
        }
        else
            return false;
    }
    
    public bool isPair(List<Card> cards)
    {
        if (cards.Count != 2)
            return false;
        else
        {
            if (cards[0].GetNumber() == cards[1].GetNumber())
            {
                TurnManager.Instance.SetLastTurnInt(cards[0].GetNumber());
                return true;
            }
            else
                return false;
        }
    }

    public bool isTriple(List<Card> cards)
    {
        if (cards.Count != 3)
            return false;
        else
        {
            if (cards[0].GetNumber() == cards[1].GetNumber() && cards[1].GetNumber() == cards[2].GetNumber() && cards[2].GetNumber() == cards[0].GetNumber())
            {
                TurnManager.Instance.SetLastTurnInt(cards[0].GetNumber());
                return true;
            }
            else
                return false;
        }
    }

    public bool isStraight(List<Card> cards)
    {
        if (cards.Count != 5)
            return false;

        int maxUseCard = CardManager.Instance.maxUseCard;
        int[] sortedByNum = Ascendingorder(cards);

        if ((sortedByNum[1] == sortedByNum[0] + 1) && (sortedByNum[2] == sortedByNum[1] + 1) && (sortedByNum[3] == sortedByNum[2] + 1) && (sortedByNum[4] == sortedByNum[3] + 1))
        {
            if (sortedByNum[0] == 1 && sortedByNum[1] == 2)
                TurnManager.Instance.SetLastTurnInt(2);
            else if (sortedByNum[0] == 2)
            {
                TurnManager.Instance.SetLastTurnInt(2);
            }
            else
            {
                TurnManager.Instance.SetLastTurnInt(sortedByNum[4]);
            }
            return true;
        }
        else if (sortedByNum[0] == 1 && sortedByNum[1] == maxUseCard - 3 && sortedByNum[2] == maxUseCard - 2 && sortedByNum[3] == maxUseCard - 1 && sortedByNum[4] == maxUseCard)
        {
            TurnManager.Instance.SetLastTurnInt(1);
            return true;
        }
        else return false;
    }

    public int StraightRank(List<Card> cards)
    {
        int maxUseCard = CardManager.Instance.maxUseCard;
        int[] sortedByNum = Ascendingorder(cards);
        if (sortedByNum[0] == 1 && sortedByNum[1] == 2)
            return 1;
        else if (sortedByNum[0] == 2)
            return 2;
        else if (sortedByNum[0] == 1 && sortedByNum[1] == maxUseCard - 3 && sortedByNum[2] == maxUseCard - 2 && sortedByNum[3] == maxUseCard - 1 && sortedByNum[4] == maxUseCard)
            return 3;
        else
            return 4;
    }

    //오름차순
    private int[] Ascendingorder(List<Card> cards)
    {
        List<int> numList = new List<int>();
        
        for(int i = 0; i < cards.Count; i++)
        {
            numList.Add(cards[i].GetNumber());
        }
        int[] numArr = numList.ToArray();
        for(int i = 0; i < numArr.Length; i++)
        {
            for (int j = i; j < numArr.Length; j++)
            {
                if (numArr[i] > numArr[j])
                {
                    int temp = numArr[i];
                    numArr[i] = numArr[j];
                    numArr[j] = temp;
                }
            }
        }
        return numArr;

    }

    public bool isFlush(List<Card> cards)
    {
        if (cards.Count != 5)
            return false;

        int maxUseCard = CardManager.Instance.maxUseCard;
        int[] sortedByNum = Ascendingorder(cards);


        if ((cards[0].GetStyle() == cards[1].GetStyle()) && (cards[1].GetStyle() == cards[2].GetStyle()) && (cards[2].GetStyle() == cards[3].GetStyle()) && (cards[3].GetStyle() == cards[4].GetStyle()))
        {
            for(int i = 0; i < sortedByNum.Length; i++)
            {
                if (sortedByNum[i] == 2)
                    sortedByNum[i] += maxUseCard;
                else if (sortedByNum[i] == 1)
                    sortedByNum[i] += maxUseCard;
            }
            int max = sortedByNum[0];
            for(int j = 1; j < sortedByNum.Length; j++)
            {
                if (max < sortedByNum[j])
                    max = sortedByNum[j];
            }
            if(max == maxUseCard)
                TurnManager.Instance.SetLastTurnInt(maxUseCard);
            else
                TurnManager.Instance.SetLastTurnInt(max % maxUseCard);
            return true;
        }
        else
            return false;
    }

    public bool isFullHouse(List<Card> cards)
    {
        if (cards.Count != 5)
            return false;
        else
        {
            int maxUseCard = CardManager.Instance.maxUseCard;
            int[] sortedByNum = Ascendingorder(cards);
            if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[1] == sortedByNum[2]) && (sortedByNum[3] == sortedByNum[4]))
            {
                TurnManager.Instance.SetLastTurnInt(sortedByNum[0]);
                return true;
            }
            else if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[2] == sortedByNum[3]) && (sortedByNum[3] == sortedByNum[4]))
            {
                TurnManager.Instance.SetLastTurnInt(sortedByNum[2]);
                return true;
            }
            else
                return false;
        }
    }

    public bool isFourCard(List<Card> cards)
    {
        if (cards.Count != 5)
            return false;
        else
        {
            int maxUseCard = CardManager.Instance.maxUseCard;
            int[] sortedByNum = Ascendingorder(cards);
            if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[1] == sortedByNum[2]) && (sortedByNum[2] == sortedByNum[3]))
            {
                TurnManager.Instance.SetLastTurnInt(sortedByNum[0]);
                return true;
            }
            else if ((sortedByNum[1] == sortedByNum[2]) && (sortedByNum[2] == sortedByNum[3]) && (sortedByNum[3] == sortedByNum[4]))
            {
                TurnManager.Instance.SetLastTurnInt(sortedByNum[1]);
                return true;
            }
            else
                return false;
        }
    }

    public bool isStraightFlush(List<Card> cards)
    {
        if (cards.Count != 5)
            return false;
        else
        {
            int maxUseCard = CardManager.Instance.maxUseCard;
            int[] sortedByNum = Ascendingorder(cards);

            if (isFlush(cards) && isStraight(cards))
            {                                  
                return true;
            }
            else
                return false;
        }
    }
}
