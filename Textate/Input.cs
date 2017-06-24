using System;
using System.Linq;

namespace Textate
{
    public class Input
    {
        public string User { get; set; }
        public string Command { get; set; }

        public Instruction GetInstruction()
        {
            var c = Command.Split(' ');

            var success = false;
            for (int i = c.Length; i > 0; i--)
            {
                success = Enum.TryParse(string.Concat(c.Take(i)), true, out Directive directive);
                if (success)
                {
                    return new Instruction(directive, string.Join(" ", c.Skip(i)));
                }
            }
            return new Instruction(Directive.Help);
        }
    }

    public class Instruction
    {
        public Directive Directive { get; }
        public string Payload { get; }

        public Instruction(Directive directive, string payload = null)
        {
            Directive = directive;
            Payload = payload;
        }
    }

    public enum Directive
    {
        CommandAdd,
        CommandEdit,
        CommandRemove,
        SetTimezone,
        Help,
    }
}