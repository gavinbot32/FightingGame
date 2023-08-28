using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : Ability
{
    bool locked;
    float origJump;
    float origSpeed;
    float cooldown;
    public float rod;
    PlayerController target;
    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = new Vector2(owner.transform.localScale.x * speed,0);
        transform.position = new Vector3 (transform.position.x,Mathf.RoundToInt(transform.position.y), transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        
        if (locked)
        {
            onLock();
        }
        lifeCheck();

    }

    private void onLock()
    {
        if(target != null)
        {
            target.moveSpeed = 0f;
            target.jumpForce = 0f;
            rig.velocity = new Vector2(0, 0);
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
            {
                target.takeDamage(damage);
                cooldown = rod;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject != owner.gameObject)
        {
            locked = true;
            if(target == null) { 
                target = collision.gameObject.GetComponent<PlayerController>();
                origJump = target.jumpForce;
                origSpeed = target.moveSpeed;
            }
        }    
    }


    public void lifeCheck()
    {
        if (lifetime <= 0)
        {
            if (target)
            {
                target.moveSpeed = origJump;
                target.jumpForce = origSpeed;
            }
            Destroy(gameObject);
        }
    }



}
