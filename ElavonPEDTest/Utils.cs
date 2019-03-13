using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElavonPEDTest
{
    public enum TransactionType : ushort
    {
        Sale = 0,
        Refund = 1,
        Void = 2,
        Duplicata = 3,
        CashAdvance = 4,
        PWCB = 5,
        PreAuth = 6,
        Completion = 7,
        VerifyAccount = 8,
        Reversal = 9,
        Force = 10,
        MotoSale = 11,
        MotoRefund = 12,
        PreAuthReversal = 13,
        Cancel = 14,
        PreAuthIncrement = 15,
    }

    public enum TxnReceiptState : ushort
    {
        TXN_RECEIPT_DISABLED = 0,
        TXN_RECEIPT_ENABLED = 1,
    }


    public enum DiagnosticErrMsg : short
    {
        OK = 0,
        UnableToConnect = 1,
        UnableToSendRequest = 2,
        BadRequestFormat = 3,
        ReceptionTimeout = 5,
        ReceptionError = 6,
        BadResponseFormat = 7,
        BadResponseSize = 7,
        PEDNotAuthenticated = 8,
        UknownValue = 9,
        NoReceivedData = 10,
        FailureToStartServer = 11,
        ServerAlreadyStarted = 12,
        FailureToStopServer = 13,
        ServerAlreadyStopped = 14,
        ErrorToFindWinsockLibrary = 15,
        ErrorSocketCreation = 16,
        ErrorBindingSocket = 17,
        ErrorListeningSocket = 18,
        ErrortoAcceptConnection = 19
    }
   

    public struct TransactionResponse
    {
        public string MessageNumberOut;
        public string TransactionStatusOut;
        public string EntryMethodOut;
        public string AcquirerMerchantIDOut;
        public string DateTimeOut;
        public string CardSchemeNameOut;
        public string PANOut;
        public string StartDateOut;
        public string ExpiryDateOut;
        public string AuthorisationCodeOut;
        public string AcquirerResponseCodeOut;
        public string MerchantNameOut;
        public string MerchantAddress1Out;
        public string MerchantAddress2Out;
        public string MerchantAddress3Out;
        public string MerchantAddress4Out;
        public string TransactionCurrencyCodeOut;
        public short TransactionCurrencyExpOut;
        public string CardCurrencyCodeOut;
        public short CardCurrencyExpOut;
        public string TotalAmountOut;
        public string AdditionalAmountOut;
        public string EMVCardExpiryDateOut;
        public string AppEffectiveDateOut;
        public string AIDOut;
        public string AppPreferredNameOut;
        public string AppLabelOut;
        public string TerminalIdentifierOut;
        public string EMVTransactionTypeOut;
        public string AppCryptogramOut;
        public string RetrievalReferenceNumOut;
        public string InvoiceNumberOut;
        public string BatchNumberOut;
        public string AcquirerNameOut;
        public string CustomLine1Out;
        public string CustomLine2Out;
        public string CustomLine3Out;
        public string CustomLine4Out;
        public short IsDCCTransactionOut;
        public string DCCAmountOut;
        public string ConversionRateOut;
        public short FXExponentAppliedOut;
        public string CommissionOut;
        public string TerminalCountryCodeOut;
        public string TerminalCurrencyCodeOut;
        public short TerminalCurrencyExpOut;
        public string FXMarkupOut;
        public string PANSequenceNumberOut;
        public string CashierIDOut;
        public string TableIdentifierOut;
        public string CardholderNameOut;
        public string AvailableBalanceOut;
        public string PreAuthRefNumOut;
        public string HostTextOut;
        public short IsTaxFreeRequiredOut;
        public short IsExchangeRateUpdateRequiredOut;
        public string ApplicationIDOut;
        public string CommercialCodeDataOut;
        public string CardResponseValueOut;
        public string DonationAmountOut;
        public string AVSResponseOut;
        public string PartialAuthAmountOut;
        public string SpanishOpNumberOut;
        public short IsSignatureRequiredOut;
        public short IsFanfareTransactionOut;
        public string LoyaltyTransactionInfoOut;
        public string FanfareTransactionIdentifierOut;
        public string FanfareApprovalCodeOut;
        public string LoyaltyDiscountAmountOut;
        public string FanfareWebURLOut;
        public string LoyaltyProgramNameOut;
        public string FanfareIdentifierOut;
        public string LoyaltyAccessCodeOut;
        public string LoyaltyMemberTypeOut;
        public short FanfareBalanceCountOut;
        public short LoyaltyPromoCodeCountOut;
        public short DiagRequestOut;
    }


    public class Utils
    {
        /// <summary>
        /// Check the numeric value of the amount
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static int GetNumericAmountValue(int amount)
        {

            if (amount <= 0)
            {
                Console.WriteLine("Invalid pay amount");
                amount = 0;
            }

            return amount;
        }

        public static string TransactionOutResult(string transactionStatusOut)
        {
            string transactionOutStr = string.Empty;

            switch (transactionStatusOut)
            {
                case "0": { transactionOutStr = "Authorised"; } break;
                case "1": { transactionOutStr = "Not Authorised "; } break;
                case "2": { transactionOutStr = "Not processed – transaction failed before or during card entry"; } break;
                case "3": { transactionOutStr = "Unable to authorise – transaction approved by host but declined by card "; } break;
                case "4": { transactionOutStr = "Unable to process – Voice referral declined due to no referral number"; } break;
                case "5": { transactionOutStr = "Unable to connect "; } break;
                case "6": { transactionOutStr = "Void"; } break;
                case "7": { transactionOutStr = "Cancelled"; } break;
                case "8": { transactionOutStr = "Invalid password"; } break;
                case "9": { transactionOutStr = "Amount exceed maximum limit"; } break;
                case "10": { transactionOutStr = "Connection failure"; } break;
                case "11": { transactionOutStr = "Timeout reached"; } break;
                case "12": { transactionOutStr = "Invoice not found"; } break;
                case "13": { transactionOutStr = "CashBack exceed maximum limit - Transaction Complete"; } break;
                case "14": { transactionOutStr = "CashBack not allowed"; } break;
                case "15": { transactionOutStr = "Incomplete(the ‘Do Transaction Completion’ service should be invoked)"; } break;
            }
                return transactionOutStr;
        }

        /// <summary>
        /// checks the DiagRequestOut variable
        /// </summary>
        /// <param name="DiagRequest"></param>
        /// <returns></returns>
        public static string GetDiagRequestString(short DiagRequest)
        {
            string DiagRequestString;

            switch (DiagRequest)
            {
                case 0x00: { DiagRequestString = "OK"; } break;
                case 0x01: { DiagRequestString = "Unable To Connect"; } break;
                case 0x02: { DiagRequestString = "Unable To Send Request"; } break;
                case 0x03: { DiagRequestString = "Bad Request Format"; } break;
                case 0x04: { DiagRequestString = "Bad Request Size"; } break;
                case 0x05: { DiagRequestString = "Reception Timeout"; } break;
                case 0x06: { DiagRequestString = "Reception Error"; } break;
                case 0x07: { DiagRequestString = "Bad Response Format"; } break;
                case 0x08: { DiagRequestString = "Bad Response Size"; } break;
                case 0x09: { DiagRequestString = "PED Not Authenticated"; } break;
                case 0x0A: { DiagRequestString = "No Received Data"; } break;
                case 0x0B: { DiagRequestString = "Failure To Start Server"; } break;
                case 0x0C: { DiagRequestString = "Server Already Started"; } break;
                case 0x0D: { DiagRequestString = "Failure to Stop Server"; } break;
                case 0x0E: { DiagRequestString = "Server Already Stopped"; } break;
                case 0x0F: { DiagRequestString = "Error Winsock Library"; } break;
                case 0x10: { DiagRequestString = "Error Socket Creation"; } break;
                case 0x11: { DiagRequestString = "Error Binding Socket"; } break;
                case 0x12: { DiagRequestString = "Error Listening Socket"; } break;
                case 0x13: { DiagRequestString = "Error To Accept Connection"; } break;
                case 0x14: { DiagRequestString = "Event Server Is Down"; } break;
                default: { DiagRequestString = "Unknown"; } break;
            }

            return DiagRequestString;
        }

       

        /// <summary>
        /// Utility to display the terminal status
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string DisplayTerminalStatus(short num)
        {
            string result = string.Empty;

            switch (num)
            {
                case 0: { result = "STATE_UNKNOWN"; } break;
                case 1: { result = "STATE_IDLE"; } break;
                case 2: { result = "STATE_BUSY"; } break;
                case 3: { result = "STATE_CARD_INSERT"; } break;
                case 4: { result = "STATE_PIN_ENTRY_FIRST_ATTEMPT"; } break;
                case 5: { result = "STATE_PIN_ENTRY_SECOND_ATTEMPT"; } break;
                case 6: { result = "STATE_PIN_ENTRY_THIRD_ATTEMPT"; } break;
                case 7: { result = "STATE_PIN_ENTRY_FAILED"; } break;
                case 8: { result = "STATE_GRATUITY_ENTRY"; } break;
                case 9: { result = "STATE_AUTHORIZING"; } break;
                case 10: { result = "STATE_COMPLETION"; } break;
                case 11: { result = "STATE_CANCELLED"; } break;
                case 12: { result = "STATE_AMOUNT_CONFIRMATION"; } break;
                case 13: { result = "STATE_SENDING"; } break;
                case 14: { result = "STATE_RECEIVING"; } break;
                case 15: { result = "STATE_UNSPECIFIED_INPUT"; } break;
                case 16: { result = "STATE_PROCESSING"; } break;
                case 17: { result = "STATE_CARD_REMOVAL"; } break;
                case 18: { result = "STATE_PRINTING_MERCHANT_COPY"; } break;
                case 19: { result = "STATE_PRINTING_CUSTOMER_COPY"; } break;
                case 20: { result = "STATE_NO_MORE_PAPER"; } break;
                case 21: { result = "STATE_LOYALTY_OPTION_SELECTION"; } break;
                case 22: { result = "STATE_PHONE_ENTRY"; } break;
                case 23: { result = "STATE_PROMO_CODE_ENTRY"; } break;
                case 24: { result = "STATE_LOYALTY_MEMBER_SELECTION"; } break;
                case 25: { result = "STATE_REWARD_OFFER"; } break;
                case 26: { result = "STATE_EXISTING_ACCOUNT"; } break;
                case 27: { result = "STATE_INVALID_ACCOUNT"; } break;
                case 28: { result = "STATE_LINK_CARD_PAYMENT"; } break;
                case 29: { result = "STATE_ADD_CARD_PAYMENT"; } break;
                case 30: { result = "STATE_CASHBACK_ENTRY"; } break;
                case 31: { result = "STATE_COMMERCIAL_CODE_ENTRY"; } break;
                case 32: { result = "STATE_WAITING_CARD"; } break;
                case 33: { result = "STATE_WAITING_DCC_ACCEPTANCE"; } break;
                default: { result = "ERROR"; } break;
            }

            return result;
        }

        /// <summary>
        /// Get the name of Transaction
        /// </summary>
        public static string GetTransactionTypeString(short TransactionTypeNum)
        {
            string TransactionTypeString;

            switch (TransactionTypeNum)
            {
                case (short)TransactionType.Sale: TransactionTypeString = "SALE"; break;
                case (short)TransactionType.Refund: TransactionTypeString = "REFUND"; break;
                case (short)TransactionType.Void: TransactionTypeString = "VOID"; break;
                case (short)TransactionType.Duplicata: TransactionTypeString = "DUPLICATA"; break;
                case (short)TransactionType.CashAdvance: TransactionTypeString = "CASH ADVANCE"; break;
                case (short)TransactionType.PWCB: TransactionTypeString = "PWCB"; break;
                case (short)TransactionType.PreAuth: TransactionTypeString = "PRE-AUTH"; break;
                case (short)TransactionType.Completion: TransactionTypeString = "COMPLETION"; break;
                case (short)TransactionType.VerifyAccount: TransactionTypeString = "VERIFY ACCOUNT"; break;
                case (short)TransactionType.Reversal: TransactionTypeString = "REVERSAL"; break;
                case (short)TransactionType.Force: TransactionTypeString = "FORCE"; break;
                case (short)TransactionType.MotoSale: TransactionTypeString = "MOTO SALE"; break;
                case (short)TransactionType.MotoRefund: TransactionTypeString = "MOTO REFUND"; break;
                case (short)TransactionType.PreAuthReversal: TransactionTypeString = "PRE-AUTH REVERSAL"; break;
                case (short)TransactionType.Cancel: TransactionTypeString = "CANCEL"; break;
                case (short)TransactionType.PreAuthIncrement: TransactionTypeString = "PRE-AUTH INCREMENT"; break;
                default: TransactionTypeString = "Unknown"; break;
            }

            return TransactionTypeString;
        }

        public static short GetTransactionTypeNum(string TransactionTypeString)
        {
            short TransactionTypeNum;

            switch (TransactionTypeString.ToUpper())
            {
                case "SALE": { TransactionTypeNum = (short)TransactionType.Sale; } break;
                case "REFUND": { TransactionTypeNum = (short)TransactionType.Refund; } break;
                case "VOID": { TransactionTypeNum = (short)TransactionType.Void; } break;
                case "DUPLICATA": { TransactionTypeNum = (short)TransactionType.Duplicata; } break;
                case "CASH ADVANCE": { TransactionTypeNum = (short)TransactionType.CashAdvance; } break;
                case "PWCB": { TransactionTypeNum = (short)TransactionType.PWCB; } break;
                case "PRE-AUTH": { TransactionTypeNum = (short)TransactionType.PreAuth; } break;
                case "COMPLETION": { TransactionTypeNum = (short)TransactionType.Completion; } break;
                case "VERIFY ACCOUNT": { TransactionTypeNum = (short)TransactionType.VerifyAccount; } break;
                case "REVERSAL": { TransactionTypeNum = (short)TransactionType.Reversal; } break;
                case "FORCE": { TransactionTypeNum = (short)TransactionType.Force; } break;
                case "MOTO SALE": { TransactionTypeNum = (short)TransactionType.MotoSale; } break;
                case "MOTO REFUND": { TransactionTypeNum = (short)TransactionType.MotoRefund; } break;
                case "PRE-AUTH REVERSAL": { TransactionTypeNum = (short)TransactionType.PreAuthReversal; } break;
                case "CANCEL": { TransactionTypeNum = (short)TransactionType.Cancel; } break;
                case "PRE-AUTH INCREMENT": { TransactionTypeNum = (short)TransactionType.PreAuthIncrement; } break;
                default: { TransactionTypeNum = -1; } break;
            }

            return TransactionTypeNum;
        }

        /// <summary>
        /// Get entry method string
        /// </summary>
        public static string GetEntryMethodString(string EntryMethod)
        {
            string EntryMethodString;

            switch (EntryMethod)
            {
                case "0": { EntryMethodString = "None"; } break;
                case "1": { EntryMethodString = "Keyed"; } break;
                case "2": { EntryMethodString = "Swiped"; } break;
                case "3": { EntryMethodString = "Inserted"; } break;
                case "4": { EntryMethodString = "Waved"; } break;
                case "5": { EntryMethodString = "Keyed Not Present"; } break;
                case "6": { EntryMethodString = "Swiped Fallback"; } break;
                case "7": { EntryMethodString = "PA Ref Num"; } break;
                default: { EntryMethodString = "Unknown"; } break;
            }

            return EntryMethodString;
        }

        /// <summary>
        /// Get the Currency Symbol String
        /// </summary>
        public static string GetCurrencySymbol(string CurrencyNum)
        {
            short CurrencyCode;
            string CurrencySymbol;

            try
            {
                CurrencyCode = short.Parse(CurrencyNum);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }

            switch (CurrencyCode)
            {
                case 36: { CurrencySymbol = "AUD"; } break;
                case 124: { CurrencySymbol = "CAD"; } break;
                case 156: { CurrencySymbol = "CNY"; } break;
                case 203: { CurrencySymbol = "CZK"; } break;
                case 208: { CurrencySymbol = "DKK"; } break;
                case 344: { CurrencySymbol = "HKD"; } break;
                case 348: { CurrencySymbol = "HUF"; } break;
                case 376: { CurrencySymbol = "ILS"; } break;
                case 392: { CurrencySymbol = "JPY"; } break;
                case 410: { CurrencySymbol = "KRW"; } break;
                case 554: { CurrencySymbol = "NZD"; } break;
                case 578: { CurrencySymbol = "NOK"; } break;
                case 643: { CurrencySymbol = "RUB"; } break;
                case 682: { CurrencySymbol = "SAR"; } break;
                case 702: { CurrencySymbol = "SGD"; } break;
                case 710: { CurrencySymbol = "ZAR"; } break;
                case 752: { CurrencySymbol = "SEK"; } break;
                case 756: { CurrencySymbol = "CHF"; } break;
                case 784: { CurrencySymbol = "AED"; } break;
                case 826: { CurrencySymbol = "GBP"; } break;
                case 840: { CurrencySymbol = "USD"; } break;
                case 949: { CurrencySymbol = "TRY"; } break;
                case 978: { CurrencySymbol = "EUR"; } break;
                case 985: { CurrencySymbol = "PLN"; } break;
                default: { CurrencySymbol = CurrencyCode.ToString(); } break;
            }

            return CurrencySymbol;
        }

        public static string FormatReceiptAmount(string amountStr)
        {
            float amount = 0.0f;
            amount = float.Parse(amountStr) / 100;
            return amount.ToString("n2");
        }

        /// <summary>
        /// Create Customer Ticket to output the reciept
        /// </summary>
        /// <param name="ticket"></param>
        public static void CreateCustomerTicket(TransactionResponse ticket)
        {
            StringBuilder ticketContent = new StringBuilder();
            string ticketStr = string.Empty;

            //get the reponse details for the ticket

            ticketContent.Append($"\n\tCUSTOMER RECEIPT\n");
            ticketContent.Append($"\t_______________________\n\n");
            ticketContent.Append($"\tMerchant Name:  {ticket.MerchantNameOut}\n");         // Merchant name
            ticketContent.Append($"\tMerchant Addr1: {ticket.MerchantAddress1Out}\n");     // Merchant Addr1
            ticketContent.Append($"\tMerchant Addr2: {ticket.MerchantAddress2Out}\n");     // Merchant Addr2
            ticketContent.Append($"\tAcquirer MerchantId: {ticket.AcquirerMerchantIDOut}\n");   // Aquirer Merchant ID name
            ticketContent.Append($"\tTID: {ticket.TerminalIdentifierOut}\n");           // Terminal ID
            ticketContent.Append($"\tCard Scheme Name: {ticket.CardSchemeNameOut}\n");       // Card Schema name
            ticketContent.Append($"\tAID: {ticket.AIDOut}\n");                  // AID
            ticketContent.Append($"\tPAN: {ticket.PANOut}\n");                  // Pan
            ticketContent.Append($"\tPAN Sequence Number: {ticket.PANSequenceNumberOut}\n");            // Pan Seq num
            ticketContent.Append($"\tMessage Number Out:  {ticket.MessageNumberOut}\n");
            ticketContent.Append($"\tSignature required:  {ticket.IsSignatureRequiredOut}\n");
            ticketContent.Append($"\tEntry Method: {GetEntryMethodString(ticket.EntryMethodOut)}\n");
            ticketContent.Append($"\tTransacion type: {GetTransactionTypeString(Convert.ToInt16(ticket.TransactionStatusOut))}\n");      // Transaction Type
            ticketContent.Append($"\tCurrency: {GetCurrencySymbol(ticket.TerminalCurrencyCodeOut)}\n");
            ticketContent.Append($"\tPurchase Amount: {FormatReceiptAmount(ticket.TotalAmountOut)}\n");  
            ticketContent.Append($"\tTransaction Date/Time: {(ticket.DateTimeOut)}\n");
            //ticketContent.Append($"\tTransaction status:{TransactionOutResult(ticket.TransactionStatusOut)}\n");
            ticketContent.Append($"\n\tHost Text out: {ticket.HostTextOut}\n");
            ticketContent.Append("\t_______________________\n");
            ticketContent.Append("\t\t" + TransactionOutResult(ticket.TransactionStatusOut) + "\n");
            ticketContent.Append("\t_______________________\n");

            ticketStr = ticketContent.ToString();

            Console.WriteLine(ticketStr);

        }
    }
}
