using GmailClient.Application.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController(IMediator mediator, ICurrentUserService currentUserService) : Controller
    {

    }
}
