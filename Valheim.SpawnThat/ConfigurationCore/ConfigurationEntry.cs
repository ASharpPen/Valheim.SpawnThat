using BepInEx.Configuration;

namespace Valheim.SpawnThat.ConfigurationCore
{
    public interface IConfigurationEntry
    {
        void Bind(ConfigFile config, string section, string key);
    }

    public class ConfigurationEntry<TIn> : IConfigurationEntry
    {
        public TIn DefaultValue { get; set; }
        public string Description { get; set; }

        public ConfigEntry<TIn> Config { get; set; }

        public void Bind(ConfigFile config, string section, string key)
        {
            if (Description is null)
            {
                Config = config.Bind<TIn>(section, key, DefaultValue);
            }
            else
            {
                Config = config.Bind<TIn>(section, key, DefaultValue, Description);
            }
        }

        public override string ToString()
        {
            return $"[{Config.Definition.Key}:{Config.Definition.Section}]: {Config.Value}";
        }

        public TIn Value 
        {
            get
            {
                if(Config is null)
                {
                    return DefaultValue;
                }

                return Config.Value;
            }
        }

        public ConfigurationEntry()
        {

        }

        public ConfigurationEntry(TIn defaultValue, string description = null)
        {
            DefaultValue = defaultValue;
        }
    }
}
