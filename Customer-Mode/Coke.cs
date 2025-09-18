using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Mode
{
    public class Coke : IProduct
    {
        public string Name => "Coke";

        public decimal Price => 1.5m;

        public ProductType Type => ProductType.Drink;

        
    }
}
