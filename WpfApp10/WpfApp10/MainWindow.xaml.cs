using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Collections;
using WpfApp10.Properties;

namespace WpfApp10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        IniFile INI = new IniFile("config.ini");
        private bool active = true; //true - если левое окно активное и наоборот
        public string crrName;
        public string DirTo; //Путь вставки файла
        public string DirFrom; //Путь копирования файла
        public class FileInf
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public string Size { get; set; }
            public string Data { get; set; }
            public string Path { get; set; }
        }

        //=======================
        public MainWindow()
        {
            InitializeComponent();
            listView_right.SelectedIndex = -1;
            listView_left.SelectedIndex = 0;
            auto_read();
            Fill_left();
            Fill_right();
            

        }
        //________________1 окно______________
        //кнопка "Перейти" - загрузка файлов и папок
        private void Fill_left()
        {
            try
            {
                //очищаем лист вью, чтобы не наславиалось 
                listView_left.Items.Clear();

                DirectoryInfo dir = new DirectoryInfo(textBox_left.Text);

                DirectoryInfo[] dirs = dir.GetDirectories();


                if (dir.ToString() != @"C:\")
                    listView_left.Items.Add(new FileInf { Name = "...", Type = "", Size = "", Data = "", Path = "" });

                foreach (DirectoryInfo crrDir in dirs)                //ищем и выводим все папки
                {
                    //фильтр на скрытые папки
                    if ((crrDir.Attributes & FileAttributes.Hidden) == 0)
                    {
                        listView_left.Items.Add(new FileInf { Name = crrDir.Name, Type = "-", Size = "Папка", Data = (crrDir.LastWriteTime).ToString(), Path = crrDir.FullName });
                    }
                }

                FileInfo[] File = dir.GetFiles();
                foreach (FileInfo crrFile in File)                //ищем и выводим все файлы
                {
                    //фильтр на скрытые файлы
                    if ((crrFile.Attributes & FileAttributes.Hidden) == 0)
                    {
                        listView_left.Items.Add(new FileInf { Name = crrFile.Name, Type = crrFile.Extension, Size = (crrFile.Length / 1048576).ToString() + " МБ", Data = (crrFile.LastWriteTime).ToString(), Path = crrFile.FullName });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        //переход по двойному клику
        private void ListView_left_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                active = true;
                var a = (FileInf)listView_left.SelectedItem;
                string crrName1 = a.Name;
                if (crrName1 == "...")
                {

                    Back_Click_left();
                }
                //если нет расширения
                else if (Path.GetExtension(Path.Combine(textBox_left.Text, crrName1)) == "")
                {

                    textBox_left.Text = Path.Combine(textBox_left.Text, crrName1);
                    Fill_left();
                }
                else
                {

                    Process.Start(Path.Combine(textBox_left.Text, crrName1));
                }
            }
            catch
            { }


        }
        //кнопка "Назад" - загрузка файлов и папок
        private void Back_Click_left()
        {
            if (textBox_left.Text[textBox_left.Text.Length - 1] == '\\')
            {
                textBox_left.Text = textBox_left.Text.Remove(textBox_left.Text.Length - 1, 1);

                while (textBox_left.Text[textBox_left.Text.Length - 1] != '\\')
                {
                    textBox_left.Text = textBox_left.Text.Remove(textBox_left.Text.Length - 1, 1);
                }
            }
            else if (textBox_left.Text[textBox_left.Text.Length - 1] != '\\')
            {
                while (textBox_left.Text[textBox_left.Text.Length - 1] != '\\')
                {
                    textBox_left.Text = textBox_left.Text.Remove(textBox_left.Text.Length - 1, 1);
                }
            }

            Fill_left();
        }
        //переход через enter
        private void TextBox_left_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        Fill_left();
                        break;
                    }
            }
        } 
        //сделать активным левое окно
        private void ListView_left_MouseDown(object sender, MouseButtonEventArgs e)
        {
            active = true;
            
        }
        //нажатие кнопок на левом окне
        private void ListView_left_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    active = true;
                    var a = (FileInf)listView_left.SelectedItem;
                    string crrName1 = a.Name;
                    if (crrName1 == "...")
                    {

                        Back_Click_left();
                    }
                    //если нет расширения
                    else if (Path.GetExtension(Path.Combine(textBox_left.Text, crrName1)) == "")
                    {

                        textBox_left.Text = Path.Combine(textBox_left.Text, crrName1);
                        Fill_left();
                    }
                    else
                    {

                        Process.Start(Path.Combine(textBox_left.Text, crrName1));
                    }
                }
                catch
                { }
            }
        }


        //________________2 окно______________

        private void Fill_right()
        {
            try
            {
                //очищаем лист вью, чтобы не наславиалось 
                listView_right.Items.Clear();

                DirectoryInfo dir = new DirectoryInfo(textBox_right.Text);

                DirectoryInfo[] dirs = dir.GetDirectories();


                if (dir.ToString() != @"C:\")
                    listView_right.Items.Add(new FileInf { Name = "...", Type = "", Size = "", Data = "", Path = "" });

                foreach (DirectoryInfo crrDir in dirs)                //ищем и выводим все папки
                {
                    //фильтр на скрытые папки
                    if ((crrDir.Attributes & FileAttributes.Hidden) == 0)
                    {
                        listView_right.Items.Add(new FileInf { Name = crrDir.Name, Type = "-", Size = "Папка", Data = (crrDir.LastWriteTime).ToString(), Path = crrDir.FullName });
                    }
                }

                FileInfo[] File = dir.GetFiles();
                foreach (FileInfo crrFile in File)                //ищем и выводим все файлы
                {
                    //фильтр на скрытые файлы
                    if ((crrFile.Attributes & FileAttributes.Hidden) == 0)
                    {
                        listView_right.Items.Add(new FileInf { Name = crrFile.Name, Type = crrFile.Extension, Size = (crrFile.Length / 1048576).ToString() + " МБ", Data = (crrFile.LastWriteTime).ToString(), Path = crrFile.FullName });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        //переход по двойному клику
        private void ListView_right_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                active = false;
                var a = (FileInf)listView_right.SelectedItem;
                string crrName1 = a.Name;
                if (crrName1 == "...")
                {
                    Back_Click_right();
                }
                //если нет расширения
                else if (Path.GetExtension(Path.Combine(textBox_right.Text, crrName1)) == "")
                {
                    textBox_right.Text = Path.Combine(textBox_right.Text, crrName1);
                    Fill_right();
                }
                else
                {
                    Process.Start(Path.Combine(textBox_right.Text, crrName1));
                }
            }
            catch
            {

            }

        }
        //кнопка "Назад" - загрузка файлов и папок
        private void Back_Click_right()
        {
            if (textBox_right.Text[textBox_right.Text.Length - 1] == '\\')
            {
                textBox_right.Text = textBox_right.Text.Remove(textBox_right.Text.Length - 1, 1);

                while (textBox_right.Text[textBox_right.Text.Length - 1] != '\\')
                {
                    textBox_right.Text = textBox_right.Text.Remove(textBox_right.Text.Length - 1, 1);
                }
            }
            else if (textBox_right.Text[textBox_right.Text.Length - 1] != '\\')
            {
                while (textBox_right.Text[textBox_right.Text.Length - 1] != '\\')
                {
                    textBox_right.Text = textBox_right.Text.Remove(textBox_right.Text.Length - 1, 1);
                }
            }

            Fill_right();
        }
        //сделать активным правое окно 
        private void ListView_right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            active = false;
           
        }
        //переход через enter
        private void TextBox_right_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        Fill_right();
                        break;
                    }
            }
        }
        //нажатие кнопок на правом окне
        private void ListView_right_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    active = false;
                    var a = (FileInf)listView_right.SelectedItem;
                    string crrName1 = a.Name;
                    if (crrName1 == "...")
                    {
                        Back_Click_right();
                    }
                    //если нет расширения
                    else if (Path.GetExtension(Path.Combine(textBox_right.Text, crrName1)) == "")
                    {
                        textBox_right.Text = Path.Combine(textBox_right.Text, crrName1);
                        Fill_right();
                    }
                    else
                    {
                        Process.Start(Path.Combine(textBox_right.Text, crrName1));
                    }
                }
                catch
                {

                }
            }
        }





        //________________НИЖНИЕ КНОПКИ______________

        //Кнопка "копировать"
        private void Copy_Click(object sender, EventArgs e) 
        {
            try {
                if (active)
                {
                    var a = (FileInf)listView_left.SelectedItem;
                    crrName = a.Name;
                    DirFrom = Path.Combine(textBox_left.Text, crrName);
                }
                else if (!active)
                {
                    var a = (FileInf)listView_right.SelectedItem;
                    crrName = a.Name;
                    DirFrom = Path.Combine(textBox_right.Text, crrName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        //Копирование папки и ее содержимого
        void DirCopy(string FromDir, string ToDir) 
        {
            
                Directory.CreateDirectory(ToDir);
                foreach (string crrDir in Directory.GetDirectories(FromDir))
                {
                    DirCopy(crrDir, Path.Combine(textBox_left.Text, Path.GetFileName(crrDir)));
                }
                foreach (string crrFile1 in Directory.GetFiles(FromDir))
                {
                    string crrFile2 = Path.Combine(ToDir, Path.GetFileName(crrFile1));
                    File.Copy(crrFile1, crrFile2);
                }

           
        }
        //Кнопка "вставить"
        private void Paste_Click(object sender, EventArgs e) 
        {
            if (active)
            {
                DirTo = Path.Combine(textBox_left.Text, crrName);
                try
                {
                    if (Path.GetExtension(DirFrom) == "")
                    {
                        string crrFile2 = Path.Combine(textBox_left.Text, Path.GetFileName(DirTo));
                        DirCopy(DirFrom, DirTo);
                    }
                    else
                    {
                        File.Copy(DirFrom, DirTo, true);
                    }

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    
                }
            }
            else if(!active)
            {
               
                DirTo = Path.Combine(textBox_right.Text, crrName);
                try
                {
                    if (Path.GetExtension(DirFrom) == "")
                    {
                        DirCopy(DirFrom, DirTo);
                    }
                    else
                    {
                        File.Copy(DirFrom, DirTo, true);
                    }   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); 
                }
            }
            Fill_left();
            Fill_right();
        }
        //кнопка "удалить"
        private void Delete_Click(object sender, RoutedEventArgs e)  
        {
            if (active)
            {
                var a = (FileInf)listView_left.SelectedItem;
                string crrName = a.Name;
                DirFrom = Path.Combine(textBox_left.Text, crrName);
                if (Path.GetExtension(DirFrom) == "")
                {
                    Directory.Delete(DirFrom, true);
                }
                else
                {
                    File.Delete(DirFrom);
                }

                Fill_left();
            }
            else if (!active)
            {
                var a = (FileInf)listView_right.SelectedItem;
                string crrName = a.Name;
                DirFrom = Path.Combine(textBox_right.Text, crrName);
                if (Path.GetExtension(DirFrom) == "")
                {
                    Directory.Delete(DirFrom, true);
                }
                else
                {
                    File.Delete(DirFrom);
                }

                Fill_right();
            }
        }
        //кнопка "вырезать/вставить"
        private void Insert_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                if (active)
                {
                    DirTo = Path.Combine(textBox_left.Text, crrName);
                    if (Path.GetExtension(DirFrom) == "")
                    {
                        Directory.Move(DirFrom, DirTo);
                    }
                    else
                    {
                        File.Move(DirFrom, DirTo);
                    }
                }
                else if(!active)
                {
                    DirTo = Path.Combine(textBox_right.Text, crrName);
                    if (Path.GetExtension(DirFrom) == "")
                    {
                        Directory.Move(DirFrom, DirTo);
                    }
                    else
                    {
                        File.Move(DirFrom, DirTo);
                    }
                }
                Fill_left();
                Fill_right();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        //кнопка "новая папка"
        private void New_Click(object sender, RoutedEventArgs e)
        {
            Window1 newFolder = new Window1();
            newFolder.Show();
            newFolder.Closing += (senderr, eventArgs) =>
            {
                try
                {
                    if (active)
                    {
                        Directory.CreateDirectory(
                            Path.Combine(textBox_left.Text, Window1.nameF));
                        Fill_left();
                    }
                    else if (!active)
                    {
                        Directory.CreateDirectory(
                            Path.Combine(textBox_right.Text, Window1.nameF));
                        Fill_right();
                    }
                }
                catch
                {
                    MessageBox.Show("Введите другое названи папки");
                }

            };
        }
        // Горячие клавиши
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    {
                        Copy_Click(sender, e);
                        break;
                    }
                case Key.F2:
                    {
                        Paste_Click(sender, e);
                        break;
                    }
                case Key.F3:
                    {
                        Delete_Click(sender, e);
                        break;
                    }
                case Key.F4:
                    {
                        Insert_Click(sender, e);
                        break;

                    }
                case Key.F5:
                    {
                        New_Click(sender, e);
                        break;
                    }
                    /*case Key.Tab :
                   {
                       if (active)
                       {
                           active = false;
                           listView_left.SelectedIndex = -1;
                           listView_right.SelectedIndex = 0;
                           break;
                       }
                       if (!active)
                       {
                           active = true;
                           listView_left.SelectedIndex = 0;
                           listView_right.SelectedIndex = -1;
                           break;
                       }
                       break;
                   }*/
            }
        }

        private void ListView_left_Drop(object sender, DragEventArgs e)
        {
            MessageBox.Show("11111111");
            if (e.Data.GetData("System.IO.DirectoryInfo") != null)
            {
                ((DirectoryInfo)e.Data.GetData("System.IO.DirectoryInfo")).MoveTo(textBox_left.Text + @"\" + ((DirectoryInfo)e.Data.GetData("System.IO.DirectoryInfo")).Name);
            }
            if (e.Data.GetData("System.IO.FileInfo") != null)
            {
                ((FileInfo)e.Data.GetData("System.IO.FileInfo")).MoveTo(textBox_left.Text + @"\" + ((FileInfo)e.Data.GetData("System.IO.FileInfo")).Name);
            }
            Fill_left();
            Fill_right();
        }

        private void ListView_right_Drop(object sender, DragEventArgs e)
        {
            MessageBox.Show("2222222");
            if (e.Data.GetData("System.IO.DirectoryInfo") != null)
            {
                ((DirectoryInfo)e.Data.GetData("System.IO.DirectoryInfo")).MoveTo(textBox_right.Text + @"\" + ((DirectoryInfo)e.Data.GetData("System.IO.DirectoryInfo")).Name);
            }
            if (e.Data.GetData("System.IO.FileInfo") != null)
            {
                ((FileInfo)e.Data.GetData("System.IO.FileInfo")).MoveTo(textBox_right.Text + @"\" + ((FileInfo)e.Data.GetData("System.IO.FileInfo")).Name);
            }
            Fill_left();
            Fill_right();
        }

        private void ListView_left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listView_left.SelectedItem != null)
            {
                ListView FileWinwow = (ListView)sender;
                if (listView_left.SelectedItem.GetType().Name == "DirectoryInfo")
                {
                    DragDrop.DoDragDrop(FileWinwow, (DirectoryInfo)listView_left.SelectedItem, DragDropEffects.Move);
                }
                if (listView_left.SelectedItem.GetType().Name == "FileInfo")
                {
                    DragDrop.DoDragDrop(FileWinwow, (FileInfo)listView_left.SelectedItem, DragDropEffects.Move);
                }
            }
        }

        private void ListView_right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (listView_right.SelectedItem != null)
            {
                ListView FileWinwow = (ListView)sender;
                if (listView_right.SelectedItem.GetType().Name == "DirectoryInfo")
                {
                    DragDrop.DoDragDrop(FileWinwow, (DirectoryInfo)listView_left.SelectedItem, DragDropEffects.Move);
                }
                if (listView_right.SelectedItem.GetType().Name == "FileInfo")
                {
                    DragDrop.DoDragDrop(FileWinwow, (FileInfo)listView_right.SelectedItem, DragDropEffects.Move);
                }
            }
        }



        ///___________INI_____________

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            INI.Write("window", "Height", window.Height.ToString());
            INI.Write("window", "Width", window.Width.ToString());
            INI.Write("path", "left", textBox_left.Text);
            INI.Write("path", "right", textBox_right.Text);
            MessageBox.Show("Настройки Window и Path сохранены", "Информация", MessageBoxButton.OK); // Говорим пользователю, что сохранили текст.

        }
        private void auto_read()
        {
            
                window.Width = int.Parse(INI.ReadINI("window", "Width"));
                window.Height = int.Parse(INI.ReadINI("window", "Height"));
                textBox_left.Text = INI.ReadINI("path", "left");
                textBox_right.Text = INI.ReadINI("path", "right");

           

        }

    }
}
