using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChristmasPastryShop.Models.Cocktails
{
    public abstract class Cocktail : ICocktail
    {
        public Cocktail(string cocktailName, string size, double price)
        {
            Name = cocktailName;
            Size = size;
            Price = price;
        }

        private string name;

        public string Name
        {
            get { return name; }
            private set 
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.NameNullOrWhitespace));
                }
                name = value; 
            }
        }

        private string size;

        public string Size
        {
            get { return size; }
            private set 
            { 
                size = value; 
            }
        }

        private double price;

        public double Price
        {
            get { return price; }
            private set 
            {
                if (Size == "Middle")
                {
                    price = value * (2.0 / 3);
                }
                else if (Size == "Small")
                {
                    price = value * (1.0 / 3);
                }
                else
                price = value; 
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Size}) - {Price:f2} lv";
        }
    }
}
