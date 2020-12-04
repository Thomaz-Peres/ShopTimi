using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimiPro.Data;
using TimiPro.Model;

namespace TimiPro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<ClientEntity>>> Get([FromServices] DataContext context)
        {
            var clients = await context.Clients.AsNoTracking().ToListAsync();
            return Ok(clients);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ClientEntity>> GetById([FromServices] DataContext context, int id)
        {
            try
            {
                var client = await context.Clients.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
                return Ok(client);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Cliente não existe" });
            }
            
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ClientEntity>> Post([FromServices] DataContext context, [FromBody] ClientEntity client)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                context.Clients.Add(client);
                await context.SaveChangesAsync();
                return Ok(new { message = "Cliente criado com sucesso" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel cadastrar o cliente" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ClientEntity>> Put([FromServices] DataContext context, int id,[FromBody] ClientEntity client)
        {
            //  Verifica se os dados sao válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //  Verifica se o ID informado é o mesmo do modelo
            if (id != client.Id)
                return NotFound(new { message = "Cliente não encontrado" });

            try
            {
                context.Entry(client).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return client;
            }
            catch
            {
                return BadRequest(new { message = "Não foi possivel atualizar o cliente" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ClientEntity>> Delete(int id, [FromServices] DataContext context)
        {
            var client = await context.Clients.FirstOrDefaultAsync(x => x.Id == id);
            
            if (client == null)
                return NotFound(new { message = "Cliente não encontrado" });

            try
            {
                context.Clients.Remove(client);
                await context.SaveChangesAsync();
                return Ok(new { message = "O cliente foi removido com sucesso" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel remover o cliente" });
            }
        }
    }
}
