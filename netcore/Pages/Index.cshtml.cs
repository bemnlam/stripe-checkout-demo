using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace stripe_checkout_demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;

        public string PriceId { get; private set; }
        public string PublicKey { get; private set; }
        public string CheckoutSessionId { get; private set; }
        public Stripe.Price Price { get; private set; }

        public IndexModel(IConfiguration configuration, ILogger<IndexModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
            Stripe.StripeConfiguration.ApiKey = _configuration.GetValue<string>("Stripe:SecretKey");
            PublicKey = _configuration.GetValue<string>("Stripe:PublicKey");
        }

        public async Task OnGet(string stripePriceId)
        {
            var service = new Stripe.Checkout.SessionService();

            PriceId = !string.IsNullOrWhiteSpace(stripePriceId) ? stripePriceId : _configuration.GetValue<string>("Stripe:PriceId");

            var options = this._createOptions(PriceId);
            var session = await service.CreateAsync(options);

            this.CheckoutSessionId = session.Id;
            this.Price = await _getPriceData(PriceId);
        }

        private async Task<Stripe.Price> _getPriceData(string stripePriceId)
        {
            var priceService = new Stripe.PriceService();

            var options = new Stripe.PriceGetOptions();
            options.AddExpand("product");

            return await priceService.GetAsync(stripePriceId, options);
        }

        private Stripe.Checkout.SessionCreateOptions _createOptions(string stripePriceId)
        {
            return new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        Price = stripePriceId,
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"https://localhost:5001/success/?stirpePriceId={ stripePriceId }",
                CancelUrl = $"https://localhost:5001/?stirpePriceId={ stripePriceId }",
                PaymentMethodTypes = new List<string> { "card" }
            };
        }
    }
}
