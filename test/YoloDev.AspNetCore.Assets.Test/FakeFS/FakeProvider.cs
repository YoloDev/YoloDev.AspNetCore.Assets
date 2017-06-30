using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace YoloDev.AspNetCore.Assets.Test.FakeFS
{
  internal class FakeProvider : IFileProvider
  {
    public static IFileProvider Create(params FakeEntry[] entries)
    {
      return new FakeProvider(entries);
    }

    readonly FakeEntry.FakeDir _root;
    readonly DateTime _created = DateTime.Now;

    private FakeProvider(FakeEntry[] entries)
    {
        _root = FakeEntry.Dir("<root>", entries);
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
      var entry = Find(subpath, out string name);
      return FakeDir.From(entry as FakeEntry.FakeDir, _created);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
      var entry = Find(subpath, out string name);
      return FakeFile.From(entry, _created, name);
    }

    public IChangeToken Watch(string filter)
    {
      throw new NotSupportedException();
    }

    private FakeEntry Find(string subpath, out string name)
    {
      var segments = subpath.Split(new[] { '/', '\\' });
      name = segments[segments.Length - 1];
      var current = _root;
      FakeEntry last = null;
      foreach (var segment in segments)
      {
        if (current == null)
          return null;

        if (!current.TryFind(segment, out last))
          return null;

        if (last is FakeEntry.FakeDir dir)
          current = dir;
        else
          current = null;
      }

      return last;
    }
  }
}
