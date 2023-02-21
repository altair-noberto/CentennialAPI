using HtmlEditorAPI.Classes;
using Microsoft.AspNetCore.Authorization;
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
        [HttpPost, Authorize(Roles = "Admin")]
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
                    publicacao.Category = pubData.categoria;
                    DateTime dateTime = DateTime.Now;
                    publicacao.PubData = String.Format("{0:dd/MM/yyyy}", dateTime);
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

        [HttpPut, Authorize(Roles = "Admin")]
        [Route("UpdatePub")]
        public async Task<ActionResult> Update([FromForm] PubDataUpdate pubData)
        {
            try
            {
                var pub = _context.Publicacoes.Where(t => t.Id == pubData.id).FirstOrDefault();
                if(pub == null)
                {
                    return BadRequest("Publicação não encontrada");
                }
                else
                {
                    string host = "https://" + HttpContext.Request.Host.Value;
                    if(pubData.img.FileName == "imageNull")
                    {
                        string Folder = Path.Combine(Directory.GetCurrentDirectory(), $"Postagens\\{pub.Title}");
                        string newFolder = Path.Combine(Directory.GetCurrentDirectory(), $"Postagens\\{pubData.title}");
                        string imgExt = pub.ImgDirectory.Split('.').Last();
                        string imgName = pubData.title + '.' + imgExt;
                        System.IO.File.Move(Path.Combine(Directory.GetCurrentDirectory(), $"Postagens\\{pub.Title}\\{pub.Title}.{imgExt}"), $"Postagens\\{pub.Title}\\{imgName}");
                        string pubName = pubData.title + ".html";
                        Directory.Move(Folder, newFolder);
                        Directory.CreateDirectory(newFolder);
                        using (Stream stream = new FileStream(Path.Combine(newFolder, pubName), FileMode.Create))
                        {
                            pubData.pub.CopyTo(stream);
                        }
                        System.IO.File.Delete(newFolder + $"\\{pub.Title}.html");
                        pub.Title = pubData.title;
                        pub.SubTitle = pubData.subtitle;
                        pub.PubDirectory = host + $"/Postagens/{pubData.title.Replace(" ", "%20")}/{pubName}";
                        pub.ImgDirectory = host + $"/Postagens/{pubData.title.Replace(" ", "%20")}/{pubData.title}.{imgExt}";
                        _context.Entry(pub).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return Ok("Publicação atualizada com sucesso");
                    }
                    string imgExtension = pubData.img.FileName.Split('.')[1];
                    if (imgExtension == "jpg" || imgExtension == "png")
                    {
                        string Folder = Path.Combine(Directory.GetCurrentDirectory(), $"Postagens\\{pub.Title}");
                        string newFolder = Path.Combine(Directory.GetCurrentDirectory(), $"Postagens\\{pubData.title}");
                        Directory.Delete(Folder, true);
                        Directory.CreateDirectory(newFolder);
                        string imgName = pubData.title + '.' + imgExtension;
                        string pubName = pubData.title + ".html";
                        using (Stream stream = new FileStream(Path.Combine(newFolder, imgName), FileMode.Create))
                        {
                            pubData.img.CopyTo(stream);
                        }
                        using (Stream stream = new FileStream(Path.Combine(newFolder, pubName), FileMode.Create))
                        {
                            pubData.pub.CopyTo(stream);
                        }
                        pub.Title = pubData.title;
                        pub.SubTitle = pubData.subtitle;
                        pub.ImgDirectory = host + $"/Postagens/{pubData.title.Replace(" ", "%20")}/{imgName}";
                        pub.PubDirectory = host + $"/Postagens/{pubData.title.Replace(" ", "%20")}/{pubName}";
                        _context.Entry(pub).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return Ok("Publicação atualizada com sucesso");
                    }
                    else
                    {
                        return BadRequest("Arquivo de imagem não suportado");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
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

        [HttpDelete("{id:int}"), Authorize(Roles = "Admin")]
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