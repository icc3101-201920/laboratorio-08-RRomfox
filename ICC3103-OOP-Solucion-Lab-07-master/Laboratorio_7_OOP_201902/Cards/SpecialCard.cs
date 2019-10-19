using Laboratorio_7_OOP_201902.Enums;
using Laboratorio_7_OOP_201902.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratorio_7_OOP_201902.Cards
{
    [Serializable]
    public class SpecialCard : Card , ICharacteristics
    {
        //Atributos
        private string buffType;

        //Propiedades
        public string BuffType
        {
            get
            {
                return this.buffType;
            }
            set
            {
                this.buffType = value;
            }
        }
        //Constructor
        public SpecialCard(string name, EnumType type, string effect)
        {
            Name = name;
            Type = type;
            Effect = effect;
            BuffType = null;
        }

        public List<string> GetCharacteristics()
        {
            List<string> list;

            if (BuffType == null)
            {
                BuffType = "none";
            }

            list = new List<string>()
            {
                {Name},
                $"Type: {Type}",
                $"Effect: {Effect}",
                $"BuffType: {BuffType}"
            };

            return list;

        }

    }
}
