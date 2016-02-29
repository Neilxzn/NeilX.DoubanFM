﻿namespace NeilX.DoubanFM.Core
{
    /// <summary>
    /// The type of change channel command
    /// </summary>
    public enum ChangeChannelCommandType
    {
        /// <summary>
        /// Normal. Such as select a channel from recommended channel or search channel result.
        /// </summary>
        Normal,
        /// <summary>
        /// Play related songs.
        /// </summary>
        PlayRelatedSongs,
    }
}
