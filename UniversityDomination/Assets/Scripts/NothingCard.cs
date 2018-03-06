using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NothingCard : Card {

	public NothingCard (Player owner) : base (owner, Resources.Load<Sprite> ("cards/Nothing")){
		return;
	}

	public override void activatePunishment(){
		Debug.Log ("DOING NOTHING");
	}
}
