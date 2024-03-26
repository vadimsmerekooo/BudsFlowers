using BudsFlowers.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudsFlowers.Views.Admin
{
    public class AdminNavPages
    {
        public static string Index => "Index";
        public static string Orders => "Orders";
        public static string Flowers => "Flowers";
        public static string Toys => "Toys";
        public static string Candies => "Candies";
        public static string Other => "Other";
        public static string Categoryes => "Categoryes";
        public static string Reviews => "Reviews";
        public static string Users => "Users";
        public static string Messages => "Messages";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string OrdersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Orders);
        public static string FlowersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Flowers);
        public static string ToysNavClass(ViewContext viewContext) => PageNavClass(viewContext, Toys);
        public static string CandiesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Candies);
        public static string OtherNavClass(ViewContext viewContext) => PageNavClass(viewContext, Other);
        public static string CategoryesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Categoryes);
        public static string ReviewsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Reviews);
        public static string UsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Users);
        public static string MessagesNavClass(ViewContext viewContext) => PageNavClass(viewContext, Messages);
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
        public static string GetActivePage(TypeCategory type)
        {
            switch(type)
            {
                case TypeCategory.Цветы: return Flowers;
                case TypeCategory.Игрушки: return Toys;
                case TypeCategory.Конфеты: return Candies;
                case TypeCategory.Другое: return Other;
                default: return Flowers;
            }
        }
    }
}
