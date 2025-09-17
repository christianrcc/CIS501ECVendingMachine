using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Mode
{
    public class ProductEventArgs
    {
        public IProduct Product { get; init; }
        public ProductEventArgs(IProduct product)
        {
            Product = product;
        }
    }
}
