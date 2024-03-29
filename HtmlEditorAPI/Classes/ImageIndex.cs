﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HtmlEditorAPI.Classes
{
    [NotMapped]
    public class ImageIndex
    {
        public ImageIndex(int id, string nome, string url)
        {
            Id = id;
            Nome = nome;
            Url = url;
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Url { get; set; }
    }
}
