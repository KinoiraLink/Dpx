using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dpx.Models;
using Dpx.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dpx.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        //public static string GetRowValue()
        //{
        //    return Grid.GetRow();
        //    //return Grid.GetRow(gridBindableObject);
        //}

        public TestPage()
        {
            InitializeComponent();
            
            
        }

    }
}