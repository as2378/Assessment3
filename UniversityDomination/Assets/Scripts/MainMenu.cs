//======================================================
//Website link with executable:
//http://www-users.york.ac.uk/~ch1575/documentation
//======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

    public Dropdown numberOfHumanPlayers;

    void Start()
    {
        staticPassArguments.loadGame = false;
    }

    public void PlayGame() {

        int humanPlayers = 0;

        switch(numberOfHumanPlayers.value)
        {
            case 0:
                humanPlayers = 2;
                break;

            case 1:
                humanPlayers = 3;
                break;

            case 2:
                humanPlayers = 4;
                break;
        }

        staticPassArguments.humanPlayers = humanPlayers; //static variable used to pass arguments between scenes

        AudioListener.pause = true;

        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next scene in build order
    }

    public void QuitGame() {

        Debug.Log("Quit"); // For Testing
        Application.Quit();

    }

	public void load() //start new game, then call the load function from game manager
    {
        staticPassArguments.loadGame = true;
        PlayGame();
    }
}

