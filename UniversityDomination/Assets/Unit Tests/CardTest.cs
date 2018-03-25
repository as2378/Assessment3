using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
/*
 * ASSESSMENT4 ADDITION:
 * This is used to test that the various types of card work as planned.
 */
public class CardTest {
	private Game game;
	private Map map;
	private Player[] players;
	private PlayerUI[] gui;
	private CardDeck cardDeck;

	[Test]
	public void Card_CorrectlyInstantiatesCard(){
		//Tests that when the cards are instantiated, it's owner, image and turncount are correctly stored.

		Player testPlayer = new GameObject ("TestPlayer").AddComponent<Player> ();
		Card testCard = new NothingCard (testPlayer);

		Sprite expectedImage = Resources.Load<Sprite> ("cards/Nothing");

		Assert.AreEqual (expectedImage, testCard.cardImage);
		Assert.AreEqual (1, testCard.GetTurnCount ());
		Assert.AreSame (testPlayer, testCard.GetOwner ());
	}

	[Test]
	public void Card_SetTurnCount_CorrectlyUpdatesValueOfTurnCount(){
		//Tests that SetTurnCount() changes the value of turnCount if the value given is positive.

		Player testPlayer = new GameObject ("TestPlayer").AddComponent<Player> ();
		Card testCard = new LecturerStrikeCard (testPlayer);

		testCard.SetTurnCount (10); //Set turnCount to 10.
		Assert.AreEqual (10, testCard.GetTurnCount ());

		testCard.SetTurnCount (-1); //Try to make the turnCount negative.
		Assert.AreEqual (10, testCard.GetTurnCount ());
	}

	[UnityTest]
	public IEnumerator Card_DeactivatePunishment_RemovesCardsOwner(){
		//For all the types of card, test that deactivatePunishment sets the owner value to null.
		Setup();
		game.InitializeMap ();
		yield return null;

		Player testPlayer = players [0];
		//Create one of each type of punishment card.
		Card[] testCards = new Card[] {
			new NothingCard (testPlayer),
			new FreshersFluCard (testPlayer),
			new LecturerStrikeCard (testPlayer),
			new KillerHangoverCard(testPlayer)
		};

		foreach (Card testCard in testCards) {
			testCard.activatePunishment ();

			Assert.AreSame (testPlayer, testCard.GetOwner ()); //Test that testPlayer is the owner.
			testCard.deactivatePunishment ();
			Assert.IsNull (testCard.GetOwner ()); //Test that testPlayer has been removed as an owner.
		}
	}

	[UnityTest]
	public IEnumerator FreshersFluCard_ActivatePunishment_RemovesBonuses(){
		//Tests that FreshersFluCard removes the bonuses from the players who do not own the card played.
		Setup();
		yield return null;

		Player testPlayer = players [0];
		Card testCard = new FreshersFluCard (testPlayer);

		//Give the players bonuses;
		players [0].SetBeer (2);
		players [0].SetKnowledge (1);
		players [1].SetBeer (3);
		players [1].SetKnowledge (1);
		players [2].SetBeer (3);
		players [2].SetKnowledge (2);
		players [3].SetBeer (4);

		testCard.activatePunishment (); //activate the punishment

		Assert.AreEqual (2, testPlayer.GetBeer ());
		Assert.AreEqual (1, testPlayer.GetKnowledge ());

		for (int i = 1; i < 4; i++) {
			Assert.Zero (players [i].GetBeer ());
			Assert.Zero (players [i].GetKnowledge ());
		}
	}

	[UnityTest]
	public IEnumerator FreshersFluCard_DeactivatePunishment_ReturnsBonuses(){
		//Tests that FreshersFluCard correctly restores the bonuses.
		Setup();
		game.InitializeMap ();
		yield return null;

		Player testPlayer = players [0];
		Card testCard = new FreshersFluCard (testPlayer);

		List<int> bonuses = new List<int> ();
		for (int i = 1; i < 4; i++){
			bonuses.Add (players [i].GetBeer ());
			bonuses.Add (players [i].GetKnowledge ());
		}
			
		testCard.activatePunishment (); //activate the punishment
		testCard.deactivatePunishment (); //deactivate the punishment

		//Test that the bonuses have been returned to normal for all "enemy" players.
		for (int i = 1; i < 4; i++) {
			Assert.AreEqual (bonuses [0], players [i].GetBeer ());
			bonuses.Remove (bonuses [0]);
			Assert.AreEqual (bonuses [0], players [i].GetKnowledge ());
			bonuses.Remove (bonuses [0]);
		}
	}

	[UnityTest]
	public IEnumerator FreshersFluCard_DeactivatePunishment_RecalculatesBonusesIfLandmarkCaptured(){
		//Tests that FreshersFluCard correctly recalculates the bonuses, which can be caused by players capturing landmarks
		//while the card is active.
		Setup();
		game.InitializeMap ();
		yield return null;

		Player testPlayer = players [0];
		Card testCard = new FreshersFluCard (testPlayer);

		Player player2 = players [1];
		int player2InitialKnowledgeBonus = player2.GetKnowledge ();

		//Create a new landmark sector that will be captured by player2 once the FreshersFluCard is played.
		Landmark newLandmark = new GameObject ("TestLandmark").AddComponent<Landmark> ();
		newLandmark.SetResourceType (Landmark.ResourceType.Knowledge);
		player2.ownedSectors [0].GetAdjacentSectors () [0].SetLandmark (newLandmark);

		testCard.activatePunishment ();
		player2.Capture (player2.ownedSectors [0].GetAdjacentSectors () [0]); //Capture the new landmark.
		testCard.deactivatePunishment();

		Assert.AreEqual (player2InitialKnowledgeBonus + 2, player2.GetKnowledge ());
	}

	[UnityTest]
	public IEnumerator FreshersFluCard_ActivatePunishment_StoresPvcBonuses(){
		//Test that activate punishment correctly stores the amount of extra bonus gained from the PVC.
		Setup();
		game.InitializeMap ();
		yield return null;

		Player testPlayer = players [0];
		FreshersFluCard testCard = new FreshersFluCard (testPlayer);

		Player player2 = players [1];
		player2.SetKnowledge (player2.GetKnowledge() + 4); //Give player2 an extra 4 knowledge points.

		testCard.activatePunishment ();

		Dictionary<Player,int[]> pvcBonuses = testCard.GetPvcBonuses();

		Assert.AreEqual (0, pvcBonuses [player2] [0]);
		Assert.AreEqual (4, pvcBonuses [player2] [1]);
	}

	[UnityTest]
	public IEnumerator KillerHangoverCard_EnemyPlayersMissTurn(){
		//Tests that all of the enemy players miss their turn when the card is played.
		Setup ();
		game.InitializeMap ();
		yield return null; //wait for test setup to load.

		Player testPlayer = players [0];
		game.currentPlayer = testPlayer;
		game.DisableTestMode ();
		Card testCard = new KillerHangoverCard (testPlayer);

		cardDeck.SetActiveCard (testCard); //add to active cards
		testCard.activatePunishment ();
		yield return null; //wait for game to update

		Assert.AreEqual (testPlayer, game.currentPlayer);
		Assert.AreEqual (Game.TurnState.Move1, game.GetTurnState ());

		game.EndTurn (); //End the turn;
		yield return null; //wait for game to update

		Assert.AreEqual (Game.TurnState.Move1, game.GetTurnState ());
		Assert.AreEqual (testPlayer, game.currentPlayer);
	}

	[UnityTest]
	public IEnumerator LectureStrikeCard_EnemyPlayersOnlyHaveOneMove(){
		//Tests that the enemy players only have one move per turn when this card is active.
		Setup ();
		game.InitializeMap ();
		yield return null; //wait for test setup to load.

		Player testPlayer = players [0];
		game.currentPlayer = testPlayer;
		game.DisableTestMode ();
		Card testCard = new LecturerStrikeCard (testPlayer);

		cardDeck.SetActiveCard (testCard); //add to active cards
		testCard.activatePunishment ();

		game.EndTurn (); //End the current turn to switch to next player
		yield return null;

		Assert.AreEqual (Game.TurnState.Move1, game.GetTurnState ());

		game.NextTurnState(); //Call next turn state;

		Assert.AreEqual (Game.TurnState.EndOfTurn, game.GetTurnState ()); //Has the turnstate jumped to EndOfTurn?
	}

	private void Setup(){
		//Load in the necessary GameObjects.
		game = MonoBehaviour.Instantiate(Resources.Load<GameObject>("GameManager")).GetComponent<Game>();
		map = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Map")).GetComponent<Map>();
		players = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Players")).GetComponentsInChildren<Player>();
		gui = MonoBehaviour.Instantiate(Resources.Load<GameObject>("GUI")).GetComponentsInChildren<PlayerUI>();
		cardDeck = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PunishmentCardGUI")).GetComponent<CardDeck> ();

		//Establish Connections
		game.players = players;
		game.gameMap = map.gameObject;
		map.game = game;
		map.sectors = map.gameObject.GetComponentsInChildren<Sector>();
		cardDeck.game = game;
		game.cardDeck = cardDeck;

		//Colour players
		players[0].SetColor(Color.red);
		players[1].SetColor(Color.blue);
		players[2].SetColor(Color.yellow);
		players[3].SetColor(Color.green);

		// establish references to a PlayerUI and Game for each player & initialize GUI
		for (int i = 0; i < players.Length; i++) 
		{
			players[i].SetGui(gui[i]);
			players[i].SetGame(game);
			players[i].GetGui().Initialize(players[i], i + 1);
			players[i].SetHuman (true);
		}

		//Initialize cardDeck
		cardDeck.Invoke("Start",0);
		// enable game's test mode
		game.EnableTestMode();
		game.currentPlayer = players [0];
		game.SetTurnState (Game.TurnState.Move1);

		//Add win screen and PVC unit
		GameObject winScreen = new GameObject("winScreen");
		winScreen.AddComponent<UnityEngine.UI.Text> ();
		game.winnerScreen = winScreen;

		GameObject pvc = new GameObject ("pvc");
		pvc.AddComponent<Landmark> ();
		pvc.AddComponent<MeshRenderer> ();
		pvc.AddComponent<MeshCollider> ();
		game.viceChancellorGameObj = pvc;
	}

	[TearDown] 
	public void ClearSceneAfterTest(){
		GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject in objects)
		{
			GameObject.Destroy (gameObject);
		}
	}
}
