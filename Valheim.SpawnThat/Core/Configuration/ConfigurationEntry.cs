using BepInEx.Configuration;
using System;
using System.Runtime.Serialization;

namespace Valheim.SpawnThat.Core.Configuration
{
    public interface IConfigurationEntry
    {
        void Bind(ConfigFile config, string section, string key);
    }

    [Serializable]
    public class ConfigurationEntry<TIn> : IConfigurationEntry
    {
        public TIn DefaultValue { get; set; }

        [NonSerialized]
        public string Description;

        [NonSerialized]
        public ConfigEntry<TIn> Config;

        [OnSerializing]
        internal void OnSerialize()
        {
            // We cheat, and don't actually use the bepinex bindings for syncronized configurations.
            // Due to Config not being set, this should result in DefaultValue always being used instead.
            if(Config != null)
            {
                DefaultValue = Config.Value;
            }
        }

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

            PostBind();
        }

        protected virtual void PostBind() { }

        public override string ToString()
        {
            if(Config == null)
            {
                return $"[Entry: {DefaultValue}]";
            }
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
            Description = description;
            DefaultValue = defaultValue;
        }
    }
}
