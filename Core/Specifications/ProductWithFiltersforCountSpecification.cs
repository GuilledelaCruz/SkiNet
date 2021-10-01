using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersforCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersforCountSpecification(ProductSpecificationParams productParams) :
            base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.Brand.HasValue || x.ProductBrandId == productParams.Brand) &&
            (!productParams.Type.HasValue || x.ProductTypeId == productParams.Type))
        { }
    }
}
