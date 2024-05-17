using UnityEngine;

public class GameManager : MonoBehaviour
{
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

        if (string.IsNullOrEmpty(enemy.EnemyName))
        {
            Debug.LogError("Enemy name is null or empty.");
        }
        else
        {
            // Handle enemy killed event, update score, etc.
            Debug.Log($"Enemy killed: {enemy.EnemyName}");
            // Update your game state, score, etc. here
        }
    }
}
