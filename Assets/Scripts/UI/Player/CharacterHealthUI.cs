using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("UI Components")]
    [SerializeField] private Image healthBarImage;
    [Space]
    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [Header("Settings")]
    [SerializeField, Range(0.01f, 10f)] private float lerpSpeed;

    private float LERP_THRESHOLD = 0.01f;

    public int maxHealth;
    public int currentHealth;

    public float targetFill;
    public float currentFill;

    private void OnEnable()
    {
        playerHealth.OnPlayerStatsInitialized += PlayerHealth_OnPlayerStatsInitialized;
        playerHealth.OnPlayerStatsUpdated += PlayerHealth_OnPlayerStatsUpdated;

        playerHealth.OnPlayerHealthTakeDamage += PlayerHealth_OnPlayerHealthTakeDamage;
        playerHealth.OnPlayerHeal += PlayerHealth_OnPlayerHeal;

        playerHealth.OnPlayerMaxHealthChanged += PlayerHealth_OnPlayerMaxHealthChanged;
        playerHealth.OnPlayerCurrentHealthClamped += PlayerHealth_OnPlayerCurrentHealthClamped;
    }

    private void OnDisable()
    {
        playerHealth.OnPlayerStatsInitialized -= PlayerHealth_OnPlayerStatsInitialized;
        playerHealth.OnPlayerStatsUpdated -= PlayerHealth_OnPlayerStatsUpdated;

        playerHealth.OnPlayerHealthTakeDamage -= PlayerHealth_OnPlayerHealthTakeDamage;
        playerHealth.OnPlayerHeal -= PlayerHealth_OnPlayerHeal;

        playerHealth.OnPlayerMaxHealthChanged -= PlayerHealth_OnPlayerMaxHealthChanged;
        playerHealth.OnPlayerCurrentHealthClamped -= PlayerHealth_OnPlayerCurrentHealthClamped;
    }

    private void Update()
    {
        HandleHealthBarLerping();
    }

    private void HandleHealthBarLerping()
    {
        if (Mathf.Abs(currentFill - targetFill) <= LERP_THRESHOLD) return;

        float newCurrentFill = (Mathf.Lerp(currentFill, targetFill, lerpSpeed * Time.deltaTime));

        SetCurrentFill(newCurrentFill);
        SetHealthBarFill(currentFill);
    }

    private void UpdateHealthValues(int currentHealth, int maxHealth)
    {
        SetCurrentHealth(currentHealth);
        SetMaxHealth(maxHealth);
    }

    private void UpdateUIByHealthValues()
    {
        SetHealthBarTexts(currentHealth, maxHealth); //Change Texts
        SetTargetFill(CalculateTargetFill(currentHealth, maxHealth));// Change Target Fill      
    }

    private void SetMaxHealth(int maxHealth) => this.maxHealth = maxHealth;
    private void SetCurrentHealth(int currentHealth) => this.currentHealth = currentHealth;
    private void SetTargetFill(float fill) => targetFill = fill;
    private void SetCurrentFill(float fill) => currentFill = fill;

    private float CalculateTargetFill(int currentHealth, int maxHealth) => (float)currentHealth / maxHealth;

    private void SetHealthBarFill(float fillAmount)
    {
        UIUtilities.SetImageFillRatio(healthBarImage, fillAmount);
    }

    private void SetHealthBarTexts(int currentHealth, int maxHealth)
    {
        currentHealthText.text = currentHealth.ToString();
        maxHealthText.text = maxHealth.ToString();
    }

    #region Subscriptions
    private void PlayerHealth_OnPlayerStatsInitialized(object sender, EntityHealth.OnEntityStatsEventArgs e)
    {
        UpdateHealthValues(e.currentHealth, e.maxHealth);
        UpdateUIByHealthValues();
        //SetCurrentFill(targetFill); //Update Current Fill Immediately
    }

    private void PlayerHealth_OnPlayerStatsUpdated(object sender, EntityHealth.OnEntityStatsEventArgs e)
    {
        UpdateHealthValues(e.currentHealth, e.maxHealth);
        UpdateUIByHealthValues();
    }

    private void PlayerHealth_OnPlayerHealthTakeDamage(object sender, EntityHealth.OnEntityHealthTakeDamageEventArgs e)
    {
        UpdateHealthValues(e.newHealth, e.maxHealth);
        UpdateUIByHealthValues();
    }

    private void PlayerHealth_OnPlayerHeal(object sender, EntityHealth.OnEntityHealEventArgs e)
    {
        UpdateHealthValues(e.newHealth, e.maxHealth);
        UpdateUIByHealthValues();
    }

    private void PlayerHealth_OnPlayerMaxHealthChanged(object sender, EntityHealth.OnEntityStatsEventArgs e)
    {
        UpdateHealthValues(e.currentHealth, e.maxHealth);
        UpdateUIByHealthValues();
    }


    private void PlayerHealth_OnPlayerCurrentHealthClamped(object sender, EntityHealth.OnEntityStatsEventArgs e)
    {
        UpdateHealthValues(e.currentHealth, e.maxHealth);
        UpdateUIByHealthValues();
    }
    #endregion
}
