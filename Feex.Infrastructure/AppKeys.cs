using static Feex.Infrastructure.AppKeys;

namespace Feex.Infrastructure
{
    public class AppKeys
    {
        public SyncBridgeSetting SyncBridge { get; set; }
        public PdfDocumentService PdfDocumnetService { get; set; }
        public NotificationServiceKeys NotificationService {  get; set; }
        public JobSwitch JobSwitch {  get; set; }
        public CBAKeys Cba { get; set; }
        public PassportServiceSettings PassportService {  get; set; }

    }


    public class NotificationServiceKeys
    {
        public string AuthKey { get; set; }
        public string BaseUrl { get; set; }
        public string ErrorDestinationEmail { get; set; }
    }

    public class PdfDocumentService
    {
        public string DocumentconvertBaseurl { get; set; }
    }

    public class CBAKeys
    {
        public string BaseUrl { get; set; }
        public string Authorization { get; set; }
        public string FineractPlatformTenantId { get; set; }
    }
    public class SyncBridgeSetting
    {
        public string BaseUrl { get; set; }
        public long DefaultTenant { get; set; }
        public string S3Bucket { get; set; }

    }
    public class JobSwitch
    {
        public string TestSwitch { get; set; }
    }
    public class PassportServiceSettings
    {
        public string AuthKey { get; set; }
        public string BaseUrl { get; set; }
    }

}
