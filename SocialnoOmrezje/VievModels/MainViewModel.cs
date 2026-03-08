using SocialnoOmrezje.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocialnoOmrezje.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Post? _selectedPost;

        public MainViewModel(User currentUser)
        {
            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            CurrentUser.Posts ??= new ObservableCollection<Post>();
            CurrentUser.Friends ??= new ObservableCollection<string>();

            foreach (var post in CurrentUser.Posts)
            {
                NormalizePost(post);
            }
        }

        public User CurrentUser { get; }

        public Post? SelectedPost
        {
            get => _selectedPost;
            set
            {
                if (_selectedPost == value)
                {
                    return;
                }

                _selectedPost = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanRemovePost));
                OnPropertyChanged(nameof(CanEditPost));
            }
        }

        public bool CanRemovePost => SelectedPost != null;

        public bool CanEditPost => SelectedPost != null;

        public void RemovePost()
        {
            if (SelectedPost == null)
            {
                return;
            }

            CurrentUser.Posts.Remove(SelectedPost);
            SelectedPost = null;
        }

        public void ApplyUserData(User source)
        {
            ArgumentNullException.ThrowIfNull(source);

            CurrentUser.Name = source.Name;
            CurrentUser.Bio = source.Bio;
            CurrentUser.ProfileImage = source.ProfileImage;

            ReplacePosts(source.Posts);
            ReplaceFriends(source.Friends);

            SelectedPost = null;
        }

        private void ReplacePosts(IEnumerable<Post>? incomingPosts)
        {
            CurrentUser.Posts.Clear();

            if (incomingPosts == null)
            {
                return;
            }

            foreach (var post in incomingPosts)
            {
                CurrentUser.Posts.Add(ClonePost(post));
            }
        }

        private void ReplaceFriends(IEnumerable<string>? incomingFriends)
        {
            CurrentUser.Friends.Clear();

            if (incomingFriends == null)
            {
                return;
            }

            foreach (var friend in incomingFriends)
            {
                if (!string.IsNullOrWhiteSpace(friend))
                {
                    CurrentUser.Friends.Add(friend.Trim());
                }
            }
        }

        private static Post ClonePost(Post source)
        {
            var copy = new Post
            {
                Content = source.Content,
                ImagePath = source.ImagePath,
                CreatedAt = source.CreatedAt,
                Location = source.Location,
                Likes = source.Likes
            };

            if (source.TaggedFriends != null)
            {
                foreach (var friend in source.TaggedFriends)
                {
                    if (!string.IsNullOrWhiteSpace(friend))
                    {
                        copy.TaggedFriends.Add(friend.Trim());
                    }
                }
            }

            return copy;
        }

        private static void NormalizePost(Post post)
        {
            post.TaggedFriends ??= new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
