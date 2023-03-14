using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ability
{

    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(owner.transform.position.x + (spawnOffset.x*owner.transform.localScale.x), owner.transform.position.y + spawnOffset.y, owner.transform.position.z);
        rig.velocity = new Vector2(owner.transform.localScale.x * speed, 0);
        Destroy(gameObject, lifetime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject != owner.gameObject)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().takeDamage(damage);
                collision.GetComponent<PlayerController>().currentAttacker = owner;
                conditionSet(collision.GetComponent<PlayerController>());
                
            }
            Destroy(gameObject);

        }
    }
}
