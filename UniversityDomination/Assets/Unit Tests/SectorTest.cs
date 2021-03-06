﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class SectorTest 
{
	private Game game;
	private Map map;
    private Player[] players;
	private PlayerUI[] gui;

    [UnityTest]
    public IEnumerator SetOwner_SectorOwnerAndColorCorrect() {
        
        Setup();

        Sector sector = map.sectors[0];
        sector.SetOwner(null);
        Player player = players[0];

        sector.SetOwner(player);
        Assert.AreSame(sector.GetOwner(), player);
        Assert.IsTrue(sector.gameObject.GetComponent<Renderer>().material.color.Equals(player.GetColor()));

        sector.SetOwner(null);
        Assert.IsNull(sector.GetOwner());
        Assert.IsTrue(sector.gameObject.GetComponent<Renderer>().material.color.Equals(Color.gray));

        yield return null;
    }

    [UnityTest]
    public IEnumerator Initialize_OwnedAndNotOwnedSectorsOwnerAndColor() {
        
        Setup();

        Sector sectorWithoutLandmark = map.sectors[0];
        Sector sectorWithLandmark = map.sectors[1];

        sectorWithoutLandmark.Initialize();
        Assert.IsNull(sectorWithoutLandmark.GetOwner());
        Assert.IsTrue(sectorWithoutLandmark.gameObject.GetComponent<Renderer>().material.color.Equals(Color.gray));
        Assert.IsNull(sectorWithoutLandmark.GetUnit());
        Assert.IsNull(sectorWithoutLandmark.GetLandmark());

        sectorWithLandmark.Initialize();
        Assert.IsNull(sectorWithLandmark.GetOwner());
        Assert.IsTrue(sectorWithLandmark.gameObject.GetComponent<Renderer>().material.color.Equals(Color.gray));
        Assert.IsNull(sectorWithLandmark.GetUnit());
        Assert.IsNotNull(sectorWithLandmark.GetLandmark());

        yield return null;
    }

    [UnityTest]
    public IEnumerator Highlight_SectorColourCorrect() {
        
        Setup();

        Sector sector = map.sectors[0];
        sector.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        float amount = 0.2f;
        Color highlightedGray = Color.gray + (Color) (new Vector4(amount, amount, amount, 1));

        sector.ApplyHighlight(amount);
        Assert.IsTrue(sector.gameObject.GetComponent<Renderer>().material.color.Equals(highlightedGray));

        sector.RevertHighlight(amount);
        Assert.IsTrue(sector.gameObject.GetComponent<Renderer>().material.color.Equals(Color.gray));

        yield return null;
    }


    [UnityTest]
    public IEnumerator ClearUnit_UnitRemovedFromSector() {
        
        Setup();

        Sector sector = map.sectors[0];
        sector.SetUnit(null);

        sector.ClearUnit();
        Assert.IsNull(sector.GetUnit());

        sector.SetUnit(MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>());
        Assert.IsNotNull(sector.GetUnit());

        sector.ClearUnit();
        Assert.IsNull(sector.GetUnit());

        yield return null;
    }

    [UnityTest]
    public IEnumerator OnMouseAsButton_CorrectUnitIsSelected() {
        
        Setup();

        Sector sectorA = map.sectors[0];
        Sector sectorB = map.sectors[1];
        Sector sectorC = map.sectors[2];
        Player playerA = players[0];
        Player playerB = players[1];
        Unit unitA = MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>();
        Unit unitB = MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>(); // *should this be players[1]?* ###########################################################################################

        // ensure sectors A & B are adjacent to each other
        Assert.Contains(sectorA, sectorB.GetAdjacentSectors());
        Assert.Contains(sectorB, sectorA.GetAdjacentSectors());

        // ensure sectors A & C are not adjacent to each other
        foreach (Sector sector in sectorA.GetAdjacentSectors())
        {
            Assert.IsFalse(sector == sectorC);
        }
        foreach (Sector sector in sectorC.GetAdjacentSectors())
        {
            Assert.IsFalse(sector == sectorA);
        }

        sectorA.SetUnit(unitA);
        unitA.SetSector(sectorA);

        sectorA.SetOwner(playerA);
        unitA.SetOwner(playerA);
        unitB.SetOwner(playerB);

        playerA.units.Add(unitA);
        playerB.units.Add(unitB);


        // test clicking a sector with a unit while the unit's owner
        // is active AND there are no units selected
        playerA.SetActive(true);
        unitA.SetSelected(false);
        unitB.SetSelected(false);

        sectorA.OnMouseUpAsButtonAccessible();
        Assert.IsTrue(unitA.IsSelected());

        // test clicking on the sector containing the selected unit
        sectorA.OnMouseUpAsButtonAccessible();
        Assert.IsFalse(unitA.IsSelected());


        // test clicking a sector with a unit while there are no
        // units selected, but the unit's owner is NOT active
        playerA.SetActive(false);
        unitA.SetSelected(false);
        unitB.SetSelected(false);

        sectorA.OnMouseUpAsButtonAccessible();
        Assert.IsFalse(unitA.IsSelected());


        // test clicking a sector with a unit while the unit's owner
        // is active, but there IS another unit selected
        playerA.SetActive(true);
        unitA.SetSelected(false);
        unitB.SetSelected(true);

        sectorA.OnMouseUpAsButtonAccessible();
        Assert.IsFalse(unitA.IsSelected());


        // test clicking on a sector adjacent to a selected unit
        unitA.SetSelected(true);
        unitB.SetSelected(false);

        sectorB.OnMouseUpAsButtonAccessible();
        Assert.IsFalse(unitA.IsSelected());

        // only need to test deselection;
        // other interactions covered in smaller tests below

        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveIntoUnoccupiedSector_NewSectorHasUnitAndOldDoesNotAndTurnStateProgressed() {
        
        Setup();

        game.SetTurnState(Game.TurnState.Move1);
        Sector sectorA = map.sectors[0];
        Sector sectorB = map.sectors[1];

        sectorA.SetUnit(MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>());
        sectorA.GetUnit().SetSector(sectorA);
        sectorB.SetUnit(null);

        sectorB.MoveIntoUnoccupiedSector(sectorA.GetUnit());
        Assert.IsNotNull(sectorB.GetUnit());
        Assert.IsNull(sectorA.GetUnit());
        Assert.IsTrue(game.GetTurnState() == Game.TurnState.Move2);

        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveIntoFriendlyUnit_UnitsSwapSectorsAndTurnStateProgressed() {
        
        Setup();

        game.SetTurnState(Game.TurnState.Move1);
        Sector sectorA = map.sectors[0];
        Sector sectorB = map.sectors[1];

        sectorA.SetUnit(MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>());
        sectorA.GetUnit().SetSector(sectorA);
        sectorA.GetUnit().SetLevel(5);
        sectorA.GetUnit().SetOwner(players[0]);
        sectorA.SetOwner(players[0]);

        sectorB.SetUnit(MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>());
        sectorB.GetUnit().SetSector(sectorB);
        sectorB.GetUnit().SetLevel(1);
        sectorB.GetUnit().SetOwner(players[0]);
        sectorB.SetOwner(players[0]);

        sectorB.MoveIntoFriendlyUnit(sectorA.GetUnit());
        Assert.IsTrue(sectorA.GetUnit().GetLevel() == 1); // level 1 unit now in sectorA
        Assert.IsTrue(sectorB.GetUnit().GetLevel() == 5); // level 2 unit now in sectorB => units have swapped locations
        Assert.IsTrue(game.GetTurnState() == Game.TurnState.Move2);

        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveIntoHostileUnit_AttackingUnitTakesSectorAndLevelUpAndTurnEnd() {
        
        Setup();

        game.SetTurnState(Game.TurnState.Move1);
        Sector sectorA = map.sectors[0];
        Sector sectorB = map.sectors[1];

        // for all tests, sectorA's unit will be the attacking unit
        // and sectorB's unit will be the defending unit

        // setup units such that the attacking unit wins
        ResetSectors(sectorA, sectorB);
        sectorA.GetOwner().SetBeer(99); // to ensure the sectorA unit will win any conflict (attacking)
        sectorB.GetOwner().SetKnowledge(0);

        sectorB.MoveIntoHostileUnit(sectorA.GetUnit(), sectorB.GetUnit());
        Assert.IsNull(sectorA.GetUnit()); // attackingg unit moved out of sectorA
        Assert.IsTrue(sectorB.GetUnit().GetLevel() == 2); // attacking unit that moved to sectorB gained a level (the unit won the conflict)
        Assert.IsTrue(game.GetTurnState() == Game.TurnState.EndOfTurn);

        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveIntoHostileUnit_DefendingUnitDefendsSectorAndTurnEnd() {
        
        Setup();

        game.SetTurnState(Game.TurnState.Move1);
        Sector sectorA = map.sectors[0];
        Sector sectorB = map.sectors[1];

        // for all tests, sectorA's unit will be the attacking unit
        // and sectorB's unit will be the defending unit

        // setup units such that the defending unit wins
        game.SetTurnState(Game.TurnState.Move1);
        ResetSectors(sectorA, sectorB);
        sectorA.GetOwner().SetBeer(0);
        sectorB.GetOwner().SetKnowledge(99); //to ensure the sectorB unit will win any conflict (defending)

        sectorB.MoveIntoHostileUnit(sectorA.GetUnit(), sectorB.GetUnit());
        Assert.IsNull(sectorA.GetUnit()); // attacking unit destroyed
        Assert.IsTrue(sectorB.GetUnit().GetLevel() == 1); // defending unit did not gain a level following defence
        Assert.IsTrue(game.GetTurnState() == Game.TurnState.EndOfTurn);

        yield return null;
    }

    [UnityTest]
    public IEnumerator MoveIntoHostileUnit_TieConflict_DefendingUnitDefendsSectorAndTurnEnd() {
        
        Setup();

        game.SetTurnState(Game.TurnState.Move1);
        Sector sectorA = map.sectors[0];
        Sector sectorB = map.sectors[1];

        // for all tests, sectorA's unit will be the attacking unit
        // and sectorB's unit will be the defending unit

        // setup units such that there is a tie (defending unit wins)

        // *** UNITCONTROLLER DESTROYSELF METHOD NEEDS TO CLEAR UNIT ***

        game.SetTurnState(Game.TurnState.Move1);
        ResetSectors(sectorA, sectorB);
        sectorA.GetUnit().SetLevel(-4);
        sectorA.GetOwner().SetBeer(0);
        sectorB.GetUnit().SetLevel(-4);
        sectorB.GetOwner().SetKnowledge(0); // making both units equal

        sectorB.MoveIntoHostileUnit(sectorA.GetUnit(), sectorB.GetUnit());
        Assert.IsNull(sectorA.GetUnit()); // attacking unit destroyed
        Assert.IsTrue(sectorB.GetUnit().GetLevel() == -4); // defending unit did not gain a level following defence
        Assert.IsTrue(game.GetTurnState() == Game.TurnState.EndOfTurn);

        yield return null;
    }

    [UnityTest]
    public IEnumerator AdjacentSelectedUnit_SectorsAreAdjacent() {
        
        Setup();

        Sector sectorA = map.sectors[0];
        Sector sectorB = map.sectors[1];

        // ensure sectors A and B are adjacent to each other
        Assert.Contains(sectorA, sectorB.GetAdjacentSectors());
        Assert.Contains(sectorB, sectorA.GetAdjacentSectors());

        // test with no unit in adjacent sector
        Assert.IsNull(sectorB.AdjacentSelectedUnit());

        // test with unselected unit in adjacent sector
        sectorA.SetUnit(MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>());
        sectorA.GetUnit().SetSelected(false);
        Assert.IsNull(sectorB.AdjacentSelectedUnit());

        // test with selected unit in adjacent sectors
        sectorA.GetUnit().SetSelected(true);
        Assert.IsNotNull(sectorB.AdjacentSelectedUnit());

        yield return null;
    }

	/*
	 * ASSESSMENT4 ADDITION:
	 * Tests to ensure that the postgrad can move to an adjacent sector or to any sector owned by the player.
	 */
	[UnityTest]
	public IEnumerator OnMouseUpAsButton_PostgradMovesToAdjacentSector(){
		//Tests that postgrads can move to adjacent sectors, like other units can.
		Setup ();

		Sector sector1 = map.sectors [0];
		Sector adjacentSector = sector1.GetAdjacentSectors () [0];
		Player testPlayer = players [0];
		Unit testUnit = MonoBehaviour.Instantiate(testPlayer.GetUnitPrefab()).GetComponent<Unit>();
		testUnit.SetOwner (testPlayer);
		testUnit.MoveTo (sector1);
		testUnit.SetLevel (5); //Make testUnit a postgrad.

		testUnit.SetSelected (true); //Select the unit.

		adjacentSector.OnMouseUpAsButtonAccessible (); //Click the adjacent sector.

		//Test that the postgrad has moved to adjacentSector.
		Assert.AreSame (testUnit, adjacentSector.GetUnit ());
		Assert.AreSame (testPlayer, adjacentSector.GetOwner ());
		Assert.IsNull (sector1.GetUnit ());

		yield return null;
	}

	/*
	 * ASSESSMENT4 ADDITION:
	 * Tests to ensure that the postgrad can move to an adjacent sector or to any sector owned by the player.
	 */
	[UnityTest]
	public IEnumerator OnMouseUpAsButton_PostgradMovesToPlayerOwnedSector(){
		//Tests that postgrads can move to any player-owned sectors.
		Setup ();

		Sector sector1 = map.sectors [0];
		Sector sector2 = map.sectors [3]; //a sector that is not adjacent to sector1.

		List<Sector> adjSectors = new List<Sector> (sector1.GetAdjacentSectors ());  
		Assert.IsFalse (adjSectors.Contains (sector2));	//ensure sector2 is not adjacent to sector1.

		Player testPlayer = players [0];
		Unit testUnit = MonoBehaviour.Instantiate(testPlayer.GetUnitPrefab()).GetComponent<Unit>();
		sector2.SetOwner (testPlayer);

		testUnit.SetOwner (testPlayer);
		testUnit.MoveTo (sector1);
		testUnit.SetLevel (5); //Make testUnit a postgrad.

		testUnit.SetSelected (true); //Select the unit.

		sector2.OnMouseUpAsButtonAccessible (); //Click the other player-owned sector.

		//Test that the postgrad has moved to sector2.
		Assert.AreSame (testUnit, sector2.GetUnit ());
		Assert.AreSame (testPlayer, sector2.GetOwner ());
		Assert.IsNull (sector1.GetUnit ());

		yield return null;
	}

	/*
	 * ASSESSMENT4 ADDITION:
	 * Tests to ensure that the postgrad can move to an adjacent sector or to any sector owned by the player.
	 */
	[UnityTest]
	public IEnumerator OnMouseUpAsButton_PostgradCannotMoveToNonAdjacentEnemySector(){
		//Tests that postgrads cannot move to non-adjacent sectors that are owned by enemy players.
		Setup ();

		Player testPlayer = players [0];
		Player enemyPlayer = players [1];

		Unit testUnit = MonoBehaviour.Instantiate(testPlayer.GetUnitPrefab()).GetComponent<Unit>();
		testUnit.SetOwner (testPlayer);

		Sector sector1 = map.sectors [0];
		testUnit.MoveTo (sector1);
		Sector sector2 = map.sectors [3]; //a sector that is not adjacent to sector1.
		sector2.SetOwner (enemyPlayer);

		List<Sector> adjSectors = new List<Sector> (sector1.GetAdjacentSectors ());  
		Assert.IsFalse (adjSectors.Contains (sector2));	//ensure sector2 is not adjacent to sector1.

		testUnit.SetLevel (5); //Make testUnit a postgrad.
		testUnit.SetSelected (true); //Select the unit.

		sector2.OnMouseUpAsButtonAccessible (); //Click the enemy-owned sector.

		//Test that the postgrad has not moved to sector2.
		Assert.AreSame (testUnit, sector1.GetUnit ());
		Assert.AreSame (testPlayer, sector1.GetOwner ());
		Assert.IsNull (sector2.GetUnit ());
		Assert.AreSame (enemyPlayer, sector2.GetOwner ());

		yield return null;
	}


	/*
	 * ASSESSMENT4 ADDITION: added code which creates a PVC unit, places it within the scene and
	 * links it to the game class.
	 */
    private void Setup() {
        
        // initialize the game, map, and players with any references needed
        // the "GameManager" asset contains a copy of the GameManager object
        // in the 4x4 Test, but its script lacks references to players & the map
        game = MonoBehaviour.Instantiate(Resources.Load<GameObject>("GameManager")).GetComponent<Game>();

        // the "Map" asset is a copy of the 4x4 Test map, complete with
        // adjacent sectors and landmarks at (0,1), (1,3), (2,0), and (3,2),
        // but its script lacks references to the game & sectors
        map = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Map")).GetComponent<Map>();

        // the "Players" asset contains 4 prefab Player game objects; only
        // references not in its script is each player's color
        players = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Players")).GetComponentsInChildren<Player>();

		// the "GUI" asset contains the PlayerUI object for each Player
		gui = MonoBehaviour.Instantiate(Resources.Load<GameObject>("GUI")).GetComponentsInChildren<PlayerUI>();

		// the "Scenery" asset contains the camera and light source of the 4x4 Test
		// can uncomment to view scene as tests run, but significantly reduces speed
		//MonoBehaviour.Instantiate(Resources.Load<GameObject>("Scenery"));

        // establish references from game to players & map
        game.players = players;
        game.gameMap = map.gameObject;
        game.EnableTestMode();

        // establish references from map to game & sectors (from children)
        map.game = game;
        map.sectors = map.gameObject.GetComponentsInChildren<Sector>();

        // establish references from each player to the game
        foreach (Player player in players)
        {
            player.SetGame(game);
        }

        // establish references to SSB 64 colors for each player
        players[0].SetColor(Color.red);
        players[1].SetColor(Color.blue);
        players[2].SetColor(Color.yellow);
        players[3].SetColor(Color.green);

		// establish references to a PlayerUI and Game for each player & initialize GUI
		for (int i = 0; i < players.Length; i++) 
		{
			players[i].SetGui(gui[i]);
			players[i].SetGame(game);
			players[i].GetGui().Initialize(players[i], i + 1);
			players [i].SetHuman (true);
		}

		// enable game's test mode
		game.EnableTestMode();

		//ASSESSMENT4 ADDITION:
		GameObject winScreen = new GameObject();
		winScreen.AddComponent<UnityEngine.UI.Text> ();
		winScreen.name = "winScreen";
		game.winnerScreen = winScreen;

		GameObject pvc = new GameObject ();
		pvc.AddComponent<Landmark> ();
		pvc.AddComponent<MeshRenderer> ();
		pvc.AddComponent<MeshCollider> ();
		pvc.name = "PVC";
		game.viceChancellorGameObj = pvc;

		CardDeck cardDeck = MonoBehaviour.Instantiate (Resources.Load<GameObject> ("PunishmentCardGUI")).GetComponent<CardDeck> ();
		game.cardDeck = cardDeck;
		cardDeck.game = game;
    }

    private void ResetSectors(Sector sectorA, Sector sectorB) {
        
        // re-initialize sectors for in between test cases in MoveIntoHostileUnitTest

        sectorA.SetUnit(MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>());
        sectorA.GetUnit().SetSector(sectorA);
        sectorA.GetUnit().SetOwner(players[0]);
        sectorA.SetOwner(players[0]);
        sectorA.GetUnit().SetLevel(1);

        sectorB.SetUnit(MonoBehaviour.Instantiate(players[0].GetUnitPrefab()).GetComponent<Unit>());
        sectorB.GetUnit().SetSector(sectorB);
        sectorB.GetUnit().SetOwner(players[1]);
        sectorB.SetOwner(players[1]);
        sectorB.GetUnit().SetLevel(1);
    }

	/*
	 * ASSESSMENT4 ADDITION: added this teardown method to clear the scene of all the gameobjects.
	 * This means that the tests do not keep adding gameobjects to the scene, which slowed them down.
	 */
	[TearDown] 
	public void ClearSceneAfterTest(){
		GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
		foreach (GameObject gameObject in objects)
		{
			GameObject.Destroy (gameObject);
		}
	}
}