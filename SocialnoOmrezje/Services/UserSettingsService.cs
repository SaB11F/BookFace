using SocialnoOmrezje.Models;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SocialnoOmrezje.Services
{
    public static class UserSettingsService
    {
        private static readonly string SettingsFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user-settings.xml");

        public static void Save(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var settings = new UserSettings
            {
                Name = user.Name,
                Bio = user.Bio,
                ProfileImage = user.ProfileImage
            };

            var serializer = new XmlSerializer(typeof(UserSettings));
            using var stream = new FileStream(SettingsFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            serializer.Serialize(stream, settings);
        }

        public static void Apply(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (!File.Exists(SettingsFilePath))
            {
                return;
            }

            var serializer = new XmlSerializer(typeof(UserSettings));
            using var stream = new FileStream(SettingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (serializer.Deserialize(stream) is not UserSettings settings)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(settings.Name))
            {
                user.Name = settings.Name;
            }

            user.Bio = settings.Bio ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(settings.ProfileImage))
            {
                user.ProfileImage = settings.ProfileImage;
            }
        }

        [Serializable]
        public class UserSettings
        {
            public string Name { get; set; } = string.Empty;

            public string Bio { get; set; } = string.Empty;

            public string ProfileImage { get; set; } = string.Empty;
        }
    }
}
