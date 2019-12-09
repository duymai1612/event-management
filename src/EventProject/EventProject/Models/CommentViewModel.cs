using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventProject.Models
{
    public class CommentViewModel
    {
        public CommentViewModel (int commentId, string content, DateTime post, int eventId, string userId,
            bool isInActive)
        {
            this.CommentID = commentId;
            this.Content = content;
            this.PostDate = post;
            this.EventID = eventId;
            this.UserID = userId;
            this.IsInActive = isInActive;
        }
        public int CommentID { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public int EventID { get; set; }
        public string UserID { get; set; }
        public bool IsInActive { get; set; }
    }
}