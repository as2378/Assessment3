//===========================================================================
//Executable Link:
//https://as2378.github.io/unlucky/files/Assessment4/UniversityDomination.zip
//===========================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ASSESSMENT4 ADDITION
 * This class is used to represent the Killer hangover punishment cards.
 */
public class KillerHangoverCard : Card {

    public KillerHangoverCard (Player owner) :base (owner, Resources.Load<Sprite>("cards/KillerHangover"), 2) {
        return;
    }

    public KillerHangoverCard(Player owner, int turnCount) : base(owner, Resources.Load<Sprite>("cards/KillerHangover"), turnCount)
    {
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
