using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    //player health

    public float currentHealth;
    public float maxHealth;

    //player stamina

    public float currentStamina;
    public float maxStamina;

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
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.N)) 
        {
            currentHealth -= 50;
        }
    }
}
