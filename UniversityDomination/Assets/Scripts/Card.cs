using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Card {
	public Sprite cardImage;
	private Player owner;

	public Player GetOwner(){
		return owner;
	}

	public void SetOwner(Player player){
		owner = player;
	}

	public Card(Player owner,Sprite image){
		this.owner = owner;
		this.cardImage = image;
	}

	public abstract void activatePunishment();
}
