//===========================================================================
//Executable Link:
//https://as2378.github.io/unlucky/files/Assessment4/UniversityDomination.zip
//===========================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This class is used to add functionality to the volume slider in the options menu.
 */ 
public class VolumeChanger : MonoBehaviour {   
	// Update is called once per frame
	void Update () {
        AudioListener.volume = this.gameObject.GetComponent<Slider>().value; // set music volume to value of slider
	}
}
