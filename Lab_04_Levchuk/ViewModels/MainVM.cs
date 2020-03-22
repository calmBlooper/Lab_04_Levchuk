using Lab_04_Levchuk.Models;
using Lab_04_Levchuk.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lab_04_Levchuk.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {

        private Person _userObject;
        private string _name = "", _surname = "", _email = "";
        private DateTime? _chosenDate;
        private int _currentIndex=-1;
        private String _mode = "Add";
        private List<Person> _usersList=new List<Person>();
        public MainVM()
        {
          
         //   FileInfo[] TXTFiles = di.GetFiles("*.xml");
         //   DirectoryInfo[] kek = di.GetDirectories("LabSaves");
          //  if (kek.Length == 0)
          //  {
            //    MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/LabSaves");
            // };
       
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "/LabSaves");
            FileInfo[] saves = di.GetFiles("*.json");
        
            if (saves.Length==0)
            {
                for (int i=0;i<50;i++)
                {
                    DateTime help = Convert.ToDateTime("05/05/2000");
             
                        _usersList.Add(new Person("Corona"+i, "Beer"+i, "corona@beer.com", help.Date));
              
                    
                }
               // MessageBox.Show(_usersList[5].Surname);
            }
            OnPropertyChanged("UsersList");
            //MessageBox.Show("lol");
            ProceedButtonCommand = new RelayCommand(o => MainButtonClick("MainButton"));
            EditButtonCommand = new RelayCommand(o => EditButtonClick("SecondaryButton"));
            DeleteButtonCommand = new RelayCommand(o => DeleteButtonClick("SecondaryButton"));
            AddButtonCommand = new RelayCommand(o => AddButtonClick("SecondaryButton"));
            UserInfo = "";
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
            get =>  _currentIndex;
        }
        public Person SelectedUser
        {
            set
            {
              //  MessageBox.Show(""+CurrentIndex+"\n"+_usersList[CurrentIndex].Surname);
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
            get => UserInfo != "" && LoaderVisibility == Visibility.Collapsed&&!CanUseSuite;
        }
        public bool ButtonEnabled
        {
            get =>  CanUseSuite&&LoaderVisibility == Visibility.Collapsed && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname) && !string.IsNullOrWhiteSpace(Email) && ChosenDate.HasValue;
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
        public string UserInfo { set; get; } = "";
        public Visibility LoaderVisibility { set; get; } = Visibility.Collapsed;
        public ICommand ProceedButtonCommand { get; set; }
        public ICommand EditButtonCommand { get; set; }
        public ICommand DeleteButtonCommand { get; set; }
        public ICommand AddButtonCommand { get; set; }
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
            MessageBox.Show("PeePeePooPoo");
        }
        private void DeleteButtonClick(object sender)
        {
            UserInfo = "";
            CheckInterface();
            OnPropertyChanged("UserInfo");
            OnPropertyChanged("CanEditOrDelete");
            MessageBox.Show("PeePeePooPoo");
        }
        private void AddButtonClick(object sender)
        {
            _mode = "Add";
            CanUseSuite = true;
            OnPropertyChanged("AddButtonEnabled");
            UserInfo = "";
            OnPropertyChanged("UserInfo");
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
           
        }
        private void ShowLoader()
        {
            LoaderVisibility = Visibility.Visible;
            OnPropertyChanged("LoaderVisibility");
            CheckInterface();
        }
        private void HideLoader()
        {
            LoaderVisibility = Visibility.Collapsed;
            OnPropertyChanged("LoaderVisibility");
        }
    }
}