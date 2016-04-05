using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    [DataContract]
    public  class PlayerEventMessage
    {
        public const string SeekCompleted = "SeekCompleted";
        public const string MediaEnd = "MediaEnd";
        public const string MediaFailed = "MediaFailed";
        public const string MediaOpened = "MediaOpened";
        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public string EventArg { get; set; }
        public PlayerEventMessage(string name,string arg)
        {
            EventName = name;
            EventArg = arg;
        }
    }
}
