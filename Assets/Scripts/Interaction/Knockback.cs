using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust = 5f;
    public float knocktime = 0.2f;
    public float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = (hit.transform.position - transform.position).normalized;

                // Knock enemy
                if (collision.gameObject.CompareTag("Enemy") && collision.isTrigger)
                {
                    hit.velocity = Vector2.zero;
                    hit.AddForce(difference * thrust, ForceMode2D.Impulse);

                    EnemyLog enemy = collision.GetComponent<EnemyLog>();
                    if (enemy != null)
                    {
                        enemy.currentState = EnemyState.stagger;
                        enemy.Knock(hit, knocktime, damage, difference); // New overload with direction
                    }
                }

                // Knock player
                if (collision.gameObject.CompareTag("Player"))
                {
                    PlayerMovement player = collision.GetComponent<PlayerMovement>();
                    if (player != null && player.currentstate != PlayerState.stagger)
                    {
                        hit.velocity = Vector2.zero;
                        hit.AddForce(difference * thrust, ForceMode2D.Impulse);
                        player.currentstate = PlayerState.stagger;
                        player.Knock(knocktime, damage, difference);

                    }
                }
            }
        }
    }
}
