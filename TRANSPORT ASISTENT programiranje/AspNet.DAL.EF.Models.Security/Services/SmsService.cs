using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AspNet.DAL.EF.Models.Security
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        { return Task.FromResult(0); }

        // Demo template
        //public Task SendAsync(IdentityMessage message)
        //{
        //    string AccountSid = "YourTwilioAccountSID";
        //    string AuthToken = "YourTwilioAuthToken";
        //    string twilioPhoneNumber = "YourTwilioPhoneNumber";

        //    var twilio = new TwilioRestClient(AccountSid, AuthToken);

        //    twilio.SendSmsMessage(twilioPhoneNumber, message.Destination, message.Body);

        //    // Twilio does not return an async Task, so we need this:
        //    return Task.FromResult(0);
        //}
    }
}
