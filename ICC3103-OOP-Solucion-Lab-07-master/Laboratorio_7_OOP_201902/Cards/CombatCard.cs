﻿using Laboratorio_7_OOP_201902.Enums;
using Laboratorio_7_OOP_201902.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratorio_7_OOP_201902.Cards
{
    [Serializable]
    public class CombatCard : Card, ICharacteristics
    {
        //Atributos
        private int attackPoints;
        private bool hero;

        //Constructor
        public CombatCard(string name, EnumType type, string effect, int attackPoints, bool hero)
        {
            Name = name;
            Type = type;
            Effect = effect;
            AttackPoints = attackPoints;
            Hero = hero;
        }

        //Propiedades
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
        public bool Hero
        { get
            {
                return this.hero;
            }
            set
            {
                this.hero = value;
            }
        }

        public List<string> GetCharacteristics()
        {
            List<string> list;

            list = new List<string>()
            {
                {Name},
                $"Type: {Type}",
                $"Effect: {Effect}",
                $"AttackPoints: {AttackPoints}",
                $"Hero: {Hero}"
            };

            return list;

        }
    }
}
