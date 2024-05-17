using System.Collections;
using System.Collections.Generic;
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
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private int maxhealth = 5;
    public Image[] hearts;
    public Sprite fullheart;
    public Sprite emptyheart;
    public AudioSource walkSource;

    private bool lockPlayerMovement;
    private bool moving = false;

    private GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
        walkSource = GetComponent<AudioSource>();
        playerhealth = 5;
        if (playerhealth == 0)
        {
            playerhealth = 5;
        }
        currentstate = PlayerState.walk;
        walkSource.Play();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        lockPlayerMovement = false;
        Time.timeScale = 1f; 
    }

    void Update()
    {
        UpdateHealthUI();

        if (playerhealth <= 0)
        {
            PlayerDies();
            gameManager.Lose();

        }

        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("attack") && currentstate != PlayerState.attack && currentstate != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentstate == PlayerState.walk || currentstate == PlayerState.idle)
        {
            UpdateAnimationAndMove();
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
        {
            playerhealth = maxhealth;
        }
    }

    private void PlayerDies()
    {
        gameObject.SetActive(false);
        playerhealth = maxhealth;
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("Attacking", true);
        currentstate = PlayerState.attack;
        yield return null;
        animator.SetBool("Attacking", false);
        yield return new WaitForSeconds(.3f);
        currentstate = PlayerState.walk;
        //walkSource.Play();
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MovePlayer();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    void MovePlayer()
    {
        if (lockPlayerMovement)
            return;

        change.Normalize();
        myRigidbody.MovePosition(transform.position + change * (speed * Time.deltaTime));
    }

    void LockPlayerMovement()
    {
        lockPlayerMovement = true;
        moving = false;
    }

    void UnlockPlayerMovement()
    {
        lockPlayerMovement = false;
    }

    public void Knock(float knocktime, float damage)
    {
        if (playerhealth > 0)
        {
            StartCoroutine(KnockCo(knocktime));
        }
    }

    private IEnumerator KnockCo(float knocktime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knocktime);
            myRigidbody.velocity = Vector2.zero;
            currentstate = PlayerState.idle;
            walkSource.Pause();
            myRigidbody.velocity = Vector2.zero;
        }
    }

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
