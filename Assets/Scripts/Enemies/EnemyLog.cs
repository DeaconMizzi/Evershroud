using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class EnemyLog : MonoBehaviour
{
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseDamage;
    public float moveSpeed;
    public int enemycount = 9;
    public AudioSource swordhit;

    private void Awake()
    {
        health = maxHealth.initialValue;
    }

    private void Start()
    {
        health = maxHealth.initialValue;
        swordhit = GetComponent<AudioSource>();
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        swordhit.Play();

        if (health <= 0)
        {
            enemycount -= 1;
            Destroy(gameObject);
            ScoreScript.scoreValue += 1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
{
    // Optional — keep it for initial hit damage
    if (collision.gameObject.tag == "Player")
    {
        PlayerMovement.playerhealth -= 1;
    }
}


    // ✅ UPDATED Knock method to include direction-based knockback
    public void Knock(Rigidbody2D myRigidbody, float knocktime, float damage, Vector2 direction)
    {
        if (myRigidbody != null)
        {
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.AddForce(direction.normalized * 5f, ForceMode2D.Impulse); // Tune 5f as needed
            StartCoroutine(KnockCo(myRigidbody, knocktime));
        }

        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knocktime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knocktime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
{
    if (collision.CompareTag("Player") && currentState != EnemyState.stagger)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null && player.currentstate != PlayerState.stagger)
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            player.Knock(0.2f, baseDamage, direction);
        }
    }
}

    void Update()
    {
        
    }
}
