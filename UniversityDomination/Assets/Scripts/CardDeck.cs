using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ASSESSMENT4 ADDITION:
 * This class is used to handle the visual elements of the card system, as well as provide a way of
 * assigning, activating and deactivating cards.
 */
public class CardDeck : MonoBehaviour {
	public GameObject menu;
	public GameObject menuBackground;
	public Game game;
	private Dictionary<GameObject,Card> cardSlots;
    private List<Card> activeCards = new List<Card>();

	public List<GameObject> GetCardSlots()
	{
		//Returns a list containing the card slots.
		return new List<GameObject>(cardSlots.Keys);
    }

	public List<Card> GetActiveCards()
	{
		return activeCards;
	}

	public void SetActiveCard(Card card)
	{
		//Adds a card to the activeCard list
		activeCards.Add (card);
	}

	public void RemoveActiveCard(Card card)
	{
		if (activeCards.Contains (card)) 
		{
			activeCards.Remove (card);
		}
	}
		
	public void AssignPunishmentCard(Player player)
	{
		//gives the player a new punishment card.

		int randInt = Random.Range (0, 100);

		if (randInt < 50) 
		{
			Card nothingCard = new NothingCard (player);
			player.AddPunishmentCard (nothingCard);
			return;
		}
		if (randInt < 100) 
		{
			Card freshersFluCard = new FreshersFluCard (player);
			player.AddPunishmentCard (freshersFluCard);
			return;
		}
	}

	public void DeactivatePunishmentCards(Player player)
	{
		//Deactivates all active cards that were played by the player if their turn count reaches 0.

		List<Card> cardsToDeactivate = new List<Card> ();
		foreach (Card activeCard in activeCards) 
		{
			if (activeCard.GetOwner () == player) 
			{
				activeCard.SetTurnCount (activeCard.GetTurnCount () - 1); //Decrease turn count of active card.
				//if turn count reaches 0, add to the cardsToDeactivate list.
				if (activeCard.GetTurnCount () == 0) 
				{
					cardsToDeactivate.Add (activeCard);
				}
			}
		}
		foreach (Card activeCard in cardsToDeactivate) 
		{
			//Deactivate Card
			activeCard.deactivatePunishment ();
			activeCards.Remove (activeCard);
		}
	}

	void Start () {
		//Goes through every child in menu and adds the cards to the cardSlot dictionary.
		cardSlots = new Dictionary<GameObject, Card>();
		for (int i = 0; i < menu.transform.childCount; i++) 
		{
			GameObject child = menu.transform.GetChild (i).gameObject;
			if (child.name.Substring (0, 4) == "Card") 
			{
				cardSlots.Add(child,null);
			}
		}
	}

	public void ShowMenu(){
		//Disable the punishment card menu if it isnt the first move of a turn or the current player is an AI.
		if (game.GetTurnState () != Game.TurnState.Move1 || game.currentPlayer.IsHuman() == false)
			return;
		
		Player currentPlayer = game.currentPlayer;
		List<Card> cards = currentPlayer.GetPunishmentCards ();

        //Assigns the player's cards to the card slots in the deck.
        List<GameObject> slotList = GetCardSlots();

        for (int i = 0; i < slotList.Count; i++) 
		{
			GameObject freeCardSlot = slotList [i];
			//if the player has a card to put into the deck:
			if (i < cards.Count) 
			{
				//assign the card to the free slot and update its image.
				cardSlots [freeCardSlot] = cards [i];	
				freeCardSlot.GetComponent<Image> ().sprite = cards [i].cardImage;
				freeCardSlot.GetComponent<Button> ().interactable = true;

				//If another of the same card is active, deactivate the cardslot.
				if (HasActiveCardOfType (cards [i].GetType ())) 
				{
					freeCardSlot.GetComponent<Button> ().interactable = false;
				}
			} 
			else 
			{
				freeCardSlot.GetComponent<Button> ().interactable = false;
				freeCardSlot.GetComponent<Image> ().sprite = null;
			}
		}

		//Show the menu and hide the map.
		menu.SetActive (true);
		menuBackground.SetActive (true);
		game.gameMap.SetActive (false);
	}

	public bool HasActiveCardOfType(System.Type cardType)
	{
		//Checks to the if a card with the same type "cardType" is currently active.
		foreach (Card activeCard in this.activeCards) 
		{
			if (activeCard.GetType () == cardType) 
			{
				return true;
			}
		}
		return false;
	}


	public void ActivateCard(GameObject slot)
	{
		//Click event for the card slots.
		Card card = cardSlots [slot];

		card.activatePunishment ();	// activates the card's effect.
		activeCards.Add (card); // adds card to the active list

		//Removes the card and hides the menu
		card.GetOwner ().GetPunishmentCards ().Remove (card);
		game.EndTurn ();
	
		HideMenu ();
	}

	public void HideMenu(){
		//Reset the cardSlot dictionary.
		List<GameObject> slotList = new List<GameObject> (cardSlots.Keys);
		for (int i = 0; i < slotList.Count; i++) 
		{
			GameObject slot = slotList [i];
			cardSlots [slot] = null;
		}
		//Hide the menu and reshow the map.
		menu.SetActive (false);
		menuBackground.SetActive (false);
		game.gameMap.SetActive (true);
	}
}