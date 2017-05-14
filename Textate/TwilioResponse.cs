using System.Net.Http;
using Twilio.TwiML;
using System.Text;

namespace Textate
{
    public class TwilioResponse
    {
        public string Message { get; }

        public TwilioResponse(string message)
        {
            Message = message;
        }

        public HttpResponseMessage ToHttpResponseMessage()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(new MessagingResponse()
                                .Message(Message)
                                .ToString()
                                .Replace("utf-16", "utf-8"),
                            Encoding.UTF8,
                            "application/xml")
            };
        }
    }
}