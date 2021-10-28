namespace PTASSyncService.Models
{
    public class ResponseModel
    {
        public int StatusCode;
        public string Message;
        public object Object;

        public ResponseModel(int statusCode)
        {
            StatusCode = statusCode;
        }

        public ResponseModel(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ResponseModel(int statusCode, object Object)
        {
            StatusCode = statusCode;
            this.Object = Object;
        }
    }
}
