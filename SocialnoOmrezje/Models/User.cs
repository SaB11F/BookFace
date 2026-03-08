using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocialnoOmrezje.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _name;
        private string _bio;
        private string _profileImage;

        public User()
        {
            _name = string.Empty;
            _bio = string.Empty;
            _profileImage = string.Empty;
            Posts = new ObservableCollection<Post>();
            Friends = new ObservableCollection<string>();
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged();
            }
        }

        public string Bio
        {
            get => _bio;
            set
            {
                if (_bio == value)
                {
                    return;
                }

                _bio = value;
                OnPropertyChanged();
            }
        }

        public string ProfileImage
        {
            get => _profileImage;
            set
            {
                if (_profileImage == value)
                {
                    return;
                }

                _profileImage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Post> Posts { get; set; }

        public ObservableCollection<string> Friends { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
