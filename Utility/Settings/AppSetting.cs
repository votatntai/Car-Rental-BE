namespace Utility.Settings
{
    public class AppSetting
    {
        public string Secret { get; set; } = null!;
        public string Bucket { get; set; } = null!;
        public string Folder { get; set; } = null!;
        public string VNPayUrl { get; set; } = null!;
        public string ReturnUrl { get; set; } = null!;
        public string MerchantId { get; set; } = null!;
        public string MerchantPassword { get; set; } = null!;
    }
}
