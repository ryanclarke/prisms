using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Textate.Storage;

namespace Textate
{
    public enum CommandType
    {
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

        public PrimaryCommand(string shortcut, string name, string description, CommandType commandType)
        {
            matcher = new Regex($"^(<action>{shortcut}|{name}) (<rest>.*)$");
            this.description = description;
            this.commandType = commandType;
        }

        public PrimaryCommand(CommandTableEntity commandTableEntity) :
            this(commandTableEntity.Shortcut, commandTableEntity.Name, commandTableEntity.Description, (CommandType)Enum.Parse(typeof(CommandType), commandTableEntity.TableEntityType.ToString()))
        {
        }

        public bool IsMatch(string input)
        {
            return matcher.IsMatch(input);
        }

        public string Action(string input)
        {
            return matcher.Match(input).Groups["rest"].Value;
        }
    }

    public static class CommandParser
    {
        public static List<CommandTableEntity> GetUserCommands =>
            new List<CommandTableEntity>() {
                        new CommandTableEntity("b", "bike", "Commuted to work by bike", TableEntityType.Date),
                        new CommandTableEntity("s", "scripture", "Read the Bible", TableEntityType.Date),
                        new CommandTableEntity("m", "movie", "Movie to watch", TableEntityType.Date),
            };

        public static List<PrimaryCommand> GetBuiltinCommands =>
            new List<PrimaryCommand>() {
                new PrimaryCommand("xc", "command", "Manage custom user commands", CommandType.Command),
                new PrimaryCommand("xs", "settings", "Adjust app settings", CommandType.Settings),
                new PrimaryCommand("xh", "help", "Get help", CommandType.Help),
            };

        public static List<PrimaryCommand> GetAllCommands
        {
            get
            {
                var l = GetBuiltinCommands;
                l.AddRange(GetUserCommands.Select(x => new PrimaryCommand(x)));
                return l;
            }
        }

        public static void Parse(string input)
        {
            var primaryCommand = GetAllCommands.Find(x => x.IsMatch(input));
            primaryCommand.Action(input);
        }
    }
}
