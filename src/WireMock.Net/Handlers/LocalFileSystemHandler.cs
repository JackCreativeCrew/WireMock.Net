﻿using System.Collections.Generic;
using System.IO;
using WireMock.Validation;

namespace WireMock.Handlers
{
    /// <summary>
    /// Default implementation for a handler to interact with the local file system to read and write static mapping files.
    /// </summary>
    public class LocalFileSystemHandler : IFileSystemHandler
    {
        private static readonly string AdminMappingsFolder = Path.Combine("__admin", "mappings");

        /// <inheritdoc cref="IFileSystemHandler.FolderExists"/>
        public bool FolderExists(string path)
        {
            Check.NotNullOrEmpty(path, nameof(path));

            return Directory.Exists(path);
        }

        /// <inheritdoc cref="IFileSystemHandler.CreateFolder"/>
        public void CreateFolder(string path)
        {
            Check.NotNullOrEmpty(path, nameof(path));

            Directory.CreateDirectory(path);
        }

        /// <inheritdoc cref="IFileSystemHandler.EnumerateFiles"/>
        public IEnumerable<string> EnumerateFiles(string path)
        {
            Check.NotNullOrEmpty(path, nameof(path));

            return Directory.EnumerateFiles(path);
        }

        /// <inheritdoc cref="IFileSystemHandler.GetMappingFolder"/>
        public string GetMappingFolder()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), AdminMappingsFolder);
        }

        /// <inheritdoc cref="IFileSystemHandler.ReadMappingFile"/>
        public string ReadMappingFile(string path)
        {
            Check.NotNullOrEmpty(path, nameof(path));

            return File.ReadAllText(path);
        }

        /// <inheritdoc cref="IFileSystemHandler.WriteMappingFile(string, string)"/>
        public void WriteMappingFile(string path, string text)
        {
            Check.NotNullOrEmpty(path, nameof(path));
            Check.NotNull(text, nameof(text));

            File.WriteAllText(path, text);
        }

        /// <inheritdoc cref="IFileSystemHandler.ReadResponseBodyAsFile"/>
        public byte[] ReadResponseBodyAsFile(string path)
        {
            Check.NotNullOrEmpty(path, nameof(path));

            // In case the path is a filename, the path will be adjusted to the MappingFolder.
            // Else the path will just be as-is.
            return File.ReadAllBytes(Path.GetFileName(path) == path ? Path.Combine(GetMappingFolder(), path) : path);
        }

        /// <inheritdoc cref="IFileSystemHandler.FileExists"/>
        public bool FileExists(string path)
        {
            Check.NotNullOrEmpty(path, nameof(path));

            return File.Exists(AdjustPath(path));
        }

        /// <inheritdoc cref="IFileSystemHandler.WriteFile(string, byte[])"/>
        public void WriteFile(string filename, byte[] bytes)
        {
            Check.NotNullOrEmpty(filename, nameof(filename));
            Check.NotNull(bytes, nameof(bytes));

            File.WriteAllBytes(AdjustPath(filename), bytes);
        }

        /// <inheritdoc cref="IFileSystemHandler.DeleteFile"/>
        public void DeleteFile(string fileName)
        {
            Check.NotNullOrEmpty(fileName, nameof(fileName));

            File.Delete(AdjustPath(fileName));
        }

        /// <inheritdoc cref="IFileSystemHandler.ReadFile"/>
        public byte[] ReadFile(string filename)
        {
            Check.NotNullOrEmpty(filename, nameof(filename));

            return File.ReadAllBytes(AdjustPath(filename));
        }

        /// <summary>
        /// Adjusts the path to the MappingFolder.
        /// </summary>
        /// <param name="filename">The path.</param>
        /// <returns>Adjusted path</returns>
        private string AdjustPath(string filename)
        {
            return Path.Combine(GetMappingFolder(), filename);
        }
    }
}
