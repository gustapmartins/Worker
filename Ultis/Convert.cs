using System.Text.Json;
using Worker.ExceptionFilter;
using Worker.Interface;

namespace Worker.Ultis;

public class Convert : IConvert
{

    public T DeserializeMessage<T>(string message)
    {
        return JsonSerializer.Deserialize<T>(message);
    }

    public TResult HandleErrorAsync<TResult>(Func<TResult> serviceMethod)
    {
        try
        {
            TResult result = serviceMethod.Invoke();

            if (result == null)
            {
                throw new NotFoundException("Este valor não existe");
            }

            return result;
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
            {
                throw;
            }

            throw new NotFoundException("Erro na solicitação", ex);
        }
    }
}
