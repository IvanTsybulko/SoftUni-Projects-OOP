using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChristmasPastryShop.Models.Delicacies
{
    public abstract class Delicacy : IDelicacy
    {
        public Delicacy(string delicacyName, double price)
        {
            Name = delicacyName;
            Price = price;
        }

        private string name;

        public string Name
        {
            get { return name; }
            private set 
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.NameNullOrWhitespace));
                }

                name = value; 
            }
        }

        private double price;

        public double Price
        {
            get { return price; }
            private set { price = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Name} - {Price:f2} lv");
            return sb.ToString();
        }
    }
}
