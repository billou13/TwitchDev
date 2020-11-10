using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TwitchDev.WebApplication
{
    /// <seealso cref="https://dev.twitch.tv/docs/irc"/>
    public class TwitchBot
    {
        private readonly string _username;
        private readonly string _oauth;
        private readonly string _channel;

        private TwitchClient _client;

        public TwitchBot(string username, string oauth, string channel)
        {
            _username = username;
            _oauth = oauth;
            _channel = channel;
        }

        public void Run()
        {
            var credentials = new ConnectionCredentials(_username, _oauth);

            _client = new TwitchClient();
            _client.Initialize(credentials, _channel);
            _client.Connect();

            _client.OnMessageReceived += OnMessageReceived; 
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var msg = e.ChatMessage;
        }
    }
}
