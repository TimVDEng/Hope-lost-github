using UnityEngine;

public class Health_Script : MonoBehaviour
{
    public int healthPoints;
    public GameObject deathEffect;

    private void Update()
    {
        if(healthPoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Destroy(Instantiate(deathEffect, transform.position, transform.rotation), 2);
    }
}
