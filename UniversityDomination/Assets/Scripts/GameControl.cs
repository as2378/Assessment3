//======================================================
//Website link with executable:
//http://www-users.york.ac.uk/~ch1575/documentation
//======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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
        //Delete previous save data if exists
        if (File.Exists(Application.persistentDataPath + "/gameInformation.dat"))
        {
            File.Delete(Application.persistentDataPath + "/gameInformation.dat");
        }

        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/gameInformation.dat", true))
        {
            Debug.Log(Application.persistentDataPath);

            //write game state
            writer.WriteLine("Current Player:" + game.currentPlayer.name);
            writer.WriteLine("Turn state:" + game.GetTurnState());

            //write player info
            foreach (Player player in game.players)
            {
                writer.WriteLine( player.name + "Beer:" + player.GetBeer());
                writer.WriteLine( player.name + "Knowledge:" + player.GetKnowledge());
                writer.WriteLine( player.name + "IsHuman:" + player.IsHuman());
                writer.WriteLine( player.name + "IsActive:" + player.IsActive());
            }

            //get array of sectors
            Sector[] sectors = map.GetComponentsInChildren<Sector>();

            foreach (Sector sector in sectors)
            {
                //write sector name and owner name
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
        Debug.Log("Entered Load");

        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/gameInformation.dat"))
        {
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

    public void DeactiveCurrentPlayer()
    {
        game.currentPlayer.SetActive(false);
        game.currentPlayer.GetGui().Deactivate();
    }
}













