using System;
using System.Windows;
using System.IO;


namespace diskInfo.settings
{
    /// <summary>
    /// Логика взаимодействия для WindowSettings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        public WindowSettings()
        {
            InitializeComponent();
        }

        private void wSettings_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string way = Environment.CurrentDirectory + @"\Settings.txt";
                StreamReader streamReader = new StreamReader(way);
                if (streamReader.ReadLine() == "true")
                    chAutoCheck.IsChecked = true;
                else chAutoCheck.IsChecked = false;
                tbIntAutoCheck.Text = streamReader.ReadLine();
                streamReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Измените настройки");
            }
        }

        private void btnSettingsConfirm_Click(object sender, RoutedEventArgs e)
        {
            string way = Environment.CurrentDirectory + @"\Settings.txt";
            staticFunc.checkCreate_Exist_SettingsFile(way);
            StreamWriter streamWriter = new StreamWriter(way, false);

            try
            {
                if (chAutoCheck.IsChecked == true)
                    streamWriter.WriteLine("true");
                else streamWriter.WriteLine("false");

                string text = tbIntAutoCheck.Text;
                Convert.ToUInt32(tbIntAutoCheck.Text);
                streamWriter.WriteLine(tbIntAutoCheck.Text);

                streamWriter.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); staticFunc.killProcessByName(); }

            wSettings.Close();
        }


    }
}
