using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnState : MonoBehaviourPun
{
    Text stateText;
    float lifeTime = 2.0f;
    Image image;
    private void Awake()
    {
        stateText = GetComponentInChildren<Text>();
        image = GetComponent<Image>();        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(lifeTime);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        
    }

    IEnumerator SwitchDisable()
    {
        while (true)
        {
            yield return null;
            if (TurnManager.Instance.isNewTurn)
            {
                gameObject.SetActive(false);
                break;
            }

        }

    }
    public void ChangeText(string str)
    {
        stateText.text = str;
        if (str == "패스")
            image.color = Color.red;
        else if(str == "등록")
            image.color = Color.yellow;
        StartCoroutine(Wait());
    }
}
