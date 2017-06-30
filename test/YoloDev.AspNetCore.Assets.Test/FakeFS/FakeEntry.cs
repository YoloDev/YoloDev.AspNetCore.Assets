using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace YoloDev.AspNetCore.Assets.Test.FakeFS
{
  internal abstract class FakeEntry
  {
    public static FakeEntry.FakeFile File(string name, string content) =>
      new FakeFile(name, content);

    public static FakeEntry.FakeDir Dir(string name, params FakeEntry[] entries) =>
      new FakeDir(name, entries.ToImmutableList());

    readonly string _name;

    private FakeEntry(string name)
    {
      _name = name;
    }

    public string Name => _name;

    public bool IsDir => this is FakeDir;

    internal sealed class FakeDir : FakeEntry
    {
      readonly IImmutableList<FakeEntry> _entries;

      public FakeDir(string name, IImmutableList<FakeEntry> entries)
        : base(name)
      {
        _entries = entries;
      }

      public IEnumerable<FakeEntry> Entries => _entries;

      public bool TryFind(string name, out FakeEntry entry)
      {
        entry = _entries
          .Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
          .FirstOrDefault();

        return entry != null;
      }
    }

    internal sealed class FakeFile : FakeEntry
    {
      readonly string _content;

      public FakeFile(string name, string content)
        : base(name)
      {
        _content = content;
      }

      public string Content => _content;
    }
  }
}
