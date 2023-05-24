namespace SmartParking.WebApp.Models.AdminGetDetails
{
    public class Operator
    {
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        /* public string Password { get; set; }*/
        public string ContactNo { get; set; }
    }
    public class OperatorResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<Operator> OperatorList { get; set; }
    }
}
