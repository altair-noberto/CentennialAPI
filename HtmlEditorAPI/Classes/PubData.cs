﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HtmlEditorAPI.Classes
{
    [NotMapped]
    public class PubData
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string categoria { get; set; }
        public IFormFile img { get; set; }
        public IFormFile pub { get; set; }
    }
}
