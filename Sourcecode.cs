using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private static int rate;
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=/.,\';][?><|:}{P";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static void Main(string[] args)
        {
            new Program().Run().GetAwaiter().GetResult();
        }
        public async Task Run()
        {
            Console.Clear();
            Console.WriteLine("Shit's 'bout to go down");
            await Task.Delay(1000);
            var _client = new DiscordClient(new DiscordConfig()
            {
                Token = File.ReadAllLines(Environment.CurrentDirectory + "/token.txt")[0], 
                TokenType = TokenType.User,
                DiscordBranch = Branch.Canary,
                LogLevel = LogLevel.Info,
                UseInternalLogHandler = true,
                AutoReconnect = true
            });

                var cnfg = new CommandsNextConfiguration { StringPrefix = "start.", SelfBot = true, EnableDms = true, EnableMentionPrefix = true, CaseSensitive = false };

            var cnext = _client.UseCommandsNext(cnfg);
            cnext.RegisterCommands<MyCommands>();
            await _client.ConnectAsync();

            _client.Ready += async (e) =>
            {
                Console.WriteLine("Ready to spaaaam");
            };
            _client.GuildAvailable += async (e) =>
            {
                Console.WriteLine(e.Guild.Name);
            };



            await Task.Delay(-1);
        }
        class MyCommands
        {
            [Command("spam")]
            public async Task Start(CommandContext e)
            {
                int count = 1;
                while (true)
                {

                    count = count + 1;
                    try
                    {
                        
                            await e.Channel.SendMessageAsync(File.ReadAllLines(Environment.CurrentDirectory + "/msg.txt")[0]);
                            Console.WriteLine("Sent Message Num:" + count);

                    }
                    catch (Exception)
                    {

                        Console.WriteLine($"Message {count} threw, Most likely ratelimited.");
                        count = count - 1;
                    }



                    await Task.Delay(int.Parse(File.ReadAllLines(Environment.CurrentDirectory + "/speed.txt")[0]));
                }
            }

            [Command("ping")]
            public async Task Ping(CommandContext e)
            {
                await e.Message.EditAsync($"Ping: {e.Client.Ping}ms");
            }
        }

    }
}
