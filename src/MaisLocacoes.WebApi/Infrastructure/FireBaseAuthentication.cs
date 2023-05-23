using Newtonsoft.Json;
using System.Text;

namespace MaisLocacoes.WebApi.Infrastructure
{
    public class FireBaseAuthentication
    {
        public static async Task<bool> IsFirebaseTokenValid(string token)
        {
            try
            {
                var appSettingsPath = "appsettings.json";
                var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(appSettingsPath, optional: true, reloadOnChange: true);
                var configuration = configurationBuilder.Build();

                var connectionString = configuration["GoogleApisConnection:ValidateTokenConnectionString"];
                var _firebaseApiKey = configuration["GoogleApisConnection:FireBaseApiKey"];

                var requestUri = string.Concat(connectionString, _firebaseApiKey);

                var client = new HttpClient();

                var content = new StringContent($"{{\"idToken\":\"{token}\"}}", Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    dynamic responseJson = JsonConvert.DeserializeObject(responseContent);
                    if (responseJson.users != null && responseJson.users.Count > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}