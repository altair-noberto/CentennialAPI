using HtmlEditorAPI.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Linq;

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
                using StreamWriter fileIndex = new("Upload//ImagesIndex.txt", append: true);
                string host = "https://" + HttpContext.Request.Host.Value;
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
                    fileIndex.WriteLineAsync(fileName + ',' + host + "/Images/" + fileName);
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

        [HttpGet]
        public List<ImageIndex> GetImages()
        {
           string[] images = System.IO.File.ReadAllLines("Upload//ImagesIndex.txt");
           List<ImageIndex> imagesIndex = new List<ImageIndex>();
           foreach (string image in images)
           {
                ImageIndex imgIndex = new ImageIndex(image.Split(',')[0], image.Split(',')[1]);
                imagesIndex.Add(imgIndex);
           }
           return imagesIndex;
        }

        [HttpDelete]
        public ActionResult Delete(string nome)
        {
            string[] images = System.IO.File.ReadAllLines("Upload//ImagesIndex.txt");
            string indexToRemove;
            List<ImageIndex> imagesIndex = new List<ImageIndex>();
            foreach (string image in images)
            {
                if (image.Split(',')[0] == nome)
                {
                   indexToRemove = image;
                   System.IO.File.Delete("Upload//Images/" + image.Split(',')[0]);
                   images = images.Where(i => i != indexToRemove).ToArray();
                   System.IO.File.WriteAllLinesAsync("Upload//ImagesIndex.txt", images);
                   return Ok();
                }
            }
            return BadRequest();
        }
    }
}
