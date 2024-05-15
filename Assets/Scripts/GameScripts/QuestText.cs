using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuestSystem;

public class SampleMisionText : MonoBehaviour
{
    public Text text;
    private QuestManager questManagerRef;

    private void Start()
    {
        questManagerRef = QuestManager.GetInstance();
    }




    // Update is called once per frame
    void Update()
    {
        string misonsLog = questManagerRef.GetCurrentQuestsInformation();
        text.text = misonsLog;
    }
}