using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DSNParser;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.ReadKey();
        }

        public static void Test()
        {
            
            if (MailDeliveryInfo.IsDsn(PARTIAL_MESSAGE))
            {
                var report = MailDeliveryInfo.TryCreate(RAW_MESSAGE);
                if (report != null)
                {
                    Console.WriteLine("{0}\r\n{2}\r\nRaw report:\r\n{1}",
                        report.Date,
                        report.RawReport,
                        string.Join(Environment.NewLine,
                            report.Status
                            .Select(t =>
                                string.Format("{0}: {1} ({2})",
                                t.Key,
                                t.Value.GetMostSignificantClassificationString(),
                                t.Value.MostSignificantStatusCode))
                            .ToArray()));
                }
                else
                {
                    Console.WriteLine("Failed to parse this message.");
                }
            }
            else
            {
                Console.WriteLine("Not a DSN.");
            }
        }

        const string PARTIAL_MESSAGE = @"Return-path: <>
Received: from xxx ([xxx])
    by xxx with ESMTP; Fri, 04 May 2012 16:18:13 +0200
From: <Mailer-Daemon@xxx> (Mail Delivery System)
To: xxx
Subject: Undelivered Mail Returned to Sender
Date: Fri, 04 May 2012 15:25:09 +0200
MIME-Version: 1.0
Content-Type: multipart/report; report-type=delivery-status;
 boundary=""HTB3nt3RR7vw/QMPR4kDPbKg+XWjXIKdC/rfHQ==""

";
        const string RAW_MESSAGE = PARTIAL_MESSAGE + @"
This is a MIME-encapsulated message.

--HTB3nt3RR7vw/QMPR4kDPbKg+XWjXIKdC/rfHQ==
Content-Description: Notification
Content-Type: text/plain

I'm sorry to have to inform you that your message could not
be delivered to one or more recipients. It's attached below.

For further assistance, please send mail to <postmaster@xxx>

If you do so, please include this problem report. You can
delete your own text from the attached returned message.

<test-dsn-failure@gmail.com>: 550-5.1.1 The email account that you tried to reach does not exist. Please try
550-5.1.1 double-checking the recipient's email address for typos or
550-5.1.1 unnecessary spaces. Learn more at
550 5.1.1 http://support.google.com/mail/bin/answer.py?answer=6596 t12si10077186weq.36


--HTB3nt3RR7vw/QMPR4kDPbKg+XWjXIKdC/rfHQ==
Content-Description: Delivery report
Content-Type: message/delivery-status

Reporting-MTA: dns; xxx
Arrival-Date: Fri, 04 May 2012 15:25:09 +0200

Final-Recipient: rfc822; test-dsn-failure@gmail.com
Status: 5.1.1
Action: failed
Last-Attempt-Date: Fri, 04 May 2012 15:25:09 +0200
Diagnostic-Code: smtp; 550-5.1.1 The email account that you tried to reach does not exist. Please try
550-5.1.1 double-checking the recipient's email address for typos or
550-5.1.1 unnecessary spaces. Learn more at
550 5.1.1 http://support.google.com/mail/bin/answer.py?answer=6596 t12si10077186weq.36

--HTB3nt3RR7vw/QMPR4kDPbKg+XWjXIKdC/rfHQ==
Content-Description: Undelivered Message
Content-Type: message/rfc822

[original message...]
";
    }

   
}
