/******************************************************************************
 * File: XmlConfigurationExtensions.cs
 */
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Brogdogg.Configuration.Xml
{
  /************************** XmlConfigurationExtensions *********************/
  /// <summary>
  /// Extension methods for adding a writable XML configuration
  /// provider/source to a <see cref="IConfigurationBuilder"/> item.
  /// </summary>
  public static class XmlConfigurationExtensions
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- AddWritableXml --------------------------------*/
    /// <summary>
    /// Adds a writable XML configuration source, using defaults.
    /// </summary>
    /// <param name="builder">Builder to add source to.</param>
    /// <param name="path">Path of the XML file.</param>
    /// <returns>
    /// The configuration builder.
    /// </returns>
    public static IConfigurationBuilder AddWritableXml(
                                          this IConfigurationBuilder builder,
                                          string path)
    {
      return builder.AddWritableXml(
        path: path,
        optional: false,
        reloadOnChange: false);
    } /* End of Function - AddWritableXml */


    /*----------------------- AddWritableXml --------------------------------*/
    /// <summary>
    /// Adds a writable XML configuration source.
    /// </summary>
    /// <param name="builder">Builder to add source to.</param>
    /// <param name="path">Path of the XML file.</param>
    /// <param name="optional">
    /// Determines if loading the file is optional.
    /// </param>
    /// <param name="reloadOnChange">
    /// Determines if data should reload on file change.
    /// </param>
    /// <returns>
    /// The configuration builder.
    /// </returns>
    public static IConfigurationBuilder AddWritableXml(
                                          this IConfigurationBuilder builder,
                                          string path,
                                          bool optional,
                                          bool reloadOnChange)
    {
      return builder.AddWritableXml(
        fileProvider: null,
        path: path,
        optional: optional,
        reloadOnChange: reloadOnChange,
        fileStreamProvider: new FileStreamProvider(),
        writer: new XmlConfigurationWriter(writeComment: false));
    } /* End of Function - AddWritableXml */


    /*----------------------- AddWritableXml --------------------------------*/
    /// <summary>
    /// Adds a writable XML configuration source, using defaults.
    /// </summary>
    /// <param name="builder">Builder to add source to.</param>
    /// <param name="fileProvider">File provider to use.</param>
    /// <param name="path">Path of the XML file.</param>
    /// <param name="optional">Determines if loading the file is optional.</param>
    /// <param name="reloadOnChange">
    /// Determines if data should reload on file change.
    /// </param>
    /// <param name="writer">
    /// Service used to write the data to a stream.
    /// </param>
    /// <returns>
    /// The configuration builder.
    /// </returns>
    public static IConfigurationBuilder AddWritableXml(
                                          this IConfigurationBuilder builder,
                                          IFileProvider fileProvider,
                                          string path,
                                          bool optional,
                                          bool reloadOnChange,
                                          IFileStreamProvider fileStreamProvider,
                                          IXmlConfigurationWriter writer)
    {
      // Build our source
      var source = new WritableXmlConfigurationSource(writer, fileStreamProvider)
      {
        FileProvider = fileProvider,
        Path = path,
        Optional = optional,
        ReloadOnChange = reloadOnChange
      };

      // Resolve the file provider
      source.ResolveFileProvider();

      // And finally add it back to the configuration builder.
      return builder.Add(source);
    } /* End of Function - AddWritableXml */


  } /* End of Class - XmlConfigurationExtensions */
} /* End of Namespace - Brogdogg.Configuration.Xml */
/* End of document - XmlConfigurationExtensions.cs */