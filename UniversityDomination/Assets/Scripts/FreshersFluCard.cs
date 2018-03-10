using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreshersFluCard : Card {
	public FreshersFluCard (Player owner) : base (owner, Resources.Load<Sprite> ("cards/FreshersFlu"), 2){
		return;
	}

	public override void activatePunishment(){
		Map map = GetOwner ().GetGame ().gameMap.GetComponent<Map>();
		foreach (Sector sector in map.sectors) 
		{
			Unit unit = sector.GetUnit ();
			if (unit != null && sector.GetOwner () != this.GetOwner ()) 
			{
				unit.SetColor(new Color (0.62f, 0.71f, 0.47f));
				unit.gameObject.GetComponent<Renderer> ().material.color = unit.GetColor ();
			}
		}
		foreach (Player player in map.game.players) 
		{
			if (player != this.GetOwner ()) 
			{
				player.SetBeer (0);
				player.SetKnowledge (0);
			}
		}
		Debug.Log ("Flu activated!");

	}

	public override void deactivatePunishment(){
		Map map = GetOwner ().GetGame ().gameMap.GetComponent<Map>();
		foreach (Sector sector in map.sectors) 
		{
			Unit unit = sector.GetUnit ();
			if (unit != null && sector.GetOwner () != this.GetOwner ()) 
			{
				unit.SetColor(Color.white);
				unit.gameObject.GetComponent<Renderer> ().material.color = unit.GetColor ();
			}
		}
		this.SetOwner (null);
	}
}