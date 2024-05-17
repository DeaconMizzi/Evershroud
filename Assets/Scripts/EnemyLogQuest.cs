using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class EnemyLogQuest : Enemy
{
    [SerializeField] 
    private EnemyState currentState;
    [SerializeField]
    protected FloatValue maxHealth;
    [SerializeField]
    protected float health;
    [SerializeField]
    private string enemyName; // Changed to private
    [SerializeField]
    protected int baseDamage;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected int enemycount = 9;
    [SerializeField]
    protected AudioSource swordhit;

    public EnemyState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public string EnemyName
    {
        get { return enemyName; }
        set { enemyName = value; }
    }

    private void Awake()
    {
        health = maxHealth.initialValue;
        // Mark the "Enemy Killed" message as permanent
        Messenger.MarkAsPermanent("Enemy Killed");
    }

    private void Start()
    {
        health = maxHealth.initialValue;
        swordhit = GetComponent<AudioSource>();
        // Debug log to check initialization
        Debug.Log($"Initialized enemy: {enemyName}");
    }

    public void Initialize(string name)
    {
        enemyName = name;
        Debug.Log($"Enemy name set to: {enemyName}");
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        swordhit.Play();

        if (health <= 0)
        {
            Debug.Log($"Enemy {enemyName} is about to be killed.");
            Messenger.Broadcast<Enemy>("Enemy Killed", this);
            enemycount = enemycount - 1;
            Destroy(this.gameObject);
            ScoreScript.scoreValue += 1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement.playerhealth -= 1;
        }
    }

    public void Knock(GameObject hit, float knocktime, float damage)
    {
        Rigidbody2D myRigidbody = hit.GetComponent<Rigidbody2D>();
        if (myRigidbody != null)
        {
            StartCoroutine(KnockCo(myRigidbody, knocktime));
            TakeDamage(damage);
        }
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knocktime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knocktime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Messenger.Broadcast<Enemy>("Enemy Killed", this);
        }
    }
}
