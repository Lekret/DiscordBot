using System.Threading.Tasks;

namespace DiscordBot.Source
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            var token = args[0];
            return new Application(token).Start();
        }
    }
}