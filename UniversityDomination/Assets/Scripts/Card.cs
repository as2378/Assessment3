using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ASSESSMENT4 ADDITIONS
public abstract class Card {
	public Sprite cardImage; //Image of the card
	private Player owner;	 //Card's owner
	private int turnCount;   //how many turns is the card's effect active for.

	public Player GetOwner(){
		return owner;
	}

	public void SetOwner(Player player){
		owner = player;
	}

	public int GetTurnCount(){
		return turnCount;
	}

	public void SetTurnCount(int value){
		if (value >= 0) 
		{
			this.turnCount = value;
		}
	}

	public Card(Player owner,Sprite image, int turnCount){
		this.owner = owner;
		this.cardImage = image;
		this.turnCount = turnCount;
	}

	public virtual void deactivatePunishment (){
		Debug.Log ("Card Deactivated");
		this.owner = null;
	}

	public override string ToString()
	{
		return "Card: "+ this.GetType ().ToString () + " ownedBy: " + owner.name;
	}
		
	public abstract void activatePunishment();
}
