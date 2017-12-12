﻿using GiftTrackerClasses;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GiftTracker
{
    public partial class AddOrEditPersonWindow : Window, INotifyPropertyChanged
    {
        Person CurrentPerson { get; set; }
        Ellipse SelectedDefaultImage { get; set; }
        string CurrentImage { get; set; }
        GTRepository<Person> PeopleRepository { get; set; }
        bool IsEdited { get; set; }
        public AddOrEditPersonWindow(GTContext context, Person person = null)
        {
            InitializeComponent();
            PeopleRepository = new GTRepository<Person>(context);
            CurrentPerson = person;
            
            if (CurrentPerson == null)
            {
                IsEdited = false;
                CurrentPerson = new Person();
                userImageItems.ItemsSource = Directory.EnumerateFiles(@"..\..\Images\DefaultUserImages", "*.png");
                userImageItems.SelectedIndex = 0;
                userImageItems.Focus();
            } else
            {
                IsEdited = true;
            }

            this.DataContext = CurrentPerson;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = OpenFileDialogHelper.OpenImageFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                userImageItems.UnselectAll();
                string fileName = dialog.FileName;
                userImage.DataContext = fileName;
                CurrentImage = fileName;
            }
        }

        private void UserImageItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (userImageItems.SelectedItems.Count != 0)
            {
                CurrentImage = userImageItems.SelectedItem.ToString();
                userImage.DataContext = CurrentImage;
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPerson.Image = ImageHelper.BitmapSourceToByteArray(CurrentImage);
            if (IsEdited)
            {
                PeopleRepository.Update(CurrentPerson);
            } else
            {
                PeopleRepository.Add(CurrentPerson);
            }
            MessageBox.Show("Saved!");

            this.Close();
        }
    }
}
