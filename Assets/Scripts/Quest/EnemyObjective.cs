using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a quest objective to kill a specific enemy.
/// </summary>
public class EnemyObjective : QuestObjective
{
    private EnemyLog enemy;
    private int numberToCollect;
    private GameObject questGiver;
    private int initialCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnemyObjective"/> class.
    /// </summary>
    /// <param name="enemy">The enemy to kill.</param>
    /// <param name="numberToCollect">The number of enemies to kill.</param>
    /// <param name="questGiver">The quest giver.</param>
    public EnemyObjective(EnemyLog enemy, int numberToCollect, GameObject questGiver)
    {
        this.enemy = enemy;
        this.numberToCollect = numberToCollect;
        this.questGiver = questGiver;
        this.initialCount = numberToCollect;
    }

    /// <summary>
    /// Initializes the objective by subscribing to the "Enemy Killed" message.
    /// </summary>
    public override void InitializeObjective()
    {
        Messenger.AddListener<EnemyLog>("Enemy Killed", OnEnemyKilled);
        Debug.Log($"Objective initialized: Kill {initialCount} of {enemy.name}");
    }

    /// <summary>
    /// Called when an enemy is killed.
    /// </summary>
    /// <param name="killedEnemy">The killed enemy.</param>
    private void OnEnemyKilled(EnemyLog killedEnemy)
    {
        if (killedEnemy.name == enemy.name)
        {
            numberToCollect--;
            Debug.Log($"Enemy killed: {killedEnemy.name}. Remaining: {numberToCollect}");
            if (numberToCollect <= 0)
            {
                Messenger.RemoveListener<EnemyLog>("Enemy Killed", OnEnemyKilled);
                CompleteObjective();
            }
        }
    }

    /// <summary>
    /// Completes the objective.
    /// </summary>
    private void CompleteObjective()
    {
        Debug.Log($"Objective completed: {enemy.name} has been killed.");
        objectiveActive = false;

        // Activate the next objective if it exists
        if (nextObjective != null)
        {
            nextObjective.SetActiveObjective();
        }
    }

    /// <summary>
    /// Cleans up the objective by unsubscribing from the "Enemy Killed" message.
    /// </summary>
    public override void CleanupObjective()
    {
        Messenger.RemoveListener<EnemyLog>("Enemy Killed", OnEnemyKilled);
    }

    /// <summary>
    /// Checks if the objective is complete.
    /// </summary>
    /// <returns>True if the objective is complete; otherwise, false.</returns>
    public override bool IsComplete()
    {
        return numberToCollect <= 0;
    }
}
