/******************************************************************************
 * File: WritableXmlConfigurationSourceTest.cs
 */
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace Brogdogg.Configuration.Xml.Tests
{
  /************************** WritableXmlConfigurationSourceTest *************/
  /// <summary>
  /// Unit tests for the <see cref="WritableXmlConfigurationSource"/> class.
  /// </summary>
  [TestClass]
  public class WritableXmlConfigurationSourceTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- BuildsAValidProvider --------------------------*/
    /// <summary>
    /// Verifies the
    /// <see cref="WritableXmlConfigurationSource.Build(IConfigurationBuilder)"/>
    /// method builds a valid
    /// <see cref="WritableXmlConfigurationProvider"/> object.
    /// </summary>
    [TestMethod]
    public void BuildsAValidProvider()
    {
      var configBuilder = Substitute.For<IConfigurationBuilder>();
      var writer = Substitute.For<IXmlConfigurationWriter>();
      var streamProvider = Substitute.For<IFileStreamProvider>();
      var source = new WritableXmlConfigurationSource(writer, streamProvider);
      var provider = source.Build(configBuilder);
      Assert.IsNotNull(provider);
      Assert.IsInstanceOfType(provider, typeof(WritableXmlConfigurationProvider));

    } /* End of Function - BuildsAValidProvider */


    /*----------------------- ThrowsForInvalidFileStreamProvider ------------*/
    /// <summary>
    /// Verifies the
    /// <see cref="WritableXmlConfigurationSource"/> constructor throws
    /// when the <see cref="IFileStreamProvider"/> argument is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidFileStreamProvider()
    {
      var writer = Substitute.For<IXmlConfigurationWriter>();
      _ = new WritableXmlConfigurationSource(writer, null);
    } /* End of Function - ThrowsForInvalidFileStreamProvider */


    /*----------------------- ThrowsForInvalidWriter ------------------------*/
    /// <summary>
    /// Verifies the <see cref="WritableXmlConfigurationSource"/>
    /// constructor throws when the
    /// <see cref="IXmlConfigurationWriter"/> is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidWriter()
    {
      var streamProvider = Substitute.For<IFileStreamProvider>();
      _ = new WritableXmlConfigurationSource(null, streamProvider);
    } /* End of Function - ThrowsForInvalidWriter */


    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - WritableXmlConfigurationSourceTest */


} /* End of Namespace - Brogdogg.Configuration.Xml.Tests */
/* End of document - WritableXmlConfigurationSourceTest.cs */