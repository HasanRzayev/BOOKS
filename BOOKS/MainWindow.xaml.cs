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

        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged();
            }
        }
        private int theme_id;

        public int Theme_ID
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

        public int Press_id
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

        public Book(int id,  string nAME, string pages, string year, int theme_ID, int catagory_id,  int author_id,  int press_id, string comment, string quality)
        {
            this.id=id;
            NAME=nAME;
            Pages=pages;
            Year=year;
            Theme_ID=theme_ID;
            Catagory_id=catagory_id;
            Author_id=author_id;
            Press_id=press_id;
            Comment=comment;
            Quality=quality;
        }

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

        ObservableCollection<Book> listboxin = new ObservableCollection<Book>();
        SqlDataReader reader = null;
        DataTable table = null;

        SqlDataAdapter dataAdapter = null;
        DataSet dataSet = null;
        SqlCommandBuilder cmdBuilder = null;

        List<string> authors = new List<string>();
        List<string> catagorys = new List<string>();
        List<string> themes = new List<string>();
        List<string> presss = new List<string>();
        string query=null;
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
        public void Selectthemes()
        {
            SqlDataReader reader = null;
            try
            {
                sqlConnection.Open();
                var cmd = new SqlCommand("Select * From Themes", sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    themes.Add((reader["Name"]).ToString());
                }
            }
            finally
            {
                sqlConnection?.Close();
                reader?.Close();
            }
        }
        public void SelectPress()
        {
            SqlDataReader reader = null;
            try
            {
                sqlConnection.Open();
                var cmd = new SqlCommand("Select * From Press", sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    presss.Add((reader["Name"]).ToString());
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
            SelectPress();
            Selectthemes();
            combobox_author.ItemsSource = authors;
            combobox_catagory.ItemsSource = catagorys;
            listbox_books.ItemsSource=listboxin;
            combobox_catagories.ItemsSource = catagorys;
            combobox_themes.ItemsSource = themes;
            combobox_press.ItemsSource = presss;
            combobox_authors.ItemsSource = authors;
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
            if ( combobox_catagory.SelectedItem!=null && combobox_catagory.SelectedItem!=null)
            {
                listboxin.Clear();
                SqlDataReader reader = null;
                try
                {

                    sqlConnection.Open();
                
                    dataAdapter=new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(
"select A.Id,A.Name,A.Pages,	A.YearPress,T.Name AS Themes_NAME,C.Name AS CATAGORY_NAME,B.FirstName AS ARTHUR_ID,P.Name AS PRESS_NAME,a.Comment,A.Quantity from Authors B INNER JOIN   Books  A   "
+ "   ON B.Id=A.Id_Author          "
+ "   INNER JOIN   Press  P        "
+ "   ON P.Id=A.Id_Press           "
+ "   INNER JOIN   Themes T        "
+ "   ON T.Id=A.Id_Themes          "
+ "   INNER JOIN   Categories C    "
+ "   ON C.Id=A.Id_Category  where B.FirstName=@LAZIM AND C.Name=@OPTION", sqlConnection);
                    query="select A.Id,A.Name,A.Pages,	A.YearPress,A.Id_Themes,A.Id_Category,A.Id_Author,A.Id_Press,a.Comment,A.Quantity from Authors B INNER JOIN   Books  A  ON B.Id=A.Id_Author INNER JOIN   Categories C ON A.Id_Category=C.Id where B.FirstName=@LAZIM AND C.Name=@OPTION";
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@LAZIM", combobox_author.SelectedItem);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@OPTION", combobox_catagory.SelectedItem);
                    cmdBuilder = new SqlCommandBuilder(dataAdapter);
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    listbox_books.ItemsSource = dataSet.Tables[0].DefaultView;
                



                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }
            }
            else if (combobox_catagory.SelectedItem!=null)
            {
                listboxin.Clear();
                SqlDataReader reader = null;
                try
                {
                    sqlConnection.Open();
                    dataAdapter=new SqlDataAdapter();


                    dataAdapter.SelectCommand = new SqlCommand(
      "select A.Id,A.Name,A.Pages,	A.YearPress,T.Name AS Themes_NAME,C.Name AS CATAGORY_NAME,B.FirstName AS ARTHUR_ID,P.Name AS PRESS_NAME,a.Comment,A.Quantity from Authors B INNER JOIN   Books  A   "
+ "   ON B.Id=A.Id_Author          "
+ "   INNER JOIN   Press  P        "
+ "   ON P.Id=A.Id_Press           "
+ "   INNER JOIN   Themes T        "
+ "   ON T.Id=A.Id_Themes          "
+ "   INNER JOIN   Categories C    "
+ "   ON C.Id=A.Id_Category  where B.Name=@LAZIM", sqlConnection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue("@LAZIM", combobox_catagory.SelectedItem);


                    cmdBuilder = new SqlCommandBuilder(dataAdapter);

                    dataSet = new DataSet();

                    dataAdapter.Fill(dataSet);

                    listbox_books.ItemsSource =dataSet.Tables[0].DefaultView;




                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }
            }
            else if (combobox_author.SelectedItem!=null)
            {
                listboxin.Clear();
                SqlDataReader reader = null;
                try
                {
                    
                    sqlConnection.Open();
                    dataAdapter=new SqlDataAdapter();

                  
                    dataAdapter.SelectCommand = new SqlCommand(
                   "select A.Id,A.Name,A.Pages,	A.YearPress,T.Name AS Themes_NAME,C.Name AS CATAGORY_NAME,B.FirstName AS ARTHUR_ID,P.Name AS PRESS_NAME,a.Comment,A.Quantity from Authors B INNER JOIN   Books  A   "
+ "   ON B.Id=A.Id_Author          "  
+ "   INNER JOIN   Press  P        "
+ "   ON P.Id=A.Id_Press           "
+ "   INNER JOIN   Themes T        "
+ "   ON T.Id=A.Id_Themes          "
+ "   INNER JOIN   Categories C    "
+ "   ON C.Id=A.Id_Category where B.FirstName=@LAZIM", sqlConnection);
                    dataAdapter.SelectCommand.Parameters.AddWithValue( "@LAZIM", combobox_author.SelectedItem);
                    query= "select A.Id,A.Name,A.Pages,	A.YearPress,A.Id_Themes,A.Id_Category,A.Id_Author,A.Id_Press,a.Comment,A.Quantity from Authors B INNER JOIN   Books  A  ON B.Id=A.Id_Author where B.FirstName=@LAZIM";
                    cmdBuilder = new SqlCommandBuilder(dataAdapter);
                    dataSet = new DataSet();
                    
                    dataAdapter.Fill(dataSet);
                    
                    listbox_books.ItemsSource =dataSet.Tables[0].DefaultView;



                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }
            }
        }

 

           

      
        private void update_button_Click(object sender, RoutedEventArgs e)
        {
            
                dataAdapter.Update(dataSet);
       
        }

        private void book_button_Click(object sender, RoutedEventArgs e)
        {
            string selectQuery = "select A.Id,A.Name,A.Pages,	A.YearPress,T.Name AS Themes_NAME,C.Name AS CATAGORY_NAME,B.FirstName AS ARTHUR_ID,P.Name AS PRESS_NAME,a.Comment,A.Quantity from Authors B INNER JOIN   Books  A   "
+ "   ON B.Id=A.Id_Author          "
+ "   INNER JOIN   Press  P        "
+ "   ON P.Id=A.Id_Press           "
+ "   INNER JOIN   Themes T        "
+ "   ON T.Id=A.Id_Themes          "
+ "   INNER JOIN   Categories C    "
+ "   ON C.Id=A.Id_Category";
            dataAdapter = new SqlDataAdapter(selectQuery, sqlConnection);
            cmdBuilder = new SqlCommandBuilder(dataAdapter);
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            listbox_books.ItemsSource =dataSet.Tables[0].DefaultView;
        }

    
        private void BUTTON_ADD_Click(object sender, RoutedEventArgs e)
        {
            book_button_Click(sender, e);
            dataAdapter.InsertCommand = new SqlCommand("insert into Books(id,name,Pages,YearPress,Id_Themes,Id_Category,Id_Author,Id_Press,Comment,Quantity) values(@id,@name,@Pages,@YearPress,@Id_Themes,@Id_Category,@Id_Author,@Id_Press,@Comment,@Quantity)", sqlConnection);
          
            try
            {
               
           
                //////////////////////////////////////////////////////
                int THEME_INDEX = 0;
                int AUTHOR_INDEX = 0;
                int CATAGORY_INDEX = 0;
                int PRESS_INDEX = 0;
                SqlDataReader reader = null;
                try
                {
                    sqlConnection.Open();
                    var lazimli = new SqlCommand("Select Id From Press  WHERE Name=@LAZIM  ", sqlConnection);
                    lazimli.Parameters.AddWithValue("@LAZIM", combobox_press.SelectedItem);
                    reader = lazimli.ExecuteReader();
                    while (reader.Read())
                    {
                        PRESS_INDEX=int.Parse( reader[0].ToString());
                    }
                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }
                //////////////////////////////////////////////////////
                try
                {
                    sqlConnection.Open();
                    var lazimli = new SqlCommand("Select Id From Authors  WHERE FirstName=@LAZIM  ", sqlConnection);
                    lazimli.Parameters.AddWithValue("@LAZIM", combobox_authors.SelectedItem);
                    reader = lazimli.ExecuteReader();
                    while (reader.Read())
                    {
                        AUTHOR_INDEX=int.Parse(reader[0].ToString());
                    }
                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }
                //////////////////////////////////////////////////////
                try
                {
                    sqlConnection.Open();
                    var lazimli = new SqlCommand("Select Id From Categories  WHERE Name=@LAZIM  ", sqlConnection);
                    lazimli.Parameters.AddWithValue("@LAZIM", combobox_catagories.SelectedItem);
                    reader = lazimli.ExecuteReader();
                    while (reader.Read())
                    {
                        CATAGORY_INDEX=int.Parse(reader[0].ToString());
                    }
                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }
                //////////////////////////////////////////////////////
                try
                {
                    sqlConnection.Open();
                    var lazimli = new SqlCommand("Select Id From Themes  WHERE Name=@LAZIM  ", sqlConnection);
                    lazimli.Parameters.AddWithValue("@LAZIM", combobox_themes.SelectedItem);
                    reader = lazimli.ExecuteReader();
                    while (reader.Read())
                    {
                        THEME_INDEX=int.Parse(reader[0].ToString());
                    }
                }
                finally
                {
                    sqlConnection?.Close();
                    reader?.Close();
                }

                dataAdapter.InsertCommand.Parameters.AddWithValue("@id", int.Parse(textbox_id.Text));
                 dataAdapter.InsertCommand.Parameters.AddWithValue("@name", textbox_name.Text);
                 dataAdapter.InsertCommand.Parameters.AddWithValue("@Pages", int.Parse(textbox_pages.Text));
                 dataAdapter.InsertCommand.Parameters.AddWithValue("@YearPress", int.Parse(textbox_year.Text));
                 dataAdapter.InsertCommand.Parameters.AddWithValue("@Id_Themes", THEME_INDEX);
                 dataAdapter.InsertCommand.Parameters.AddWithValue("@Id_Author", AUTHOR_INDEX);
                 dataAdapter.InsertCommand.Parameters.AddWithValue("@Id_Category", CATAGORY_INDEX);
                 dataAdapter.InsertCommand.Parameters.AddWithValue("@Id_Press", PRESS_INDEX);
                 dataAdapter.InsertCommand.Parameters.AddWithValue("@Comment", textbox_comment.Text);
                dataAdapter.InsertCommand.Parameters.AddWithValue("@Quantity", int.Parse(textbox_quality.Text));
                
                
                try
                {
                    sqlConnection.Open();
                    
                    cmdBuilder = new SqlCommandBuilder(dataAdapter);
                    dataSet = new DataSet();

                    dataAdapter.Fill(dataSet);
                    
                    dataAdapter.InsertCommand.ExecuteNonQuery();
                    dataAdapter.Update(dataSet);
                    book_button_Click(sender,e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
           
            finally
            {
                sqlConnection.Close();
            }

           
        }

        private void DELETE_button_Click(object sender, RoutedEventArgs e)
        {
            
            if (axtaris_textbox.Text!=null)
            {
                DataRowView row = (DataRowView)listbox_books.SelectedItem;
                
                dataSet.Rows.Remove(row.Row);
            }
        }
    }
}
