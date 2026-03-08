using SocialnoOmrezje.Models;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SocialnoOmrezje.Services
{
    public static class XmlDataService
    {
        private static readonly string DefaultFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml");

        public static string DataFilePath => DefaultFilePath;

        public static void Save(User user)
            => Save(user, DefaultFilePath);

        public static void Save(User user, string filePath)
        {
            ArgumentNullException.ThrowIfNull(user);

            var resolvedPath = ResolvePath(filePath);
            EnsureDirectory(resolvedPath);

            var serializer = new XmlSerializer(typeof(User));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = new UTF8Encoding(false)
            };

            using var stream = new FileStream(resolvedPath, FileMode.Create, FileAccess.Write, FileShare.None);
            using var writer = XmlWriter.Create(stream, settings);
            serializer.Serialize(writer, user);
        }

        public static User? Load()
            => Load(DefaultFilePath);

        public static User? Load(string filePath)
        {
            var resolvedPath = ResolvePath(filePath);
            if (!File.Exists(resolvedPath))
            {
                return null;
            }

            var serializer = new XmlSerializer(typeof(User));
            using var stream = new FileStream(resolvedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return serializer.Deserialize(stream) as User;
        }

        private static string ResolvePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Datotečna pot ne sme biti prazna.", nameof(filePath));
            }

            return Path.IsPathRooted(filePath)
                ? filePath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
        }

        private static void EnsureDirectory(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
