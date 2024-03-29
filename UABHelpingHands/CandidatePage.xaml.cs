﻿using UABHelpingHands.Data;
using UABHelpingHands.Common;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página dividida está documentada en http://go.microsoft.com/fwlink/?LinkId=234234

namespace UABHelpingHands
{
    /// <summary>
    /// Página en la que se muestra un título de grupo, una lista de los elementos contenidos en el grupo y detalles del
    /// elemento seleccionado actualmente.
    /// </summary>
    public sealed partial class CandidatePage : UABHelpingHands.Common.LayoutAwarePage
    {
        public CandidatePage()
        {
            this.InitializeComponent();
        }

        #region Page state management

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            //var group = SampleDataSource2.GetGroup((String)navigationParameter);
            var group = SampleDataSource2.GetGroup("Redes");
            this.DefaultViewModel["Group"] = group;
            this.DefaultViewModel["Items"] = group.Items;

            if (pageState == null)
            {
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (pageState.ContainsKey("SelectedItem") && this.itemsViewSource.View != null)
                {
                    var selectedItem = SampleDataSource2.GetItem((String)pageState["SelectedItem"]);
                    this.itemsViewSource.View.MoveCurrentTo(selectedItem);
                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            if (this.itemsViewSource.View != null)
            {
                var selectedItem = (SampleDataItem2)this.itemsViewSource.View.CurrentItem;
                if (selectedItem != null) pageState["SelectedItem"] = selectedItem.UniqueId;
            }
        }

        #endregion

        #region Navegación de páginas lógicas

        // En la administración de estados visuales normalmente se reflejan directamente los cuatro estados de vista de la aplicación
        // (Landscape y Portrait en modo Full y las vistas Snapped y Fill). La página dividida está
        // diseñada de forma que los estados de vista Snapped y Portrait tengan dos subestados distintos cada uno:
        // se muestra la lista de elementos o los detalles, pero no ambos al mismo tiempo.
        //
        // Todo esto se implementa con una única página física que puede representar a dos páginas
        // lógicas. El código siguiente logra este objetivo sin que el usuario note
        // la distinción.

        /// <summary>
        /// Se invoca para determinar si la página debe actuar como una página lógica o como dos.
        /// </summary>
        /// <param name="viewState">El estado de vista para el que se plantea la cuestión, o null
        /// para el estado de vista actual. Este parámetro es opcional con null como
        /// valor predeterminado.</param>
        /// <returns>Es true cuando el estado de vista en cuestión es Portrait o Snapped, de lo contrario
        /// es false.</returns>
        private bool UsingLogicalPageNavigation(ApplicationViewState? viewState = null)
        {
            if (viewState == null) viewState = ApplicationView.Value;
            return viewState == ApplicationViewState.FullScreenPortrait ||
                viewState == ApplicationViewState.Snapped;
        }

        /// <summary>
        /// Se invoca al seleccionarse un elemento de la lista.
        /// </summary>
        /// <param name="sender">Objeto GridView (o ListView cuando la aplicación tiene el estado Snapped)
        /// que muestra el elemento seleccionado.</param>
        /// <param name="e">Datos de evento que describen cómo se cambió la selección.</param>
        void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidar el estado de vista cuando la navegación de páginas lógicas está activa, ya que un cambio en
            // la selección puede causar un cambio correspondiente en la página lógica actual. Cuando
            // se selecciona un elemento, el efecto es el mismo obtenido al cambiar de mostrar la lista de elementos
            // a mostrar los detalles del elemento seleccionado. Al borrar la selección, el
            // efecto es el contrario.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();
        }

        /// <summary>
        /// Se invoca al presionar el botón Atrás de la página.
        /// </summary>
        /// <param name="sender">Instancia del botón Atrás.</param>
        /// <param name="e">Datos de evento que describen cómo se hizo clic en el botón Atrás.</param>
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation() && itemListView.SelectedItem != null)
            {
                // Cuando la navegación de páginas lógicas está activa y hay un elemento seleccionado, los
                // detalles de este se muestran en ese momento. Al borrar la selección se volverá
                // a la lista de elementos. Desde el punto de vista del usuario, esta es una navegación regresiva
                // lógica.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                // Cuando la navegación de páginas lógicas no está activa, o no hay ningún elemento
                // seleccionado, use el comportamiento predeterminado del botón Atrás.
                base.GoBack(sender, e);
            }
        }

        /// <summary>
        /// Se invoca para determinar el nombre del estado visual correspondiente al
        /// estado de vista de una aplicación.
        /// </summary>
        /// <param name="viewState">Estado de vista para el que se plantea la cuestión.</param>
        /// <returns>Nombre del estado visual deseado. Este coincide con el nombre del
        /// estado de vista, salvo cuando hay un elemento seleccionado en las vistas Portrait y Snapped donde
        /// esta página lógica adicional se representa mediante la adición de un sufijo de _Detail.</returns>
        protected override string DetermineVisualState(ApplicationViewState viewState)
        {
            // Actualizar el estado habilitado del botón Atrás al cambiar el estado de vista
            var logicalPageBack = this.UsingLogicalPageNavigation(viewState) && this.itemListView.SelectedItem != null;
            var physicalPageBack = this.Frame != null && this.Frame.CanGoBack;
            this.DefaultViewModel["CanGoBack"] = logicalPageBack || physicalPageBack;

            // Determinar los estados visuales para los diseños de tipo Landscape en función no del estado de vista, sino
            // del ancho de la ventana. Esta página tiene un diseño apropiado para un ancho de
            // 1366 píxeles virtuales o más y otro para presentaciones más estrechas o aquellas en las que una aplicación en estado
            // Snapped reduce el espacio horizontal disponible a menos de 1366 píxeles.
            if (viewState == ApplicationViewState.Filled ||
                viewState == ApplicationViewState.FullScreenLandscape)
            {
                var windowWidth = Window.Current.Bounds.Width;
                if (windowWidth >= 1366) return "FullScreenLandscapeOrWide";
                return "FilledOrNarrow";
            }

            // En las vistas Portrait o Snapped, comenzar con el nombre de estado visual predeterminado y, a continuación, agregar un
            // sufijo al ver los detalles en lugar de la lista
            var defaultStateName = base.DetermineVisualState(viewState);
            return logicalPageBack ? defaultStateName + "_Detail" : defaultStateName;
        }

        #endregion
    }
}
