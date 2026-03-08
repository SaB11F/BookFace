using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SocialnoOmrezje.Views
{
    public partial class FriendsWindow : Window
    {
        private readonly ObservableCollection<string> _friends;

        public FriendsWindow(ObservableCollection<string> friends)
        {
            InitializeComponent();

            _friends = friends ?? throw new ArgumentNullException(nameof(friends));
            FriendsListBox.ItemsSource = _friends;
        }

        private void AddFriend_Click(object sender, RoutedEventArgs e)
        {
            var name = FriendNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vnesi ime prijatelja.");
                return;
            }

            if (_friends.Any(friend => string.Equals(friend, name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Ta prijatelj je ze na seznamu.");
                return;
            }

            _friends.Add(name);
            FriendNameTextBox.Clear();
        }

        private void UpdateFriend_Click(object sender, RoutedEventArgs e)
        {
            if (FriendsListBox.SelectedItem is not string selected)
            {
                MessageBox.Show("Najprej izberi prijatelja za urejanje.");
                return;
            }

            var updated = FriendNameTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(updated))
            {
                MessageBox.Show("Vnesi novo ime prijatelja.");
                return;
            }

            if (_friends.Any(friend =>
                !string.Equals(friend, selected, StringComparison.OrdinalIgnoreCase)
                && string.Equals(friend, updated, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Prijatelj s tem imenom ze obstaja.");
                return;
            }

            var index = _friends.IndexOf(selected);
            if (index >= 0)
            {
                _friends[index] = updated;
            }
        }

        private void RemoveFriend_Click(object sender, RoutedEventArgs e)
        {
            if (FriendsListBox.SelectedItem is not string selected)
            {
                MessageBox.Show("Najprej izberi prijatelja za odstranitev.");
                return;
            }

            _friends.Remove(selected);
            FriendNameTextBox.Clear();
        }

        private void FriendsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FriendsListBox.SelectedItem is string selected)
            {
                FriendNameTextBox.Text = selected;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}
