// Source: https://github.com/eric-b/DSN-Parser
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace DSNParser
{
    /// <summary>
    /// <para>Represents main error categories (Class.Subject.Detail).</para>
    /// <para>Interpretation of several codes can be ambigous. For more informations about a code, see this excellent document: 
    /// http://www.chicagonettech.com/pdf/ChicagoNetTech-SMTP_Server_Status_Codes_and_SMTP_Error_Codes.pdf </para>
    /// <remarks>These values are specified by RFC 3463 (http://tools.ietf.org/html/rfc3463).</remarks>
    /// </summary>
    [Flags]
    public enum StatusCodeClassification : ulong
    {
        #region #.X.X
        /// <summary>
        /// 2.X.X 
        /// </summary>
        Success                     = 1,
        /// <summary>
        /// 4.X.X 
        /// </summary>
        PersistentTransientFailure  = 2,
        /// <summary>
        /// 5.X.X
        /// </summary>
        PermanentFailure            = 4,
        #endregion

        #region X.#.X
        /// <summary>
        /// X.0.X
        /// </summary>
        OtherOrUndefinedStatus      = 8,
        /// <summary>
        /// X.1.X
        /// </summary>
        AddressingStatus            = 16,
        /// <summary>
        /// X.2.X
        /// </summary>
        MailboxStatus               = 32,
        /// <summary>
        /// X.3.X
        /// </summary>
        MailSystemStatus            = 64,
        /// <summary>
        /// X.4.X
        /// </summary>
        NetworkandRoutingStatus     = 128,
        /// <summary>
        /// X.5.X
        /// </summary>
        MailDeliveryProtocolStatus  = 256,
        /// <summary>
        /// X.6.X
        /// </summary>
        MessageContentOrMediaStatus = 512,
        /// <summary>
        /// X.7.X
        /// </summary>
        SecurityOrPolicyStatus = 1024,
        #endregion

        /// <summary>
        /// X.0.0
        /// </summary>
        OtherUndefinedStatus = 2048,

        #region X.1.X
        /// <summary>
        /// X.1.0
        /// </summary>
        OtherAddressStatus = 4096,
        /// <summary>
        /// X.1.1
        /// </summary>
        BadDestinationMailboxAddress = 8192,
        /// <summary>
        /// X.1.2
        /// </summary>
        BadDestinationSystemAddress = 16384,
        /// <summary>
        /// X.1.3
        /// </summary>
        BadDestinationMailboxAddressSyntax = 32768,
        /// <summary>
        /// X.1.4
        /// </summary>
        DestinationMailboxAddressAmbiguous = 65536,
        /// <summary>
        /// X.1.5
        /// </summary>
        DestinationAddressValid = 131072,
        /// <summary>
        /// X.1.6
        /// </summary>
        DestinationMailboxHasMoved_NoForwardingAddress = 262144,
        /// <summary>
        /// X.1.7
        /// </summary>
        BadSendersMailboxAddressSyntax = 524288,
        /// <summary>
        /// X.1.8
        /// </summary>
        BadSendersSystemAddress = 1048576,
        #endregion

        #region X.2.X
        /// <summary>
        /// X.2.0
        /// </summary>
        OtherOrUndefinedMailboxStatus = 2097152,
        /// <summary>
        /// X.2.1
        /// </summary>
        MailboxDisabled_NotAcceptingMessages = 4194304,
        /// <summary>
        /// X.2.2
        /// </summary>
        MailboxFull = 8388608,
        /// <summary>
        /// X.2.3
        /// </summary>
        MessageLengthExceedsAdministrativeLimit = 16777216,
        /// <summary>
        /// X.2.4
        /// </summary>
        OtherMailingListExpansionProblem = 33554432,
        #endregion

        #region X.3.X
        /// <summary>
        /// X.3.0
        /// </summary>
        OtherOrUndefinedMailSystemStatus = 67108864,
        /// <summary>
        /// X.3.1
        /// </summary>
        MailSystemFull = 134217728,
        /// <summary>
        /// X.3.2
        /// </summary>
        SystemNotAcceptingNetworkMessages = 268435456,
        /// <summary>
        /// X.3.3
        /// </summary>
        SystemNotCapableOfSelectedFeatures = 536870912,
        /// <summary>
        /// X.3.4
        /// </summary>
        MessageTooBigForSystem = 1073741824,
        /// <summary>
        /// X.3.5
        /// </summary>
        SystemIncorrectlyConfigured = 2147483648,
        #endregion

        #region X.4.X
        /// <summary>
        /// X.4.0
        /// </summary>
        OtherOrUndefinedNetworkOrRoutingStatus = 4294967296,
        /// <summary>
        /// X.4.1
        /// </summary>
        NoAnswerFromHost = 8589934592,
        /// <summary>
        /// X.4.2
        /// </summary>
        BadConnection = 17179869184,
        /// <summary>
        /// X.4.3
        /// </summary>
        DirectoryServerFailure = 34359738368,
        /// <summary>
        /// X.4.4
        /// </summary>
        UnableToRoute = 68719476736,
        /// <summary>
        /// X.4.5
        /// </summary>
        MailSsystemCongestion = 137438953472,
        /// <summary>
        /// X.4.6
        /// </summary>
        RoutingLoopDetected = 274877906944,
        /// <summary>
        /// X.4.7
        /// </summary>
        DeliveryTimeExpired = 549755813888,
        #endregion

        #region X.5.X
        /// <summary>
        /// X.5.0
        /// </summary>
        OtherOrUndefinedProtocolStatus = 1099511627776,
        /// <summary>
        /// X.5.1
        /// </summary>
        InvalidCommand = 2199023255552,
        /// <summary>
        /// X.5.2
        /// </summary>
        SyntaxError = 4398046511104,
        /// <summary>
        /// X.5.3
        /// </summary>
        TooManyRecipients = 8796093022208,
        /// <summary>
        /// X.5.4
        /// </summary>
        InvalidCommandArguments = 17592186044416,
        /// <summary>
        /// X.5.5
        /// </summary>
        WrongProtocolVersion = 35184372088832,
        #endregion

        #region X.6.X
        /// <summary>
        /// X.6.0
        /// </summary>
        OtherOrUndefinedMediaError = 70368744177664,
        /// <summary>
        /// X.6.1
        /// </summary>
        MediaNotSupported = 140737488355328,
        /// <summary>
        /// X.6.2
        /// </summary>
        ConversionRequiredAndProhibited = 281474976710656,
        /// <summary>
        /// X.6.3
        /// </summary>
        ConversionRequiredButNotSupported = 562949953421312,
        /// <summary>
        /// X.6.4
        /// </summary>
        ConversionWithLossPerformed = 1125899906842624,
        /// <summary>
        /// X.6.5
        /// </summary>
        ConversionFailed = 2251799813685248,
        #endregion

        #region X.7.X
        /// <summary>
        /// X.7.0
        /// </summary>
        OtherOrUndefinedSecurityStatus = 4503599627370496,
        /// <summary>
        /// X.7.1
        /// </summary>
        DeliveryNotAuthorized_MessageRefused = 9007199254740992,
        /// <summary>
        /// X.7.2
        /// </summary>
        MailingListExpansionProhibited = 18014398509481984,
        /// <summary>
        /// X.7.3
        /// </summary>
        SecurityConversionRequiredButNotPossible = 36028797018963968,
        /// <summary>
        /// X.7.4
        /// </summary>
        SecurityFeaturesNotSupported = 72057594037927936,
        /// <summary>
        /// X.7.5
        /// </summary>
        CryptographicFailure = 144115188075855872,
        /// <summary>
        /// X.7.6
        /// </summary>
        CryptographicAlgorithmNotSupported = 288230376151711744,
        /// <summary>
        /// X.7.7
        /// </summary>
        MessageIntegrityFailure = 576460752303423488,
        #endregion
    }

    /// <summary>
    /// <para>Represents literal values for the field "Status" 
    /// specified by RFC 3464 (http://tools.ietf.org/html/rfc3464).</para>
    /// </summary>
    public enum ActionStatus
    {
        /// <summary>
        /// <para>Indicates that the message could not be delivered to the
        /// recipient.  The Reporting MTA has abandoned any attempts
        /// to deliver the message to this recipient.  No further
        /// notifications should be expected.</para>
        /// </summary>
        Failed,
        /// <summary>
        /// <para>Indicates that the Reporting MTA has so far been unable
        /// to deliver or relay the message, but it will continue to
        /// attempt to do so.  Additional notification messages may
        /// be issued as the message is further delayed or
        /// successfully delivered, or if delivery attempts are later
        /// abandoned</para>
        /// </summary>
        Delayed,
        /// <summary>
        /// <para>Indicates that the message was successfully delivered to
        /// the recipient address specified by the sender, which
        /// includes "delivery" to a mailing list exploder.  It does
        /// not indicate that the message has been read.  This is a
        /// terminal state and no further DSN for this recipient
        /// should be expected</para>
        /// </summary>
        Delivered,
        /// <summary>
        /// <para>Indicates that the message has been relayed or gatewayed
        /// into an environment that does not accept responsibility
        /// for generating DSNs upon successful delivery.  This
        /// action-value SHOULD NOT be used unless the sender has
        /// requested notification of successful delivery for this recipient.</para>
        /// </summary>
        Relayed,
        /// <summary>
        /// <para>indicates that the message has been successfully
        /// delivered to the recipient address as specified by the
        /// sender, and forwarded by the Reporting-MTA beyond that
        /// destination to multiple additional recipient addresses.
        /// An action-value of "expanded" differs from "delivered" in
        /// that "expanded" is not a terminal state.  Further
        /// "failed" and/or "delayed" notifications may be provided.</para>
        /// </summary>
        Expanded
    }

    /// <summary>
    /// <para>Represents a partial delivery status report relative to one specific recipient.</para>
    /// <para>This structure allows to identify an problematic destination and its cause.</para>
    /// </summary>
    public struct Status
    {
        #region private fields
        private KeyValuePair<string, StatusCodeClassification> _innerCode;

        /// <summary>
        /// Tries to match a pattern like "5.5.2".
        /// </summary>
        private static readonly Regex _reDiagCodeClass = new Regex(@"\b[245]\.[0-7]\.\d{1,3}\b");       

        /// <summary>
        /// Tries to match a pattern like "552".
        /// </summary>
        private static readonly Regex _reDiagCodeClass_Secondary = new Regex(@"\b[245][0-7]\d{1,3}\b");

        private static readonly Regex _reMailboxIsFull = new Regex(@"Mailbox .*?is full", RegexOptions.IgnoreCase);

        /// <summary>
        /// Tries to match a description that indicate a non-existant address (commonly marked with a generic code 5.5.0).
        /// </summary>
        private static readonly Regex _reMailboxNotFound = new Regex(@"(Invalid recipient|(User account is|Mailbox|Account|Address) (unavailable|rejected|not available|does not exist)|No such user|Not our customer)", RegexOptions.IgnoreCase);
        #endregion

        /// <summary>
        /// <para>The Action field indicates the action performed by the Reporting-MTA 
        /// as a result of its attempt to deliver the message to this recipient address.  
        /// This field MUST be present for each recipient named in the DSN.
        /// Ref: RFC 3464, 2.3.3</para>.
        /// </summary>
        public ActionStatus Action;
        
        /// <summary>
        /// Status field value.
        /// </summary>
        [Obsolete("Most of the time, use MostSignificantStatusCode property.")]
        public string StatusCode;

        /// <summary>
        /// <para>Gets the most precise status code between <see cref="StatusCode"/> and <see cref="InnerDiagnosticClassificationCode"/>.
        /// The latter is only filled if it is more accurate than the first.</para>
        /// <para><see cref="StatusCode"/> is obtained from the report status. 
        /// <see cref="InnerDiagnosticClassificationCode"/> is obtained from <see cref="DiagnosticCode"/>.</para>
        /// </summary>
        public string MostSignificantStatusCode
        {
            get
            {
#pragma warning disable 0618
                return _innerCode.Key != null ? _innerCode.Key : StatusCode;
#pragma warning restore 0618
            }
        }

        /// <summary>
        /// Categorization of the <see cref="StatusCode"/>.
        /// </summary>
        [Obsolete("Most of the time, use MostSignificantClasses property.")]
        public StatusCodeClassification Classes;

        /// <summary>
        /// <para>Gets the most precise status categorization between <see cref="Classes"/> and <see cref="InnerDiagnosticClassificationCode"/>.
        /// The latter is only filled if it is more accurate than the first.</para>
        /// <para><see cref="Classes"/> is obtained from the report status. 
        /// <see cref="InnerDiagnosticClassificationCode"/> is obtained from the report status.
        /// <see cref="InnerDiagnosticClassificationCode"/> is obtained from <see cref="DiagnosticCode"/>.</para>
        /// </summary>
        public StatusCodeClassification MostSignificantClasses
        {
            get
            {
#pragma warning disable 0618
                return _innerCode.Key != null ? _innerCode.Value : Classes;
#pragma warning restore 0618
            }
        }

        /// <summary>
        /// <para>If filled, gets the status returned by the MTA at which the message has been relayed.</para>
        /// <para>Should contain the SMTP status code with a short explanation. The explanation can contain
        /// a more accurate status code.</para>
        /// </summary>
        public string DiagnosticCode;

        /// <summary>
        /// <para>If <see cref="DiagnosticCode"/> contains a more accurate status code, gets it. Otherwise returns a default structure.</para>
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, StatusCodeClassification> InnerDiagnosticClassificationCode
        {
            get
            {
                return _innerCode;
            }
        }

        /// <summary>
        /// <para>If <see cref="IsDelayed"/>=<c>true</c>, should get date and time up to which the MTA will attempt to relay the message.</para>
        /// </summary>
        public DateTime? WillRetryUntil;

        /// <summary>
        /// Gets <c>true</c> if <see cref="StatusCode"/> refers to a permanent error.
        /// </summary>
        public bool IsPermanentFailure
        {
            get
            {
#pragma warning disable 0618
                return (Classes & StatusCodeClassification.PermanentFailure) == StatusCodeClassification.PermanentFailure;
#pragma warning restore 0618
            }
        }

        /// <summary>
        /// Gets <c>true</c> if <see cref="StatusCode"/> refers to a temporary error, except <see cref="StatusCodeClassification.DeliveryTimeExpired"/> (ie. final state unknown).
        /// </summary>
        public bool IsTemporaryFailure
        {
            get
            {
#pragma warning disable 0618
                return Action != ActionStatus.Delayed && (Classes & StatusCodeClassification.PersistentTransientFailure) == StatusCodeClassification.PersistentTransientFailure;
#pragma warning restore 0618
            }
        }

        /// <summary>
        /// <para>Gets <c>true</c> if <see cref="Action"/>=<see cref="ActionStatus.Delayed"/>.</para>
        /// <para>The status should be <see cref="StatusCodeClassification.DeliveryTimeExpired"/> (status 4.4.7, "message delayed").</para>
        /// <para>This case is unusual because it is not an error nor a success.</para>
        /// </summary>
        public bool IsDelayed
        {
            get
            {
                return Action == ActionStatus.Delayed;
            }
        }

        /// <summary>
        /// <para>Gets <c>true</c> if <see cref="MostSignificantStatusCode"/> indicates an unknown recipient.</para> 
        /// <para>If the status is not accurate, tries also to match several well known expressions via a regex on <see cref="DiagnosticCode"/>.</para>
        /// </summary>
        public bool IsMailAddressUnknown
        {
            get
            {
                const StatusCodeClassification check = StatusCodeClassification.BadDestinationMailboxAddress;

                if ((MostSignificantClasses & check) == check)
                    return true;

                if (!string.IsNullOrEmpty(DiagnosticCode))
                    return _reMailboxNotFound.IsMatch(DiagnosticCode);
                return false;
            }
        }

        /// <summary>
        /// <para>Gets <c>true</c> if the status code indicates a full mailbox.</para>
        /// <para>If the status is not accurate, tries also to match several well known expressions via a regex on <see cref="DiagnosticCode"/>.</para>
        /// </summary>
        public bool IsMailBoxFull
        {
            get
            {
                // Note: StatusCodeClassification.MailSystemFull a une signification différente qui ne devrait pas être assimilé à IsMailBoxFull.
                const StatusCodeClassification check = StatusCodeClassification.MailboxFull;

                if ((MostSignificantClasses & check) == check)
                    return true;

                if (!string.IsNullOrEmpty(DiagnosticCode))
                    return _reMailboxIsFull.IsMatch(DiagnosticCode);
                return false;
            }
        }

        #region ctor

        /// <summary>
        /// <para>Creates an instance based on action, status and diagnostic code of a report.</para>
        /// </summary>
        /// <param name="action">Example: "failed".</param>
        /// <param name="statusCode">Example: "5.1.1"</param>
        /// <param name="diagnosticCode">Optional. Represents response from the final MTA.</param>
        public Status(string action, string statusCode, string diagnosticCode)
        {
            if (string.IsNullOrEmpty(action))
                throw new ArgumentNullException("action");
            if (string.IsNullOrEmpty(statusCode))
                throw new ArgumentNullException("statusCode");
			if (!Char.IsDigit(statusCode[0]))
                throw new UnknownStatusException(statusCode);

            WillRetryUntil = null;

#pragma warning disable 0618
            StatusCode = statusCode;
            Classes = ParseStatusCode(statusCode);
#pragma warning restore 0618
            Action = (ActionStatus)Enum.Parse(typeof(ActionStatus), action, true);
            DiagnosticCode = diagnosticCode;


            _innerCode = new KeyValuePair<string, StatusCodeClassification>();
            if (!string.IsNullOrEmpty(diagnosticCode))
            {
                bool firstMatch;
                var match = _reDiagCodeClass.Match(diagnosticCode);
                firstMatch = match.Success;
                while (match.Success)
                {
                    var innerClasses = ParseStatusCode(match.Value);
#pragma warning disable 0618
                    if (innerClasses > Classes
                        && (_innerCode.Key == null || _innerCode.Value < innerClasses))
                    {
                        // Categorization more accurante found in diagnosticCode:
                        _innerCode = new KeyValuePair<string, StatusCodeClassification>(match.Value, innerClasses);
                    }
#pragma warning restore 0618
                    match = match.NextMatch();
                }

                if (!firstMatch)
                {
                    match = _reDiagCodeClass_Secondary.Match(diagnosticCode);
                    try
                    {
                        while (match.Success)
                        {
                            var value = match.Value;
                            var c = string.Format("{0}.{1}.{2}", value[0], value[1], value.Substring(2));
                            var innerClasses = ParseStatusCode(c);
#pragma warning disable 0618
                            if (innerClasses > Classes
                                && (_innerCode.Key == null || _innerCode.Value < innerClasses))
                            {
                                // Categorization more accurante found in diagnosticCode:
                                _innerCode = new KeyValuePair<string, StatusCodeClassification>(c, innerClasses);
                            }
#pragma warning restore 0618
                            match = match.NextMatch();
                        }
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// <para>Parses a code like "X.X.X".</para>
        /// </summary>
        /// <param name="statusCode">Code like "X.X.X".</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">First numeric value out of range (2, 4 or 5).</exception>
        /// <exception cref="FormatException"></exception>
        private static StatusCodeClassification ParseStatusCode(string statusCode)
        {
            const string INVALID_FORMAT_DESC_FORMAT = "Invalid format: statusCode={0}. Expected format: X.X.X.";
            const string OUT_OF_RANGE_DESC_FORMAT = "Status class for {0} is unexpected. Expected classes are specified by RFC 3463 (2, 4 or 5).";
            var index1 = statusCode.IndexOf('.');
            if (index1 == -1)
                throw new FormatException(string.Format(INVALID_FORMAT_DESC_FORMAT, statusCode));
            var index2 = statusCode.LastIndexOf('.');
            if (index2 == -1 || index2 == index1)
                throw new FormatException(string.Format(INVALID_FORMAT_DESC_FORMAT, statusCode));
            var statusCodeClass = statusCode.Substring(0, index1); // statusCode="5.2.2" => "5"
            var statusCodeSubject = statusCode.Substring(index1 + 1, index2 - (index1 + 1)); // ex: "2"
            var statusCodeDetail = statusCode.Substring(index2 + 1); // ex: "2"
            var statusCodeSubjectDetail = statusCode.Substring(index1 + 1); // ex: "2.2"

            StatusCodeClassification result;
            #region Default categorization based on status class
            switch (statusCodeClass)
            {
                case "2":
                    result = StatusCodeClassification.Success | StatusCodeClassification.OtherOrUndefinedStatus | StatusCodeClassification.OtherUndefinedStatus;
                    break;
                case "4":
                    result = StatusCodeClassification.PersistentTransientFailure | StatusCodeClassification.OtherOrUndefinedStatus | StatusCodeClassification.OtherUndefinedStatus;
                    break;
                case "5":
                    result = StatusCodeClassification.PermanentFailure | StatusCodeClassification.OtherOrUndefinedStatus | StatusCodeClassification.OtherUndefinedStatus;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("statusCode", string.Format(OUT_OF_RANGE_DESC_FORMAT, statusCode));
            }
            #endregion

            #region Categorization more precise if possible
            bool detailCategoryKnown = true;
            switch (statusCodeSubjectDetail)
            {
                case "0.0":
                    break;
                #region 1.X
                case "1.0":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.OtherAddressStatus;
                    break;
                case "1.1":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.AddressingStatus | StatusCodeClassification.BadDestinationMailboxAddress;
                    break;
                case "1.2":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.AddressingStatus | StatusCodeClassification.BadDestinationSystemAddress;
                    break;
                case "1.3":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.AddressingStatus | StatusCodeClassification.BadSendersMailboxAddressSyntax;
                    break;
                case "1.4":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.AddressingStatus | StatusCodeClassification.DestinationMailboxAddressAmbiguous;
                    break;
                case "1.5":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.AddressingStatus | StatusCodeClassification.DestinationAddressValid;
                    break;
                case "1.6":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.AddressingStatus | StatusCodeClassification.DestinationMailboxHasMoved_NoForwardingAddress;
                    break;
                case "1.7":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.AddressingStatus | StatusCodeClassification.BadSendersMailboxAddressSyntax;
                    break;
                case "1.8":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.AddressingStatus | StatusCodeClassification.BadSendersSystemAddress;
                    break;
                #endregion
                #region 2.X
                case "2.0":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailboxStatus | StatusCodeClassification.OtherOrUndefinedMailboxStatus;
                    break;
                case "2.1":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailboxStatus | StatusCodeClassification.MailboxDisabled_NotAcceptingMessages;
                    break;
                case "2.2":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailboxStatus | StatusCodeClassification.MailboxFull;
                    break;
                case "2.3":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailboxStatus | StatusCodeClassification.MessageLengthExceedsAdministrativeLimit;
                    break;
                case "2.4":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailboxStatus | StatusCodeClassification.OtherMailingListExpansionProblem;
                    break;
                #endregion
                #region 3.X
                case "3.0":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailSystemStatus | StatusCodeClassification.OtherOrUndefinedMailSystemStatus;
                    break;
                case "3.1":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailSystemStatus | StatusCodeClassification.MailSystemFull;
                    break;
                case "3.2":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailSystemStatus | StatusCodeClassification.SystemNotAcceptingNetworkMessages;
                    break;
                case "3.3":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailSystemStatus | StatusCodeClassification.SystemNotCapableOfSelectedFeatures;
                    break;
                case "3.4":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailSystemStatus | StatusCodeClassification.MessageTooBigForSystem;
                    break;
                case "3.5":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailSystemStatus | StatusCodeClassification.SystemIncorrectlyConfigured;
                    break;
                #endregion
                #region 4.X
                case "4.0":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.NetworkandRoutingStatus | StatusCodeClassification.OtherOrUndefinedNetworkOrRoutingStatus;
                    break;
                case "4.1":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.NetworkandRoutingStatus | StatusCodeClassification.NoAnswerFromHost;
                    break;
                case "4.2":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.NetworkandRoutingStatus | StatusCodeClassification.BadConnection;
                    break;
                case "4.3":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.NetworkandRoutingStatus | StatusCodeClassification.DirectoryServerFailure;
                    break;
                case "4.4":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.NetworkandRoutingStatus | StatusCodeClassification.UnableToRoute;
                    break;
                case "4.5":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.NetworkandRoutingStatus | StatusCodeClassification.MailSsystemCongestion;
                    break;
                case "4.6":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.NetworkandRoutingStatus | StatusCodeClassification.RoutingLoopDetected;
                    break;
                case "4.7":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.NetworkandRoutingStatus | StatusCodeClassification.DeliveryTimeExpired;
                    break;
                #endregion
                #region 5.X
                case "5.0":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailDeliveryProtocolStatus | StatusCodeClassification.OtherOrUndefinedProtocolStatus;
                    break;
                case "5.1":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailDeliveryProtocolStatus | StatusCodeClassification.InvalidCommand;
                    break;
                case "5.2":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailDeliveryProtocolStatus | StatusCodeClassification.SyntaxError;
                    break;
                case "5.3":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailDeliveryProtocolStatus | StatusCodeClassification.TooManyRecipients;
                    break;
                case "5.4":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailDeliveryProtocolStatus | StatusCodeClassification.InvalidCommandArguments;
                    break;
                case "5.5":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MailDeliveryProtocolStatus | StatusCodeClassification.WrongProtocolVersion;
                    break;
                #endregion
                #region 6.X
                case "6.0":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MessageContentOrMediaStatus | StatusCodeClassification.OtherOrUndefinedMediaError;
                    break;
                case "6.1":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MessageContentOrMediaStatus | StatusCodeClassification.MediaNotSupported;
                    break;
                case "6.2":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MessageContentOrMediaStatus | StatusCodeClassification.ConversionRequiredAndProhibited;
                    break;
                case "6.3":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MessageContentOrMediaStatus | StatusCodeClassification.ConversionRequiredButNotSupported;
                    break;
                case "6.4":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MessageContentOrMediaStatus | StatusCodeClassification.ConversionWithLossPerformed;
                    break;
                case "6.5":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.MessageContentOrMediaStatus | StatusCodeClassification.ConversionFailed;
                    break;
                #endregion
                #region 7.X
                case "7.0":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.SecurityOrPolicyStatus | StatusCodeClassification.OtherOrUndefinedSecurityStatus;
                    break;
                case "7.1":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.SecurityOrPolicyStatus | StatusCodeClassification.DeliveryNotAuthorized_MessageRefused;
                    break;
                case "7.2":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.SecurityOrPolicyStatus | StatusCodeClassification.MailingListExpansionProhibited;
                    break;
                case "7.3":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.SecurityOrPolicyStatus | StatusCodeClassification.SecurityConversionRequiredButNotPossible;
                    break;
                case "7.4":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.SecurityOrPolicyStatus | StatusCodeClassification.SecurityFeaturesNotSupported;
                    break;
                case "7.5":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.SecurityOrPolicyStatus | StatusCodeClassification.CryptographicFailure;
                    break;
                case "7.6":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.SecurityOrPolicyStatus | StatusCodeClassification.CryptographicAlgorithmNotSupported;
                    break;
                case "7.7":
                    result = result
                        ^ StatusCodeClassification.OtherUndefinedStatus ^ StatusCodeClassification.OtherOrUndefinedStatus
                        | StatusCodeClassification.SecurityOrPolicyStatus | StatusCodeClassification.MessageIntegrityFailure;
                    break;
                #endregion
                default:
                    System.Diagnostics.Debug.WriteLine(string.Format("Level 3 category unknown: X.{0}", statusCodeSubjectDetail));
                    detailCategoryKnown = false;
                    break;
            }
            #endregion

            #region Categorization by subject if possible
            if (!detailCategoryKnown)
            {
                switch (statusCodeSubject)
                {
                    case "0":
                        break;
                    case "1":
                        result = result
                            ^ StatusCodeClassification.OtherOrUndefinedStatus
                            | StatusCodeClassification.OtherAddressStatus;
                        break;
                    case "2":
                        result = result
                            ^ StatusCodeClassification.OtherOrUndefinedStatus
                            | StatusCodeClassification.MailboxStatus;
                        break;
                    case "3":
                        result = result
                            ^ StatusCodeClassification.OtherOrUndefinedStatus
                            | StatusCodeClassification.MailSystemStatus;
                        break;
                    case "4":
                        result = result
                            ^ StatusCodeClassification.OtherOrUndefinedStatus
                            | StatusCodeClassification.NetworkandRoutingStatus;
                        break;
                    case "5":
                        result = result
                            ^ StatusCodeClassification.OtherOrUndefinedStatus
                            | StatusCodeClassification.MailDeliveryProtocolStatus;
                        break;
                    case "6":
                        result = result
                            ^ StatusCodeClassification.OtherOrUndefinedStatus
                            | StatusCodeClassification.MessageContentOrMediaStatus;
                        break;
                    case "7":
                        result = result
                            ^ StatusCodeClassification.OtherOrUndefinedStatus
                            | StatusCodeClassification.SecurityOrPolicyStatus;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine(string.Format("Level 2 category unknown: X.{0}.X", statusCodeSubject));
                        break;
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Parses a code like "X.X.X" and returns its literal description "class/subject/detail".
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static string GetClassificationString(string statusCode)
        {
            return GetClassificationString(ParseStatusCode(statusCode));
        }

        private static string GetClassificationString(StatusCodeClassification classes)
        {
            var flags = Enum.GetValues(typeof(StatusCodeClassification)).Cast<StatusCodeClassification>();
            List<StatusCodeClassification> flagsSet = new List<StatusCodeClassification>(3);
            foreach (var flag in flags)
            {
                if ((classes & flag) == flag)
                    flagsSet.Add(flag);
            }
            return string.Join("/", flagsSet.Select(t => t.ToString()).ToArray());
        }

        /// <summary>
        /// Returns <see cref="Classes"/> represented with a literal description "class/subject/detail".
        /// </summary>
        /// <returns></returns>
        public string GetMostSignificantClassificationString()
        {
            return GetClassificationString(MostSignificantClasses);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1}: {2})", Action, MostSignificantStatusCode, GetMostSignificantClassificationString());
        }

        #endregion
    }

   

    /// <summary>
    /// <para>Represents a delivery status notification associated with a message that could not be delivered to one or more recipients.</para>
    /// <para>This object allows to identify the recipients who did not receive the message, the cause and original headers of the message.</para>
    /// </summary>
    public class MailDeliveryInfo
    {
        private const string FIELD_CONTENT_TYPE_RFC822_HEADERS = "Content-Type: text/rfc822-headers";
        private const string FIELD_CONTENT_TYPE_RFC822 = "Content-Type: message/rfc822";
        private static readonly string[] SUPPORTED_FIELDS_CONTENT_TYPE_RFC822 = { FIELD_CONTENT_TYPE_RFC822_HEADERS, FIELD_CONTENT_TYPE_RFC822 };

        /// <summary>
        /// Sepoarators to parse boundary field (double quote, space).
        /// </summary>
        private static readonly char[] BOUNDARY_SEP_CHAR = { '"', ' ' };

        /// <summary>
        /// First character of a continuation line (space, tab).
        /// </summary>
        private static readonly char[] LINE_CONTINUE_CHAR = { ' ', '\t' };

        /// <summary>
        /// Headers of the original message.
        /// </summary>
        private List<KeyValuePair<string, string>> _originalMsgHeaders;

        /// <summary>
        /// <para>Arbitrary value not filled by this object.</para>
        /// <para>Allows to identify the report/message parsed. Should be filled by the caller to the method <see cref="TryCreate"/>.</para>
        /// </summary>
        public string Uid { get; set; }


        /// <summary>
        /// <para>Date field of the report.</para>
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// <para>"Reporting-MTA" field of the report (mandatory).</para>
        /// </summary>
        public string ReportingTransferAgent { get; set; }

        /// <summary>
        /// "Received-From-MTA" field of the report (optional).
        /// </summary>
        public string ReceivedFromTransferAgent { get; set; }

        /// <summary>
        /// "Arrival-Date" field of the report (optional).
        /// </summary>
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// <para>List of status contained in the report, by problematic recipient.
        /// The key is an email address.</para>
        /// </summary>
        public Dictionary<string, Status> Status { get; set; }

        /// <summary>
        /// Raw report.
        /// </summary>
        public string RawReport { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MailDeliveryInfo()
        {
            Status = new Dictionary<string, Status>();
        }

		/// <summary>
        /// <para>Returns the headers of the original message.</para>
        /// </summary>
        /// <param name="decode">
        /// <para>If <c>true</c>, tries to decode values encoded with base64 format.
        /// <remarks>Quoted Printable format is not implemented.</remarks>
        /// </para>
        /// </param>
        /// <returns>Headers</returns>
        public KeyValuePair<string, string>[] GetOriginalMessageHeaders(bool decode)
        {
            if (_originalMsgHeaders == null || _originalMsgHeaders.Count == 0)
                return new KeyValuePair<string, string>[0];
            if (!decode)
                return _originalMsgHeaders.ToArray();

            return _originalMsgHeaders.Select(t =>
                {
                    string decodedValue;
                    if (TryDecode(t.Value, out decodedValue))
                        return new KeyValuePair<string, string>(t.Key, decodedValue);
                    else
                        return new KeyValuePair<string, string>(t.Key, t.Value);
                }).ToArray();
        }
	
        /// <summary>
        /// Returns a specific header value(s).
        /// </summary>
        /// <param name="name">Header field name (example: "To").</param>
        /// <returns>List of values (duplicates allowed) or empty list.</returns>
        public IEnumerable<string> GetOriginalMessageHeader(string name)
        {
            return GetOriginalMessageHeader(name, false);
        }

        /// <summary>
        /// Returns a specific header value(s).
        /// </summary>
        /// <param name="name">Header field name (example: "To").</param>
        /// <param name="decode">
        /// <para>If <c>true</c>, tries to decode values encoded with base64 format.
        /// <remarks>Quoted Printable format is not implemented.</remarks>
        /// </para>
        /// </param>
        /// <returns>List of values (duplicates allowed) or empty list.</returns>
        public IEnumerable<string> GetOriginalMessageHeader(string name, bool decode)
        {
            if (_originalMsgHeaders == null || _originalMsgHeaders.Count == 0)
                return new string[0];

            return _originalMsgHeaders.Where(t => t.Key.Equals(name, StringComparison.OrdinalIgnoreCase)).Select(t => 
                {
                    if (decode && !string.IsNullOrEmpty(t.Value))
                    {
                        string decodedValue;
                        return TryDecode(t.Value, out decodedValue) ? decodedValue : t.Value;
                    }
                    return t.Value;
                });
        }

        /// <summary>
        /// <para>Tries to decode an encoded value with base64 format and returns <c>true</c> if succeeds or if the value does not seem to be encoded.</para>
        /// <remarks>This methods should be used on header values only.</remarks>
        /// </summary>
        /// <param name="value">Value to decode</param>
        /// <param name="decodedValue">Decoded value</param>
        /// <returns><c>true</c> if <paramref name="decodedValue"/> is filled.</returns>
        private bool TryDecode(string value, out string decodedValue)
        {
            const string PREFX_QUOTED_PRINTABLE = "=?utf-8?Q?"; // ToDo !
            const string PREFX_BASE64 = "=?utf-8?B?";
            const bool SUCCESS = true, FAILURE = false;
            if (string.IsNullOrEmpty(value))
            {
                decodedValue = value;
                return SUCCESS;
            }

            if (value.StartsWith(PREFX_BASE64)) 
            {
                /* Ex1: =?utf-8?B?MiBlcnJldXJzIGTDqXRlY3TDqWVzIChEaXNrKQ==?=
                 * Ex2: =?utf-8?B?TWVzc2FnZSAqdGVzdCogKHRlc3QgZW5jb2RhZ2U6ICYgw6kg?= =?utf-8?B?QCDDqiAp?=
                 * */
                var encodedValues = value.Split(new string[] { PREFX_BASE64 }, StringSplitOptions.RemoveEmptyEntries);
                decodedValue = string.Empty;
                for (int i = 0; i < encodedValues.Length; i++)
                {
                    string encodedValue = encodedValues[i]; //value.Substring(PREFX_BASE64.Length); // "MiBlcnJldXJzIGTDqXRlY3TDqWVzIChEaXNrKQ==?="
                    int indexPad = encodedValue.IndexOf('?');
                    if (indexPad != -1)
                        encodedValue = encodedValue.Substring(0, indexPad); // "MiBlcnJldXJzIGTDqXRlY3TDqWVzIChEaXNrKQ"

                    try
                    {
                        decodedValue += Encoding.UTF8.GetString(System.Convert.FromBase64String(encodedValue));
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Failed to decode string '{1}' (value: '{2}'): {0}", ex, encodedValue, value));
                        decodedValue = null;
                        return FAILURE;
                    }
                }
                return SUCCESS;
            }
            else
            {
                // ToDo: Quoted Printable
                decodedValue = null;
                return FAILURE;
            }
        }

        /// <summary>
        /// <para>Tries to parse a datetime with format "Mon, 23 Apr 2012 22:26:32 +0100".</para>
        /// </summary>
        /// <param name="date">Datetime like "Mon, 23 Apr 2012 22:26:32 +0100".</param>
        /// <returns>DateTime or <c>null</c>.</returns>
        public static DateTime? ParseDate(string date)
        {
            const string FORMAT = "ddd, d MMM yyyy HH:mm:ss zzz";
            const string FORMAT2 = "ddd, dd MMM yyyy HH:mm:ss zzz";
            const string FORMAT3 = "dd MMM yyyy HH:mm:ss zzz";
            const string FORMAT4 = "d MMM yyyy HH:mm:ss zzz";
            DateTime d;
            if (DateTime.TryParseExact(date, new string[] { FORMAT, FORMAT2, FORMAT3, FORMAT4 }, CultureInfo.InvariantCulture, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out d))
                return d;
            Debug.WriteLine(string.Format("Date specified can not be parsed (unexpected format): '{0}'", date));
            return null;
        }

        /// <summary>
        /// <para>Tries to identify headers of a Delivery Status Notification report.</para>
        /// </summary>
        /// <param name="rawHeaders">Partial raw message (only the headers will be read).</param>
        /// <returns><c>true</c> if a report has been identified.</returns>
        public static bool IsDsn(string rawHeaders)
        {
            if (rawHeaders == null)
                throw new ArgumentNullException("rawHeaders");

            TextReader reader;
            string notUsed;
            if (IsDsn(rawHeaders, out reader, out notUsed))
            {
                reader.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// <para>Tries to identify headers of a Delivery Status Notification report.</para>
        /// </summary>
        /// <param name="rawHeaders">Partial raw message (only the headers will be read).</param>
        /// <param name="reader">If this method returns <c>true</c>, gets a reader with cursor placed afeter "Content-Type" header.</param>
        /// <param name="boundary">If this method returns <c>true</c>, gets the boundary value obtained from "Content-Type" header.</param>
        /// <returns><c>true</c> if a report has been identified.</returns>
        private static bool IsDsn(string rawHeaders, out TextReader reader, out string boundary)
        {
            reader = null;
            boundary = null;
            if (!rawHeaders.StartsWith("Return-path: <>", StringComparison.OrdinalIgnoreCase))
            {
                // practical condition as defined in the spec (RFC 5321).
                return false; 
            }

            bool isReport = false;
            reader = new StringReader(rawHeaders);
            try
            {
                string line;
                const string FIELD_CONTENT_TYPE = "Content-Type:";
                const string STR_REPORT_MARKER = "report-type=delivery-status";
                const string FIELD_BOUNDARY = "boundary=";
                while ((line = reader.ReadLine()) != null)
                {
                    int indexStart;
                    if ((indexStart = line.IndexOf(FIELD_CONTENT_TYPE, StringComparison.OrdinalIgnoreCase)) == 0                                                // StartsWith(Content-Type:)
                        && (indexStart = line.IndexOf(STR_REPORT_MARKER, indexStart + FIELD_CONTENT_TYPE.Length, StringComparison.OrdinalIgnoreCase)) != -1)    // Contains(report-type=delivery-status)
                    {
                        // Report identified - tries to get "boundary" mandatory field .
                        int nextChar;
                        while ((nextChar = reader.Peek()) != -1 && LINE_CONTINUE_CHAR.Contains((char)nextChar))
                            line += " " + reader.ReadLine().TrimStart(LINE_CONTINUE_CHAR); // line of more than 76 characters
                        int endBoundary;
                        if ((indexStart = line.IndexOf(FIELD_BOUNDARY, indexStart + STR_REPORT_MARKER.Length)) != -1
                            && (endBoundary = line.IndexOfAny(BOUNDARY_SEP_CHAR, indexStart + FIELD_BOUNDARY.Length + 1)) != -1)
                        {
                            boundary = line.Substring(indexStart + FIELD_BOUNDARY.Length, endBoundary - (indexStart + FIELD_BOUNDARY.Length)).TrimStart(BOUNDARY_SEP_CHAR);
                            isReport = true;
			    break;
                        }
                    }
                }
            }
            finally
            {
                if (!isReport)
                {
                    reader.Close();
                    reader = null;
                }
            }

            return isReport;
        }


        /// <summary>
        /// <para>Tries to parse a raw message and to return a DSN report.</para>
        /// </summary>
        /// <param name="rawMessage">Raw message.</param>
        /// <returns>DSN report or <c>null</c>.</returns>
        public static MailDeliveryInfo TryCreate(string rawMessage)
        {
            if (rawMessage == null)
                throw new ArgumentNullException("rawMessage");

            TextReader reader;
            string boundary;
            if (IsDsn(rawMessage, out reader, out boundary))
            {
                try
                {
                    string line;
                    int indexStart;
                    int nextChar;

                    /* Ignores content of the message until the report
                             * Example of content to reach:
--9B095B5ADSN=_01CD10A8173FC8FE000027ABSMRFGENE3K13.rad
Content-Type: message/delivery-status
...
                                * 
                                * 
--s4k5OQAFkMI7RvUt+K1HA0ya1bT161KolxPk4w==
Content-Description: Delivery report
Content-Type: message/delivery-status
...
                                * */
                    var boundaryLine = string.Format("--{0}", boundary);

                    while ((line = reader.ReadLine()) != null
                        &&
                        (line != boundaryLine)) { }
                    while ((line = reader.ReadLine()) != null
                        &&
                        (line != boundaryLine)
                        &&
                        !line.StartsWith("Content-Type: message/delivery-status", StringComparison.OrdinalIgnoreCase)) { }
                    if (line == null)
                        return null;


                    // Parses the report (cf. http://tools.ietf.org/html/rfc3464)
                    var report = new MailDeliveryInfo();
                    #region Date of report
                    const string FIELD_DATE = "Date:";
                    if ((indexStart = rawMessage.IndexOf(Environment.NewLine + FIELD_DATE)) != -1)
                    {
                        var indexNL = rawMessage.IndexOf(Environment.NewLine, indexStart + FIELD_DATE.Length);
                        var date = rawMessage.Substring(indexStart + Environment.NewLine.Length + FIELD_DATE.Length, indexNL - (indexStart + Environment.NewLine.Length + FIELD_DATE.Length));
                        if (indexNL != -1
                            && rawMessage.Length > indexNL + Environment.NewLine.Length
                            && LINE_CONTINUE_CHAR.Contains(rawMessage[indexNL + Environment.NewLine.Length])
                            && (indexStart = rawMessage.IndexOf(Environment.NewLine, indexNL + Environment.NewLine.Length)) != -1)
                        {
                            // unusual case: date in 2 lines
                            date += ' ' + rawMessage.Substring(indexNL + Environment.NewLine.Length, indexStart - (indexNL + Environment.NewLine.Length)).TrimStart(LINE_CONTINUE_CHAR);
                        }
                        var reportDate = ParseDate(date);
                        if (reportDate.HasValue)
                            report.Date = reportDate.Value;
                    }
                    #endregion

                    string rawReport = string.Empty;
                    while ((line = reader.ReadLine()) != null
                        && line != boundaryLine)
                    {
                        while ((nextChar = reader.Peek()) != -1 && LINE_CONTINUE_CHAR.Contains((char)nextChar))
                            line += " " + reader.ReadLine().TrimStart(LINE_CONTINUE_CHAR); // line of more than 76 characters

                        rawReport += line + Environment.NewLine;
                        if ((indexStart = line.IndexOf(':')) != -1)
                        {
                            var fieldName = line.Substring(0, indexStart).Trim();
                            var fieldValue = line.Substring(indexStart + 1).Trim();
                            bool endOfReport = false;
                            switch (fieldName)
                            {
                                case "Reporting-MTA":
                                    report.ReportingTransferAgent = fieldValue;
                                    break;
                                case "Received-From-MTA":
                                    report.ReceivedFromTransferAgent = fieldValue;
                                    break;
                                case "Arrival-Date":
                                    report.ArrivalDate = ParseDate(fieldValue);
                                    break;
                                case "Final-Recipient":
                                    // One or more recipients
                                    endOfReport = true;
                                    const string RECIPIENT_PREFIX = "rfc822;"; // examples: "rfc822; name@domain.fr", "rfc822;name@domain.com"
                                    var finalRecipient = fieldValue.StartsWith(RECIPIENT_PREFIX) ? fieldValue.Substring(RECIPIENT_PREFIX.Length).TrimStart() : fieldValue;
                                    string finalRecipientAction = null, finalRecipientStatus = null, finalRecipientDiagCode = null, willRetryUntil = null;
                                    while ((line = reader.ReadLine()) != null
                                        && line != boundaryLine)
                                    {
                                        while ((nextChar = reader.Peek()) != -1 && LINE_CONTINUE_CHAR.Contains((char)nextChar))
                                            line += " " + reader.ReadLine().TrimStart(LINE_CONTINUE_CHAR); // line of more than 76 characters
                                        rawReport += line + Environment.NewLine;
                                        if ((indexStart = line.IndexOf(':')) != -1)
                                        {
                                            fieldName = line.Substring(0, indexStart).Trim();
                                            fieldValue = line.Substring(indexStart + 1).Trim();
                                            switch (fieldName)
                                            {
                                                case "Action":
                                                    finalRecipientAction = fieldValue;
                                                    break;
                                                case "Status":
                                                    finalRecipientStatus = fieldValue;
                                                    break;
                                                case "Diagnostic-Code":
                                                    finalRecipientDiagCode = fieldValue;
                                                    break;
                                                case "Will-Retry-Until":
                                                    willRetryUntil = fieldValue;
                                                    break;
                                                case "Final-Recipient":
                                                    var recipientStatus = new Status(finalRecipientAction, finalRecipientStatus, finalRecipientDiagCode);
                                                    if (!string.IsNullOrEmpty(willRetryUntil))
                                                        recipientStatus.WillRetryUntil = ParseDate(willRetryUntil);
                                                    report.Status.Add(finalRecipient, recipientStatus);
                                                    finalRecipientAction = null;
                                                    finalRecipientStatus = null;
                                                    finalRecipientDiagCode = null;
                                                    willRetryUntil = null;
                                                    finalRecipient = fieldValue;
                                                    break;
                                                default:
                                                    Debug.WriteLine(string.Format("Unknown field: {0}={1}", fieldName, fieldValue));
                                                    break;
                                            }
                                        }
                                    }
                                    var recipientStatus2 = new Status(finalRecipientAction, finalRecipientStatus, finalRecipientDiagCode);
                                    if (!string.IsNullOrEmpty(willRetryUntil))
                                        recipientStatus2.WillRetryUntil = ParseDate(willRetryUntil);
                                    report.Status.Add(finalRecipient, recipientStatus2);
                                    break;
                                default:
                                    Debug.WriteLine(string.Format("Unknown field: {0}={1}", fieldName, fieldValue));
                                    break;
                            }

                            if (endOfReport)
                                break;

                        }
                    }
                    report.RawReport = rawReport;

                    #region Tries to find the original message to extract its headers
                    for (int i = 0; i < 2; i++)
                    {
                        if (line == null)
                            line = string.Empty;
                        while (
                        SUPPORTED_FIELDS_CONTENT_TYPE_RFC822.FirstOrDefault(t => line.StartsWith(t, StringComparison.OrdinalIgnoreCase)) == null
                        &&
                        (line = reader.ReadLine()) != null
                        &&
                        (line != boundaryLine)) { }
                        if (line != null && SUPPORTED_FIELDS_CONTENT_TYPE_RFC822.FirstOrDefault(t => line.StartsWith(t, StringComparison.OrdinalIgnoreCase)) != null)
                            break;
                    }

                    if (line != null && SUPPORTED_FIELDS_CONTENT_TYPE_RFC822.FirstOrDefault(t => line.StartsWith(t, StringComparison.OrdinalIgnoreCase)) != null)
                    {
                        #region original message found
                        report._originalMsgHeaders = new List<KeyValuePair<string, string>>();
                        bool breakOnEmptyLine = false;
                        while ((line = reader.ReadLine()) != null
                            && line != boundaryLine)
                        {
                            if (line == string.Empty)
                            {
                                if (breakOnEmptyLine)
                                    break;
                                continue;
                            }
                            while ((nextChar = reader.Peek()) != -1 && LINE_CONTINUE_CHAR.Contains((char)nextChar))
                                line += " " + reader.ReadLine().TrimStart(LINE_CONTINUE_CHAR); // line of more than 76 characters
                            breakOnEmptyLine = true; // an empty line after a header field marks the end of headers
                            if ((indexStart = line.IndexOf(':')) != -1)
                            {
                                var fieldName = line.Substring(0, indexStart).Trim();
                                var fieldValue = line.Substring(indexStart + 1).Trim();
                                report._originalMsgHeaders.Add(new KeyValuePair<string, string>(fieldName, fieldValue));
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Debug.WriteLine("Original message not identified!");
                    }

                    #endregion

                    return report;
                }
                catch (Exception ex)
                {
					if (ex is MailDeliveryInfoException)
                    {
                        Debug.WriteLine(string.Format("Failed to parse this message: {0}\r\n{1}", ex.Message, rawMessage));
                        return null;
                    }
                    throw new Exception(string.Format("Failed to parse this message: {0}\r\n{1}", ex.Message, rawMessage), ex);
                }
                finally
                {
                    reader.Close();
                }
            }

            return null;
        }

    }

    
    public abstract class MailDeliveryInfoException : Exception
    {
        public MailDeliveryInfoException(string message) : base(message)
        {

        }

        public MailDeliveryInfoException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    [Serializable]
    public class UnknownStatusException : MailDeliveryInfoException
    {
        public UnknownStatusException(string status) : base(string.Format("Status '{0}' is not a valid status code. This message does not comply with RFC 3464.", status))
        {

        }
    }

}
