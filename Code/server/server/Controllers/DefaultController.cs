using server.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Http;

namespace server.Controllers
{
    public class DefaultController : ApiController
    {
        // POST: api/Default
        public FormattedAddress Post([FromBody]string RawAddress)
        {
            string myInputFile = Path.Combine(Path.GetTempPath(), String.Concat("INPUT","SaveFile2.txt"));
            var inputFileDeleteValue = DeleteTmpFile(myInputFile);
            File.WriteAllText(myInputFile, RawAddress);

            var p = new Process();
            //"D:\Work\DailyNew\Hexathon\LibPostalNet-master_TRY5\LibPostalConsole\bin\x64\Debug\LibPostal//Console.exe"
            //FOR RANJIT
            //p.StartInfo = new ProcessStartInfo(@"D:\Work\DailyNew\Hexathon\SumedhRepository\hexathon\server\LibPostalConsole\bin\x64\Debug\LibPostalConsole.exe", "-n")
            p.StartInfo = new ProcessStartInfo(@"D:\Work\DailyNew\Hexathon\LibPostalNet-master_TRY5\LibPostalConsole\bin\x64\Debug\LibPostalConsole.exe", "-n")
            {
                UseShellExecute = false
            };

            p.Start();
            p.WaitForExit();
            //p.Kill();
            string myTempFile = Path.Combine(Path.GetTempPath(), "SaveFile2.txt");
            IFormatter formatter = new BinaryFormatter();
            Dictionary<string, string> ydict2;
            using (var stream = new FileStream(myTempFile, FileMode.Open, FileAccess.Read))
            {
                ydict2 = (Dictionary<string, string>)formatter.Deserialize(stream);
            }

            FormattedAddress lobjFormattedAddress = new FormattedAddress()
            {
                InputAddress = RawAddress,
                House = ydict2.Where(y => y.Key == "House").Select(g => g.Value).First(),
                Unit = ydict2.Where(y => y.Key == "Unit").Select(g => g.Value).First(),
                State = ydict2.Where(y => y.Key == "State").Select(g => g.Value).First(),
                City = ydict2.Where(y => y.Key == "City").Select(g => g.Value).First(),
                StreetName = ydict2.Where(y => y.Key == "StreetName").Select(g => g.Value).First(),
                Country = ydict2.Where(y => y.Key == "Country").Select(g => g.Value).First(),
                PinCode = Convert.ToInt32(ydict2.Where(y => y.Key == "PinCode").Select(g => g.Value).First()),
            };

            return lobjFormattedAddress;
        }

        // GET: api/Default
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Default/5
        public string Get(int id)
        {
            return "value";
        }

        private bool DeleteTmpFile(string tmpFile)
        {

            bool lblnReturnValue = false;
            try
            {

                // Delete the temp file (if it exists)
                if (File.Exists(tmpFile))
                {
                    File.Delete(tmpFile);
                    //Console.WriteLine("TEMP file deleted.");
                }
                lblnReturnValue = true;
            }
            catch (Exception ex)
            {

                //Console.WriteLine(ex.Message);

                lblnReturnValue = false;
                //Console.WriteLine("Temp directory is full.  Please clear your temp directory" + Path.GetTempPath() + "-" +ex.Message);
            }
            return lblnReturnValue;
        }

        // PUT: api/Default/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Default/5
        public void Delete(int id)
        {
        }
    }
}
