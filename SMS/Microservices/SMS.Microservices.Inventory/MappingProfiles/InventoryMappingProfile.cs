using AutoMapper;
using SMS.Contracts.Inventory;
using SMS.Microservices.Inventory.Models;

namespace SMS.Microservices.Inventory.MappingProfiles;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<InventoryItem, InventoryItemResponse>();
        CreateMap<CreateInventoryItemRequest, InventoryItem>();
        CreateMap<UpdateInventoryItemRequest, InventoryItem>();
    }
}
