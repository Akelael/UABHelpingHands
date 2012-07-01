using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UABHelpingHands
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TextBox loginNIUTB;
        private PasswordBox loginPassTB;
        private TextBox regNiuTB;
        private PasswordBox regPassTB1;
        private PasswordBox regPassTB2;
        private TextBlock registerRes;
        private TextBox regName;
        private TextBox regSurname;
        private TextBlock loginResult;

        private List<Usuario> userdb = new List<Usuario>();
        public MainPage()
        {
            this.InitializeComponent();
            /*Creación de una miniDB para test*/
            // Creates and initializes a new ArrayList.
            Usuario newUser = new Usuario();
            newUser.NIU = "1";
            newUser.Nombre = "User1";
            newUser.Apellidos = "Ap1";
            newUser.Comentario = "Com1";
            newUser.Password = "1";
            userdb.Add(newUser);
            newUser = new Usuario();
            newUser.NIU = "2";
            newUser.Nombre = "User2";
            newUser.Apellidos = "Ap2";
            newUser.Comentario = "Com2";
            newUser.Password = "Pass2";
            userdb.Add(newUser);
            newUser = new Usuario();
            newUser.NIU = "3";
            newUser.Nombre = "User3";
            newUser.Apellidos = "Ap3";
            newUser.Comentario = "Com3";
            newUser.Password = "Pass3";
            userdb.Add(newUser);
            newUser = new Usuario();
            newUser.NIU = "4";
            newUser.Nombre = "User4";
            newUser.Apellidos = "Ap4";
            newUser.Comentario = "Com4";
            newUser.Password = "Pass4";
            userdb.Add(newUser);
            newUser = new Usuario();
            newUser.NIU = "5";
            newUser.Nombre = "User5";
            newUser.Apellidos = "Ap5";
            newUser.Comentario = "Com5";
            newUser.Password = "Pass5";
            userdb.Add(newUser);

        }

        private void OnClickSendButton(object sender, RoutedEventArgs e)
        {
            //Código para comprobar que el usuario está en el sistema
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            regPassTB1 = (PasswordBox)FindName("pass1RegTB");
            regPassTB2 = (PasswordBox)FindName("pass2RegTB");
            regNiuTB = (TextBox)FindName("regNIUTB");
            registerRes = (TextBlock)FindName("regRes");
            if (regPassTB1.Password == regPassTB2.Password)
            {
                bool found = false;
                int i = 0;
                while (!found && i < userdb.Count)
                {
                    Usuario useri = userdb.ElementAt(i);
                    if (useri.NIU.ToString().Equals(regNiuTB.Text))
                    {
                        found = true;
                        registerRes.Text = "NIU ya registrado";
                    }
                    else
                        i++;
                }
                if (!found)
                {
                    regName = (TextBox)FindName("regNameTB");
                    regSurname = (TextBox)FindName("regSurnameTB");
                    Usuario newUser = new Usuario();
                    newUser.NIU = regNiuTB.Text;
                    newUser.Nombre = regName.Text;
                    newUser.Apellidos = regSurname.Text;
                    newUser.Comentario = "";
                    newUser.Password = regPassTB1.Password;
                    userdb.Add(newUser);
                    registerRes.Text = "Registrado Correctamente";

                }
            }
            else
            {
                registerRes.Text = "Passwords no coinciden";
            }

        }

        private void loginButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            bool found = false;
            int i = 0;
            loginNIUTB = (TextBox)FindName("niuLoginTB");
            String NIU = loginNIUTB.Text;
            loginResult = (TextBlock)FindName("loginRes");
            while (!found && i < userdb.Count)
            {
                Usuario useri = userdb.ElementAt(i);
                if (useri.NIU.ToString().Equals(NIU))
                {
                    found = true;
                    loginPassTB = (PasswordBox)FindName("passLoginTB");
                    String password = loginPassTB.Password;
                    if (useri.Password.Equals(password))
                    {
                        loginResult.Text = "Autenticación correcta";

                        this.Frame.Navigate(typeof(DashBoardPage), "AllGroups");
                    }
                    else
                    {
                        loginResult.Text = "Password erróneo";
                    }
                }
                else
                    i++;
                if (!found)
                    loginResult.Text = "NIU erróneo";
            }
        }
    }

    public class Usuario
    {
        public String NIU { get; set; }
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String Comentario { get; set; }
        public int LlOros { get; set; }
        public int Reputacion { get; set; }
        public String Password { get; set; }

        public Usuario()
        {
            NIU = "";
            Nombre = "";
            Apellidos = "";
            Comentario = "";
            LlOros = 0;
            Reputacion = 0;
        }
    }
}