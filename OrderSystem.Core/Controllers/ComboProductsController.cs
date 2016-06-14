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
    public class ComboProductsController : ApiController
    {
        private OrderDbContext db = new OrderDbContext();


        // GET: api/ComboProducts/5
        [ResponseType(typeof(ComboProduct))]
        public async Task<IHttpActionResult> PostComboProduct(ProductQueryModel pqm)
        {
            List<ComboProduct> comboProducts = new List<ComboProduct>();


            if (string.IsNullOrEmpty(pqm.ProductName) && string.IsNullOrEmpty(pqm.ProductCode))
            {
                return NotFound();
            }
            else
            {
                if (string.IsNullOrEmpty(pqm.ProductCode))
                {
                    comboProducts = await (from p in db.ComboProducts where p.ComboName.Contains(pqm.ProductName) select p).ToListAsync();
                }
                else
                    if (string.IsNullOrEmpty(pqm.ProductName))
                    {
                        comboProducts = await (from p in db.ComboProducts where p.ComboCode.Contains(pqm.ProductCode) select p).ToListAsync();
                    }
                    else
                    {
                        comboProducts = await (from p in db.ComboProducts where p.ComboCode.Contains(pqm.ProductCode) && p.ComboName.Contains(pqm.ProductName) select p).ToListAsync();
                    }
            }

            if (comboProducts.Count == 0)
            {
                return NotFound();
            }
            var pvms = comboProducts.Select(v => new ProductViewModel
            {
                Id = v.Id,
                Name = v.ComboName,
                ProductCode = v.ComboCode,
                BeginDate = v.ValidStartDate,
                EndDate = v.ValidEndDate,
                Price = v.Price,
                Provider = "套餐",
                Type = ProductType.Combo
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

        private bool ComboProductExists(int id)
        {
            return db.ComboProducts.Count(e => e.Id == id) > 0;
        }
    }
}