using FakeItEasy;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using System;
using System.Collections.Generic;
using System.IO;

namespace YoloDev.AspNetCore.Assets.Test
{
  internal class TestableHtmlGenerator : DefaultHtmlGenerator
  {
    private readonly IDictionary<string, object> _validationAttributes;

    public TestableHtmlGenerator(IModelMetadataProvider metadataProvider)
      : this(metadataProvider, A.Fake<IUrlHelper>())
    { }

    public TestableHtmlGenerator(IModelMetadataProvider metadataProvider, IUrlHelper urlHelper)
      : this(
          metadataProvider,
          GetOptions(),
          urlHelper,
          validationAttributes: new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase))
    { }

    public TestableHtmlGenerator(
      IModelMetadataProvider metadataProvider,
      IOptions<MvcViewOptions> options,
      IUrlHelper urlHelper,
      IDictionary<string, object> validationAttributes)
      : base(
          A.Fake<IAntiforgery>(),
          options,
          metadataProvider,
          CreateUrlHelperFactory(urlHelper),
          new HtmlTestEncoder(),
          new DefaultValidationHtmlAttributeProvider(options, metadataProvider, new ClientValidatorCache()))
    {
      _validationAttributes = validationAttributes;
    }

    public IDictionary<string, object> ValidationAttributes => _validationAttributes;

    public static ViewContext GetViewContext(
      object model,
      IHtmlGenerator htmlGenerator,
      IModelMetadataProvider metadataProvider)
    {
      return GetViewContext(model, htmlGenerator, metadataProvider, modelState: new ModelStateDictionary());
    }

    public static ViewContext GetViewContext(
      object model,
      IHtmlGenerator htmlGenerator,
      IModelMetadataProvider metadataProvider,
      ModelStateDictionary modelState)
    {
      var actionContext = new ActionContext(
        new DefaultHttpContext(),
        new RouteData(),
        new ActionDescriptor(),
        modelState);

      var viewData = new ViewDataDictionary(metadataProvider, modelState)
      {
        Model = model
      };

      var viewContext = new ViewContext(
        actionContext,
        A.Fake<IView>(),
        viewData,
        A.Fake<ITempDataDictionary>(),
        TextWriter.Null,
        new HtmlHelperOptions());

      return viewContext;
    }

    public override IHtmlContent GenerateAntiforgery(ViewContext viewContext)
    {
      var tagBuilder = new TagBuilder("input")
      {
        Attributes =
        {
          { "name", "__RequestVerificationToken" },
          { "type", "hidden" },
          { "value", "olJlUDjrouRNWLen4tQJhauj1Z1rrvnb3QD65cmQU1Ykqi6S4" }, // 50 chars of a token.
        },

        TagRenderMode = TagRenderMode.SelfClosing,
      };

      return tagBuilder;
    }

    protected override void AddValidationAttributes(
      ViewContext viewContext,
      TagBuilder tagBuilder,
      ModelExplorer modelExplorer,
      string expression)
    {
      tagBuilder.MergeAttributes(ValidationAttributes);
    }

    private static IOptions<MvcViewOptions> GetOptions()
    {
      var options = A.Fake<IOptions<MvcViewOptions>>();
      A.CallTo(() => options.Value).Returns(new MvcViewOptions());
      return options;
    }

    private static IUrlHelperFactory CreateUrlHelperFactory(IUrlHelper urlHelper)
    {
      var factory = A.Fake<IUrlHelperFactory>();
      A.CallTo(() => factory.GetUrlHelper(A<ActionContext>.Ignored))
        .Returns(urlHelper);
      return factory;
    }
  }
}
