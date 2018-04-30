//===========================================================================
//Executable Link:
//https://as2378.github.io/unlucky/files/Assessment4/UniversityDomination.zip
//===========================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * This class is used to control the PVC minigame.
 */ 
public class viceScript : MonoBehaviour {
    public GameObject speechBubble; //the speech bubble showing how many clothing items were guessed correctly, set in editor
    public GameObject mainCamera;
    public GameObject submitButton;

	private int guesses; //integer representing the number of guesses the user has submitted so far

	private int correctTorso; //integer representing the correct torso clothing
	private int correctLegs; //integer representing the correct legs clothing

    public List<Sprite> torsos; //list of possible torso clothes, set in editor
    public List<Sprite> legs; //list of possible leg clothes, set in editor

    public GameObject torso; //panel gameobject representing torso, set in editor
    public GameObject leg; //panel gameobject representing legs, set in editor

	private int torsoChoice; //integer representing which torso sprite the user has selected
    private int legChoice; //integer representing which leg sprite the user has selected

	//ASSESSMENT4 ADDITIONS ---------
	//Added accessors to allow for easier unit testing.
	public int GetNumberOfGuesses(){
		return this.guesses;
	}
	public int GetCorrectTorso(){
		return this.correctTorso;
	}
	public int GetCorrectLegs(){
		return this.correctLegs;
	}
	public int GetTorsoChoice(){
		return this.torsoChoice;
	}
	public int GetLegChoice(){
		return this.legChoice;
	}
	//------------------------------

    void Start()
    {
        guesses = 0;

        System.Random rnd = new System.Random();

        correctTorso = rnd.Next(0, torsos.Count);
        correctLegs = rnd.Next(0, legs.Count);

        torsoChoice = 0;
        legChoice = 0;

        torso.GetComponent<Image>().sprite = torsos[torsoChoice];
        leg.GetComponent<Image>().sprite = legs[legChoice];

        speechBubble.SetActive(false);
    }

	//ASSESSMENT4 ADDITION: Switched right and left increment/decrement to match leg buttons.
	public void torsoRightPressed()
    {
        torsoChoice = (torsoChoice + 1) % torsos.Count;
        torso.GetComponent<Image>().sprite = torsos[torsoChoice];
    }

	//ASSESSMENT4 ADDITION:
	//Changes because test failed. Makes more sense that the left/right buttons traverse the list of torsos
	//in opposite directions, not the same.
	public void torsoLeftPressed()
    {
		torsoChoice = (torsoChoice - 1);
		if (torsoChoice < 0) {
			torsoChoice = torsos.Count - 1;
		}
		torso.GetComponent<Image>().sprite = torsos[torsoChoice];
    }

    public void legsRightPressed()
    {
        legChoice = (legChoice + 1) % legs.Count;
        leg.GetComponent<Image>().sprite = legs[legChoice];
    }

	//ASSESSMENT4 ADDITION:
	//Changes because test failed. Makes more sense that the left/right buttons traverse the list of legs
	//in opposite directions, not the same.
    public void legsLeftPressed()
    {
		legChoice = (legChoice - 1);
		if (legChoice < 0) {
			legChoice = legs.Count - 1;
		}
		leg.GetComponent<Image>().sprite = legs[legChoice];
    }

    public void submitGuess()
    {
        speechBubble.SetActive(true);

        guesses++;

        int correctGuesses = 0;

        if (System.Math.Abs(legChoice) == correctLegs)
        {
            correctGuesses++;
        }

        if (System.Math.Abs(torsoChoice) == correctTorso)
        {
            correctGuesses++;
        }

        if (correctGuesses == 2)
        {
            speechBubble.GetComponentInChildren<Text>().text = "I love it! You win! \nHere's some beer & knowledge!";
            GameObject.Find("arrowContainer").SetActive(false);

            //reward player with points (4 beer, 5 knowledge) 
            Game gameManager = SceneManager.GetSceneByName("TestScene").GetRootGameObjects()[0].GetComponent<Game>();
            int newBeer = gameManager.currentPlayer.GetComponent<Player>().GetBeer() + 4;
            int newKnowledge = gameManager.currentPlayer.GetComponent<Player>().GetKnowledge() + 4;
            gameManager.currentPlayer.GetComponent<Player>().SetBeer(newBeer);
            gameManager.currentPlayer.GetComponent<Player>().SetKnowledge(newKnowledge);
            gameManager.GetComponent<Game>().NextTurnState();

            StartCoroutine(waitSecs());
        }
        else if (guesses == 3)
        {
            speechBubble.GetComponentInChildren<Text>().text = "I've given you enough chances! Get out!";
            GameObject.Find("arrowContainer").SetActive(false);

            //reward player with points (4 beer, 5 knowledge) 
            Game gameManager = SceneManager.GetSceneByName("TestScene").GetRootGameObjects()[0].GetComponent<Game>();
            gameManager.GetComponent<Game>().NextTurnState();

            StartCoroutine(waitSecs());
        }
        else
        {
            speechBubble.GetComponentInChildren<Text>().text = "You correctly guessed " + correctGuesses.ToString() +
                " item(s) of clothing out of 2. You have " + (3 - guesses).ToString() + " guesses left.";
        }
    }

    IEnumerator waitSecs()
    {
        submitButton.SetActive(false);

        yield return new WaitForSecondsRealtime(3);

        mainCamera.GetComponent<AudioListener>().enabled = false;

        foreach (GameObject rootObject in SceneManager.GetSceneByName("TestScene").GetRootGameObjects())
        {
            rootObject.SetActive(true);
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("miniGameScene"));
    }
}
