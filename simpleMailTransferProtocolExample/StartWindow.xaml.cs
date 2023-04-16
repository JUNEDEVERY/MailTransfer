using MaterialDesignThemes.Wpf;
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
using System.Windows.Media.Imaging;
using System.Timers;
using System.Windows.Threading;
using System.IO;
using System.Windows.Media.Animation;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Diagnostics;

namespace simpleMailTransferProtocolExample
{
    /// <summary>
    /// Логика взаимодействия для StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window

    {
        #region Объявление глобальных переменных
        private readonly List<string> _imagePaths = new List<string>(); // список путей к изображениям
        private int _currentIndex = 0; // текущий индекс изображения
        #endregion
        #region Главный метод

        public StartWindow()
        {
            InitializeComponent();
            string path = Environment.CurrentDirectory + @"\Images\imgonline-com-ua-Resize-I0MMIfYSSTpL.png";
            string path1 = Environment.CurrentDirectory + @"\Images\imgonline-com-ua-Resize-P4SUh3SF9mOBzvA.png";
            _imagePaths.Add(path);

            _imagePaths.Add(path1);
            //_imagePaths.Add("../Images/2.png"); 
            //_imagePaths.Add("../Images/3.jpg");
            //_imagePaths.Add("../Images/4.png");

            var notify = new ToastContentBuilder();
            notify.AddHeader("6289", "Приложение активно", "action=openConversation&id=6289");
            notify.AddText("Приятного использования");
          
         
            // notify.AddAppLogoOverride(new Uri(@""));
            notify.Show();

            // запускаем таймер для переключения изображений
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();

            // выводим первое изображение
            ShowImage();

        }
        private bool IsDarkModeEnabled()
        {
            // Implement the logic to determine if dark mode is enabled
            // For example, you could check the system preferences or app settings
            return true; // Change this to return true or false based on your requirements
        }
        #endregion
        #region Обработчик на расскрытие

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(this).X < 10)
            {
                var drawerHost = FindVisualChild<DrawerHost>(this);
                drawerHost.IsLeftDrawerOpen = true;
            }
        }
        #endregion
        #region Хрень какая-то
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    var result = FindVisualChild<T>(child);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
        #endregion
        #region Слайдшоу фото

        private void Timer_Tick(object sender, EventArgs e)
        {
            // переключаемся на следующее изображение
            _currentIndex++;
            if (_currentIndex >= _imagePaths.Count)
            {
                _currentIndex = 0;
            }

            ShowImage();
        }
        private void ShowImage()
        {
            // загружаем изображение по указанному пути
            var bitmap = new BitmapImage(new Uri(_imagePaths[_currentIndex]));

            // создаем анимацию для перехода между изображениями
            var fadeIn = new DoubleAnimation(0, 2, TimeSpan.FromSeconds(1));
            var fadeOut = new DoubleAnimation(2, 0, TimeSpan.FromSeconds(1));
            fadeOut.BeginTime = TimeSpan.FromSeconds(4);

            // добавляем анимацию к изображению
            var image = new Image();
            image.Source = bitmap;
            image.BeginAnimation(OpacityProperty, fadeIn);
            image.BeginAnimation(OpacityProperty, fadeOut);

            // заменяем текущее изображение на новое
            this.ImageContainer.Children.Clear();
            this.ImageContainer.Children.Add(image);
        }
        #endregion

        #region Переход в почту
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.ShowDialog();
        }
        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vh vh = new vh();   
            vh.Show();
        }

        private void OnSwitchLightThemeClicked(object sender, RoutedEventArgs e)
        {
            this.Background = Brushes.White;
            this.Foreground = Brushes.Black;
        }

        private void OnSwitchDarkThemeClicked(object sender, RoutedEventArgs e)
        {
            this.Background = Brushes.Black;
            this.Foreground = Brushes.White;
        }

        private void btnTBclick_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://transset.ru/products/?klyuchevye-napravleniya=transport_security#klyuchevye-napravleniya";
            Process.Start(new ProcessStartInfo(url));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Contacts c = new Contacts();
            c.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            About a = new About();
            a.Show();
        }
    }

}
