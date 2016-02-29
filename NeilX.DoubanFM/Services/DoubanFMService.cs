using NeilX.DoubanFM.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.Services
{
    public class DoubanFMService
    {
        private static string ClientId { get; } = "02646d3fb69a52ff072d47bf23cef8fd";
        private static string ClientSecret { get; } = "cde5d61429abcd7c";
        private static Uri RedirectUri { get; } = new Uri("http://www.douban.com/mobile/fm");
        private static string AppName { get; } = "radio_iphone";
        private static string AppVersion { get; } = "100";
        private static string Udid { get; } = Guid.NewGuid().ToString("N");
        private static IServerConnection ServerConnection { get; }
        private static ISession Session { get; }

        public static IPlayer Player { get; }

        public static IDiscovery Discovery { get; }
        static DoubanFMService()
        {
            ServerConnection = new ServerConnection(ClientId, ClientSecret, AppName, AppVersion, RedirectUri, Udid);
            Session = new Session(ServerConnection);
            Player = new Player(Session);
            Discovery = new Discovery(Session);
        }

        public static async Task<IList<Channel>> SearchChannelAsync(string query, int start, int size)
        {
            var channels = await Discovery.SearchChannel(query, start, size);
            return  channels.CurrentList;
        }

        public async static void ChangeFMChannel(Channel channel)
        {
            await Player.ChangeChannel(channel);
        }
        public async static Task<Channel[]> GetRecommendedChannels()
        {
            var channelGroups =  await Discovery.GetRecommendedChannels();
            return channelGroups.SelectMany(group => group.Channels).ToArray();
        }

        public async static void SkipToNextSong(NextCommandType commandType)
        {
            await Player.Next(commandType);
        }

    }
}
