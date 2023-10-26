using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChristmasPastryShop.Models.Booths
{
    public class Booth : IBooth
    {
        private IRepository<IDelicacy> delicacyMenu;
        private IRepository<ICocktail> cocktailMenu;
        public Booth(int boothId, int capacity)
        {
            BoothId = boothId;
            Capacity = capacity;

            DelicacyMenu = new DelicacyRepository();
            CocktailMenu = new CocktailRepository();
        }

        private int boothId;

        public int BoothId
        {
            get { return boothId; }
            private set { boothId = value; }
        }

        private int capacity;

        public int Capacity
        {
            get { return capacity; }
            private set 
            { 
                if(value <= 0)
                {
                    throw new ArgumentException(String.Format(ExceptionMessages.CapacityLessThanOne));
                }
                capacity = value; 
            }
        }



        private double currentBill = 0;

        public double CurrentBill
        {
            get { return currentBill; }
        }

        private double turnover = 0;

        public double Turnover
        {
            get { return turnover; }
        }

        private bool isReserved;

        public bool IsReserved
        {
            get { return isReserved; }
            private set { isReserved = value; }
        }

        public IRepository<IDelicacy> DelicacyMenu
        {
            get { return delicacyMenu; }
            private set
            {
                delicacyMenu = value;
            }
        }

        public IRepository<ICocktail> CocktailMenu
        {
            get { return cocktailMenu; }
            private set
            {
                cocktailMenu = value;
            }
        }

        public void ChangeStatus()
        {
            isReserved = !isReserved;
        }

        public void Charge()
        {
            turnover += CurrentBill;
            currentBill = 0;
        }

        public void UpdateCurrentBill(double amount)
        {
            currentBill += amount;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Booth: {BoothId}");
            sb.AppendLine($"Capacity: {Capacity}");
            sb.AppendLine($"Turnover: {Turnover:f2} lv");
            sb.AppendLine($"-Cocktail menu:");

            foreach (var cocktail in CocktailMenu.Models)
            {
                sb.AppendLine($"--" + cocktail.ToString());
            }

            sb.AppendLine($"-Delicacy menu:");
            foreach (var delicacy in DelicacyMenu.Models)
            {
                sb.AppendLine($"--" + delicacy.ToString());
            }

            return sb.ToString();
        }
    }
}
