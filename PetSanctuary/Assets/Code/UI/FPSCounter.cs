using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsDisplay; // Assign this in the Unity Inspector
    private float deltaTime = 0.0f;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        // Calculate deltaTime
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate FPS
        float fps = 1.0f / deltaTime;

        // Update the display text
        fpsDisplay.text = string.Format("FPS: {0:0.}", fps);
    }
}
