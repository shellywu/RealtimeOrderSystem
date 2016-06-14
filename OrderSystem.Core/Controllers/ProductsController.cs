using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using OrderSystem.Core.Models;

namespace OrderSystem.Core.Controllers
{
    public class ProductsController : ApiController
    {
        private OrderDbContext db = new OrderDbContext();

        // GET: api/Products/5
        [ResponseType(typeof(List<ProductViewModel>))]
        public async Task<IHttpActionResult> PostProduct(ProductQueryModel pqm)
        {
            List<Product> products = new List<Product>();


            if (string.IsNullOrEmpty(pqm.ProductName) && string.IsNullOrEmpty(pqm.ProductCode))
            {
                return NotFound();
            }
            else
            {
                if (string.IsNullOrEmpty(pqm.ProductCode))
                {
                    products = await (from p in db.Products.Include(p => p.Partner) where p.ProductName.Contains(pqm.ProductName) select p).ToListAsync();
                }
                else
                    if (string.IsNullOrEmpty(pqm.ProductName))
                    {
                        products = await (from p in db.Products.Include(p => p.Partner) where p.ProductCode.Contains(pqm.ProductCode) select p).ToListAsync();
                    }
                    else
                    {
                        products = await (from p in db.Products.Include(p => p.Partner) where p.ProductCode.Contains(pqm.ProductCode) && p.ProductName.Contains(pqm.ProductName) select p).ToListAsync();
                    }
            }

            if (products.Count == 0)
            {
                return NotFound();
            }
            var pvms = products.Select(v => new ProductViewModel
            {
                Id = v.Id,
                Name = v.ProductName,
                ProductCode = v.ProductCode,
                BeginDate = v.ValidStartDate,
                EndDate = v.ValidEndDate,
                Price = v.Price,
                Provider = v.Partner.Name,
                Type = v.ProductType
            });



            return Ok(pvms);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}