using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public static QuestHandler Instance { get; private set; }

    private Dictionary<string, Quest> quests;
    private Dictionary<string, Quest> completedQuests;
    private Dictionary<string, QuestGiver> questGivers;
    private List<GameObject> nearbyQuestGivers;
    private Quest questToWaitFor;
    private bool waitingForReward;

    private void OnEnable()
    {
        Messenger.AddListener("AcceptQuest", AddQuest);
        Messenger.AddListener("RejectQuest", RejectQuest);
        Messenger.AddListener("QuestComplete", CompleteQuest);
    }

    private void OnDisable()
    {
        Messenger.RemoveListener("AcceptQuest", AddQuest);
        Messenger.RemoveListener("RejectQuest", RejectQuest);
        Messenger.RemoveListener("QuestComplete", CompleteQuest);
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ensure the handler persists across scenes if needed
        }
        else
        {
            Destroy(gameObject);  // Ensure only one instance exists
            return;
        }

        quests = new Dictionary<string, Quest>();
        completedQuests = new Dictionary<string, Quest>();
        questGivers = new Dictionary<string, QuestGiver>();
        nearbyQuestGivers = new List<GameObject>();
        waitingForReward = false;

        foreach (GameObject questGiver in GameObject.FindGameObjectsWithTag("Character"))
        {
            QuestGiver tempQuestGiver = questGiver.GetComponent<QuestGiver>();

            if (tempQuestGiver != null)
                questGivers.Add(questGiver.name, tempQuestGiver);
        }
    }

    void Update() { }

    public void StartQuestDialogue()
    {
        if (nearbyQuestGivers.Count < 1)
            return;

        Quest tempQuest = questGivers[nearbyQuestGivers[0].name].GetQuest();

        if (tempQuest == null)
            return;

        Debug.Log("Starting Quest Conversation");
        ConversationHandler.conversationHandler.StartConversation(tempQuest.GetQuestDialogue());
        questToWaitFor = tempQuest;
    }

    public void AddQuest()
    {
        Debug.Log("Adding Quest: " + questToWaitFor.GetQuestName());
        questToWaitFor.TransferOwnership(gameObject);
        questGivers[questToWaitFor.GetQuestGiver().name].RemoveQuest(questToWaitFor);
        quests.Add(questToWaitFor.GetQuestGiver().name, questToWaitFor);
        InstantiateObjectives(questToWaitFor);
    }

    private void InstantiateObjectives(Quest newQuest)
    {
        foreach (List<QuestObjective> objectivePath in newQuest.GetObjectives())
        {
            foreach (QuestObjective subObjective in objectivePath)
            {
                if (subObjective.GetType() == typeof(LocationObjective))
                    Instantiate(subObjective.GetObjectiveObject(), subObjective.GetObjectiveObject().transform.position, Quaternion.identity);
            }
        }
    }

    public void RejectQuest()
    {
        questToWaitFor = null;
    }

    public void TurnInQuest()
    {
        if (nearbyQuestGivers.Count == 0)
            return;

        if (!quests.ContainsKey(nearbyQuestGivers[0].gameObject.name))
            return;

        Quest quest = quests[nearbyQuestGivers[0].gameObject.name];

        if (!quest.IsQuestComplete())
        {
            Debug.Log("Quest is not complete yet.");
            ConversationHandler.conversationHandler.StartConversation(quest.GetInProgressDialogue());
            return;
        }

        waitingForReward = true;

        if (quest.GetTurnInDialogue() != null)
        {
            ConversationHandler.conversationHandler.StartConversation(quest.GetTurnInDialogue());
            questToWaitFor = quest;
            return;
        }

        CompleteQuest();
    }

    private void CompleteQuest()
    {
        if (!waitingForReward)
            return;

        Quest quest = quests[nearbyQuestGivers[0].gameObject.name];

        Debug.Log("Quest Complete: " + quest.GetQuestName());
        completedQuests.Add(quest.GetQuestName(), quest);
        RemoveQuest(quest.GetQuestGiver().GetComponent<QuestGiver>());
        waitingForReward = false;
    }

    public Dictionary<string, Quest> GetCompletedQuests()
    {
        return completedQuests;
    }

    private void RemoveQuest(QuestGiver questGiver)
    {
        quests.Remove(questGiver.gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!questGivers.ContainsKey(collision.gameObject.name))
            return;

        nearbyQuestGivers.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearbyQuestGivers.Contains(collision.gameObject))
            nearbyQuestGivers.Remove(collision.gameObject);
    }

    // New method to notify when an enemy is killed
    public void NotifyEnemyKilled(string enemyName)
    {
        foreach (Quest quest in quests.Values)
        {
            foreach (List<QuestObjective> objectivePath in quest.GetObjectives())
            {
                foreach (QuestObjective objective in objectivePath)
                {
                    if (objective is EnemyObjective enemyObjective && enemyObjective.enemy.enemyName == enemyName)
                    {
                        enemyObjective.EnemyKilled();
                    }
                }
            }
        }
    }
}
