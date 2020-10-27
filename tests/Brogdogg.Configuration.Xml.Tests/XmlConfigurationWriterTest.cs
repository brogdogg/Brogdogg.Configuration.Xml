/******************************************************************************
 * File: XmlConfigurationWriterTest.cs
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Brogdogg.Configuration.Xml.Tests
{
  /************************** XmlConfigurationWriterTest *********************/
  /// <summary>
  /// Unit tests for <see cref="XmlConfigurationWriter"/> class.
  /// </summary>
  [TestClass]
  public class XmlConfigurationWriterTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- ConstructWithDefaults -------------------------*/
    /// <summary>
    /// Verifies constructor with default parameters.
    /// </summary>
    [TestMethod]
    public void ConstructWithDefaults()
    {
      var cfgWriter = new FakeXmlConfigurationWriter();
      Assert.AreEqual("configuration", cfgWriter.RootName);
      Assert.IsTrue(cfgWriter.WriteComment);
    } /* End of Function - ConstructWithDefaults */


    /*----------------------- ConstructWithNonDefaults ----------------------*/
    /// <summary>
    /// Verifies calling the constructor with custom values is respected.
    /// </summary>
    [TestMethod]
    public void ConstructWithNonDefaults()
    {
      var fakeXmlWriter = Substitute.For<XmlWriter>();
      var settings = new XmlWriterSettings()
      {
        NewLineChars = "test"
      };
      var cfgWriter = new FakeXmlConfigurationWriter(
                            settings,
                            "root",
                            false);
      cfgWriter.XmlWriterFactory = (stream) =>
      {
        return fakeXmlWriter;
      };
      using var stream = new MemoryStream();
      cfgWriter.Write(stream, new Dictionary<string, string>());
      fakeXmlWriter.Received().WriteStartDocument();
      fakeXmlWriter.Received().WriteStartElement("root");
      fakeXmlWriter.Received().WriteEndDocument();
      Assert.AreEqual("root", cfgWriter.RootName);
      Assert.AreEqual(settings, cfgWriter.XmlWriterSettings);
      Assert.IsFalse(cfgWriter.WriteComment);

    } /* End of Function - ConstructWithNonDefaults */


    /*----------------------- DoesNotWriteComment ---------------------------*/
    /// <summary>
    /// Verifies when constructing with indication to not write a comment,
    /// the XML writer does used to write a comment.
    /// </summary>
    [TestMethod]
    public void DoesNotWriteComment()
    {
      var fakeXmlWriter = Substitute.For<XmlWriter>();
      var cfgWriter = new FakeXmlConfigurationWriter(writeComment: false);

      cfgWriter.XmlWriterFactory = (stream) =>
      {
        return fakeXmlWriter;
      };

      using var stream = new MemoryStream();
      cfgWriter.Write(stream, new Dictionary<string, string>());
      fakeXmlWriter.Received().WriteStartDocument();
      fakeXmlWriter.DidNotReceive().WriteComment(Arg.Any<string>());
      fakeXmlWriter.Received().WriteEndDocument();
    } /* End of Function - DoesNotWriteComment */


    /*----------------------- WritesCorrectData -----------------------------*/
    /// <summary>
    /// Verifies the writing some data.
    /// </summary>
    [TestMethod]
    public void WritesCorrectData()
    {
      // Create a control group
      var data = new Dictionary<string, string>()
      {
        { "test:element", "value" },
        { "test:element2", "value2" },
        { "test", "updateValue" }
      };
      using var stream = new MemoryStream();
      var fakeXmlWriter = Substitute.For<XmlWriter>();
      var cfgWriter = new FakeXmlConfigurationWriter();

      // Make sure to setup our fake XmlWriter, so we verify the correct
      // calls were made based on the names in the data.
      cfgWriter.XmlWriterFactory = (stream) =>
      {
        return fakeXmlWriter;
      };

      // Act
      cfgWriter.Write(stream, data);

      // Now make sure the correct methods were called on the writer, to persist
      // to disk.
      fakeXmlWriter.Received().WriteStartDocument();
      fakeXmlWriter.Received().WriteComment(Arg.Any<string>());
      fakeXmlWriter.Received().WriteEndDocument();
      fakeXmlWriter.Received().WriteStartElement("configuration");
      fakeXmlWriter.Received().WriteStartElement("test");
      fakeXmlWriter.Received().WriteStartElement("element");
      fakeXmlWriter.Received().WriteStartElement("element2");
      fakeXmlWriter.Received().WriteString("value");
      fakeXmlWriter.Received().WriteString("updateValue");
    } /* End of Function - WritesCorrectData */


    /*----------------------- WriteThrowsForInvalidXmlWriter ----------------*/
    /// <summary>
    /// Verifies the write method throws when a valid <see cref="XmlWriter"/>
    /// is not created.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void WriteThrowsForInvalidXmlWriter()
    {
      var cfgWriter = new FakeXmlConfigurationWriter();
      cfgWriter.XmlWriterFactory = (s) => null;
      using var stream = new MemoryStream();
      cfgWriter.Write(stream, new Dictionary<string, string>());
    } /* End of Function - WriteThrowsForInvalidXmlWriter */


    /*----------------------- WriteThrowsForNullData ------------------------*/
    /// <summary>
    /// Verifies the
    /// <see cref="XmlConfigurationWriter.Write(Stream, IDictionary{string, string})"/>
    /// method throws a <see cref="ArgumentNullException"/> when the
    /// data is in fact null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void WriteThrowsForNullData()
    {
      using var stream = new MemoryStream();
      var cfgWriter = new XmlConfigurationWriter();
      cfgWriter.Write(stream, null);
    } /* End of Function - WriteThrowsForNullData */


    /*----------------------- WriteThrowsForNullStream ----------------------*/
    /// <summary>
    /// Verifies the
    /// <see cref="XmlConfigurationWriter.Write(Stream, IDictionary{string, string})"/>
    /// method throws a <see cref="ArgumentNullException"/> when the
    /// stream is in fact null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void WriteThrowsForNullStream()
    {
      var cfgWriter = new XmlConfigurationWriter();
      cfgWriter.Write(null, new Dictionary<string, string>());
    } /* End of Function - WriteThrowsForNullStream */


    /************************ Fields *****************************************/
    /************************ Static *****************************************/


    /*======================= PRIVATE =======================================*/
    /************************ Types ******************************************/
    /// <summary>
    /// Class for configuring some fake data around the
    /// <see cref="XmlConfigurationWriter"/> class.
    /// </summary>
    private class FakeXmlConfigurationWriter : XmlConfigurationWriter
    {
      public FakeXmlConfigurationWriter(
        XmlWriterSettings settings = null,
        string rootName = null,
        bool writeComment = true)
        : base(settings, rootName, writeComment)
      {
        return;
      }

      public new string RootName { get { return base.RootName; } }
      public new bool WriteComment { get { return base.WriteComment; } }
      public new XmlWriterSettings XmlWriterSettings
      {
        get { return base.XmlWriterSettings; }
      }
      public Func<Stream, XmlWriter> XmlWriterFactory { get; set; }
      protected override XmlWriter CreateXmlWriter(Stream stream)
      {
        return XmlWriterFactory(stream);
      }
    }
  } /* End of Class - XmlConfigurationWriterTest */


} /* End of Namespace - Brogdogg.Configuration.Xml.Tests */
/* End of document - XmlConfigurationWriterTest.cs */