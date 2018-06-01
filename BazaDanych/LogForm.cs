using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazaDanych
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            
            User user = new User(textBoxLogin.Text, textBoxPassword.Text);
            string login = user.Name;
            string pass = user.Password;

            bool checkIfDataBaseEmpty = User.isDataBaseEmpty(user);
            bool checkCredentials = User.CheckUserAndPassword(user);

            if (checkIfDataBaseEmpty == true)
            {
                this.Visible = false;
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
            if (checkCredentials == true)
            {
                this.Visible = false;
                Form1 form1 = new Form1();
                MessageBox.Show("Jesteś zalogowany!");
                form1.ShowDialog();
            }
            else
            {
                MessageBox.Show("User i haslo nie istnieje w bazie danych. Spróbuj ponownie.");
            }

        }
    }
}
