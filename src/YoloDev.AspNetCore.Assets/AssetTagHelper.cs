using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace YoloDev.AspNetCore.Assets
{
  [HtmlTargetElement("asset", TagStructure = TagStructure.NormalOrSelfClosing)]
  public class AssetTagHelper : ITagHelper
  {
    private static readonly char[] ValidAttributeWhitespaceChars =
      new[] { '\t', '\n', '\u000C', '\r', ' ' };

    private readonly IAssetService _asset;

    public AssetTagHelper(
      IAssetService asset)
    {
      _asset = asset;
    }

    [HtmlAttributeName("type")]
    public AssetType Type { get; set; } = AssetType.Script;

    [HtmlAttributeName("src")]
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ViewContext"/> for the current request.
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public int Order => 0;

    public void Init(TagHelperContext context)
    {
    }

    public async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
      if (context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      if (output == null)
      {
        throw new ArgumentNullException(nameof(output));
      }

      string url;
      if (!TryResolveUrl(Source, out url))
      {
        throw new InvalidOperationException($"Cannot resolve url ${Source}.");
      }

      // At this point, url should always start with ~/
      var assetUrl = await _asset.GetAsset(new PathString(url.Substring(1)), ViewContext.HttpContext);

      switch (Type)
      {
        case AssetType.Script:
          output.TagName = "script";
          output.TagMode = TagMode.StartTagAndEndTag;
          OverwriteAttribute(output, "type", "text/javascript");
          OverwriteAttribute(output, "src", assetUrl);
          break;

        default:
          throw new InvalidOperationException("Invalid asset type provided to asset tag.");
      }
    }

    private static void OverwriteAttribute(TagHelperOutput output, string name, string value)
    {
      if (output.Attributes.ContainsName(name))
      {
        output.Attributes.Remove(output.Attributes[name]);
      }

      output.Attributes.Add(name, value);
    }

    private static bool TryResolveUrl(string url, out string resolvedUrl)
    {
      resolvedUrl = null;
      var start = FindRelativeStart(url);
      if (start == -1)
      {
        return false;
      }

      resolvedUrl = CreateTrimmedString(url, start);
      return true;
    }

    private static int FindRelativeStart(string url)
    {
      if (url == null || url.Length < 2)
      {
        return -1;
      }

      var maxTestLength = url.Length - 2;
      var start = 0;
      for (; start < url.Length; start++)
      {
        if (start > maxTestLength)
        {
          return -1;
        }

        if(!IsCharWhitespace(url[start]))
        {
          break;
        }
      }

      // Before doing more work, ensure that the URL we're looking at is app-relative.
      if (url[start] != '~' || url[start + 1] != '/')
      {
        return -1;
      }

      return start;
    }

    private static string CreateTrimmedString(string input, int start)
    {
      var end = input.Length - 1;
      for (; end >= start; end--)
      {
        if (!IsCharWhitespace(input[end]))
        {
          break;
        }
      }

      var len = end - start + 1;

      // Substring returns same string if start == 0 && len == Length
      return input.Substring(start, len);
    }

    private static bool IsCharWhitespace(char ch)
    {
      for (var i = 0; i < ValidAttributeWhitespaceChars.Length; i++)
      {
        if (ValidAttributeWhitespaceChars[i] == ch)
        {
          return true;
        }
      }

      // the character is not white space
      return false;
    }
  }
}
