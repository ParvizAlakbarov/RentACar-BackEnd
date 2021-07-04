using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public List<Product> GetAll()
        {
            //Is kodlari bura yazilmalidir 
            //example: Authentication process 
           return  _productDal.GetAll();
        }

        public List<Product> GetAllByCategoryId(int id)
        {
            return _productDal.GetAll(p=>p.CategoryID == id);
        }

        public List<Product> GetByUnitPrice(decimal decimalMin, decimal decimalMax)
        {
            return _productDal.GetAll(p => p.UnitPrice <= decimalMax && p.UnitPrice >= decimalMin);
        }

        public List<ProductDetailDto> GetProductsDetails()
        {
            return _productDal.GetProductDetails();
        }
    }
}
