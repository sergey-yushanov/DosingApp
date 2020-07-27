using DosingApp.Models;
using DosingApp.Services;
using DosingApp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DosingApp.Views
{
    public partial class FieldPage : ContentPage
    {
        //public FieldViewModel FieldViewModel { get; private set; }
        
        public FieldPage(FieldViewModel fieldViewModel)
        {
            InitializeComponent();

            //FieldViewModel = fieldViewModel;
            //this.BindingContext = FieldViewModel;
        }
    }
}