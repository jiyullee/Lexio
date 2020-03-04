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
            TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }
        else if (comPareNum == originNum)
        {
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
            TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }else if(comPareNum == originNum)
        {
            if (ComPareStyle_Pair(originCards, comparingCards) == comparingCards)
            {
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

        if (maxClass_in_Cards1 > maxClass_in_Cards2)
            return cards1;
        else
            return cards2;
    }
    private List<Card> ComPareStyle_Triple(List<Card> cards1, List<Card> cards2)
    {
        int maxClass_in_Cards1 = MaxStyleClass(cards1[0].GetStyleClass(), cards1[1].GetStyleClass(), cards1[2].GetStyleClass());
        int maxClass_in_Cards2 = MaxStyleClass(cards2[0].GetStyleClass(), cards2[1].GetStyleClass(), cards2[2].GetStyleClass());

        if (maxClass_in_Cards1 > maxClass_in_Cards2)
            return cards1;
        else
            return cards2;
    }

    private List<Card> ComPareStyle_Straight(List<Card> cards1, List<Card> cards2, int i1, int i2)
    {
        int maxUseCard = CardManager.Instance.maxUseCard;
        int maxNumCards1 = 0, maxNumCards2 = 0;
        for (int i = 0; i < cards1.Count; i++)
            if (cards1[i].GetNumber() == (i1 % maxUseCard))
                maxNumCards1 = cards1[i].GetStyleClass();
        for (int i = 0; i < cards2.Count; i++)
            if (cards2[i].GetNumber() == (i2 % maxUseCard))
                maxNumCards2 = cards2[i].GetStyleClass();

        return MaxStyleClass(maxNumCards1, maxNumCards2) == maxNumCards1 ? cards1 : cards2;
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
            TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }
        else if (comPareNum == originNum)
        {
            if (ComPareStyle_Triple(originCards, comparingCards) == comparingCards)
            {
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
        int maxUseCard = CardManager.Instance.maxUseCard;
        int originNum = ComPare_Straight(originCards);
        int comPareNum = ComPare_Straight(comparingCards);
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
            TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }
           
        else if (comPareNum == originNum)
        {
            List<Card> list = ComPareStyle_Straight(originCards, comparingCards, originNum, comPareNum);
            if (list == comparingCards)
            {
                TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
                return true;
            }
            else
                return false;
        }
        else
            return false;
           
    }

    private int ComPare_Straight(List<Card> cards)
    {
        int maxUseCard = CardManager.Instance.maxUseCard;
        int[] sortedByNum = Ascendingorder(cards);
        int min = sortedByNum[0];
        if (min >= 3)
        {
            return min + 4;
        }
        else
        {
            if (min == 1)
            {
                if (sortedByNum[0] == 1 && sortedByNum[1] == maxUseCard - 3 && sortedByNum[2] == maxUseCard - 2 && sortedByNum[3] == maxUseCard - 1 && sortedByNum[4] == maxUseCard)
                    return maxUseCard + 1;
                else
                    return maxUseCard + 2;
            }
            else
                return maxUseCard + 2;            
        }
    }
    //플러쉬  2
    public bool CanFlush(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isFlush(comparingCards))
            return false;
        int maxUseCard = CardManager.Instance.maxUseCard;
        int originNum = ComPare_Flush(originCards);
        int comPareNum = ComPare_Flush(comparingCards);
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
            TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }
        else if (comPareNum == originNum)
        {
            List<Card> list = ComPareStyle_Straight(originCards, comparingCards, originNum, comPareNum);
            if (list == comparingCards)
            {
                TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    private int ComPare_Flush(List<Card> cards)
    {
        int maxUseCard = CardManager.Instance.maxUseCard;
        int[] sortedByNum = Ascendingorder(cards);
        int min = sortedByNum[0];
        if (min >= 3)
        {
            return sortedByNum[4];
        }
        else
        {
            if (min == 1)
            {
                if (sortedByNum[1] == 2)
                    return maxUseCard + 2;
                else
                    return maxUseCard + 1;
            }
            else
                return maxUseCard + 2;
        }
    }
    //풀하우스 3
    public bool CanFullHouse(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isFullHouse(comparingCards))
            return false;
        int maxUseCard = CardManager.Instance.maxUseCard;
        int originNum = GetMaxNum_FullHouse(originCards);
        int comPareNum = GetMaxNum_FullHouse(comparingCards);
        if (originNum == 1)
            originNum = maxUseCard + 1;
        else if (originNum == 2)
            originNum = maxUseCard + 2;

        if (comPareNum == 1)
            comPareNum = maxUseCard + 1;
        else if (comPareNum == 2)
            comPareNum = maxUseCard + 2;

        if (originNum < comPareNum)
        {
            TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }
        else if (originNum == comPareNum)
        {
            if (ComPareStyle_Straight(originCards, comparingCards, originNum, comPareNum) == comparingCards)
            {
                TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
                return true;
            }
            else
                return false;
        }
        else
            return false;        
    }

    public int GetMaxNum_FullHouse(List<Card> cards)
    {
        int max = 0;
        int[] sortedByNum = Ascendingorder(cards);
        if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[1] == sortedByNum[2]) && (sortedByNum[3] == sortedByNum[4]))
            max = sortedByNum[0];
        else if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[2] == sortedByNum[3]) && (sortedByNum[3] == sortedByNum[4]))
            max = sortedByNum[2];

        return max;
    }
    //포카드 4
    public bool CanFourCard(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isFourCard(comparingCards))
            return false;
        int maxUseCard = CardManager.Instance.maxUseCard;
        int originNum = GetMaxNum_FourCard(originCards);
        int comPareNum = GetMaxNum_FourCard(comparingCards);
        
        if (originNum == 1)
            originNum = maxUseCard + 1;
        else if (originNum == 2)
            originNum = maxUseCard + 2;

        if (comPareNum == 1)
            comPareNum = maxUseCard + 1;
        else if (comPareNum == 2)
            comPareNum = maxUseCard + 2;
        if (originNum < comPareNum)
        {
            TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
            return true;
        }
        else if (originNum == comPareNum)
        {
            if (ComPareStyle_Straight(originCards, comparingCards, originNum, comPareNum) == comparingCards)
            {
                TurnManager.Instance.SetLastTurnInt(comPareNum % maxUseCard);
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    public int GetMaxNum_FourCard(List<Card> cards)
    {
        int max = 0;
        int[] sortedByNum = Ascendingorder(cards);
        if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[1] == sortedByNum[2]) && (sortedByNum[2] == sortedByNum[3]))
            max = sortedByNum[0];
        else if ((sortedByNum[1] == sortedByNum[2]) && (sortedByNum[2] == sortedByNum[3]) && (sortedByNum[3] == sortedByNum[4]))
            max = sortedByNum[1];

        return max;
    }
    //스트레이트플러쉬 5
    public bool CanStraightFlush(List<Card> originCards, List<Card> comparingCards)
    {
        if (!isStraightFlush(comparingCards))
            return false;
        int maxUseCard = CardManager.Instance.maxUseCard;
        int maxInOrigin = ComPare_Straight(originCards);
        int maxInCompare = ComPare_Straight(comparingCards);

        if (maxInOrigin == 1)
            maxInOrigin = maxUseCard + 1;
        else if (maxInOrigin == 2)
            maxInOrigin = maxUseCard + 2;

        if (maxInCompare == 1)
            maxInCompare = maxUseCard + 1;
        else if (maxInCompare == 2)
            maxInCompare = maxUseCard + 2;

        if (maxInCompare > maxInOrigin)
        {
            TurnManager.Instance.SetLastTurnInt(maxInCompare % maxUseCard);
            return true;
        }
        else if (maxInCompare == maxInOrigin)
        {
            List<Card> list = ComPareStyle_StraightFlush(originCards, comparingCards);
            if (list == comparingCards)
            {
                TurnManager.Instance.SetLastTurnInt(maxInCompare % maxUseCard);
                return true;
            }
            else
                return false;
        }
        else
            return false;

    }

    public int GetMaxNum_StraightFlush(List<Card> cards)
    {
        int max = 0;
        int[] sortedByNum = Ascendingorder(cards);
        if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[1] == sortedByNum[2]) && (sortedByNum[2] == sortedByNum[3]))
            max = sortedByNum[0];
        else if ((sortedByNum[1] == sortedByNum[2]) && (sortedByNum[2] == sortedByNum[3]) && (sortedByNum[3] == sortedByNum[4]))
            max = sortedByNum[1];

        return max;
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
        int maxUseCard = CardManager.Instance.maxUseCard;
        if (cards.Count != 5)
            return false;
        else
        {
            int[] sortedByNum = Ascendingorder(cards);
            
            int min = sortedByNum[0];
                      
            if ((sortedByNum[1] == min + 1) && (sortedByNum[2] == min + 2) && (sortedByNum[3] == min + 3) && (sortedByNum[4] == min + 4))
            {
                if (sortedByNum[1] == 2)
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[1] % maxUseCard);
                else if (sortedByNum[0] == 2)
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[0] % maxUseCard);
                else if (sortedByNum[0] == 1)
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[0] % maxUseCard);
                else
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[4] % maxUseCard);
                return true;
            }
            else return false;
        }
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
        int temp;
        for(int i = 0; i < numArr.Length; i++)
        {
            for (int j = 0; j < numArr.Length - 1; j++)
            {
                if (numArr[j] > numArr[j + 1])
                {
                    temp = numArr[j];
                    numArr[j] = numArr[j + 1];
                    numArr[j + 1] = temp;
                }
            }
        }
        return numArr;

    }

    private int ReturnBigInStraight(int i)
    {
        int max = CardManager.Instance.maxUseCard;
        if (i == 1)
            return 2;
        else if (i == 2)
            return 2;
        else if (i <= 3 && i <= max - 4)
            return i + 4;
        else if (i == max - 3)
            return 1;
        else if (i == max - 2)
            return 2;
        else if (i == max - 1)
            return 2;
        else
            return 2;        
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
                if(sortedByNum[i] == 2)
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[i] % maxUseCard);
                else if(sortedByNum[i] == 1)
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[i] % maxUseCard);
                else
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[4] % maxUseCard);
            }
            
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
            for (int i = 0; i < sortedByNum.Length; i++)
            {
                if (sortedByNum[i] == 1)
                    sortedByNum[i] = maxUseCard + 1;
                if (sortedByNum[i] == 2)
                    sortedByNum[i] = maxUseCard + 2;
            }
            if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[1] == sortedByNum[2]) && (sortedByNum[3] == sortedByNum[4]))
            {
                TurnManager.Instance.SetLastTurnInt(sortedByNum[0] % maxUseCard);
                return true;
            }
            else if ((sortedByNum[0] == sortedByNum[1]) && (sortedByNum[2] == sortedByNum[3]) && (sortedByNum[3] == sortedByNum[4]))
            {
                TurnManager.Instance.SetLastTurnInt(sortedByNum[2] % maxUseCard);
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
                TurnManager.Instance.SetLastTurnInt(sortedByNum[0] % maxUseCard);
                return true;
            }
            else if ((sortedByNum[1] == sortedByNum[2]) && (sortedByNum[2] == sortedByNum[3]) && (sortedByNum[3] == sortedByNum[4]))
            {
                TurnManager.Instance.SetLastTurnInt(sortedByNum[1] % maxUseCard);
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
                if (sortedByNum[1] == 2)
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[1] % maxUseCard);
                else if (sortedByNum[0] == 2)
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[0] % maxUseCard);
                else if (sortedByNum[0] == 1)
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[0] % maxUseCard);
                else
                    TurnManager.Instance.SetLastTurnInt(sortedByNum[4] % maxUseCard);
                return true;
            }
            else
                return false;
        }
    }
}
