using CheckDuties.Domain.Enums;

namespace CheckDuties.App.UsualDto;

public class HandlersCommunicationDto<T>
{
    public CommunicationType CommunicationType { get; set; }
    public string WsKey { get; set; }
    public T Data { get; set; }
}
