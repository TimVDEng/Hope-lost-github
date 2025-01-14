using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForDamage : MonoBehaviour
{
    public bool canDamage;
    public Health_Script health;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Health_Script>() != null)
        {
            health = other.GetComponent<Health_Script>();
            canDamage = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject == health.gameObject)
        {
            health = null;
            canDamage = false;
        }
    }
}
