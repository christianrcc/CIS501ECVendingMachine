using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Customer_Mode
{
    public partial class UserInterface : Window, INotifyPropertyChanged
    {
        // ===== Bindable state =====
        private decimal _userBalance;
        public decimal UserBalance
        {
            get => _userBalance;
            private set
            {
                if (_userBalance != value)
                {
                    _userBalance = value;
                    OnPropertyChanged(nameof(UserBalance));
                    UpdateEnabled();
                }
            }
        }

        private decimal _totalCost;
        public decimal TotalCost
        {
            get => _totalCost;
            private set
            {
                if (_totalCost != value)
                {
                    _totalCost = value;
                    OnPropertyChanged(nameof(TotalCost));
                    UpdateEnabled();
                }
            }
        }

        // Cart & reservations
        public ObservableCollection<IProduct> UserSelection { get; } = new();
        private readonly Dictionary<IProduct, int> _reserved = new(); // how many of each product are in the cart (but not yet vended)

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public UserInterface()
        {
            InitializeComponent();
            DataContext = this;

            LoadProductsIntoGrid();
            WireMoneyButtons();
            WireActionButtons();

            UpdateEnabled(); // initialize button states
        }

        // =======================
        // Products / Stock tiles
        // =======================
        private void LoadProductsIntoGrid()
        {
            if (FindName("ProductItems") is not ItemsControl productItems)
                return;

            var products = new IProduct[]
            {
                new Pepsi(), new Coke(),
                new Funions(), new Cheetos(),
                new Twix(), new Snickers()
            };

            foreach (var p in products)
            {
                var stock = new Stock(p)
                {
                    Margin = new Thickness(8)
                };
                stock.SelectEvent += AddItem; // "Add" click
                productItems.Items.Add(stock);
            }
        }

        // ==================
        // Money buttons
        // ==================
        private void WireMoneyButtons()
        {
            TryWire("QuarterBtn", () => CoinInsert(Coins.TWENTYFIVE_CENT));
            TryWire("DimeBtn", () => CoinInsert(Coins.TEN_CENT));
            TryWire("NickelBtn", () => CoinInsert(Coins.FIVE_CENT));

            TryWire("OneBtn", () => BillInsert(Bills.ONE_DOLLAR));
            TryWire("FiveBtn", () => BillInsert(Bills.FIVE_DOLLAR));
            TryWire("TenBtn", () => BillInsert(Bills.TEN_DOLLAR));
        }

        private void WireActionButtons()
        {
            TryWire("FinishBtn", FinishTransaction);
            TryWire("RefundBtn", ReturnChange);
            TryWire("CancelBtn", CancelTransaction);
        }

        private void TryWire(string buttonName, Action handler)
        {
            if (FindName(buttonName) is Button btn)
                btn.Click += (_, __) => handler();
        }

        // ==========
        // Handlers
        // ==========
        private void CoinInsert(Coins coin)
        {
            UserBalance += coin switch
            {
                Coins.FIVE_CENT => 0.05m,
                Coins.TEN_CENT => 0.10m,
                Coins.TWENTYFIVE_CENT => 0.25m,
                _ => 0m
            };
        }

        private void BillInsert(Bills bill)
        {
            UserBalance += bill switch
            {
                Bills.ONE_DOLLAR => 1.00m,
                Bills.FIVE_DOLLAR => 5.00m,
                Bills.TEN_DOLLAR => 10.00m,
                _ => 0m
            };
        }

        // Reservation-aware Add
        private void AddItem(object? sender, ProductEventArgs e)
        {
            var product = e.Product;

            // Find the Stock control for this product
            if (FindName("ProductItems") is not ItemsControl productItems)
                return;

            var stock = productItems.Items.OfType<Stock>().FirstOrDefault(s => ReferenceEquals(s.Product, product));
            if (stock is null) return;

            // Remaining reservable units = actual quantity - already reserved
            var reserved = GetReserved(product);
            var remaining = stock.Quantity - reserved;

            if (remaining <= 0)
            {
                MessageBox.Show($"{product.Name} is out of stock.");
                UpdateEnabled();
                return;
            }

            // Affordability check based on remaining funds after current cart
            var remainingFunds = UserBalance - TotalCost;
            if (remainingFunds < product.Price)
            {
                MessageBox.Show($"Not enough balance to add {product.Name}.");
                UpdateEnabled();
                return;
            }

            // Reserve one and add to cart
            _reserved[product] = reserved + 1;
            UserSelection.Add(product);
            RecomputeTotals();      // updates TotalCost -> calls UpdateEnabled
        }

        private int GetReserved(IProduct p) =>
            _reserved.TryGetValue(p, out var n) ? n : 0;

        private void RecomputeTotals()
        {
            TotalCost = UserSelection.Sum(p => p.Price);
        }

        private void FinishTransaction()
        {
            if (TotalCost <= 0m)
            {
                MessageBox.Show("No items selected.");
                return;
            }
            if (UserBalance < TotalCost)
            {
                MessageBox.Show("Insufficient balance.");
                return;
            }

            // Convert reservations to real decrements
            if (FindName("ProductItems") is ItemsControl productItems)
            {
                foreach (var kvp in _reserved.ToList())
                {
                    var product = kvp.Key;
                    var needed = kvp.Value;

                    var stock = productItems.Items.OfType<Stock>().FirstOrDefault(s => ReferenceEquals(s.Product, product));
                    if (stock is null) continue;

                    // Remove 'needed' times (Stock.RemoveItem() protects against underflow)
                    for (int i = 0; i < needed; i++)
                        stock.RemoveItem();
                }
            }

            UserBalance -= TotalCost;
            UserSelection.Clear();
            _reserved.Clear();
            RecomputeTotals(); // sets TotalCost = 0
            UpdateEnabled();

            MessageBox.Show("Transaction complete! Enjoy your items!");
        }

        private void ReturnChange()
        {
            if (UserBalance > 0m)
            {
                MessageBox.Show($"Refunded {UserBalance:C}.");
                UserBalance = 0m;
            }
            // Refund does not touch the cart by default
            UpdateEnabled();
        }

        private void CancelTransaction()
        {
            if (UserSelection.Count > 0)
                MessageBox.Show("Selection cleared.");

            UserSelection.Clear();
            _reserved.Clear();
            RecomputeTotals();
            UpdateEnabled();
        }

        // ===== Enable/disable logic =====
        private void UpdateEnabled()
        {
            // Finish enabled only if cart has items and balance covers total
            if (FindName("FinishBtn") is Button finish)
                finish.IsEnabled = TotalCost > 0m && UserBalance >= TotalCost;

            // Refund enabled only if there is balance to return
            if (FindName("RefundBtn") is Button refund)
                refund.IsEnabled = UserBalance > 0m;

            // Enable/disable each Stock tile based on:
            //  - reservable units left (Quantity - reserved > 0)
            //  - remaining funds (UserBalance - TotalCost) >= product.Price
            if (FindName("ProductItems") is ItemsControl productItems)
            {
                var remainingFunds = UserBalance - TotalCost;

                foreach (var stock in productItems.Items.OfType<Stock>())
                {
                    var reserved = GetReserved(stock.Product);
                    var reservableLeft = stock.Quantity - reserved;

                    bool hasUnits = reservableLeft > 0;
                    bool canAffordAnother = remainingFunds >= stock.Product.Price;

                    stock.IsEnabled = hasUnits && canAffordAnother;
                }
            }
        }
    }
}
