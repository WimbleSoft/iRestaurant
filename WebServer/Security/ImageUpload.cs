using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace iRestaurant.Security
{
    /// <summary>
    /// Validates and stores uploaded images. Previously the original file name or the
    /// client-supplied content type was used to build the path, which allowed uploading
    /// executable files (e.g. "shell.aspx") or writing outside the folder.
    /// Now only real JPEG/PNG/GIF images are accepted and the server chooses the file name.
    /// </summary>
    public static class ImageUpload
    {
        public const int MaxBytes = 5 * 1024 * 1024;

        private static readonly Dictionary<string, byte[][]> Signatures = new Dictionary<string, byte[][]>(StringComparer.OrdinalIgnoreCase)
        {
            { ".jpg", new[] { new byte[] { 0xFF, 0xD8, 0xFF } } },
            { ".jpeg", new[] { new byte[] { 0xFF, 0xD8, 0xFF } } },
            { ".png", new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".gif", new[] { new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 } } },
        };

        public static bool HasFile(HttpPostedFileBase file) => file != null && file.ContentLength > 0;

        /// <summary>True when there is no file, or the file is an acceptable image.</summary>
        public static bool IsEmptyOrAcceptable(HttpPostedFileBase file)
        {
            if (!HasFile(file))
                return true;
            string extension = Path.GetExtension(Path.GetFileName(file.FileName) ?? string.Empty);
            byte[][] signatures;
            return file.ContentLength <= MaxBytes && Signatures.TryGetValue(extension, out signatures) && StartsWithAny(file.InputStream, signatures);
        }

        /// <summary>
        /// Saves the image as "&lt;baseName&gt;&lt;ext&gt;" inside <paramref name="virtualFolder"/>.
        /// Returns the stored file name, or null when the upload is not an acceptable image.
        /// </summary>
        public static string Save(HttpPostedFileBase file, HttpServerUtilityBase server, string virtualFolder, string baseName)
        {
            if (!HasFile(file) || file.ContentLength > MaxBytes)
                return null;

            string extension = Path.GetExtension(Path.GetFileName(file.FileName) ?? string.Empty).ToLowerInvariant();
            byte[][] signatures;
            if (!Signatures.TryGetValue(extension, out signatures) || !StartsWithAny(file.InputStream, signatures))
                return null;

            if (extension == ".jpeg")
                extension = ".jpg";

            string safeBaseName = Sanitize(baseName);
            string folder = server.MapPath(virtualFolder);
            string fileName = safeBaseName + extension;
            string fullPath = Path.GetFullPath(Path.Combine(folder, fileName));
            if (!fullPath.StartsWith(Path.GetFullPath(folder), StringComparison.OrdinalIgnoreCase))
                return null;

            Directory.CreateDirectory(folder);
            file.InputStream.Position = 0;
            file.SaveAs(fullPath);
            return fileName;
        }

        /// <summary>Deletes a stored image, refusing anything that is not a plain file name in the folder.</summary>
        public static void Delete(HttpServerUtilityBase server, string virtualFolder, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName != Path.GetFileName(fileName) || fileName.StartsWith("no-image", StringComparison.OrdinalIgnoreCase))
                return;
            string path = Path.Combine(server.MapPath(virtualFolder), fileName);
            if (File.Exists(path))
                File.Delete(path);
        }

        private static string Sanitize(string baseName)
        {
            var chars = new List<char>();
            foreach (char c in baseName ?? string.Empty)
            {
                if (char.IsLetterOrDigit(c) || c == '-' || c == '_')
                    chars.Add(c);
            }
            return chars.Count == 0 ? SecureRandom.HexToken(8) : new string(chars.ToArray());
        }

        private static bool StartsWithAny(Stream stream, byte[][] signatures)
        {
            var header = new byte[8];
            stream.Position = 0;
            int read = stream.Read(header, 0, header.Length);
            stream.Position = 0;
            foreach (byte[] signature in signatures)
            {
                if (read < signature.Length)
                    continue;
                bool match = true;
                for (int i = 0; i < signature.Length && match; i++)
                    match = header[i] == signature[i];
                if (match)
                    return true;
            }
            return false;
        }
    }
}
