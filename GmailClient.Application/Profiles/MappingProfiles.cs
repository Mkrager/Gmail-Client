using AutoMapper;
using GmailClient.Application.DTOs;
using GmailClient.Application.Features.Gmails.Queries.GetMessagesList;

namespace GmailClient.Application.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GmailMessageDto, GetMessagesListVm>().ReverseMap();
        }
    }
}
