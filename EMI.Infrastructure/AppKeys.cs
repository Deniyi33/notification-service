using static EMI.Infrastructure.AppKeys;

namespace EMI.Infrastructure
{
    public class AppKeys
    {
        public SyncBridgeSetting SyncBridge { get; set; } = null!;
        public PdfDocumentService PdfDocumnetService { get; set; } = null!;
        public NotificationServiceKeys NotificationService { get; set; } = null!;
        public JobSwitch JobSwitch { get; set; } = null!;
        public CBAKeys Cba { get; set; } = null!;
        public PassportServiceSettings PassportService { get; set; } = null!;

    }


    public class NotificationServiceKeys
    {
        public string AuthKey { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
        public string ErrorDestinationEmail { get; set; } = null!;
    }

    public class PdfDocumentService
    {
        public string DocumentconvertBaseurl { get; set; } = null!;
    }

    public class CBAKeys
    {
        public string BaseUrl { get; set; } = null!;
        public string Authorization { get; set; } = null!;
        public string FineractPlatformTenantId { get; set; } = null!;
    }
    public class SyncBridgeSetting
    {
        public string BaseUrl { get; set; } = null!;
        public long DefaultTenant { get; set; }
        public string S3Bucket { get; set; } = null!;

    }
    public class JobSwitch
    {
        public string TestSwitch { get; set; } = null!;
    }
    public class PassportServiceSettings
    {
        public string AuthKey { get; set; } = null!;
        public string BaseUrl { get; set; } = null!;
    }

}
