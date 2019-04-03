using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ECRUtilATLLib;

namespace ElavonPEDTest
{
    public class ECRUtilATLApi : IDisposable
    {
        TerminalIPAddress termimalIPAddress;
        TerminalEventClass terminalEvent;
        InitTxnReceiptPrint initTxnReceiptPrint;
        TimeDate pedDateTime;
        Status pedStatus;
        TransactionClass transaction;
        TransactionResponse transactionResponse;
        Signature checkSignature;
      //  Thread SignatureVerificationThread;


        public ECRUtilATLApi()
        {
            transaction = new TransactionClass();
        }

        public DiagnosticErrMsg Connect(string ipAddress)
        {

            DiagnosticErrMsg diagErr = DiagnosticErrMsg.OK;

            //Check IP Address
            diagErr = CheckIPAddress(ipAddress);
            if (diagErr != DiagnosticErrMsg.OK) return diagErr;

            //Check ECR Server
            diagErr = CheckECRServer();
            if (diagErr != DiagnosticErrMsg.OK) return diagErr;

            //Check Reciept status
            diagErr = CheckReceiptInit();
            if (diagErr != DiagnosticErrMsg.OK) return diagErr;

            //Check the status
            diagErr = CheckStatus();
            if (diagErr != DiagnosticErrMsg.OK) return diagErr;

            //set the PED timeDate
            diagErr =  SetTimeDate();
            if (diagErr != DiagnosticErrMsg.OK) return diagErr;

            //no error
            return diagErr;
        }

        /// <summary>
        /// Disconect the transaction
        /// </summary>
        public DiagnosticErrMsg Disconnect()
        {
            Console.WriteLine("Disconnecting...");

            transaction = null;

            DiagnosticErrMsg disconnResult = DiagnosticErrMsg.UknownValue;

            if ((DiagnosticErrMsg)Convert.ToInt32(transactionResponse.DiagRequestOut) == DiagnosticErrMsg.OK)
                disconnResult = DiagnosticErrMsg.OK;
            else
                disconnResult = DiagnosticErrMsg.UknownValue;

            return disconnResult;

        }

        /// <summary>
        /// The Payment
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public DiagnosticErrMsg Pay(int amount, out TransactionResponse result)
        {
            int intAmount;
            Console.WriteLine($"Executing payment - Amount: {amount}");

            //check amount is valid
            intAmount = Utils.GetNumericAmountValue(amount);

            if (intAmount == 0)
                throw new Exception("Error in input");

            // Transaction Sale details
            //
            DoTransaction(amount, TransactionType.Sale.ToString());

            result = PopulateResponse(transaction);
            return (DiagnosticErrMsg)Convert.ToInt32(transaction.DiagRequestOut);
        }
        

        /// <summary>
        /// Do the financial transaction
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="transactionType"></param>
        public void DoTransaction(int amount, string transactionType)
        {
            Random randomNum = new Random();
            transaction.MessageNumberIn = randomNum.Next(100).ToString();
            transaction.Amount1In = amount.ToString().PadLeft(12, '0'); 
            transaction.Amount1LabelIn = "AMOUNT";
            transaction.TransactionTypeIn = "0";

            //check if a signature needed
            CheckSignatureVerification();

            // Launch the transaction
            transaction.DoTransaction();
           
            Console.WriteLine($"Terminal Event = {Utils.GetEventRequestString(terminalEvent.EventIdentifierOut)}");

            if ( transaction.DiagRequestOut == 0) //no error
            {
                //display all the returned data
                Console.WriteLine("No error on transaction: " + Utils.GetTransactionTypeString(Convert.ToInt16(transaction.TransactionStatusOut)));
            }
            else
            {
                Console.WriteLine("Transaction Error: " + Utils.GetDiagRequestString(transaction.DiagRequestOut));
                return;
            }

            Console.WriteLine($"Transaction status:{Utils.TransactionOutResult(transaction.TransactionStatusOut)}\n");

     

        }

        
        /// <summary>
        /// Verify if a Signature is needed 
        /// then Void the transaction if it is
        /// </summary>
        public async void CheckSignatureVerification()
        {

            try
            {
                checkSignature = new SignatureClass();

                Console.WriteLine("Running Check Signature - call the wait terminal event");

                terminalEvent.GetServerState();
                Console.WriteLine($"Get Event Id before Wait = {Utils.GetEventRequestString(terminalEvent.EventIdentifierOut)}");
                Console.WriteLine("Display Terminal State before wait  : " + Utils.DisplayTerminalStatus(terminalEvent.DiagRequestOut));

                // the main thread will continue after the
                await Task.Run(new Action(terminalEvent.WaitTerminalEvent));

                Console.WriteLine($"Get Event Id after Wait = {Utils.GetEventRequestString(terminalEvent.EventIdentifierOut)}");
                Console.WriteLine("Display Terminal State after wait : " + Utils.DisplayTerminalStatus(terminalEvent.DiagRequestOut));


                //void the signature if set
                checkSignature.SignatureStatusIn = 0x00; /* SIGN_NOT_ACCEPTED */
                checkSignature.SetSignStatus();


                Console.WriteLine($"Signature status : {checkSignature.DiagRequestOut}");

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error" + ex);
            }
           
           
        }


        /// <summary>
        /// Populate the transactionResponse object
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>the transaction response</returns>
        private TransactionResponse PopulateResponse(TransactionClass transaction)
        {
            Console.WriteLine("Populating Transaction Response");
            /* Set the transaction output */
            transactionResponse.MessageNumberOut = transaction.MessageNumberOut;
            transactionResponse.TransactionStatusOut = transaction.TransactionStatusOut;
            transactionResponse.EntryMethodOut = transaction.EntryMethodOut;
            transactionResponse.AcquirerMerchantIDOut = transaction.AcquirerMerchantIDOut;
            transactionResponse.DateTimeOut = transaction.DateTimeOut;
            transactionResponse.CardSchemeNameOut = transaction.CardSchemeNameOut;
            transactionResponse.PANOut = transaction.PANOut;
            transactionResponse.StartDateOut = transaction.StartDateOut;
            transactionResponse.ExpiryDateOut = transaction.ExpiryDateOut;
            transactionResponse.AuthorisationCodeOut = transaction.AuthorisationCodeOut;
            transactionResponse.AcquirerResponseCodeOut = transaction.AcquirerResponseCodeOut;
            transactionResponse.MerchantNameOut = transaction.MerchantNameOut;
            transactionResponse.MerchantAddress1Out = transaction.MerchantAddress1Out;
            transactionResponse.MerchantAddress2Out = transaction.MerchantAddress2Out;
            transactionResponse.MerchantAddress3Out = transaction.MerchantAddress3Out;
            transactionResponse.MerchantAddress4Out = transaction.MerchantAddress4Out;
            transactionResponse.TransactionCurrencyCodeOut = transaction.TransactionCurrencyCodeOut;
            transactionResponse.TransactionCurrencyExpOut = transaction.TransactionCurrencyExponentOut;
            transactionResponse.CardCurrencyCodeOut = transaction.CardCurrencyCodeOut;
            transactionResponse.CardCurrencyExpOut = transaction.CardCurrencyExponentOut;
            transactionResponse.TotalAmountOut = transaction.TotalAmountOut;
            transactionResponse.AdditionalAmountOut = transaction.AdditionalAmountOut;
            transactionResponse.EMVCardExpiryDateOut = transaction.EMVCardExpirationDateOut;
            transactionResponse.AppEffectiveDateOut = transaction.AppEffectiveDateOut;
            transactionResponse.AIDOut = transaction.AIDOut;
            transactionResponse.AppPreferredNameOut = transaction.AppPreferredNameOut;
            transactionResponse.AppLabelOut = transaction.AppLabelOut;
            transactionResponse.TerminalIdentifierOut = transaction.TerminalIdentifierOut;
            transactionResponse.EMVTransactionTypeOut = transaction.EMVTransactionTypeOut;
            transactionResponse.AppCryptogramOut = transaction.AppCryptogramOut;
            transactionResponse.RetrievalReferenceNumOut = transaction.RetrievalReferenceNumberOut;
            transactionResponse.InvoiceNumberOut = transaction.InvoiceNumberOut;
            transactionResponse.BatchNumberOut = transaction.BatchNumberOut;
            transactionResponse.AcquirerNameOut = transaction.AcquirerNameOut;
            transactionResponse.CustomLine1Out = transaction.CustomLine1Out;
            transactionResponse.CustomLine2Out = transaction.CustomLine2Out;
            transactionResponse.CustomLine3Out = transaction.CustomLine3Out;
            transactionResponse.CustomLine4Out = transaction.CustomLine4Out;
            transactionResponse.IsDCCTransactionOut = transaction.IsDCCTransactionOut;
            transactionResponse.DCCAmountOut = transaction.DCCAmountOut;
            transactionResponse.ConversionRateOut = transaction.ConversionRateOut;
            transactionResponse.FXExponentAppliedOut = transaction.FXExponentAppliedOut;
            transactionResponse.CommissionOut = transaction.CommissionOut;
            transactionResponse.TerminalCountryCodeOut = transaction.TerminalCountryCodeOut;
            transactionResponse.TerminalCurrencyCodeOut = transaction.TerminalCurrencyCodeOut;
            transactionResponse.TerminalCurrencyExpOut = transaction.TerminalCurrencyExponentOut;
            transactionResponse.FXMarkupOut = transaction.FXMarkupOut;
            transactionResponse.PANSequenceNumberOut = transaction.PANSequenceNumberOut;
            transactionResponse.CashierIDOut = transaction.CashierIdentifierOut;
            transactionResponse.TableIdentifierOut = transaction.TableIdentifierOut;
            transactionResponse.CardholderNameOut = transaction.CardholderNameOut;
            transactionResponse.AvailableBalanceOut = transaction.AvailableBalanceOut;
            transactionResponse.PreAuthRefNumOut = transaction.PreAuthRefNumOut;
            transactionResponse.HostTextOut = transaction.HostTextOut;
            transactionResponse.IsTaxFreeRequiredOut = transaction.IsTaxFreeRequiredOut;
            transactionResponse.IsExchangeRateUpdateRequiredOut = transaction.IsExchangeRateUpdateRequiredOut;
            transactionResponse.ApplicationIDOut = transaction.ApplicationIDOut;
            transactionResponse.CommercialCodeDataOut = transaction.CommercialCodeDataOut;
            transactionResponse.CardResponseValueOut = transaction.CardResponseValueOut;
            transactionResponse.DonationAmountOut = transaction.DonationAmountOut;
            transactionResponse.AVSResponseOut = transaction.AVSResponseOut;
            transactionResponse.PartialAuthAmountOut = transaction.PartialAuthAmountOut;
            transactionResponse.SpanishOpNumberOut = transaction.SpanishOpNumberOut;
            transactionResponse.IsSignatureRequiredOut = transaction.IsSignatureRequiredOut;
            transactionResponse.IsFanfareTransactionOut = transaction.IsFanfareTransactionOut;
            transactionResponse.LoyaltyTransactionInfoOut = transaction.LoyaltyTransactionInfoOut;
            transactionResponse.FanfareTransactionIdentifierOut = transaction.FanfareTransactionIdentifierOut;
            transactionResponse.FanfareApprovalCodeOut = transaction.FanfareApprovalCodeOut;
            transactionResponse.LoyaltyDiscountAmountOut = transaction.LoyaltyDiscountAmountOut;
            transactionResponse.FanfareWebURLOut = transaction.FanfareWebURLOut;
            transactionResponse.LoyaltyProgramNameOut = transaction.LoyaltyProgramNameOut;
            transactionResponse.FanfareIdentifierOut = transaction.FanfareIdentifierOut;
            transactionResponse.LoyaltyAccessCodeOut = transaction.LoyaltyAccessCodeOut;
            transactionResponse.LoyaltyMemberTypeOut = transaction.LoyaltyMemberTypeOut;
            transactionResponse.FanfareBalanceCountOut = transaction.FanfareBalanceCountOut;
            transactionResponse.LoyaltyPromoCodeCountOut = transaction.LoyaltyPromoCodeCountOut;
            transactionResponse.DiagRequestOut = transaction.DiagRequestOut;

            return transactionResponse;
        }

        /// <summary>
        /// Set the Ped Date/Time
        /// </summary>
        /// <returns>diagnostic value</returns>
        private DiagnosticErrMsg SetTimeDate()
        {
            //Set the PED Date time inputs
            pedDateTime = new TimeDate();
            pedDateTime.YearIn = DateTime.Now.Year.ToString();
            pedDateTime.MonthIn = DateTime.Now.Month.ToString();
            pedDateTime.DayIn = DateTime.Now.Day.ToString();
            pedDateTime.HourIn = DateTime.Now.Hour.ToString();
            pedDateTime.MinuteIn = DateTime.Now.Minute.ToString();
            pedDateTime.SecondIn = DateTime.Now.Second.ToString();

            //check the connection result 
            return (DiagnosticErrMsg)(pedDateTime.DiagRequestOut);
        }

        /// <summary>
        /// Check the PED Status
        /// </summary>
        /// <returns>Diagnostic value</returns>
        private DiagnosticErrMsg CheckStatus()
        {
            //Check status at Idle
            pedStatus = new Status();
            pedStatus.GetTerminalState();
            Console.WriteLine($"Status: {Utils.DisplayTerminalStatus(pedStatus.StateOut)}");

            //check the connection result 
            return (DiagnosticErrMsg)(pedStatus.DiagRequestOut);
        }

        /// <summary>
        /// Disable reciept printing
        /// </summary>
        /// <returns>Diagnostic value</returns>
        private DiagnosticErrMsg CheckReceiptInit()
        {
            // disable the receipt Printing
            initTxnReceiptPrint = new InitTxnReceiptPrint();
            initTxnReceiptPrint.StatusIn = (short)TxnReceiptState.TXN_RECEIPT_DISABLED;
            initTxnReceiptPrint.SetTxnReceiptPrintStatus();
            
            //check printing disabled
            if (initTxnReceiptPrint.DiagRequestOut == 0)
                Console.WriteLine("apiInitTxnReceiptPrint OFF");
            else
                Console.WriteLine("apiInitTxnReceiptPrint ON");

            //check the connection result 
            return (DiagnosticErrMsg)(initTxnReceiptPrint.DiagRequestOut);
        }

        /// <summary>
        /// Check ECR Server is running
        /// </summary>
        /// <returns>Diagnostic value</returns>
        private DiagnosticErrMsg CheckECRServer()
        {
            //Set the Event Server 
            terminalEvent = new TerminalEventClass();

            // Start the ECR server
            terminalEvent.StartServer();

            terminalEvent.GetServerState();
            Console.WriteLine($"Terminal Start Check: {Utils.GetDiagRequestString(terminalEvent.DiagRequestOut)}");
            //check the connection result 
            return (DiagnosticErrMsg)(terminalEvent.DiagRequestOut);
        }

        /// <summary>
        /// Check the IP Address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>Diagnostic value</returns>
        private DiagnosticErrMsg CheckIPAddress(string ipAddress)
        {
            //set static IP address
            termimalIPAddress = new TerminalIPAddress();
            termimalIPAddress.IPAddressIn = ipAddress;
            termimalIPAddress.SetIPAddress();
            Console.WriteLine($"IP Address Out: {termimalIPAddress.IPAddressOut}");

            //check the connection result 
            return (DiagnosticErrMsg)(termimalIPAddress.DiagRequestOut);
        }

       
        public void Dispose()
        {
        }
    }
}
