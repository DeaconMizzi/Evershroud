using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a quest objective to kill a specific enemy.
/// </summary>
public class EnemyObjective : QuestObjective
{
    private Enemy enemy;
    private int numberToCollect;
    private int numberToKill;
    private int numberRemaining;
    private GameObject questGiver;
    private int initialCount;
    private bool isComplete;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnemyObjective"/> class.
    /// </summary>
    /// <param name="enemy">The enemy to kill.</param>
    /// <param name="numberToCollect">The number of enemies to kill.</param>
    /// <param name="questGiver">The quest giver.</param>
    public EnemyObjective(Enemy newEnemy, int newNumberToKill, GameObject newOwner)
    {
        enemy = newEnemy;
        numberToKill = newNumberToKill;
        numberRemaining = newNumberToKill;
        questOwner = newOwner;
        objectiveObject = newEnemy.gameObject;
        Messenger.AddListener<Enemy>("Enemy Killed", EnemyKilled); 

        Debug.Log($"Initializing EnemyObjective: Kill {numberToCollect} of {enemy.name}");
    }

    private void EnemyKilled(Enemy enemyThatWasKilled)
    {
        if (questOwner == null)
            return;

        if (questOwner.tag != "Player")
            return;

        if (IsComplete() || !objectiveActive)
            return;

        if (enemy.GetName() == enemyThatWasKilled.GetName())
        {
            // Check and make sure the message we received was for this specific enemy.

            numberRemaining--; // decrement remaining count.

            if (IsComplete() && nextObjective != null)
                nextObjective.SetActiveObjective(); // set the next objective to active.

            Debug.Log("Enemy " + enemyThatWasKilled.GetName() + " Killed " + numberRemaining + " Remain");
        }
    }

    /// <summary>
    /// Called when an enemy is killed.
    /// </summary>
    /// <param name="killedEnemy">The killed enemy.</param>
    /*private void OnEnemyKilled(EnemyLog killedEnemy)
    {
        if (questOwner == null || questOwner.tag != "Player" || IsComplete() || !objectiveActive)
        {
            return;
        }

        if (killedEnemy.EnemyName == enemy.name && objectiveActive)
        {
            numberRemaining--;
            Debug.Log($"Enemy killed: {enemy.name}. Number remaining: {numberRemaining}");

            if (IsComplete())
            {
                Debug.Log("Objective complete!");
                objectiveActive = false;
                Messenger.RemoveListener<EnemyLog>("Enemy Killed", OnEnemyKilled);

                if (nextObjective != null)
                {
                    nextObjective.SetActiveObjective();
                }
            }
        }
    }*/

    public override bool IsComplete()
    {
        Debug.Log($"Checking if objective is complete: {numberRemaining}");
        return numberRemaining <= 0 ? true : false;
    }
}
