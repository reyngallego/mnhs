using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Video
    {
        public int VideoId { get; set; }
        public string VideoName { get; set; }
        public byte[] VideoData { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
    }
}