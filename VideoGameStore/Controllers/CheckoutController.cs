using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using VideoGameStore.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;
using EllipticCurve.Utils;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VideoGameStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly IVideoGameStoreContext _context;

        public CheckoutController(ILogger<CheckoutController> logger, IVideoGameStoreContext context)
        {
            _logger = logger;
            _context = context;
        }

        private static readonly HttpClient httpClient = new HttpClient();
        public authResp authentication = new authResp();
        public class authResp
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
        }

        [HttpGet("fetchfedex/{postalCode}/{orderID}")]
        public async void FetchFedex(string postalCode, int orderID)
        {
            try
            {
                string apiUrl = "https://apis-sandbox.fedex.com/oauth/token"; // Replace with the actual FedEx API URL
                string apiKey = "l702a5cad1fbc94f9c8e2e9c35f6706167"; // Your FedEx API key
                string apiPassword = "bfdce51975594d88be152dba46c5e865"; // Your FedEx API password

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Headers.Add("X-locale", "en_US");


                // Build the request body with necessary details
                string jsonRequestBody = @"grant_type=client_credentials&client_id="+apiKey+@"&client_secret="+apiPassword;
                request.Content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = httpClient.Send(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    authentication = JsonConvert.DeserializeObject<authResp>(content);
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode + response.Content.ReadAsStream());
                }

                Thread.Sleep(1000);
                apiUrl = "https://apis-sandbox.fedex.com/rate/v1/rates/quotes"; // Replace with the actual FedEx API URL
                string accountNumber = "740561073 "; // Your FedEx account number

                request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Headers.Add("Authorization", "Bearer " + authentication.access_token);
                request.Headers.Add("X-locale", "en_US");


                // Build the request body with necessary details
                jsonRequestBody = @"{""accountNumber"": { ""value"": ""740561073"" },
                    ""requestedShipment"": {
                        ""shipper"": {
                            ""address"": {
                                ""postalCode"": 44499,
                                ""countryCode"": ""LT""
                            }
                        },
                        ""recipient"": {
                            ""address"": {
                                ""postalCode"": " + postalCode + @",
                                ""countryCode"": ""LT""
                            }
                        },
                        ""pickupType"": ""DROPOFF_AT_FEDEX_LOCATION"",
                        ""serviceType"": ""STANDARD_OVERNIGHT"",
                        ""rateRequestType"": [ ""LIST"", ""ACCOUNT"" ],
                        ""requestedPackageLineItems"": [{
                            ""weight"": { ""units"": ""LB"", ""value"": 1 },
                            ""dimensions"": { ""length"": 5, ""width"": 15, ""height"": 5, ""units"": ""IN"" }
                        }]
                    }
                }";
                request.Content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

                response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    //----------> Console.WriteLine(content);
                    dynamic obj = JObject.Parse(content);
                    _context.UpdateOrderShipping(orderID, float.Parse(obj.output.rateReplyDetails[0].ratedShipmentDetails[0].totalNetCharge.ToString(Formatting.None)));
                    _context.UpdateOrderAddressByPostalCode(orderID, postalCode);
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode + response.Content.ReadAsStream());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        // api/cart/:id
        [HttpGet("{username}")]
        public ActionResult<IEnumerable<User>> Get(string username)
        {
            try
            {
                var userData = _context.GetUserByUsername(username);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }

        // api/cart/:id
        [HttpPost("{orderID}")]
        public ActionResult Post(int orderID)
        {
            try
            {
                _context.UpdateOrderStatus(orderID, 2);
                _context.CreateNewBuildOrderFromOrderID(orderID);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }


        // api/cart/:id
        [HttpPost("setprice/{orderID}/{price}")]
        public ActionResult SetPrice(int orderID, float price)
        {
            try
            {
                _context.UpdateOrderPrice(orderID, price);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }

        // api/cart/:id
        [HttpPost("decreaseItem/{id}/{stock}")]
        public ActionResult DecreaseItem(int id, int stock)
        {
            try
            {
                _context.DecrementItem(id, stock);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }

        // api/cart/:id
        [HttpPost("cancelOrder/{id}")]
        public ActionResult CancelOrder(int id)
        {
            try
            {
                _context.UpdateOrderStatus(id, 6);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }

        // api/cart/:id
        [HttpPost("updateorder/{orderID}/{newStatus}")]
        public ActionResult UpdateOrder(int orderID, int newStatus)
        {
            try
            {
                _context.UpdateOrderStatus(orderID, newStatus);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getaddresses/{username}")]
        public ActionResult<IEnumerable<Address>> GetAddresses(string username)
        {
            try
            {
                var userData = _context.GetAddressesByUsername(username);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }

        public class tempAddress
        {
            public string city { get; set; }
            public string street { get; set; }
            public int building { get; set; }
            public string postalCode { get; set; }
        }

        // api/cart/:id
        [HttpPost("addaddress/{username}")]
        public ActionResult AddAddress([FromBody] tempAddress address, string username)
        {
            try
            {
                var newAddress = new Address { 
                    fk_account = username, 
                    building = address.building,
                    street = address.street,
                    postal_code = address.postalCode,
                    city = address.city,
                };
                Console.WriteLine("Hi");
                Console.WriteLine(newAddress.street);
                Console.WriteLine(newAddress.postal_code);
                _context.AddAddressToUsername(newAddress);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart items");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
