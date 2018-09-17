using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KvizZnanja.Models
{
    public class PitanjaSaOdgovorima
    {
        public Pitanje Pitanje { get; set; }
        public List<Odgovor> Odgovori { get; set; } = new List<Odgovor>();
    }

    public class ZapocetiKvizData
    {
        public Kviz Kviz { get; set; }
        public List<PitanjaSaOdgovorima> Data { get; set; } = new List<PitanjaSaOdgovorima>();
    }
}