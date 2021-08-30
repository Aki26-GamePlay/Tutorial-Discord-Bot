using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ExampleBot
{
    public class MiscModule : ModuleBase<SocketCommandContext>
    {
        public object RequestOptions { get; private set; }

        [Command("Ping")]
        [Alias("P")]
        public async Task Ping()
        {
            var builder = new EmbedBuilder();
            _ = builder.WithFooter(Program.MainFooter);
            _ = builder.WithCurrentTimestamp();
            _ = builder.WithAuthor(Context.Message.Author);
            _ = builder.WithTitle("RangerBot • Ping");
            _ = builder.Description = ":ping_pong: Pong!";

            if (Context.Client.Latency < 100)
                _ = builder.Color = Color.Green;
            if (Context.Client.Latency > 100)
                _ = builder.Color = Color.DarkGreen;
            if (Context.Client.Latency > 200)
                _ = builder.Color = Color.Gold;
            if (Context.Client.Latency > 300)
                _ = builder.Color = Color.Red;
            if (Context.Client.Latency > 400)
                _ = builder.Color = Color.DarkRed;
            if (Context.Client.Latency > 500)
                _ = builder.Color = Color.Default;

            builder.AddField(x =>
            {
                x.Name = "A Bot pingje:";
                x.Value = Context.Client.Latency + "ms";
            });
            await ReplyAsync("", false, builder.Build()).ConfigureAwait(false);

            Run.Log(Context);
        }
        [Command("BotInfo")]
        [Alias("BI")]
        public async Task BotInfo()
        {
            System.TimeSpan UpTimeGet = DateTime.Now - Process.GetCurrentProcess().StartTime;
            string UpTime = null;
            if (UpTimeGet.Days != 0)
                UpTime += UpTimeGet.Days + " nap, ";
            if (UpTimeGet.Hours != 0)
                UpTime += UpTimeGet.Hours + " óra, ";
            UpTime += UpTimeGet.Minutes + " perc, ";
            UpTime += UpTimeGet.Seconds + " másodperc";

            var builder = new EmbedBuilder();
            _ = builder.WithFooter(Program.MainFooter);
            _ = builder.WithCurrentTimestamp();
            _ = builder.WithAuthor(Context.Message.Author);
            _ = builder.WithTitle("RangerBot • BotInfo");
            _ = builder.WithThumbnailUrl("https://cdn.discordapp.com/avatars/" + Context.Client.CurrentUser.Id + "/" + Context.Client.CurrentUser.AvatarId + ".png?size=1024");
            _ = builder.WithColor(Color.Blue);

            builder.AddField("Serverek Száma", Context.Client.Guilds.Count + " Szerver");
            builder.AddField("Ping", Context.Client.Latency + "ms");
            builder.AddField("UpTime", UpTime);

            await ReplyAsync("", false, builder.Build()).ConfigureAwait(false);

            Run.Log(Context);
        }

        //Info Commands
        [Command("ServerInfo")]
        [Alias("SI")]
        public async Task Serverinfo()
        {
            var bot = Context.Guild.GetUser(712588281769885736);

            var builder = new EmbedBuilder();
            _ = builder.WithFooter(Program.MainFooter);
            _ = builder.WithCurrentTimestamp();
            _ = builder.WithAuthor(Context.Message.Author);
            _ = builder.WithTitle("RangerBot • ServerInfo");
            _ = builder.WithThumbnailUrl("https://cdn.discordapp.com/icons/" + Context.Guild.Id + "/" + Context.Guild.IconId + ".png?size=1024");
            _ = builder.WithColor(Color.Blue);

            builder.AddField("Név • ID", Context.Guild.Name + " • " + Context.Guild.Id);
            builder.AddField("Tulajdonos • Tulajdonos ID", Context.Guild.Owner + " • " + Context.Guild.OwnerId);
            builder.AddField("Általános Csatorna • Általános Csatorna ID", Context.Guild.DefaultChannel + " • " + Context.Guild.DefaultChannel.Id);
            builder.AddField("Icon", "[Kattints ide!](https://cdn.discordapp.com/icons/" + Context.Guild.Id + "/" + Context.Guild.IconId + ".png?size=1024)");
            builder.AddField("Létrehozva", $"{Context.Guild.CreatedAt.Year}. {Context.Guild.CreatedAt.Month}. {Context.Guild.CreatedAt.Day}. {Context.Guild.CreatedAt.Hour}:{Context.Guild.CreatedAt.Minute}");
            builder.AddField("Csatornák száma • Rangok száma • Tagok száma • Emojik száma", Context.Guild.Channels.Count + " • " + Context.Guild.Roles.Count + " • " + Context.Guild.MemberCount + " • " + Context.Guild.Emotes.Count);
            //builder.AddField("Rangok", string.Join(", ", Context.Guild.Roles));
            //builder.AddField("Csatornák", string.Join(", ", Context.Guild.Channels));
            // TÚL HOSSZÚ builder.AddField("Emojik", string.Join(", ", Context.Guild.Emotes));
            // BUGOS builder.AddField("Tagok", string.Join(", ", Context.Guild.Users));
            builder.AddField("Előfizetett felhasználók száma", Context.Guild.PremiumSubscriptionCount);
            builder.AddField("Boost szint", Context.Guild.PremiumTier);
            builder.AddField("Régió", Context.Guild.VoiceRegionId);
            builder.AddField("AFK Csatorna • AFK Időkorlát", Context.Guild.AFKChannel + " • " + Context.Guild.AFKTimeout);
            if (bot.GuildPermissions.Administrator)
                builder.AddField("A Bot jogosultsági szintje", "Administrator");

            //IGuild guild = await Context.Client.GetGuildAsync(712944899187671112).ConfigureAwait(false);
            //string ServerIcon = $"{guild.IconUrl}";
            //builder.WithThumbnailUrl(ServerIcon);

            Run.Log(Context);

            await ReplyAsync("", false, builder.Build()).ConfigureAwait(false);
        }
        [Command("UserInfo")]
        [Alias("UI")]
        public async Task UserInfo(SocketUser userName_)
        {
            SocketGuildUser userName = userName_ as SocketGuildUser;

            var builder = new EmbedBuilder();
            _ = builder.WithFooter(Program.MainFooter);
            _ = builder.WithCurrentTimestamp();
            _ = builder.WithAuthor(Context.Message.Author);
            _ = builder.WithTitle("RangerBot • UserInfo");
            _ = builder.WithThumbnailUrl("https://cdn.discordapp.com/avatars/" + userName.Id + "/" + userName.AvatarId + ".png?size=1024");
            _ = builder.WithColor(Color.Blue);
            builder.AddField("Felhasználó", userName);
            builder.AddField("Felhasználó ID", userName.Id);
            builder.AddField("Említés", userName.Mention);
            if (userName.Nickname != null)
                builder.AddField("Becenév", userName.Nickname);
            builder.AddField("Státusz", userName.Status);
            if (userName.Activity != null)
                builder.AddField("Activity", userName.Activity);

            builder.AddField("Profilkép", "[[Letöltés]](https://cdn.discordapp.com/avatars/" + userName.Id + "/" + userName.AvatarId + ".png?size=1024)");
            builder.AddField($"Rangok ({userName.Roles.Count})", string.Join(", ", userName.Roles.Select(x => x.Mention)));
            //builder.AddField("Egyéb szerverek, amin a RangerBot is bent van", string.Join(", ", userName.MutualGuilds.Select(x => x.Name)));
            //builder.AddField("Készítve", $"{userName.CreatedAt.Year}. {userName.CreatedAt.Month}. {userName.CreatedAt.Day}. {userName.CreatedAt.Hour}:{userName.CreatedAt.Minute}");
            builder.AddField("Készítve", userName.CreatedAt);
            builder.AddField("Csatlakozva", userName.JoinedAt);
            if (userName.IsBot)
                builder.AddField("Bot?", "Igen");
            if (userName.IsWebhook)
                builder.AddField("Webhook?", "Igen");
            if (userName.IsMuted)
                builder.AddField("Némítva?", "Igen");
            if (userName.IsDeafened)
                builder.AddField("Süketítve?", "Igen");
            if (userName.IsSelfMuted)
                builder.AddField("ÖnNémítva?", "Igen");
            if (userName.IsSelfDeafened)
                builder.AddField("ÖnSüketítve?", "Igen");
            if (userName.IsStreaming)
                builder.AddField("Streamel?", "Igen");

            await ReplyAsync("", false, builder.Build()).ConfigureAwait(false);

            Run.Log(Context);
        }
        [Command("Avatar")]
        [Alias("A")]
        public async Task Avatar(SocketUser userName)
        {
            var builder = new EmbedBuilder();
            _ = builder.WithFooter(Program.MainFooter);
            _ = builder.WithCurrentTimestamp();
            _ = builder.WithAuthor(Context.Message.Author);
            _ = builder.WithTitle("RangerBot • Avatar");
            _ = builder.WithDescription("[[Letöltés]](https://cdn.discordapp.com/avatars/" + userName.Id + "/" + userName.AvatarId + ".png?size=2048)");
            _ = builder.WithImageUrl("https://cdn.discordapp.com/avatars/" + userName.Id + "/" + userName.AvatarId + ".png?size=1024");
            _ = builder.WithColor(Color.Blue);

            await ReplyAsync("", false, builder.Build()).ConfigureAwait(false);

            Run.Log(Context);
        }
    }
}