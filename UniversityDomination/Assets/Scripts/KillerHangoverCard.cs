using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillerHangoverCard : Card {

    public KillerHangoverCard (Player owner) :base (owner, Resources.Load<Sprite>("cards/KillerHangover"), 2) {
        return;
    }

	public override void activatePunishment () {
        //This method is called when the card is activated.
        Debug.Log("Killer Hangover activated");
    }

    public override void deactivatePunishment() {
        Debug.Log("Killer Hangover deactivated");
        this.SetOwner(null);
    }
}
