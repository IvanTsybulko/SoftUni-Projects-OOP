using ChristmasPastryShop.Core.Contracts;
using ChristmasPastryShop.Models.Booths;
using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristmasPastryShop.Core
{
    public class Controller : IController
    {
        public Controller()
        {
            booths = new BoothRepository();
        }

        private BoothRepository booths;

        public string AddBooth(int capacity)
        {
            Booth booth = new Booth(booths.Models.Count + 1, capacity);
            
            booths.AddModel(booth);

            return String.Format(OutputMessages.NewBoothAdded, booth.BoothId, capacity);
        }

        public string AddCocktail(int boothId, string cocktailTypeName, string cocktailName, string size)
        {
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);

            if (cocktailTypeName == "MulledWine")
            {
                if (size != "Small" && size != "Middle" && size != "Large")
                {
                    return String.Format(OutputMessages.InvalidCocktailSize, size);
                } 

                ICocktail cocktail = new MulledWine(cocktailName, size);

                if (booth.CocktailMenu.Models.Contains(cocktail))
                {
                    return String.Format(OutputMessages.CocktailAlreadyAdded, cocktailTypeName, cocktailName);
                }

                booth.CocktailMenu.AddModel(cocktail);

                return String.Format(OutputMessages.NewCocktailAdded, size, cocktailName, cocktailTypeName);
            }
            else if (cocktailTypeName == "Hibernation")
            {
                if (size != "Small" && size != "Middle" && size != "Large")
                {
                    return String.Format(OutputMessages.InvalidCocktailSize, size);
                }

                ICocktail cocktail = new Hibernation(cocktailName, size);

                if (booth.CocktailMenu.Models.Contains(cocktail))
                {
                    return String.Format(OutputMessages.CocktailAlreadyAdded, cocktailTypeName, cocktailName);
                }

                booth.CocktailMenu.AddModel(cocktail);

                return String.Format(OutputMessages.NewCocktailAdded, size, cocktailName, cocktailTypeName);
            }

            return String.Format(OutputMessages.InvalidCocktailType, cocktailTypeName);
        }

        public string AddDelicacy(int boothId, string delicacyTypeName, string delicacyName)
        {
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);

            if (delicacyTypeName == "Gingerbread")
            {
                IDelicacy delicacy = new Gingerbread(delicacyName);

                if (booth.DelicacyMenu.Models.Contains(delicacy))
                {
                    return String.Format(OutputMessages.DelicacyAlreadyAdded, delicacyName);
                }

                booth.DelicacyMenu.AddModel(delicacy);

                return String.Format(OutputMessages.NewDelicacyAdded,delicacyTypeName, delicacyName);
            }
            else if(delicacyTypeName == "Stolen")
            {
                IDelicacy delicacy = new Stolen(delicacyName);

                if (booth.DelicacyMenu.Models.Contains(delicacy))
                {
                    return String.Format(OutputMessages.DelicacyAlreadyAdded, delicacyName);
                }

                booth.DelicacyMenu.AddModel(delicacy);

                return String.Format(OutputMessages.NewDelicacyAdded, delicacyTypeName, delicacyName);
            }

            return String.Format(OutputMessages.InvalidDelicacyType, delicacyTypeName);
        }

        public string BoothReport(int boothId)
        {
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);

            return booth.ToString();
        }

        public string LeaveBooth(int boothId)
        {
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Bill {booth.CurrentBill:f2} lv");
            sb.AppendLine($"Booth {boothId} is now available!");

            booth.Charge();
            booth.ChangeStatus();

            return sb.ToString().Trim();
        }

        public string ReserveBooth(int countOfPeople)
        {
            List<IBooth> orderedBooths = booths.Models.Where(b=>b.IsReserved == false && b.Capacity >= countOfPeople)
                .OrderBy(b => b.Capacity).ThenByDescending(b => b.BoothId).ToList();

            if(orderedBooths.Count == 0)
            {
                return String.Format(OutputMessages.NoAvailableBooth, countOfPeople);
            }

            IBooth booth = orderedBooths[0];

            booth.ChangeStatus();
            return String.Format(OutputMessages.BoothReservedSuccessfully, booth.BoothId, countOfPeople);
        }

        public string TryOrder(int boothId, string order)
        {
            IBooth booth = booths.Models.FirstOrDefault(x => x.BoothId == boothId);
            string[] orderTokens = order.Split('/').ToArray();

            string itemtype = orderTokens[0];
            string name = orderTokens[1];
            int quantity = int.Parse(orderTokens[2]);

            if (itemtype== "MulledWine" || itemtype == "Hibernation")
            {
                if (booth.CocktailMenu.Models.FirstOrDefault(x => x.Name == name) == null)
                {
                    return String.Format(OutputMessages.NotRecognizedItemName, itemtype, name);
                }

                ICocktail cocktail = booth.CocktailMenu.Models.FirstOrDefault(x => x.Name == name && x.Size == orderTokens[3]);


                if (booth.CocktailMenu.Models.Contains(cocktail))
                {
                    booth.UpdateCurrentBill(cocktail.Price * quantity);
                    return String.Format(OutputMessages.SuccessfullyOrdered, booth.BoothId, quantity, name);
                }

                return String.Format(OutputMessages.CocktailStillNotAdded, orderTokens[3], name);
            }
            else if (itemtype == "Stolen" || itemtype == "Gingerbread")
            {
                IDelicacy delicacy = booth.DelicacyMenu.Models.FirstOrDefault(x => x.Name == name);

                if (delicacy == null)
                {
                    return String.Format(OutputMessages.NotRecognizedItemName, itemtype, name);
                }

                booth.UpdateCurrentBill(delicacy.Price * quantity);
                return String.Format(OutputMessages.SuccessfullyOrdered, booth.BoothId, quantity, name);
            }

             return String.Format(OutputMessages.NotRecognizedType, itemtype);
        }
    }
}
