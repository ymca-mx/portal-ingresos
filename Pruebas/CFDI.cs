using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Pruebas
{
    [TestClass]
    public class CFDI
    {
        HttpClient client = new HttpClient();

        [TestMethod]
        public async Task PruebasApi()
        {
            client.BaseAddress = new Uri("http://localhost:44314/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            object ComplementoEducativo = new
            {
                alumnoId = 803,
                nombre = "Demetrio Valdez Alfaro",
                curp = "acvbsdfg123456789"
            };


            string obj = await UpdateCFDI(ComplementoEducativo);
            JObject o = JObject.Parse(obj);


            Console.WriteLine("Termine");
            Console.WriteLine("Resultado..." + (string)o["message"]);
        }

        async Task<string> UpdateCFDI(object ComplementoEducativo)
        {
            HttpResponseMessage obj = await client.PostAsJsonAsync(
                "api/Universal/EditComplementoEducativo", ComplementoEducativo);

            return obj.Content.ReadAsStringAsync().Result;
        }
    }
}
