using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreshersFluCard : Card {
	private Dictionary<Player,int[]> playerPvcBonuses = new Dictionary<Player, int[]> ();

	public FreshersFluCard (Player owner) : base (owner, Resources.Load<Sprite> ("cards/FreshersFlu"), 2){
		return;
	}

	public override void activatePunishment(){
		//This method is called when the card is activated. It first colours all enemy units green, then calculates the amount of PVC bonus
		//earnt by each player. The enemy player's bonuses are then reset.

		Map map = GetOwner ().GetGame ().gameMap.GetComponent<Map>();
		//Colour the units.
		foreach (Sector sector in map.sectors) 
		{
			Unit unit = sector.GetUnit ();
			if (unit != null && sector.GetOwner () != this.GetOwner ()) 
			{
				unit.SetColor(new Color (0.62f, 0.71f, 0.47f));
				unit.gameObject.GetComponent<Renderer> ().material.color = unit.GetColor ();
			}
		}


		//Calculate the amount of PVC bonus each of the players own and add it to the playerPvcBonuses list.
		foreach (Player player in map.game.players) 
		{
			if (player != this.GetOwner ()) 
			{
				int[] bonuses = new int[2];
				bonuses [0] = player.GetBeer ();
				bonuses [1] = player.GetKnowledge ();

				foreach (Sector sector in player.ownedSectors)
				{
					Landmark landmark = sector.GetLandmark ();
					if (landmark != null) 
					{
						if (landmark.GetResourceType () == Landmark.ResourceType.Beer) 
						{
							bonuses [0] = bonuses [0] - landmark.GetAmount ();
						} 
						else if (landmark.GetResourceType () == Landmark.ResourceType.Knowledge) 
						{
							bonuses [1] = bonuses [1] - landmark.GetAmount ();
						}
					}
				}
				playerPvcBonuses [player] = bonuses;

				// Reset the players bonuses
				player.SetBeer (0);
				player.SetKnowledge (0);
			}
		}
		Debug.Log ("Flu activated!");
	}

	public override void deactivatePunishment(){
		Map map = GetOwner ().GetGame ().gameMap.GetComponent<Map>();

		//Deactivates the card's visual effect.
		foreach (Sector sector in map.sectors) 
		{
			Unit unit = sector.GetUnit ();
			if (unit != null && sector.GetOwner () != this.GetOwner ()) 
			{
				unit.SetColor(Color.white);
				unit.gameObject.GetComponent<Renderer> ().material.color = unit.GetColor ();
			}
		}
		//Returns the bonuses back to normal by recalculating how much bonus they should have.
		foreach (Player player in playerPvcBonuses.Keys) 
		{
			//Reset the player's bonuses => any PVC bonus gained when the card is active is lost.
			player.SetBeer (0);
			player.SetKnowledge (0);

			int[] bonuses = playerPvcBonuses [player];
			foreach (Sector sector in player.ownedSectors) 
			{
				Landmark landmark = sector.GetLandmark ();
				if (landmark != null) 
				{
					if (landmark.GetResourceType () == Landmark.ResourceType.Beer) 
					{
						bonuses [0] += landmark.GetAmount ();
					} 
					else if (landmark.GetResourceType () == Landmark.ResourceType.Knowledge)
					{
						bonuses [1] += landmark.GetAmount ();
					}
				}
			}
			// Set the players bonuses back to normal.
			player.SetBeer(player.GetBeer() + bonuses[0]);
			player.SetKnowledge (player.GetKnowledge () + bonuses [1]);
		}
		this.SetOwner (null);
	}
}