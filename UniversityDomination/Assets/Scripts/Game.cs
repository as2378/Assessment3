//======================================================
//Website link with executable:
//http://www-users.york.ac.uk/~ch1575/documentation
//======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public Player[] players;
	public GameObject gameMap;
    public Player currentPlayer;
	public CardDeck cardDeck;


    public GameObject viceChancellorGameObj;    //Set in editor, stores vice chance game object
    public GameObject winner;                   //Stores which player has won.
    public GameObject winnerScreen;             //Reference to the screen displayed on game end.


    public enum TurnState { Move1, Move2, EndOfTurn, NULL };
    [SerializeField] private TurnState turnState;
    [SerializeField] private bool gameFinished = false;
    [SerializeField] private bool testMode = false;

    public TurnState GetTurnState() {
        return turnState;
    }

    public void SetTurnState(TurnState turnState) {
        this.turnState = turnState;
    }

    public bool IsFinished() {
        return gameFinished;
    }

    public void EnableTestMode() {
        testMode = true;
    }

    public void DisableTestMode() {
        testMode = false;
    }

	/*
	 * ASSESSMENT4 ADDITION: added an (i < numberOfPlayers) if statement to ensure that the correct
	 * number of players are human/non-human.
	 */
    public void CreatePlayers(int numberOfPlayers){

        // ensure that the specified number of players
        // is at least 2 and does not exceed 4
        if (numberOfPlayers < 2)
            numberOfPlayers = 2;

        if (numberOfPlayers > 4)
            numberOfPlayers = 4;

        // mark the specified number of players as human and non-human
        for (int i = 0; i < 4; i++)
        {
			if (i < numberOfPlayers) {
				players [i].SetHuman (true);
			} else {
				players [i].SetHuman (false);
			}
        }

        // give all players a reference to this game
		// and initialize their GUIs
		for (int i = 0; i < 4; i++)
		{
			players[i].SetGame(this);
			players[i].GetGui().Initialize(players[i], i + 1);
		}

    }

	public void InitializeMap() {

        // initialize all sectors, allocate players to landmarks,
        // and spawn units

        // get an array of all sectors
        Sector[] sectors = gameMap.GetComponentsInChildren<Sector>();

		// initialize each sector
        foreach (Sector sector in sectors)
		{
            sector.Initialize();
		}

        //===================code by charlie===================
        spawnVice(-1); //space vice chancellor (-1 indicates to spawn him randomly)

        //=====================================================

        // get an array of all sectors containing landmarks
        Sector[] landmarkedSectors = GetLandmarkedSectors(sectors);

        // ensure there are at least as many landmarks as players
        if (landmarkedSectors.Length < players.Length)
        {
            throw new System.Exception("Must have at least as many landmarks as players; only " + landmarkedSectors.Length.ToString() + " landmarks found for " + players.Length.ToString() + " players.");
        }

        // randomly allocate sectors to players
        foreach (Player player in players)
		{
			bool playerAllocated = false;
            while (!playerAllocated) {

				// choose a landmarked sector at random
                int randomIndex = Random.Range (0, landmarkedSectors.Length);

                // if the sector is not yet allocated, allocate the player

                //===================code by charlie===================
                Sector selectedSector = (Sector)landmarkedSectors[randomIndex];


                if (selectedSector.GetLandmark().GetComponent<Landmark>().GetResourceType() != Landmark.ResourceType.ViceChancellor)
                {
                    if (selectedSector.GetOwner() == null)
                    {
                        player.Capture(landmarkedSectors[randomIndex]);
                        playerAllocated = true;
                    }
                }
                //======================================================

                // retry until player is allocated
			}
		}

        // spawn units for each player
        foreach (Player player in players)
        {
            player.SpawnUnits();
        }

	}

    public void spawnVice(int sectorID) //set vice chancellor landmark, if sectorID is -1 then place randomly
    {
        Sector[] sectors = gameMap.GetComponentsInChildren<Sector>();

        Sector moveViceChance = sectors[0];

        if (sectorID == -1)
        {

            System.Random rnd = new System.Random(); //used to generate random numbers in selecting a random sector

            do
            {
                moveViceChance = sectors[rnd.Next(0, sectors.Length)]; //randomly select a sector (to vice the vice in to)
            } while (moveViceChance.GetComponent<Sector>().GetLandmark() != null); //do not place vice on a sector already with landmark

        }
        else
        {
            moveViceChance = sectors[sectorID];
        }

        viceChancellorGameObj.transform.SetParent(moveViceChance.transform); //move vice to selected sector

        MeshCollider toMoveSectorPosition = moveViceChance.GetComponent<MeshCollider>(); //place vice on selected sector
        viceChancellorGameObj.transform.position = new Vector3(toMoveSectorPosition.bounds.center.x, 2.03f, toMoveSectorPosition.bounds.center.z);

        viceChancellorGameObj.GetComponent<Landmark>().SetResourceType(Landmark.ResourceType.ViceChancellor);

        moveViceChance.GetComponent<Sector>().SetLandmark(viceChancellorGameObj.GetComponent<Landmark>());
    }

    private Sector[] GetLandmarkedSectors(Sector[] sectors) {

        // return a list of all sectors that contain landmarks from the given array

        List<Sector> landmarkedSectors = new List<Sector>();
        foreach (Sector sector in sectors)
        {
            if (sector.GetLandmark() != null)
            {
                landmarkedSectors.Add(sector);
            }
        }

        return landmarkedSectors.ToArray();
    }

    public bool NoUnitSelected() {

        // return true if no unit is selected, false otherwise


        // scan through each player
        foreach (Player player in players)
        {
            // scan through each unit of each player
            foreach (Unit unit in player.units)
            {
                // if a selected unit is found, return false
                if (unit.IsSelected() == true)
                    return false;
            }
        }

        // otherwise, return true
        return true;
    }

    public Unit GetSelectedUnit()
    {
        // Returns the selected unit if there is one,
        // null otherwise

        foreach(Sector sector in gameMap.GetComponent<Map>().sectors)
        {
            Unit unit = sector.GetUnit();

            if(unit != null && unit.IsSelected())
            {
                return sector.GetUnit();
            }
        }

        return null;
    }

    public void NextPlayer() {
		// set the current player to the next player in the order

        // deactivate the current player
        currentPlayer.SetActive(false);
		currentPlayer.GetGui().Deactivate();

        // find the index of the current player
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == currentPlayer)
            {
                // set the next player's index
                int nextPlayerIndex = i + 1;

                // if end of player list is reached, loop back to the first player
                if (nextPlayerIndex == players.Length)
                {
                    currentPlayer = players[0];
                    players[0].SetActive(true);
					players[0].GetGui().Activate();
                }

                // otherwise, set the next player as the current player
                else
                {
                    currentPlayer = players[nextPlayerIndex];
                    players[nextPlayerIndex].SetActive(true);
					players[nextPlayerIndex].GetGui().Activate();
                    break;
                }
            }
        }
        //ASSESSMENT4 ADDITION ------------------------------
        // Check if any of the active cards need to be deactivated and deactivate them
        cardDeck.DeactivatePunishmentCards (currentPlayer);
		cardDeck.AssignPunishmentCard (currentPlayer);
		//---------------------------------------------------
    }


    public void NextTurnState() {
        // change the turn state to the next in the order,
        // or to initial turn state if turn is completed

        switch (turnState)
        {
		case TurnState.Move1:
				//ASSESSMENT4 ADDITION: if the lectureStike card is active, end the turn after move1.
				bool foundCard = false;	
				if (cardDeck.GetActiveCards ().Count != 0) 
				{	
					foreach (Card activeCard in cardDeck.GetActiveCards()) 
					{
						if (activeCard.GetType () == typeof(LecturerStrikeCard) && activeCard.GetOwner () != currentPlayer) 
						{
							foundCard = true;
						} 
					}
				}
				if (foundCard) 
				{
					turnState = TurnState.EndOfTurn;
				} 
				else 
				{
					turnState = TurnState.Move2;
				}
                break;

            case TurnState.Move2:
                turnState = TurnState.EndOfTurn;
                break;

            case TurnState.EndOfTurn:
                turnState = TurnState.Move1;
                break;

            default:
                break;
        }
		UpdateGUI();
    }

    public void EndTurn() {

        // end the current turn and make sure that none of the units are selected
        turnState = TurnState.EndOfTurn;

		foreach (Player player in players)
		{
			//scan through each unit of each player
			foreach (Unit unit in player.units)
			{
				// if a selected unit is found, deselect it.
				if (unit.IsSelected () == true)
				{
					unit.Deselect ();
				}
			}
		}
    }

    public Player GetWinner() {

        // return the winning player, or null if no winner yet

        Player winner = null;

        // scan through each player
        foreach (Player player in players)
        {
            // if the player hasn't been eliminated
            if (!player.IsEliminated())
            {
                // if this is the first player found that hasn't been eliminated,
                // assume the player is the winner
                if (winner == null)
                    winner = player;

                // if another player that was not eliminated was already,
                // found, then return null
                else
                    return null;
            }
        }

        // if only one player hasn't been eliminated, then return it as the winner
        this.winner = winner.gameObject;
        return winner;
    }

    public void EndGame() {
        gameFinished = true;
        currentPlayer.SetActive(false);
        gameMap.SetActive(false);
        currentPlayer = null;
        turnState = TurnState.NULL;

        //======code by charlie=======
        winnerScreen.SetActive(true);
		if (winner != null) {
			string congratsMessage = "Congratulations " + winner.name + " you won!";
			winnerScreen.GetComponentInChildren<Text> ().text = congratsMessage;
		}
        //this.gameObject.SetActive(false);
        //============================
    }

	public void UpdateGUI() {
		// update all players' GUIs
		for (int i = 0; i < 4; i++) {
			players [i].GetGui ().UpdateDisplay ();
		}
	}

    public void Initialize () {
        // initialize the game
        // create a specified number of human players
        CreatePlayers( staticPassArguments.humanPlayers );

        //======code by charlie======
        if (staticPassArguments.loadGame == true)
        {
            this.gameObject.GetComponent<GameControl>().Load();
        }
        else
        {
            // initialize the map and allocate players to landmarks
            InitializeMap();

            // initialize the turn state
            turnState = TurnState.Move1;

            // set Player 1 as the current player
            currentPlayer = players[0];
            currentPlayer.GetGui().Activate();
            players[0].SetActive(true);
			cardDeck.AssignPunishmentCard (currentPlayer); //ASSESSMENT4 ADDITION
        }

        //===========================

        // update GUIs
        UpdateGUI();

	}

	/*
	 * ASSESSMENT4 ADDITION:
	 * Removed duplication in Update and UpdateAccessible.
	 */
    void Update () {
        // at the end of each turn, check for a winner and end the game if
        // necessary; otherwise, start the next player's turn

		if (!testMode) 
		{
			UpdateAccessible ();
		}
	}

    public void UpdateAccessible () {
		// if the current turn has ended 
		if (turnState == TurnState.EndOfTurn)
		{
			// if there is no winner yet
			if (GetWinner() == null)
			{
				//ASSESSMENT4 ADDITION:-------------------------------------
				//skip other players turns due to killer hangover
				if (cardDeck.HasActiveCardOfType (typeof(KillerHangoverCard)))
					cardDeck.DeactivatePunishmentCards (currentPlayer);
				//start next player's turn
				else
					NextPlayer();
				//----------------------------------------------------------
				NextTurnState();

				// skip eliminated players
				while (currentPlayer.IsEliminated())
					NextPlayer();

				// spawn units for the next player
				currentPlayer.SpawnUnits();
			}
			else
				if (!gameFinished)
					EndGame();
		}
    }

}
