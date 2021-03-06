using System;
using System.Linq;
using System.Text;
using Entities.DTOs;
using FluentValidation;
using Business.Abstract;
using Entities.Concrete;
using Business.Constans;
using System.Transactions;
using DataAccess.Abstract;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Core.Utilities.Business;
using Core.CrossCuttingConcerns;
using System.Collections.Generic;
using Core.Aspects.Autofac.Caching;
using DataAccess.Concrete.InMemory;
using Core.Aspects.Autofac.Validation;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Transaction;
using Business.ValidationRules.FluentValidation;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }


        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            //ValidationTool.Validate(new ProductValidator(), product);

            IResult result = BusinessRules.Run(
                CheckIfProductCategoryCorrect(product.CategoryID), 
                CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

        }
        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            //Business codes 
            //example: Authentication process 

            if (DateTime.Now.Hour == 2)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryID == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductID == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal decimalMin, decimal decimalMax)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice <= decimalMax && p.UnitPrice >= decimalMin));
        }

        public IDataResult<List<ProductDetailDto>> GetProductsDetails()
        {
            if (DateTime.Now.Hour == 11)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(), Messages.ProductsListed);
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            //Business Codes
            return new SuccessResult();
        }

        private IResult CheckIfProductCategoryCorrect(int categoryId)
        {
            int countOfProduct = _productDal.GetAll(p => p.CategoryID == categoryId).Count();
            if (countOfProduct > 9)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }

            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }

            return new SuccessResult();
        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
                
            Add(product);
            if (product.UnitPrice<10)
            {
                throw new Exception("");

            }
            Add(product);
           
            return null;
        }
    }
}
