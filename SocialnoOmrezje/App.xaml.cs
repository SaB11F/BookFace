using SocialnoOmrezje.Models;
using SocialnoOmrezje.Services;
using SocialnoOmrezje.ViewModels;
using SocialnoOmrezje.Views;
using System;
using System.IO;
using System.Windows;

namespace SocialnoOmrezje
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var defaultProfileImagePath = GetDefaultProfileImagePath();
            var currentUser = XmlDataService.Load() ?? CreateDefaultUser(defaultProfileImagePath);
            UserSettingsService.Apply(currentUser);
            EnsureProfileImage(currentUser, defaultProfileImagePath);

            var vm = new MainViewModel(currentUser);

            var mainWindow = new MainWindow
            {
                DataContext = vm
            };

            MainWindow = mainWindow;
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (MainWindow?.DataContext is MainViewModel vm)
            {
                try
                {
                    XmlDataService.Save(vm.CurrentUser);
                    UserSettingsService.Save(vm.CurrentUser);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Napaka pri shranjevanju podatkov v {XmlDataService.DataFilePath}.\n\n{ex}",
                        "Napaka pri shranjevanju",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }

            base.OnExit(e);
        }

        private static User CreateDefaultUser(string defaultImagePath)
        {
            var user = new User
            {
                Name = "Mark Zuckerberg",
                Bio = "Trying to make the world more open.",
                ProfileImage = File.Exists(defaultImagePath) ? defaultImagePath : string.Empty
            };

            user.Friends.Add("Ana Novak");
            user.Friends.Add("Luka Kranjc");
            user.Friends.Add("Maja Zupan");
            user.Friends.Add("Nina Rozman");

            var post1 = new Post
            {
                Content = "Prva objava na mojem novem zidu.",
                Location = "Ljubljana",
                Likes = 12,
                CreatedAt = DateTime.Now.AddDays(-2)
            };
            post1.TaggedFriends.Add("Ana Novak");

            var post2 = new Post
            {
                Content = "Danes sem končal vmesnik za SocialnoOmrezje!",
                Location = "Maribor",
                Likes = 31,
                CreatedAt = DateTime.Now.AddHours(-6)
            };  

            user.Posts.Add(post1);
            user.Posts.Add(post2);

            return user;
        }

        private static string GetDefaultProfileImagePath()
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "default-profile.png");

        private static void EnsureProfileImage(User user, string defaultProfileImagePath)
        {
            if (string.IsNullOrWhiteSpace(user.ProfileImage) && File.Exists(defaultProfileImagePath))
            {
                user.ProfileImage = defaultProfileImagePath;
            }
        }
    }
}
