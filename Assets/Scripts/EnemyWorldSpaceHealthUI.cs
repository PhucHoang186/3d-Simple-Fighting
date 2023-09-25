using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldSpaceHealthUI : MonoBehaviour
{
    [SerializeField] EntityHandleTakenDamage handleDamage;
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
        }
    }

    public void Start()
    {
        InitHealth();
        handleDamage.InitActions(OnHit, OnDestroyed);
    }

    private void InitHealth()
    {
        this.maxHealth = handleDamage.MaxHealth;
        Health = maxHealth;
    }

    private void OnDestroyed()
    {
        this.gameObject.SetActive(false);
    }

    private void OnHit(float damageAmount)
    {
        Health -= damageAmount;
    }
}
