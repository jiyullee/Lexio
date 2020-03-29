using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CanvasManager : MonoBehaviour
{
    public Transform canvasPos;
    private void Start()
    {
        if(!transform.IsChildOf(Game_MainCanvasManager.Instance.transform))
            GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        transform.localPosition = canvasPos.localPosition;
        GetComponent<RectTransform>().localPosition = canvasPos.GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().sizeDelta = canvasPos.GetComponent<RectTransform>().sizeDelta;


    }
}
