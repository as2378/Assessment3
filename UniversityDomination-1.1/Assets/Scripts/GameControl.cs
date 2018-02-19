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
            //write game state
            writer.WriteLine("Current Player: " + game.currentPlayer.name);
            writer.WriteLine("Turn state: " + game.GetTurnState());

            //write player info
            foreach (Player player in game.players)
            {
                writer.WriteLine(player.GetBeer());
                writer.WriteLine(player.GetKnowledge());
                writer.WriteLine(player.IsHuman());
                writer.WriteLine(player.IsActive());
            }

            //get array of sectors
            Sector[] sectors = map.GetComponentsInChildren<Sector>();

            foreach (Sector sector in sectors)
            {
                //write sector name and owner name
                writer.WriteLine(sector.name);
                if (sector.GetOwner() == null)
                {
                    writer.WriteLine("null");
                }
                else
                {
                    writer.WriteLine(sector.GetOwner().name);
                }

                //write landmark info
                if (sector.GetLandmark() == null)
                {
                    writer.WriteLine("null");
                }
                else
                {
                    writer.WriteLine(sector.GetLandmark().GetResourceType());
                    writer.WriteLine(sector.GetLandmark().GetAmount());
                }

                //write unit info
                if (sector.GetUnit() == null)
                {
                    writer.WriteLine("null");
                }
                else
                {
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

        Debug.Log("Entered Load");

        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/gameInformation.dat"))
        {
            string line = reader.ReadLine();
            //restore current player
            if (line == "Current Player: Player1")
            {
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find("Player1").GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            }
            else if (line == "Current Player: Player2")
            {
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find("Player2").GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            }
            else if (line == "Current Player: Player3")
            {
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find("Player3").GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            }
            else if (line == "Current Player: Player4")
            {
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find("Player4").GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            }

            //restore movestate
            line = reader.ReadLine();
            if (line == "Move1")
            {
                game.SetTurnState(Game.TurnState.Move1);
            }
            else if (line == "Move2")
            {
                game.SetTurnState(Game.TurnState.Move2);
            }
            else if (line == "EndOfTurn")
            {
                game.SetTurnState(Game.TurnState.EndOfTurn);
            }
            else if (line == "NULL")
            {
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
            line = reader.ReadLine();
            if (line == "Player1")
            {
                sectors[0].SetOwner(game.players[0]);
            }
            else if (line == "Player2")
            {
                sectors[0].SetOwner(game.players[1]);
            }
            else if (line == "Player3")
            {
                sectors[0].SetOwner(game.players[2]);
            }
            else if (line == "Player4")
            {
                sectors[0].SetOwner(game.players[3]);
            }

            line = reader.ReadLine();
            if (line == "ViceChancellor")
            {
                //sectors[0].SetLandmark( viceChancellorGameObj.GetComponent<Landmark>());
                sectors[0].GetLandmark().SetResourceType(Landmark.ResourceType.Beer);
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













