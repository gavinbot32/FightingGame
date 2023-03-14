using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : Condition
{
    public float lifetime;
    
    // Start is called before the first frame update
    void Start()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        if(lifetime >= 0)
        {
            owner.moveSpeed = 0.5f;
            owner.jumpForce = 0f;
        }
        else
        {
            owner.resetMods();
            Destroy(gameObject);
        }
        

    }
}
