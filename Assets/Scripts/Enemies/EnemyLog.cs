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

<<<<<<< Updated upstream

public class EnemyLog : MonoBehaviour {

=======
public class EnemyLog : MonoBehaviour
{
>>>>>>> Stashed changes
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseDamage;
    public float moveSpeed;
    public int enemycount = 9;
    public AudioSource swordhit;
<<<<<<< Updated upstream

=======
    protected bool isDead = false;
>>>>>>> Stashed changes

    private void Awake()
    {
        health = maxHealth.initialValue;
    }
<<<<<<< Updated upstream
    
=======

>>>>>>> Stashed changes
    private void Start()
    {
        health = maxHealth.initialValue;
        swordhit = GetComponent<AudioSource>();
    }

<<<<<<< Updated upstream
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

=======
    internal virtual void TakeDamage(float damage)
    {
        health -= damage;
        swordhit.Play();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (QuestHandler.Instance != null)
        {
            QuestHandler.Instance.NotifyEnemyKilled(enemyName);
        }
        Destroy(gameObject);
        ScoreScript.scoreValue += 1;
    }

>>>>>>> Stashed changes
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement.playerhealth -= 1;
        }
    }
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    public void Knock(Rigidbody2D myRigidbody, float knocktime, float damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knocktime));
        TakeDamage(damage);
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
<<<<<<< Updated upstream
        
    }

=======
    }
>>>>>>> Stashed changes
}