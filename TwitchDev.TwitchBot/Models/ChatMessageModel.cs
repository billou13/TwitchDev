using System;

namespace TwitchDev.TwitchBot.Models
{
    public class ChatMessageModel
    {
        public DateTime CreatedUtcDate { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
