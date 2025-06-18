using System.Collections;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class EnemyLog : MonoBehaviour
{
    [Header("Base Stats")]
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    public string enemyName;
    public int baseDamage;
    public float moveSpeed;
    public int enemycount = 9;

    [Header("FX")]
    public AudioSource swordhit;

    protected bool isDead = false;
    protected Animator anim;
    protected Rigidbody2D rb;

    private void Awake()
    {
        health = maxHealth.initialValue;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        health = maxHealth.initialValue;
        swordhit = GetComponent<AudioSource>();
    }

    public void Knock(Rigidbody2D myRigidbody, float knocktime, float damage, Vector2 direction)
    {
        if (isDead) return;

        if (myRigidbody != null)
        {
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.AddForce(direction.normalized * 5f, ForceMode2D.Impulse);
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

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        if (swordhit != null) swordhit.Play();

        if (anim != null)
            anim.SetTrigger("hurt");

        if (health <= 0)
        {
            StartCoroutine(DeathCo());
        }
    }

    protected virtual IEnumerator DeathCo()
    {
        isDead = true;
        currentState = EnemyState.stagger;

        if (anim != null)
        {
            anim.SetTrigger("death");
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length * 0.9f);
            anim.speed = 0f; // freeze on last frame
        }

        GetComponent<Collider2D>().enabled = false;
        enemycount -= 1;
        ScoreScript.scoreValue += 1;

        Destroy(gameObject); // default destroy
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement.playerhealth -= 1;
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
}
