using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessSceneBlock : MonoBehaviour
{
    public bool bossIsDead = false;  // Set to true when the boss dies

    void Update()
    {
        // Check if the boss is dead
        if (bossIsDead)
        {
            // Deactivate the block
            gameObject.SetActive(false);
        }
    }
}
