using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace stripe_checkout_demo.Pages
{
    public class SuccessModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SuccessModel> _logger;

        public SuccessModel(IConfiguration configuration, ILogger<SuccessModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
            Stripe.StripeConfiguration.ApiKey = _configuration.GetValue<string>("Stripe:SecretKey");
        }

        public Stripe.Charge RecentCharge { get; private set; }

        public void OnGet()
        {
            var service = new Stripe.ChargeService();

            RecentCharge = service.ListAutoPaging(new Stripe.ChargeListOptions
            {
                Limit = 1,
            }).FirstOrDefault();
        }
    }
}
