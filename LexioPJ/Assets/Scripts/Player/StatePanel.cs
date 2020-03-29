using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatePanel : MonoBehaviour
{
    public Image childImage;
    public Text stateText;
    void Start()
    {
        
    }

    public void ShowState(string str)
    {
        childImage.gameObject.SetActive(true);
        SetStateText(str);
        
    }

    IEnumerator UnShownState()
    {
        yield return new WaitForSeconds(2.0f);
        NoShowState();
    }

    public void NoShowState()
    {
        childImage.gameObject.SetActive(false);
    }

    private void SetStateText(string str)
    {
        stateText.text = str;
    }


}
