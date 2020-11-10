using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TwitchDev.TwitchBot.Configuration;
using TwitchDev.TwitchBot.Storage;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchDev.TwitchBot
{
    public class TwitchBotService : BackgroundService
    {
        private readonly TwitchBotConfiguration _configuration;
        private readonly ILogger<TwitchBotService> _logger;
        private readonly TwitchBotStorage _storage;

        private TwitchClient _client = null;

        public TwitchBotService(IOptions<TwitchBotConfiguration> options, ILogger<TwitchBotService> logger, TwitchBotStorage storage)
        {
            _configuration = options.Value;
            _logger = logger;
            _storage = storage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TwitchBot is starting.");

            var credentials = new ConnectionCredentials(_configuration.Username, _configuration.Oauth);

            _client = new TwitchClient();
            _client.Initialize(credentials, _configuration.Channel);
            _client.Connect();

            _client.OnMessageReceived += OnMessageReceived;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogTrace("TwitchBot is running.");

                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation("TwitchBot is stopping.");
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            _logger.LogDebug($"{e.ChatMessage.Username}: {e.ChatMessage.Message}");
            _storage.LastChatMessage = e.ChatMessage;
        }

        public override void Dispose()
        {
            if (_client != null && _client.IsConnected)
            {
                _client.Disconnect();
            }

            base.Dispose();
        }
    }
}
