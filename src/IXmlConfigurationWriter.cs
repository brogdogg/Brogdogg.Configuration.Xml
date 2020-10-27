/******************************************************************************
 * File: IXmlConfigurationWriter.cs
 */
using System.Collections.Generic;
using System.IO;

namespace Brogdogg.Configuration.Xml
{
  /************************** IXmlConfigurationWriter ************************/
  /// <summary>
  /// Represents a service capable of writing configuration data as XML
  /// data to a stream.
  /// </summary>
  public interface IXmlConfigurationWriter
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Writes the configuration data represented by the dictionary to the
    /// specified stream.
    /// </summary>
    /// <param name="stream">Stream to write to</param>
    /// <param name="data">Configuration data source</param>
    void Write(Stream stream, IDictionary<string, string> data);
  } /* End of Interface - IXmlConfigurationWriter */
} /* End of Namespace - Brogdogg.Configuration.Xml */
/* End of document - IXmlConfigurationWriter.cs */