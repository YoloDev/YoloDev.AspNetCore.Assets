using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;

namespace YoloDev.AspNetCore.Assets.Test.FakeFS
{
  internal class FakeDir : IDirectoryContents
  {
    private static FakeDir NonExisting = new FakeDir();

    internal static FakeDir From(FakeEntry.FakeDir entry, DateTime created)
    {
      if (entry == null)
        return NonExisting;

      return new FakeDir(entry.Entries.Select(e => FakeFile.From(e, created, e.Name)));
    }

    readonly bool _exists;
    readonly IEnumerable<IFileInfo> _content;

    private FakeDir()
    {
      _exists = false;
      _content = Enumerable.Empty<IFileInfo>();
    }

    public FakeDir(IEnumerable<IFileInfo> files)
    {
      _exists = true;
      _content = files;
    }

    public bool Exists => _exists;

    public IEnumerator<IFileInfo> GetEnumerator() =>
      _content.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
      _content.GetEnumerator();
  }
}
