using Cards_Products_API.Data;
using Cards_Products_API.Dtos;
using Cards_Products_API.Messaging;
using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cards_Products_API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;
        private readonly IEventPublisher _publisher;
        private readonly Random _rnd = new();

        public OrderService(AppDbContext db, IEventPublisher publisher)
        {
            _db = db;
            _publisher = publisher;
        }

        public async Task<Order> SimulateOnceAsync(SimulateOrderDto input, string? externalTraceId = null)
        {
            var cardId = await PickCardIdAsync(input.CardId); // mock o real
            if (cardId == 0) throw new InvalidOperationException("No hay tarjeta disponible.");

            var itemsCount = Math.Clamp(input.ItemsCount ?? 2, 1, 3);

            // IMPORTANTE: ajusta estos nombres a tu entidad Product real del ZIP
            // En tu ZIP vi: Product_Name, Quantity, Price
            var candidates = await _db.Products
                .AsNoTracking()
                .Where(p => p.Quantity > 0)
                .OrderBy(_ => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            if (!candidates.Any())
                throw new InvalidOperationException("No hay productos con stock.");

            var picked = candidates.OrderBy(_ => Guid.NewGuid()).Take(itemsCount).ToList();

            using var tx = await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                OrderId = Guid.NewGuid().ToString("N"),
                CardId = cardId,
                Currency = "CRC",
                Status = OrderStatus.Pending,
                TraceId = externalTraceId ?? Guid.NewGuid().ToString("N"),
                CreatedAt = DateTime.UtcNow
            };

            decimal total = 0m;

            foreach (var p in picked)
            {
                var qty = Math.Min(_rnd.Next(1, 4), p.Quantity);
                if (qty <= 0) continue;

                total += p.Price * qty;

                order.Items.Add(new OrderItem
                {
                    ProductId = p.Id,
                    Qty = qty,
                    UnitPrice = p.Price
                });

                var prod = await _db.Products.FirstAsync(x => x.Id == p.Id);
                if (prod.Quantity < qty)
                    throw new InvalidOperationException($"Stock insuficiente para producto {prod.Id}");

                prod.Quantity -= qty;
            }

            order.Total = total;

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            var payload = new
            {
                eventType = "order.created",
                orderId = order.OrderId,
                cardId = order.CardId,
                amount = order.Total,
                currency = order.Currency,
                items = order.Items.Select(i => new { productId = i.ProductId, qty = i.Qty, unitPrice = i.UnitPrice }),
                createdAt = order.CreatedAt,
                traceId = order.TraceId
            };
            await _publisher.PublishOrderCreatedAsync(payload);

            return order;
        }

        // ── Selección de tarjeta (mock o real) ────────────────────────────────
        private async Task<int> PickCardIdAsync(int? forcedId)
        {
            // 1) Si te pasan una tarjeta específica, úsala tal cual
            if (forcedId.HasValue && forcedId.Value > 0)
                return forcedId.Value;

            // 2) Si existe tabla Cards y hay tarjetas reales, toma una activa/no expirada (ajusta si tu compa define distinto)
            var hasCardsTable = _db.Database.GetDbConnection()
                .GetSchema("Tables")
                .Rows
                .Cast<System.Data.DataRow>()
                .Any(r => r["TABLE_NAME"]?.ToString()?.Equals("Cards", StringComparison.OrdinalIgnoreCase) == true);

            if (hasCardsTable)
            {
                // Si tu compañero define columnas diferentes, ajusta aquí.
                var card = await _db.Set<dynamic>().FromSqlRaw("SELECT * FROM Cards LIMIT 1").FirstOrDefaultAsync();
                if (card != null && card.Id != null) return (int)card.Id;
            }

            // 3) MOCK: si no hay tarjetas, usamos un pool fijo (configurable si quieres)
            // Puedes cambiarlo por un valor fijo 1..10 si prefieres.
            return 1; // mock cardId
        }
    }
}
