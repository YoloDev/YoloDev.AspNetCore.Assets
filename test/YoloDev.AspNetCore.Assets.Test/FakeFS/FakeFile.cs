using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace YoloDev.AspNetCore.Assets.Test.FakeFS
{
  internal class FakeFile : IFileInfo
  {
    public static FakeFile From(FakeEntry entry, DateTime lastModified, string name)
    {
      if (entry == null)
        return new FakeFile(name, false, lastModified);

      if (entry.IsDir)
        return new FakeFile(name, true, lastModified);

      return new FakeFile(name, ((FakeEntry.FakeFile)entry).Content, lastModified);
    }

    static readonly Encoding _enc = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    readonly bool _exists;
    readonly bool _isDir;
    readonly string _name;
    readonly string _content;
    readonly DateTimeOffset _lastModified;

    public FakeFile(string name, bool isDir, DateTime lastModified)
    {
      _name = name;
      _exists = isDir;
      _isDir = isDir;
      _content = null;
      _lastModified = lastModified;
    }

    public FakeFile(string name, string content, DateTime lastModified)
    {
      _name = name;
      _exists = true;
      _isDir = false;
      _content = content;
      _lastModified = lastModified;
    }

    public bool Exists => _exists;

    public long Length => _content != null ? _enc.GetByteCount(_content) : -1;

    public string PhysicalPath => null;

    public string Name => _name;

    public DateTimeOffset LastModified => _lastModified;

    public bool IsDirectory => _isDir;

    public Stream CreateReadStream()
    {
      if (_content != null)
      {
        var bytes = _enc.GetBytes(_content);
        return new MemoryStream(bytes);
      }

      if (_isDir)
      {
        throw new InvalidOperationException("Cannot read directory");
      }

      throw new FileNotFoundException($"File {_name} not found");
    }
  }
}
