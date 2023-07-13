

namespace SmartParking.WebApp.Models.AdminGetDetails
{
    public class AdminLoginRequest
    {

        public string AdminId { get; set; }

        
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class AdminLoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AdminLoginInformation data { get; set; }
        public AdminToken AdminToken { get; set; }

        internal IEnumerable<AdminLoginRequest> AdminLoginAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class AdminLoginInformation
    {
        public int AdminId { get; set; }
        /*        public string Password { get; set; }*/
        public string Email { get; set; }
        public string Role { get; set; }

    }
    public class AdminToken
    {
        public string JWTToken { get; set; }
        public string AdminRefreshToken { get; set; }
    }
}
