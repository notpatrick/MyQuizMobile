using System;
using System.Collections.Generic;
using System.Text;

namespace MyQuizMobile
{
    public class BeantworteteFrage
    {
        public Veranstaltung Veranstaltung { get; set; }
        public Person Person { get; set; }
        public Frage Frage { get; set; }
        public Antwort Antwort { get; set; }
    }
}
