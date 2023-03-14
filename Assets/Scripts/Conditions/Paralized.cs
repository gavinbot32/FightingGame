using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralized : Condition
{
    public float lifetime;
    public float origSpeed;
    public float origJump;
    // Start is called before the first frame update
    void Start()
    {
        origJump = owner.jumpForce;
        origSpeed = owner.moveSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime >= 0)
        {
            owner.moveSpeed = 0f;
            owner.jumpForce = 0f;
        }
        else
        {
            owner.moveSpeed = origSpeed;
            owner.jumpForce = origJump;
            Destroy(gameObject);
        }


    }
}
