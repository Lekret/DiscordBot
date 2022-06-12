using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Source.Utils;

namespace DiscordBot.Source.Modules
{
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        public CommandModule(DiscordSocketClient client)
        {
            client.SelectMenuExecuted += Handler;
        }
        
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

        [Command("test")]
        public async Task Test()
        {
            await ExecuteOnSelectedUser("Test", null);
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

        private async Task ExecuteOnSelectedUser(string name, Action<IUser> execute)
        {
            var menuBuilder = new SelectMenuBuilder().WithCustomId("select_user");
            foreach (var user in Context.Guild.Users)
            {
                menuBuilder.AddOption(user.Username, $"{user.Id}");
            }

            var componentBuilder = new ComponentBuilder()
                .WithButton("Do", "do_button", ButtonStyle.Success)
                .WithSelectMenu(menuBuilder);
            await ReplyAsync(name, components: componentBuilder.Build());
        }

        private static async Task Handler(SocketMessageComponent arg)
        {
            var text = string.Join(", ", arg.Data.Values);
            await arg.RespondAsync($"You have selected {text}");
        }
    }
}