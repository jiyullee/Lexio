using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_LineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;



    // Use this for initialization

    void Start()

    {
        //라인렌더러 설정

        lineRenderer = GetComponent<LineRenderer>();

 

        //라인렌더러 처음위치 나중위치

        lineRenderer.SetPosition(0, transform.position);

        lineRenderer.SetPosition(1, transform.position + new Vector3(0, 10, 0));



    }

}
