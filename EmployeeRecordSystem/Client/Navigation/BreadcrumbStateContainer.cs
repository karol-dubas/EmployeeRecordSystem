using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRecordSystem.Client.Navigation
{
    public class BreadcrumbStateContainer
    {
        private List<BreadcrumbItem> _items = new();

        public event Action OnChange;

        public List<BreadcrumbItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
