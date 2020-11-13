using System;
using System.Collections.Generic;
using TwitchDev.TwitchBot.Models;

namespace TwitchDev.TwitchBot.Interfaces
{
    public interface ITwitchService
    {
        string GetChannel();
        void InsertMessage(ChatMessageModel message);
        ChatMessageModel GetLastMessage();
        List<ChatMessageModel> GetAllMessages(DateTime minDate, DateTime maxDate);
    }
}
