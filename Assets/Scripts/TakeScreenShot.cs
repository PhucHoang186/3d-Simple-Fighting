using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TakeScreenShot : MonoBehaviour
{
    [SerializeField] int screenshotWidth;
    [SerializeField] int screenshotHeight;
    [SerializeField] Camera mainCamera;
    [SerializeField] string screenshotName;
    [SerializeField] string saveFolderName;
    [SerializeField] string folderName;
    [SerializeField] Image previewImage;

    [Button]
    public void TakeScreenshot()
    {
        RenderTexture renderTexture = new RenderTexture(screenshotWidth, screenshotHeight, 24);
        mainCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        mainCamera.Render();
        Texture2D texture2D = new Texture2D(screenshotWidth, screenshotHeight);
        texture2D.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        byte[] byteArray = texture2D.EncodeToPNG();
        string path = Application.dataPath + saveFolderName + screenshotName + ".png";
        string relativePath = folderName + screenshotName + ".png";
        System.IO.File.WriteAllBytes(path, byteArray);
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(path);

        TextureImporter importer = AssetImporter.GetAtPath(relativePath) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.maxTextureSize = 512;
        AssetDatabase.WriteImportSettingsIfDirty(relativePath);

        previewImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
    }
}
