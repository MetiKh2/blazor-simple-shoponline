using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.models.DTOs
{
    public class CartItemQuantityUpdateDto
    {
        public int CartItemDto { get; set; }
        public int Quantity { get; set; }
    }
}
