using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Camera mainCam;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
