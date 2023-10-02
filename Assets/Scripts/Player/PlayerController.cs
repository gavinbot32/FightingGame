using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameManager manager;
    public PauseManager pauseManager;
    public PlayerSettings settings;
    public PlayerSettings backup_settings;

    [Header("Max Values")]
    public int maxHp;
    public int maxJumps;

    [Header("Cur Values")]
    public int curHp;
    public int curJumps;
    public int score;
    public float curMoveInput;
    public bool died;
    public float origGravity;
    [Header("Modifiers")]
    public float moveSpeed;
    public float jumpForce;
    private float moveBase;
    private float jumpBase;

    [Header("Attacking")]
    [SerializeField]
    public PlayerController currentAttacker;
    public float attackRate;
    public float lastAttackTime;
    public int attackSpeed;
    public float attackDmg;
    public GameObject attackPrefab;
    public GameObject passivePrefab;
    private bool canMelee;
    [SerializeField]
    [Header("Components")]
    public int skinIndex;
    public Rigidbody2D rig;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private AudioSource audi;
    public AudioClip[] playlist;
    [SerializeField]
    private Transform muzzle;
    public PlayerContainerUI uiContainer;
    public PlayerControlUI uiControls;
    public GameObject deathParticle;

    public GameObject spriteObj;

    [Header("Attack Components")]
    public GameObject meleePrefab;
    public float meleeSeconds;
    public float meleeROA;
    public float lastMelee;
    public int meleeDamage;
    //Jump = 0-2, hurt = 3-5, Taunt 1 = 6-8, death 9-10


    [Header("Colors")]
    Color damageRed = new Color(255f, 153f, 153f, 255f);

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        pauseManager = FindObjectOfType<PauseManager>();
        audi = GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody2D>();
        muzzle = GameObject.FindGameObjectWithTag("Muzzle").GetComponent<Transform>();
        if (settings.attackPrefabs == null || settings.attackPrefabs.Length <= 0)
        {
            attackPrefab = backup_settings.attackPrefabs[Random.Range(0, backup_settings.attackPrefabs.Length)];
        }
        else
        {
            attackPrefab = settings.attackPrefabs[Random.Range(0, settings.attackPrefabs.Length)];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        origGravity = rig.gravityScale;
        moveBase = moveSpeed;
        jumpBase = jumpForce;
        anim = GetComponent<Animator>();
        curHp = maxHp;
        curJumps = maxJumps;
        died = false;
        score = 0;
        uiContainer.updateHealthBar(curHp, maxHp);
        canMelee = true;
    }

    // Update is called once per frame
    void Update()
    {

        deathCheck();
        if (attackPrefab != null)
        {
            uiContainer.updateChargeBar(Time.time - lastAttackTime, attackPrefab.GetComponent<Ability>().rof);
        }
    }

    public void deathCheck()
    {
        if (transform.position.y < -10|| curHp <= 0)
        {
            die(true);
            died = true;
        }
    }
    private void FixedUpdate()
    {
        move();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D x in collision.contacts)
        {
            if (x.collider.CompareTag("Ground"))
            {
                if(x.point.y < transform.position.y)
                {
                    int index = Random.Range(3, 5);

                    audi.PlayOneShot(playlist[index]);

                    curJumps = maxJumps;
                    died = false;

                }
                if((x.point.x > transform.position.x || x.point.x < transform.position.x)&&(x.point.y < transform.position.y))
                {
                    if(curJumps < maxJumps)
                    {
                        curJumps++;
                    }
                }
            }
        }
    }
    public void jump()
    {
        rig.velocity = new Vector2(rig.velocity.x, 0);

        int index = Random.Range(0, 2);

        audi.PlayOneShot(playlist[index]);

        rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    public void move()
    {

        //When Holding down L joystick it will not change dirction if you "orbit" to the other side while still on it.
        rig.velocity = new Vector2(curMoveInput * moveSpeed, rig.velocity.y);


        if(curMoveInput != 0.0f)
        {
            anim.SetBool("isWalking", true);
            transform.localScale = new Vector3(curMoveInput > 0 ? 1 : -1, 1, 1);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
    public void die(bool fallOff)
    {
        audi.PlayOneShot(playlist[Random.Range(9,11)]);
        foreach (Condition con in GetComponentsInChildren<Condition>())
        {
            if (con)
            {
                Destroy(con.gameObject);
            }
        }
        
        if(currentAttacker != null)
        {
            currentAttacker.addScore();
        }
        if (fallOff)
        {
            score--;
        }
        if(score < 0)
        {
            score = 0;
        }
        uiContainer.updateScore(score);

        Vector3 parVector = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        if(transform.position.y <= -10)
        {
            parVector.y = -6.5f;
        }

        GameObject par = Instantiate(deathParticle, parVector, Quaternion.identity);
        Destroy(par, 2);
        respawn();
    }
    public void addScore()
    {
        score++;
        uiContainer.updateScore(score);
    }
    public void takeDamage(int amount)
    {
        curHp -= amount;
        anim.SetTrigger("isHurt");
        int index = Random.Range(3, 5);
        StartCoroutine(MeleeBoolCour());
        audi.PlayOneShot(playlist[index]);
        uiContainer.updateHealthBar(curHp, maxHp);

    }
    public void takeDamage(float amount)
    {
        curHp -= (int)amount;
        anim.SetTrigger("isHurt");
        int index = Random.Range(3, 5);
        StartCoroutine(MeleeBoolCour());
        audi.PlayOneShot(playlist[index]);
        uiContainer.updateHealthBar(curHp, maxHp);

    }

    public void youSuck()
    {
        Destroy(uiContainer.gameObject);
        Destroy(gameObject);
    }
    private void respawn()
    {
        resetMods(true);
        currentAttacker = null;
        transform.position = manager.spawnPoints[Random.Range(0, manager.spawnPoints.Length)].position;
        uiContainer.updateHealthBar(curHp, maxHp);

    }

    
    public void resetMods(bool fullAccess)
    {
        curHp = maxHp;
        curJumps = maxJumps;
        moveSpeed = moveBase;
        jumpForce = jumpBase;
        rig.gravityScale = origGravity;
        canMelee = true;
    }

    public void resetMods()
    {
       
        
        moveSpeed = moveBase;
        jumpForce = jumpBase;
        rig.gravityScale = origGravity;

    }

    public void spawnAbilityPrefab()
    {
        Ability ability = attackPrefab.GetComponent<Ability>();
        GameObject prefab = Instantiate(attackPrefab, new Vector3(transform.position.x+ability.spawnOffset.x, transform.position.y+ability.spawnOffset.y, transform.position.z), Quaternion.identity);
        prefab.GetComponent<Ability>().owner = this;

    }

    public void onPunchAttack()
    {
        /*Vector3 orjSpritePos = spriteObj.transform.position;
        print(orjSpritePos);
        Vector3 targetPos = new Vector3(orjSpritePos.x + 2f, orjSpritePos.y, orjSpritePos.z);
        print(targetPos);
        spriteObj.transform.position = targetPos;*/
        anim.SetTrigger("punch");
        StartCoroutine(PunchCour());
    }

    private IEnumerator PunchCour()
    {
       
        meleePrefab.SetActive(true);
        yield return new WaitForSeconds(meleeSeconds);
//        spriteObj.transform.position = orjSpritePos;
        meleePrefab.SetActive(false);
    }

    private IEnumerator MeleeBoolCour()
    {
        canMelee = false;
        yield return new WaitForSeconds(0.5f);
        canMelee = true;
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Melee_Attack")&&collision.gameObject != meleePrefab)
        {
           
            takeDamage(meleeDamage);
            currentAttacker = collision.transform.parent.GetComponent<PlayerController>();
        }
    }
    public void setUIContainer(PlayerContainerUI pcu)
    {
        this.uiContainer = pcu;
    }
    public void setUIControls(PlayerControlUI pcu)
    {
        this.uiControls = pcu;
    }
    public void setCondition(Condition condition)
    {
        GameObject con = Instantiate(condition.gameObject, transform.position, Quaternion.identity);
        con.transform.SetParent(transform);
        con.GetComponent<Condition>().owner = this;
    }
    public void onJumpInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            print("pressed jump");
            if(curJumps > 0)
            {
                curJumps--;
                jump();
            }
        }
    }
    public void onMoveInput(InputAction.CallbackContext context)
    {


       float x = context.ReadValue<float>();
        if(x > 0 || x < 0) {
            if (x >= 0.45f)
            {
                curMoveInput = 1;
            }
            else if (x <= -0.45f)
            {
                curMoveInput = -1;
            }
            else
            {
                curMoveInput = context.ReadValue<float>();
            }
        }
        else
        {
            curMoveInput = 0;
        }
        print(curMoveInput);
    }
    public void onBlockInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            print("pressed block");
           
        }
    }  
    public void onPunchInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && Time.time - lastMelee > meleeROA && canMelee)
        {
            lastMelee = Time.time;
            onPunchAttack();
           
        }
    }
    public void onRangedInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            print("pressed Ranged");

        }
    }
    public void onAbilityInput(InputAction.CallbackContext context)
    {
        if (attackPrefab == null) { return; } 
        if (context.phase == InputActionPhase.Performed && Time.time - lastAttackTime > attackPrefab.GetComponent<Ability>().rof)
        {
           
            lastAttackTime = Time.time;
            spawnAbilityPrefab();
        }
    }
    public void onTauntInput(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Performed)
        {
            int index = Random.Range(6, 8);

            audi.PlayOneShot(playlist[index]);

        }
    }
    public void onPauseInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            pauseManager.onPause();
        }   
    }
    public void onTabInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            uiControls.mainContainer.gameObject.SetActive(true);
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            uiControls.mainContainer.gameObject.SetActive(false);
        }
    }



}
