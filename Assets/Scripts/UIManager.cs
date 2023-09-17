using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Space(10)]
    [SerializeField] Image healthBar;
    [SerializeField] Image manaBar;
    private int healthUICount;
    private int manaUICount;
    private int maxHealthUICount;
    private int maxManaUICount;

    // property
    public int HealthUICount
    {
        get => healthUICount;
        set
        {
            healthUICount = value;
            healthBar.fillAmount = healthUICount / maxHealthUICount;
        }
    }
    public int ManaUICount
    {
        get => manaUICount;
        set
        {
            manaUICount = value;
            healthBar.fillAmount = manaUICount / maxManaUICount;
        }
    }

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
