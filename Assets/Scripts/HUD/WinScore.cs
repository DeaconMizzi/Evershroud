using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScore : MonoBehaviour
{
    int scoreValue;
    Text score;
    // Start is called before the first frame update
    void Start()
    {
        scoreValue = PlayerPrefs.GetInt("score", ScoreScript.scoreValue);
        score = GetComponent<Text>();
        score.text = "Coins Collected: " + scoreValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
