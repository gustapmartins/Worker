using System.Text.Json;
using Worker.Interface;

namespace Worker.Ultis;

public class Convert: IConvert
{

    public T DeserializeMessage<T>(string message)
    {
        return JsonSerializer.Deserialize<T>(message);
    }
}
