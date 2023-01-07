using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace HtmlEditorAPI.Classes
{
    [NotMapped]
    public class RequestReturn
    {
        public string Name { get; set; }
        public ActionResult result { get; set; }
    }
}
