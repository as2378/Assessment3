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
 * This class is used to represent the strike punishment cards. When activated, enemy players can only move once per turn.
 */
public class LecturerStrikeCard : Card
{
    public LecturerStrikeCard(Player owner) : base(owner, Resources.Load<Sprite>("cards/Strike"), 3){
        return;
    }

    public LecturerStrikeCard(Player owner, int turnCount) : base (owner, Resources.Load<Sprite>("cards/Strike"), turnCount) {
        return;
    }

    public override void activatePunishment()
    {
        //When this card is active, enemy players will only be able to move once per turn.

		//Load in the strike scene and place it into the game.
		GameObject strikeScene = GameObject.Instantiate (Resources.Load<GameObject> ("strike_model/StrikePeople"));
		strikeScene.name = "StrikePeople";

        Debug.Log("Strike Activated!");
    }

	public override void deactivatePunishment() {
		Debug.Log("Strike deactivated");

		//Remove the strike scene from the game.
		GameObject strikeScene = GameObject.Find ("StrikePeople");
		if (strikeScene != null) 
		{
			GameObject.Destroy (strikeScene);
		}

		this.SetOwner(null);
	}
}
