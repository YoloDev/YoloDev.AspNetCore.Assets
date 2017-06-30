using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace YoloDev.AspNetCore.Assets.Test
{
  public class AssetPathNormalizerTest
  {
    private static readonly PathString InputPath = new PathString("/Mixed/Case/Path.js");
    private static readonly PathString ExpectedPath = new PathString("/mixed/case/path.js");

    [Fact]
    public void ReturnsLowerCaseVariantOfPath()
    {
      var sut = new AssetPathNormalizer();
      var result = sut.Normalize(InputPath);
      Assert.Equal(ExpectedPath, result);
    }
  }
}
