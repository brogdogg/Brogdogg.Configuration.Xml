/******************************************************************************
 * File: XmlConfigurationExtensionsTest.cs
 */
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Brogdogg.Configuration.Xml.Tests
{
  /************************** XmlConfigurationExtensionsTest *****************/
  /// <summary>
  /// Unit tests for the extension methods for
  /// <see cref="IConfigurationBuilder"/>.
  /// </summary>
  [TestClass]
  public class XmlConfigurationExtensionsTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- AddsWritableXmlWithPath -----------------------*/
    /// <summary>
    /// Verifies we can add a <see cref="WritableXmlConfigurationSource"/>
    /// when specifying just a path.
    /// </summary>
    [TestMethod]
    public void AddsWritableXmlWithPath()
    {
      var expectedPath = "test/path";
      var configBuilder = Substitute.For<IConfigurationBuilder>();
      configBuilder.AddWritableXml(expectedPath);
      configBuilder.Received().Add(Arg.Is<WritableXmlConfigurationSource>(arg =>
        arg.Path == expectedPath));
        
      return;
    } /* End of Function - AddsWritableXmlWithPath */


    /*----------------------- AddsWritableXmlWithWriter ---------------------*/
    /// <summary>
    /// Verifies we can add a <see cref="WritableXmlConfigurationSource"/>
    /// when specifying a path with some of the other options.
    /// </summary>
    [TestMethod]
    public void AddsWritableXmlWithWriter()
    {
      var configBuilder = Substitute.For<IConfigurationBuilder>();
      var fileProvider = Substitute.For<IFileProvider>();
      var expectedPath = "test/path";
      var writer = Substitute.For<IXmlConfigurationWriter>();
      var streamProvider = Substitute.For<IFileStreamProvider>();

      configBuilder.AddWritableXml(
                      fileProvider,
                      expectedPath,
                      true,
                      true,
                      streamProvider,
                      writer);

      configBuilder.Received().Add(
        Arg.Is<WritableXmlConfigurationSource>(arg =>
          arg.Writer == writer
          && arg.Optional == true
          && arg.ReloadOnChange == true
          && arg.FileProvider == fileProvider));
      return;
    } /* End of Function - AddsWritableXmlWithWriter */


    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - XmlConfigurationExtensionsTest */


} /* End of Namespace - Brogdogg.Configuration.Xml.Tests */
/* End of document - XmlConfigurationExtensionsTest.cs */