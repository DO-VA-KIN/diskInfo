using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Threading;

namespace diskInfo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DriveInfo[] Drives;
        bool isAutoCheck = true;
        uint intAutoCheck = 30;

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = new TimeSpan(0, 0, (int)intAutoCheck);
            timer.Tick += new EventHandler(timer_Tick);

            readConfig();

            driveSearch();
            cmbDisk.SelectedIndex = 0;


        }
        private void timer_Tick(object sender, EventArgs e)
        {
            cmbDisk_SelectionChanged(null, null);
        }

        private void cmbDisk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDisk.SelectedIndex == -1)
                return;

            try
            {
                driveSearch();
                int ind = cmbDisk.SelectedIndex;

                if (Drives[ind].IsReady)
                {
                    chReady.IsChecked = true;
                    chReady.Content = "Готов к работе";
                }
                else
                {
                    chReady.IsChecked = false;
                    chReady.Content = "выключен";
                }

                lName.Content = "Имя: " + Drives[ind].Name;
                lType.Content = "Тип: " + Drives[ind].DriveType;
                lFileSystem.Content = "Файловая система: " + Drives[ind].DriveFormat;
                lVolumeLabel.Content = "Метка тома: " + Drives[ind].VolumeLabel;

                lTotalSize.Content = "Размер: " + Drives[ind].TotalSize;
                lAvailableSpace.Content = "Доступно: " + Drives[ind].AvailableFreeSpace;
                lFreeSpace.Content = "Свободно: " + Drives[ind].TotalFreeSpace;
                lFullSpace.Content = "Занято: " + (Drives[ind].TotalSize - Drives[ind].TotalFreeSpace);

                pbSize.Value = 100 - (Drives[ind].TotalFreeSpace * 100 / Drives[ind].TotalSize);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }




        public void driveSearch()
        {
            Drives = DriveInfo.GetDrives();
            if (Drives.Length != cmbDisk.Items.Count)
            {
                cmbDisk.Items.Clear();

                foreach (DriveInfo drive in Drives)
                    cmbDisk.Items.Add(drive.Name);

                cmbDisk.SelectedIndex = 0;
            }
        }




        private void miFile_Click(object sender, RoutedEventArgs e)
        {
            settings.WindowSettings window = new settings.WindowSettings();
            window.ShowDialog();

            readConfig();
        }
        private void readConfig()
        {
            try
            {
                string way = Environment.CurrentDirectory + @"\Settings.txt";
                StreamReader streamReader = new StreamReader(way);
                if (streamReader.ReadLine() == "true")
                { isAutoCheck = true; timer.Start(); }
                else isAutoCheck = false;
                intAutoCheck = Convert.ToUInt32(streamReader.ReadLine());
                timer.Interval = new TimeSpan(0, 0, (int)intAutoCheck);
                streamReader.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message + " Измените настройки"); 
                staticFunc.killProcessByName();
                miFile_Click(null, null); }
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Завершение работы?", "Внимание!",
                                MessageBoxButton.OKCancel,
                                MessageBoxImage.Question,
                                MessageBoxResult.Cancel,
                                MessageBoxOptions.ServiceNotification) != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
