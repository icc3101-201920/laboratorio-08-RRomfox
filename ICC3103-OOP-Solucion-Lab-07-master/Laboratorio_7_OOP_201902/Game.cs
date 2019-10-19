using Laboratorio_7_OOP_201902.Cards;
using Laboratorio_7_OOP_201902.Enums;
using Laboratorio_7_OOP_201902.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Laboratorio_7_OOP_201902
{
    [Serializable]
    public class Game
    {
        //Constantes
        private const int DEFAULT_CHANGE_CARDS_NUMBER = 3;

        //Atributos
        private Player[] players;
        private Player activePlayer;
        private List<Deck> decks;
        private List<SpecialCard> captains;
        private Board boardGame;
        internal int turn;

        //Constructor
        public Game()
        {
            Random random = new Random();
            decks = new List<Deck>();
            captains = new List<SpecialCard>();
            AddDecks();
            AddCaptains();
            players = new Player[2] { new Player(), new Player() };
            ActivePlayer = Players[random.Next(2)];
            boardGame = new Board();
            //Add board to players
            players[0].Board = boardGame;
            players[1].Board = boardGame;
            turn = 0;
        }
        //Propiedades
        public Player[] Players
        {
            get
            {
                return this.players;
            }
            set
            {
                this.players = value;
            }
        }
        public Player ActivePlayer
        {
            get
            {
                return this.activePlayer;
            }
            set
            {
                activePlayer = value;
            }
        }
        public List<Deck> Decks
        {
            get
            {
                return this.decks;
            }
            set
            {
                this.decks = value;
            }
        }
        public List<SpecialCard> Captains
        {
            get
            {
                return this.captains;
            }
            set
            {
                this.captains = value;
            }
        }
        
        public Board BoardGame
        {
            get
            {
                return this.boardGame;
            }
            set
            {
                this.boardGame = value;
            }
        }


        //Metodos
        public bool CheckIfEndGame()
        {
            if (players[0].LifePoints == 0 || players[1].LifePoints == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int GetWinner()
        {
            if (players[0].LifePoints == 0 && players[1].LifePoints > 0)
            {
                return 1;
            }
            else if (players[1].LifePoints == 0 && players[0].LifePoints > 0)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        public int GetRoundWinner()
        {
            if (Players[0].GetAttackPoints()[0] == Players[1].GetAttackPoints()[0])
            {
                return -1;
            }
            else
            {
                int winner = Players[0].GetAttackPoints()[0] > Players[1].GetAttackPoints()[0] ? 0 : 1;
                return winner;
            }
        }

        public void Play()
        {
            string[] loadChecker = Directory.GetFiles(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files", "Game*");
            if (loadChecker.Any())
            {
                LoadData(1);
                Console.WriteLine(turn);
                if (turn != -1)
                {
                    Visualization.ShowProgramMessage("Load previous game data?\n");
                    Visualization.ShowListOptions(new List<string>() { "Yes", "No" });
                    int lc = Visualization.GetUserInput(1);
                    if (lc == 0)
                    {
                        Visualization.ClearConsole();
                        LoadData();
                    }
                }
                if (turn == -1)
                {
                    turn = 0;
                }
            }

            int userInput = 0;
            int firstOrSecondUser = ActivePlayer.Id == 0 ? 0 : 1;
            int winner = -1;
            bool bothPlayersPlayed = false;
            

            while (turn < 4 && !CheckIfEndGame())
            {
                bool drawCard = false;
                //turno 0 o configuracion
                if (turn == 0)
                {
                    for (int _ = 0; _<Players.Length; _++)
                    {
                        ActivePlayer = Players[firstOrSecondUser];
                        Visualization.ClearConsole();
                        //Mostrar mensaje de inicio
                        Visualization.ShowProgramMessage($"Player {ActivePlayer.Id+1} select Deck and Captain:");
                        //Preguntar por deck
                        Visualization.ShowDecks(this.Decks);
                        userInput = Visualization.GetUserInput(this.Decks.Count - 1);
                        Deck deck = new Deck();
                        deck.Cards = new List<Card>(Decks[userInput].Cards);
                        ActivePlayer.Deck = deck;
                        //Preguntar por capitan
                        Visualization.ShowCaptains(Captains);
                        userInput = Visualization.GetUserInput(this.Captains.Count - 1);
                        ActivePlayer.ChooseCaptainCard(new SpecialCard(Captains[userInput].Name, Captains[userInput].Type, Captains[userInput].Effect));
                        //Asignar mano
                        ActivePlayer.FirstHand();
                        userInput = 0;
                        while (userInput != 1)
                        {
                            Visualization.ClearConsole();
                            //Mostrar mano
                            Visualization.ShowHand(ActivePlayer.Hand);
                            //Mostar opciones, cambiar carta o pasar
                            Visualization.ShowListOptions(new List<string>() { "Change 3 cards", "Pass", "See specific card" }, "Select one of the following options:");
                            userInput = Visualization.GetUserInput(2);
                            if (userInput == 0)
                            {
                                Visualization.ClearConsole();
                                Visualization.ShowProgramMessage($"Player {ActivePlayer.Id + 1} change cards:");
                                Visualization.ShowHand(ActivePlayer.Hand);
                                for (int i = 0; i < DEFAULT_CHANGE_CARDS_NUMBER; i++)
                                {
                                    Visualization.ShowProgramMessage($"Input the numbers of the cards to change (max {DEFAULT_CHANGE_CARDS_NUMBER}). To stop enter -1");
                                    userInput = Visualization.GetUserInput(ActivePlayer.Hand.Cards.Count, true);
                                    if (userInput == -1) break;
                                    ActivePlayer.ChangeCard(userInput);

                                    int userInputH = -1;
                                    while (userInputH != 0)
                                    {
                                        Visualization.ClearConsole();
                                        Visualization.ShowHand(ActivePlayer.Hand);
                                        Visualization.ShowListOptions(new List<string>() { "Continue", "See specific card" }, "\nSelect one of the following options:");
                                        userInputH = Visualization.GetUserInput(1);
                                        if (userInputH == 0)
                                        {
                                            userInput = 1;
                                            break;
                                        }
                                        if (userInputH == 1)
                                        {
                                            Visualization.ShowHand(ActivePlayer.Hand, 1);
                                            userInput = 1;
                                        }
                                    }
                                    if (userInputH == 0)
                                    {
                                        break;
                                    }

                                }
                            }
                            if (userInput == 2)
                            {
                                Visualization.ShowHand(ActivePlayer.Hand, 1);
                            }
                        }
                        firstOrSecondUser = ActivePlayer.Id == 0 ? 1 : 0;
                        Visualization.ClearConsole();
                    }
                    turn += 1;
                    SaveData();
                }
                //turnos siguientes
                else
                {
                    while (true)
                    {
                        ActivePlayer = Players[firstOrSecondUser];
                        //Obtener lifePoints
                        int[] lifePoints = new int[2] { Players[0].LifePoints, Players[1].LifePoints };
                        //Obtener total de ataque:
                        int[] attackPoints = new int[2] { Players[0].GetAttackPoints()[0], Players[1].GetAttackPoints()[0] };
                        //Mostrar tablero, mano y solicitar jugada
                        Visualization.ClearConsole();
                        Visualization.ShowBoard(boardGame, ActivePlayer.Id, lifePoints,attackPoints);
                        //Robar carta
                        if (!drawCard)
                        {
                            ActivePlayer.DrawCard();
                            drawCard = true;
                        }
                        Visualization.ShowHand(ActivePlayer.Hand);
                        Visualization.ShowListOptions(new List<string> { "Play Card", "Pass", "See specific card" }, $"Make your move player {ActivePlayer.Id+1}:");
                        userInput = Visualization.GetUserInput(2);
                        if (userInput == 2)
                        {
                            Visualization.ShowHand(ActivePlayer.Hand, 1);
                            Visualization.ClearConsole();
                            continue;
                        }
                        if (userInput == 0)
                        {
                            //Si la carta es un buff solicitar a la fila que va.
                            Visualization.ShowProgramMessage($"Input the number of the card to play. To cancel enter -1");
                            userInput = Visualization.GetUserInput(ActivePlayer.Hand.Cards.Count, true);
                            if (userInput != -1)
                            {
                                if (ActivePlayer.Hand.Cards[userInput].Type == EnumType.buff)
                                {
                                    Visualization.ShowListOptions(new List<string> { "Melee", "Range", "LongRange" }, $"Choose row to buff {ActivePlayer.Id}:");
                                    int cardId = userInput;
                                    userInput = Visualization.GetUserInput(2);
                                    if (userInput == 0)
                                    {
                                        ActivePlayer.PlayCard(cardId, EnumType.buffmelee);
                                    }
                                    else if (userInput == 1)
                                    {
                                        ActivePlayer.PlayCard(cardId, EnumType.buffrange);
                                    }
                                    else
                                    {
                                        ActivePlayer.PlayCard(cardId, EnumType.bufflongRange);
                                    }
                                }
                                else
                                {
                                    ActivePlayer.PlayCard(userInput);
                                }
                            }
                            //Revisar si le quedan cartas, si no le quedan obligar a pasar.
                            if (ActivePlayer.Hand.Cards.Count == 0)
                            {
                                firstOrSecondUser = ActivePlayer.Id == 0 ? 1 : 0;
                                break;
                            }
                        }
                        else
                        {
                            firstOrSecondUser = ActivePlayer.Id == 0 ? 1 : 0;
                            SaveData();
                            break;
                        }
                    }
                    //Cambiar al oponente si no ha jugado
                    if (!bothPlayersPlayed)
                    {
                        bothPlayersPlayed = true;
                        drawCard = false;
                    }
                    //Si ambos jugaron obtener el ganador de la ronda, reiniciar el tablero y pasar de turno.
                    else
                    {
                        winner = GetRoundWinner();
                        if (winner == 0)
                        {
                            Players[1].LifePoints -= 1;
                        }
                        else if (winner == 1)
                        {
                            Players[0].LifePoints -= 1;
                        }
                        else
                        {
                            Players[0].LifePoints -= 1;
                            Players[1].LifePoints -= 1;
                        }
                        bothPlayersPlayed = false;
                        turn += 1;
                        //Destruir Cartas
                        BoardGame.DestroyCards();
                    }
                }
            }
            //Definir cual es el ganador.
            winner = GetWinner();
            if (winner == 0)
            {
                Visualization.ClearConsole();
                Visualization.ShowProgramMessage($"Player 1 is the winner!");
            }
            else if (winner == 1)
            {
                Visualization.ClearConsole();
                Visualization.ShowProgramMessage($"Player 2 is the winner!");
            }
            else
            {
                Visualization.ClearConsole();
                Visualization.ShowProgramMessage($"Draw!");
            }
            turn = -1;
            SaveData();

        }
        public void AddDecks()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Decks.txt";
            StreamReader reader = new StreamReader(path);
            int deckCounter = 0;
            List<Card> cards = new List<Card>();


            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string [] cardDetails = line.Split(",");

                if (cardDetails[0] == "END")
                {
                    decks[deckCounter].Cards = new List<Card>(cards);
                    deckCounter += 1;
                }
                else
                {
                    if (cardDetails[0] != "START")
                    {
                        if (cardDetails[0] == nameof(CombatCard))
                        {
                            cards.Add(new CombatCard(cardDetails[1], (EnumType) Enum.Parse(typeof(EnumType),cardDetails[2]), cardDetails[3], Int32.Parse(cardDetails[4]), bool.Parse(cardDetails[5])));
                        }
                        else
                        {
                            cards.Add(new SpecialCard(cardDetails[1], (EnumType)Enum.Parse(typeof(EnumType), cardDetails[2]), cardDetails[3]));
                        }
                    }
                    else
                    {
                        decks.Add(new Deck());
                        cards = new List<Card>();
                    }
                }

            }
            
        }
        public void AddCaptains()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Captains.txt";
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] cardDetails = line.Split(",");
                captains.Add(new SpecialCard(cardDetails[1], (EnumType)Enum.Parse(typeof(EnumType), cardDetails[2]), cardDetails[3]));
            }
        }

        public void SaveData()
        {
            // Streams donde guardaremos la informacion
            string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameDecks.txt";
            FileStream fs = new FileStream(fileName, FileMode.Create);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, Decks);
            fs.Close();

            string fileName1 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GamePlayers.txt";
            FileStream fs1 = new FileStream(fileName1, FileMode.Create);
            IFormatter formatter1 = new BinaryFormatter();
            formatter1.Serialize(fs1, Players);
            fs1.Close();

            string fileName2 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameActivePlayer.txt";
            FileStream fs2 = new FileStream(fileName2, FileMode.Create);
            IFormatter formatter2 = new BinaryFormatter();
            formatter2.Serialize(fs2, ActivePlayer);
            fs2.Close();

            string fileName3 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameBoardGame.txt";
            FileStream fs3 = new FileStream(fileName3, FileMode.Create);
            IFormatter formatter3 = new BinaryFormatter();
            formatter3.Serialize(fs3, BoardGame);
            fs3.Close();

            string fileName4 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameTurn.txt";
            FileStream fs4 = new FileStream(fileName4, FileMode.Create);
            IFormatter formatter4 = new BinaryFormatter();
            formatter4.Serialize(fs4, turn.ToString());
            fs4.Close();

            string fileName5 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameCaptains.txt";
            FileStream fs5 = new FileStream(fileName5, FileMode.Create);
            IFormatter formatter5 = new BinaryFormatter();
            formatter5.Serialize(fs5, Captains);
            fs5.Close();

        }

        public bool LoadData(int newGame = 0)
        {
            if (newGame == 0)
            {
                string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameDecks.txt";
                if (!File.Exists(fileName))
                {
                    return false;
                }
                FileStream fs = new FileStream(fileName, FileMode.Open);
                IFormatter formatter = new BinaryFormatter();
                Decks = formatter.Deserialize(fs) as List<Deck>;
                fs.Close();

                string fileName1 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GamePlayers.txt";
                if (!File.Exists(fileName1))
                {
                    return false;
                }
                FileStream fs1 = new FileStream(fileName1, FileMode.Open);
                IFormatter formatter1 = new BinaryFormatter();
                Players = formatter1.Deserialize(fs1) as Player[];
                fs1.Close();

                string fileName2 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameActivePlayer.txt";
                if (!File.Exists(fileName2))
                {
                    return false;
                }
                FileStream fs2 = new FileStream(fileName2, FileMode.Open);
                IFormatter formatter2 = new BinaryFormatter();
                ActivePlayer = formatter2.Deserialize(fs2) as Player;
                fs2.Close();

                string fileName3 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameBoardGame.txt";
                if (!File.Exists(fileName3))
                {
                    return false;
                }
                FileStream fs3 = new FileStream(fileName3, FileMode.Open);
                IFormatter formatter3 = new BinaryFormatter();
                BoardGame = formatter3.Deserialize(fs3) as Board;
                fs3.Close();

                string fileName4 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameTurn.txt";
                if (!File.Exists(fileName4))
                {
                    return false;
                }
                FileStream fs4 = new FileStream(fileName4, FileMode.Open);
                IFormatter formatter4 = new BinaryFormatter();
                turn = int.Parse(formatter4.Deserialize(fs4) as string);
                fs4.Close();

                string fileName5 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameCaptains.txt";
                if (!File.Exists(fileName5))
                {
                    return false;
                }
                FileStream fs5 = new FileStream(fileName5, FileMode.Open);
                IFormatter formatter5 = new BinaryFormatter();
                Captains = formatter5.Deserialize(fs5) as List<SpecialCard>;
                fs5.Close();
            }
            
            if (newGame == 1)
            {
                string fileName4 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\GameTurn.txt";
                if (!File.Exists(fileName4))
                {
                    return false;
                }
                FileStream fs4 = new FileStream(fileName4, FileMode.Open);
                IFormatter formatter4 = new BinaryFormatter();
                turn = int.Parse(formatter4.Deserialize(fs4) as string);
                fs4.Close();
            }

            return true;
        }
    }
}
