using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public Text healthCounter;

    public GameObject playerState;

    private float currentHealth;
    private float maxHealth;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (playerState != null)
        {
            currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
            maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

            float fillValue = currentHealth / maxHealth;
            slider.value = fillValue;

            healthCounter.text = currentHealth + "/" + maxHealth;

            // Change slider background color based on the slider value
            ChangeSliderBackgroundColor(fillValue);
        }
        else
        {
            Debug.Log("Player state object is not assigned to HealthBar.");
        }
    }

    void ChangeSliderBackgroundColor(float fillValue)
    {
        Image sliderBackground = slider.fillRect.GetComponentInParent<Image>();

        if (fillValue < 0.3f)
        {
            // Set slider background color to red
            sliderBackground.color = Color.blue;
        }
        else if (fillValue < 0.75f)
        {
            // Set slider background color to yellow
            sliderBackground.color = Color.cyan;
        }
        else
        {
            // Set slider background color to green
            sliderBackground.color = Color.magenta;
        }
    }
}
