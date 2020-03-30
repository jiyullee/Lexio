using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Lobby_Charge : MonoBehaviour
{
    Random r = new System.Random(Guid.NewGuid().GetHashCode());
    public Lobby_UserInfo Lobby_UserInfo;
    int[] RandMoney = { 10000, 20000, 50000, 100000, 500000, 1000000, 10000000 };
    double[] Weight = { 0.9, 0.04, 0.04, 0.01, 0.008, 0.0018, 0.0002 };

    public Text remainCountText;
    private void Start()
    {
        
    }
    public void OnClick_Charge()
    {
        Lobby_MainCanvasManager.Instance.buttonSound.Play();
        if (Lobby_UserInfo.money == 0)
        {
            if(Lobby_UserInfo.chargeCount > 0)
            {
                Lobby_UserInfo.Charge(SelectRand());             
                
            }
            else
            {
               //충전실패 이유 : 횟수 부족
            }

        }
        else
        {
            //충전실패 : 잔액 있음
        }
    }

    private int SelectRand()
    {
        double sum = 0;
        
        double rand = r.NextDouble();
        for (int i = 0; i < Weight.Length; i++)
        {
            sum += Weight[i];
            if(sum >= rand)
            {
                return RandMoney[i];
            }
        }
        return RandMoney[0];
    }

    public void Onclick_DisappearPanel()
    {
        Lobby_MainCanvasManager.Instance.buttonSound.Play();
        remainCountText.text = "남은 횟수 : " + Lobby_UserInfo.chargeCount + "회";
        gameObject.SetActive(false);
    }
}
