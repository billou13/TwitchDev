using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Timers;
using TwitchDev.DataStorage.Interfaces;
using TwitchDev.TwitchBot;
using TwitchDev.TwitchBot.Models;

namespace TwitchDev.BlazorServer.Components
{
    public class TwitchConsoleBase : ComponentBase, IDisposable
    {
        [Inject] ILogger<TwitchConsoleBase> Logger { get; set; }
        [Inject] IRedisService RedisService { get; set; }

        private Timer timer;

        public string LastMessage { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                timer = new Timer(1000);
                timer.Elapsed += OnElapsed;
                timer.AutoReset = true;

                timer.Start();
            }

            base.OnAfterRender(firstRender);
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            var lastMessage = RedisService.Get<ChatMessageModel>(TwitchBotService.LastMessageStorageKey);
            if (lastMessage == null)
            {
                return;
            }

            string message = $"{lastMessage.Username}: {lastMessage.Message}";
            if (LastMessage != message)
            {
                Logger.LogDebug($"TwitchConsoleBase.OnElapsed - New message: {message}");
                LastMessage = message;
                InvokeAsync(() => StateHasChanged());
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
