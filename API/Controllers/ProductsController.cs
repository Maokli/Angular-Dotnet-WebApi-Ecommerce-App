using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using AutoMapper;
using API.DTOs;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
  public class ProductsController : BaseApiController
  {
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
    private readonly IGenericRepository<ProductType> _productTypesRepo;
    private readonly IMapper _mapper;

    public ProductsController(
      IGenericRepository<Product> productRepo,
      IGenericRepository<ProductBrand> productBrandsRepo,
      IGenericRepository<ProductType> productTypesRepo,
      IMapper mapper)
    {
      _productRepo = productRepo;
      _productBrandsRepo = productBrandsRepo;
      _productTypesRepo = productTypesRepo;
      _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts()
    {
      var spec = new ProductsWithTypesAndBrandsSpecification();
      var products = await _productRepo.ListAsync(spec);
      return Ok(_mapper
      .Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDTO>>(products));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
    {
      var spec = new ProductsWithTypesAndBrandsSpecification(id);
      var product = await _productRepo.GetEntityWithSpec(spec);
      if(product == null)
        return NotFound(new ApiResponse(404));
      return Ok(_mapper.Map<Product,ProductToReturnDTO>(product));
    }
    [HttpGet("brands")]
    public async Task<ActionResult<Product>> GetBrands()
    {
      return Ok(await _productBrandsRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<Product>> GetTypes()
    {
      return Ok(await _productTypesRepo.ListAllAsync());
    }
  }
}