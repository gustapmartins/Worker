namespace Worker.Interface;

public interface IConvert
{
    T DeserializeMessage<T>(string message);

    TResult HandleErrorAsync<TResult>(Func<TResult> serviceMethod);
}
