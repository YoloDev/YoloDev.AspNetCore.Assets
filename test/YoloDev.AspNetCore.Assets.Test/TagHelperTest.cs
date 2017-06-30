using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace YoloDev.AspNetCore.Assets.Test
{
  public class TagHelperTest
  {
    [Fact]
    public async Task ProcessAsync_GeneratesExpectedOutput()
    {
      // arrange
      var metadataProvider = new TestModelMetadataProvider();

      var tagHelperContext = new TagHelperContext(
        allAttributes: new TagHelperAttributeList
        {
          { "id", "my-asset" },
          { "src", "~/assets/in.js" }
        },
        items: new Dictionary<object, object>(),
        uniqueId: "test");

      var output = new TagHelperOutput(
        "asset",
        attributes: new TagHelperAttributeList
        {
          { "id", "my-asset" }
        },
        getChildContentAsync: (useCachedResult, encoder) =>
        {
          var tagHelperContent = new DefaultTagHelperContent();
          return Task.FromResult<TagHelperContent>(tagHelperContent);
        });

      var service = A.Fake<IAssetService>();
      A.CallTo(() => service.GetAsset(new PathString("/assets/in.js"), A<HttpContext>.Ignored))
        .Returns(new PathString("/root/assets/out.js"));

      var htmlGenerator = new TestableHtmlGenerator(metadataProvider);
      var viewContext = TestableHtmlGenerator.GetViewContext(
        model: null,
        htmlGenerator: htmlGenerator,
        metadataProvider: metadataProvider);
      var assetTagHelper = new AssetTagHelper(service)
      {
        Source = "~/assets/in.js",
        ViewContext = viewContext
      };

      // Act
      await assetTagHelper.ProcessAsync(tagHelperContext, output);

      // Assert
      Assert.Equal(3, output.Attributes.Count);
      var attribute = Assert.Single(output.Attributes, attr => attr.Name.Equals("id"));
      Assert.Equal("my-asset", attribute.Value);

      attribute = Assert.Single(output.Attributes, attr => attr.Name.Equals("type"));
      Assert.Equal("text/javascript", attribute.Value);

      attribute = Assert.Single(output.Attributes, attr => attr.Name.Equals("src"));
      Assert.Equal("/root/assets/out.js", attribute.Value);
      Assert.Equal("", output.Content.GetContent());
      Assert.Equal("script", output.TagName);

      A.CallTo(() => service.GetAsset(new PathString("/assets/in.js"), A<HttpContext>.Ignored))
        .MustHaveHappened();
    }
  }
}
