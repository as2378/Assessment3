using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LecturerStrikeCard : Card
{

    public Game game;

    public LecturerStrikeCard(Player owner) : base(owner, Resources.Load<Sprite>("cards/Strike"), 3){
        return;

    }

    public override void activatePunishment()
    {
        //Calls for the end of the current players turn so they cannot continue with their go due to the 'strike'.

        Debug.Log("Strike Activated!");
    }

}
