using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using ProjectApi.Models;

namespace ProjectApi.Controllers
{
   //[Authorize]
    public class ProductsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        /*[ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

*/
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSave(int id)
        {
            string Image = null;
            var httpRequest = HttpContext.Current.Request;

            var postedFile = httpRequest.Files["Image"];

            Image = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");

            Image = Image + DateTime.Now.ToString("yymmssff") + Path.GetExtension(postedFile.FileName);

            var filePath = HttpContext.Current.Server.MapPath("~/Image/" + Image);
            postedFile.SaveAs(filePath);

            Product p = new Product()
            {
                Name = httpRequest["Name"],
                Description = httpRequest["Description"],
                Category = httpRequest["Category"],
                price = double.Parse(httpRequest["Price"]),
                Image = Image

            };
            Product prod = db.Products.FirstOrDefault(pr => pr.Id == id);
            prod.Name = p.Name;
            prod.Description = p.Description;
            prod.Category = p.Category;
            prod.price = p.price;
            prod.Image = p.Image;

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);

        }

        //POST: api/Products
        /* [ResponseType(typeof(Product))]
          public IHttpActionResult PostProduct(Product product)
          {
              if (!ModelState.IsValid)
              {
                  return BadRequest(ModelState);
              }

              db.Products.Add(product);
              db.SaveChanges();

              return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
          }*/


        [HttpPost]
        public HttpResponseMessage Upload()
        {
            string Image = null;
            var httpRequest = HttpContext.Current.Request;

            var postedFile = httpRequest.Files["Image"];

            Image = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");

            Image = Image + DateTime.Now.ToString("yymmssff") + Path.GetExtension(postedFile.FileName);

            var filePath = HttpContext.Current.Server.MapPath("~/Uploads/" + Image);
            postedFile.SaveAs(filePath);

            Product p = new Product()
            {
                Name = httpRequest["Name"],
                Description = httpRequest["Description"],
                price = double.Parse(httpRequest["price"]),
                Image = Image,
                Category= httpRequest["Category"]

            };
            db.Products.Add(p);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);

        }
        //[HttpPost]
        //[ResponseType(typeof(Product))]
        //public IHttpActionResult UploadFile(Product product)
        //{
        //    var ctx = HttpContext.Current;
        //    var root = ctx.Server.MapPath("~/Uploads");
        //    var provider = new MultipartFormDataStreamProvider(root);
        //    var localFileName = "";

        //         Request.Content.ReadAsMultipartAsync(provider);
        //        foreach (var file in provider.FileData)
        //        {
        //            var name = file.Headers.ContentDisposition.FileName;
        //            //Remove double quotes from string
        //            name = name.Trim('"');

        //             localFileName = file.LocalFileName;
        //            var filePath = Path.Combine(root, name);
        //            File.Move(localFileName, filePath);
        //        }
        //    product.Image = localFileName;
        //    db.Products.Add(product);
        //    db.SaveChanges();
        //    return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);

        //}


        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        [HttpDelete]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
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