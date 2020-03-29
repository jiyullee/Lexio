using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_Rating : MonoBehaviour
{
    public Text Rank;
    public Text Name;
    public Text Value;

    public void SetText(string rank, string name, string value)
    {
        Rank.text = rank;
        Name.text = name;
        Value.text = value;
    }
}
