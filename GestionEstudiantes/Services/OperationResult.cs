namespace GestionEstudiantes.Services
{
    public class OperationResult
    {
        public string message { get; set; }
        public bool success { get; set; }
        public dynamic? data { get; set; }

        public OperationResult(string message, bool success, dynamic? data = null)
        {
            this.message = message;
            this.success = success;
            this.data = data;
        }

        public static OperationResult SuccessResult(string message, dynamic? data = null)
        {
            return new OperationResult(message, true, data);
        }

        public static OperationResult FailureResult(string message, dynamic? data = null)
        {
            return new OperationResult(message, false, data);
        }
    }
}
