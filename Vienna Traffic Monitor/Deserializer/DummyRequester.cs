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
        private const String _folderPath = "Ressources\\DummyResponses\\DemoResponse";
        private const int _dummyElementCount = 15;
        private const String _fileExtension = ".txt";
        private static int _index = 0;

        /// <summary>
        /// Gibt die Dummy-Responses aus dem im folderPath spezifizierten Ordner in aufsteigender
        /// Reihenfolge zurück. Nachdem das Ende erreicht ist beginnt er wieder mit DemoResponse0
        /// Der übergebene Parameter wird ignoriert.
        /// </summary>
        /// <param name="rbl"></param>
        /// <returns></returns>
        public async Task<Response> GetResponseAsync(int rbl)
        {
            //Die egtl Methode wird in einen Task gewrappt, um Asynchronität zu erzeugen
            return await Task.Run(() =>
            {
                //Baut den String zu einer der DummyDateien zusammen
                String filePath = StrLib.StrCat(_folderPath, _fileExtension, _index.ToString());
                //Index hochzählen und Modulo der Anzahl rechnen
                _index = (_index + 1) % _dummyElementCount;
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
        public async Task<Response> GetResponseAsync(ISet<int> rblEnumerable)
        {
            return await GetResponseAsync(0);
        }

    }
}
