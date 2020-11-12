using TwitchDev.TwitchBot.Models;
using TwitchLib.Client.Models;

namespace TwitchDev.TwitchBot.Assemblers
{
    public static class ChatMessageAssembler
    {
        public static ChatMessageModel ToModel(this ChatMessage e)
        {
            return new ChatMessageModel
            {
                Username = e.Username,
                Message = e.Message
            };
        }
    }
}
