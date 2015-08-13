using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobilehome.insure.Areas.Admin.Models
{
    public class ImportCsvStatus
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public int CountDuplicates { get; set; }

        public string Message { get; set; }

        public string ClassName { get; set; }
    }
}