# This repository has been moved

New location of this repository is: https://codeberg.org/eric-b/DSN-Parser

Following content is not kept up to date.

# Mail Delivery Status Notification (DSN) parser

## What is it ?

It's a [single file](https://github.com/eric-b/DSN-Parser/blob/master/DSNParser/MailDeliveryInfo.cs) you can drop in your project. You can also install the [nuget package](http://nuget.org/packages/DSNParser).

There is two helper methods to identify and parse a report as defined by [RFC 3464](http://tools.ietf.org/html/rfc3464).

### MailDeliveryInfo.IsDsn(string partialMessage)

This method should be used only if the complete message has not yet been downloaded.

Reads only the headers and returns true essentially if :

* First header is: **Return-path: <>**
* **Content-Type** contains **report-type=delivery-status**

Means that *MailDeliveryInfo.TryCreate()* should return a result for that message.

### MailDeliveryInfo.TryCreate(string rawMessage)

Evaluates MailDeliveryInfo.IsDsn(rawMessage) and tries to parse the report. Returns null if it fails or if it is not a report.

## Informations parsed

Basically, each instance of *MailDeliveryInfo* defines:

* a date,
* the raw report (with *Content-Type: message/delivery-status*),
* a list of status, each associated with an e-mail (see below),
* an arbitrary *Uid* property (not filled by the helper methods): can be used by the caller to store the report identifier (from POP3 for example).

The headers of the original (not delivered) message are also accessible.

Each status defines basically:

* an action (ie "Failed", "Delayed", "Delivered", "Relayed" or "Expanded"),
* a status code (ie. "5.1.1"),
* a classification string (ie. "PermanentFailure/AddressingStatus/BadDestinationMailboxAddress"),
* a diagnostic code (ie "smtp; 550-5.1.1 The email account that you tried to reach does not exist.").

## Example usage

	if (MailDeliveryInfo.IsDsn(PARTIAL_MESSAGE)) // PARTIAL_MESSAGE: headers only
	{
		var report = MailDeliveryInfo.TryCreate(RAW_MESSAGE); // RAW_MESSAGE: full message
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
	
	// Output:
	/*
	04/05/2012 15:25:09
	test-dsn-failure@gmail.com: PermanentFailure/AddressingStatus/BadDestinationMailboxAddress (5.1.1)
	Raw report:
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
	*/
	
## Limitations, caveats, known bugs
[Let me know](https://github.com/eric-b/DSN-Parser/issues) if you have troubles with use of this library.



