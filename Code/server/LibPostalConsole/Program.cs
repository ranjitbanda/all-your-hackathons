using LibPostalNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibPostalConsole
{
    [Serializable]
    internal class Program
    {
        /// <summary>
        /// An example of using the LibPostalNet library
        /// </summary>
        /// <param name="args">args[0] should be the path to libpostal data files</param>
        private static void Main(string[] args)
        {
            //if (args.Length > 0)
            //{
                //string dataPath = args[0];
                string dataPath = "..\\..\\..\\..\\libpostal";


                libpostal.LibpostalSetupDatadir(dataPath);
                libpostal.LibpostalSetupParserDatadir(dataPath);
                libpostal.LibpostalSetupLanguageClassifierDatadir(dataPath);
            //}
            //else
            //{
            //    libpostal.LibpostalSetup();
            //    libpostal.LibpostalSetupParser();
            //    libpostal.LibpostalSetupLanguageClassifier();
            //}

            //string myInputFile = Path.Combine(Path.GetTempPath(), String.Concat("INPUT", "SaveFile2.txt"));

            string query = string.Empty;
            //string query = File.ReadAllText(myInputFile);
            if (string.IsNullOrWhiteSpace(query))
                query = "hexagon capability Centre India Private Limited ground 9 10 and 11 floors divyasree Trinity campus plot number 5 Hitech City Madhapur Hyderabad Telangana 500081";

            LibpostalAddressParserResponse response = libpostal.LibpostalParseAddress(query, new LibpostalAddressParserOptions());

            List<KeyValuePair<string, string>> x = response.Results;

            string House = x.Where(y => y.Key == "house").Select(g => g.Value).FirstOrDefault();
            string State = x.Where(y => y.Key == "state").Select(g => g.Value).FirstOrDefault();
            string City = x.Where(y => y.Key == "city").Select(g => g.Value).FirstOrDefault();
            string StreetName = x.Where(y => y.Key == "road").Select(g => g.Value).FirstOrDefault();
            string PinCode = x.Where(y => y.Key == "postcode").Select(g => g.Value).FirstOrDefault();
            string Unit = x.Where(y => y.Key == "unit").Select(g => g.Value).FirstOrDefault();
            string Country = x.Where(y => y.Key == "country").Select(g => g.Value).FirstOrDefault();

            LogInConsole(string.Concat("Original Input -", query));
            LogInConsole(Environment.NewLine);
            LogInConsole(Environment.NewLine);
            LogInConsole(Environment.NewLine);


            Dictionary<string, string> ydict = new Dictionary<string, string>
            {
                {"House", House },
                {"State", State },
                {"City", City },
                {"StreetName", StreetName },
                {"PinCode", PinCode },
                {"Unit", Unit },
                {"Country", Country }
            };

            foreach (var item in ydict)
            {
                LogInConsole(string.Concat("Key -", item.Key, ", Value -", item.Value));
            }

            IFormatter formatter = new BinaryFormatter();
            string myTempFile = Path.Combine(Path.GetTempPath(), "SaveFile2.txt");
            //LogInConsole(myTempFile);
            if (DeleteTmpFile(myTempFile))
            {
                //LogInConsole("If loop of DeleteTmpFile");

                using (Stream stream = new FileStream(myTempFile, FileMode.Create, FileAccess.Write))
                {
                    formatter.Serialize(stream, ydict);
                }

            }

            libpostal.LibpostalAddressParserResponseDestroy(response);

            // Teardown (only called once at the end of your program)
            libpostal.LibpostalTeardown();
            libpostal.LibpostalTeardownParser();
            libpostal.LibpostalTeardownLanguageClassifier();
            Console.ReadKey();
        }

        private static void LogInConsole(string tmpFile)
        {
            Console.WriteLine(tmpFile);
        }

        private static bool DeleteTmpFile(string tmpFile)
        {

            bool lblnReturnValue = false;
            try
            {

                // Delete the temp file (if it exists)
                if (File.Exists(tmpFile))
                {
                    //LogInConsole("File.Exists(tmpFile) pass");

                    File.Delete(tmpFile);
                    //Console.WriteLine("TEMP file deleted.");
                }
                //LogInConsole("File.Exists(tmpFile) fail");

                lblnReturnValue = true;
            }
            catch (Exception ex)
            {
                //LogInConsole("File.Delete(tmpFile) exception");

                //Console.WriteLine(ex.Message);

                lblnReturnValue = false;
                //Console.WriteLine("Temp directory is full.  Please clear your temp directory" + Path.GetTempPath() + "-" +ex.Message);
            }
            return lblnReturnValue;
        }
    }
}
