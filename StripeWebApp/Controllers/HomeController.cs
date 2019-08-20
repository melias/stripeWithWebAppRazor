using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripeWebApp.Data;
using StripeWebApp.Models;

namespace StripeWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Charge(string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();
            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = 500,
                Description = "Test Payment",
                Currency = "eur",
                CustomerId = customer.Id,
                ReceiptEmail = stripeEmail,
                Metadata = new System.Collections.Generic.Dictionary<string, string>()
                {
                    { "OrderId", "111" },
                    { "Postcode", "LEE111" }
                }
            });
            if (charge.Status == "succeeded")
            {
                var balanceTransactionId = charge.BalanceTransactionId;
                return View("Index", new PaymentReturn { Succeeded = true });
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}