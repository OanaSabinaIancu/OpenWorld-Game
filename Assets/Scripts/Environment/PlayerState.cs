using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }

    // Player health
    public float currentHealth;
    public float maxHealth;

    // Player stamina
    public float currentStamina;
    public float maxStamina;
    public float sprintStaminaConsumptionRate = 5f; // Lowered sprint stamina consumption rate
    public float staminaRegen = 0.7f;

    private bool isSprinting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        StartCoroutine(DecreaseStamina());
    }

    IEnumerator DecreaseStamina()
    {
        while (true)
        {
            // Check for sprinting input
            isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetMouseButton(1);

            // Only decrease stamina if sprinting
            if (isSprinting && currentStamina > 0)
            {
                currentStamina -= 10;
            }
            else // Regenerate stamina if not sprinting
            {
                currentStamina = Mathf.Min(currentStamina + staminaRegen, maxStamina);
            }
            
            yield return new WaitForSeconds(3); // Wait for 3 seconds before repeating
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            currentHealth -= 50;
        }
    }
}
