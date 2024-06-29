using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace WebAPI.Utilities.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Aşağıda BookDtoForUpdate'in bir book nesnesine gönderileceğini bildiemiş olduk
        CreateMap<BookDtoForUpdate, Book>().ReverseMap(); //CreateMap<Source, Destination>()

        CreateMap<Book, BookDto>();

        CreateMap<BookDtoForInsertion, Book>();
    }
}