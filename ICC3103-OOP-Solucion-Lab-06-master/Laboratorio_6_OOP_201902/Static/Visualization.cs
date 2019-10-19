using Laboratorio_6_OOP_201902.Cards;
using Laboratorio_6_OOP_201902.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratorio_6_OOP_201902.Static
{
    public static class Visualization
    {
        public static void ShowHand(Hand hand, int specific = 0)
        {
            if (specific == 0)
            {
                CombatCard combatCard;
                Console.WriteLine("Hand: ");
                for (int i = 0; i < hand.Cards.Count; i++)
                {
                    if (hand.Cards[i] is CombatCard)
                    {
                        combatCard = hand.Cards[i] as CombatCard;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"|({i}) {combatCard.Name} ({combatCard.Type}): {combatCard.AttackPoints} |");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"|({i}) {hand.Cards[i].Name} ({hand.Cards[i].Type}) |");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            if (specific == 1)
            {
                Console.WriteLine("\nSelect one card in your hand:\n");
                int op = GetUserInput(hand.Cards.Count);
                Console.WriteLine();

                if (hand.Cards[op] is CombatCard)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (string j in (hand.Cards[op] as CombatCard).GetCharacteristics())
                    {
                        Console.WriteLine(j);
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    foreach (string j in (hand.Cards[op] as SpecialCard).GetCharacteristics())
                    {
                        Console.WriteLine(j);
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
                Console.ResetColor();

                Console.WriteLine("\nPress any key to continue");
                Console.ReadLine();
            }

            
        }
        public static void ShowDecks(List<Deck> decks)
        {
            Console.WriteLine("Select one Deck:\n");
            for (int i = 0; i<decks.Count; i++)
            {
                Console.WriteLine($"({i}) Deck {i+1}:");
                foreach (string j in decks[i].GetCharacteristics())
                {
                    Console.WriteLine(j);
                }
                Console.WriteLine();
            }
        }
        public static void ShowCaptains(List<SpecialCard> captains)
        {
            Console.WriteLine("\nSelect one captain:\n");
            for (int i = 0; i < captains.Count; i++)
            {
                Console.Write($"({i}) ");
                foreach (string j in captains[i].GetCharacteristics())
                {
                    Console.WriteLine(j);
                }
                Console.WriteLine();
            }
        }
        public static int GetUserInput(int maxInput, bool stopper = false)
        {
            bool valid = false;
            int value;
            int minInput = stopper ? -1 : 0;
            while (!valid)
            {

                if (int.TryParse(Console.ReadLine(), out value))
                {
                    if (value >= minInput && value <= maxInput)
                    {
                        return value;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine($"The option ({value}) is not valid, try again");
                        Console.ResetColor();
                    }
                }
                else
                {
                    ConsoleError($"Input must be a number, try again");
                }
            }
            return -1;
        }
        public static void ConsoleError(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void ShowProgramMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void ShowListOptions (List<string> options, string message = null)
        {
            if (message != null) Console.WriteLine($"{message}");
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"({i}) {options[i]}");
            }
        }
        public static void ClearConsole()
        {
            Console.ResetColor();
            Console.Clear();
        }

    }
    
}
