using Cards_Products_API.Services;
using Cards_Products_API.Interfaces;
using Quartz;

namespace Cards_Products_API.Jobs
{
    public class GenerateDataJob : IJob
    {
        private readonly ICardService _cardService;
        private readonly IProductService _productService;

        public GenerateDataJob(ICardService cardService, IProductService productService)
        {
            _cardService = cardService;
            _productService = productService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Ejecutando GenerateDataJob...");

            // Generar 5 tarjetas
            for (int i = 0; i < 5; i++)
            {
                var card = await _cardService.CreateRandomCard();
                Console.WriteLine($"Tarjeta creada: {card.Card_Type} - {card.Card_Number}");
            }

            // Generar 10 productos
            for (int i = 0; i < 10; i++)
            {
                var product = await _productService.CreateRandomProducts();
                Console.WriteLine($"Producto creado: {product.Product_Name} - ${product.Price}");
            }

            Console.WriteLine("Finalizó GenerateDataJob correctamente.\n\n\n");
        }
    }
}