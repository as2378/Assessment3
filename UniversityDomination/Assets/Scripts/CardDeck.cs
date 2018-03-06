using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour {
	public GameObject menu;
	public GameObject menuBackground;
	public Game game;
	private Dictionary<GameObject,Card> cardSlots;

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
		if (game.GetTurnState () != Game.TurnState.Move1)
			return;
		
		Player currentPlayer = game.currentPlayer;
		List<Card> cards = currentPlayer.GetPunishmentCards ();

		//In Game, first player first turn does not assign a card

		//Assigns the player's cards to the card slots in the deck.
		List<GameObject> slotList = new List<GameObject> (cardSlots.Keys);
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

	public void ActivateCard(GameObject slot)
	{
		//Click event for the card slots
		Card card = cardSlots [slot];
		card.activatePunishment ();	// activates the card's effect.

		//Removes the card and hides the menu
		card.GetOwner ().GetPunishmentCards ().Remove (card);
		card.SetOwner (null);
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
