using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using System.Linq;

namespace RandoStandardBot
{
    public class MagicModule : ModuleBase<SocketCommandContext>
    {
        private static Random rng = new Random();

        [Command("gen")]
        [Summary("Generates a random standard environment.")]
        public async Task GenerateAsync()
        {
            List<Set> sets = GetSets();
            List<Set> selected = new List<Set>();

            List<Set> coreSets = sets.Where(set => set.Type == Set.SetType.Core).ToList();
            List<Set> legalSets = sets.Where(set => set.Type == Set.SetType.Expansion).ToList();

            for (int i = 0; i < 6; i++)
            {
                Set set;
                do
                {
                    set = legalSets[rng.Next(legalSets.Count)];
                } while (selected.Contains(set));

                selected.Add(set);
            }

            selected.Add(coreSets[rng.Next(coreSets.Count)]);

            string msg = "```\nRando Standard Sets:\n";

            foreach(Set set in selected)
            {
                msg += $"\n{set.Name} ({set.Code})";
            }

            msg += "\n```";
            await ReplyAsync(msg);
        }

        private List<Set> GetSets()
        {
            List<Set> sets = new List<Set>();
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"..\..\..\Sets.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] tokens = line.Split('\t');
                Set set = new Set(DateTime.Parse(tokens[0].Trim(' ')), tokens[1].Trim(' '), tokens[3].Trim(' '), Set.SetType.Box, tokens[5].Trim(' '));
                set.Type = (tokens[4].Trim(' ')) switch
                {
                    "Supplemental set" => Set.SetType.Supplemental,
                    "Box set" => Set.SetType.Box,
                    "Expansion set" => Set.SetType.Expansion,
                    "Core set" => Set.SetType.Core,
                    "Compilation set" => Set.SetType.Compilation,
                    "Special Edition" => Set.SetType.Special_Edition,
                    "Starter" => Set.SetType.Starter,
                    _ => Set.SetType.Un,
                };
                sets.Add(set);
            }

            return sets;
        }
    }
}
