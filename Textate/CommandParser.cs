using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Textate.Storage;

namespace Textate
{
    public enum CommandType
    {
        Final,
        Command,
        Settings,
        Help,
        Date,
        String,
    }

    public enum PayloadType
    {
        None,
        Date,
        String,
        Index,
    }

    public class PrimaryCommand {
        private CommandType commandType;

        public string Name { get; }

        public PrimaryCommand(string name, CommandType commandType)
        {
            this.commandType = commandType;
            Name = name;
        }

        public PrimaryCommand(CommandTableEntity commandTableEntity) :
            this(commandTableEntity.Name, (CommandType)Enum.Parse(typeof(CommandType), commandTableEntity.TableEntityType.ToString()))
        {
        }

        public bool IsMatch(string token) =>
            string.Equals(token, Name, StringComparison.CurrentCultureIgnoreCase);

        public Subcommand Subcommand(string token) =>
            CommandParser.SubcommandsDictionary[commandType].Find(x => x.IsMatch(token));
    }

    public class Subcommand
    {
        private Func<Subcommand, string> run;

        public string Name;

        public PayloadType PayloadType { get; }

        public Subcommand(string name, Func<Subcommand, string> run, PayloadType payloadType = PayloadType.None)
        {
            Name = name;
            this.run = run;
            PayloadType = payloadType;
        }

        public bool IsMatch(string token) =>
            string.Equals(token, Name, StringComparison.CurrentCultureIgnoreCase);
    }

    public static class CommandParser
    {
        public static Dictionary<CommandType, List<Subcommand>> SubcommandsDictionary =>
            new Dictionary<CommandType, List<Subcommand>>
            {
                { CommandType.Date, DateSubcommands },
                { CommandType.String, StringSubcommands },
            };

        public static List<Subcommand> DateSubcommands =>
            new List<Subcommand>
            {
                new Subcommand("add", x => x.Name, PayloadType.Date),
                new Subcommand("delete", x => x.Name),
                new Subcommand("view", x => x.Name)
            };

        public static List<Subcommand> StringSubcommands =>
            new List<Subcommand>
            {
                new Subcommand("add", x => x.Name, PayloadType.String),
                new Subcommand("delete", x => x.Name),
                new Subcommand("view", x => x.Name)
            };

        public static List<PrimaryCommand> UserCommands =>
            new List<PrimaryCommand>
            {
                new PrimaryCommand(new CommandTableEntity("bike", TableEntityType.Date)),
                new PrimaryCommand(new CommandTableEntity("bible", TableEntityType.Date)),
                new PrimaryCommand(new CommandTableEntity("movie", TableEntityType.String)),
            };

        public static List<PrimaryCommand> BuiltinCommands =>
            new List<PrimaryCommand>
            {
                new PrimaryCommand("command", CommandType.Command),
                new PrimaryCommand("settings", CommandType.Settings),
                HelpCommand,
            };

        public static PrimaryCommand HelpCommand = new PrimaryCommand("help", CommandType.Help);

        public static List<PrimaryCommand> AllCommands
        {
            get
            {
                var l = BuiltinCommands;
                l.AddRange(UserCommands);
                return l;
            }
        }

        public static ExecutionPlan Parse(string input)
        {
            var tokens = Regex.Split(input, @"[\s]+");
            var commands = AllCommands;

            var ep = new ExecutionPlan();

            var c = commands.Find(x => x.IsMatch(tokens.First()));
            ep.Partition = c.Name;
            var s = c.Subcommand(tokens.Skip(1).First());
            ep.Query = s.Name;
            var rest = string.Join(" ", tokens.Skip(2));
            if (rest != string.Empty) {
                switch (s.PayloadType)
                {
                    case PayloadType.Date:
                        var d = rest + "/" + DateTime.Today.Year.ToString();
                        var p = DateTime.ParseExact(d, "M/d/yyyy", CultureInfo.CurrentCulture);
                        ep.Payload = p.ToString("yyyy-MM-dd");
                        break;
                    case PayloadType.String:
                        ep.Payload = rest;
                        break;
                }
            }

            return ep;
        }
    }

    public class ExecutionPlan
    {
        public string Partition { get; set; }
        public string Query { get; set; }
        public string Payload { get; set; }

        public string Stringify()
        {
            return string.Join(":", Partition, Query, Payload).TrimEnd(':');
        }
    }
}
