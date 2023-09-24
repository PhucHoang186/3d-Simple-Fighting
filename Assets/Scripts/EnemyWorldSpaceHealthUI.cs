using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldSpaceHealthUI : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] private TMP_Text healthText;
    private float maxHealth;
    private float currentHealth;

    public float Health
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            healthText.text = String.Format("{0}/{1}", currentHealth, maxHealth);
            healthBar.fillAmount = currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public void Init(float maxHealth)
    {
        this.maxHealth = maxHealth;
        Health = maxHealth;
    }
}
