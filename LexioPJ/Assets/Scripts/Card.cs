using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int num;
    public string style;
    private int styleClass;
    Image image;
    Button button;
    bool isSelected;
    public GameObject selectIcon;
    public Sprite backGround;
    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent <Image>();
    }
    public Card(string str, int n)
    {
        style = str;
        num = n;
        SetStyle(style);
        
    }

    public void OnClick_Select()
    {
        if (!isSelected)
        {
            selectIcon.SetActive(true);
            TurnManager.Instance.SelectCard(this);
            isSelected = true;
        }
        else
        {
            selectIcon.SetActive(false);
            TurnManager.Instance.UnSelectCard(this);
            isSelected = false;

        }
            
    }

    public void SetCardInfo(string str, int n)
    {
        style = str;
        num = n;
    }
   
    public void SetStyle(string str)
    {
        style = str;
        if (style == "Sun")
        {
            styleClass = 4;
        }
        else if (style == "Moon")
        {
            styleClass = 3;
        }
        else if (style == "Star")
        {
            styleClass = 2;
        }
        else if (style == "Wind")
        {
            styleClass = 1;
        }
    }

    public Sprite GetImage()
    {
        Sprite sprite = null;
        if (style == "Sun")
        {
            sprite = Game_MainCanvasManager.Instance.SunImages[num - 1];
        }
        else if (style == "Moon")
        {
            sprite = Game_MainCanvasManager.Instance.MoonImages[num - 1];
        }
        else if (style == "Star")
        {
            sprite = Game_MainCanvasManager.Instance.StarImages[num - 1];
        }
        else if (style == "Wind")
        {
            sprite = Game_MainCanvasManager.Instance.WindImages[num - 1];
        }
        return sprite;
    }
    
    public void SetImage()
    {
        image.sprite = GetImage();
    }
    public void SetBgImage()
    {
        selectIcon.SetActive(false);
        image.sprite = backGround;
    }
    public void SetNullImage()
    {
        image.sprite = null;
        button.interactable = true;
        isSelected = false;
    }

    public void SetNumber(int n)
    {
        num = n;
    }

    public int GetNumber()
    {
        return num;
    }
    public string GetStyle()
    {
        return style;
    }

    public int GetStyleClass()
    {
        return styleClass;
    }
}
