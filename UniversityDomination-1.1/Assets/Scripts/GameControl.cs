using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

<<<<<<< HEAD
public class GameControl : MonoBehaviour {
	
	//Can easily access this object with GameControl.control.FIELD/METHOD
	public static GameControl control;
    public Game game;
    public GameObject map;
    public GameObject unitPrefab;
    public enum TurnState { Move1, Move2, EndOfTurn, NULL };
    public enum ResourceType { Beer, Knowledge, ViceChancellor };
    private Material level1Material;
    private Material level2Material;
    private Material level3Material;
    private Material level4Material;
    private Material level5Material;

    void Awake () 
	{
		control = this;
	}
	
	public void Save()
	{
		Debug.Log("Entered Save Function");
=======
public class GameControl : MonoBehaviour
{

    //Can easily access this object with GameControl.control.FIELD/METHOD
    public static GameControl control;
    public Game game;
    public GameObject map;
    public enum TurnState { Move1, Move2, EndOfTurn, NULL };
    public enum ResourceType { Beer, Knowledge, ViceChancellor };
    public GameObject viceChancellor;
    public GameObject player1UnitPrefab;
    public GameObject player2UnitPrefab;
    public GameObject player3UnitPrefab;
    public GameObject player4UnitPrefab;

    void Awake()
    {
        control = this;
    }

    public void Save()
    {
        Debug.Log("Entered Save Function");
>>>>>>> origin/MenuMerge
        //Delete previous save data if exists
        if (File.Exists(Application.persistentDataPath + "/gameInformation.dat"))
        {
            File.Delete(Application.persistentDataPath + "/gameInformation.dat");
        }
<<<<<<< HEAD
        
        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/gameInformation.dat", true))
        {
            //write game state
            writer.WriteLine("Current Player: " + game.currentPlayer.name);
            writer.WriteLine("Turn state: " + game.GetTurnState());
=======

        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/gameInformation.dat", true))
        {
            Debug.Log(Application.persistentDataPath);

            //write game state
            writer.WriteLine("Current Player:" + game.currentPlayer.name);
            writer.WriteLine("Turn state:" + game.GetTurnState());
>>>>>>> origin/MenuMerge

            //write player info
            foreach (Player player in game.players)
            {
<<<<<<< HEAD
                writer.WriteLine(player.GetBeer());
                writer.WriteLine(player.GetKnowledge());
                writer.WriteLine(player.IsHuman());
                writer.WriteLine(player.IsActive());
=======
                writer.WriteLine( player.name + "Beer:" + player.GetBeer());
                writer.WriteLine( player.name + "Knowledge:" + player.GetKnowledge());
                writer.WriteLine( player.name + "IsHuman:" + player.IsHuman());
                writer.WriteLine( player.name + "IsActive:" + player.IsActive());
>>>>>>> origin/MenuMerge
            }

            //get array of sectors
            Sector[] sectors = map.GetComponentsInChildren<Sector>();

            foreach (Sector sector in sectors)
            {
                //write sector name and owner name
<<<<<<< HEAD
                writer.WriteLine(sector.name);
                if (sector.GetOwner() == null)
                {
                    writer.WriteLine("null");
                } else
                {
                    writer.WriteLine(sector.GetOwner().name);
                }
                
                //write landmark info
                if (sector.GetLandmark() == null)
                {
                    writer.WriteLine("null");
                } else {
                    writer.WriteLine(sector.GetLandmark().GetResourceType());
                    writer.WriteLine(sector.GetLandmark().GetAmount());
                }
                
                //write unit info
                if (sector.GetUnit() == null)
                {
                    writer.WriteLine("null");
                } else {
                    writer.WriteLine(sector.GetUnit().GetOwner().name);
                    writer.WriteLine(sector.GetUnit().GetLevel());
                    writer.WriteLine(sector.GetUnit().IsSelected());
                }
            }
        }
		Debug.Log("Saved!");
        
	}
	
	public void Load()
	{
        //GameObject.Find("Player1");
        //this.gameObject.GetComponentInChildren

=======
                writer.WriteLine(sector.name + ":");
                if (sector.GetOwner() == null)
                {
                    writer.WriteLine("owner:null");
                }
                else
                {
                    writer.WriteLine("owner:" + sector.GetOwner().name);
                }

                //write viceChance info
                if (sector.GetLandmark() == null)
                {
                    writer.WriteLine("viceChance:null");
                }
                else
                {
                    if (sector.GetLandmark().GetResourceType() == Landmark.ResourceType.ViceChancellor)
                    {
                        writer.WriteLine("viceChance:" + sector.GetLandmark().GetResourceType());
                    }
                    else
                    {
                        writer.WriteLine("viceChance:"+ sector.GetLandmark().GetResourceType().ToString());
                    }
                }

                //write unit info
                if (sector.GetUnit() == null)
                {
                    writer.WriteLine("unit:null");
                }
                else
                {
                    writer.WriteLine("unitOwner:"+sector.GetUnit().GetOwner().name+":"+sector.GetUnit().GetLevel());
                }
            }
        }
        Debug.Log("Saved!");

    }

    public void Load()
    {
>>>>>>> origin/MenuMerge
        Debug.Log("Entered Load");

        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/gameInformation.dat"))
        {
<<<<<<< HEAD
            string line = reader.ReadLine();
            //restore current player
            if (line == "Current Player: Player1") {
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find("Player1").GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            } else if (line == "Current Player: Player2") {
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find("Player2").GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            } else if (line == "Current Player: Player3") {
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find("Player3").GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            } else if (line == "Current Player: Player4") {
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find("Player4").GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            }

            //restore movestate
            line = reader.ReadLine();
            if (line == "Move1") {
                game.SetTurnState(Game.TurnState.Move1);
            } else if (line == "Move2") {
                game.SetTurnState(Game.TurnState.Move2);
            } else if (line == "EndOfTurn") {
                game.SetTurnState(Game.TurnState.EndOfTurn);
            } else if (line == "NULL") {
                game.SetTurnState(Game.TurnState.NULL);
            }

            //restore player1
            game.players[0].SetBeer(int.Parse(reader.ReadLine()));
            game.players[0].SetKnowledge(int.Parse(reader.ReadLine()));
            game.players[0].SetHuman(bool.Parse(reader.ReadLine()));
            game.players[0].SetActive(bool.Parse(reader.ReadLine()));
            //restore player2
            game.players[1].SetBeer(int.Parse(reader.ReadLine()));
            game.players[1].SetKnowledge(int.Parse(reader.ReadLine()));
            game.players[1].SetHuman(bool.Parse(reader.ReadLine()));
            game.players[1].SetActive(bool.Parse(reader.ReadLine()));
            //restore player3
            game.players[2].SetBeer(int.Parse(reader.ReadLine()));
            game.players[2].SetKnowledge(int.Parse(reader.ReadLine()));
            game.players[2].SetHuman(bool.Parse(reader.ReadLine()));
            game.players[2].SetActive(bool.Parse(reader.ReadLine()));
            //restore player4
            game.players[3].SetBeer(int.Parse(reader.ReadLine()));
            game.players[3].SetKnowledge(int.Parse(reader.ReadLine()));
            game.players[3].SetHuman(bool.Parse(reader.ReadLine()));
            game.players[3].SetActive(bool.Parse(reader.ReadLine()));

            //sectors
            Sector[] sectors = map.GetComponentsInChildren<Sector>();

            //sector 1
            //sector 1 owner
            line = reader.ReadLine();
            line = reader.ReadLine();
            if (line == "Player1") {
                sectors[0].SetOwner(game.players[0]);
            } else if (line == "Player2") {
                sectors[0].SetOwner(game.players[1]);
            } else if (line == "Player3") {
                sectors[0].SetOwner(game.players[2]);
            } else if (line == "Player4") {
                sectors[0].SetOwner(game.players[3]);
            } else if (line == "null")
            {
                sectors[0].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor") {
                sectors[0].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[0].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_02")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                } else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                } else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                } else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                } else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }
                
                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                } else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                } else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                } else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[0].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 2
            //sector 2 owner
            line = reader.ReadLine();
            Debug.Log("Start sector 2 part: " + line);
            if (line == "Player1")
            {
                sectors[1].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[1].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[1].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[1].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[1].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            Debug.Log("Start sector 2 part2: " + line);
            if (line == "ViceChancellor")
            {
                sectors[1].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[1].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            Debug.Log("Start sector 2 part3: " + line);
            line = reader.ReadLine();
            Debug.Log("Start sector 2 part4: " + line);

            //check if unit in sector
            if (line != "sector_03")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[1].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 3
            //sector 3 owner
            Debug.Log("Entered sector 3 section");
            Debug.Log("Start sector 3 part: " + line);
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[2].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[2].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[2].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[2].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[2].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[2].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[2].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_04")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[2].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 4
            //sector 4 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[3].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[3].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[3].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[3].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[3].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[3].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[3].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_05")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[3].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 5
            //sector 5 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[4].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[4].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[4].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[4].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[4].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[4].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[4].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_06")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[4].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 6
            //sector 6 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[5].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[5].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[5].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[5].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[5].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[5].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[5].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_07")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[5].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 7
            //sector 7 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[6].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[6].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[6].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[6].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[6].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[6].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[6].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_08")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[6].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 8
            //sector 8 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[7].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[7].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[7].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[7].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[7].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[7].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[7].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_09")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[7].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 9
            //sector 9 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[8].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[8].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[8].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[8].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[8].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[8].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[8].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_10")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[8].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 10
            //sector 10 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[9].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[9].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[9].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[9].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[9].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[5].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[5].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_11")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[9].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 11
            //sector 11 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[10].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[10].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[10].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[10].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[10].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[10].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[10].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_12")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[10].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 12
            //sector 12 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[11].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[11].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[11].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[11].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[11].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[11].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[11].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_13")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[11].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 13
            //sector 13 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[12].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[12].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[12].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[12].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[12].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[12].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[12].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_14")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[12].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 14
            //sector 14 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[13].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[13].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[13].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[13].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[13].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[13].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[13].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_15")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[13].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 15
            //sector 15 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[14].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[14].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[14].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[14].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[14].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[14].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[14].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_16")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[14].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 16
            //sector 16 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[15].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[15].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[15].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[15].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[15].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[15].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[15].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_17")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[15].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 17
            //sector 17 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[16].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[16].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[16].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[16].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[16].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[16].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[16].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_18")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[16].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 18
            //sector 18 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[17].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[17].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[17].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[17].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[17].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[17].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[17].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_19")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[17].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 19
            //sector 19 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[18].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[18].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[18].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[18].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[18].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[18].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[18].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_20")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[18].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 20
            //sector 20 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[19].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[19].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[19].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[19].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[19].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[19].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[19].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_21")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[19].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 21
            //sector 21 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[20].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[20].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[20].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[20].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[20].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[20].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[20].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_22")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[20].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 22
            //sector 22 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[21].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[21].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[21].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[21].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[21].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[21].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[21].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_23")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[21].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 23
            //sector 23 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[22].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[22].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[22].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[22].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[22].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[22].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[22].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_24")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[22].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 24
            //sector 24 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[23].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[23].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[23].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[23].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[23].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[23].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[23].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_25")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[23].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 25
            //sector 25 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[24].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[24].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[24].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[24].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[24].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[24].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[24].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_26")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[24].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 26
            //sector 26 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[25].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[25].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[25].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[25].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[25].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[25].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[25].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_27")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[25].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 27
            //sector 27 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[26].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[26].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[26].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[26].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[26].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[26].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[26].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_28")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[26].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 28
            //sector 28 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[27].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[27].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[27].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[27].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[27].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[27].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[27].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_29")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[27].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 29
            //sector 29 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[28].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[28].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[28].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[28].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[28].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[28].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[28].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_30")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[28].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 30
            //sector 30 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[29].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[29].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[29].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[29].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[29].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[29].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[29].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_31")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[29].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 31
            //sector 31 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[30].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[30].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[30].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[30].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[30].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[30].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[30].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != "sector_32")
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[30].SetUnit(newUnit);
                line = reader.ReadLine();
            }

            //sector 32
            //sector 32 owner
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[31].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[31].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[31].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[31].SetOwner(game.players[3]);
            }
            else if (line == "null")
            {
                sectors[31].SetOwner(null);
            }

            //if Vice setup
            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                sectors[31].SetLandmark(game.viceChancellorGameObj.GetComponent<Landmark>());
                sectors[31].GetLandmark().SetResourceType(Landmark.ResourceType.ViceChancellor);
            }
            line = reader.ReadLine();
            line = reader.ReadLine();

            //check if unit in sector
            if (line != null)
            {
                //there is unit in sector
                String unitOwner = line;
                int unitlevel = int.Parse(reader.ReadLine());
                bool unitIsSelected = bool.Parse(reader.ReadLine());
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                if (unitlevel == 1)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level1Material;
                }
                else if (unitlevel == 2)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level2Material;
                }
                else if (unitlevel == 3)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level3Material;
                }
                else if (unitlevel == 4)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level4Material;
                }
                else if (unitlevel == 5)
                {
                    newUnit.GetComponent<MeshRenderer>().material = level5Material;
                }

                if (unitOwner == "Player1")
                {
                    newUnit.SetOwner(game.players[0]);
                    game.players[0].units.Add(newUnit);
                }
                else if (unitOwner == "Player2")
                {
                    newUnit.SetOwner(game.players[1]);
                    game.players[1].units.Add(newUnit);
                }
                else if (unitOwner == "Player3")
                {
                    newUnit.SetOwner(game.players[2]);
                    game.players[2].units.Add(newUnit);
                }
                else if (unitOwner == "Player4")
                {
                    newUnit.SetOwner(game.players[3]);
                    game.players[3].units.Add(newUnit);
                }

                newUnit.level = unitlevel;
                newUnit.SetSelected(unitIsSelected);
                newUnit.SetSector(sectors[0]);
                sectors[31].SetUnit(newUnit);
                line = reader.ReadLine();
            }
        }

        Debug.Log("Loaded!");
	}
=======
            string[] line = reader.ReadLine().Split(':');

            //restore current player
            if (line[0] == "Current Player")
            {
                if (line[1] == "Player1")
                {
                    DeactiveCurrentPlayer();
                    //load in player saved
                    game.currentPlayer = GameObject.Find("Player1").GetComponent<Player>();
                    game.currentPlayer.SetActive(true);
                    game.currentPlayer.GetGui().Activate();
                }
                else if (line[1] == "Player2")
                {
                    DeactiveCurrentPlayer();
                    //load in player saved
                    game.currentPlayer = GameObject.Find("Player2").GetComponent<Player>();
                    game.currentPlayer.SetActive(true);
                    game.currentPlayer.GetGui().Activate();
                }
                else if (line[1] == "Player3")
                {
                    DeactiveCurrentPlayer();
                    //load in player saved
                    game.currentPlayer = GameObject.Find("Player3").GetComponent<Player>();
                    game.currentPlayer.SetActive(true);
                    game.currentPlayer.GetGui().Activate();
                }
                else if (line[1] == "Player4")
                {
                    DeactiveCurrentPlayer();
                    //load in player saved
                    game.currentPlayer = GameObject.Find("Player4").GetComponent<Player>();
                    game.currentPlayer.SetActive(true);
                    game.currentPlayer.GetGui().Activate();
                }
            }

            //restore movestate
            line = reader.ReadLine().Split(':');

            if (line[0] == "Turn state")
            {
                if (line[1] == "Move1")
                {
                    game.SetTurnState(Game.TurnState.Move1);
                }
                else if (line[1] == "Move2")
                {
                    game.SetTurnState(Game.TurnState.Move2);
                }
                else if (line[1] == "EndOfTurn")
                {
                    game.SetTurnState(Game.TurnState.EndOfTurn);
                }
                else if (line[1] == "NULL")
                {
                    game.SetTurnState(Game.TurnState.NULL);
                }
            }

            //restore player1
            game.players[0].SetBeer( int.Parse(reader.ReadLine().Split(':')[1]) );
            game.players[0].SetKnowledge(int.Parse(reader.ReadLine().Split(':')[1]));
            game.players[0].SetHuman(bool.Parse(reader.ReadLine().Split(':')[1]));
            game.players[0].SetActive(bool.Parse(reader.ReadLine().Split(':')[1]));
            //restore player2
            game.players[1].SetBeer(int.Parse(reader.ReadLine().Split(':')[1]));
            game.players[1].SetKnowledge(int.Parse(reader.ReadLine().Split(':')[1]));
            game.players[1].SetHuman(bool.Parse(reader.ReadLine().Split(':')[1]));
            game.players[1].SetActive(bool.Parse(reader.ReadLine().Split(':')[1]));
            //restore player3
            game.players[2].SetBeer(int.Parse(reader.ReadLine().Split(':')[1]));
            game.players[2].SetKnowledge(int.Parse(reader.ReadLine().Split(':')[1]));
            game.players[2].SetHuman(bool.Parse(reader.ReadLine().Split(':')[1]));
            game.players[2].SetActive(bool.Parse(reader.ReadLine().Split(':')[1]));
            //restore player4
            game.players[3].SetBeer(int.Parse(reader.ReadLine().Split(':')[1]));
            game.players[3].SetKnowledge(int.Parse(reader.ReadLine().Split(':')[1]));
            game.players[3].SetHuman(bool.Parse(reader.ReadLine().Split(':')[1]));
            game.players[3].SetActive(bool.Parse(reader.ReadLine().Split(':')[1]));

            //sectors
            Sector[] sectors = map.GetComponentsInChildren<Sector>();

            for (int i = 0; i < 32; i ++)
            {
                string sectorName = reader.ReadLine().Split(':')[0];

                Debug.Log(sectorName);

                //set sector owner
                line = reader.ReadLine().Split(':');

                string playerName = line[1];

                if (playerName == "Player1")
                {
                    sectors[i].SetOwner(game.players[0]);
                    game.players[0].ownedSectors.Add(sectors[i]);
                }
                else if (playerName == "Player2")
                {
                    sectors[i].SetOwner(game.players[1]);
                    game.players[1].ownedSectors.Add(sectors[i]);
                }
                else if (playerName == "Player3")
                {
                    sectors[i].SetOwner(game.players[2]);
                    game.players[2].ownedSectors.Add(sectors[i]);
                }
                else if (playerName == "Player4")
                {
                    sectors[i].SetOwner(game.players[3]);
                    game.players[3].ownedSectors.Add(sectors[i]);
                }
                else
                {
                    sectors[i].SetOwner(null);
                }

                //capture landmarks (and place vice chancellor if present)
                line = reader.ReadLine().Split(':');

                if (line[1] == "ViceChancellor")
                {
                    game.spawnVice(i);
                }
                else if (line[1] != "null")
                {
                    if (playerName == "Player1")
                    {
                        game.players[0].Capture(sectors[i]);
                    }
                    else if (playerName == "Player2")
                    {
                        game.players[1].Capture(sectors[i]);
                    }
                    else if (playerName == "Player3")
                    {
                        game.players[2].Capture(sectors[i]);
                    }
                    else if (playerName == "Player4")
                    {
                        game.players[3].Capture(sectors[i]);
                    }
                }

                //spawn units (if present
                //line = reader.ReadLine().Split();
                string aay = reader.ReadLine();


                line = aay.Split(':');

                if (line[0] != "null") //if there is a unit to place
                {
                    if (line[1] == "Player1")
                    {
                        // instantiate a new unit at the sector
                        Unit newUnit = Instantiate(player1UnitPrefab).GetComponent<Unit>();

                        // initialize the new unit
                        newUnit.Initialize(game.players[0], sectors[i]);

                        // add the new unit to the player's list of units and 
                        // the sector's unit parameters
                        game.players[0].units.Add(newUnit);
                        //sectors[i].SetUnit(newUnit);

                        newUnit.SetLevel( int.Parse(line[2]) );
                    }
                    if (line[1] == "Player2")
                    {
                        // instantiate a new unit at the sector
                        Unit newUnit = Instantiate(player2UnitPrefab).GetComponent<Unit>();

                        // initialize the new unit
                        newUnit.Initialize(game.players[1], sectors[i]);

                        // add the new unit to the player's list of units and 
                        // the sector's unit parameters
                        game.players[1].units.Add(newUnit);
                        //sectors[i].SetUnit(newUnit);

                        newUnit.SetLevel(int.Parse(line[2]));
                    }
                    if (line[1] == "Player3")
                    {
                        // instantiate a new unit at the sector
                        Unit newUnit = Instantiate(player3UnitPrefab).GetComponent<Unit>();

                        // initialize the new unit
                        newUnit.Initialize(game.players[2], sectors[i]);

                        // add the new unit to the player's list of units and 
                        // the sector's unit parameters
                        game.players[2].units.Add(newUnit);
                        //sectors[i].SetUnit(newUnit);

                        newUnit.SetLevel(int.Parse(line[2]));
                    }
                    if (line[1] == "Player4")
                    {
                        // instantiate a new unit at the sector
                        Unit newUnit = Instantiate(player4UnitPrefab).GetComponent<Unit>();

                        // initialize the new unit
                        newUnit.Initialize(game.players[3], sectors[i]);

                        // add the new unit to the player's list of units and 
                        // the sector's unit parameters
                        game.players[3].units.Add(newUnit);
                        sectors[i].SetUnit(newUnit);

                        newUnit.SetLevel(int.Parse(line[2]));
                    }
                }
            }

        }

        Debug.Log("Loaded!");
    }
>>>>>>> origin/MenuMerge

    public void DeactiveCurrentPlayer()
    {
        game.currentPlayer.SetActive(false);
        game.currentPlayer.GetGui().Deactivate();
    }
}













