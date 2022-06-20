using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Timers;

namespace dotnet_course
{
    class Program
    {
        static Timer _timer = new Timer();
        static DateTime _birthdayDate = new DateTime();
        static DateTime _nextBirthday = new DateTime();
        static int _age = 0;

        static void Main(string[] args)
        {
            string personInfo = "";
            string firstName = "";
            string lastName = "";
            string[] parsedInfo;
            string parsedBirthday;

            Match birthdayMatch;
            Regex separator = new Regex(@"\W");
            Regex birthdayRegex = new Regex(@"\d{2}\W\d{2}\W\d{4}");

            DateTime today = DateTime.Now;

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
            DateTime.TryParseExact(parsedBirthday, "dd-MM-yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _birthdayDate);
            _age = GetAge(_birthdayDate);
            _nextBirthday = GetNextBirthDay(_birthdayDate);

            personInfo = birthdayRegex.Replace(personInfo, "");
            parsedInfo = Regex.Split(personInfo, @"\W");
            firstName = parsedInfo[0];
            lastName = parsedInfo[1];

            

            Console.WriteLine($"Имя: { firstName }");
            Console.WriteLine($"Фамилия: { lastName }");
            Console.WriteLine($"Родился: { _birthdayDate.ToLongDateString() }");
            Console.WriteLine($"Кол-во полных лет: { _age }");

            _timer.Elapsed += TimerToNextBirthday;
            _timer.Interval = 1000;
            _timer.Start();
            Console.ReadLine();
        }

        private static void TimerToNextBirthday(object o, ElapsedEventArgs e)
        {
            TimeSpan countdownToNextBirthday = _nextBirthday - e.SignalTime;

            if ((int)countdownToNextBirthday.TotalSeconds == 0)
            {
                Console.WriteLine($"Поздравляем вам исполнилось: { _age + 1 }");
                _timer.Stop();
            }

            Console.WriteLine($"Следующий день рождение через: { countdownToNextBirthday.Days } days { countdownToNextBirthday.Hours } hours { countdownToNextBirthday.Minutes } minutes { countdownToNextBirthday.Seconds } seconds.");
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        private static DateTime GetNextBirthDay(DateTime birthdate)
        {
            DateTime today = DateTime.Now;
            DateTime nextBirthday = birthdate.AddYears(today.Year - birthdate.Year);

            if (nextBirthday < today)
            {
                if (!DateTime.IsLeapYear(nextBirthday.Year + 1))
                    nextBirthday = nextBirthday.AddYears(1);
                else
                    nextBirthday = new DateTime(nextBirthday.Year + 1, birthdate.Month, birthdate.Day);
            }

            return nextBirthday;
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
