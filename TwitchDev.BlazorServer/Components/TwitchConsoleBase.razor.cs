using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Timers;
using TwitchDev.TwitchBot.Storage;

namespace TwitchDev.BlazorServer.Components
{
    public class TwitchConsoleBase : ComponentBase, IDisposable
    {
        [Inject] ILogger<TwitchConsoleBase> Logger { get; set; }
        [Inject] TwitchBotStorage TwitchBotStorage { get; set; }

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
            if (TwitchBotStorage.LastChatMessage == null)
            {
                return;
            }

            string message = $"{TwitchBotStorage.LastChatMessage.Username}: {TwitchBotStorage.LastChatMessage.Message}";
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
