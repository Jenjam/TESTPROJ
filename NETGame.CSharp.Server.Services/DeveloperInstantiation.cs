using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;

namespace NETGame.CSharp.Server.Services
{
    public class DeveloperInstantation
    {
        public List<string> GetNameAPI(float numberOfName)
        {
            List<string> lstNames = new List<string>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = httpClient.GetAsync(new Uri("https://randomuser.me/api/?results=" + numberOfName + "&exc=gender,location,email,login,registered,dob,phone,cell,id,picture,nat&noinfo$nat=FR")).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ComponentService>(responseContent.Result);
                result.Results.ForEach(n => lstNames.Add(n.Name.First));
            }
            return lstNames;
        }
        public List<string> GetNameJson(float numberOfName)
        {
            List<string> myNames = new List<string>();
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filePath = Path.Combine(appDataFolder, "names.json");
            var test = JsonConvert.DeserializeObject<ComponentService>(File.ReadAllText(filePath));
            myNames.AddRange(test.Results.Where(item => myNames.Count < numberOfName).Select(item => item.Name.First));
            return myNames;
        }


    }
}
