using Dashboard.Models.DTO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Dashboard.Utillities.Helper
{
    public static class GenralPurpose
    {
        //static async Task Main(string[] args)
        //{
        //    var result = await SendPostRequestAsync();
        //    await SendGetRequestAsync(result);
        //}


        //For Getting key for Authentication
        public static async Task<string> SendPostRequestAsync()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);

            // Set the base address
            client.BaseAddress = new Uri("https://104.219.233.15:8443/");

            // Set the authentication credentials
            var byteArray = Encoding.ASCII.GetBytes("admin:7a24De3j#ajs,aaa9i8j");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Prepare the content (empty JSON object in this case)
            string jsonContent = "{}";
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the POST request
            HttpResponseMessage response = await client.PostAsync("api/v2/auth/keys", content);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();


                int equalsSignIndex = responseContent.IndexOf(':');

                // Get the value part of the key-value pair.
                string value = responseContent.Substring(equalsSignIndex + 1);

                // Remove any extra quotes from the value.
                value = value.Replace("\"", "");
                value = value.Trim();
                value = value.Trim('\"');
                value = value.Trim('}');
                value = value.Trim('\n');


                Console.WriteLine(value);
                return value;

                // var key = responseContent.Split(':');
                // Console.WriteLine("Response: " + key[1]);
                // return responseContent.Trim('"');
            }
            else
            {
                Console.WriteLine("Error: " + response.ReasonPhrase);
                return string.Empty;
            }


        }

        // For Getting List of All Domains 
        public static async Task<string> SendGetRequestAsync(string apiKey)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using HttpClient client = new HttpClient(clientHandler);

            // Set the base address and include the API key in the header
            client.BaseAddress = new Uri("https://104.219.233.15:8443/");
            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            //client.DefaultRequestHeaders.Add ("X - API - Key: ",apiKey);
            client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);



            // Send the GET request
            HttpResponseMessage response = await client.GetAsync("api/v2/domains");

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
                //Console.WriteLine("Response: " + responseContent);
            }
            else
            {
                return "Error: " + response.ReasonPhrase;
            }
        }

        // For Creating Domain
        public static async Task<string> SendPostDomainCreateRequestAsync(string apiKey, string name)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);

            // Set the base address
            client.BaseAddress = new Uri("https://104.219.233.15:8443/");
            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);

            var obj = new Domain();

            obj.name = name + ".navedge.co";
            obj.hosting_settings.ftp_login = name + "_ftp";
            var test = JsonSerializer.Serialize(obj);


            // Prepare the content 
            var content = new StringContent(test, Encoding.UTF8, "application/json");


            // Send the POST request
            HttpResponseMessage response = await client.PostAsync("api/v2/domains", content);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return response.StatusCode.ToString();
            }
            else
            {
                Console.WriteLine("Error: " + response.ReasonPhrase);
                return string.Empty;
            }

        }

        // For Creating SubDomain
        public static async Task<string> SendPostSubDomainCreateRequestAsync(string apiKey, string name)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using HttpClient client = new HttpClient(clientHandler);

            // Set the base address
            client.BaseAddress = new Uri("https://104.219.233.15:8443/");

            //client.DefaultRequestHeaders.Add("accept", "application/json");
            client.DefaultRequestHeaders.Add("X-API-Key", apiKey);


            var obj = new SubDomain();

            obj.@params.Insert(1, name);
            obj.@params.Insert(5, name+".navedge.co");


            var test = JsonSerializer.Serialize(obj);
            Console.WriteLine(test);
            

            var content = new StringContent(test, Encoding.UTF8, "application/json");

            // Send the POST request
            HttpResponseMessage response = await client.PostAsync("api/v2/cli/subdomain/call", content);

            // Check the response

            string responseContent = await response.Content.ReadAsStringAsync();
            //var obj2 = new responseObj();
            var obj2 = JsonSerializer.Deserialize<ResponseObj>(responseContent);
            if (obj2?.code != 0)
            {
                Console.WriteLine("Error: " + obj2?.stdout);
                return string.Empty;
            }
            else
            {
                Console.WriteLine("Created");
                return "Created";
            }

        }
    }
}
