﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using University_students.Messager;
using University_students.Models;
using University_students.Serializable;

namespace University_students.ViewModel
{
    class SignUpViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        USDbContext db;

        public SignUpViewModel()
        {
            _isEnabled = true;
            db = new USDbContext();
            ListUniversities = db.Universities.ToList();
        }

        private bool _isActiveMessage;
        public bool IsActiveMessage
        {
            get => _isActiveMessage;
            set
            {
                _isActiveMessage = value;
                OnPropertyChanged("IsActiveMessage");
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private List<University> _listUniversities;
        public List<University> ListUniversities
        {
            get => _listUniversities;
            set
            {
                _listUniversities = value;
                OnPropertyChanged("ListUniversities");
            }
        }

        private University _selectedUniversity;
        public University SelectedUniversity
        {
            get => _selectedUniversity;
            set
            {
                _selectedUniversity = value;
                ListFaculties = value?.Faculties.ToList();
                OnPropertyChanged("SelectedUniversity");
            }
        }

        private List<Faculty> _listFaculties;
        public List<Faculty> ListFaculties
        {
            get => _listFaculties;
            set
            {
                _listFaculties = value;
                OnPropertyChanged("ListFaculties");
            }
        }

        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                _selectedFaculty = value;
                if (value != null)
                    ListGroupsString =  ListGroupSer.ToListGroup((from g in db.Groups
                                            join sp in db.Specialities on g.SpecialityId equals sp.Id
                                            join f in db.Faculties on sp.FacultyId equals f.Id
                                            where f.Id == value.Id
                                            select g).ToList());
                OnPropertyChanged("SelectedFaculty");
            }
        }

        private List<Models.Group> _listGroups;
        public List<Models.Group> ListGroups
        {
            get => _listGroups;
            set => _listGroups = value;
        }

        private List<GroupString> _listGroupsString;
        public List<GroupString> ListGroupsString
        {
            get => _listGroupsString;
            set
            {
                _listGroupsString = value;
                OnPropertyChanged("ListGroupsString");
            }
        }

        private GroupString _selectedGroup;
        public GroupString SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                IsEnabled = CheckField();
                OnPropertyChanged("SelectedGroup");
            }
        }


        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        private string _confirmed_password;
        public string ConfirmedPassword
        {
            get { return _confirmed_password; }
            set
            {
                _confirmed_password = value;
                OnPropertyChanged("ConfirmedPassword");
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public ICommand SignUpCommand
        {
            get
            {
                return new RelayCommand(
                    () => CanSignUp()
                );
            }
        }

        public ICommand UndoCommand
        {
            get
            {
                return new RelayCommand(
                    () => CanUndoMessage()
                );
            }
        }

        private string _error;
        public string Error
        {
            get => _error;
            set
            {
                if (value != null)  IsEnabled = false;
                _error = value;
            }
        }

        public string this[string columnName] {
            get
            {
                Regex regexLogin = new Regex(RegexPattern.loginPattern);
                Regex regexPassword = new Regex(RegexPattern.passwordPattern);
                Regex regexFirstName = new Regex(RegexPattern.firstNamePattern);
                Regex regexLastName = new Regex(RegexPattern.lastNamePattern);
                Error= null;
                if (Login == null)
                {
                    IsEnabled = false;
                    return null;
                }
                IsEnabled = CheckField();
                switch (columnName)
                {
                    case "Login":
                        if (!regexLogin.IsMatch(Login))
                        {
                            Error = "Login is not validated";
                        }
                        break;
                    case "Password":
                        if (!regexPassword.IsMatch(Password) || Password.Length < 6)
                        {
                            Error = "Password is not validated";
                        }
                        break;
                    case "ConfirmedPassword":
                        if(Password != ConfirmedPassword)
                        {
                            Error = "Confirmed Password is not equal to Password";
                        }
                        break;
                    case "FirstName":
                        if (!regexFirstName.IsMatch(FirstName))
                        {
                            Error = "First Name is not validated";
                        }
                        break;
                    case "LastName":
                        if (!regexLastName.IsMatch(LastName))
                        {
                            Error = "Last Name is not validated";
                        }
                        break;
                }
                return Error;
            }
        }

        private bool CheckField()
        {
            bool result = true;
            if (String.IsNullOrEmpty(Login)) result = false;
            if (String.IsNullOrEmpty(Password)) result = false;
            if (String.IsNullOrEmpty(ConfirmedPassword)) result = false;
            if (String.IsNullOrEmpty(FirstName)) result = false;
            if (String.IsNullOrEmpty(LastName)) result = false;
            if (SelectedGroup == null) result = false;
            //return true; // if need admin;
            return result;
        }

        private void CanUndoMessage()
        {
            IsActiveMessage = false;
            Message = String.Empty;
        }

        private void CanSignUp()
        {
            if(ConfirmedPassword != Password )
            {
                return;
            }

            if(db.Users.FirstOrDefault(user => user.Login == Login) != null)
            {
                Message = "User already exist";
                IsActiveMessage = true;
                return;
            }
            //-------------------------------------------------------------//
            //Add Admin
            //User newUser = new User()
            //{
            //    Login = "admin",
            //    FirstName = "admin",
            //    LastName = "admin",
            //    TypeUser = Enums.Role.Admin,
            //    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
            //};
            ////-------------------------------------------------------------//
            User newUser = new User()
            {
                Login = _login,
                FirstName = _firstName,
                LastName = _lastName,
                TypeUser = Enums.Role.Students,
                Password = BCrypt.Net.BCrypt.HashPassword(_password),
                Group = SelectedGroup.Group
            };
            // ---------------------------------------------------------------------------- //
            db.Users.Add(newUser);
            db.SaveChanges();
            // comment if need admin
            foreach (TaughtGroups tg in SelectedGroup.Group.TaughtGroups)
            {
                var newSP = new SubjectProgress()
                {
                    User = newUser,
                    UnValidExcuses = 0,
                    ValidExcuses = 0,
                    TaughtGroups = tg,
                    IsExamPassed = Enums.StateExam.Waiting,
                    IsStartCertifiationPassed = Enums.StateCertification.Waiting,
                    IsFinishCertifiationPassed = Enums.StateCertification.Waiting,
                };
                db.SubjectProgress.Add(newSP);
            }
            db.SaveChanges();
            // ---------------------------------------------------------------------------- //
            Login = String.Empty;
            FirstName = String.Empty;
            LastName = String.Empty;
            Password = String.Empty;
            ConfirmedPassword = String.Empty;
            GoToUserPage(newUser);
        }

        private object GoToUserPage(User user)
        {
            var msg = new ChangeNavigationPageMessage() { CurrentUser = user };
            Messenger.Default.Send<ChangeNavigationPageMessage>(msg);
            return null;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
