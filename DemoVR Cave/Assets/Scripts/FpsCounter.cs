using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; 

public class FpsCounter : MonoBehaviour
{
    private int frameCounter = 0;
    private float timeCounter = 0.0f;
    private float refreshTime = 0.1f;
    
    [SerializeField]
    private TextMeshProUGUI framerateText; 

    void Update()
    {
        if (timeCounter < refreshTime)
        {
            timeCounter += Time.deltaTime;
            frameCounter++;
        }
        else
        {
            float lastFramerate = frameCounter / timeCounter;
            frameCounter = 0;
            timeCounter = 0.0f;
            framerateText.text = lastFramerate.ToString("n2");
        }
    }
}