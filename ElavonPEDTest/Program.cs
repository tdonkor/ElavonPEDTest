using System;
using System.IO;
using System.Threading;


namespace ElavonPEDTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string answer = "Y";
            

            while( answer== "Y")
            { 
           
               // Console.Clear();

                Console.WriteLine("\n\tElavon Payment Simulator");
                Console.WriteLine("\t________________________\n\n");
                int amount = 0;

                using (var api = new ECRUtilATLApi())
                {
                   //var connectResult = api.Connect("192.168.254.98");
                   var connectResult = api.Connect("1.1.1.2");

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
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\tConnection result = {connectResult}\n");
                        Console.ForegroundColor = ConsoleColor.White;
                    }


                    Console.Write("Enter the Amount(no decimal point allowed): ");
                    try
                    {
                        amount = int.Parse(Console.ReadLine());
                        var payResult = api.Pay(amount, out var payResponse);
                        Console.WriteLine($"\nIs Pay Result valid: {Utils.GetDiagRequestString(Convert.ToInt16(payResult))}");

                     
                        if ((DiagnosticErrMsg)Convert.ToInt16(payResponse.DiagRequestOut) == DiagnosticErrMsg.OK)
                        {
                            //display the customer ticket
                            Utils.CreateCustomerTicket(payResponse);

                            //disconnect
                            api.Disconnect();

                        }
                    }
                    catch (Exception ex )
                    {

                        Console.WriteLine("Error" + ex.Message);
                    }
                    
                    Console.Write("\n\nWould you like another transaction?:(Y/N) ");
                    answer = Console.ReadLine().ToUpper();
                    if (string.IsNullOrEmpty(answer))
                    {
                        answer = "Y";
                    }


                }
      
            }

 
        }
   }
}
