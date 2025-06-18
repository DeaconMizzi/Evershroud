using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerState
{
    walk,
    attack,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentstate;
    public float speed;
    public static int playerhealth = 5;

    private int maxhealth = 5;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;

    public Image[] hearts;
    public Sprite fullheart;
    public Sprite emptyheart;
    public AudioSource walkSource;

    private bool lockPlayerMovement;

    private void Start()
    {
        walkSource = GetComponent<AudioSource>();
        playerhealth = maxhealth;

        currentstate = PlayerState.walk;
        walkSource.Play();

        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);

        lockPlayerMovement = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        UpdateHealthUI();

        // 🔁 Global health check
        if (playerhealth <= 0)
        {
            SceneManager.LoadScene("Lose");
        }

        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("attack") &&
            currentstate != PlayerState.attack &&
            currentstate != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }

        if (change != Vector3.zero)
        {
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    private void FixedUpdate()
    {
        if ((currentstate == PlayerState.walk || currentstate == PlayerState.idle) &&
            change != Vector3.zero && !lockPlayerMovement)
        {
            change.Normalize();
            myRigidbody.MovePosition(transform.position + change * (speed * Time.fixedDeltaTime));
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < playerhealth ? fullheart : emptyheart;
            hearts[i].enabled = i < maxhealth;
        }

        if (playerhealth > maxhealth)
            playerhealth = maxhealth;
    }

    private IEnumerator HandleDeath()
    {
        animator.SetTrigger("death");
        myRigidbody.velocity = Vector2.zero;
        currentstate = PlayerState.stagger;

        yield return new WaitForSeconds(1f);

        Debug.Log("Attempting to load Lose scene...");
        SceneManager.LoadScene("Lose"); // Make sure this matches your scene file name
    }

    public void Knock(float knocktime, float damage, Vector2 direction)
    {
        if (playerhealth > 0)
        {
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.AddForce(direction.normalized * 5f, ForceMode2D.Impulse);

            playerhealth -= (int)damage;

            Debug.Log("Damage applied: " + damage + " | New health: " + playerhealth);

            if (playerhealth <= 0)
            {
                Debug.Log("Health dropped to zero inside Knock()");
                StartCoroutine(HandleDeath());
            }
            else
            {
                StartCoroutine(KnockCo(knocktime));
            }
        }
    }

    private IEnumerator KnockCo(float knocktime)
    {
        yield return new WaitForSeconds(knocktime);
        myRigidbody.velocity = Vector2.zero;
        currentstate = PlayerState.idle;
        walkSource.Pause();
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("Attacking", true);
        currentstate = PlayerState.attack;
        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.3f);
        currentstate = PlayerState.walk;
    }

    private void LockPlayerMovement() => lockPlayerMovement = true;
    private void UnlockPlayerMovement() => lockPlayerMovement = false;

    private void OnEnable()
    {
        Messenger.AddListener("LockPlayerMovement", LockPlayerMovement);
        Messenger.AddListener("UnlockPlayerMovement", UnlockPlayerMovement);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener("LockPlayerMovement", LockPlayerMovement);
        Messenger.RemoveListener("UnlockPlayerMovement", UnlockPlayerMovement);
    }
}
