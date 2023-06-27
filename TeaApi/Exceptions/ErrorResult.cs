using System.Text.Json.Serialization;

namespace com.mahonkin.tim.TeaApi.Exceptions
{
    [JsonSerializable(typeof(ErrorResult))]
    internal class ErrorResult
    {
        [JsonInclude]
        public System.Net.HttpStatusCode ErrorCode;
        [JsonInclude]
        public string Message;

        [JsonConstructor]
        public ErrorResult(System.Net.HttpStatusCode code, string message)
        {
            ErrorCode = code;
            Message = message;
        }
    }
}