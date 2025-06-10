using AutoMapper;
using GmailClient.Application.Contracts.Infrastructure;
using GmailClient.Application.Contracts.Persistance;
using GmailClient.Domain.Entities;
using MediatR;

namespace GmailClient.Application.Features.Tokens.Commands.SaveTokens
{
    public class SaveTokensCommandHandler : IRequestHandler<SaveTokensCommand, Guid>
    {
        private readonly IAsyncRepository<UserGmailToken> _tokenRepository;
        private readonly ITokenEncryptionService _tokenEncryptionService;
        private readonly IMapper _mapper;

        public SaveTokensCommandHandler(IAsyncRepository<UserGmailToken> tokenRepository, IMapper mapper, ITokenEncryptionService tokenEncryptionService)
        {
            _tokenRepository = tokenRepository;
            _mapper = mapper;
            _tokenEncryptionService = tokenEncryptionService;
        }

        public async Task<Guid> Handle(SaveTokensCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<UserGmailToken>(request);

            entity.ExpiresAt = DateTime.UtcNow.AddSeconds(request.ExpiresAt);

            entity.RefreshToken = _tokenEncryptionService.Encrypt(entity.RefreshToken);

            var added = await _tokenRepository.AddAsync(entity);

            return added.Id;
        }
    }
}
