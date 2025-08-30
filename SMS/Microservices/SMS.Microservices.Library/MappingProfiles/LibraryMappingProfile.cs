using AutoMapper;
using SMS.Contracts.Library;
using SMS.Microservices.Library.Models;

namespace SMS.Microservices.Library.MappingProfiles;

public class LibraryMappingProfile : Profile
{
    public LibraryMappingProfile()
    {
        CreateMap<Book, BookResponse>();
        CreateMap<BookLoan, BookLoanResponse>();
        CreateMap<CreateBookRequest, Book>();
        CreateMap<UpdateBookRequest, Book>();
        CreateMap<UpdateBookLoanRequest, BookLoan>();
    }
}
