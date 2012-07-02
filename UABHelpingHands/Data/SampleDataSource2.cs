using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace UABHelpingHands.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon2 : UABHelpingHands.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon2(String uniqueId, String title, String apellido, String imagePath, String description,
                                int lloros, int rep, String NIU)
        {
            this._uniqueId = uniqueId;
            this._nombre = title;
            this._apellido = apellido;
            this._description = description;
            this._imagePath = imagePath;
            this._lloros = lloros;
            this._reputacion = rep;
            this._NIU = NIU;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _nombre = string.Empty;
        public string Title
        {
            get { return this._nombre; }
            set { this.SetProperty(ref this._nombre, value); }
        }

        private string _apellido = string.Empty;
        public string Subtitle
        {
            get { return this._apellido; }
            set { this.SetProperty(ref this._apellido, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
       
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon2._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        // TODO
        private int _lloros = 0;
        public int Lloros
        {
            get { return this._lloros; }
            set { this.SetProperty(ref this._lloros, value); }
        }
        private int _reputacion = 0;
        public int Reputacion
        {
            get { return this._reputacion; }
            set { this.SetProperty(ref this._reputacion, value); }
        }
        private String _NIU = "";
        public string NIU
        {
            get { return this._NIU; }
            set { this.SetProperty(ref this._NIU, value); }
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem2 : SampleDataCommon2
    {
        public SampleDataItem2(String uniqueId, String title, String subtitle, String imagePath, String description,int lloros,int rep,String NIU, String content, SampleDataGroup2 group)
            : base(uniqueId, title, subtitle, imagePath, description, lloros, rep, NIU)
        {
            this._content = content;
            this._group = group;
            
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup2 _group;
        public SampleDataGroup2 Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup2 : SampleDataCommon2
    {
        public SampleDataGroup2(String uniqueId, String title, String subtitle, String imagePath, String description,
                                int lloros, int rep, String niu)
            : base(uniqueId, title, subtitle, imagePath, description, lloros, rep, niu)
        {
        }

        private ObservableCollection<SampleDataItem2> _items = new ObservableCollection<SampleDataItem2>();
        public ObservableCollection<SampleDataItem2> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem2> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource2
    {
        private static SampleDataSource2 _sampleDataSource = new SampleDataSource2();

        private ObservableCollection<SampleDataGroup2> _allGroups = new ObservableCollection<SampleDataGroup2>();
        public ObservableCollection<SampleDataGroup2> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup2> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup2 GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem2 GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource2()
        {
            String ITEM_CONTENT = String.Format("DETALLES: ");

            var group1 = new SampleDataGroup2("Redes",
                    "Xarxes de Computadors",
                    "",
                    "Assets/xarxes.jpg",
                    "Group Description: ", 0, 0, "");
			
            group1.Items.Add(new SampleDataItem2("Group-1-Item-1",
                    "Nombre: Carlos Sobera",
                    "Especialidades: Redes, IA.",
                    "Assets/Carlos.jpg"," Está en 4º curso de Ingeniería Informática. Ofrece su ayuda a todo el que lo necesite.",5,2000,"1",
                    ITEM_CONTENT,
                    group1));



            group1.Items.Add(new SampleDataItem2("Group-1-Item-2",
                    "Wally Buscando",
                    "Especialidades: Lógica Computacional, IA, Algoritmos de Programación.",
                    "Assets/wally.jpg",
                    "Descripción: Una persona cuya calidad principal es la seriedad. Es fácil encontrárlo, debido a la gran disponibilidad. Ha alcanzado 4º curso de Ingeniería Informatica.",
                    100,1600,"2",
                    ITEM_CONTENT,
                    group1));

            group1.Items.Add(new SampleDataItem2("Group-1-Item-3",
                    "Rajoy President",
                    "Especialidades: Economia de la Empresa, Legislación Informatica, Ética para Ingenieros.",
                    "Assets/rajoy.jpg",
                    "Descripción: A pesar de lo que pueda parecer, lo que mejor se le da es la economia y la legislación.",
                    300,700,"3",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem2("Group-1-Item-4",
                    "Napoleón Bonaparte",
                    "Especialidades: Fundamentos de Computadores, Métodos de Cálculo Numérico, Herramientas de Cálculo Simbólico.",
                    "Assets/napoleon.jpeg",
                    "Descripción: Alumnno de 3er curso, con matrícula de honor en sus especialidades.",
                    1000,654,"4",
                    ITEM_CONTENT,
                    group1));
            this.AllGroups.Add(group1);
        }
    }
}
