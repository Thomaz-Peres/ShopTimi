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
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<ProductsEntity>>> Get([FromServices] DataContext context)
        {
            var products = await context.Products.AsNoTracking().ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductsEntity>> GetById([FromServices] DataContext context, int id)
        {
            var product = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(new { message = $"Produto encontrado {product}" });
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ProductsEntity>> Post([FromServices] DataContext context, [FromBody] ProductsEntity product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                context.Products.Add(product);
                await context.SaveChangesAsync();
                return Ok(new { message = "Produto cadastrado com sucesso" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel cadastrar o Produto" });
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ProductsEntity>> Put(int id, [FromBody] ProductsEntity product, [FromServices] DataContext context)
        {
            if (id != product.Id)
                return NotFound(new { message = "Produto não foi encontrado" });

         
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry(product).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new { message = "O produto foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar o produto" });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ProductsEntity>> Delete(int id, [FromServices] DataContext context)
        {
            var product = await context.Products.AsQueryable().Where(x => x.Id == id).FirstOrDefaultAsync();


            if (product == null)
                return NotFound(new { message = "Produto não encontrado" });

            if(product.Client  != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return Ok(new { message = "Produto removido com sucesso" });
            }
            else
            {
                return BadRequest(new { message = "O produto tem um cliente" });
            }
        }
    }
}
