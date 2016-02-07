﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using Newtonsoft.Json;

namespace PlayStation_App.Models.MessageGroups
{

    public class MessageGroup
    {

        [JsonProperty("messageGroupId")]
        public string MessageGroupId { get; set; }

        [JsonProperty("messageGroupModifiedDate")]
        public string MessageGroupModifiedDate { get; set; }

        [JsonProperty("messageGroupDetail")]
        public MessageGroupDetail MessageGroupDetail { get; set; }

        [JsonProperty("totalUnseenMessages")]
        public int TotalUnseenMessages { get; set; }

        [JsonProperty("totalMessages")]
        public int TotalMessages { get; set; }

        [JsonProperty("latestMessage")]
        public LatestMessage LatestMessage { get; set; }

        [JsonProperty("thumbnailDetail")]
        public ThumbnailDetail ThumbnailDetail { get; set; }

        [JsonProperty("messageUid")]
        public int MessageUid { get; set; }

        [JsonProperty("sentMessageId")]
        public string SentMessageId { get; set; }
    }

}
