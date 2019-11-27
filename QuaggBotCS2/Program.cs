using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace QuaggBotCS2
{
    public class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            string jsonData = File.ReadAllText("appSettings.Debug.json");
            JObject jsonObject = JObject.Parse(jsonData);
            JToken jsonConnString = jsonObject["connString"];
            string connString = jsonConnString.ToString();
            DataHandler.DiscordToken = jsonObject["disKey"].ToString();
            DataHandler.MWKey = jsonObject["mwKey"].ToString();
            DataHandler.OpenWeather = jsonObject["oWeather"].ToString();
            DataHandler.ConnString = jsonObject["connString"].ToString();
            LoadContext();

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = DataHandler.DiscordToken,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });
            discord.GuildCreated += async e =>
            {
                await Task.Run(() =>
                {
                    var server = new Server
                    {
                        ServerName = e.Guild.Name,
                        ServerSnow = e.Guild.Id,
                        SettingsJson = "{ \"warnWords\": [], \"deleteWords\": []}"
                    };

                    DataHandler.Context.Servers.Add(server);

                    foreach (var user in e.Guild.Members)
                    {
                        var newUser = new User
                        {
                            UserSnow = user.Id,
                            Name = user.Username,
                            Discriminator = user.Discriminator,
                            Guild = server,
                            Strikes = 0
                        };
                        server.Users.Add(newUser);
                    }
                });
            };
            discord.MessageCreated += async e =>
            {
                var servers = DataHandler.Context.Servers;
                Server s = servers.Find(x => x.ServerSnow == e.Guild.Id);
                User u = s.Users.ToList().Find(x => x.UserSnow == e.Message.Author.Id);
                if (u.Muted == true)
                {
                    await e.Message.DeleteAsync();
                    return;
                }
            };
            discord.MessageCreated += async e =>
            {
                if (e.Author.IsBot == false)
                {
                    try
                    {
                        var member = (DiscordMember)e.Author;
                        bool admin = false;
                        foreach (DiscordRole role in member.Roles)
                        {
                            if (role.Permissions.HasPermission(Permissions.ManageGuild))
                            {
                                admin = true;
                            }
                        }
                        if (admin == false)
                        {
                            await CheckMessageClean(e);
                        }
                    }
                    catch (System.Exception)
                    {
                        return;
                    }
                }

            };
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "<>",
                CaseSensitive = false
            });
            commands.RegisterCommands<Commands>();
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        public static async Task CheckMessageClean(DSharpPlus.EventArgs.MessageCreateEventArgs args)
        {

            try
            {


                var servers = DataHandler.Context.Servers;
                Server server = new Server()
                {
                    SettingsJson = "{ \"warnWords\": [], \"deleteWords\": []}",
                    Users = new User[]
                    {
                        new User()
                        {
                            UserSnow = 0
                        }
                    }
                };
                foreach (Server s in servers)
                {
                    if (s.ServerSnow == args.Guild.Id)
                    {
                        server = s;
                        break;
                    }
                }
                var users = server.Users;
                foreach (User u in users)
                {
                    if (u.UserSnow == args.Author.Id)
                    {
                        if (u.Strikes >= 3)
                        {
                            await args.Message.DeleteAsync();
                            await args.Guild.Owner.SendMessageAsync($"User {args.Author.Username}#{args.Author.Discriminator} has just hit 3 strikes for time number {u.TotalMutes}. They're being auto-muted until you or an admin remove it.");
                            u.Strikes = 0;
                        }
                    }
                }

                var settings = Setting.FromJson(server.SettingsJson);

                foreach (string word in settings.WarnWords)
                {
                    if (args.Message.Content.Contains(word))
                    {
                        await args.Message.RespondAsync("Hey now, that's not allowed here. I'm gonna give you a warning for now. Three warnings will give a temporary mute, so be careful!");
                        foreach (User u in server.Users)
                        {
                            if (u.UserSnow == args.Author.Id)
                            {
                                ++u.Strikes;
                                if (u.Strikes >= 3)
                                {
                                    await args.Message.DeleteAsync();
                                    await args.Guild.Owner.SendMessageAsync($"User {args.Author.Username}#{args.Author.Discriminator} has just hit 3 strikes for time number {u.TotalMutes}d. They're being auto-muted until you or an admin remove it.");
                                    u.Strikes = 0;
                                }
                            }
                        }
                        break;
                    }
                }

                foreach (string word in settings.DeleteWords)
                {
                    if (args.Message.Content.Contains(word))
                    {
                        await args.Message.RespondAsync("Wow, that was too much for me to just warn you, that's getting deleted, and the owner of the server will be notified as well.");
                        await args.Message.DeleteAsync($"Language: {word}");
                        await args.Guild.Owner.SendMessageAsync($"Message deleted in channel #{args.Channel.Name}");
                    }
                } 
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.InnerException);
                throw;
            }
        }

        public static void LoadContext()
        {
            if (File.Exists("botContext.bin"))
            {
                Stream openFileStream = File.OpenRead("botContext.bin");
                BinaryFormatter deserializer = new BinaryFormatter();
                DataHandler.Context = (BotContext)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
            }
            else
            {
                var stream = File.Create("botContext.bin");
                stream.Close();
                DataHandler.Context = new BotContext();
            }
            DataHandler.Context.LoopNewThread();
        }
    }

    public static class DataHandler
    {
        public static string DiscordToken { get; set; }
        public static string MWKey { get; set; }
        public static string OpenWeather { get; set; }
        public static string ConnString { get; set; }
        public static BotContext Context { get; set; }
    }
}

