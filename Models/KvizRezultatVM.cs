using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KvizZnanja.Models
{
    public class KvizRezultatOdgovorVM
    {
        public long ID { get; set; }
        public long KvizRezultatID { get; set; }
        public bool IsTocan { get; set; }
        public Pitanje Pitanje { get; set; }
        public Odgovor Odgovor { get; set; }
        public Odgovor TocanOdgovor { get; set; }
    }
    public class KvizRezultatVM
    {
        public KvizRezultat KvizRezultat { get; set; }
        public List<KvizRezultatOdgovorVM> Odgovori { get; set; } = new List<KvizRezultatOdgovorVM>();
        public User User { get; set; }
    }
}