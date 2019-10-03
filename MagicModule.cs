﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Linq;

namespace RandoStandardBot
{
    public class MagicModule : ModuleBase<SocketCommandContext>
    {
        private readonly Random _rng;
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

            await ReplyAsync("", false, FormulateEmbed(selected));

        }

        private Embed FormulateEmbed(List<Set> selected)
        {
            var embed = new EmbedBuilder
            {
                Title = "Rando Standard Sets",
                Description = "Time to get brewing!",
            };

            var field1 = new EmbedFieldBuilder
            {
                Name = "Expansions"
            };

            foreach (Set set in selected)
            {
                if (set.Type == Set.SetType.Expansion)
                    field1.Value += $"[{set.Name} ({set.Code})]({$"\nhttps://www.mythicspoiler.com/{set.Code.Substring(0, 3).ToLower()}/index.html"})\n";
            }
            embed.AddField(field1);


            var field2 = new EmbedFieldBuilder
            {
                Name = "Core Set"
            };
            foreach (Set set in selected)
            {
                if (set.Type == Set.SetType.Core)
                    field2.Value += $"[{set.Name} ({set.Code})]({$"\nhttps://www.mythicspoiler.com/{set.Code.Substring(0, 3).ToLower()}/index.html"})\n";
            }
            embed.AddField(field2);

            return embed.Build();
        }
    }
}
