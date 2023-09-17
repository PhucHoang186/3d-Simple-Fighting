using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TakeScreenShot : MonoBehaviour
{
    [SerializeField] int screenshotWidth;
    [SerializeField] int screenshotHeight;
    [SerializeField] Camera mainCamera;

    [Button]
    public void TakeScreenshot()
    {
        RenderTexture renderTexture = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        mainCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        mainCamera.Render();
        Texture2D texture2D =  new Texture2D(screenshotWidth, screenshotHeight);
        texture2D.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        byte[] byteArray = texture2D.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/cameracapture.png", byteArray);
    }
}
