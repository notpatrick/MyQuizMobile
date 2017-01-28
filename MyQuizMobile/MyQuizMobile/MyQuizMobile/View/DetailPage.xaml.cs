﻿using MyQuizMobile.Model;
using MyQuizMobile.ViewModel;

using Xamarin.Forms;

namespace MyQuizMobile.View
{
    public partial class DetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;
        public DetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

            this.viewModel.OnFinished += OnFinished;
        }

        async void OnFinished(Item item)
        {
            viewModel.OnFinished -= OnFinished;
            await Navigation.PopAsync();
        }
    }
}
