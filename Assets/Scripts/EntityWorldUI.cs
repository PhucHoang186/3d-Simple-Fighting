using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWorldUI : MonoBehaviour
{
    [SerializeField] private Transform worldCanvas;
    [SerializeField] private GameObject locktargetIcon;
    [SerializeField] Vector3 lockOffset;
    private Transform init;

    void Start()
    {
        init = transform.parent;
    }

    public void ToggleLockTargetUI(bool isToggle, Transform target = null)
    {
        locktargetIcon.SetActive(isToggle);

        if (isToggle && target != null)
        {
            worldCanvas.position = target.position + lockOffset;
            worldCanvas.transform.parent = target;
        }
        else
        {
            worldCanvas.transform.parent = init;
        }
    }

}
