using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Shapes;

namespace BOOKS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    class Book : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value;
                OnPropertyChanged();
            }
        }


        private string name;

        public string NAME
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private string pages;

        public string Pages
        {
            get { return pages; }
            set
            {
                pages = value;
                OnPropertyChanged();
            }
        }

        private string year;

        public string YAER
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged();
            }
        }
        private int theme_id;

        public int THEME_ID
        {
            get { return theme_id; }
            set
            {
                theme_id = value;
                OnPropertyChanged();
            }
        }
        private int catagory_id;

        public int Catagory_id
        {
            get { return catagory_id; }
            set
            {
                catagory_id = value;
                OnPropertyChanged();
            }
        }
        private int author_id;

        public int Author_id
        {
            get { return author_id; }
            set
            {
                author_id = value;
                OnPropertyChanged();
            }
        }
        private int press_id;

        public int PRESS_id
        {
            get { return press_id; }
            set
            {
                press_id = value;
                OnPropertyChanged();
            }
        }
        private string comment;

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged();
            }
        }
        private string quality;

        public string Quality
        {
            get { return quality; }
            set
            {
                quality = value;
                OnPropertyChanged();
            }
        }
    }
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        SqlConnection sqlConnection = null;
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        ObservableCollection<string> listboxin = new ObservableCollection<string>();
 
        List<string> authors = new List<string>();
        List<string> catagorys = new List<string>();

        public void SelectAuthor()
        {
            SqlDataReader reader = null;
            try
            {
                sqlConnection.Open();
                var cmd = new SqlCommand("Select * From Authors", sqlConnection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //Console.WriteLine($"{reader[0]} {reader[1]} {reader[2]}");
                    authors.Add((reader[1]).ToString());
                }
            }
            finally
            {
                sqlConnection?.Close();
                reader?.Close();
            }
        }
        public void SelectCatagory()
        {
            SqlDataReader reader = null;
            try
            {
                sqlConnection.Open();
                var cmd = new SqlCommand("Select * From Categories", sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    catagorys.Add((reader["Name"]).ToString());
                }
            }
            finally
            {
                sqlConnection?.Close();
                reader?.Close();
            }
        }
        public MainWindow() 
        {
            InitializeComponent();
            var conStr = "Data Source = WIN-EA8010O87DM; Initial Catalog = Library; Integrated Security = True;";
            sqlConnection = new SqlConnection(conStr);
            SelectAuthor();
            SelectCatagory();
            combobox_author.ItemsSource = authors;
            combobox_catagory.ItemsSource = catagorys;
            listbox_books.ItemsSource=listboxin;
   
            DataContext =this;
        }


        private void LOAD_GRID()
        {
            SqlDataReader reader = null;
            var cmd = new SqlCommand("select * from Books", sqlConnection);
            DataTable dt= new DataTable();

            sqlConnection.Open();
            reader = cmd.ExecuteReader();
            dt.Load(reader);
            sqlConnection?.Close();
            listbox_books.ItemsSource=dt.DefaultView;

        }



        private void author_button_Click(object sender, RoutedEventArgs e)
        {
         
            if (combobox_author.SelectedItem!=null)
            {
                listboxin.Clear();
                SqlDataReader reader = null;
                try
                {
                    
                    sqlConnection.Open();
                    var cmd = new SqlCommand("select A.Name from Authors B INNER JOIN   Books  A  ON B.Id=A.Id_Author where B.FirstName=@LAZIM", sqlConnection);

                    cmd.Parameters.AddWithValue("@LAZIM", combobox_author.SelectedItem);

                    reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        listboxin.Add(reader[0].ToString());
                    }
                   
                      
                    
                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }
            }
        }

        private void Catagory_button_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_catagory.SelectedItem!=null)
            {
                listboxin.Clear();
                SqlDataReader reader = null;
                try
                {

                    sqlConnection.Open();
                    var cmd = new SqlCommand("select A.Name from Categories B INNER JOIN   Books  A  ON B.Id=A.Id_Category where B.Name=@LAZIM", sqlConnection);

                    cmd.Parameters.AddWithValue("@LAZIM", combobox_catagory.SelectedItem);

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        listboxin.Add(reader[0].ToString());
                    }



                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }
            }
        }
    }
}
