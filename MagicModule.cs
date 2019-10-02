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
        private Random _rng;
        private readonly List<Set> _sets;

        public MagicModule(List<Set> sets, Random rng)
        {
            _sets = sets;
            _rng = rng;
        }

        [Command("gen")]
        [Summary("Generates a random standard environment.")]
        public async Task GenerateAsync()
        {
            List<Set> selected = new List<Set>();

            List<Set> coreSets = _sets.Where(set => set.Type == Set.SetType.Core).ToList();
            List<Set> legalSets = _sets.Where(set => set.Type == Set.SetType.Expansion).ToList();

            for (int i = 0; i < 6; i++)
            {
                Set set;
                do
                {
                    set = legalSets[_rng.Next(legalSets.Count)];
                } while (selected.Contains(set));

                selected.Add(set);
            }

            selected.Add(coreSets[_rng.Next(coreSets.Count)]);

            string msg = "```\nRando Standard Sets:\n";

            foreach(Set set in selected)
            {
                msg += $"\n{set.Name} ({set.Code})";
            }

            msg += "\n```";
            await ReplyAsync(msg);
        }
    }
}
