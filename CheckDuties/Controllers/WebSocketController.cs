using CheckDuties.App.Commands.WebSocketCommand.WebSocketCommandHandler;
using CheckDuties.App.UsualDto;
using CheckDuties.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace CheckDuties.Controllers;

[ApiController]
[Route("ws")]
public class WebSocketController : ControllerBase
{
    private readonly WebSocketCommandHandler _wsHandler;

    public WebSocketController(WebSocketCommandHandler wsHandler)
    {
        _wsHandler = wsHandler;
    }

    [HttpGet("connect")]
    public async Task Connect()
    {
        var context = ControllerContext.HttpContext;

        if (!context.WebSockets.IsWebSocketRequest)
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        else
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await HandleWebSocketCommunication(webSocket);
        }
    }

    private async Task HandleWebSocketCommunication(WebSocket webSocket)
    {
        var wsKey = Guid.NewGuid().ToString();
        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var buffer = new byte[1024 * 4];
            var successConnectionMessage = new HandlersCommunicationDto<string>
            {
                CommunicationType = CommunicationType.Connection,
                WsKey = wsKey,
                Data = "Connection established"
            };
            var responseMessage = JsonSerializer.Serialize(successConnectionMessage);
            var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);

            await webSocket.SendAsync(
                new ArraySegment<byte>(responseBuffer),
                WebSocketMessageType.Text,
                true,
                cancellationToken
            );

            _wsHandler.AddSocket(successConnectionMessage.WsKey, webSocket);

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                }
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await webSocket.SendAsync(Encoding.UTF8.GetBytes(responseMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await _wsHandler.RemoveSocketAsync(wsKey, webSocket);
            }
        }
    }
}



