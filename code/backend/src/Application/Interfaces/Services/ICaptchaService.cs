namespace budgetApplyApi.Application.Interfaces.Services
{
    public interface ICaptchaService
    {
        string ComputeMd5Hash(string input);
        byte[] GenerateCaptchaImage(string text);
        string GenerateRandomText(int textLength);
    }
}
