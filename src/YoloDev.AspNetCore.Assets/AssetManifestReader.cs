using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace YoloDev.AspNetCore.Assets
{
  public class AssetManifestReader : IAssetManifestReader
  {
    private readonly JsonSerializer _serializer;

    public AssetManifestReader()
    {
      _serializer = JsonSerializer.Create();
    }

    public AssetManifestReader(JsonSerializer serializer)
    {
      if (serializer == null)
      {
        throw new ArgumentNullException(nameof(serializer));
      }

      _serializer = serializer;
    }

    public AssetManifestReader(JsonSerializerSettings serializerSettings)
    {
      if (serializerSettings == null)
      {
        throw new ArgumentNullException(nameof(serializerSettings));
      }

      _serializer = JsonSerializer.Create(serializerSettings);
    }

    public async Task<IAssetManifest> Read(PathString path, Stream stream)
    {
      string json;
      using (var streamReader = new StreamReader(stream))
        json = await streamReader.ReadToEndAsync();

      IDictionary<string, string> assets;
      using (var stringReader = new StringReader(json))
      using (var jsonReader = new JsonTextReader(stringReader))
        assets = _serializer.Deserialize<IDictionary<string, string>>(jsonReader);

      var converted = assets
        .Select(kvp => new KeyValuePair<PathString, PathString>(
          new PathString("/" + kvp.Key),
          new PathString("/" + kvp.Value)
        ));

      return new AssetManifest(path, converted.ToImmutableDictionary());
    }
  }
}
