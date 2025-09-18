using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Customer_Mode
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class Stock : UserControl, INotifyPropertyChanged
    {
        
        public event EventHandler<ProductEventArgs>? SelectEvent;
        public event PropertyChangedEventHandler? PropertyChanged;

        public IProduct Product { get; init; }
        private int _quantity = 5;
        public int Quantity
        {
            get => _quantity;
            private set
            {
                _quantity = value; // <-- update the backing field first
                OnPropertyChange(nameof(Quantity)); // then notify UI

                // Disable Add button if stock is depleted
                if (AddButton != null)
                    AddButton.IsEnabled = _quantity > 0;
            }
        }
        public Stock(IProduct product)
        {
            InitializeComponent();
            Product = product;
            DataContext = this;
            AddButton.Click += AddClick;
        }

        public bool RemoveItem()
        {
            if (Quantity == 0) return false;
            Quantity--;
            
            return true;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            SelectEvent?.Invoke(this, new ProductEventArgs(Product));
        }

        private void OnPropertyChange(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
