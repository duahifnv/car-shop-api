using AutoMapper;
using cart_service.Dto;
using cart_service.Models;

namespace cart_service.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductResponse>();
        CreateMap<ProductRequest, Product>();
        CreateMap<ProductResponse, Product>();
        CreateMap<CategoryRequest, Category>();
        // Маппинг для CartItem
        CreateMap<CartItem, CartItemDto>();
        CreateMap<CartItemDto, CartItem>();
        // Маппинг для Cart
        CreateMap<Cart, CartDto>();
        CreateMap<CartDto, Cart>();
    }
}