using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//===========code by charlie=======
using UnityEngine.SceneManagement;
//=================================

public class Initializer : MonoBehaviour {

    public Game game;

	// Use this for initialization
	void Start () {
        AudioListener.pause = false;

        game.Initialize();
    }


	
}
