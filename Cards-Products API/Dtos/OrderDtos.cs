using System.Collections.Generic;

namespace Cards_Products_API.Dtos
{
    public class SimulateOrderDto
    {
        public int? CardId { get; set; }          // si no viene: se escoge una válida o mock
        public int? ItemsCount { get; set; } = 2; // 1–3
    }

    public class OrderResponseDto
    {
        public string OrderId { get; set; } = null!;
        public decimal Total { get; set; }
        public string Currency { get; set; } = "CRC";
        public string Status { get; set; } = "PENDING";
        public IEnumerable<Item> Items { get; set; } = new List<Item>();
        public string? TraceId { get; set; }

        public class Item
        {
            public int ProductId { get; set; }
            public int Qty { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
