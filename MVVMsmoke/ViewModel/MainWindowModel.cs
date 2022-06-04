using MVVMsmoke.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVVMsmoke.ViewModel
{
    internal class MainWindowModel : INotifyPropertyChanged
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

        public event EventHandler EventCloseWindow;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        private BaseCommands loginUser;
        public BaseCommands LoginUser
        {
            get
            {
                return loginUser ??
                    (loginUser = new BaseCommands(obj =>
                    {
                        PasswordBox pb = (PasswordBox)obj;
                        using(DbSettings db = new DbSettings())
                        {
                            var user = db.User.Where(u => u.Login == Login
                                                    && u.Password == pb.Password).FirstOrDefault();
                            if (user != null)
                            {
                                CurrentUser.Id = user.ID;
                                CurrentUser.UserEmail = user.Email;
                                CurrentUser.Login = user.Login;
                                WindowsBuilder.ShowStoreWindow();
                                CloseWindow();
                            }
                            else
                            {
                                MessageBox.Show("Пользователь не найден");
                            }
                        }                    
                    }));
            }
        }

        private BaseCommands changeToRegWindow;
        public BaseCommands ChangeToRegWindow
        {
            get
            {
                return changeToRegWindow ??
                    (changeToRegWindow = new BaseCommands(obj =>
                        {
                            WindowsBuilder.ShowRegWindow();
                            CloseWindow();
                        }));
            }
        }
        public void CloseWindow() => EventCloseWindow?.Invoke(this, EventArgs.Empty);
    }
}
