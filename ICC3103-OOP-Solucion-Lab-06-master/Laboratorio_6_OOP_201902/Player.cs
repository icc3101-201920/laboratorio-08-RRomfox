using Laboratorio_6_OOP_201902.Cards;
using Laboratorio_6_OOP_201902.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Laboratorio_6_OOP_201902
{
    [Serializable]
    public class Player
    {
        //Constantes
        private const int LIFE_POINTS = 2;
        private const int START_ATTACK_POINTS = 0;

        //Static
        private static int idCounter;

        //Atributos
        private int id;
        private int lifePoints;
        private int attackPoints;
        private Deck deck;
        private Hand hand;
        private Board board;
        private SpecialCard captain;

        //Constructor
        public Player()
        {
            LifePoints = LIFE_POINTS;
            AttackPoints = START_ATTACK_POINTS;
            Deck = new Deck();
            Hand = new Hand();
            Id = idCounter++;
        }

        //Propiedades
        public int Id { get => id; set => id = value; }
        public int LifePoints
        {
            get
            {
                return this.lifePoints;
            }
            set
            {
                this.lifePoints = value;
            }
        }
        public int AttackPoints
        {
            get
            {
                return this.attackPoints;
            }
            set
            {
                this.attackPoints = value;
            }
        }
        public Deck Deck
        {
            get
            {
                return this.deck;
            }
            set
            {
                this.deck = value;
            }
        }
        public Hand Hand
        {
            get
            {
                return this.hand;
            }
            set
            {
                this.hand = value;
            }
        }
        public Board Board
        {
            get
            {
                return this.board;
            }
            set
            {
                this.board = value;
            }
        }
        public SpecialCard Captain
        {
            get
            {
                return this.captain;
            }
            set
            {
                this.captain = value;
            }
        }

        //Metodos
        public void DrawCard(int cardId = 0)
        {
            Card tempCard = CreateTempCard(cardId);
            hand.AddCard(tempCard);
            deck.DestroyCard(cardId);
        }
        public void PlayCard(int cardId, EnumType buffRow = EnumType.None)
        {
            
            Card tempCard = CreateTempCard(cardId, false);

            if (tempCard is CombatCard)
            {
                board.AddCard(tempCard, this.Id);
            }
            else
            {
                if (tempCard.Type == EnumType.buff)
                {
                    board.AddCard(tempCard, this.Id, buffRow);
                }
                else
                {
                    board.AddCard(tempCard);
                }
            }
            hand.DestroyCard(cardId);
        }

        public void ChangeCard(int cardId)
        {
            Card tempCard = CreateTempCard(cardId, false);
            hand.DestroyCard(cardId);
            Random random = new Random();
            int deckCardId = random.Next(0, deck.Cards.Count);
            Card tempDeckCard = CreateTempCard(deckCardId);
            hand.AddCard(tempDeckCard);
            deck.DestroyCard(deckCardId);
            deck.AddCard(tempCard);
        }

        public void FirstHand()
        {
            Random random = new Random();
            for (int i = 0; i<10; i++)
            {
                DrawCard(random.Next(0, deck.Cards.Count));
            }
        }

        public void ChooseCaptainCard(SpecialCard captainCard)
        {
            Captain = captainCard;
            board.AddCard(new SpecialCard(Captain.Name, Captain.Type, Captain.Effect), Id);
        }


        public Card CreateTempCard(int cardId, bool useDeck = true)
        {
            Deck cardList = useDeck ? deck : hand;

            if (cardList.Cards[cardId] is CombatCard)
            {
                CombatCard card = cardList.Cards[cardId] as CombatCard;
                return new CombatCard(card.Name, card.Type, card.Effect, card.AttackPoints, card.Hero);
            }
            else
            {
                SpecialCard card = cardList.Cards[cardId] as SpecialCard;
                return new SpecialCard(card.Name, card.Type, card.Effect);
            }
        }

        //public void SaveData()
        //{
        //    Streams donde guardaremos la informacion
        //    string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Decks" + Id.ToString() + ".txt";
        //    FileStream fs = new FileStream(fileName, FileMode.Create);
        //    IFormatter formatter = new BinaryFormatter();
        //    formatter.Serialize(fs, Deck);
        //    fs.Close();
        //    string fileName1 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\LifePoints" + Id.ToString() + ".txt";
        //    FileStream fs1 = new FileStream(fileName1, FileMode.Create);
        //    IFormatter formatter1 = new BinaryFormatter();
        //    formatter.Serialize(fs1, LifePoints);
        //    fs.Close();
        //    string fileName2 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\AttackPoints" + Id.ToString() + ".txt";
        //    FileStream fs2 = new FileStream(fileName2, FileMode.Create);
        //    IFormatter formatter2 = new BinaryFormatter();
        //    formatter.Serialize(fs2, AttackPoints);
        //    fs.Close();
        //    string fileName3 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Hand" + Id.ToString() + ".txt";
        //    FileStream fs3 = new FileStream(fileName3, FileMode.Create);
        //    IFormatter formatter3 = new BinaryFormatter();
        //    formatter.Serialize(fs3, Hand);
        //    fs.Close();
        //    string fileName4 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Id" + Id.ToString() + ".txt";
        //    FileStream fs4 = new FileStream(fileName4, FileMode.Create);
        //    IFormatter formatter4 = new BinaryFormatter();
        //    formatter.Serialize(fs4, Hand);
        //    fs.Close();
        //}

        //public bool LoadData()
        //{
        //    string fileName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Decks" + Id.ToString() + ".txt";
        //    if (!File.Exists(fileName))
        //    {
        //        return false;
        //    }
        //    FileStream fs = new FileStream(fileName, FileMode.Open);
        //    IFormatter formatter = new BinaryFormatter();
        //    Deck = formatter.Deserialize(fs) as Deck;
        //    fs.Close();
        //    string fileName1 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\LifePoints" + Id.ToString() + ".txt";
        //    if (!File.Exists(fileName1))
        //    {
        //        return false;
        //    }
        //    FileStream fs1 = new FileStream(fileName1, FileMode.Open);
        //    IFormatter formatter1 = new BinaryFormatter();
        //    Deck = formatter.Deserialize(fs1) as Deck;
        //    fs1.Close();
        //    string fileName2 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\AttackPoints" + Id.ToString() + ".txt";
        //    if (!File.Exists(fileName2))
        //    {
        //        return false;
        //    }
        //    FileStream fs2 = new FileStream(fileName2, FileMode.Open);
        //    IFormatter formatter2 = new BinaryFormatter();
        //    Deck = formatter.Deserialize(fs2) as Deck;
        //    fs2.Close();
        //    string fileName3 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Hand" + Id.ToString() + ".txt";
        //    if (!File.Exists(fileName3))
        //    {
        //        return false;
        //    }
        //    FileStream fs3 = new FileStream(fileName3, FileMode.Open);
        //    IFormatter formatter3 = new BinaryFormatter();
        //    Deck = formatter.Deserialize(fs3) as Deck;
        //    fs3.Close();
        //    string fileName4 = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent + @"\Files\Id" + Id.ToString() + ".txt";
        //    if (!File.Exists(fileName4))
        //    {
        //        return false;
        //    }
        //    FileStream fs4 = new FileStream(fileName4, FileMode.Open);
        //    IFormatter formatter4 = new BinaryFormatter();
        //    Id = formatter.Deserialize(fs4) as int;
        //    fs4.Close();
        //    return true;
        //}

    }
}
