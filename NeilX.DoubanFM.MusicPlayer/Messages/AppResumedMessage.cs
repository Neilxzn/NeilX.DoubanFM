using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NeilX.DoubanFM.MusicPlayer.Messages
{
    [DataContract]
    public class AppResumedMessage
    {
        public AppResumedMessage()
        {
            this.Timestamp = DateTime.Now;
        }

        public AppResumedMessage(DateTime timestamp)
        {
            this.Timestamp = timestamp;
        }

        [DataMember]
        public DateTime Timestamp;
    }
}
