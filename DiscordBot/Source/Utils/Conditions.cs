using Discord;

namespace DiscordBot.Source.Utils
{
    public static class Conditions
    {
        public static bool IsCreator(this IUser user)
        {
            return user.Username == "Lekret" && user.Discriminator == "1995";
        }
    }
}