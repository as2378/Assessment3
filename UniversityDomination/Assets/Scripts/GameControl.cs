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
                writer.WriteLine(player.name + "Beer:" + player.GetBeer());
                writer.WriteLine(player.name + "Knowledge:" + player.GetKnowledge());
                writer.WriteLine(player.name + "IsHuman:" + player.IsHuman());
                writer.WriteLine(player.name + "IsActive:" + player.IsActive());

                // ASSESSMENT 4 ADDITION (22/03/2018)
                writer.Write(player.name + "PunishmentCards:");

                List<Card> cards = player.GetPunishmentCards();

                for(int i = 0; i < cards.Count; i++)
                {
                    writer.Write(cards[i].GetType().Name);

                    if(i + 1 != cards.Count)
                    {
                        writer.Write(",");
                    }
                }

                writer.WriteLine();
                //------------------------------------
            }

            // write sector info
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
                    //ASSESSMENT 4 ADDITION (22/03/2018)
                    Color unitColor = sector.GetUnit().GetColor();
                    string ownerName = sector.GetUnit().GetOwner().name;
                    int unitLevel = sector.GetUnit().GetLevel();

                    writer.WriteLine("unitOwner:" + ownerName + ":" + unitLevel + ":" + unitColor.r + ":" + unitColor.g + ":" + unitColor.b);
                    //----------------------------------
                }
            }

            // ASSESSMENT 4 ADDITION (22/03/2018)
            // Save active cards and their state
            List<Card> activeCards = game.cardDeck.GetActiveCards();
            writer.WriteLine("activeCards:" + activeCards.Count);

            foreach (Card activeCard in activeCards)
            {
                writer.WriteLine(activeCard.GetType().Name + ":" + activeCard.GetOwner().name + ":" + activeCard.GetTurnCount());

                switch (activeCard.GetType().Name)
                {
                    case "FreshersFluCard":
                        FreshersFluCard card = (FreshersFluCard)activeCard;
                        Dictionary<Player, int[]> playerPvcBonuses = card.GetPvcBonuses();

                        foreach(KeyValuePair<Player, int[]> entry in playerPvcBonuses)
                        {
                            writer.WriteLine(entry.Key.name + ":" + entry.Value[0] + ":" + entry.Value[1]);
                        }

                        break;
                }
            }
            //------------------------------------
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
                DeactiveCurrentPlayer();
                //load in player saved
                game.currentPlayer = GameObject.Find(line[1]).GetComponent<Player>();
                game.currentPlayer.SetActive(true);
                game.currentPlayer.GetGui().Activate();
            }

            //restore move state
            line = reader.ReadLine().Split(':');

            if (line[0] == "Turn state")
            {
                switch(line[1])
                {
                    case "Move1":
                        game.SetTurnState(Game.TurnState.Move1);
                        break;
                    case "Move2":
                        game.SetTurnState(Game.TurnState.Move2);
                        break;
                    case "EndOfTurn":
                        game.SetTurnState(Game.TurnState.EndOfTurn);
                        break;
                    case "NULL":
                        game.SetTurnState(Game.TurnState.NULL);
                        break;
                }
            }

            // ASSESSMENT 4 ADDITION (22/03/2018)
            // restore players
            for(int i = 0; i < 4; i++)
            {
                game.players[i].SetBeer(int.Parse(reader.ReadLine().Split(':')[1].ToString()));
                game.players[i].SetKnowledge(int.Parse(reader.ReadLine().Split(':')[1].ToString()));
                game.players[i].SetHuman(bool.Parse(reader.ReadLine().Split(':')[1]));
                game.players[i].SetActive(bool.Parse(reader.ReadLine().Split(':')[1]));

                string[] cards = reader.ReadLine().Split(':')[1].Split(',');

                foreach(string card in cards)
                {
                    switch(card)
                    {
                        case "FreshersFluCard":
                            Card freshersFluCard = new FreshersFluCard(game.players[i]);
                            game.players[i].AddPunishmentCard(freshersFluCard);
                            break;
                        case "LecturerStrikeCard":
                            Card lecturerStrikeCard = new LecturerStrikeCard(game.players[i]);
                            game.players[i].AddPunishmentCard(lecturerStrikeCard);
                            break;
                        case "KillerHangoverCard":
                            Card killerHangoverCard = new KillerHangoverCard(game.players[i]);
                            game.players[i].AddPunishmentCard(killerHangoverCard);
                            break;
                        case "NothingCard":
                            Card nothingCard = new NothingCard(game.players[i]);
                            game.players[i].AddPunishmentCard(nothingCard);
                            break;
                    }
                }
            }
            //-----------------------------------

            //sectors
            Sector[] sectors = map.GetComponentsInChildren<Sector>();

            for (int i = 0; i < 32; i ++)
            {
                string sectorName = reader.ReadLine().Split(':')[0];

                //set sector owner
                line = reader.ReadLine().Split(':');

                string playerName = line[1];
                int playerID = GetPlayerIDByName(playerName);

                if (playerID != -1)
                {
                    sectors[i].SetOwner(game.players[playerID]);
                    game.players[playerID].ownedSectors.Add(sectors[i]);
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
                    game.players[playerID].Capture(sectors[i]);
                }

                //spawn units (if present)
                //line = reader.ReadLine().Split();
                string aay = reader.ReadLine();
                line = aay.Split(':');
                
                if (line[1] != "null") //if there is a unit to place
                {
                    GameObject[] playerPrefabs = new GameObject[] {
                        player1UnitPrefab, player2UnitPrefab, player3UnitPrefab, player4UnitPrefab
                    };

                    // instantiate a new unit at the sector
                    Unit newUnit = Instantiate(playerPrefabs[playerID]).GetComponent<Unit>();

                    // initialize the new unit
                    newUnit.Initialize(game.players[playerID], sectors[i]);

                    // add the new unit to the player's list of units and 
                    // the sector's unit parameters
                    game.players[playerID].units.Add(newUnit);
                    //sectors[i].SetUnit(newUnit);

                    newUnit.SetLevel(int.Parse(line[2]));

                    //ASSESSMENT 4 ADDITION (22/03/2018)
                    Color unitColor = new Color(
                            float.Parse(line[3]),
                            float.Parse(line[4]),
                            float.Parse(line[5])
                        );
                    newUnit.SetColor(unitColor);
                    newUnit.gameObject.GetComponent<Renderer>().material.color = unitColor;
                    //----------------------------------
                }
            }

            // ASSESSMENT 4 ADDITION (22/03/2018)
            // Reset active cards and their states
            line = reader.ReadLine().Split(':');

            if(line[0] == "activeCards")
            {
                for(int i = 0; i < int.Parse(line[1]); i++)
                {
                    string[] card = reader.ReadLine().Split(':');
                    int playerID = GetPlayerIDByName(card[1]);
                    int turnCount = int.Parse(card[2]);

                    switch (card[0])
                    {
                        case "FreshersFluCard":
                            FreshersFluCard freshersFluCard = new FreshersFluCard(game.players[playerID], turnCount);

                            Dictionary<Player, int[]> bonuses = new Dictionary<Player, int[]>();

                            for(int j = 0; j < 3; j++)
                            {
                                string[] playerLine = reader.ReadLine().Split(':');
                                playerID = GetPlayerIDByName(playerLine[0]);
                                bonuses.Add(game.players[playerID], new int[] {
                                    int.Parse(playerLine[1]),
                                    int.Parse(playerLine[2])
                                });
                            }

                            freshersFluCard.SetPvcBonuses(bonuses);
                            game.cardDeck.SetActiveCard(freshersFluCard);

                            break;
                        case "LecturerStrikeCard":
                            LecturerStrikeCard lecturerStrikeCard = new LecturerStrikeCard(game.players[playerID], turnCount);
                            game.cardDeck.SetActiveCard(lecturerStrikeCard);
							GameObject strikeScene = GameObject.Instantiate (Resources.Load<GameObject> ("strike_model/StrikePeople"));
							strikeScene.name = "StrikePeople";
                            break;
                        case "KillerHangoverCard":
                            KillerHangoverCard killerHangoverCard = new KillerHangoverCard(game.players[playerID], turnCount);
                            game.cardDeck.SetActiveCard(killerHangoverCard);
                            break;
                    }
                }
            }
        }

        Debug.Log("Loaded!");
    }

    private int GetPlayerIDByName(string name)
    {
        switch (name)
        {
            case "Player1":
                return 0;
            case "Player2":
                return 1;
            case "Player3":
                return 2;
            case "Player4":
                return 3;
            default:
                return -1;
        }
    }

    public void DeactiveCurrentPlayer()
    {
        game.currentPlayer.SetActive(false);
        game.currentPlayer.GetGui().Deactivate();
    }
}