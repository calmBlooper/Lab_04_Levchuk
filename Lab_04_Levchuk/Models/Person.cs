using System;
using System.Text.RegularExpressions;
using Lab_04_Levchuk.Tools.Exceptions;
namespace Lab_04_Levchuk.Models
{
    class Person
    {
        private String _name, _surname, _email;
        private DateTime _birthDay;
        //Eastern zodiacs helping array, for easier and faster computation of the "ChineseSign" method
        private string[] _chineseZodiacs = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };
        private Person(string name, string surname, string email) : this(name, surname, email, DateTime.Now)
        {
        }

        public Person(string name, string surname, string email, DateTime birthDay)
        {
            _name = name;
            _surname = surname;

            CheckEmail(email);
            _email = email;
            BirthdayCorrect(birthDay);
            _birthDay = birthDay;
        }
        private Person(string name, string surname, DateTime birthDay) : this(name, surname, string.Empty, birthDay)
        {
        }
        private Person()
        {
            _name = "Generic name";
            _surname = "Generic surname";


            _email = "genericmail@mail.com";

            _birthDay = DateTime.Now;
        }
        public String Name
        {
            set => _name = value;
            get => _name;
        }
        public String Surname
        {
            set => _surname = value;
            get => _surname;
        }
        public String Email
        {
            set
            {
                CheckEmail(value);
                _email = value;
            }
            get => _email;
        }
        public DateTime BirthDay
        {
            set
            {
                BirthdayCorrect(value);
                _birthDay = value;
            }
            get => _birthDay.Date;
        }
        public bool IsAdult
        {
            get
            {
                int Age = DateTime.Now.Year - _birthDay.Year;
                if (_birthDay.AddYears(Age) > DateTime.Now) Age--;
                return Age > 18;
            }
        }
        public string SunSign
        {
            get
            {
                int Month = _birthDay.Month, Day = _birthDay.Day;
                switch (Month)
                {
                    case 1:
                        if (Day < 20)
                            return "Capricorn";
                        else
                            return "Aquarius";
                    case 2:
                        if (Day < 19)
                            return "Aquarius";
                        else
                            return "Pisces";
                    case 3:
                        if (Day < 21)
                            return "Pisces";
                        else
                            return "Aries";
                    case 4:
                        if (Day < 20)
                            return "Aries";
                        else
                            return "Taurus";
                    case 5:
                        if (Day < 21)
                            return "Taurus";
                        else
                            return "Gemini";
                    case 6:
                        if (Day < 21)
                            return "Gemini";
                        else
                            return "Cancer";
                    case 7:
                        if (Day < 23)
                            return "Cancer";
                        else
                            return "Leo";
                    case 8:
                        if (Day < 23)
                            return "Leo";
                        else
                            return "Virgo";
                    case 9:
                        if (Day < 23)
                            return "Virgo";
                        else
                            return "Libra";
                    case 10:
                        if (Day < 23)
                            return "Libra";
                        else
                            return "Scorpio";
                    case 11:
                        if (Day < 22)
                            return "Scorpio";
                        else
                            return "Sagittarius";
                    default:
                        if (Day < 22)
                            return "Sagittarius";
                        else
                            return "Capricorn";
                }
            }
        }
        public string ChineseSign
        {
            get
            {
                return _chineseZodiacs[_birthDay.Year % 12];
            }
        }
        public bool IsBirthday
        {
            get
            {
                return DateTime.Now.Month == _birthDay.Month && DateTime.Now.Day == _birthDay.Day;
            }
        }
        private void CheckEmail(string input)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            if (!Regex.IsMatch(input, regex, RegexOptions.IgnoreCase)) throw new WrongEmailException("Wrong email! The correct format is: username@domainname.com");
        }
        private int GetAge(DateTime enteredDate)
        {
            int Age = DateTime.Now.Year - enteredDate.Year;
            if (enteredDate.AddYears(Age) > DateTime.Now) Age--;
            return Age;
        }
        private void BirthdayCorrect(DateTime enteredDate)
        {
            if (enteredDate.Year > DateTime.Now.Year) throw new FutureDateException("User can not be born in the future! Try again.");
            else
            {
                if (enteredDate.Year == DateTime.Now.Year)
                {
                    if (enteredDate.Month > DateTime.Now.Month) throw new FutureDateException("You can not be born in the future! Try again.");
                    else if (enteredDate.Month == DateTime.Now.Month && enteredDate.Day > DateTime.Now.Day) throw new FutureDateException("You can not be born in the future! Try again.");
                }
                else if (GetAge(enteredDate) > 135) throw new ArentYouDeadException("User can not be more than 135 years old! Try again.");
            }
        }
    }
}