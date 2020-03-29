using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int num;
    public string style;
    public int styleClass;
    Image image;
    Button button;
    bool isSelected;
    public GameObject selectIcon;
    public Sprite backGround;
    public int PosNum;
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

    public void UnselectCard()
    {
        selectIcon.SetActive(false);
        TurnManager.Instance.UnSelectCard(this);
        isSelected = false;
    }
    public void SetCardInfo(string str, int n)
    {
        num = n;
        SetStyle(str);
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
        return image.sprite;
    }
    public Sprite GetImage(string style, int num)
    {
        Sprite sprite = backGround;

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
        Sprite sprite = backGround;

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
        image.sprite = sprite;
    }
    public void SetImage(Sprite sprite)
    {
        if (sprite != backGround)
            button.interactable = true;
        else
            button.interactable = false;
        image.sprite = sprite;
    }
    public void SetBgImage()
    {
        selectIcon.SetActive(false);
        image.sprite = backGround;
        button.interactable = false;
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

    public void SetStyleClass(int n)
    {
        styleClass = n;
    }
    public int GetStyleClass()
    {
        return styleClass;
    }

    public void SetPosNum(int n)
    {
        PosNum = n;
    }
    public int GetPosNum()
    {
        return PosNum;
    }
}
