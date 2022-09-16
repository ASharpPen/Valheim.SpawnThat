using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace SpawnThat.Core.Network;

internal class YamlEnumWriter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type.IsEnum;

    public object ReadYaml(IParser parser, Type type)
    {
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        var yamlValue = (int)value;
        var scalar = new Scalar(yamlValue.ToString());

        emitter.Emit(scalar);
    }
}
