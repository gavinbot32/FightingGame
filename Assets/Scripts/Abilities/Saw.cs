using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : Ability
{
    bool locked;
    float origJump;
    float origSpeed;
    float cooldown;
    float rotZ; //45 135
    public float rod;
    PlayerController target;

    public Transform groundEffect;

    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = new Vector2(owner.transform.localScale.x * speed,0);
        transform.localScale = new Vector2(transform.localScale.x * owner.transform.localScale.x, transform.localScale.y);
        transform.position = new Vector3 (transform.position.x,Mathf.RoundToInt(transform.position.y), transform.position.z);
        if(transform.localScale.x == 2) {
            rotZ = 45f;
        }
        else
        {
            rotZ = 135f;
        }
        groundEffect.rotation = Quaternion.Euler(Vector3.forward * rotZ);

        groundEffect.gameObject.SetActive(false);

        audio.PlayOneShot(playlist[0]);
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

        rayCastChecks();

    }

    private void rayCastChecks()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y -0.25f), Vector2.up, 0.25f);
        RaycastHit2D hitDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.25f), Vector2.down, 0.25f);

        if (hitUp)
        {
            if (hitUp.collider.gameObject.CompareTag("Ground"))
            {
                print(hitUp + "Up");
                groundEffect.rotation = Quaternion.Euler(Vector3.forward * (rotZ * -1));

                return;
            }
        }
        if (hitDown)
        {
   
            if (hitDown.collider.gameObject.CompareTag("Ground"))
            {
                print(hitDown + "down");
                groundEffect.rotation = Quaternion.Euler(Vector3.forward * rotZ);

                return;
            }
        }
    }

    private void onLock()
    {
        if(target != null)
        {
            if (target.died)
            {
                locked = false;
                target = null;
                return;
            }
            target.moveSpeed = 0f;
            target.jumpForce = 0f;
            target.rig.gravityScale = 0f;
            target.rig.velocity = Vector2.zero;
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
                audio.PlayOneShot(playlist[1]);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            groundEffect.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundEffect.gameObject.SetActive(false);
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
                target.rig.gravityScale = target.origGravity;
            }
            Destroy(gameObject);
        }
    }



}
