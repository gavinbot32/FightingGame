using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisoned : Condition
{
    public float lifetime;

    public int damage;
    private float cooldown;
    public float rod;
    public float dmgStop;
    // Start is called before the first frame update
    void Start()
    {
        cooldown = rod;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            if (owner.curHp > (owner.maxHp * dmgStop))
            {
                owner.takeDamage(damage);
                cooldown = rod;
            }
        }

        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

    }
}
