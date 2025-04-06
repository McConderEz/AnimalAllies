using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Services;

namespace TelegramBotService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserStateServices _userStateService;
    private readonly ITelegramBotClient _botClient;

    public AccountController(
        IMediator mediator,
        UserStateServices userStateService,
        ITelegramBotClient botClient)
    {
        _mediator = mediator;
        _userStateService = userStateService;
        _botClient = botClient;
    }

    [HttpPost]
    [Consumes("application/json")]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        var message = update.Message;
        if (message == null) return Ok();

        var chatId = message.Chat.Id;
        var currentState = _userStateService.GetState(chatId);
        var nextState = await currentState.HandleAsync(message, _botClient);
        _userStateService.SetState(chatId, nextState);

        return Ok();
    }
}