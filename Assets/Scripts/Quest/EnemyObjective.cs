using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a quest objective to kill a specific enemy.
/// </summary>
public class EnemyObjective : QuestObjective
{
    public EnemyLog enemy;
    public int numberToKill;
    private int killCount;
    private bool isComplete;

    public EnemyObjective(EnemyLog enemy, int numberToKill, GameObject questGiver)
    {
        this.enemy = enemy;
        this.numberToKill = numberToKill;
        this.killCount = 0;
        this.isComplete = false;
    }

    public void EnemyKilled()
    {
        killCount++;
        if (killCount >= numberToKill)
        {
            CompleteObjective();
        }
    }

    private void CompleteObjective()
    {
        isComplete = true;
        objectiveActive = false;

        // Activate the next objective if it exists
        if (nextObjective != null)
        {
            nextObjective.SetActiveObjective();
        }
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override void InitializeObjective()
    {
        killCount = 0;
        isComplete = false;
        objectiveActive = true;
    }

    public override void CleanupObjective()
    {
        // Cleanup logic if needed
    }

    public override string GetObjectiveDescription()
    {
        return $"Kill {numberToKill} {enemy.enemyName} (Killed: {killCount}/{numberToKill})";
    }

    public override GameObject GetObjectiveObject()
    {
        return enemy.gameObject;
    }
}