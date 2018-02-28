using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

/*
 * ASSESSMENT4 ADDITION:
 * The previous team did not expand the unit tests to include the PVC minigame, therefore new tests
 * have been added to ensure the minigame functions correctly and further changes do not break it. 
 */ 
public class ViceScriptTest {
	private viceScript viceMinigame;

	//ASSESSMENT4 ADDITION
	private IEnumerator LoadMiniGame(LoadSceneMode loadType){
		//Loads the minigame scene, waits for it to be loaded and then finds the viceScript within the scene.
		SceneManager.LoadScene("miniGameScene",loadType);
		yield return new WaitUntil (() => SceneManager.GetSceneByName ("miniGameScene").isLoaded);

		viceMinigame = GameObject.Find ("Canvas").GetComponentInChildren<viceScript> ();
	}
	//ASSESSMENT4 ADDITION
	private IEnumerator LoadMainGame(){
		//Loads the main game scene with 4 human players, waits for it to load.
		staticPassArguments.humanPlayers = 4;
		staticPassArguments.loadGame = false;

		SceneManager.LoadScene("TestScene",LoadSceneMode.Single);
		yield return new WaitUntil (() => SceneManager.GetSceneByName ("TestScene").isLoaded);
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator MiniGameScene_LoadedWhenCurrentPlayerOwnsAndClicksThePVCSector(){
		yield return LoadMainGame (); //Load the main game (TestScene)

		//Find the PVC sector and capture it as the current player.
		Game game = SceneManager.GetActiveScene ().GetRootGameObjects () [0].GetComponent<Game> ();
		Landmark pvcLandmark = game.viceChancellorGameObj.GetComponent<Landmark> ();
		Sector pvcSector = pvcLandmark.gameObject.transform.parent.gameObject.GetComponent<Sector>();
		game.currentPlayer.units [0].MoveTo (pvcSector);

		//Once captured, when Landmark.OnMouseDown is called, the minigame scene should load.
		//Otherwise test will timeout after 30 seconds.
		pvcLandmark.OnMouseDown();
		yield return new WaitUntil (() => SceneManager.GetSceneByName ("miniGameScene").isLoaded);
		Assert.That (SceneManager.GetSceneByName ("miniGameScene") == SceneManager.GetActiveScene ());
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator Start_MinigameLoadsInDefaultState (){
		yield return LoadMiniGame (LoadSceneMode.Single); //Load the minigame scene

		//Tests that the number of guesses is set to zero.
		Assert.Zero (viceMinigame.GetNumberOfGuesses ());

		//Tests that the torso and leg choices are set to zero.
		Assert.Zero (viceMinigame.GetTorsoChoice ());
		Assert.Zero (viceMinigame.GetLegChoice ());

		//Tests that the correct torso and leg values are valid.
		int correctTorso = viceMinigame.GetCorrectTorso();
		int correctLegs = viceMinigame.GetCorrectLegs ();
		Assert.GreaterOrEqual (correctTorso, 0);
		Assert.Less (correctTorso, viceMinigame.torsos.Count);

		Assert.GreaterOrEqual (correctLegs, 0);
		Assert.Less (correctLegs, viceMinigame.legs.Count);
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator TorsoRightPressed_IncrementsTorsoChoiceOrLoopsToStart(){
		yield return LoadMiniGame (LoadSceneMode.Single); //Load the minigame scene

		//Test that torsoRightPressed increases the value of torsoChoice by 1.
		int expectedTorsoChoice = viceMinigame.GetTorsoChoice() + 1;
		viceMinigame.torsoRightPressed();
		Assert.AreEqual (expectedTorsoChoice, viceMinigame.GetTorsoChoice ());

		//Test that torsoRightPressed resets the value of torsoChoice to zero when it reaches the end of the torso list.
		int torsoCount = viceMinigame.torsos.Count;
		for (int i = 1; i < torsoCount; i++) {
			viceMinigame.torsoRightPressed ();
		}
		Assert.AreEqual (0, viceMinigame.GetTorsoChoice ());
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator TorsoLeftPressed_DecrementsTorsoChoiceOrLoopsToEnd(){
		yield return LoadMiniGame (LoadSceneMode.Single); //Load the minigame scene

		//Test that torsoLeftPressed sets the value of torsoChoice to point to the end of the torso list.
		int expectedTorsoChoice = viceMinigame.torsos.Count - 1;
		viceMinigame.torsoLeftPressed();
		Assert.AreEqual (expectedTorsoChoice, viceMinigame.GetTorsoChoice ());

		//Test that torsoLeftPressed decrements the value of torsoChoice by 1 when torsoChoice > 0.
		expectedTorsoChoice = viceMinigame.GetTorsoChoice() - 1;
		viceMinigame.torsoLeftPressed();
		Assert.AreEqual (expectedTorsoChoice, viceMinigame.GetTorsoChoice ());
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator LegsRightPressed_IncrementsLegChoiceOrLoopsToStart(){
		yield return LoadMiniGame (LoadSceneMode.Single); //Load the minigame scene

		//Test that legsRightPressed increases the value of legChoice by 1.
		int expectedLegChoice = viceMinigame.GetLegChoice() + 1;
		viceMinigame.legsRightPressed();
		Assert.AreEqual (expectedLegChoice, viceMinigame.GetLegChoice ());

		//Test that legsRightPressed resets the value of legChoice to zero when it reaches the end of the legs list.
		int legCount = viceMinigame.legs.Count;
		for (int i = 1; i < legCount; i++) {
			viceMinigame.legsRightPressed ();
		}
		Assert.AreEqual (0, viceMinigame.GetLegChoice ());
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator LegsLeftPressed_DecrementsLegChoiceOrLoopsToEnd(){
		yield return LoadMiniGame (LoadSceneMode.Single); //Load the minigame scene

		//Test that legsLeftPressed sets the value of legChoice to point to the end of the leg list.
		int expectedLegChoice = viceMinigame.legs.Count - 1;
		viceMinigame.legsLeftPressed();
		Assert.AreEqual (expectedLegChoice, viceMinigame.GetLegChoice ());

		//Test that legsLeftPressed decrements the value of legChoice by 1 when legChoice > 0.
		expectedLegChoice = viceMinigame.GetLegChoice() - 1;
		viceMinigame.legsLeftPressed();
		Assert.AreEqual (expectedLegChoice, viceMinigame.GetLegChoice ());
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator SubmitGuess_OneGuessFullyIncorrect(){
		yield return LoadMiniGame (LoadSceneMode.Single); //Load the minigame scene

		int correctTorso = viceMinigame.GetCorrectTorso ();
		int correctLegs = viceMinigame.GetCorrectLegs ();

		//ensure that the guesses for torso & legs are both incorrect.
		if (viceMinigame.GetTorsoChoice () == correctTorso) {
			viceMinigame.torsoRightPressed ();
		}
		if (viceMinigame.GetLegChoice () == correctLegs) {
			viceMinigame.legsRightPressed ();
		}
		viceMinigame.submitGuess ();

		//Test that the number of guesses has increased from 0 to 1.
		Assert.AreEqual(1,viceMinigame.GetNumberOfGuesses());

		//Test that the speech bubble has updated its text.
		string expectedText = "You correctly guessed 0 item(s) of clothing out of 2. You have 2 guesses left.";
		Assert.AreEqual(expectedText,viceMinigame.speechBubble.GetComponentInChildren<UnityEngine.UI.Text>().text);
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator SubmitGuess_TwoGuessesFullyIncorrect(){
		yield return LoadMiniGame (LoadSceneMode.Single); //Load the minigame scene

		int correctTorso = viceMinigame.GetCorrectTorso ();
		int correctLegs = viceMinigame.GetCorrectLegs ();

		//ensure that the guesses for torso & legs are both incorrect.
		if (viceMinigame.GetTorsoChoice () == correctTorso) {
			viceMinigame.torsoRightPressed ();
		}
		if (viceMinigame.GetLegChoice () == correctLegs) {
			viceMinigame.legsRightPressed ();
		}
		viceMinigame.submitGuess ();
		viceMinigame.submitGuess ();

		//Test that the number of guesses has increased from 0 to 2.
		Assert.AreEqual(2,viceMinigame.GetNumberOfGuesses());

		//Test that the speech bubble has updated its text.
		string expectedText = "You correctly guessed 0 item(s) of clothing out of 2. You have 1 guesses left.";
		Assert.AreEqual(expectedText,viceMinigame.speechBubble.GetComponentInChildren<UnityEngine.UI.Text>().text);
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator SubmitGuess_OneGuessHalfIncorrect(){
		yield return LoadMiniGame (LoadSceneMode.Single); //Load the minigame scene

		int correctTorso = viceMinigame.GetCorrectTorso ();
		int correctLegs = viceMinigame.GetCorrectLegs ();

		//ensure that the torsoChoice is incorrect
		if (viceMinigame.GetTorsoChoice () == correctTorso) {
			viceMinigame.torsoRightPressed ();
		}
		//ensure that the legsChoice is correct
		while (viceMinigame.GetLegChoice () != correctLegs) {
			viceMinigame.legsRightPressed ();
		}

		viceMinigame.submitGuess ();

		//Test that the number of guesses has increased from 0 to 1.
		Assert.AreEqual(1,viceMinigame.GetNumberOfGuesses());

		//Test that the speech bubble has updated its text.
		string expectedText = "You correctly guessed 1 item(s) of clothing out of 2. You have 2 guesses left.";
		Assert.AreEqual(expectedText,viceMinigame.speechBubble.GetComponentInChildren<UnityEngine.UI.Text>().text);
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator SubmitGuess_AllGuessesIncorrectReturnToGame(){
		yield return LoadMainGame (); //Load the main game
		yield return LoadMiniGame(LoadSceneMode.Additive); //Load the minigame
		//Deactivate the TestScene gameobjects
		foreach (GameObject obj in SceneManager.GetSceneByName("TestScene").GetRootGameObjects()) {
			obj.SetActive (false);
		}

		int correctTorso = viceMinigame.GetCorrectTorso ();
		int correctLegs = viceMinigame.GetCorrectLegs ();

		//ensure that the guesses for torso & legs are both incorrect.
		if (viceMinigame.GetTorsoChoice () == correctTorso) {
			viceMinigame.torsoRightPressed ();
		}
		if (viceMinigame.GetLegChoice () == correctLegs) {
			viceMinigame.legsRightPressed ();
		}
		//Make 3 guesses.
		for (int i = 0; i < 3; i++) {
			viceMinigame.submitGuess ();
		}
		//Test that the speech bubble has updated its text.
		string expectedText = "I've given you enough chances! Get out!";
		Assert.AreEqual(expectedText,viceMinigame.speechBubble.GetComponentInChildren<UnityEngine.UI.Text>().text);

		//Test that the active scene returns to TestScene (the main game).
		yield return new WaitUntil (() => SceneManager.sceneCount == 1);
		Assert.That (SceneManager.GetSceneByName ("TestScene") == SceneManager.GetActiveScene ());
	}


	[UnityTest] //ASSESSMENT4 ADDITION
	public IEnumerator SubmitGuess_CorrectGuessReturnToGameAndIncreaseBeerKnowledgeValues(){
		yield return LoadMainGame (); //Load the main game
		//Record the expected beer & knowledge values for the current player
		Game game = SceneManager.GetSceneByName("TestScene").GetRootGameObjects()[0].GetComponent<Game>();
		int expectedBeerValue = game.currentPlayer.GetBeer () + 4;
		int expectedKnowledgeValue = game.currentPlayer.GetKnowledge () + 4;

		yield return LoadMiniGame(LoadSceneMode.Additive); //Load the minigame
		//Deactivate the TestScene gameobjects
		foreach (GameObject obj in SceneManager.GetSceneByName("TestScene").GetRootGameObjects()) {
			obj.SetActive (false);
		}

		int correctTorso = viceMinigame.GetCorrectTorso ();
		int correctLegs = viceMinigame.GetCorrectLegs ();

		//ensure that the guesses for torso & legs are both correct.
		while (viceMinigame.GetTorsoChoice () != correctTorso) {
			viceMinigame.torsoRightPressed ();
		}
		while (viceMinigame.GetLegChoice () != correctLegs) {
			viceMinigame.legsRightPressed ();
		}
		//Call submitGuess to make the guess
		viceMinigame.submitGuess ();

		//Test that the speech bubble has updated its text.
		string expectedText = "I love it! You win! \nHere's some beer & knowledge!";
		Assert.AreEqual(expectedText,viceMinigame.speechBubble.GetComponentInChildren<UnityEngine.UI.Text>().text);

		//Test that the active scene returns to TestScene (the main game).
		yield return new WaitUntil (() => SceneManager.sceneCount == 1);
		Assert.That (SceneManager.GetSceneByName ("TestScene") == SceneManager.GetActiveScene ());

		//Test that the currentPlayer has recieved 4 beer and 4 knowledge.
		Assert.AreEqual(expectedBeerValue,game.currentPlayer.GetBeer());
		Assert.AreEqual (expectedKnowledgeValue, game.currentPlayer.GetKnowledge ());
	}
}