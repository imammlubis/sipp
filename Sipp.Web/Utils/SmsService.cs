using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;

namespace Sipp.Web.Utils
{
    public class SmsService
    {
        public string SendSms(SmsContent content) {
            string AccountSid = "AC17765be1b711b56d35999e27f212b202";
            string AuthToken = "3e1225f2f319500e7c04d98f7131db41";
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var message = twilio.SendMessage(
                "+14133043999", content.To,
                content.Body
            );

            return message.Sid;
        }
    }
}