using System.Collections;
using UnityEngine;

public class Viper : EnemyLog
{
    [Header("Viper Settings")]
    public Transform target;
    public float chaseRadius = 6f;
    public float attackRadius = 1.2f;
    public float attackCooldown = 0.5f;

    private bool canAttack = true;
    private bool facingRight = true;

    private void Start()
    {
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (isDead || target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        FlipSprite();

        if (distance <= attackRadius && canAttack && currentState != EnemyState.stagger)
        {
            StartCoroutine(AttackCo());
            return;
        }

        if (distance <= chaseRadius && distance > attackRadius &&
            (currentState == EnemyState.idle || currentState == EnemyState.walk))
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            UpdateAnimDirection(nextPosition - transform.position);
            rb.MovePosition(nextPosition);

            currentState = EnemyState.walk;
            anim.SetBool("wakeUp", true);
        }
        else if (distance > chaseRadius)
        {
            anim.SetBool("wakeUp", false);
            currentState = EnemyState.idle;
        }
    }

    private void FlipSprite()
    {
        if (target == null) return;

        float xDirection = target.position.x - transform.position.x;

        if (xDirection > 0 && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        else if (xDirection < 0 && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void UpdateAnimDirection(Vector3 direction)
    {
        Vector2 dir = direction.normalized;
        anim.SetFloat("moveX", Mathf.Round(dir.x));
        anim.SetFloat("moveY", Mathf.Round(dir.y));
    }

    private IEnumerator AttackCo()
    {
        currentState = EnemyState.attack;
        canAttack = false;
        rb.velocity = Vector2.zero;

        anim.SetTrigger("attack");

        yield return new WaitForSeconds(attackCooldown);

        currentState = EnemyState.idle;
        canAttack = true;
    }

    public override void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        if (swordhit != null) swordhit.Play();

        if (anim != null)
            anim.SetTrigger("hurt");

        if (health <= 0)
        {
            StartCoroutine(ViperDeathCo());
        }
    }

    private IEnumerator ViperDeathCo()
    {
        isDead = true;
        currentState = EnemyState.stagger;

        if (anim != null)
        {
            anim.SetTrigger("death");
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length * 0.9f);
            anim.speed = 0f;
        }

        GetComponent<Collider2D>().enabled = false;
        enemycount -= 1;
        ScoreScript.scoreValue += 1;

        yield return new WaitForSeconds(1f); // 1-second delay
        Destroy(gameObject);
    }
}
