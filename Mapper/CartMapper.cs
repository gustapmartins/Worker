using AutoMapper;
using Worker.DTO;
using Worker.Model;

namespace Worker.Mapper;

public class CartMapper: Profile
{
    public CartMapper()
    {
        CreateMap<CreateCartDto, Carts>();
    }
}
