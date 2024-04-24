using System;
using System.ComponentModel.DataAnnotations;

namespace QLKTXWEBSITE.Models
{
    public partial class News
    {
        public int NewsId { get; set; }


        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string? Author { get; set; }
    }
}
