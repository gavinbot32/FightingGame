using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonCloud : Ability
{
    int damageCooldown;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject != owner.gameObject)
        {
            if (other.CompareTag("Player"))
            {
                damageCooldown -= 1;
                if(damageCooldown <= 0) {
                    damageCooldown = 30;
                    other.GetComponentInChildren<PlayerController>().takeDamage(damage);
                }
                conditionSet(other.gameObject.GetComponentInChildren<PlayerController>());
            }

        }
    }
   

}
