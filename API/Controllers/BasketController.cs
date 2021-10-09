using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            CustomerBasket basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDTO basketdto)
        {
            CustomerBasket basket = _mapper.Map<CustomerBasketDTO, CustomerBasket>(basketdto);
            CustomerBasket updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketById(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}