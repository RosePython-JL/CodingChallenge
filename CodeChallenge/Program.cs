using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
          //Sample Input String
                string inputOne = @"
                    [orderId] => 212939129
                    [orderNumber] => INV10001
                    [salesTax] => 1.00
                    [amount] => 21.00
                    [terminal] => 5
                    [currency] => 1
                    [type] => purchase
                    [avsStreet] => 123 Road
                    [avsZip] => A1A 2B2
                    [customerCode] => CST1001
                    [cardId] => 18951828182
                    [cardHolderName] => John Smith
                    [cardNumber] => 5454545454545454
                    [cardExpiry] => 1025
                    [cardCVV] => 100";

                string inputTwo = @"Request=Credit Card.Auth Only&Version=4022&HD.Network_Status_Byte=*&HD.Application_ID=TZAHSK!&HD.Terminal_ID=12991kakajsjas&HD.Device_Tag=000123&07.POS_Entry_Capability=1&07.PIN_Entry_Capability=0&07.CAT_Indicator=0&07.Terminal_Type=4&07.Account_Entry_Mode=1&07.Partial_Auth_Indicator=0&07.Account_Card_Number=4242424242424242&07.Account_Expiry=1024&07.Transaction_Amount=142931&07.Association_Token_Indicator=0&17.CVV=200&17.Street_Address=123 Road SW&17.Postal_Zip_Code=90210&17.Invoice_Number=INV19291";
                
                string inputThree = @"{
                                ""MsgTypId"": 111231232300,
                                ""CardNumber"": ""4242424242424242"",
                                ""CardExp"": 1024,
                                ""CardCVV"": 240,
                                ""TransProcCd"": ""004800"",
                                ""TransAmt"": ""57608"",
                                ""MerSysTraceAudNbr"": ""456211"",
                                ""TransTs"": ""180603162242"",
                                ""AcqInstCtryCd"": ""840"",
                                ""FuncCd"": ""100"",
                                ""MsgRsnCd"": ""1900"",
                                ""MerCtgyCd"": ""5013"",
                                ""AprvCdLgth"": ""6"",
                                ""RtrvRefNbr"": ""1029301923091239"",
                            }";

                string inputFour = @"
                        <?xml version='1.0' encoding='UTF-8'?>
                        <Request>
	                        <NewOrder>
		                        <IndustryType>MO</IndustryType>
		                        <MessageType>AC</MessageType>
		                        <BIN>000001</BIN>
		                        <MerchantID>209238</MerchantID>
		                        <TerminalID>001</TerminalID>
		                        <CardBrand>VI</CardBrand>
		                        <CardDataNumber>5454545454545454</AccountNum>
		                        <Exp>1026</Exp>
		                        <CVVCVCSecurity>300</Exp>
		                        <CurrencyCode>124</CurrencyCode>
		                        <CurrencyExponent>2</CurrencyExponent>
		                        <AVSzip>A2B3C3</AVSzip>
		                        <AVSaddress1>2010 Road SW</AVSaddress1>
		                        <AVScity>Calgary</AVScity>
		                        <AVSstate>AB</AVSstate>
		                        <AVSname>JOHN R SMITH</AVSname>
		                        <OrderID>23123INV09123</OrderID>
		                        <Amount>127790</Amount>
	                        </NewOrder>
                        </Request>";
               //call the function for parsing the string and masking sensitive data
                maskedFunction(inputOne);
                maskedFunction(inputTwo);
                maskedFunction(inputThree);
                maskedFunction(inputFour);

            //Support Copy and Paste the string for the input
            Console.WriteLine("");
            Console.WriteLine("Input Date:");
                string line;
                string text = "";
                while ((line = Console.ReadLine()) != "")
                {
                    text += line + "\n";
                }

                maskedFunction(text);
        }

        static void maskedFunction(string originInput)
        {
            //Remove spaces and convert to lowercase
            string newInput = originInput.Replace(" ", String.Empty).ToLower();
           
            //Remove symbols and replace it as a space
            StringBuilder sb = new StringBuilder();
            foreach (char c in newInput) {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) {
                            sb.Append(c);
                }
                else
                {
                    sb.Append(" ");
                }
            }

            string sen = sb.ToString();
            
            sen = Regex.Replace(sen, @"\s+", " ");
            
           
            string[] newSen = sen.Split(' ');
            string cardNum = "";
            string cvv = "";
            string expiry = "";
            Console.WriteLine("");
            // Find out Card Number, CVV and Expiry Date in Array
            for (int i = 0; i < newSen.Length; i++ )
                 {
                //Looking for the keywords
                    if(newSen[i].Contains("card") || newSen[i].Contains("cvv") || newSen[i].Contains("exp") || newSen[i].Contains("expiry") || newSen[i].Contains("number"))
                    {
                        //credit card number = 16 digits
                        if(newSen[i+1].Length == 16)
                        {
                        Console.WriteLine("Card Number: "+ newSen[i+1]);
                         cardNum = String.Format(@"\b{0}\b", newSen[i+1]);
                        }
                        //expiry date = 4 digits
                         if(newSen[i+1].Length == 4)
                        {
                        Console.WriteLine("Expiry Number: "+ newSen[i+1]);
                         expiry = String.Format(@"\b{0}\b", newSen[i+1]);
                        }
                         //cvv = 3 digits
                         if(newSen[i+1].Length == 3)
                        {
                         Console.WriteLine("CVV: "+ newSen[i+1]);
                         cvv = String.Format(@"\b{0}\b", newSen[i+1]);;
                        }
                    }
                 }
            /*Console.WriteLine("");
            Console.WriteLine("Card Number: "+ cardNum);
            Console.WriteLine("CVV : "+ cvv);
            Console.WriteLine("Expiry Number: "+ expiry);*/

            //Replace the sensitive data with Asterix character
            //Print error if the result is null
            try
            {
                originInput = Regex.Replace(originInput, cardNum, "****************");
            }
            catch
            {
                Console.WriteLine("No Card Number");
            }
             try
            {
                originInput = Regex.Replace(originInput,cvv, "***");
            }
            catch
            {
                Console.WriteLine("No CVV");
            }
             try
            {
                originInput = Regex.Replace(originInput,expiry, "****");
            }
            catch
            {
                Console.WriteLine("No expiry");
            }

            //Print the result
            Console.WriteLine("Output: "+ originInput);

        }
    }
}
