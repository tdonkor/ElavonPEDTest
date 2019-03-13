using System;
using System.IO;
using System.Threading;


namespace ElavonPEDTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("\n\tElavon Payment Simulator");
            Console.WriteLine("\t________________________\n\n");
            int amount = 0;
            Console.ForegroundColor = ConsoleColor.Green;

            using (var api = new ECRUtilATLApi())
            {
                var connectResult = api.Connect("192.168.254.98");
                if (connectResult != DiagnosticErrMsg.OK)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\tConnect Result = {connectResult}");
                    Console.WriteLine("\tExiting the Application...");
                    Thread.Sleep(10);
                    Environment.Exit(0);
                }
                else
                {
                   
                    Console.WriteLine("\t______________________");
                    Console.WriteLine($"\tConnection result = {connectResult}");
                    Console.WriteLine("\t______________________\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                try
                {
                    Console.Write("Enter the Amount(no decimal point allowed): ");
                    amount = int.Parse(Console.ReadLine());

                    var payResult = api.Pay(amount, out var payResponse);
                    Console.WriteLine($"\nIs Pay Result valid: {Utils.GetDiagRequestString(Convert.ToInt16(payResult))}");
                  
                    if ((DiagnosticErrMsg)Convert.ToInt32(payResponse.DiagRequestOut) == DiagnosticErrMsg.OK)
                    {
                        //display the customer ticket
                        Utils.CreateCustomerTicket(payResponse);

                        //disconnect
                        api.Disconnect();
                       
                    
                    Console.WriteLine("Press any key to continue....");
                    Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error : {ex}");
                }
            }
        }
   }
}
