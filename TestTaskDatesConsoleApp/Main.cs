using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestTaskDatesCommon.Models;
using TestTaskDatesCommon.Payloads;

namespace TestTaskDatesConsoleApp
{
    class Main
    {
        public string _token = null;
        private string _login = "";

        private bool IsAuthorized => !String.IsNullOrEmpty(_login);

        public void Test()
        {

        }

        public void MenuMain(string firstMessage = null)
        {
            while (true)
            {
                Console.Clear();
                if (firstMessage != null)
                {
                    Console.WriteLine(firstMessage);
                    firstMessage = null;
                }

                Console.WriteLine();
                Console.Write("============== ");
                Console.Write(IsAuthorized ? string.Format("Authorized as " + _login) : "Unauthorized");
                Console.WriteLine(" ==============");
                Console.WriteLine();
                Console.WriteLine("Type number of command:");
                Console.WriteLine(String.Format("1: {0}.", IsAuthorized ? "Logout" : "Login / Registration"));
                Console.WriteLine("2: Print list of all date-ranges in DB.");

                if (IsAuthorized)
                {
                    Console.WriteLine("3: Insert date-range into DB.");
                    Console.WriteLine("4: Check if date-range is inside of any DB date-range.");
                }

                Console.WriteLine();

                int? intInput = ConsoleReadLineInputInteger();
                if (intInput == null)
                {
                    MenuMain(firstMessage: "Incorrect input.");
                    return;
                }

                switch (intInput)
                {
                    case 1:
                        if (IsAuthorized)
                        {
                            Logout();
                        }
                        else
                        {
                            MenuLoginOrRegistration();
                        }

                        break;

                    case 2:
                        GetAndDisplayAddDateRanges();
                        break;

                    case 3:
                        if (!IsAuthorized)
                        {
                            MenuMain(firstMessage: "Incorrect input.");
                            return;
                        }

                        ActionInsertNewDateRange();

                        break;

                    case 4:
                        if (!IsAuthorized)
                        {
                            MenuMain(firstMessage: "Incorrect input.");
                            return;
                        }

                        try
                        {
                            var intersectDateRanges = MenuCheckIntersectDateRange();

                            string result = "";

                            if (intersectDateRanges.Count == 0)
                            {
                                result += "No intersects founded.";
                            }
                            else
                            {
                                result += "Intersect DataRanges:\n";

                                foreach (var element in intersectDateRanges)
                                {
                                    result += element + "\n";
                                }

                                result += "\n";
                            }

                            MenuMain(firstMessage: result);
                            return;
                        }
                        catch (WebException ex)
                        {
                            MenuMain(firstMessage: "Failed. Error Occured. " + ex.Message);
                            return;
                        }

                    default:
                        MenuMain(firstMessage: "Incorrect input.");
                        return;
                }
            }
        }

        private void Logout()
        {
            _token = null;
            _login = "";
        }

        private void MenuLoginOrRegistration(string firstMessage = null)
        {
            while (true)
            {
                Console.Clear();

                if (firstMessage != null)
                {
                    Console.WriteLine(firstMessage);
                    firstMessage = null;
                }

                Console.WriteLine();
                Console.WriteLine("Type number of command:");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Registration");
                Console.WriteLine("3. Back");

                Console.WriteLine();

                int? intInput = ConsoleReadLineInputInteger();
                if (intInput == null)
                {
                    Console.WriteLine("Incorrect input.");
                    continue;
                }

                switch (intInput)
                {
                    case 1:
                        MenuLogin();
                        break;

                    case 2:
                        MenuRegistration();
                        break;

                    case 3:
                        MenuMain();
                        break;

                    default:
                        Console.WriteLine("Incorrect input.");
                        continue;
                }
            }
        }

        private void ActionInsertNewDateRange()
        {
            DateRange dateRange = ConsoleInputDateRange("Insert new DateRange.");

            if (dateRange == null)
            {
                throw new Exception("dateRange == null");
            }

            Console.WriteLine();

            Console.Write("Trying to insert new DateRange to db... ");

            try
            {
                new Requests().TryToInsertNewDateRange(dateRange, _token);
            }
            catch (WebException ex)
            {
                Console.WriteLine("Failed. Error Occured. " + ex.Message);
                return;
            }

            Console.WriteLine("Success. DateRange inserted.");
        }

        private List<DateRange> MenuCheckIntersectDateRange()
        {
            DateRange dateRange = ConsoleInputDateRange("Check DateRange intersection with existed dates.");

            if (dateRange == null)
            {
                throw new Exception("dateRange == null");
            }

            //Console.WriteLine();
            //Console.Write("Trying to get response about DateRange intersection... ");

            List<DateRange> intersectDateRanges = new Requests().GetDateRangeIntersects(dateRange, _token);
            return intersectDateRanges;
        }

        private DateRange ConsoleInputDateRange(string title = null)
        {
            DateRange dateRange;

            while (true)
            {
                dateRange = new DateRange();

                Console.WriteLine();

                if (title != null)
                    Console.WriteLine("Check DateRange intersection with existed dates.");

                Console.Write("Start date: ");
                string startDateStr = Console.ReadLine();

                DateTime startDate;
                if (DateTime.TryParse(startDateStr, out startDate))
                {
                    dateRange.Start = startDate;
                }
                else
                {
                    Console.WriteLine("Incorrect date format.");
                    continue;
                }

                Console.Write("End date: ");
                string endDateStr = Console.ReadLine();

                DateTime endDate;
                if (DateTime.TryParse(endDateStr, out endDate))
                {
                    if (endDate < startDate)
                    {
                        Console.WriteLine("EndDate should be bigger then StartDate.");
                        continue;
                    }
                    else
                    {
                        dateRange.End = endDate;
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect date format.");
                    continue;
                }

                break;
            }

            return dateRange;
        }

        private void MenuLogin(string firstMessage = null)
        {
            while (true)
            {
                Console.Clear();

                if (firstMessage != null)
                {
                    Console.WriteLine(firstMessage);
                    firstMessage = null;
                }

                User user = new User();

                Console.WriteLine();
                Console.Write("Login: ");
                user.Login = Console.ReadLine();
                Console.Write("Password: ");
                user.Password = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Trying to login with entered credentials... ");

                LoginResultPayload loginResultPayload = null;
                try
                {
                    loginResultPayload = new Requests().TryToLogin(user);
                }
                catch (WebException ex)
                {
                    MenuLoginOrRegistration(firstMessage: "Failed. Error Occured. " + ex.Message);
                    return;
                }

                _token = loginResultPayload.access_token;
                _login = loginResultPayload.username;

                MenuMain(firstMessage: "Success.");
            }
        }

        private void MenuRegistration(string firstMessage = null)
        {
            while (true)
            {
                Console.Clear();

                if (firstMessage != null)
                {
                    Console.WriteLine(firstMessage);
                    firstMessage = null;
                }

                User user = new User();

                Console.WriteLine();
                Console.Write("Login: ");
                user.Login = Console.ReadLine();
                Console.Write("Password: ");
                user.Password = Console.ReadLine();
                Console.Write("Repeat password: ");
                string repeatPassword = Console.ReadLine();

                Console.WriteLine();

                if (!String.Equals(repeatPassword, user.Password))
                {
                    Console.WriteLine("Error. Typed passwords doesn't match.");
                    continue;
                }

                Console.Write("Trying to register new user... ");

                GeneralResponsePayload registrationResultPayload = null;
                try
                {
                    registrationResultPayload = new Requests().TryToRegister(user);
                }
                catch (WebException ex)
                {
                    MenuRegistration(firstMessage: "Failed. Error Occured. " + ex.Message);
                    return;
                }

                if (!registrationResultPayload.isSuccess)
                {
                    MenuRegistration(firstMessage: "Failed. Error Occured. " + registrationResultPayload.errorText);
                    return;
                }

                MenuMain(firstMessage: "Success. New user registered.");
            }
        }

        private int? ConsoleReadLineInputInteger()
        {
            string inputStr = Console.ReadLine();
            int intInput;
            if (!int.TryParse(inputStr, out intInput))
            {
                return null;
            }

            return intInput;
        }

        public void GetAndDisplayAddDateRanges()
        {
            List<DateRange> allDatesList;

            try
            {
                allDatesList = new Requests().GetAllDateRanges();
            }
            catch (WebException ex)
            {
                MenuMain(firstMessage: "Error Occured. " + ex.Message);
                return;
            }

            if (allDatesList != null)
            {
                string result = "";

                if (allDatesList.Count == 0)
                {
                    result = "No saved DataRanges in database.";
                }
                else
                {

                    result += "Date Ranges in database:\n";

                    foreach (var element in allDatesList)
                    {
                        result += element + "\n";
                    }

                    result += "\n";
                }

                MenuMain(firstMessage: result);
                return;
            }
        }
    }
}
