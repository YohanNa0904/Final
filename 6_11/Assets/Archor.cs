using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archor : MonoBehaviour
{
    public int playerHealth = 100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damageAmount)
    {
        playerHealth -= damageAmount;
        if (playerHealth <= 0)
        {
            // Player is dead, handle accordingly
            // For example, you can end the game or trigger a game over screen
            Debug.Log("Player is dead!");
        }
    }

}
