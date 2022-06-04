using MVVMsmoke.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MVVMsmoke.Model;
using System.Windows;

namespace MVVMsmoke.ViewModel
{
    class RegistrationWindowModel : INotifyPropertyChanged
    {
        private string login;
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get 
            { 
                return email; 
            }
            set 
            { 
                email = value; OnPropertyChanged(); 
            }
        }

        public event EventHandler EventCloseWindow;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string  prop = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private BaseCommands changeToMainWindow;
        public BaseCommands ChangeToMainWindow
        {
            get
            {
                return changeToMainWindow ??
                    (changeToMainWindow = new BaseCommands(obj =>
                    {
                        WindowsBuilder.ShowMainWindow();
                        CloseWindow();
                    }));
            }
        }
        private BaseCommands regIn;

        public RegistrationWindowModel()
        {
        }

        public BaseCommands RegIn
        {
            get
            {
                return regIn ??
                    (regIn = new BaseCommands(obj =>
                    {
                        using (DbSettings db = new DbSettings())
                        {
                            PasswordBox pb = (PasswordBox)obj;
                            string password = pb.Password;
                            User user = db.User.Where(u => u.Login == this.Login).FirstOrDefault();
                            if (user == null && FieldCheker.CheckLogin(login) && FieldCheker.CheckPassword(pb.Password)/* && FieldCheker.CheckEmail()*/) //добавить емейл!!!
                            {
                                if (password != null)
                                {
                                    int maxID = db.User.Max(u => u.ID);
                                    User newUser = new User(maxID + 1, login, email, password);
                                    db.User.Add(newUser);
                                    db.SaveChanges();
                                    MessageBox.Show("Пользователь добавлен");
                                    CurrentUser.Id = newUser.ID;
                                    CurrentUser.Login = newUser.Login;
                                    CurrentUser.UserEmail = newUser.Email;
                                    WindowsBuilder.ShowStoreWindow();
                                    CloseWindow();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Ошибка: Пользователь существует или ошибка заполнения");
                            }           
                        }
                    }));
            }
        }
        public void CloseWindow() => EventCloseWindow?.Invoke(this, EventArgs.Empty);
    }
}
