using BudsFlowers.Areas.Identity.Data;
using Humanizer;
using System;
using System.Text.Json;

namespace BudsFlowers.Services
{
    public static class BasketService
    {
        private readonly static string PathFileBaskets = "Basket.json";
        private static List<Basket> Baskets { get; set; } = new List<Basket>();


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
        public static bool AddItem(long code, BasketFlower flower)
        {
            try
            {
                if(Baskets.Any(b => b.Code.Equals(code)))
                {
                    if (Baskets.FirstOrDefault(b => b.Code.Equals(code)).Flowers.Any(f => f.FlowerId == flower.FlowerId))
                    {
                        int count = Baskets.FirstOrDefault(b => b.Code.Equals(code)).Flowers.FirstOrDefault(f => f.FlowerId == flower.FlowerId).Count;
                        Baskets.FirstOrDefault(b => b.Code.Equals(code)).Flowers.FirstOrDefault(f => f.FlowerId == flower.FlowerId).Count = count++;
                    }
                    else
                    {
                        Baskets.FirstOrDefault(b => b.Code.Equals(code)).Flowers.Add(flower);
                    }
                }
                else
                {
                    Basket basket = new Basket()
                    {
                        Code = code,
                        Flowers = new List<BasketFlower>() { flower }
                    };
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool ClearBasket(long code)
        {
            try
            {
                if(Baskets.Any(b => b.Code.Equals(code)))
                {
                    Baskets.FirstOrDefault(c => c.Code.Equals(code)).Flowers.Clear();
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
        public static int Count() => Baskets.Count();

        static async Task<bool> Serialize()
        {
            try
            {
                using (FileStream fs = new FileStream(PathFileBaskets, FileMode.OpenOrCreate))
                {
                    await JsonSerializer.SerializeAsync(fs, Baskets);
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
                    var jsonDeserialized = await JsonSerializer.DeserializeAsync<List<Basket>>(fs);
                    if(jsonDeserialized != null)
                    {
                        Baskets = jsonDeserialized;
                    }
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
