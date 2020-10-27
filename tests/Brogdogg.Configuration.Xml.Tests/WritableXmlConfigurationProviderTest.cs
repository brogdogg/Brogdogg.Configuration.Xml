/******************************************************************************
 * File: WritableXmlConfigurationProviderTest.cs
 */
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.IO;

namespace Brogdogg.Configuration.Xml.Tests
{
  /************************** WritableXmlConfigurationProviderTest ***********/
  /// <summary>
  /// Tests the logic of the
  /// <see cref="WritableXmlConfigurationProvider"/> class.
  /// </summary>
  [TestClass]
  public class WritableXmlConfigurationProviderTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- CanSetKeyValue --------------------------------*/
    /// <summary>
    /// Verifies the
    /// <see cref="WritableXmlConfigurationProvider.Set(string, string)"/>
    /// method correctly writes data as expected.
    /// </summary>
    [TestMethod]
    public void CanSetKeyValue()
    {
      var expectedKey = "testKey";
      var expectedValue = "testValue";
      var expectedPath = "expectedPath";

      // Setup a fake writer
      var writer = Substitute.For<IXmlConfigurationWriter>();


      // Setup a pretend stream to hand back for the file path
      using var stream = new MemoryStream();
      var streamProvider = Substitute.For<IFileStreamProvider>();
      // Return our memory stream for the path we setup to expect
      streamProvider.GetWritableStream(Arg.Is(expectedPath)).Returns(stream);

      var source = new WritableXmlConfigurationSource(writer, streamProvider);
      // Set the source path to our expected path
      source.Path = expectedPath;

      // Setup a fake file info to use for returning the value
      var fileInfo = Substitute.For<IFileInfo>();
      fileInfo.PhysicalPath.Returns(expectedPath);

      // Setup a fake file provider
      var fileProvider = Substitute.For<IFileProvider>();
      // And set it on the source we are using.
      source.FileProvider = fileProvider;
      // When requested from the file provider for the file info,
      // return our fake file info.
      fileProvider.GetFileInfo(expectedPath).Returns(fileInfo);

      // Finally construct our provider
      var provider = new WritableXmlConfigurationProvider(source);
      // And perform the set action
      provider.Set(expectedKey, expectedValue);

      // Ensure our writer received a call to write to our memory stream
      writer.Received().Write(stream, Arg.Any<IDictionary<string, string>>());

      return;
    } /* End of Function - CanSetKeyValue */


    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - WritableXmlConfigurationProviderTest */


} /* End of Namespace - Brogdogg.Configuration.Xml.Tests */
/* End of document - WritableXmlConfigurationProviderTest.cs */