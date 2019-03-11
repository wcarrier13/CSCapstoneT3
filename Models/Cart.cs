using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Lizst.Models
{
    public class Cart
    {
        public static ICollection<Score> ShoppingCart { get; } = new List<Score>();

        public static ICollection<Score> GetCart()
        {
            return ShoppingCart;
        }

    }
}
