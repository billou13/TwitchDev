using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchDev.Core;
using TwitchDev.DataStorage.Interfaces;
using TwitchDev.TwitchBot.Configuration;
using TwitchDev.TwitchBot.Interfaces;
using TwitchDev.TwitchBot.Models;

namespace TwitchDev.TwitchBot
{
    public class TwitchService : ITwitchService
    {
        private readonly TwitchConfiguration _configuration;
        private readonly ILogger<TwitchService> _logger;
        private readonly IRedisService _redisService;

        public TwitchService(IOptions<TwitchConfiguration> options, ILogger<TwitchService> logger, IRedisService redisService)
        {
            _configuration = options.Value;
            _logger = logger;
            _redisService = redisService;
        }

        public string GetChannel()
        {
            return _configuration.Channel;
        }

        public void InsertMessage(ChatMessageModel message)
        {
            _redisService.SortedSetAdd(GetStorageKey(TwitchBotKeys.Messages), message.CreatedUtcDate.ToUnixTimestamp(), message);
            _redisService.Set(GetStorageKey(TwitchBotKeys.LastMessage), message);
        }

        public ChatMessageModel GetLastMessage()
        {
            return _redisService.Get<ChatMessageModel>(GetStorageKey(TwitchBotKeys.LastMessage));
        }

        public List<ChatMessageModel> GetAllMessages(DateTime minDate, DateTime maxDate)
        {
            double start = minDate.ToUniversalTime().ToUnixTimestamp();
            double stop = maxDate.ToUniversalTime().ToUnixTimestamp();

            var items = _redisService.SortedSetRangeByScore<ChatMessageModel>(GetStorageKey(TwitchBotKeys.Messages), start, stop);
            return items.ToList();
        }

        private string GetStorageKey(string key)
        {
            return $"{_configuration.Channel}:{key}";
        }
    }
}
