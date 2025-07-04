using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject Game; 
    public GameObject loseGame;
    public GameObject winGame;

    private void Start()
    {
        Game.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void OnPlayButtonClicked()
    {
        mainMenu.SetActive(false);
        Game.SetActive(true);
    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }
    
    public void Lose()
    {
        SceneManager.LoadScene("Lose");
    }
    private void OnEnable()
    {
        Messenger.AddListener<EnemyLog>("Enemy Killed", OnEnemyKilled);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener<EnemyLog>("Enemy Killed", OnEnemyKilled);
    }

    private void OnEnemyKilled(EnemyLog enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("Enemy is null in OnEnemyKilled.");
            return;
        }

        if (string.IsNullOrEmpty(enemy.enemyName))
        {
            Debug.LogError("Enemy name is null or empty.");
        }
        else
        {
            // Handle enemy killed event, update score, etc.
            Debug.Log($"Enemy killed: {enemy.enemyName}");
            // Update your game state, score, etc. here
        }
    }
}
