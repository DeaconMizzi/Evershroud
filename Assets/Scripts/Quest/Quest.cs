using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    [SerializeField]
    private string questName;

    [SerializeField]
    private GameObject questGiver;

    private List<List<QuestObjective>> objectives;
    private List<string> requiredQuests;

    private DialogueFSM questDialogue;
    private DialogueFSM turnInDialogue;
    private DialogueFSM inProgressDialogue;

    public Quest(QuestNodeCanvas questCanvas)
    {
        questName = questCanvas.newQuest.questName;
        questGiver = questCanvas.newQuest.questGiver;

        objectives = new List<List<QuestObjective>>();
        questDialogue = null;
        turnInDialogue = null;

        foreach (ObjectivePath path in questCanvas.objectivePaths)
        {
            objectives.Add(new List<QuestObjective>());
            foreach (SubObjective subObjective in path.objectives)
            {
                if (subObjective.objective.tag == "Enemy")
                    objectives[objectives.Count - 1].Add(new EnemyObjective(subObjective.objective.GetComponent<EnemyLog>(), subObjective.numberToCollect, questCanvas.newQuest.questGiver));
                else if (subObjective.objective.tag == "Location")
                    objectives[objectives.Count - 1].Add(new LocationObjective(subObjective.objective));
                // Additional types of objectives can be added here
            }
        }

        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].Count > 0)
            {
                objectives[i][0].SetActiveObjective();
            }
        }

        foreach (List<QuestObjective> objectivePath in objectives)
        {
            for (int i = 0; i < objectivePath.Count - 1; i++)
            {
                objectivePath[i].SetNextObjective(objectivePath[i + 1]);
            }
        }

        requiredQuests = new List<string>();
        foreach (QuestAsset quest in questCanvas.requiredQuests)
        {
            requiredQuests.Add(quest.questName);
        }
    }

    public bool IsQuestComplete()
    {
        foreach (List<QuestObjective> objectivePath in objectives)
        {
            foreach (QuestObjective objective in objectivePath)
            {
                if (!objective.IsComplete())
                    return false;
            }
        }
        return true;
    }

    public bool IsComplete()
    {
        foreach (var objectivePath in objectives)
        {
            foreach (var objective in objectivePath)
            {
                if (!objective.IsComplete())
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void TransferOwnership(GameObject newOwner)
    {
        foreach (List<QuestObjective> objectivePath in objectives)
        {
            foreach (QuestObjective subObjective in objectivePath)
            {
                subObjective.TransferOwner(newOwner);
            }
        }
    }

    public bool CheckPrerequisites()
    {
        QuestHandler playerQuestHandler = GameObject.FindWithTag("Player").GetComponent<QuestHandler>();

        foreach (string requiredQuest in requiredQuests)
        {
            if (!playerQuestHandler.GetCompletedQuests().ContainsKey(requiredQuest))
                return false;
        }
        return true;
    }

    public void AddDialogue(DialogueFSM newDialogue)
    {
        questDialogue = newDialogue;
    }

    public void AddTurnInDialogue(DialogueFSM newDialogue)
    {
        turnInDialogue = newDialogue;
    }

    public void AddInProgressDialogue(DialogueFSM newDialogue)
    {
        inProgressDialogue = newDialogue;
    }

    public DialogueFSM GetQuestDialogue()
    {
        return questDialogue;
    }

    public DialogueFSM GetTurnInDialogue()
    {
        return turnInDialogue;
    }

    public DialogueFSM GetInProgressDialogue()
    {
        return inProgressDialogue;
    }

    public string GetQuestName()
    {
        return questName;
    }

    public GameObject GetQuestGiver()
    {
        return questGiver;
    }

    public List<List<QuestObjective>> GetObjectives()
    {
        return objectives;
    }

    public List<string> GetRequiredQuests()
    {
        return requiredQuests;
    }
}
