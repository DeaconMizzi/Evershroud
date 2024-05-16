using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knocktime;
    public float damage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = (hit.transform.position - transform.position);
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);

                if (collision.gameObject.CompareTag("enemy") && collision.isTrigger)
                {
                    EnemyLog enemyLog = collision.GetComponent<EnemyLog>();
                    if (enemyLog != null)
                    {
                        enemyLog.CurrentState = EnemyState.stagger;  // Use the property
                        enemyLog.Knock(hit.gameObject, knocktime, damage);
                    }
                }

                if (collision.gameObject.CompareTag("Player"))
                {
                    PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
                    if (playerMovement != null && playerMovement.currentstate != PlayerState.stagger)
                    {
                        playerMovement.currentstate = PlayerState.stagger;
                        playerMovement.Knock(knocktime, damage);
                    }
                }
            }
        }
    }
}
