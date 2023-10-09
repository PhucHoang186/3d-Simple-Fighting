using System;
using System.Collections;
using System.Collections.Generic;
using Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldSpaceHealthUI : MonoBehaviour
{
    [SerializeField] EntityHandleHealth handleHealth;
    [SerializeField] Image healthBar;
    [SerializeField] private TMP_Text healthText;

    public void Start()
    {
        handleHealth.InitActions(OnHit, OnDestroyed);
    }

    private void OnDestroyed()
    {
        this.gameObject.SetActive(false);
    }

    private void OnHit(float currentHealth, float maxHealth)
    {
        healthText.text = String.Format("{0}/{1}", currentHealth, maxHealth);
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
