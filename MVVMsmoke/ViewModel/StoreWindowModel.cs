using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using MVVMsmoke.Commands;
using Microsoft.Win32;
using MVVMsmoke.Model;

namespace MVVMsmoke.ViewModel
{
    class StoreWindowModel : INotifyPropertyChanged 
    {
        public event EventHandler EventCloseWindow;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        BitmapImage avatar;
        public BitmapImage Avatar
        {
            get { return avatar; }
            set 
            { 
                avatar = value;
                OnPropertyChanged();
            } 
        }
        private BaseCommands loadAvatar;
        public BaseCommands LoadAvatar
        {
            get
            {//?? - оператор объединения с NULL
                return loadAvatar ??
                    (loadAvatar = new BaseCommands(obj =>
                    {
                        OpenFileDialog opf = new OpenFileDialog();
                        opf.Filter = "Images (*.jpg)|*.jpg";
                        opf.ShowDialog();
                        if(opf.FileName != "")
                        {
                            BitmapImage image = new BitmapImage(new Uri(opf.FileName));
                            using(DbSettings db = new DbSettings())
                            {
                                foreach(var u in db.User)
                                {
                                    u.Avatar = DataTransform.JpgToByte(image);
                                    Avatar = image;
                                    break;
                                }
                                db.SaveChanges();
                            }
                        }
                    }));
            }
        }
        public StoreWindowModel()
        {
            UserName = CurrentUser.Login;
            using(DbSettings db = new DbSettings())
            {
                User currentUser = db.User.FirstOrDefault(u => u.ID == CurrentUser.Id);
                if(currentUser.Avatar.Length == 0)
                {
                    var image = new BitmapImage();
                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string folderPath = System.IO.Path.GetDirectoryName(exePath);
                    image.BeginInit();
                    image.UriSource = new Uri(folderPath + @"\Images\defolt.jpg");
                    image.EndInit();
                    Avatar = image;
                }
                else
                {
                    Avatar = DataTransform.ByteToImage(currentUser.Avatar);
                }
            }
        }
        private BaseCommands logOut;
        public BaseCommands LogOut
        {
            get 
            { 
                return logOut ??
                    (logOut = new BaseCommands(obj =>
                    {
                        CurrentUser.Login = "";
                        CurrentUser.Id = 0;
                        CurrentUser.UserEmail = "";
                        WindowsBuilder.ShowMainWindow();
                        CloseWindow();
                    })); 
            }
        }
        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }
        public void CloseWindow() => EventCloseWindow?.Invoke(this, EventArgs.Empty);

    }
}
