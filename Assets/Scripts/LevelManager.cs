using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel(string name)
    {

        //outputs in the Console
        print("Loading level " + name);

        //loads the scene name
        SceneManager.LoadScene(name);
    }

    public void QuitGameEXE()
    {
        Application.Quit();
    }

    
}