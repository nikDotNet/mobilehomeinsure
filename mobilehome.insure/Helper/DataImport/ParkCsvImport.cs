using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper;
using System.IO;

namespace mobilehome.insure.Helper.DataImport
{
    public class ParkCsvImport
    {
        public void ParkImport(string Path)
        {
            TextReader textReader = new StreamReader(Path);
            var csv = new CsvReader(textReader);


        }
    }
}