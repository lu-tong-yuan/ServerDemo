namespace serverDemo.Services
{
    using BCryptNet = BCrypt.Net.BCrypt;

    public class PasswordService
    {
        public string HashPassword(string password)
        {
            // 生成密碼的雜湊值
            string hashedPassword = BCryptNet.HashPassword(password);

            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // 驗證密碼是否匹配雜湊值
            bool passwordMatches = BCryptNet.Verify(password, hashedPassword);

            return passwordMatches;
        }
    }

}
