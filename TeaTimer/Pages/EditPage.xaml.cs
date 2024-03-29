﻿using com.mahonkin.tim.maui.TeaTimer.ViewModels;
using Microsoft.Maui.Controls;

namespace com.mahonkin.tim.maui.TeaTimer.Pages;

/// <inheritdoc cref="ContentPage" />
public partial class EditPage : ContentPage
{
    #region Constructors
    /// <summary>
    /// EditPage constructor.
    /// </summary>
    public EditPage(EditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    #endregion Constructors
}
