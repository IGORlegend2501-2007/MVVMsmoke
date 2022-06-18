using MVVMsmoke.CustomControls;
using MVVMsmoke.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MVVMsmoke.Views
{
    /// <summary>
    /// Логика взаимодействия для StoreWindow.xaml
    /// </summary>
    public partial class StoreWindow : Window
    {
        public StoreWindow()
        {
            InitializeComponent();
            StoreBorder storeBorder = new StoreBorder(this);
            MainGrid.SetValue(Grid.RowProperty, 0);
            MainGrid.Children.Add(storeBorder);
            FillGrid();
            InitTreeView();
        }
        private void InitTreeView()
        {
            using (DbSettings db = new DbSettings())
            {
                User currentUser = db.User.FirstOrDefault(user => user.ID == CurrentUser.Id);
                foreach (Game game in currentUser.Games)
                {
                    var item = new TreeViewItem();
                    item.Header = game.Name;
                    tv_userGames.Items.Add(item);
                }
            }
        }
        void FillGrid()
        {
            using(DbSettings dbSettings = new DbSettings())
            {
                List<RowDefinition> rows = new List<RowDefinition>();
                int gamesCount = dbSettings.Game.Count();
                int gamesRowsCount = (gamesCount + 1) / 2;
                for(int i = 0; i < gamesRowsCount; i++)
                {
                    rows.Add(new RowDefinition());
                    rows[i * 2].Height = new GridLength(30);
                    rows.Add(new RowDefinition());
                    rows[i*2 + 1].Height = new GridLength(130, GridUnitType.Star);//не влияет
                }
                rows.Add(new RowDefinition());
                rows[rows.Count - 1].Height = new GridLength(30);

                for(int i = 0; i < rows.Count-1; i++)
                {
                    GameField.RowDefinitions.Add(rows[i]);
                }

                int columnNum = 1,
                    rowNum = 1,
                    index = 0;

                foreach (Game game in dbSettings.Game)
                {
                    StackPanel sp = new StackPanel();
                    sp.SetValue(Grid.RowProperty, rowNum);
                    sp.SetValue(Grid.ColumnProperty, columnNum);

                    Label price = new Label();
                    price.HorizontalAlignment = HorizontalAlignment.Center;
                    price.Content = game.Price + "$";

                    Label name = new Label();
                    name.HorizontalAlignment = HorizontalAlignment.Center;
                    name.Content = game.Name;

                    Image currentImage = new Image();
                    BitmapImage logo = DataTransform.ByteToImage(game.Image);
                    currentImage.Source = logo;
                    columnNum = (columnNum == 1) ? 3 : 1;

                    sp.Children.Add(currentImage);
                    sp.Children.Add(name);
                    sp.Children.Add(price);
                    GameField.Children.Add(sp);

                    if ((index + 1) % 2 == 0)
                    {
                        rowNum += 2;
                    }
                    index++;
                }
            }
        }
    }
}
