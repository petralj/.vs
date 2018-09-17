using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KvizZnanja.Models
{
    public class KvizPitanjaInsertData
    {
        public long KvizID { get; set; }
        public string NazivKviza { get; set; }
        public int Razina { get; set; }
        public List<long> PitanjaIDs { get; set; }
    }
}