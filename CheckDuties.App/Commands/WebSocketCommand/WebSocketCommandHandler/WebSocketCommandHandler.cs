using System.Net.WebSockets;

namespace CheckDuties.App.Commands.WebSocketCommand.WebSocketCommandHandler;

public class WebSocketCommandHandler
{
    private readonly Dictionary<string, WebSocket> _sockets = new Dictionary<string, WebSocket>();

    public void AddSocket(string id, WebSocket socket)
    {
        _sockets[id] = socket;
    }

    public async Task RemoveSocketAsync(string id, WebSocket socket)
    {
        if (_sockets.ContainsKey(id))
            _sockets.Remove(id);

        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close connection by server", CancellationToken.None);
    }
}
