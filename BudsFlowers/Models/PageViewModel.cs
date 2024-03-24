using BudsFlowers.Areas.Identity.Data;

namespace BudsFlowers.Models
{
    public class PageViewModel
    {
        public string ControllerName { get; private set; }
        public string ActionName { get; private set; }
        public string Id { get; private set; }
        public int Min { get; private set; }
        public int Max { get; private set; }
        public TypeSort Type { get; private set; }
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public bool IsSort { get; private set; }


        public PageViewModel(string controllerName, string actionName, string id, int count, int pageNumber, int pageSize)
        {
            ControllerName = controllerName;
            ActionName = actionName;
            Id = id;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        public PageViewModel(string controllerName, string actionName, string id, int count, int min, int max, TypeSort type, int pageNumber, int pageSize)
        {
            ControllerName = controllerName;
            ActionName = actionName;
            Id = id;
            Min = min;
            Max = max;
            Type = type;
            IsSort = true;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }
    }
}
