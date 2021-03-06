﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using University_students.Models;

namespace University_students.ViewModel
{
    class SupportViewModel : ViewModelBase, INotifyPropertyChanged
    {

        MailAddress from;
        MailAddress to; 
        MailMessage message;
        SmtpClient smtp;
        public SupportViewModel()
        {
            to = new MailAddress(Secrets.EMAIL_TO);
            smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials =  new NetworkCredential(Secrets.EMAIL_TO, Secrets.PASSWORD);
            Message = "Message";
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
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

        private bool Ping()
        {
            WebClient client = new WebClient();
            try
            {
                using (client.OpenRead("http://www.google.com"))
                    return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        public ICommand SupportCommand
        {
            get
            {
                return new RelayCommand(
                    () => CanSupport()
                );
            }
        }

        private void CanSupport()
        {
            if(!new Regex(RegexPattern.emailPattern).IsMatch(Email))
            {
                new CustomBoxes.CustomMessageBox("Email is not validated").Show();
                return;
            }

            if(Message == String.Empty || Message == null)
            {
                new CustomBoxes.CustomMessageBox("Message empty").Show();
                return;
            }

            if (Ping())
            {
                from = new MailAddress(_email);
                message = new MailMessage(from, to);
                message.Subject = Secrets.SUBJECT_EMAIL;
                message.Body = $"<h2>{_message}</h2>";
                message.IsBodyHtml = true;
                smtp.EnableSsl = true;
                smtp.Send(message);
                Email = String.Empty;
                Message = String.Empty;
                new CustomBoxes.CustomMessageBox("Complete").Show();
            }
            else new CustomBoxes.CustomMessageBox("Connection is fail").Show();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
