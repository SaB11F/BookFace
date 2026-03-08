using Microsoft.Win32;
using SocialnoOmrezje.Services;
using SocialnoOmrezje.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SocialnoOmrezje.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddPost_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
            {
                return;
            }

            var window = new AddPostWindow
            {
                Owner = this
            };

            if (window.ShowDialog() == true && window.NewPost != null)
            {
                vm.CurrentUser.Posts.Add(window.NewPost);
                SaveState(vm);
            }
        }

        private void EditPost_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm || vm.SelectedPost == null)
            {
                MessageBox.Show("Izberi objavo, ki jo zelis urediti.");
                return;
            }

            var window = new AddPostWindow(vm.SelectedPost)
            {
                Owner = this
            };

            if (window.ShowDialog() == true)
            {
                SaveState(vm);
            }
        }

        private void RemovePost_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.RemovePost();
                SaveState(vm);
            }
        }

        private void PostsListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.SelectedPost != null)
            {
                MessageBox.Show(
                    vm.SelectedPost.Content,
                    "Vsebina objave",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void OpenFriendsWindow_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
            {
                return;
            }

            var window = new FriendsWindow(vm.CurrentUser.Friends)
            {
                Owner = this
            };

            window.ShowDialog();
            RefreshFriendFilter();
            SaveState(vm);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
            => Close();

        private void ImportXml_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
            {
                return;
            }

            var dialog = new OpenFileDialog
            {
                Filter = "XML datoteke (*.xml)|*.xml",
                Title = "Uvozi podatke"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            try
            {
                var loadedUser = XmlDataService.Load(dialog.FileName);
                if (loadedUser == null)
                {
                    MessageBox.Show("Datoteka ne vsebuje veljavnih podatkov uporabnika.");
                    return;
                }

                vm.ApplyUserData(loadedUser);
                RefreshFriendFilter();
                SaveState(vm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Napaka pri uvozu XML datoteke.\n\n{ex}");
            }
        }

        private void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
            {
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "XML datoteke (*.xml)|*.xml",
                Title = "Izvozi podatke",
                FileName = "socialno-omrezje-export.xml"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            try
            {
                XmlDataService.Save(vm.CurrentUser, dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Napaka pri izvozu XML datoteke.\n\n{ex}");
            }
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
            {
                return;
            }

            try
            {
                SaveState(vm);
                MessageBox.Show("Podatki uporabnika so bili shranjeni.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Napaka pri shranjevanju podatkov uporabnika.\n\n{ex}");
            }
        }

        private void SelectProfileImage_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainViewModel vm)
            {
                return;
            }

            var dialog = new OpenFileDialog
            {
                Filter = "Slike (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (dialog.ShowDialog() == true)
            {
                vm.CurrentUser.ProfileImage = dialog.FileName;
                SaveState(vm);
            }
        }

        private void FriendSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
            => RefreshFriendFilter();

        private void RefreshFriendFilter()
        {
            if (DataContext is not MainViewModel vm)
            {
                return;
            }

            var search = FriendSearchTextBox.Text.Trim();
            var view = CollectionViewSource.GetDefaultView(vm.CurrentUser.Friends);
            if (view == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(search))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = item =>
                    item is string friend
                    && friend.Contains(search, StringComparison.OrdinalIgnoreCase);
            }

            view.Refresh();
        }

        private static void SaveState(MainViewModel vm)
        {
            XmlDataService.Save(vm.CurrentUser);
            UserSettingsService.Save(vm.CurrentUser);
        }
    }
}
