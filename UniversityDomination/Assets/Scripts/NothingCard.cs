//===========================================================================
//Executable Link:
//https://as2378.github.io/unlucky/files/Assessment4/UniversityDomination.zip
//===========================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ASSESSMENT4 ADDITION:
 * This class is used to represent the Nothing Card.
 */ 
public class NothingCard : Card {

	public NothingCard (Player owner) : base (owner, Resources.Load<Sprite> ("cards/Nothing"), 1){
		return;
	}

	//This method has to be included because activatePunishment in Card is abstract.
	public override void activatePunishment(){
		Debug.Log ("DOING NOTHING");
	}
}
