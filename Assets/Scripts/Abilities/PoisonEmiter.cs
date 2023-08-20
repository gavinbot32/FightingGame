using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEmiter : Ability
{

    public GameObject cloudPrefab;

    private int updateCount;
    private int nextSpawn;


    // Start is called before the first frame update
    void Start()
    {
        nextSpawn = 10;
        updateCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        updateCount++;
        if(updateCount >= nextSpawn)
        {
            updateCount = 0;
            nextSpawn = Random.RandomRange(90, 180);    
            GameObject cloud = Instantiate(cloudPrefab, transform);
            cloud.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.2f,0.4f));
        }
        
    }
    private void FixedUpdate()
    {

        lifetime -= Time.deltaTime;

        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
