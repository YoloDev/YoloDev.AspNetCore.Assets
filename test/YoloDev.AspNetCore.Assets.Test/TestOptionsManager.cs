using Microsoft.Extensions.Options;
using System.Linq;

namespace YoloDev.AspNetCore.Assets.Test
{
  internal class TestOptionsManager<T> : OptionsManager<T>
    where T : class, new()
  {
    public TestOptionsManager()
      : base(Enumerable.Empty<IConfigureOptions<T>>())
    {
    }
  }
}