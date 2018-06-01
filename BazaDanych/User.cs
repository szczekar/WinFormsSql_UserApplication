using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace BazaDanych
{
    public class User
    {
        private string name;
        private string password;

        public User(string name, string password)
        {
            this.name = name;
            this.password = password;
        }

        public User(string name)
        {
            this.Name = name;
        }


        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }


        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        static SqlCommand sqlCommand;
        static string connectionString = "Server=KAROL;Database=TEST_CS;Trusted_Connection=true";
        static SqlConnection connection = new SqlConnection(connectionString);



        public static void openCloseDBconnection(string value, Dictionary<string, object> keyValuePairs)
        {

            using (sqlCommand = new SqlCommand(value, connection))
            {

                foreach (KeyValuePair<string, object> item in keyValuePairs)
                {

                    sqlCommand.Parameters.AddWithValue(item.Key, item.Value);
                }

                connection.Open();
                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void Add()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@Name", this.Name);
            dict.Add("@MyPassword", this.Password);

            try
            {
                if (String.IsNullOrEmpty(this.Name) || String.IsNullOrEmpty(this.Password))
                {
                    MessageBox.Show("Wpisz imie i haslo !");

                }
                else
                {
                    string newNameAndPassword = "INSERT INTO dbo.Users(Name,MyPassword) VALUES (@Name,@MyPassword)";
                    openCloseDBconnection(newNameAndPassword, dict);
                    MessageBox.Show("Imie i hasło zapisane do bazy !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Delete()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@Name", this.Name);

            try
            {
                if (!String.IsNullOrWhiteSpace(this.Name))
                {
                    string userToDelete = "DELETE FROM dbo.Users WHERE Name = @Name";
                    openCloseDBconnection(userToDelete, dict);

                    MessageBox.Show("User usunięty z bazy !");
                }
                else
                {
                    MessageBox.Show("Pole uzytkownika do usuniecia nie moze byc puste!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void Update(User user1, User user2)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@Name1", user1.Name);
            dict.Add("@Name2", user2.Name);

            try
            {
                if (String.IsNullOrEmpty(user1.Name) || String.IsNullOrEmpty(user2.Name))
                {
                    MessageBox.Show("Wpisz stare imie i nowe imie usera !");

                }
                else
                {
                    string userToUpdate = "UPDATE dbo.Users SET Name = @Name2 WHERE Name = @Name1";
                    openCloseDBconnection(userToUpdate, dict);
                    MessageBox.Show("User zaktualizowany!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public static List<string> Display()
        {
            List<string> usersList = new List<string>();
            try
            {
               string query = "SELECT[Name] FROM dbo.Users";

               using (SqlCommand cmd = new SqlCommand(query, connection))
               {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usersList.Add(reader[0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return usersList;
        }

        public static bool CheckUserAndPassword(User user)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@Name", user.Name);
            dict.Add("@MyPassword", user.Password);

            string username = null;
            string userpassword = null;

            string logAndPassword = "SELECT * FROM dbo.Users WHERE Name = @Name AND MyPassword = @MyPassword";

            try
            {

                using (SqlCommand cmd = new SqlCommand(logAndPassword, connection))
                {

                    foreach (KeyValuePair<string, object> item in dict)
                    {

                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }

                    connection.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            username = reader["Name"].ToString().Trim();
                            userpassword = reader["MyPassword"].ToString().Trim();
                        }
                         
                        if(username==user.Name && userpassword== user.Password)
                        {
                            return true;
                        }
                    }
                 
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

            finally
            {
                connection.Close();
            }
            return false;
        }

        public static bool isDataBaseEmpty(User user)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@Name",user.Name);
            dict.Add("@MyPassword",user.Password);

            try
            {
                connection.Open();
                string isAnyUserInDatabase = "SELECT COUNT(*) FROM dbo.Users";

                using (sqlCommand = new SqlCommand(isAnyUserInDatabase, connection))
                {
                    foreach (KeyValuePair<string, object> item in dict )
                    {

                        sqlCommand.Parameters.AddWithValue(item.Key, item.Value);
                    }

                    int rowsNumber = int.Parse(sqlCommand.ExecuteScalar().ToString());

                    if (rowsNumber==0)
                   {
                        MessageBox.Show("W bazie jest pusto, dodaj użytkownika !");
                        return true;
                   }
                   
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            finally
            {
                connection.Close();
            }

            return false;
        }
    }
 }

