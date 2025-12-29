using UnityEngine;
using UnityEngine.UI;

public class UIScaleApplier : MonoBehaviour
{
    //attach this to every canvas
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int ui = PlayerPrefs.GetInt("UISize", 1);

        Vector2 baseRes = new Vector2(1920, 1080);

        float multiplier =
            ui == 0 ? 0.9f :   // Small
            ui == 1 ? 1.0f :   // Medium
                      1.1f;    // Large

        CanvasScaler scaler = GetComponent<CanvasScaler>();
        scaler.referenceResolution = baseRes * multiplier;

    }

   
}
