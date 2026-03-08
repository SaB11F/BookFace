using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocialnoOmrezje.Models
{
    public class Post : INotifyPropertyChanged
    {
        private string _content = string.Empty;
        private string _imagePath = string.Empty;
        private ObservableCollection<string> _taggedFriends;
        private DateTime _createdAt;
        private string _location = string.Empty;
        private int _likes;

        public Post()
        {
            _taggedFriends = new ObservableCollection<string>();
            _createdAt = DateTime.Now;
        }

        public string Content
        {
            get => _content;
            set
            {
                if (_content == value)
                {
                    return;
                }

                _content = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath == value)
                {
                    return;
                }

                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> TaggedFriends
        {
            get => _taggedFriends;
            set
            {
                if (_taggedFriends == value)
                {
                    return;
                }

                _taggedFriends = value ?? new ObservableCollection<string>();
                OnPropertyChanged();
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt == value)
                {
                    return;
                }

                _createdAt = value;
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                if (_location == value)
                {
                    return;
                }

                _location = value;
                OnPropertyChanged();
            }
        }

        public int Likes
        {
            get => _likes;
            set
            {
                if (_likes == value)
                {
                    return;
                }

                _likes = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
