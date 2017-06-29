using System;
using System.Collections.Generic;
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

    public class PrimaryCommand {
        private Regex matcher;
        private string description;
        private CommandType commandType;

        public List<PrimaryCommand> Subcommands => CommandParser.CommandTypeDictionary[commandType];

        public string Name { get; internal set; }

        public PrimaryCommand(string shortcut, string name, string description, CommandType commandType)
        {
            matcher = new Regex($"^(?<action>{shortcut}|{name})(?<rest>.*)$", RegexOptions.IgnoreCase);
            this.description = description;
            this.commandType = commandType;
            Name = name;
        }

        public PrimaryCommand(CommandTableEntity commandTableEntity) :
            this(commandTableEntity.Shortcut, commandTableEntity.Name, commandTableEntity.Description, (CommandType)Enum.Parse(typeof(CommandType), commandTableEntity.TableEntityType.ToString()))
        {
        }

        public bool IsMatch(string input) => matcher.IsMatch(input);

        public string Unparsed(string input) =>
            IsMatch(input)
            ? matcher.Match(input).Groups["rest"].Value.Trim()
            : null;
    }

    public static class CommandParser
    {
        public static List<PrimaryCommand> GetDateCommands =>
            new List<PrimaryCommand>
            {
                new PrimaryCommand("$", "add", "Add record for today", CommandType.Final),
                new PrimaryCommand("x", "remove", "Remove record for today", CommandType.Final)
            };

        public static Dictionary<CommandType, List<PrimaryCommand>> CommandTypeDictionary =>
            new Dictionary<CommandType, List<PrimaryCommand>>
            {
                [CommandType.Final] = new List<PrimaryCommand>(),
                [CommandType.Date] = GetDateCommands
            };

        public static List<CommandTableEntity> GetUserCommands =>
            new List<CommandTableEntity>
            {
                new CommandTableEntity("b", "bike", "Commuted to work by bike", TableEntityType.Date),
                new CommandTableEntity("s", "scripture", "Read the Bible", TableEntityType.Date),
                new CommandTableEntity("m", "movie", "Movie to watch", TableEntityType.Date),
            };

        public static List<PrimaryCommand> GetBuiltinCommands => new List<PrimaryCommand>
            {
                new PrimaryCommand("xc", "command", "Manage custom user commands", CommandType.Command),
                new PrimaryCommand("xs", "settings", "Adjust app settings", CommandType.Settings),
                HelpCommand,
            };

        public static PrimaryCommand HelpCommand = new PrimaryCommand("xh", "help", "Get help", CommandType.Help);

        public static List<PrimaryCommand> GetAllCommands
        {
            get
            {
                var l = GetBuiltinCommands;
                l.AddRange(GetUserCommands.Select(x => new PrimaryCommand(x)));
                return l;
            }
        }

        public static List<string> Parse(string input, List<PrimaryCommand> commands = null)
        {
            var unparsed = input;
            commands = commands ?? GetAllCommands;
            var sequence = new List<string>();
            while (unparsed != null && commands.Any())
            {
                var i = commands.FindIndex(x => x.IsMatch(unparsed));
                if (i == -1)
                {
                    sequence.Add(HelpCommand.Name);
                    break;
                }
                else
                {
                    sequence.Add(commands[i].Name);
                    unparsed = commands[i].Unparsed(input);
                    commands = commands[i].Subcommands;
                }
            }

            return sequence;
        }
    }
}
