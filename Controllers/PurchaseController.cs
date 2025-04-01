using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using System.Text.Json;

using TicketHubApi.Models;

namespace TicketHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Purchase purchase)
        {
            if (purchase == null)
                return BadRequest("Payload is missing.");

            var errors = new List<string>();

            if (purchase.ConcertId <= 0)
                errors.Add("ConcertId must be greater than 0.");
            if (string.IsNullOrWhiteSpace(purchase.Email) || !purchase.Email.Contains("@"))
                errors.Add("A valid Email is required.");
            if (string.IsNullOrWhiteSpace(purchase.Name))
                errors.Add("Name is required.");
            if (string.IsNullOrWhiteSpace(purchase.Phone))
                errors.Add("Phone is required.");
            if (purchase.Quantity <= 0)
                errors.Add("Quantity must be greater than 0.");
            if (string.IsNullOrWhiteSpace(purchase.CreditCard))
                errors.Add("CreditCard is required.");
            if (string.IsNullOrWhiteSpace(purchase.Expiration))
                errors.Add("Expiration is required.");
            if (string.IsNullOrWhiteSpace(purchase.SecurityCode))
                errors.Add("SecurityCode is required.");
            if (string.IsNullOrWhiteSpace(purchase.Address))
                errors.Add("Address is required.");
            if (string.IsNullOrWhiteSpace(purchase.City))
                errors.Add("City is required.");
            if (string.IsNullOrWhiteSpace(purchase.Province))
                errors.Add("Province is required.");
            if (string.IsNullOrWhiteSpace(purchase.PostalCode))
                errors.Add("PostalCode is required.");
            if (string.IsNullOrWhiteSpace(purchase.Country))
                errors.Add("Country is required.");

            if (errors.Any())
                return BadRequest(string.Join(" ", errors));

            // Send to Azure Queue
            var connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            var queueClient = new QueueClient(connectionString, "tickethub");
            await queueClient.CreateIfNotExistsAsync();

            string jsonMessage = JsonSerializer.Serialize(purchase);
            await queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonMessage)));

            return Ok("Purchase accepted and queued.");
        }
    }
}