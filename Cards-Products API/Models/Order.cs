using System;
using System.Collections.Generic;

namespace Cards_Products_API.Models
{
    public enum OrderStatus
    {
        Pending = 1,
        Paid = 2,
        Failed = 3
    }

    public class Order
    {
        public int Id { get; set; }
        public string OrderId { get; set; } = Guid.NewGuid().ToString("N");
        public int CardId { get; set; }              // ← referencia a tarjeta (real o mock)
        public decimal Total { get; set; }
        public string Currency { get; set; } = "CRC";
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? TraceId { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}
