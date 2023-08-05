using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StaminaBar : MonoBehaviour
{
    private Slider staminaBarSlider;
    [SerializeField] private Slider staminaEffectSlider;

    private void Awake()
    {
         staminaBarSlider= GetComponent<Slider>();
    }

    public void UpdateStaminaBar(float maxStamina, float currentStamina)
    {
        staminaBarSlider.maxValue = maxStamina;
        staminaBarSlider.minValue = 0;
        staminaBarSlider.value = currentStamina;

        staminaEffectSlider.maxValue = maxStamina;
        StaminaEffectAnimation(currentStamina);
    }

    private void StaminaEffectAnimation(float targetValue)
    {
        staminaEffectSlider.DOKill();
        staminaEffectSlider.DOValue(targetValue, 0.2f);
    }
}