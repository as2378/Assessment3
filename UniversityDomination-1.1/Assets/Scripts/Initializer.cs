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
        game.Initialize();

        //=======code by charlie================
        //SceneManager.LoadScene("miniGameScene");
        //SceneManager.SetActiveScene()
        //======================================
	}


	
}
