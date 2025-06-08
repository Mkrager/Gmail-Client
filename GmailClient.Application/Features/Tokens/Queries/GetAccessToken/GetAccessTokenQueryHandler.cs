//using AutoMapper;
//using GmailClient.Application.Contracts.Infrastructure;
//using MediatR;

//namespace GmailClient.Application.Features.Tokens.Queries.GetAccessToken
//{
//    public class GetAccessTokenQueryHandler : IRequestHandler<GetAccessTokenQuery, string>
//    {
//        private readonly ITokenService _tokenService;
//        public GetAccessTokenQueryHandler(ITokenService tokenService)
//        {
//            _tokenService = tokenService;
//        }
//        public async Task<string> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
//        {
//            var accessToken = await _tokenService.GetAccessTokenAsync(request.refreshToken);

//            if (accessToken.AccessToken == null)
//            {
//                throw new ArgumentNullException(nameof(accessToken));
//            }

//            return accessToken;
//        }
//    }
//}
