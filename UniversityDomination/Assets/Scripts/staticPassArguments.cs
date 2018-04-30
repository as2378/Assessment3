//===========================================================================
//Executable Link:
//https://as2378.github.io/unlucky/files/Assessment4/UniversityDomination.zip
//===========================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used to link the main menu scene to the game's scene.
 */ 
static public class staticPassArguments //contains static variable that is passed from main menu to game
{
    static public int humanPlayers; //number of human players selected
    static public bool loadGame; //indicates whether a game is loaded
}