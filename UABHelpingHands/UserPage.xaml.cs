using System;
using System.Collections.Generic;
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

using UABHelpingHands.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UABHelpingHands
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserPage : Page
    {
        public UserPage()
        {
            this.InitializeComponent();

            ((ListView)FindName("ListRep")).DataContext = Coleccion.GetObjects();
            ((ListView)FindName("ValPList")).DataContext = Coleccion2.GetObjects();
        }
        
        private void backTo(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			this.Frame.GoBack();
        }
        

        public class Coleccion
        {
            public static List<ObjetoAListar> GetObjects()
            {
                List<ObjetoAListar> objetos = new List<ObjetoAListar>()
                {
                    new ObjetoAListar("Matemáticas", 100),
                    new ObjetoAListar("Ingenieria", 100),
                    new ObjetoAListar("Redes", 100),
                    new ObjetoAListar("Arquitecturas", 100),
                    new ObjetoAListar("Compiladores", 10),
                };

                return objetos;
            }
        }

        public class Coleccion2
        {
            public static List<ObjetoAListar2> GetObjects()
            {
                List<ObjetoAListar2> objetos2 = new List<ObjetoAListar2>()
                {
                    new ObjetoAListar2("Bear Grylls"),
                    new ObjetoAListar2("Wally Buscando"),
                    new ObjetoAListar2("Rajoy President"),
                    new ObjetoAListar2("Mr. Bean"),
                    new ObjetoAListar2("Napoleon Bonaparte"),
                };

                return objetos2;
            }
        }

        public class ObjetoAListar : UABHelpingHands.Common.BindableBase
        {
            public String Reputacion { get; set; }
            public int Cantidad { get; set; }

            public ObjetoAListar(String _reputacion, int _cantidad)
            {
                Reputacion = _reputacion;
                Cantidad = _cantidad;
            }
        }

        public class ObjetoAListar2 : UABHelpingHands.Common.BindableBase
        {
            public String Alumno { get; set; }

            public ObjetoAListar2(String _alumno)
            {
                Alumno = _alumno;

            }
        }
    }
}
