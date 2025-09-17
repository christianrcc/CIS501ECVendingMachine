using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Mode
{
    public interface IProduct
    {
        public string Name { get; }
        public decimal Price { get; }
        ProductType Type { get; }
    }
}
