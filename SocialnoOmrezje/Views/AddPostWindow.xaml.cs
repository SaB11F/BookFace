using Microsoft.Win32;
using SocialnoOmrezje.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SocialnoOmrezje.Views
{
    public partial class AddPostWindow : Window
    {
        private readonly Post? _editingPost;

        public Post? NewPost { get; private set; }

        public AddPostWindow(Post? postToEdit = null)
        {
            InitializeComponent();

            _editingPost = postToEdit;
            if (_editingPost != null)
            {
                LoadPost(_editingPost);
                Title = "Uredi objavo";
                ConfirmButton.Content = "Shrani";
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
            => SelectImage();

        private void PreviewImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => SelectImage();

        private void SelectImage()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Slike (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (dialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = dialog.FileName;
                PreviewImage.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ContentTextBox.Text))
            {
                MessageBox.Show("Vsebina ne sme biti prazna.");
                return;
            }

            if (!int.TryParse(LikesTextBox.Text, out var likes))
            {
                likes = 0;
            }

            var targetPost = _editingPost ?? new Post { CreatedAt = DateTime.Now };

            targetPost.Content = ContentTextBox.Text.Trim();
            targetPost.Location = LocationTextBox.Text.Trim();
            targetPost.Likes = likes;
            targetPost.ImagePath = ImagePathTextBox.Text.Trim();

            targetPost.TaggedFriends.Clear();
            var friends = TaggedFriendsTextBox.Text
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(friend => friend.Trim())
                .Where(friend => !string.IsNullOrWhiteSpace(friend));

            foreach (var friend in friends)
            {
                targetPost.TaggedFriends.Add(friend);
            }

            NewPost = targetPost;
            DialogResult = true;
            Close();
        }

        private void LoadPost(Post post)
        {
            ContentTextBox.Text = post.Content;
            LocationTextBox.Text = post.Location;
            LikesTextBox.Text = post.Likes.ToString();
            ImagePathTextBox.Text = post.ImagePath;
            TaggedFriendsTextBox.Text = string.Join(", ", post.TaggedFriends);

            if (!string.IsNullOrWhiteSpace(post.ImagePath))
            {
                try
                {
                    PreviewImage.Source = new BitmapImage(new Uri(post.ImagePath));
                }
                catch
                {
                    PreviewImage.Source = null;
                }
            }
        }
    }
}
