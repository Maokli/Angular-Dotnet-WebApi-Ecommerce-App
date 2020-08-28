using System;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersOrCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersOrCountSpecification(ProductSpecParams productParams)
        :base(x => 
        (String.IsNullOrEmpty(productParams.Search) || x.Name.ToLower()
        .Contains(productParams.Search)) &&
        (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
        (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
        {
            
        }
    }
}