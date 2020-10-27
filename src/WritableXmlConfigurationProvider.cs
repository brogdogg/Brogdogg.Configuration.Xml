/******************************************************************************
 * File...: WritableXmlConfigurationProvider.cs
 * Remarks: Extension of the main XmlConfigurationProvider, providing a way to
 *          save settings.
 */
using Microsoft.Extensions.Configuration.Xml;

namespace Brogdogg.Configuration.Xml
{
  /************************** WritableXmlConfigurationProvider ***************/
  /// <summary>
  /// Extension of <see cref="XmlConfigurationProvider"/>, which adds the
  /// ability too save settings as well
  /// </summary>
  public class WritableXmlConfigurationProvider : XmlConfigurationProvider
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- WritableXmlConfigurationProvider --------------*/
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="source">
    /// XML configuraiton source
    /// </param>
    public WritableXmlConfigurationProvider(
             WritableXmlConfigurationSource source)
      : base(source)
    {
      Writer = source.Writer;

      FileStreamProvider = source.StreamProvider;

      return;
    } /* End of Function - WritableXmlConfigurationProvider */


    /************************ Methods ****************************************/
    /*----------------------- Set -------------------------------------------*/
    /// <inheritdoc/>
    /// <remarks>
    /// Overriddent to write the data to the source file.
    /// </remarks>
    public override void Set(string key, string value)
    {
      base.Set(key, value);
      var fullPath = Source.FileProvider.GetFileInfo(Source.Path).PhysicalPath;
      using var stream = FileStreamProvider.GetWritableStream(fullPath);
      Writer.Write(stream, Data);
      return;
    } /* End of Function - Set */


    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /*----------------------- FileStreamProvider ----------------------------*/
    /// <summary>
    /// Gets the file stream provider.
    /// </summary>
    IFileStreamProvider FileStreamProvider { get; }


    /*----------------------- Writer ----------------------------------------*/
    /// <summary>
    /// Gets the configuration writer to use
    /// </summary>
    IXmlConfigurationWriter Writer { get; }


    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - WritableXmlConfigurationProvider */
} // end of namespace - Brogdogg.Configuration.Xml
/* End of document - WritableXmlConfigurationProvider.cs */