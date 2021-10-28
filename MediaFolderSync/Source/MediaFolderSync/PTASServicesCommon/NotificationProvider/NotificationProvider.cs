using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PTASServicesCommon.TokenProvider;

/// <summary>
/// Internal notification sender.
/// </summary>
public class NotificationProvider
{
    private readonly string apimEndpoint;
    private readonly string notificatioServiceResourceId;
    private readonly string apimSubscriptionKey;
    private readonly string magicLinkServiceClientId;
    private readonly string magicLinkServiceClientSecret;
    private readonly string undeliverableEmailRecipient;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationProvider"/> class.
    /// </summary>
    /// <param name="apimEndpoint">APIM Endpoint.</param>
    /// <param name="notificationServiceResourceId">NotificationServiceResourceId.</param>
    /// <param name="apimSubscriptionKey">ApimSubscriptionKey.</param>
    /// <param name="magicLinkServiceClientId">MagicLinkServiceClientId.</param>
    /// <param name="magicLinkServiceClientSecret">MagicLinkServiceClientSecret.</param>
    /// <param name="undeliverableEmailRecipient">UndeliverableEmailRecipient.</param>
    public NotificationProvider(string apimEndpoint, string notificationServiceResourceId, string apimSubscriptionKey, string magicLinkServiceClientId, string magicLinkServiceClientSecret, string undeliverableEmailRecipient)
    {
        this.apimEndpoint = apimEndpoint;
        this.notificatioServiceResourceId = notificationServiceResourceId;
        this.apimSubscriptionKey = apimSubscriptionKey;
        this.magicLinkServiceClientId = magicLinkServiceClientId;
        this.magicLinkServiceClientSecret = magicLinkServiceClientSecret;
        this.undeliverableEmailRecipient = undeliverableEmailRecipient;
    }

    /// <summary>
    /// Send a notification through the APIM.
    /// </summary>
    /// <param name="from">from email.</param>
    /// <param name="recepients">recepients for message.</param>
    /// <param name="subject">Subject.</param>
    /// <param name="body">Text body to send.</param>
    /// <returns>Task.</returns>
    public async Task SendNotification(string from, string recepients, string subject, string body)
    {
        string notificationServiceToken = await new AzureTokenProvider().GetKcServiceAccessTokenAsync(
            this.apimEndpoint,
            this.notificatioServiceResourceId,
            this.apimSubscriptionKey,
            this.magicLinkServiceClientId,
            this.magicLinkServiceClientSecret);

        HttpClient clientNotify = this.CreateClient(Guid.NewGuid(), notificationServiceToken);

        Uri apimUri = new Uri(this.apimEndpoint);
        Uri tokenServiceUri = new Uri(apimUri, "/notifications/v1/email");

        var addresses = recepients.Split(';');
        foreach (var r in addresses)
        {
            byte[] byteData = this.CreateEmailBytes(from, r, subject, body);
            using (var content = new ByteArrayContent(byteData))
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await clientNotify.PostAsync(tokenServiceUri.ToString(), content);
                var responseStr = await response.Content.ReadAsStringAsync();
            }
        }
    }

    private HttpClient CreateClient(Guid correlationID, string notificationServiceToken)
    {
        var clientNotify = new HttpClient();

        // Request headers
        // Add unique correlation id
        clientNotify.DefaultRequestHeaders.Add("kc-correlation-id", correlationID.ToString());
        clientNotify.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.apimSubscriptionKey);
        clientNotify.DefaultRequestHeaders.Add("Authorization", "Bearer " + notificationServiceToken);
        return clientNotify;
    }

    private byte[] CreateEmailBytes(string from, string recepients, string subject, string body)
    {
        string str = JsonConvert.SerializeObject(new
        {
            notificationEvent = new
            {
                id = "5d883c77-831d-46e2-a91e-907e795518a2",
                timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffff"),
                type = "email",
                notification = new
                {
                    attachments = new string[] { },
                    bccList = new string[] { },
                    body = body,
                    ccList = new string[] { },
                    from = from,
                    fromDisplayName = "KC PTAS",
                    importance = "high",
                    isHtml = true,
                    replyToList = new string[] { },
                    subject = subject,
                    toList = new string[] { recepients },
                    undeliverableEmail = this.undeliverableEmailRecipient,
                },
            },
        });
        return Encoding.UTF8.GetBytes(str);
    }
}
