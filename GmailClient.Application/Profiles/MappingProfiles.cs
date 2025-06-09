using AutoMapper;
using GmailClient.Application.DTOs;
using GmailClient.Application.Features.Account.Commands.Registration;
using GmailClient.Application.Features.Account.Queries.Authentication;
using GmailClient.Application.Features.Gmails.Queries.GetMessagesList;
using GmailClient.Application.Features.Tokens.Commands.SaveTokens;
using GmailClient.Application.Features.User.Queries;
using GmailClient.Domain.Entities;

namespace GmailClient.Application.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GmailMessageDto, GetMessagesListDto>().ReverseMap();
            CreateMap<GmailMessageResponse, GetMessagesListVm>().ReverseMap();

            CreateMap<RegistrationRequest, RegistrationCommand>().ReverseMap();

            CreateMap<AuthenticationRequest, AuthenticationQuery>().ReverseMap();
            CreateMap<AuthenticationResponse, AuthenticationVm>().ReverseMap();

            CreateMap<SaveTokensCommand, UserGmailToken>();

            CreateMap<UserDetailsResponse, UserDetailsVm>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
