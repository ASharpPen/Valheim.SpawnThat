namespace SpawnThat.Core.Toml;

internal class TomlWriterSettings
{
    /// <summary>
    /// Text to add to top of file.
    /// </summary>
    public string Header { get; set; }

    public bool AddComments { get; set; }

    /// <summary>
    /// File name including extension to write toml to.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Description to add into general debug file writing log.
    /// Eg., 'Writing {FileDescription} to file'
    /// </summary>
    public string FileDescription { get; set; }
}
