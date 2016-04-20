﻿using System;

namespace NeilX.DoubanFM.Core
{
    /// <summary>
    /// An instance of Channel class indicates a channel in douban.fm. A channel usually contains a lot of songs.
    /// </summary>
    public class Channel : IEquatable<Channel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="id">The channel ID.</param>
        public Channel(int id)
        {
            Id = id;
        }

#pragma warning disable 1591
        public bool Equals(Channel other)
        {
            return other != null && Id == other.Id;
        }

        public override int GetHashCode()
#pragma warning restore 1591
        {
            return Id;
        }

        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the description of the channel.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets the ID of the channel.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public int Id { get; }

        /// <summary>
        /// Gets or sets the start song code.
        /// </summary>
        /// <value>
        /// The start song code.
        /// </value>
        public string Start { get; set; }

        /// <summary>
        /// Gets or sets the song count of the channel.
        /// </summary>
        /// <value>
        /// The song count.
        /// </value>
        public int SongCount { get; set; }

        /// <summary>
        /// Gets or sets the cover URL of the channel.
        /// </summary>
        /// <value>
        /// The cover URL.
        /// </value>
        public string CoverUrl { get; set; }

        public string ChannelGroupName { get; set; }
#pragma warning disable 1591
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Channel)obj);
        }

        public static bool operator ==(Channel left, Channel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Channel left, Channel right)
        {
            return !(left == right);
        }

        public override string ToString()
#pragma warning restore 1591
        {
            var result = $"Id: {Id}, Name: {Name}";
            if (!string.IsNullOrEmpty(Artist))
            {
                result += $", Artist: {Artist}";
            }
            return result;
        }
    }
}