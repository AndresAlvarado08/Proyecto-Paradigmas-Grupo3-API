using Bogus;
using System.Text;
using Cards_Products_API.Data; // Ajusta namespace
using Cards_Products_API.Models;
using Cards_Products_API.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Card> CreateRandomCard(double probabilityExpired = 0.35, bool formatWithSpaces = true)
    {
        var cardTypes = new[] { "Visa", "MasterCard", "American Express", "Discover" };
        var faker = new Faker();

        var cardType = faker.PickRandom(cardTypes);
        var rawNumber = GenerateCardNumberByType(cardType, faker); // sin formato
        var formattedNumber = formatWithSpaces ? FormatCardNumber(rawNumber, cardType) : rawNumber;

        var moneyValue = faker.Finance.Random.Int(500, 20000);
        var expired = faker.Random.Double() < probabilityExpired; // true => generar vencida
        var expiration = GenerateExpirationDate(expired, faker);

        var card = new Card
        {
            Card_Type = cardType,
            Card_Number = formattedNumber,
            User_Id = 1,        // Cambiar a relacion en el futuro
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
            bool chooseOldRange = faker.Random.Bool(); // 50/50, se puede ajustar
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
            // AmEx: 34 o 37, longitud 15
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
    private DateTime GenerateExpirationDate(bool expired, Faker faker)
    {
        if (expired)
        {
            // Fecha pasada: entre 1 mes y 5 años en el pasado
            // Elegimos año y mes pasado
            var past = faker.Date.Past(5);
            int year = past.Year;
            int month = past.Month;
            var lastDay = DateTime.DaysInMonth(year, month);
            return new DateTime(year, month, lastDay, 23, 59, 59, DateTimeKind.Utc);
        }
        else
        {
            // Futuro: entre 1 mes y 5 años en el futuro
            var future = faker.Date.Future(5, DateTime.UtcNow.AddMonths(1));
            int year = future.Year;
            int month = future.Month;
            var lastDay = DateTime.DaysInMonth(year, month);
            return new DateTime(year, month, lastDay, 23, 59, 59, DateTimeKind.Utc);
        }
    }
}
