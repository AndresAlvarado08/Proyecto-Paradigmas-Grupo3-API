using Bogus;
using System.Text;
using Cards_Products_API.Data;
using Cards_Products_API.Models;
using Cards_Products_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Cards_Products_API.DTO_s;

public class CardService : ICardService
{
    private readonly AppDbContext _context;

    public CardService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Card>> GetAllCards()
    {
        return await _context.Cards.ToListAsync();
    }

    public async Task<PaginacionDTO<Card>> GetAllCardsPaged(int page = 1, int pageSize = 10)
    {
        try
        {
            // Total de registros en la tabla Cards
            var totalRecords = await _context.Cards.CountAsync();

            // Total de páginas
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Datos paginados
            var cards = await _context.Cards
                .OrderBy(c => c.Card_Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginacionDTO<Card>
            {
                Total_Records = totalRecords,
                Total_Pages = totalPages,
                Page = page,
                Page_Size = pageSize,
                Items = cards
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Card> CreateRandomCard(double probabilityExpired = 0.35, bool formatWithSpaces = true)
    {
        var cardTypes = new[] { "Visa", "MasterCard", "American Express", "Discover" };
        var users = await _context.Users.ToListAsync();
        var faker = new Faker();

        var cardType = faker.PickRandom(cardTypes);
        var rawNumber = GenerateCardNumberByType(cardType, faker); // sin formato
        var formattedNumber = formatWithSpaces ? FormatCardNumber(rawNumber, cardType) : rawNumber;

        var moneyValue = faker.Finance.Random.Int(100000, 300000);
        var expired = faker.Random.Double() < probabilityExpired; // true => generar vencida
        var expiration = GenerateExpirationDate(expired, faker);

        if (users == null || !users.Any())
        {
            throw new InvalidOperationException("No hay usuarios disponibles para asignar la tarjeta.");
        }

        var card = new Card
        {
            Card_Type = cardType,
            Card_Number = formattedNumber,
            User_Id = faker.PickRandom(users).User_Id,
            Money = moneyValue,
            Expiration_Date = expiration
        };

        _context.Cards.Add(card);
        await _context.SaveChangesAsync();

        return card;
    }

    private string GenerateCardNumberByType(string cardType, Faker faker)
    {
        if (cardType == "Visa")
        {
            // Visa: comienza con 4, longitud 16 (también puede ser 13 o 19; usamos 16)
            return GenerateNumberWithPrefixAndLength(new[] { "4" }, 16, faker);
        }

        if (cardType == "MasterCard")
        {
            // MasterCard: 51-55 o 2221-2720 (16 dígitos)
            var prefixes = new List<string>();
            // 51-55
            for (int i = 51; i <= 55; i++) prefixes.Add(i.ToString());
            // 2221-2720 (agregamos como rango dinámico, no llenamos lista completa)
            // Para MasterCard, elegimos aleatoriamente entre 51-55 o 2221-2720:
            bool chooseOldRange = faker.Random.Bool(); // 50/50
            if (chooseOldRange)
            {
                return GenerateNumberWithPrefixAndLength(prefixes.ToArray(), 16, faker);
            }
            else
            {
                int randPrefix = faker.Random.Number(2221, 2720);
                return GenerateNumberWithPrefixAndLength(new[] { randPrefix.ToString() }, 16, faker);
            }
        }

        if (cardType == "American Express" || cardType == "AmEx" || cardType == "Amex")
        {
            // American Express: 34 o 37, longitud 15
            return GenerateNumberWithPrefixAndLength(new[] { "34", "37" }, 15, faker);
        }

        if (cardType == "Discover")
        {
            // Discover: 6011, 65, 644-649 (16 dígitos)
            var prefixes = new List<string> { "6011", "65" };
            for (int i = 644; i <= 649; i++) prefixes.Add(i.ToString());
            return GenerateNumberWithPrefixAndLength(prefixes.ToArray(), 16, faker);
        }

        // Fallback: número genérico de 16 dígitos
        return GenerateNumberWithPrefixAndLength(new[] { faker.Random.Number(1, 9).ToString() }, 16, faker);
    }

    private string GenerateNumberWithPrefixAndLength(string[] prefixes, int totalLength, Faker faker)
    {
        // Escoge un prefijo
        var prefix = faker.PickRandom(prefixes);

        var sb = new StringBuilder(prefix);
        // Generar dígitos aleatorios hasta totalLength-1 (reservamos último dígito para Luhn)
        while (sb.Length < totalLength - 1)
        {
            sb.Append(faker.Random.Number(0, 9));
        }

        var partial = sb.ToString();
        var checkDigit = CalculateLuhnCheckDigit(partial);
        sb.Append(checkDigit);

        return sb.ToString();
    }

    // Calcula el dígito de control Luhn para una secuencia numérica (sin el dígito final)
    private int CalculateLuhnCheckDigit(string numberWithoutCheckDigit)
    {
        // Convertir a array de dígitos
        int[] digits = numberWithoutCheckDigit.Select(c => c - '0').ToArray();
        int sum = 0;
        // Procesar desde el final (derecha). Posición 0 desde la derecha no se duplica, posición 1 se duplica, etc.
        for (int i = digits.Length - 1, posFromRight = 0; i >= 0; i--, posFromRight++)
        {
            int d = digits[i];
            if (posFromRight % 2 == 0)
            {
                // índice par desde la derecha -> no duplicar
                sum += d;
            }
            else
            {
                // índice impar desde la derecha -> duplicar
                int doubled = d * 2;
                if (doubled > 9) doubled -= 9;
                sum += doubled;
            }
        }
        int mod = sum % 10;
        int check = (10 - mod) % 10;
        return check;
    }

    // Formatea el número: AmEx -> 4-6-5, otros -> grupos de 4
    private string FormatCardNumber(string number, string cardType)
    {
        if (cardType.StartsWith("American", StringComparison.OrdinalIgnoreCase) ||
            cardType.Equals("AmEx", StringComparison.OrdinalIgnoreCase) ||
            cardType.Equals("Amex", StringComparison.OrdinalIgnoreCase))
        {
            // AmEx 15 dígitos -> 4-6-5
            if (number.Length != 15) return number;
            return $"{number.Substring(0, 4)} {number.Substring(4, 6)} {number.Substring(10, 5)}";
        }
        else
        {
            // Grupo de 4 en adelante (soporta 16, 19, etc.)
            var groups = new List<string>();
            for (int i = 0; i < number.Length; i += 4)
            {
                int len = Math.Min(4, number.Length - i);
                groups.Add(number.Substring(i, len));
            }
            return string.Join(' ', groups);
        }
    }

    // Genera la Expiration Date como último día del mes y 23:59:59 UTC
    private DateOnly GenerateExpirationDate(bool expired, Faker faker)
    {
        if (expired)
        {
            // Fecha pasada: entre 1 mes y 5 años en el pasado
            var past = faker.Date.Past(5); // DateTime
            int year = past.Year;
            int month = past.Month;
            int lastDay = DateTime.DaysInMonth(year, month);
            return new DateOnly(year, month, lastDay);
        }
        else
        {
            // Futuro: entre 1 mes y 5 años en el futuro
            var future = faker.Date.Future(5, DateTime.UtcNow.AddMonths(1)); // DateTime
            int year = future.Year;
            int month = future.Month;
            int lastDay = DateTime.DaysInMonth(year, month);
            return new DateOnly(year, month, lastDay);
        }
    }

    public async Task<Card?> UpdateCard(int cardId, UpdateCardDTO dto)
    {
        var card = await _context.Cards.FindAsync(cardId);
        if (card == null) return null;

        if (dto.Money.HasValue)
            card.Money = dto.Money.Value;

        if (dto.Expiration_Date.HasValue)
            card.Expiration_Date = dto.Expiration_Date.Value;

        await _context.SaveChangesAsync();
        return card;
    }

    public async Task<List<UpdateMoneyCard>> IncreaseCardMoney()
    {
        const int amount = 30000; // monto fijo a aumentar
        const int count = 10;     // cantidad de tarjetas con menor saldo

        // Tomar las 10 tarjetas con menor saldo
        var cards = await _context.Cards
            .OrderBy(c => c.Money)
            .Take(count)
            .ToListAsync();

        if (cards == null || !cards.Any())
            return new List<UpdateMoneyCard>();

        var results = new List<UpdateMoneyCard>();

        foreach (var card in cards)
        {
            int previous = card.Money;

            // Aumentar el saldo
            card.Money += amount;

            results.Add(new UpdateMoneyCard
            {
                Card_Id = card.Card_Id,
                Previous_Balance = previous,
                New_Balance = card.Money
            });
        }

        await _context.SaveChangesAsync();

        return results;
    }
}
