using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VtmFramework.Library;

namespace ViennaTrafficMonitor.Deserializer
{
    public class DummyRequester : IRequester
    {
        private const String folderPath = "Ressources\\DummyResponses\\DemoResponse";
        private const int dummyElementCount = 10;
        private const String fileExtension = ".txt";
        private Random rand = new Random();

        /// <summary>
        /// Gibt eine Dummy-Response aus dem im folderPath spezifizierten Ordner zurück.
        /// Der übergebene Parameter wird ignoriert.
        /// </summary>
        /// <param name="rbl"></param>
        /// <returns></returns>
        public async Task<Response> GetResponseAsync(int rbl)
        {
            //Die egtl Methode wird in einen Task gewrappt, um Asynchronität zu erzeugen
            return await Task.Run(() =>
            {
                int randomNumber = rand.Next(dummyElementCount);
                //Baut den String zu einer der DummyDateien zusammen
                String filePath = StrLib.StrCat(folderPath, fileExtension, randomNumber.ToString());
                //DummyDatei einlesen... 
                StreamReader fileReader = new StreamReader(filePath, System.Text.Encoding.UTF8);
                String data = fileReader.ReadToEnd();
                fileReader.Close();
                //und deserialisieren
                JavaScriptSerializer deserializer = new JavaScriptSerializer();
                Response response = deserializer.Deserialize<Response>(data);
                return response;
            });



        }

        /// <summary>
        /// Ruft die GetResponseAsync(int rbl)-Methode auf. Der übergebene Parameter wird ignoriert.
        /// </summary>
        /// <param name="rblEnumerable"></param>
        /// <returns></returns>
        public async Task<Response> GetResponseAsync(IEnumerable<int> rblEnumerable)
        {
            return await GetResponseAsync(0);
        }

    }
}
