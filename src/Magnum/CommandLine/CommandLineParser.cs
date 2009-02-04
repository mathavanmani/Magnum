namespace Magnum.CommandLine
{
    using System.Collections.Generic;

    public class CommandLineParser
    {
        private Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();


        public object Parse(string[] commandLine)
        {
            var commandName = commandLine.Head();
            string[] remainder = commandLine.Tail();

            object command = _commands[commandName];

            new ArgumentOrderPolicy().Verify(remainder);

            return null;
        }
        public Output<ARGS> Parse<ARGS>(string[] commandLine) where ARGS : new()
        {
            Output<ARGS> result = new Output<ARGS>();
            result.CommandName = commandLine.Head();
            result.Command = (IArgCommand<ARGS>)_commands[result.CommandName];
            string[] remainder = commandLine.Tail();

            new ArgumentOrderPolicy().Verify(remainder);

            result.Args = new ArgumentParsingInstructions<ARGS>().Build(remainder);
            return result;
        }


        public void AddCommand<COMMAND, ARGS>() where COMMAND : IArgCommand<ARGS>, new() where ARGS : new()
        {
            string key = new UseTypeName().GetName<COMMAND>();
            var api = new ArgumentParsingInstructions<ARGS>();
            _commands.Add(key, new COMMAND());
        }
    }
}