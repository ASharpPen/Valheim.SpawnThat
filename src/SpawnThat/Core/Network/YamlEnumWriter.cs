using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace SpawnThat.Core.Network;

internal class YamlEnumWriter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type.IsEnum;

    public object ReadYaml(IParser parser, Type type, ObjectDeserializer _)
    {
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer _)
    {
        var yamlValue = (int)value;
        var scalar = new Scalar(yamlValue.ToString());

        emitter.Emit(scalar);
    }
}
