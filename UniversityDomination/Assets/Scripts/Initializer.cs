//===========================================================================
//Executable Link:
//https://as2378.github.io/unlucky/files/Assessment4/UniversityDomination.zip
//===========================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This class is used to initialise the game when the scene is loaded.
 */
public class Initializer : MonoBehaviour {

    public Game game;

	// Use this for initialization
	void Start () {
        AudioListener.pause = false;

        game.Initialize();
    }
}
