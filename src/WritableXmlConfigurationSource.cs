/******************************************************************************
 * File: WritableXmlConfigurationSource.cs
 */
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Xml;
using System;

namespace Brogdogg.Configuration.Xml
{
  /************************** WritableXmlConfigurationSource *****************/
  /// <summary>
  /// An XML file based <see cref="FileConfigurationSource"/>, which provides
  /// a <see cref="XmlConfigurationReadWriteProvider"/> provider, for saving
  /// settings.
  /// </mummary>
  public class WritableXmlConfigurationSource : XmlConfigurationSource
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /*----------------------- StreamProvider --------------------------------*/
    /// <summary>
    /// Gets the file stream provider.
    /// </summary>
    public IFileStreamProvider StreamProvider { get; }


    /*----------------------- Writer ----------------------------------------*/
    /// <summary>
    /// Gets the XML configuration writer for the source.
    /// </summary>
    public IXmlConfigurationWriter Writer { get; }


    /************************ Construction ***********************************/
    /*----------------------- WritableXmlConfigurationSource ----------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="fileStreamProvider"></param>
    public WritableXmlConfigurationSource(
             IXmlConfigurationWriter writer,
             IFileStreamProvider fileStreamProvider)
    {
      Writer = writer ?? throw new ArgumentNullException(nameof(writer));
      StreamProvider = fileStreamProvider ??
        throw new ArgumentNullException(nameof(fileStreamProvider));
      return;
    } /* End of Function - WritableXmlConfigurationSource */


    /************************ Methods ****************************************/
    /*----------------------- Build -----------------------------------------*/
    /// <summary>
    /// Builds the
    /// <see cref="XmlConfigurationReadWriteProvider"/> for this source.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IConfigurationBuilder"/>.
    /// </param>
    /// <returns>A <see cref="XmlConfigurationReadWriteProvider"/></returns>
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
      EnsureDefaults(builder);
      return new WritableXmlConfigurationProvider(this);
    } /* End of Function - Build */


    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - WritableXmlConfigurationSource */
} /* End of Namespace - Brogdogg.Configuration.Xml */
/* End of document - WritableXmlConfigurationSource.cs */