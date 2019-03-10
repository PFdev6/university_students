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
            from = new MailAddress(_email);
            message = new MailMessage(from, to);
            // тема письма
            message.Subject = Secrets.SUBJECT_EMAIL;
            // текст письма
            message.Body = $"<h2>{_message}</h2>";
            // письмо представляет код html
            message.IsBodyHtml = true;
            smtp.EnableSsl = true;
            smtp.Send(message);
            Email = String.Empty;
            Message = String.Empty;
            MessageBox.Show("Complete");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}