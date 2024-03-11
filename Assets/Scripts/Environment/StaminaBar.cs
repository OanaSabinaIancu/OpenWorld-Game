using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public static StaminaBar Instance { get; private set; }

    private Slider slider;
    public Text staminaCounter;
    public float staminaRegenRate = 3f; // Stamina regeneration rate per second
    public float movementStaminaConsumptionRate = 5f; // Stamina consumption rate while moving

    public GameObject playerState;

    public float currentStamina = 2200f;
    public float maxStamina = 2200f;

    private bool isMoving = false;

    private void Awake()
    {
        slider = GetComponent<Slider>(); // Initialize slider
        Instance = this;
    }

    void Update()
    {
        if (playerState != null)
        {
            // Get player's current and max stamina
            currentStamina = playerState.GetComponent<PlayerState>().currentStamina;
            maxStamina = playerState.GetComponent<PlayerState>().maxStamina;

            // Calculate movement stamina consumption
            float movementStaminaConsumption = isMoving ? movementStaminaConsumptionRate * Time.deltaTime : 0f;

            // Update current stamina based on movement and regeneration rates
            currentStamina = Mathf.Clamp(currentStamina - movementStaminaConsumption + staminaRegenRate * Time.deltaTime, 0f, maxStamina);

            // Update slider value and counter text
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

    // Method to set the movement state (moving or not moving)
    public void SetMovingState(bool moving)
    {
        isMoving = moving;
    }
}
