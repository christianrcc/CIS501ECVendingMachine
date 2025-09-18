using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Mode
{
    public class Pepsi : IProduct
    {
        public string Name => "Pepsi";

        public decimal Price => 5.50m;

        public ProductType Type => ProductType.Drink;
    }
}
