using HtmlEditorAPI.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HtmlEditorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PublicacaoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("SavePub")]
        public ActionResult Post([FromForm] PubData pubData)
        {
            string Folder = Path.Combine(Directory.GetCurrentDirectory(), $"Postagens\\{pubData.title}");
            string host = "https://" + HttpContext.Request.Host.Value;
            Publicacao publicacao = new Publicacao();
            string imgExtension = pubData.img.FileName.Split('.')[1];
            if (imgExtension == "jpg" || imgExtension == "png")
            {
                Directory.CreateDirectory(Folder);
                string imgName = pubData.title + '.' + imgExtension;
                string pubName = pubData.title + ".html";
                try
                {
                    using (Stream stream = new FileStream(Path.Combine(Folder, imgName), FileMode.Create))
                    {
                        pubData.img.CopyTo(stream);
                    }
                    using (Stream stream = new FileStream(Path.Combine(Folder, pubName), FileMode.Create))
                    {
                        pubData.pub.CopyTo(stream);
                    }
                    publicacao.Title = pubData.title;
                    publicacao.SubTitle = pubData.subtitle;
                    publicacao.PubData = DateTime.Now;
                    publicacao.PubDirectory = host + $"/Postagens/{pubData.title.Replace(" ", "%20")}/{pubName}";
                    publicacao.ImgDirectory = host + $"/Postagens/{pubData.title.Replace(" ", "%20")}/{imgName}";
                    _context.Publicacoes.Add(publicacao);
                    _context.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK);
                }
                catch(Exception) {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("{id:int}", Name = "GetPub")]
        public ActionResult<Publicacao> GetPub(int id)
        {
            try
            {
                var pub = _context.Publicacoes.Where(t => t.Id == id).First();
                if (pub == null) return NotFound("ID inválido");
                else return Ok(pub);
            }
            catch
            {
                return BadRequest("ID inválido");
            }
        }
        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<Publicacao>>> GetAllPubs()
        {
            try
            {
                var pubs = await _context.Publicacoes.ToListAsync();
                return Ok(pubs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePub(int id)
        {
            try
            {
                var Pub = _context.Publicacoes.Where(p => p.Id == id).First();
                if (Pub == null) return NotFound("Publicação não encontrada");
                else
                {
                    string Folder = Path.Combine(Directory.GetCurrentDirectory(), $"Postagens\\{Pub.Title}");
                    Directory.Delete(Folder, true);
                    _context.Publicacoes.Remove(Pub);
                    await _context.SaveChangesAsync();
                    return Ok("Publicação removida com sucesso");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
