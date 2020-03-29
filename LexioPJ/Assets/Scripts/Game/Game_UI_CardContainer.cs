using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game_UI_CardContainer : MonoBehaviour
{
    public Card[] Slots;

    private void Awake()
    {
        Slots = GetComponentsInChildren<Card>();
    }
}
