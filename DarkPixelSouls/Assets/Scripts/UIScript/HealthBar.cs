using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
     private Slider healthBarSlider;
    [SerializeField] private Slider damageEffectSlider;

    private void Awake()
    {
        healthBarSlider = GetComponent<Slider>();
    }

    public void UpdateHealthbar(int maxHealth,int currentHealth)
    {
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.minValue = 0;
        healthBarSlider.value = currentHealth;

        damageEffectSlider.maxValue = maxHealth;
        DamageEffectAnimation(currentHealth);
    }

    private void DamageEffectAnimation(int targetValue)
    {
        damageEffectSlider.DOKill();
        damageEffectSlider.DOValue(targetValue, 0.2f);
    }
}