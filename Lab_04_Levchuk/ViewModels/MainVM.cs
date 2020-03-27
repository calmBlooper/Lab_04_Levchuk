using Lab_04_Levchuk.Models;
using Lab_04_Levchuk.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lab_04_Levchuk.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {

        private Person _userObject;
        private string _name = "", _surname = "", _email = "";
        private DateTime? _chosenDate;
        private int _currentIndex = -1;
        private String _mode = "add";
        private List<Person> _usersList = new List<Person>(), _backup;
        private bool _nameS = true, _surnameS = true, _emailS = true, _dateS = true, _birthdayS = true, _chineseSignS = true, _sunSignS = true, _adultS = true;
        public MainVM()
        {

            System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/LabSaves");


            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/LabSaves");
            FileInfo[] saves = di.GetFiles("*.json");

            if (saves.Length == 0)
            {
                for (int i = 0; i < 50; i++)
                {
                    DateTime help = Convert.ToDateTime("05/05/2000");

                    _usersList.Add(new Person("Corona" + i, "Beer" + i, "corona@beer.com", help.Date));


                }
            }
            else
            {
                string jsonString = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/LabSaves/Users.json");
                _usersList = JsonSerializer.Deserialize<List<Person>>(jsonString);


            }
            OnPropertyChanged("UsersList");
            ProceedButtonCommand = new RelayCommand(o => MainButtonClick("MainButton"));
            EditButtonCommand = new RelayCommand(o => EditButtonClick("SecondaryButton"));
            DeleteButtonCommand = new RelayCommand(o => DeleteButtonClick("SecondaryButton"));
            AddButtonCommand = new RelayCommand(o => AddButtonClick("SecondaryButton"));
            SaveButtonCommand = new RelayCommand(o => SaveButtonClick("SecondaryButton"));
            NameSortCommand = new RelayCommand(o => NameButtonClick("SecondaryButton"));
            SurnameSortCommand = new RelayCommand(o => SurnameButtonClick("SecondaryButton"));
            EmailSortCommand = new RelayCommand(o => EmailButtonClick("SecondaryButton"));
            DateSortCommand = new RelayCommand(o => DateButtonClick("SecondaryButton"));
            ChineseSignSortCommand = new RelayCommand(o => ChineseSignButtonClick("SecondaryButton"));
            SunSignSortCommand = new RelayCommand(o => SunSignButtonClick("SecondaryButton"));
            BirthdaySortCommand = new RelayCommand(o => BirthdayButtonClick("SecondaryButton"));
            AdultSortCommand = new RelayCommand(o => AdultButtonClick("SecondaryButton"));
            FilterButtonCommand = new RelayCommand(o => FilterButtonClick("SecondaryButton"));
            ResetFilterCommand = new RelayCommand(o => ResetFilterButtonClick("SecondaryButton"));
            UserInfo = "";
            _backup = new List<Person>(_usersList);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool CanUseSuite = false;
        public List<Person> UsersList
        {
            set => _usersList = value;
            get => _usersList;
        }
        public int CurrentIndex
        {
            set
            {
                _currentIndex = value;
            }
            get => _currentIndex;
        }
        public Person SelectedUser
        {
            set
            {

                _userObject = value;
                UserInfo = "Name: " + _userObject.Name +
              "\nSurname: " + _userObject.Surname +
              "\nEmail: " + _userObject.Email +
              "\nDate of birth: " + _userObject.BirthDay.ToShortDateString() +
              "\nIs adult: " + (_userObject.IsAdult ? "Yes" : "No") +
              "\nSun sign: " + _userObject.SunSign +
              "\nChinese sign: " + _userObject.ChineseSign +
              "\nIs birthday today: " + (_userObject.IsBirthday ? "Yes" : "No");
                CanUseSuite = false;
                OnPropertyChanged("UserInfo");
                OnPropertyChanged("CanEditOrDelete");
            }
            get => _userObject;
        }
        public string Name
        {
            set
            {
                _name = value;
                OnPropertyChanged("ButtonEnabled");
            }
            get => _name;
        }
        public string Surname
        {
            set
            {
                _surname = value;
                OnPropertyChanged("ButtonEnabled");
            }
            get => _surname;
        }
        public string Email
        {
            set
            {
                _email = value;
                OnPropertyChanged("ButtonEnabled");
            }
            get => _email;
        }
        public DateTime? ChosenDate
        {
            set
            {
                _chosenDate = value;
                OnPropertyChanged("ButtonEnabled");
            }
            get => _chosenDate;
        }
        public bool CanEditOrDelete
        {
            get => UserInfo != "" && LoaderVisibility == Visibility.Collapsed && !CanUseSuite;
        }
        public bool ButtonEnabled
        {
            get => CanUseSuite && LoaderVisibility == Visibility.Collapsed && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname) && !string.IsNullOrWhiteSpace(Email) && ChosenDate.HasValue;
        }
        public bool AddButtonEnabled
        {
            get => !CanUseSuite;
        }
        public bool NameEnabled
        {
            get => CanUseSuite && LoaderVisibility == Visibility.Collapsed;
        }
        public bool SurnameEnabled
        {
            get => CanUseSuite && LoaderVisibility == Visibility.Collapsed;
        }
        public bool EmailEnabled
        {
            get => CanUseSuite && LoaderVisibility == Visibility.Collapsed;
        }
        public bool DateEnabled
        {
            get => CanUseSuite && LoaderVisibility == Visibility.Collapsed;
        }
        public string NameF { set; get; } = "";
        public string SurnameF { set; get; } = "";
        public string EmailF { set; get; } = "";
        public DateTime DateF { set; get; } = DateTime.Now.AddDays(1).Date;
        public bool? AdultF { set; get; } = null;
        public string SunSignF { set; get; } = "";
        public string ChineseSignF { set; get; } = "";
        public bool? BirthdayF { set; get; } = null;
        public bool WindowEnabled { get; set; } = true;
        public string UserInfo { set; get; } = "";
        public Visibility LoaderVisibility { set; get; } = Visibility.Collapsed;
        public ICommand ProceedButtonCommand { get; set; }
        public ICommand EditButtonCommand { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand AddButtonCommand { get; set; }
        public ICommand SaveButtonCommand { get; set; }
        public ICommand NameSortCommand { get; set; }
        public ICommand SurnameSortCommand { get; set; }
        public ICommand EmailSortCommand { get; set; }
        public ICommand DateSortCommand { get; set; }
        public ICommand ChineseSignSortCommand { get; set; }
        public ICommand SunSignSortCommand { get; set; }
        public ICommand BirthdaySortCommand { get; set; }
        public ICommand AdultSortCommand { get; set; }
        public ICommand FilterButtonCommand { get; set; }
        public ICommand ResetFilterCommand { get; set; }
        protected virtual void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }



        private void CheckInterface()
        {
            OnPropertyChanged("NameEnabled");
            OnPropertyChanged("SurnameEnabled");
            OnPropertyChanged("EmailEnabled");
            OnPropertyChanged("DateEnabled");
            OnPropertyChanged("ButtonEnabled");
            OnPropertyChanged("CanEditOrDelete");
        }
        private void EditButtonClick(object sender)
        {
            _mode = "edit";
            CanUseSuite = true;
            OnPropertyChanged("AddButtonEnabled");
            CheckInterface();
        }
        private async void DeleteButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                UserInfo = "";
                List<Person> kek = new List<Person>();
                for (int i = 0; i < _usersList.Count; i++) if (i != CurrentIndex) kek.Add(new Person(_usersList[i].Name, _usersList[i].Surname, _usersList[i].Email, _usersList[i].BirthDay));
                _currentIndex = -1;
                _usersList = new List<Person>(kek);
                OnPropertyChanged("UsersList");
                CheckInterface();
                OnPropertyChanged("UserInfo");
                _backup = new List<Person>(_usersList);
                OnPropertyChanged("CanEditOrDelete");
            });
            HideLoader();
        }
        private async void SaveButtonClick(object sender)
        {

            using (FileStream fs = File.Create(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/LabSaves/Users.json"))
            {
                await JsonSerializer.SerializeAsync(fs, _usersList);
            }

        }
        private void AddButtonClick(object sender)
        {
            _mode = "add";
            CanUseSuite = true;
            OnPropertyChanged("AddButtonEnabled");
            UserInfo = "";
            OnPropertyChanged("UserInfo");
            _backup = new List<Person>(_usersList);
            CheckInterface();
        }
        private async void MainButtonClick(object sender)
        {

            ShowLoader();
            await Task.Run(() =>
            {

                try
                {
                    if (_userObject == null) _userObject = new Person(Name, Surname, Email, ChosenDate.Value);
                    else
                    {
                        _userObject.Name = Name;
                        _userObject.Surname = Surname;
                        _userObject.Email = Email;
                        _userObject.BirthDay = ChosenDate.Value;
                    }

                    UserInfo = "Name: " + _userObject.Name +
                    "\nSurname: " + _userObject.Surname +
                    "\nEmail: " + _userObject.Email +
                    "\nDate of birth: " + _userObject.BirthDay.ToShortDateString() +
                    "\nIs adult: " + (_userObject.IsAdult ? "Yes" : "No") +
                    "\nSun sign: " + _userObject.SunSign +
                    "\nChinese sign: " + _userObject.ChineseSign +
                    "\nIs birthday today: " + (_userObject.IsBirthday ? "Yes" : "No");
                    CanUseSuite = false;
                    OnPropertyChanged("UserInfo");
                    if (_mode == "add")
                    {
                        Person buffer = new Person(Name, Surname, Email, ChosenDate.Value);
                        List<Person> kek = new List<Person>();
                        foreach (Person h in _usersList) kek.Add(new Person(h.Name, h.Surname, h.Email, h.BirthDay));
                        kek.Add(buffer);
                        _usersList = new List<Person>(kek);
                        OnPropertyChanged("UsersList");
                    }
                    else if (_mode == "edit")
                    {
                        Person buffer = new Person(Name, Surname, Email, ChosenDate.Value);
                        List<Person> kek = new List<Person>();
                        foreach (Person h in _usersList) kek.Add(new Person(h.Name, h.Surname, h.Email, h.BirthDay));
                        kek[CurrentIndex] = buffer;
                        _usersList = new List<Person>(kek);
                        OnPropertyChanged("UsersList");
                    }

                    HideLoader();
                    if (_userObject.IsBirthday) MessageBox.Show("He, it`s your birthday today! Congratulations!");
                    CheckInterface();
                    OnPropertyChanged("AddButtonEnabled");

                }
                catch (Exception ex)
                {
                    HideLoader();
                    UserInfo = "";
                    OnPropertyChanged("UserInfo");

                    MessageBox.Show(ex.Message);
                    CheckInterface();
                }
            });
            _backup = new List<Person>(_usersList);
        }
        private async void NameButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                List<Person> SortedList;
                if (_nameS) SortedList = _usersList.OrderBy(o => o.Name).ToList();
                else SortedList = _usersList.OrderByDescending(o => o.Name).ToList();
                _nameS = !_nameS;
                _usersList = new List<Person>(SortedList);
                OnPropertyChanged("UsersList");
            });
            HideLoader();
        }
        private async void SurnameButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                List<Person> SortedList;
                if (_surnameS) SortedList = _usersList.OrderBy(o => o.Surname).ToList();
                else SortedList = _usersList.OrderByDescending(o => o.Surname).ToList();
                _surnameS = !_surnameS;
                _usersList = new List<Person>(SortedList);
                OnPropertyChanged("UsersList");
            });
            HideLoader();
        }
        private async void EmailButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                List<Person> SortedList;
                if (_emailS) SortedList = _usersList.OrderBy(o => o.Email).ToList();
                else SortedList = _usersList.OrderByDescending(o => o.Email).ToList();
                _emailS = !_emailS;
                _usersList = new List<Person>(SortedList);
                OnPropertyChanged("UsersList");
            });
            HideLoader();
        }
        private async void DateButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                List<Person> SortedList;
                if (_dateS) SortedList = _usersList.OrderBy(o => o.BirthDay).ToList();
                else SortedList = _usersList.OrderByDescending(o => o.BirthDay).ToList();
                _dateS = !_dateS;
                _usersList = new List<Person>(SortedList);
                OnPropertyChanged("UsersList");
            });
            HideLoader();
        }
        private async void ChineseSignButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                List<Person> SortedList;
                if (_chineseSignS) SortedList = _usersList.OrderBy(o => o.ChineseSign).ToList();
                else SortedList = _usersList.OrderByDescending(o => o.ChineseSign).ToList();
                _chineseSignS = !_chineseSignS;
                _usersList = new List<Person>(SortedList);
                OnPropertyChanged("UsersList");
            });
            HideLoader();
        }
        private async void SunSignButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                List<Person> SortedList;
                if (_sunSignS) SortedList = _usersList.OrderBy(o => o.SunSign).ToList();
                else SortedList = _usersList.OrderByDescending(o => o.SunSign).ToList();
                _sunSignS = !_sunSignS;
                _usersList = new List<Person>(SortedList);
                OnPropertyChanged("UsersList");
            });
            HideLoader();
        }
        private async void BirthdayButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                List<Person> SortedList;
                if (_birthdayS) SortedList = _usersList.OrderBy(o => o.IsBirthday).ToList();
                else SortedList = _usersList.OrderByDescending(o => o.IsBirthday).ToList();
                _birthdayS = !_birthdayS;
                _usersList = new List<Person>(SortedList);
                OnPropertyChanged("UsersList");
            });
            HideLoader();
        }
        private async void AdultButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                List<Person> SortedList;
                if (_adultS) SortedList = _usersList.OrderBy(o => o.IsAdult).ToList();
                else SortedList = _usersList.OrderByDescending(o => o.IsAdult).ToList();
                _adultS = !_adultS;
                _usersList = new List<Person>(SortedList);
                OnPropertyChanged("UsersList");
            });
            HideLoader();
        }
        private async void FilterButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                if (NameF != "") _usersList = new List<Person>(_usersList.Where(o => o.Name == NameF).ToList());

                if (SurnameF != "") _usersList = new List<Person>(_usersList.Where(o => o.Surname == SurnameF).ToList());

                if (EmailF != "") _usersList = new List<Person>(_usersList.Where(o => o.Email == EmailF).ToList());

                if (DateF.Date != DateTime.Now.AddDays(1).Date) _usersList = new List<Person>(_usersList.Where(o => o.BirthDay == DateF).ToList());

                if (AdultF != null) _usersList = new List<Person>(_usersList.Where(o => o.IsAdult == AdultF).ToList());

                if (ChineseSignF != "") _usersList = new List<Person>(_usersList.Where(o => o.ChineseSign == ChineseSignF).ToList());

                if (SunSignF != "") _usersList = new List<Person>(_usersList.Where(o => o.SunSign == SunSignF).ToList());

                if (BirthdayF != null) _usersList = new List<Person>(_usersList.Where(o => o.IsBirthday == BirthdayF).ToList());


                OnPropertyChanged("UsersList");
            });
            HideLoader();

        }
        private async void ResetFilterButtonClick(object sender)
        {
            ShowLoader();
            await Task.Run(() =>
            {
                NameF = "";
                SurnameF = "";
                EmailF = "";
                SunSignF = "";
                ChineseSignF = "";
                DateF = DateTime.Now.AddDays(1).Date;
                AdultF = null;
                BirthdayF = null;
                _usersList = new List<Person>(_backup);
                OnPropertyChanged("UsersList");
                OnPropertyChanged("NameF");
                OnPropertyChanged("SurnameF");
                OnPropertyChanged("EmailF");
                OnPropertyChanged("SunSignF");
                OnPropertyChanged("ChineseSignF");
                OnPropertyChanged("DateF");
                OnPropertyChanged("AdultF");
                OnPropertyChanged("BirthdayF");
            });
            HideLoader();
        }
        private void ShowLoader()
        {
            LoaderVisibility = Visibility.Visible;
            OnPropertyChanged("LoaderVisibility");
            CheckInterface();
            WindowEnabled = false;
            OnPropertyChanged("WindowEnabled");
        }
        private void HideLoader()
        {
            LoaderVisibility = Visibility.Collapsed;
            OnPropertyChanged("LoaderVisibility");
            WindowEnabled = true;
            OnPropertyChanged("WindowEnabled");
        }
    }
}