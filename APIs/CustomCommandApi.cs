using Il2Cpp_Scripts.Systems.Chat;

namespace CustomCommandApi
{
    public class CommandHandler
    {
        public static readonly Dictionary<string, (Func<string[], string> Code, string response, int requiredArgs)> commandList = [];

        public static void NewCommand(string commandName, Func<string[], string> code, int requiredArgs, string response = "")
        {
            NewCommand(commandName, "/", code, requiredArgs, response);
        }

        public static void NewCommand(string commandName, string prefix, Func<string[], string> code, int requiredArgs, string response = "")
        {
            string fullCommand = prefix + commandName;

            if(!commandList.ContainsKey(fullCommand))
            {
                commandList.Add(fullCommand, (code, response, requiredArgs));
            }
        }

        public static bool RunCommand(string input)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0];
            string[] args = [.. parts.Skip(1)];

            if(commandList.TryGetValue(command, out var commandData))
            {
                if(args.Length < commandData.requiredArgs)
                {
                    ChatManager.Instance.SendLocalSystemChatMessage(ChatManager.Instance._chatCommandExecutor.CommandResponseString(CommandResponseType.ErrorInvalidArguments));
                    return false;
                }

                string response = commandData.Code(args);

                string finalMessage = !string.IsNullOrEmpty(response) ? response : commandData.response;

                ChatManager.Instance.SendLocalSystemChatMessage(finalMessage);

                return false;
            }

            return true;
        }
    }
}