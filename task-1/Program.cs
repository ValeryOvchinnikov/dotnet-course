using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace dotnet_course
{
    class Program
    {
        private static Timer _timer = null;

        static void Main(string[] args)
        {
            string personInfo = "";
            string firstName = "";
            string lastName = "";
            string[] parsedInfo;
            string parsedBirthday;

            Match birthdayMatch;
            Regex separator = new Regex(@"\W");
            DateTime birthdayDate = new DateTime();
            Regex birthdayRegex = new Regex(@"\d{2}\W\d{2}\W\d{4}");

            Console.WriteLine("Введите имя, фамилию, дату и год рождения(в формате dd-mm-yyyy).");
            personInfo = Console.ReadLine();

            while (true)
            {
                if (personInfo.Length == 0 || personInfo.Length > 39)
                {
                    Console.WriteLine("Пожалуйста, проверьте корректность вводимых данных");
                    personInfo = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            birthdayMatch = birthdayRegex.Match(personInfo);
            parsedBirthday = separator.Replace(birthdayMatch.Value, "-");
            DateTime.TryParseExact(parsedBirthday, "dd-MM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out birthdayDate);
            personInfo = birthdayRegex.Replace(personInfo, "");
            parsedInfo = Regex.Split(personInfo, @"\W");
            firstName = parsedInfo[0];
            lastName = parsedInfo[1];

            Console.WriteLine($"Имя: {firstName}");
            Console.WriteLine($"Фамилия: {lastName}");
            Console.WriteLine($"Родился: {birthdayDate.ToLongDateString()}");
            Console.WriteLine($"Кол-во полных лет: {GetAge(birthdayDate)}");
            _timer = new Timer(TimerToNextBirthday, birthdayDate, 0, 1000);
            Console.ReadLine();
        }

        private static void TimerToNextBirthday(Object birthday)
        {
            DateTime parsedBirthday = (DateTime)birthday;
            DateTime today = DateTime.Now;
            DateTime nextBirthday = parsedBirthday.AddYears(today.Year - parsedBirthday.Year);

            if (nextBirthday < today)
            {
                if (!DateTime.IsLeapYear(nextBirthday.Year + 1))
                    nextBirthday = nextBirthday.AddYears(1);
                else
                    nextBirthday = new DateTime(nextBirthday.Year + 1, parsedBirthday.Month, parsedBirthday.Day);
            }

            TimeSpan countdownToNextBirthday = nextBirthday - today;

            if (parsedBirthday.Day == today.Day && parsedBirthday.Month == today.Month)
            {
                Console.WriteLine($"Поздравляем вам исполнилось: {GetAge(parsedBirthday)}");
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            else
            {
                Console.WriteLine("Следующий день рождение через: " + countdownToNextBirthday.Days + " days " + countdownToNextBirthday.Hours + " hours " + countdownToNextBirthday.Minutes + " minutes " + countdownToNextBirthday.Seconds + " seconds.");
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }

        private static int GetAge(DateTime birthdate)
        {
            DateTime today = DateTime.Today;

            int age = today.Year - birthdate.Year;

            if (birthdate.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}
