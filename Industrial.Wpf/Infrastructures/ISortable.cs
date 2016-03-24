using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Industrial.Wpf.Infrastructures
{
    public interface ISortable
    {
        void Sort(string propertyName, string direction);
    }

    public class SortablePageableCollection<T> : PageableCollection<T>, ISortable
    {
        public SortablePageableCollection(IEnumerable<T> allObjects, int count,int currentPage, int? entriesPerPage = null)
            : base(allObjects,count,currentPage, entriesPerPage)
        {
        }

        public void Sort(string propertyName, string direction)
        {
            PropertyInfo prop = typeof(T).GetProperty(propertyName);

            if (string.IsNullOrEmpty(direction) || direction.ToLower() == "descending")
                AllObjects = new ObservableCollection<T>(CurrentPageItems.OrderByDescending(x => prop.GetValue(x, null)));
            else
                AllObjects = new ObservableCollection<T>(CurrentPageItems.OrderBy(x => prop.GetValue(x, null)));

            CurrentPageNumber = 1;
           // SetCurrentPageItems();
        }
    }
}