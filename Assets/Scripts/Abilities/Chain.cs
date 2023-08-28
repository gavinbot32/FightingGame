using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chain : Ability
{
    public string[] ignoreTags;
    public bool hooked;

    public GameObject target;
    public Vector3 targetPos;
    Vector3 curPos;
    // Start is called before the first frame update
    void Start()
    {

       
        transform.position = new Vector3(owner.transform.position.x + (spawnOffset.x * owner.transform.localScale.x), owner.transform.position.y + spawnOffset.y, owner.transform.position.z);
        rig.velocity = new Vector2(owner.transform.localScale.x * speed, 0);

        audio.PlayOneShot(playlist[Random.Range(0, 1)]);
    }


    private void FixedUpdate()
    {


      
    }

    // Update is called once per frame
    void Update()
    {
        owner.moveSpeed = 0;
        owner.jumpForce = 0;
        owner.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        owner.GetComponent<Rigidbody2D>().gravityScale = 0;
        lifetime -= Time.deltaTime;
        if(lifetime <= 0 && hooked == false)
        {
            kill();

            Destroy(gameObject);
        }
        if (hooked) {
            

            targetPos = new Vector3(owner.transform.position.x + (spawnOffset.x * owner.transform.localScale.x), owner.transform.position.y + spawnOffset.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            target.transform.position = transform.position;
            PlayerController tarCon = target.GetComponent<PlayerController>();
            tarCon.moveSpeed = 0;
            tarCon.jumpForce = 0;
            target.GetComponent<Rigidbody2D>().gravityScale = 0;
            target.GetComponent<Rigidbody2D>().velocity = new Vector2(target.GetComponent<Rigidbody2D>().velocity.x, 0);

        }
        if(hooked && Vector3.Distance(transform.position,targetPos) <= 0.01f)
        {

            kill();
            Destroy(gameObject);

        }
        if(lifetime < -5)
        {
            kill();
            Destroy(gameObject);
        }
    }

    private void kill()
    {
        owner.resetMods();
        if (target)
        {
            PlayerController tarCon = target.GetComponent<PlayerController>();
            tarCon.resetMods();
            //target.GetComponent<Collider2D>().enabled = true; 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(ignoreTags.Contains(collision.tag))
        {
            return;
        }
        if (collision.gameObject != owner.gameObject && !hooked)
        {
           
            if (collision.gameObject.CompareTag("Player"))
            {
                print("player");
                curPos = transform.position;
                collision.GetComponent<PlayerController>().takeDamage(damage);
                collision.GetComponent<PlayerController>().currentAttacker = owner;
                target = collision.gameObject;
            
                print(target);
                rig.velocity = Vector2.zero;
                //collision.enabled = false;

                hooked = true;
                audio.PlayOneShot(playlist[Random.Range(2, 3)]);


            }
            else
            {
                kill();
                Destroy(gameObject);

            }

        }
    }
}
