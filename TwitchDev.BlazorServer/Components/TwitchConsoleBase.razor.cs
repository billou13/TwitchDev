using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Timers;
using TwitchDev.TwitchBot.Interfaces;
using TwitchDev.TwitchBot.Models;

namespace TwitchDev.BlazorServer.Components
{
    public class TwitchConsoleBase : ComponentBase, IDisposable
    {
        [Inject] ILogger<TwitchConsoleBase> Logger { get; set; }
        [Inject] ITwitchService Service { get; set; }

        private Timer timer;

        public ChatMessageModel LastMessage { get; set; }

        public List<ChatMessageModel> Messages { get; set; }

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
            var lastMessage = Service.GetLastMessage();
            if (LastMessage != null && lastMessage.CreatedUtcDate == LastMessage.CreatedUtcDate)
            {
                return;
            }

            Logger.LogDebug("OnElapsed - New message...");

            LastMessage = lastMessage;
            Messages = Service.GetAllMessages(DateTime.Today, DateTime.Today.AddDays(1));

            InvokeAsync(() => StateHasChanged());
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
