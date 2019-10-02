using System;
using System.Collections.Generic;
using System.Text;

namespace RandoStandardBot
{
    public class Set
    {
        public enum SetType { Expansion, Box, Supplemental, Compilation, Core, Special_Edition, Starter, Un };

        public DateTime ReleaseDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public SetType Type { get; set; }
        public string Notes { get; set; }

        public Set(DateTime date, string name, string code, SetType type, string notes)
        {
            ReleaseDate = date;
            Name = name;
            Code = code;
            Type = type;
            Notes = notes;
        }
    }
}
