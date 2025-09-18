using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Mode
{
    public class Snickers : IProduct
    {
        public string Name => "Snickers";

        public decimal Price => 1.49m;

        public ProductType Type => ProductType.Candy;
    }
}
