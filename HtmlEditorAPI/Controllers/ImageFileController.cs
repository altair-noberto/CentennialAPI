using HtmlEditorAPI.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImageUploaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageFileController : ControllerBase
    {
        [HttpPost]
        [Route("SaveImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public RequestReturn Post([FromForm] ImageModel file)
        {
            RequestReturn requestReturn = new RequestReturn();
            string extension = file.Name.Split('.')[1];
            if (extension == "png" || extension == "jpg")
            {
                string fileName = file.Name.Split('.')[0] + DateTime.Now.Ticks + '.' + extension;
                try
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "Upload//Images", fileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                    requestReturn.Name = fileName;
                    requestReturn.result = StatusCode(StatusCodes.Status201Created);
                    return requestReturn;
                }
                catch (Exception)
                {
                    requestReturn.result = StatusCode(StatusCodes.Status500InternalServerError);
                    requestReturn.Name = "FailInAPI";
                    return requestReturn;
                }
            }
            else
            {
                requestReturn.result = StatusCode(StatusCodes.Status400BadRequest);
                requestReturn.Name = "ArquivoInvalido";
                return requestReturn;
            }
        }
    }
}
