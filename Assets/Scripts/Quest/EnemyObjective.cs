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
    private bool isComplete;

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
        this.isComplete = false;
    }

    /// <summary>
    /// Initializes the objective by subscribing to the "Enemy Killed" message.
    /// </summary>
    public override void InitializeObjective()
    {
        Messenger.AddListener<EnemyLog>("Enemy Killed", OnEnemyKilled);
        Debug.Log($"Objective initialized: Kill {initialCount} of {enemy.EnemyName}");
    }

    /// <summary>
    /// Called when an enemy is killed.
    /// </summary>
    /// <param name="killedEnemy">The killed enemy.</param>
    private void OnEnemyKilled(EnemyLog killedEnemy)
    {
        if (killedEnemy.EnemyName == enemy.EnemyName && objectiveActive)
        {
            numberToCollect--;
            Debug.Log($"Enemy killed: {killedEnemy.EnemyName}. Remaining: {numberToCollect}");
            if (numberToCollect <= 0)
            {
                CompleteObjective();
            }
        }
    }

    /// <summary>
    /// Completes the objective.
    /// </summary>
    private void CompleteObjective()
    {
        Debug.Log($"Objective completed: {enemy.EnemyName} has been killed.");
        isComplete = true;
        objectiveActive = false;

        Messenger.RemoveListener<EnemyLog>("Enemy Killed", OnEnemyKilled);

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
        return isComplete;
    }

    /// <summary>
    /// Gets the objective description.
    /// </summary>
    /// <returns>The objective description.</returns>
    public override string GetObjectiveDescription()
    {
        return $"Kill {initialCount} of {enemy.EnemyName} (Remaining: {numberToCollect})";
    }

    /// <summary>
    /// Transfers ownership of the objective to a new owner.
    /// </summary>
    /// <param name="newOwner">The new owner.</param>
    public override void TransferOwner(GameObject newOwner)
    {
        questOwner = newOwner;
    }

    /// <summary>
    /// Gets the objective object.
    /// </summary>
    /// <returns>The objective object.</returns>
    public override GameObject GetObjectiveObject()
    {
        return enemy.gameObject;
    }

    /// <summary>
    /// Sets this objective as the active objective in this objective path.
    /// </summary>
    public override void SetActiveObjective()
    {
        objectiveActive = true;
    }

    /// <summary>
    /// Sets the objective that comes after this objective.
    /// </summary>
    /// <param name="newNextObjective">The next objective.</param>
    public override void SetNextObjective(QuestObjective newNextObjective)
    {
        nextObjective = newNextObjective;
    }
}
