using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider slider;
    public Text staminaCounter;

    public GameObject playerState;

    private float currentStamina;
    private float maxStamina;

    public float getStamina()
    {
        return currentStamina;
    }

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (playerState != null)
        {
            currentStamina = playerState.GetComponent<PlayerState>().currentStamina;
            maxStamina = playerState.GetComponent<PlayerState>().maxHealth;

            float fillValue = currentStamina / maxStamina;
            slider.value = fillValue;

            staminaCounter.text = currentStamina + "/" + maxStamina;

            // Change slider background color based on the slider value
            ChangeSliderBackgroundColor(fillValue);
        }
        else
        {
            Debug.Log("Player state object is not assigned to StaminaBar.");
        }
    }

    void ChangeSliderBackgroundColor(float fillValue)
    {
        Image sliderBackground = slider.fillRect.GetComponentInParent<Image>();

        if (fillValue < 0.3f)
        {
            // Set slider background color to red
            sliderBackground.color = Color.gray;
        }
        else if (fillValue < 0.75f)
        {
            // Set slider background color to yellow
            sliderBackground.color = Color.cyan;
        }
        else
        {
            // Set slider background color to green
            sliderBackground.color = Color.blue;
        }
    }
}
