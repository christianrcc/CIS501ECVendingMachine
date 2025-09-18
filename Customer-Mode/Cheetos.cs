using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Mode
{
    public class Cheetos : IProduct
    {
        public string Name => "Cheetos";

        public decimal Price => 3.10m;

        public ProductType Type => ProductType.Chips;

        
    }
}
