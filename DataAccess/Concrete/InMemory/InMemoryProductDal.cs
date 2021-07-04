    using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryProductDal : IProductDal
    {
        List<Product> _products;
        public InMemoryProductDal()
        {
            _products = new List<Product> {
                new Product{ProductID=1,CategoryID=1,ProductName="Glass",UnitPrice=15,UnitsInStock= 15 },
                new Product{ProductID=2,CategoryID=1,ProductName="Camera",UnitPrice=500,UnitsInStock= 3 },
                new Product{ProductID=3,CategoryID=2,ProductName="Phone",UnitPrice=1500,UnitsInStock= 2 },
                new Product{ProductID=4,CategoryID=2,ProductName="Keyboard",UnitPrice=150,UnitsInStock= 65 },
                new Product{ProductID=5,CategoryID=2,ProductName="Mouse",UnitPrice=85,UnitsInStock= 1 }

            };
        }
        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {
            //Product productToDelete = null;
            //foreach (var pro in _products)
            //{
            //    if (product.ProductID == pro.ProductID)
            //    {
            //        productToDelete = pro;
            //    }
                
            //}

            //
            //bu te  madi basa salmaq ucun 
            //productToDelete = _products.SingleOrDefault(pro=>pro.ProductID == product.ProductID);
            Product productToDelete = _products.SingleOrDefault(pro => pro.ProductID == product.ProductID);

            _products.Remove(productToDelete);
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return _products;
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllByCategory(int categoryId)
        {
            //List<Product> tempList = null;

            //foreach (var pro in _products)
            //{
            //    if (pro.CategoryID == categoryId)
            //    {
            //        tempList.Add(pro);
            //    }

            //}
            //return tempList;

            //BU QEDER SEY YAZMAQDANSA LINQ LE BU COX ASANDIR

                return _products.Where(p => p.CategoryID == categoryId).ToList();

        }

        public List<ProductDetailDto> GetProductDetails()
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            Product productToUpdate = _products.SingleOrDefault(pro => pro.ProductID == product.ProductID);
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;
            productToUpdate.CategoryID = product.CategoryID;
            productToUpdate.ProductName = product.ProductName;
        }
    }
}
