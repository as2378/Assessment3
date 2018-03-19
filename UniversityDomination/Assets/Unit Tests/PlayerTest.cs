using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class PlayerTest 
{
    private Game game;
    private Map map;
	private Player[] players;
	private PlayerUI[] gui;
	private CardDeck cardDeck;

    [UnityTest]
    public IEnumerator CaptureSector_ChangesOwner() {
        
        Setup();
        game.InitializeMap();

        Player previousOwner = map.sectors[0].GetOwner();

        game.players[0].Capture(map.sectors[0]);
        Assert.AreSame(map.sectors[0].GetOwner(), game.players[0]); // owner stored in sector
        Assert.IsTrue(game.players[0].ownedSectors.Contains(map.sectors[0])); // sector is stored as owned by the player

		if (previousOwner != null) // if sector had previous owner
        {
            Assert.IsFalse(previousOwner.ownedSectors.Contains(map.sectors[0])); // sector has been removed from previous owner list
        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator CaptureLandmark_BothPlayersBeerAmountCorrect() {
        
        Setup();

        // capturing landmark
        Sector landmarkedSector = map.sectors[1]; 
        landmarkedSector.Initialize();
        Landmark landmark = landmarkedSector.GetLandmark();
        Player playerA = game.players[0];
        Player playerB = game.players[1];
        playerB.Capture(landmarkedSector);

        // ensure 'landmarkedSector' is a landmark of type Beer
        Assert.IsNotNull(landmarkedSector.GetLandmark());
        landmark.SetResourceType(Landmark.ResourceType.Beer);

        // get beer amounts for each player before capture
        int attackerBeerBeforeCapture = playerA.GetBeer();
        int defenderBeerBeforeCapture = playerB.GetBeer();
        Player previousOwner = landmarkedSector.GetOwner();

        playerA.Capture(landmarkedSector);

        // ensure sector is captured correctly
        Assert.AreSame(landmarkedSector.GetOwner(), playerA);
        Assert.IsTrue(playerA.ownedSectors.Contains(landmarkedSector));

        // ensure resources are transferred correctly
        Assert.IsTrue(attackerBeerBeforeCapture + landmark.GetAmount() == playerA.GetBeer());
        Assert.IsTrue(defenderBeerBeforeCapture - landmark.GetAmount() == previousOwner.GetBeer());

        yield return null;
    }

    [UnityTest]
    public IEnumerator CaptureLandmark_BothPlayersKnowledgeAmountCorrect() {
        
        Setup();

        // capturing landmark
        Sector landmarkedSector = map.sectors[1]; 
        landmarkedSector.Initialize();
        Landmark landmark = landmarkedSector.GetLandmark();
        Player playerA = game.players[0];
        Player playerB = game.players[1];
        playerB.Capture(landmarkedSector);

        // ensure 'landmarkedSector' is a landmark of type Knowledge
        Assert.IsNotNull(landmarkedSector.GetLandmark());
        landmark.SetResourceType(Landmark.ResourceType.Knowledge);

        // get knowledge amounts for each player before capture
        int attackerKnowledgeBeforeCapture = playerA.GetKnowledge();
        int defenderKnowledgeBeforeCapture = playerB.GetKnowledge() + 2;
        Player previousOwner = landmarkedSector.GetOwner();

        playerA.Capture(landmarkedSector);

        // ensure sector is captured correctly
        Assert.AreSame(landmarkedSector.GetOwner(), playerA);
        Assert.IsTrue(playerA.ownedSectors.Contains(landmarkedSector));

        // ensure resources are transferred correctly
        Assert.IsTrue(attackerKnowledgeBeforeCapture + landmark.GetAmount() == playerA.GetKnowledge());
        Assert.IsTrue(defenderKnowledgeBeforeCapture - landmark.GetAmount() == previousOwner.GetKnowledge());

        yield return null;
    }

    [UnityTest]
    public IEnumerator CaptureLandmark_NeutralLandmarkPlayerBeerAmountCorrect() {
        
        Setup();

        // capturing landmark
        Sector landmarkedSector = map.sectors[1]; 
        landmarkedSector.Initialize();
        Landmark landmark = landmarkedSector.GetLandmark();
        Player playerA = game.players[0];

        // ensure 'landmarkedSector' is a landmark of type Beer
        Assert.IsNotNull(landmarkedSector.GetLandmark());
        landmark.SetResourceType(Landmark.ResourceType.Beer);

        // get player beer amount before capture
        int oldBeer = playerA.GetBeer();

        playerA.Capture(landmarkedSector);

        // ensure sector is captured correctly
        Assert.AreSame(landmarkedSector.GetOwner(), playerA);
        Assert.IsTrue(playerA.ownedSectors.Contains(landmarkedSector));

        // ensure resources are gained correctly
        Assert.IsTrue(playerA.GetBeer() - oldBeer == landmark.GetAmount());
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator CaptureLandmark_NeutralLandmarkPlayerKnowledgeAmountCorrect() {
        
        Setup();

        // capturing landmark
        Sector landmarkedSector = map.sectors[1]; 
        landmarkedSector.Initialize();
        Landmark landmark = landmarkedSector.GetLandmark();
        Player playerA = game.players[0];

        // ensure 'landmarkedSector' is a landmark of type Knowledge
        Assert.IsNotNull(landmarkedSector.GetLandmark());
        landmark.SetResourceType(Landmark.ResourceType.Knowledge);

        // get player knowledge amount before capture
        int oldKnowledge = playerA.GetKnowledge();

        playerA.Capture(landmarkedSector);

        // ensure sector is captured correctly
        Assert.AreSame(landmarkedSector.GetOwner(), playerA);
        Assert.IsTrue(playerA.ownedSectors.Contains(landmarkedSector));

        // ensure resources are gained correctly
        Assert.IsTrue(playerA.GetKnowledge() - oldKnowledge == landmark.GetAmount());

        yield return null;
    }

    [UnityTest]
    public IEnumerator SpawnUnits_SpawnedWhenLandmarkOwnedAndUnoccupied() {
        
        Setup();

        Sector landmarkedSector = map.sectors[1]; 
        Player playerA = game.players[0];

        // ensure that 'landmarkedSector' is a landmark and does not contain a unit
        landmarkedSector.Initialize();
        landmarkedSector.SetUnit(null);
        Assert.IsNotNull(landmarkedSector.GetLandmark());

        playerA.Capture(landmarkedSector);
        playerA.SpawnUnits();

        // ensure a unit has been spawned for playerA in landmarkedSector
        Assert.IsTrue(playerA.units.Contains(landmarkedSector.GetUnit()));

        yield return null;
    }

    [UnityTest]
    public IEnumerator SpawnUnits_NotSpawnedWhenLandmarkOwnedAndOccupied() {

        Setup();

        Sector landmarkedSector = map.sectors[1]; 
        Player playerA = game.players[0];

        // ensure that 'landmarkedSector' is a landmark and contains a Level 5 unit
        landmarkedSector.Initialize();
        landmarkedSector.SetUnit(MonoBehaviour.Instantiate(playerA.GetUnitPrefab()).GetComponent<Unit>());
        landmarkedSector.GetUnit().SetLevel(5);
        landmarkedSector.GetUnit().SetOwner(playerA);
        Assert.IsNotNull(landmarkedSector.GetLandmark());

        playerA.Capture(landmarkedSector);
        playerA.SpawnUnits();

        // ensure a Level 1 unit has not spawned over the Level 5 unit already in landmarkedSector
        Assert.IsTrue(landmarkedSector.GetUnit().GetLevel() == 5);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SpawnUnits_NotSpawnedWhenLandmarkNotOwned() {

        Setup();

        Sector landmarkedSector = map.sectors[1]; 
        Player playerA = game.players[0];
        Player playerB = game.players[1];
        landmarkedSector.SetUnit(null);

        // ensure that 'landmarkedSector' is a landmark and does not contain a unit
        landmarkedSector.Initialize();
        landmarkedSector.SetUnit(null);
        Assert.IsNotNull(landmarkedSector.GetLandmark());

        playerB.Capture(landmarkedSector);
        playerA.SpawnUnits();

        // ensure no unit is spawned at landmarkedSector
        Assert.IsNull(landmarkedSector.GetUnit());

        yield return null;
    }

    [UnityTest]
    public IEnumerator IsEliminated_PlayerWithNoUnitsAndNoLandmarksEliminated() {
        
        Setup();
        game.InitializeMap();

        Player playerA = game.players[0];

        Assert.IsFalse(playerA.IsEliminated()); // not eliminated because they have units

        for (int i = 0; i < playerA.units.Count; i++)
        {
            playerA.units[i].DestroySelf(); // removes units
        }
        Assert.IsFalse(playerA.IsEliminated()); // not eliminated because they still have a landmark

        // player[0] needs to lose their landmark
        for (int i = 0; i < playerA.ownedSectors.Count; i++)
        {
            if (playerA.ownedSectors[i].GetLandmark() != null)
            {
                playerA.ownedSectors[i].SetLandmark(null); // player[0] no longer has landmarks
            }
        }
        Assert.IsTrue(playerA.IsEliminated());

        yield return null;
    }

	/*
	 * ASSESSMENT4 ADDITION: added tests to the AI methods because they were not provided by the previous
	 * team.
	 */
	[UnityTest]
	public IEnumerator ComputerTurn_MakesAValidMove(){
		//Tests that calling ComputerTurn allows the player to make a valid move.
		Setup ();
		game.InitializeMap ();

		Player nonHumanPlayer = players [0]; //Player's isHuman() = true so that ComputerTurn isnt called automatically.
		Sector unitsInitialSector = nonHumanPlayer.units [0].GetSector ();

		nonHumanPlayer.ComputerTurn ();

		yield return new WaitForSeconds (1); //wait for player to make their move.

		int numberOfOwnedSectors = nonHumanPlayer.ownedSectors.Count;
		Sector unitsNewSector = nonHumanPlayer.units [0].GetSector ();

		//Tests that a valid move was made by calling ComputerTurn
		Assert.AreEqual (2, numberOfOwnedSectors,"The computer player did not capture another sector");
		Assert.AreNotSame (unitsInitialSector, unitsNewSector, "The unit did not move sectors");
		Assert.Contains (unitsNewSector, unitsInitialSector.GetAdjacentSectors (), "The unit did not move into an adjacent sector");
	}

	/*
	 * ASSESSMENT4 ADDITION: added tests to the AI methods because they were not provided by the previous
	 * team.
	 */
	[UnityTest]
	public IEnumerator ComputerTurn_InvalidTurnStateNoMoveIsMade(){
		//Tests that calling ComputerTurn when the turnState is either EndOfTurn or NULL, means that the player does not make a move.
		Setup ();
		game.InitializeMap ();

		Player nonHumanPlayer = players [0]; //Player's isHuman() = true so that ComputerTurn isnt called automatically.
		Sector unitsInitialSector = nonHumanPlayer.units [0].GetSector ();
		int previousNumberOfOwnedSectors = nonHumanPlayer.ownedSectors.Count;


		game.SetTurnState (Game.TurnState.EndOfTurn); //Set the turnstate to EndOfTurn.
		nonHumanPlayer.ComputerTurn (); 
		yield return new WaitForSeconds (1); //wait for player to potentially make a move.

		game.SetTurnState (Game.TurnState.NULL); //Set the turnstate to NULL.
		nonHumanPlayer.ComputerTurn ();
		yield return new WaitForSeconds (1); //wait for player to potentially make a move.

		int currentNumberOfOwnedSectors = nonHumanPlayer.ownedSectors.Count;
		Sector unitsNewSector = nonHumanPlayer.units [0].GetSector ();

		//Tests that the state of the game has not changed.
		Assert.AreEqual (previousNumberOfOwnedSectors, currentNumberOfOwnedSectors,"The computer player captured another sector.");
		Assert.AreSame (unitsInitialSector, unitsNewSector, "The unit moved sectors in an invalid turnstate");
	}

	/*
	 * ASSESSMENT4 ADDITION: added tests for the AI playing punishment cards.
	 */
	[UnityTest]
	public IEnumerator ComputerPlayPunishmentCard_CardPlayed(){
		//Tests to see if ComputerPlayPunishmentCard correctly plays a card.
		Setup();
		game.SetTurnState (Game.TurnState.Move1);
		//Add a test card to the current player.
		Player testPlayer = game.currentPlayer;
		Card testCard = new FreshersFluCard (testPlayer);
		testPlayer.AddPunishmentCard (testCard);

		testPlayer.ComputerPlayPunishmentCard (); //Try to play a card.

		//Test if testCard has been played
		Assert.Contains (testCard, cardDeck.GetActiveCards ());
		Assert.IsFalse (testPlayer.GetPunishmentCards ().Contains (testCard));
		Assert.AreEqual (Game.TurnState.EndOfTurn, game.GetTurnState ());
		yield return null;
	}

	/*
	 * ASSESSMENT4 ADDITION: added tests for the AI playing punishment cards.
	 */
	[UnityTest]
	public IEnumerator ComputerPlayPunishmentCard_MakesNormalMoveWhenNoCardsOwned(){
		//Tests that the AI makes a normal move when ComputerPlayPunishmentCard is called when the AI doesn't have any cards.
		Setup();
		game.InitializeMap ();
		Player testPlayer = game.currentPlayer;
		Sector unitsInitialSector = testPlayer.units [0].GetSector ();

		testPlayer.GetPunishmentCards ().Clear (); //Ensure testPlayer does not have any cards.
		testPlayer.ComputerPlayPunishmentCard();

		yield return new WaitForSeconds (1); //wait for player to make their move.

		int numberOfOwnedSectors = testPlayer.ownedSectors.Count;
		Sector unitsNewSector = testPlayer.units [0].GetSector ();

		//Tests that a normal move was made.
		Assert.AreEqual (2, numberOfOwnedSectors,"The computer player did not capture another sector");
		Assert.AreNotSame (unitsInitialSector, unitsNewSector, "The unit did not move sectors");
		Assert.Contains (unitsNewSector, unitsInitialSector.GetAdjacentSectors (), "The unit did not move into an adjacent sector");
	}

	[UnityTest]
	public IEnumerator ComputerPlayPunishmentCard_MakesNormalMoveWhenInvalidTurnState(){
		//Tests that the AI makes a normal move when ComputerPlayPunishmentCard is called and the turnstate is not Move1.
		Setup();
		game.InitializeMap ();
		Player testPlayer = game.currentPlayer;
		Sector unitsInitialSector = testPlayer.units [0].GetSector ();
		Card testCard = new NothingCard (testPlayer);

		testPlayer.AddPunishmentCard (testCard); 	//Add a card to testPlayer...
		game.SetTurnState (Game.TurnState.Move2);	//...but make the TurnState Move2.
		testPlayer.ComputerPlayPunishmentCard();

		yield return new WaitForSeconds (1); //wait for player to make their move.

		int numberOfOwnedSectors = testPlayer.ownedSectors.Count;
		Sector unitsNewSector = testPlayer.units [0].GetSector ();

		//Tests that a normal move was made.
		Assert.IsFalse(cardDeck.GetActiveCards().Contains(testCard));
		Assert.AreEqual (2, numberOfOwnedSectors,"The computer player did not capture another sector");
		Assert.AreNotSame (unitsInitialSector, unitsNewSector, "The unit did not move sectors");
		Assert.Contains (unitsNewSector, unitsInitialSector.GetAdjacentSectors (), "The unit did not move into an adjacent sector");
	}

	/*
	 * ASSESSMENT4 ADDITION: added tests for the AI playing punishment cards.
	 */
	[UnityTest]
	public IEnumerator ComputerPlayPunishmentCard_MakesNormalMoveWhenAllCardsOwnedAreActive(){
		//Tests that the AI makes a normal move if ComputerPlayPunishmentCard is called when all of the cards
		//owned by the AI are already active in the game.
		Setup();
		game.InitializeMap ();

		//Activate a NothingCard and a FreshersFluCard.
		cardDeck.SetActiveCard(new NothingCard(null));
		cardDeck.SetActiveCard (new FreshersFluCard (null));

		Player testPlayer = game.currentPlayer;
		Sector unitsInitialSector = testPlayer.units [0].GetSector ();
		Card testCard1 = new NothingCard (testPlayer);
		Card testCard2 = new FreshersFluCard (testPlayer);

		//Add the testCards to the testPlayer
		testPlayer.AddPunishmentCard (testCard1);
		testPlayer.AddPunishmentCard (testCard2);

		testPlayer.ComputerPlayPunishmentCard(); //Try to play a punishment card.

		yield return new WaitForSeconds (1); //wait for the player to make their move.

		int numberOfOwnedSectors = testPlayer.ownedSectors.Count;
		Sector unitsNewSector = testPlayer.units [0].GetSector ();

		//Tests that a normal move was made.
		Assert.IsFalse(cardDeck.GetActiveCards().Contains(testCard1),"The AI's NothingCard was activated");
		Assert.IsFalse(cardDeck.GetActiveCards().Contains(testCard2),"The AI's FreshersFluCard was activated");
		Assert.AreEqual (2, numberOfOwnedSectors,"The computer player did not capture another sector");
		Assert.AreNotSame (unitsInitialSector, unitsNewSector, "The unit did not move sectors");
		Assert.Contains (unitsNewSector, unitsInitialSector.GetAdjacentSectors (), "The unit did not move into an adjacent sector");

		yield return null;
	}


	/*
	 * ASSESSMENT4 ADDITION: added tests for the new punishment card features.
	 */
	[UnityTest]
	public IEnumerator GetPunishmentCards_NewPlayerReturnsEmptyCardList(){
		//Tests that when a new player is created, GetPunishmentCards returns an empty list of cards.
		GameObject playerObject = new GameObject ("TestPlayer");
		Player player = playerObject.AddComponent<Player> ();

		List<Card> expectedList = new List<Card> ();
		List<Card> actualList = player.GetPunishmentCards ();

		Assert.AreEqual (expectedList, actualList);
		yield return null;
	}

	/*
	 * ASSESSMENT4 ADDITION: added tests for the new punishment card features.
	 */
	[UnityTest]
	public IEnumerator AddPunishmentCard_CorrectlyAddsCardToList(){
		//Tests if AddPunishmentCard adds the card to the punishmentCards list when the card is owned by the player
		// and the punishmentCards list is empty.

		GameObject playerObject = new GameObject ("TestPlayer");
		Player testPlayer = playerObject.AddComponent<Player> ();

		Card testCard = new NothingCard (testPlayer);

		List<Card> expectedList = new List<Card>(new Card[]{testCard}); //Expect a list containing TestCard.

		testPlayer.GetPunishmentCards ().Clear (); //Ensure list is clear.
		testPlayer.AddPunishmentCard(testCard);

		List<Card> actualList = testPlayer.GetPunishmentCards ();

		Assert.AreEqual (expectedList, actualList);
		Assert.AreSame (testCard, actualList [0]);
		yield return null;
	}

	/*
	 * ASSESSMENT4 ADDITION: added tests for the new punishment card features.
	 */
	[UnityTest]
	public IEnumerator AddPunishmentCard_DoesNotAddCardIfListIsFull(){
		//Tests that the card is not added to a player's punishmentCards list if it already contains 5 cards.
		GameObject playerObject = new GameObject ("TestPlayer");
		Player testPlayer = playerObject.AddComponent<Player> ();

		testPlayer.GetPunishmentCards ().Clear (); //Ensure list is clear.
		//Give the player 5 cards.
		for (int i = 0; i < 5; i++) {
			testPlayer.GetPunishmentCards ().Add (new NothingCard (testPlayer));
		}

		Card testCard = new FreshersFluCard (testPlayer);
		testPlayer.AddPunishmentCard (testCard); //Try to add the testCard.

		Assert.False (testPlayer.GetPunishmentCards ().Contains (testCard)); //Test whether testCard has been added or not.
		yield return null;
	}

	/*
	 * ASSESSMENT4 ADDITION: added tests for the new punishment card features.
	 */
	[UnityTest]
	public IEnumerator AddPunishmentCard_DoesNotAddCardIfCardIsNotOwnedByPlayer(){
		//Tests that cards are not added to a player's punishmentCards list if the player does not own the card.
		GameObject playerObject1 = new GameObject ("TestPlayer_1");
		GameObject playerObject2 = new GameObject ("TestPlayer_2");
		Player currentPlayer = playerObject1.AddComponent<Player> ();
		Player otherPlayer = playerObject2.AddComponent<Player> ();

		Card testCard = new FreshersFluCard (otherPlayer);
		currentPlayer.AddPunishmentCard (testCard); //Try to add the testCard.

		Assert.False (currentPlayer.GetPunishmentCards ().Contains (testCard)); //Test whether testCard has been added or not.
		yield return null;
	}

	/*
	 * ASSESSMENT4 ADDITION: added code which creates a PVC unit, places it within the scene and
	 * links it to the game class.
	 */
    private void Setup() {
        
        // initialize the game, map, and players with any references needed
        // the "GameManager" asset contains a copy of the GameManager object
        // in the 4x4 Test, but its script lacks references to players & the map
        game = MonoBehaviour.Instantiate(Resources.Load<GameObject>("GameManager")).GetComponent<Game>();

        // the "Map" asset is a copy of the 4x4 Test map, complete with
        // adjacent sectors and landmarks at (0,1), (1,3), (2,0), and (3,2),
        // but its script lacks references to the game & sectors
        map = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Map")).GetComponent<Map>();

        // the "Players" asset contains 4 prefab Player game objects; only
        // references not in its script is each player's color
        players = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Players")).GetComponentsInChildren<Player>();

		// the "GUI" asset contains the PlayerUI object for each Player
		gui = MonoBehaviour.Instantiate(Resources.Load<GameObject>("GUI")).GetComponentsInChildren<PlayerUI>();

		// ASSESSMENT4 ADDITION: load in the PunishmentCardGUI
		cardDeck = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PunishmentCardGUI")).GetComponent<CardDeck> ();

        // establish references from game to players & map
        game.players = players;
        game.gameMap = map.gameObject;
        

        // establish references from map to game & sectors (from children)
        map.game = game;
        map.sectors = map.gameObject.GetComponentsInChildren<Sector>();

		//ASSESSMENT4 ADDITION: establish reference to cardDeck & game
		cardDeck.game = game;
		game.cardDeck = cardDeck;

        // establish references to SSB 64 colors for each player
        players[0].SetColor(Color.red);
        players[1].SetColor(Color.blue);
        players[2].SetColor(Color.yellow);
        players[3].SetColor(Color.green);

		// establish references to a PlayerUI and Game for each player & initialize GUI
		for (int i = 0; i < players.Length; i++) 
		{
			players [i].SetGui(gui[i]);
			players [i].SetGame(game);
			players [i].GetGui().Initialize(players[i], i + 1);
			players [i].SetHuman (true);
		}
		// enable game's test mode
		game.EnableTestMode();

		//ASSESSMENT4 ADDITION:
		GameObject winScreen = new GameObject();
		winScreen.AddComponent<UnityEngine.UI.Text> ();
		winScreen.name = "winScreen";
		game.winnerScreen = winScreen;

		GameObject pvc = new GameObject ();
		pvc.AddComponent<Landmark> ();
		pvc.AddComponent<MeshRenderer> ();
		pvc.AddComponent<MeshCollider> ();
		pvc.name = "PVC";
		game.viceChancellorGameObj = pvc;
		game.currentPlayer = players [0];
    }

	/*
	 * ASSESSMENT4 ADDITION: added this teardown method to clear the scene of all the gameobjects.
	 * This means that the tests do not keep adding gameobjects to the scene, which slowed them down.
	 */
	[TearDown] 
	public void ClearSceneAfterTest(){
		GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject in objects)
		{
			GameObject.Destroy (gameObject);
		}
	}
}