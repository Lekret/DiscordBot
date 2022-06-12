using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Source.Utils;

namespace DiscordBot.Source.Modules
{
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "Тебе нельзя")]
        [Command("kick")]
        public async Task Kick(IUser target = null, [Remainder] string reason = "")
        {
            if (target == null)
            {
                await ReplyAsync($"{Context.User.Mention} kick [user] [reason]");
                return;
            }

            try
            {
                await Context.Guild.GetUser(target.Id).KickAsync(reason);
                await ReplyAsync($"{target.Mention} Кик по причине {reason}");
            }
            catch
            {
                await ReplyAsync($"{Context.User.Mention} Ты не можешь кикнуть {target.Mention}");
            }
        }
        
        [Command("hello")]
        public async Task Hello()
        {
            var user = Context.User;
            if (user.IsCreator())
                await ReplyAsync($"{user.Mention} Привет, создатель!");
            else
                await ReplyAsync($"{user.Mention} Приветствую!");
        }
    }
}