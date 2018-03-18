using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * ASSESSMENT4 ADDITION:
 * Added in order to test the functionality of CardDeck and the card allocation system.
 */
public class CardDeckTest {
	private Game game;
	private Map map;
	private Player[] players;
	private PlayerUI[] gui;
	private CardDeck cardDeck;

	[UnityTest]
	public IEnumerator SetActiveCard_AddsCardToActiveCardList(){
		//Tests that SetActiveCard correctly adds cards to the activeCards list.
		cardDeck = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PunishmentCardGUI")).GetComponent<CardDeck> ();
		//Create two new cards
		Card card1 = new NothingCard (null); 
		Card card2 = new FreshersFluCard (null);

		cardDeck.GetActiveCards ().Clear (); //Clear the active card list
		cardDeck.SetActiveCard(card1); //Add card1
		Assert.Contains (card1, cardDeck.GetActiveCards ()); //Has card1 been added?

		cardDeck.SetActiveCard (card2); //Add card2
		Assert.Contains(card1,cardDeck.GetActiveCards()); //Is card1 still active?
		Assert.Contains(card2,cardDeck.GetActiveCards()); //Has card2 been added?
		yield return null;
	}

	[UnityTest]
	public IEnumerator RemoveActiveCard_RemovesCardFromActiveCards(){
		//Tests that RemoveActiveCard correctly removes cards from the activeCards list.
		cardDeck = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PunishmentCardGUI")).GetComponent<CardDeck> ();

		//Create two new cards
		Card card1 = new NothingCard (null); 
		Card card2 = new FreshersFluCard (null);

		cardDeck.GetActiveCards ().Clear (); //Clear the active card list
		cardDeck.SetActiveCard(card1);  //Add card1
		cardDeck.SetActiveCard (card2); //Add card2

		cardDeck.RemoveActiveCard (card1); //Remove card1
		Assert.Contains(card2,cardDeck.GetActiveCards()); //Is card2 still active?
		Assert.False(cardDeck.GetActiveCards().Contains(card1)); //Has card1 been removed?

		cardDeck.RemoveActiveCard (card2); //Remove card2
		Assert.AreEqual(0, cardDeck.GetActiveCards().Count); //Test that the list is empty

		yield return null;
	}

	[UnityTest]
	public IEnumerator AssignPunishmentCard_GivesPlayerANewPunishmentCard(){
		//Tests if AssignPunishmentCard adds a card to a player's punishment card list.
		Setup ();

		//Get player1's current punishment card list
		Player player1 = players [0];
		List<Card> player1Cards = new List<Card>(player1.GetPunishmentCards ()); //Copy the current punishment card list

		cardDeck.AssignPunishmentCard (player1); //Assign a card to player1

		Assert.AreNotEqual (player1Cards, player1.GetPunishmentCards ()); //Has the list changed?
		Assert.AreEqual (player1Cards.Count + 1, player1.GetPunishmentCards ().Count); //Has exactly 1 card been added?
		Assert.IsNotNull (player1.GetPunishmentCards () [0]); //Is the element a valid card (i.e. not null)?
		yield return null;
	}

	[UnityTest]
	public IEnumerator DeactivatePunishmentCards_NoActiveCards(){
		//Tests that when DeactivatePunishmentCards is called when the player has no active cards, nothing changes.
		Setup();

		Player player1 = players [0];
		cardDeck.AssignPunishmentCard (player1); //Give the player a punishment card
		Card player1Card = player1.GetPunishmentCards()[0]; //Get the card that was just added.
		int initialTurnCount = player1Card.GetTurnCount();

		Assert.IsFalse (cardDeck.GetActiveCards ().Contains (player1Card)); //Assert that the card added has not been activated.

		cardDeck.DeactivatePunishmentCards (player1); //Call deactivatePunishmentCards for player1.

		Assert.IsFalse (cardDeck.GetActiveCards ().Contains (player1Card)); //Assert that the card added has not been activated.
		Assert.AreEqual(initialTurnCount,player1Card.GetTurnCount()); //Test that the turncount hasn't decreased.
		Assert.AreSame (player1, player1Card.GetOwner ()); //Test if the card is still owned by player1.
		yield return null;
	}

	[UnityTest]
	public IEnumerator DeactivatePunishmentCards_OneActiveCardToDeactivate(){
		//Tests that if there is an active card owned by the player, it becomes deactivated when deactivatePunishmentCards is called.
		Setup();

		Player player1 = players [0];
		cardDeck.AssignPunishmentCard (player1); //Give the player a punishment card
		Card player1Card = player1.GetPunishmentCards()[0]; //Get the card that was just added.
		player1Card.SetTurnCount(1);
		cardDeck.GetActiveCards ().Add (player1Card); //Add player1's card to the active card list

		cardDeck.DeactivatePunishmentCards (player1);

		//Test that the card has been deactivated.
		Assert.IsFalse (cardDeck.GetActiveCards ().Contains (player1Card));
		Assert.Zero (player1Card.GetTurnCount ());
		Assert.IsNull (player1Card.GetOwner ());

		yield return null;
	}

	[UnityTest]
	public IEnumerator DeactivatePunishmentCards_TwoActiveCardsToDeactivate(){
		//Tests that if there is an active card owned by the player, it becomes deactivated when deactivatePunishmentCards is called.
		Setup();
		//Give the player two punishment cards
		Player player1 = players [0];

		Card card1 = new FreshersFluCard (player1);
		Card card2 = new NothingCard (player1);
		player1.AddPunishmentCard (card1);
		player1.AddPunishmentCard (card2);

		//Activate cards
		cardDeck.GetActiveCards ().Add (card1); 
		cardDeck.GetActiveCards ().Add (card2);

		card1.SetTurnCount (1);
		card2.SetTurnCount (1);

		cardDeck.DeactivatePunishmentCards (player1);

		//Test that both cards have been deactivated.
		foreach(Card card in new Card[]{card1,card2})
		{
			Assert.IsFalse (cardDeck.GetActiveCards ().Contains (card));
			Assert.Zero (card.GetTurnCount ());
			Assert.IsNull (card.GetOwner ());
		}

		yield return null;
	}

	[UnityTest]
	public IEnumerator DeactivatePunishmentCards_TwoActiveCardsOneToDeactivate(){
		//Tests that if there is an active card owned by the player, it becomes deactivated when deactivatePunishmentCards is called.
		Setup();
		//Give the player two punishment cards
		Player player1 = players [0];

		Card card1 = new FreshersFluCard (player1);
		Card card2 = new NothingCard (player1);
		player1.AddPunishmentCard (card1);
		player1.AddPunishmentCard (card2);

		//Activate cards
		cardDeck.GetActiveCards ().Add (card1); 
		cardDeck.GetActiveCards ().Add (card2);

		card1.SetTurnCount (2); //Card will not be deactivated this turn...
		card2.SetTurnCount (1); //...But this one will.

		cardDeck.DeactivatePunishmentCards (player1);

		//Test that card1 is still active.
		Assert.Contains(card1,cardDeck.GetActiveCards());
		Assert.AreEqual (1, card1.GetTurnCount ());
		Assert.AreSame (player1, card1.GetOwner ());

		//Test that card2 has been deactivated.
		Assert.IsFalse (cardDeck.GetActiveCards ().Contains (card2));
		Assert.Zero (card2.GetTurnCount ());
		Assert.IsNull (card2.GetOwner ());
		yield return null;
	}

	[UnityTest]
	public IEnumerator DeactivatePunishmentCards_OtherPlayerHasActiveCardNoneToDeactivate(){
		//Tests that if deactivatedPunishmentCards is called for a player with no active cards, another player's active card will not be deactivated.
		Setup();
		//Give player1 a punishment cards
		Player player1 = players [0];
		Card activeCard = new FreshersFluCard (player1);
		player1.AddPunishmentCard (activeCard);

		//Activate card
		cardDeck.GetActiveCards ().Add (activeCard); 
		activeCard.SetTurnCount (1);

		Player player2 = players [1];
		cardDeck.DeactivatePunishmentCards (player2); //Call DeactivatePunishmentCards for player2.

		//Test that activeCard is still active.
		Assert.Contains(activeCard,cardDeck.GetActiveCards());
		Assert.AreEqual (1, activeCard.GetTurnCount ());
		Assert.AreSame (player1, activeCard.GetOwner ());
		yield return null;
	}

	[UnityTest]
	public IEnumerator ShowMenu_DoesNotShowMenu(){
		//Test if ShowMenu doesn't show the menu when the TurnState is not Move1 or the currentPlayer is not human.
		Setup ();
		yield return null;

		Game.TurnState[] incorrectTurnStates = new Game.TurnState[] {
			Game.TurnState.Move2,
			Game.TurnState.EndOfTurn,
			Game.TurnState.NULL
		};
		foreach (Game.TurnState turnState in incorrectTurnStates) 
		{
			game.SetTurnState (turnState); //Change the turnState
			cardDeck.ShowMenu(); // Try to show the menu
			//Test that the menu remains hidden.
			Assert.IsFalse (cardDeck.menu.activeInHierarchy);
			Assert.IsTrue (map.gameObject.activeInHierarchy);
		}
		game.SetTurnState (Game.TurnState.Move1); //Reset the turnState
		game.currentPlayer.SetHuman(false); //Make current player nonHuman.
		cardDeck.ShowMenu(); // Try to show the menu
		//Test that the menu remains hidden.
		Assert.IsFalse (cardDeck.menu.activeInHierarchy);
		Assert.IsTrue (map.gameObject.activeInHierarchy);
	}

	[UnityTest]
	public IEnumerator ShowMenu_PlayerHasNoCards(){
		//Tests that if the current player has no cards, all the cards slots will be empty & disabled.
		Setup();
		yield return null;

		Player testPlayer = game.currentPlayer;
		testPlayer.GetPunishmentCards ().Clear (); //Ensure testPlayer has no cards.

		cardDeck.ShowMenu ();

		foreach (GameObject cardSlot in cardDeck.GetCardSlots()) 
		{
			//Test that all card slots have been disabled and their images removed.
			Assert.IsFalse(cardSlot.GetComponent<Button> ().IsInteractable());
			Assert.IsNull (cardSlot.GetComponent<Image> ().sprite);
		}
	}

	[UnityTest]
	public IEnumerator ShowMenu_OneCardPlayableOneCardNot(){
		//Tests that if the current player owns a card that has not been activated, it will appear in a card slot.
		//Tests that if the current player owns a card that has been activated by another player, it will be disabled.
		Setup();
		yield return null;
		cardDeck.SetActiveCard (new FreshersFluCard (null)); //Activate a FreshersFluCard
		Player testPlayer = game.currentPlayer;
		testPlayer.GetPunishmentCards ().Clear (); //Ensure testPlayer has no cards.

		//Give the testPlayer two cards; One Nothing and one FreshersFlu
		Card card1 = new NothingCard(testPlayer);
		Card card2 = new FreshersFluCard (testPlayer);
		testPlayer.AddPunishmentCard (card1);
		testPlayer.AddPunishmentCard (card2);

		cardDeck.ShowMenu (); //Show the card deck menu

		List<GameObject> cardSlots = cardDeck.GetCardSlots ();

		//Test if the card images have been added to card slots
		Assert.AreEqual (card1.cardImage, cardSlots [0].GetComponent<Image> ().sprite);
		Assert.AreEqual (card2.cardImage, cardSlots [1].GetComponent<Image> ().sprite);

		//Test if card1's slot is enabled and card2's slot is disabled.
		Assert.IsTrue(cardSlots[0].GetComponent<Button>().IsInteractable());
		Assert.IsFalse (cardSlots [1].GetComponent<Button> ().IsInteractable ());
	}

	[UnityTest]
	public IEnumerator HasActiveCardOfType_NoMatchingActiveCardReturnsFalse(){
		//Test that HasActiveCardOfType returns false if there are no active cards matching the type specified.
		cardDeck = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PunishmentCardGUI")).GetComponent<CardDeck> ();
		cardDeck.SetActiveCard (new NothingCard (null)); // Add a NothingCard to the activeCards list.

		Assert.IsFalse(cardDeck.HasActiveCardOfType (typeof(FreshersFluCard))); //Check if a freshersFluCard is active
		yield return null;
	}

	[UnityTest]
	public IEnumerator HasActiveCardOfType_InvalidCardTypeReturnsFalse(){
		//Test that HasActiveCardOfType returns false if an invalid card type is used.
		cardDeck = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PunishmentCardGUI")).GetComponent<CardDeck> ();
		cardDeck.SetActiveCard (new NothingCard (null)); // Add a NothingCard to the activeCards list.

		//Check if a card of type String is active, this should return false.
		Assert.IsFalse(cardDeck.HasActiveCardOfType (typeof(string))); 
		yield return null;
	}

	[UnityTest]
	public IEnumerator HasActiveCardOfType_FindsMatchingActiveCardReturnsTrue(){
		//Test that HasActiveCardOfType returns true if a card matching the type specified is active.
		cardDeck = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PunishmentCardGUI")).GetComponent<CardDeck> ();
		cardDeck.SetActiveCard (new NothingCard (null)); // Add a NothingCard to the activeCards list.

		//Check if a card of type NothingCard is active, this should return true.
		Assert.IsTrue(cardDeck.HasActiveCardOfType (typeof(NothingCard)));

		//Check if a card of type Card is active, this should return false.
		Assert.IsFalse(cardDeck.HasActiveCardOfType (typeof(Card)));

		yield return null;
	}
		
	[UnityTest]
	public IEnumerator ActivateCard_CardIsTakenFromPlayerAndActivated(){
		//Tests if the ActivateCard method correctly activates the card, removes it from the player's ownership and ends the turn.
		Setup ();
		yield return null;
		game.SetTurnState (Game.TurnState.Move1); //Ensure turnState is Move1.
		//Give player1 a punishment card.
		Player player1 = game.currentPlayer;
		Card testCard = new FreshersFluCard (player1);
		player1.AddPunishmentCard (testCard);

		cardDeck.ShowMenu (); //Initialize player1's cardDeck.
		cardDeck.ActivateCard(cardDeck.GetCardSlots()[0]); //Activate the card

		Assert.Contains (testCard, cardDeck.GetActiveCards ()); //Test that testCard has been added to the active list.
		Assert.IsFalse (player1.GetPunishmentCards ().Contains (testCard)); //Test that the card has been removed from the player's card list
		Assert.AreEqual(Game.TurnState.EndOfTurn, game.GetTurnState()); //Test that the turn has finished.
	}

	[UnityTest]
	public IEnumerator HideMenu_MenuHiddenAndMapShown(){
		//Tests that HideMenu hides the card menu and reshows the map.
		Setup ();
		yield return null;

		cardDeck.ShowMenu();

		Assert.IsTrue (cardDeck.menu.activeInHierarchy);
		Assert.IsFalse (map.gameObject.activeInHierarchy);

		cardDeck.HideMenu (); //Hide the menu.

		Assert.IsFalse (cardDeck.menu.activeInHierarchy);
		Assert.IsTrue (map.gameObject.activeInHierarchy);
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
