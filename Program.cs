using System;
using System.Threading.Tasks;
using Discord;
using System.Collections.Generic;

namespace RandoStandardBot
{
    public class Program
    {
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            List<Set> sets = GetSets();
            foreach(Set set in sets)
            {
                Console.WriteLine($"{set.Name}, {set.ReleaseDate}, {set.Code}, {set.Type}");
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private List<Set> GetSets()
        {
            List<Set> sets = new List<Set>();
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"..\..\..\Sets.txt");
            while((line = file.ReadLine()) != null)
            {
                string[] tokens = line.Split('\t');
                Set set = new Set(DateTime.Parse(tokens[0]), tokens[1], tokens[3], Set.SetType.Box, tokens[5]);
                set.Type = (tokens[4]) switch
                {
                    "Supplemental set " => Set.SetType.Supplemental,
                    "Box set " => Set.SetType.Box,
                    "Expansion set " => Set.SetType.Expansion,
                    "Core set " => Set.SetType.Core,
                    "Compilation set " => Set.SetType.Compilation,
                    "Special Edition " => Set.SetType.Special_Edition,
                    "Starter " => Set.SetType.Starter,
                    _ => Set.SetType.Un,
                };
                Console.WriteLine(tokens[4]);
                sets.Add(set);
            }

            return sets;
        }
    }
}
