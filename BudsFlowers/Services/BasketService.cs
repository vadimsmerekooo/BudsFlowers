using BudsFlowers.Areas.Identity.Data;
using Humanizer;
using System;
using System.Text.Json;
using System.Xml.Serialization;

namespace BudsFlowers.Services
{
    public static class BasketService
    {
        private readonly static string PathFileBaskets = "Basket.xml";
        private static List<Basket> Baskets { get; set; } = new List<Basket>();

        static XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Basket>));


        public static bool Add(Basket basket)
        {
            try
            {
                Baskets.Add(basket);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> AddItem(long code, BasketFlower flower)
        {
            try
            {
                if (Baskets.Any(b => b.Code.Equals(code)))
                {
                    Basket basket = Baskets.FirstOrDefault(b => b.Code.Equals(code));
                    if (basket.Flowers is null)
                    {
                        basket.Flowers = new List<BasketFlower>();
                    }
                    if (basket.Flowers.Any(f => f.FlowerId == flower.FlowerId))
                    {
                        BasketFlower flowerBasket = basket.Flowers.FirstOrDefault(f => f.FlowerId == flower.FlowerId);
                        flowerBasket.Count = flowerBasket.Count + 1;
                    }
                    else
                    {
                        basket.Flowers.Add(flower);
                    }
                }
                else
                {
                    Basket basket = new Basket()
                    {
                        Code = code,
                        Flowers = new List<BasketFlower>() { flower }
                    };
                    Baskets.Add(basket);
                }
                await Serialize();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public static async Task<bool> MinusItem(long code, string flowerId)
        {
            try
            {
                if (Baskets.Any(b => b.Code.Equals(code)))
                {
                    Basket basket = Baskets.FirstOrDefault(b => b.Code.Equals(code));
                    if (basket.Flowers is null)
                    {
                        basket.Flowers = new List<BasketFlower>();
                    }
                    if (basket.Flowers.Any(f => f.FlowerId == flowerId))
                    {
                        BasketFlower flowerBasket = basket.Flowers.FirstOrDefault(f => f.FlowerId == flowerId);
                        flowerBasket.Count = flowerBasket.Count - 1;
                        if (flowerBasket.Count == 0)
                            flowerBasket.Count = 1;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static async Task<bool> PlusItem(long code, string flowerId)
        {
            try
            {
                if (Baskets.Any(b => b.Code.Equals(code)))
                {
                    Basket basket = Baskets.FirstOrDefault(b => b.Code.Equals(code));
                    if (basket.Flowers is null)
                    {
                        basket.Flowers = new List<BasketFlower>();
                    }
                    if (basket.Flowers.Any(f => f.FlowerId == flowerId))
                    {
                        BasketFlower flowerBasket = basket.Flowers.FirstOrDefault(f => f.FlowerId == flowerId);
                        flowerBasket.Count = flowerBasket.Count + 1;
                        if (flowerBasket.Count == 101)
                            flowerBasket.Count = 100;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        public static async Task<bool> Delete(long code, string id)
        {
            try
            {
                if(Baskets.Any(b => b.Code.Equals(code)) && Baskets.FirstOrDefault(b => b.Code.Equals(code)).Flowers.Any(f => f.FlowerId == id))
                {
                    Basket basket = Baskets.FirstOrDefault(b => b.Code.Equals(code));
                    BasketFlower flower = basket.Flowers.FirstOrDefault(f => f.FlowerId == id);
                    basket.Flowers.Remove(flower);

                    return await Serialize();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> ClearBasket(long code)
        {
            try
            {
                if(Baskets.Any(b => b.Code.Equals(code)))
                {
                    Baskets.FirstOrDefault(c => c.Code.Equals(code)).Flowers.Clear();
                    await Serialize();

                    return true;

                }
                return false;
            }
            catch
            {

            }
            return false;
        }
        public static long SetId()
        {
            try
            {
                long code = 0;
                if (Count() >= 1)
                {
                    code = Baskets.Last().Code + 1;
                }
                else
                {
                    code = 1;
                }
                Basket basket = new Basket()
                {
                    Code = code
                };
                Baskets.Add(basket);
                if (Serialize().Result)
                {
                    return code;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public static Basket GetBasketByCode(long code) => Baskets.FirstOrDefault(c => c.Code.Equals(code));
        public static int GetBasketCount(long code) => Baskets.Any(c => c.Code == code) ? Baskets.FirstOrDefault(c => c.Code == code).Flowers.Sum(s => s.Count) : 0;
        public static int Count() => Baskets.Count();

        static async Task<bool> Serialize()
        {
            try
            {
                File.Delete(PathFileBaskets);
                using (FileStream fs = new FileStream(PathFileBaskets, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, Baskets);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> Deserialize() 
        {
            try
            {
                using (FileStream fs = new FileStream(PathFileBaskets, FileMode.OpenOrCreate))
                {
                    Baskets = xmlSerializer.Deserialize(fs) as List<Basket>;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
