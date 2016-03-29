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
        public const string SeekEvent = "SeekCompleted";
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
