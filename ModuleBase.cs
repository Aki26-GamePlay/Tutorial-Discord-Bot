using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleBot.Common
{
    public abstract class MonkeyModuleBase : ModuleBase
    {
        protected async Task ReplyAndDeleteAsync(string message = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, int delayMS = 5000)
        {
            IUserMessage msg = await Context.Channel.SendMessageAsync(message, isTTS, embed, options).ConfigureAwait(false);
            await Task.Delay(delayMS).ConfigureAwait(false);
            await Context.Channel.DeleteMessageAsync(msg).ConfigureAwait(false);
            await Context.Channel.DeleteMessageAsync(Context.Message).ConfigureAwait(false);
        }

        protected async Task<IGuildUser> GetUserInGuildAsync(string userName)
        {
            if (userName == "" || userName == null)
            {
                _ = await ReplyAsync("Kérlek nevezz meg valaki/valamit!").ConfigureAwait(false);
                return null;
            }
            IGuildUser user = null;
            if (userName.StartsWith("<@", StringComparison.InvariantCulture) && ulong.TryParse(userName.Replace("<@", "").Replace(">", ""), out ulong id))
            {
                user = await Context.Guild.GetUserAsync(id).ConfigureAwait(false);
            }
            else
            {
                IEnumerable<IGuildUser> users = (await (Context.Guild?.GetUsersAsync()).ConfigureAwait(false))?.Where(x => x.Username.Contains(userName));
                if (users != null && users.Count() == 1)
                {
                    user = users.First();
                }
                else
                {
                    _ = users == null
                        ? await ReplyAsync("Ez a Felhasználó nem található meg a listában.").ConfigureAwait(false)
                        : await ReplyAsync("Ez a Felhasználó megtalálható a listában."
                                           + Environment.NewLine
                                           + string.Join(", ", users.Select(x => x.Username))
                                          ).ConfigureAwait(false);
                }
            }
            return user;
        }

        protected async Task<ITextChannel> GetTextChannelInGuildAsync(string channelName, bool defaultToCurrent)
        {
            if ((channelName == "" || channelName == null) && !defaultToCurrent)
            {
                _ = await ReplyAsync("Kérlek add meg a Csatorna nevét!").ConfigureAwait(false);
                return null;
            }
            IReadOnlyCollection<ITextChannel> allChannels = await Context.Guild.GetTextChannelsAsync().ConfigureAwait(false);
            ITextChannel channel = channelName != ""
                ? allChannels.FirstOrDefault(x => x.Name.Equals(channelName, StringComparison.OrdinalIgnoreCase))
                : defaultToCurrent ? Context.Channel as ITextChannel : null;
            if (channel == null)
            {
                _ = await ReplyAsync("A megadott Csatorna nem található.").ConfigureAwait(false);
            }
            return channel;
        }

        protected async Task<IRole> GetRoleInGuildAsync(string roleName)
        {
            if (roleName == "" || roleName == null)
            {
                _ = await ReplyAsync("Kérlek add meg a Rang nevét!").ConfigureAwait(false);
                return null;
            }
            IRole role = Context.Guild.Roles.FirstOrDefault(x => x.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            if (role == null)
            {
                _ = await ReplyAsync("A megadott Rang nem található.").ConfigureAwait(false);
            }
            return role;
        }

        /// <summary>Get the bot's highest ranked role with permission Manage Roles</summary>
        public async Task<IRole> GetManageRolesRoleAsync()
        {
            IGuildUser thisBot = await Context.Guild.GetUserAsync(Context.Client.CurrentUser.Id).ConfigureAwait(false);
            IRole ownrole = Context.Guild.Roles.FirstOrDefault(x => x.Permissions.ManageRoles && x.Id == thisBot.RoleIds.Max());
            return ownrole;
        }
    }
}