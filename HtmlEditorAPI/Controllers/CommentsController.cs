using CentennialAPI.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentennialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("Pub/{id:int}", Name = "GetByPubId")]
        public async Task<ActionResult<IAsyncEnumerable<Comment>>> GetbyPub(int id)
        {
            try
            {
                var comments = _context.Comments.Where(c => c.PublicacaoId == id);
                return Ok(comments);
            }
            catch (Exception ex){
                return BadRequest(ex);
            }
        }
        [HttpGet("User/{id:int}", Name = "GetByUserId")]
        public async Task<ActionResult<IAsyncEnumerable<Comment>>> GetbyUser(int id)
        {
            try
            {
                var comments = _context.Comments.Where(c => c.UserId == id);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Authorize]
        public ActionResult Post(CommentModel commentModel)
        {
            try
            {
                Comment comment = new Comment();
                comment.UserId = commentModel.UserId;
                comment.PublicacaoId = commentModel.PubId;
                comment.CommentText = commentModel.CommentText;
                comment.CreatedAt = DateTime.Now;

                _context.Comments.Add(comment);
                _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        [Authorize]
        public ActionResult Put(CommentModelUpdate commentRequest)
        {
            try
            {
                var comment = _context.Comments.Where(c => c.Id == commentRequest.id).FirstOrDefault();
                if (comment == null)
                {
                    return BadRequest("Requisição inválida");
                }
                comment.CommentText = commentRequest.NewComment;
                _context.Entry(comment).State = EntityState.Modified;
                _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpDelete("{id:int}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                var comment = _context.Comments.Where(c => c.Id == id).FirstOrDefault();
                if (comment == null)
                {
                    return BadRequest("Requisição inválida");
                }
                _context.Comments.Remove(comment);
                _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
