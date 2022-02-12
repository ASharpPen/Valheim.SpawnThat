using BepInEx.Configuration;
using YamlDotNet.Serialization;

namespace SpawnThat.Core.Configuration;

internal interface IConfigurationEntry
{
    void Bind(ConfigFile config, string section, string key);
}

internal class ConfigurationEntry<TIn> : IConfigurationEntry
{
    public TIn DefaultValue { get; set; }

    [YamlIgnore]
    public string Description;

    [YamlIgnore]
    public ConfigEntry<TIn> Config;

    public void Bind(ConfigFile config, string section, string key)
    {
        if (Description is null)
        {
            Config = config.Bind<TIn>(section, key, DefaultValue);
            // Hack: Ensures default value get set before sync.
            DefaultValue = Config.Value;
        }
        else
        {
            Config = config.Bind<TIn>(section, key, DefaultValue, Description);
            // Hack: Ensures default value get set before sync.
            DefaultValue = Config.Value;
        }

        PostBind();
    }

    protected virtual void PostBind() { }

    public override string ToString()
    {
        if (Config == null)
        {
            return $"[Entry: {DefaultValue}]";
        }
        return $"[{Config.Definition.Key}:{Config.Definition.Section}]: {Config.Value}";
    }

    [YamlIgnore]
    public TIn Value
    {
        get
        {
            if (Config is null)
            {
                return DefaultValue;
            }

            return Config.Value;
        }
        set
        {
            if (Config is null)
            {
                DefaultValue = value;
            }
            else
            {
                Config.Value = value;
            }
        }
    }

    public ConfigurationEntry()
    {

    }

    public ConfigurationEntry(TIn defaultValue, string description = null)
    {
        Description = description;
        DefaultValue = defaultValue;
    }
}
