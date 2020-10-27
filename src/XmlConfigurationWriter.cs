/******************************************************************************
 * File: XmlConfigurationWriter.cs
 */
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Brogdogg.Configuration.Xml
{
  /************************** XmlConfigurationWriter *************************/
  /// <inheritdoc/>
  public class XmlConfigurationWriter : IXmlConfigurationWriter
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- XmlConfigurationWriter ------------------------*/
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="rootName">Optional name to use for root element.</param>
    /// <param name="settings">
    /// Optional <see cref="XmlWriterSettings"/> to use when writing the
    /// XML data.
    /// </param>
    /// <param name="writeComment">
    /// Flag indicating if comment should be written to XML file indicating
    /// date/time of modification.
    /// </param>
    public XmlConfigurationWriter(
             XmlWriterSettings settings = null,
             string rootName = null,
             bool writeComment = true)
    {
      // Make sure we have a root name
      RootName = rootName ?? DEFAULT_ROOT_NAME;

      // And make sure we have valid set of settings
      XmlWriterSettings = settings ?? new XmlWriterSettings()
      {
        NewLineHandling = NewLineHandling.Replace,
        NamespaceHandling = NamespaceHandling.Default,
        Indent = true,
      };

      WriteComment = writeComment;

      return;
    } /* End of Function - XmlConfigurationWriter */


    /************************ Methods ****************************************/

    /*----------------------- Write -----------------------------------------*/
    /// <inheritdoc/>
    public virtual void Write(Stream stream, IDictionary<string, string> data)
    {
      if (null == stream) throw new ArgumentNullException(nameof(stream));
      if (null == data) throw new ArgumentNullException(nameof(data));


      // Build up the XML tree from the data
      var tree = GetXmlTree(data);

      // Then create an XML writer to write out the tree
      using XmlWriter writer = CreateXmlWriter(stream)
        ?? throw new InvalidOperationException(
                       $"A valid XmlWriter object is needed.");

      // Start the document
      writer.WriteStartDocument();

      if (WriteComment)
        writer.WriteComment($"Auto generated from {this.GetType()} on {DateTime.Now}");

      // Write the entire tree out
      WriteXmlTree(writer, tree);

      writer.WriteEndDocument();

      return;
    } /* End of Function - Write */


    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /// <summary>
    /// Class used to track XML items to write at a later point.
    /// </summary>
    protected class XmlItem
    {
      /// <summary>
      /// Gets the list of children for the this item.
      /// </summary>
      public IList<XmlItem> Children { get; } = new List<XmlItem>();
      /// <summary>
      /// Gets the parent, if any, for this item.
      /// </summary>
      public XmlItem Parent { get; }
      /// <summary>
      /// Gets the name of this XML item.
      /// </summary>
      public string Name { get; }
      /// <summary>
      /// Gets the value, if any.
      /// </summary>
      public string Value { get; set; }
      /// <summary>
      /// Constructor.
      /// </summary>
      /// <param name="name">Required name to use for the Xml element.</param>
      /// <param name="value">Optional value for the element.</param>
      /// <param name="parent">Optional parent for the element.</param>
      public XmlItem(string name, string value = null, XmlItem parent = null)
      {
        Parent = parent;
        // A name is required!
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value;
      } // end of function - XmlItem
    } // end of class - XmlItem
    /************************ Properties *************************************/


    /************************ RootName ***************************************/
    /// <summary>
    /// Gets the name to use for the root element when writing the XML
    /// data to a file.
    /// </summary>
    protected string RootName { get; }


    /************************ XmlWriterSettings ******************************/
    /// <summary>
    /// Gets/Sets the <see cref="XmlWriterSettings"/> used for creating
    /// the <see cref="XmlWriter"/> object for saving the XML data.
    /// </summary>
    protected XmlWriterSettings XmlWriterSettings { get; }


    /************************ WriteComment ***********************************/
    /// <summary>
    /// Gets a flag indicating if a comment should be written to the XML
    /// file for date/time written.
    /// </summary>
    protected bool WriteComment { get; }


    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- CreateXmlWriter -------------------------------*/
    /// <summary>
    /// Creates a <see cref="XmlWriter"/> to use for writing XML data.
    /// </summary>
    /// <param name="stream">
    /// Destination stream.
    /// </param>
    protected virtual XmlWriter CreateXmlWriter(Stream stream)
    {
      return XmlWriter.Create(stream, XmlWriterSettings);
    } /* End of Function - CreateXmlWriter */


    /*----------------------- GetXmlItem ------------------------------------*/
    /// <summary>
    /// Gets an <see cref="XmlItem"/> for the specified configuraion path/value.
    /// </summary>
    /// <param name="root">
    /// Required root item to search in
    /// </param>
    /// <param name="path">
    /// Path as defined by <see cref="ConfigurationPath"/> syntax.
    /// </param>
    /// <param name="value">
    /// Optional value for the item.
    /// </param>
    protected virtual XmlItem GetXmlItem(
                                ref XmlItem root,
                                string path,
                                string value)
    {
      XmlItem retval = null;
      XmlItem parent = null;


      if (string.IsNullOrEmpty(path))
        throw new InvalidOperationException(
          $"{nameof(path)} must be a valid configuraiton path.");

      // Parse the parent path from the key, if any
      var parentPath = ConfigurationPath.GetParentPath(path);
      // And get the current key
      var key = ConfigurationPath.GetSectionKey(path);

      // If we do not have a parent, then we will assume the root item as our
      // parent.
      if (null == parentPath)
        parent = root;
      // Otherwise, fetch the parent XML node
      else
        parent = GetXmlItem(ref root, parentPath, null);

      // At this point, we can look for a child in the parent that already
      // matches our key
      retval = parent.Children.Where(v => v.Name == key).SingleOrDefault();

      // If we didn't find one, will create and add to the parent's
      // children.
      if (null == retval)
      {
        retval = new XmlItem(key, value, parent);
        parent.Children.Add(retval);
      } // end of if - child doesn't exist yet
      // Otherwise, if we have a valid value, then we will update at this point
      else if (null != value)
        retval.Value = value;

      return retval;
    } /* End of Function - GetXmlItem */


    /*----------------------- GetXmlTree ------------------------------------*/
    /// <summary>
    /// Builds an XML tree representation from the data parsed by the
    /// XML configuration provider.
    /// </summary>
    /// <param name="data">
    /// Dictionary of key/value pairs, with the keys based on the path
    /// delimited item with respect to <see cref="ConfigurationPath"/>.
    /// </param>
    protected virtual XmlItem GetXmlTree(IDictionary<string, string> data)
    {
      // Create the root element
      var retval = new XmlItem(RootName, null, null);
      // And iterate over the data, converting to our internal
      // XmlItem tree representation
      foreach (var keyValue in data)
        _ = GetXmlItem(ref retval, keyValue.Key, keyValue.Value);

      return retval;
    } /* End of Function - GetXmlTree */


    /*----------------------- WriteXmlTree ----------------------------------*/
    /// <summary>
    /// Writes the XML for the item.
    /// </summary>
    /// <param name="writer">XmlWriter instance to use.</param>
    /// <param name="root">Root of the XML element to write</param>
    /// <remarks>
    /// Does not actually write start/end document, just starts writing
    /// the XML elements.
    /// </remarks>
    protected virtual void WriteXmlTree(XmlWriter writer, XmlItem root)
    {
      if (null == writer)
        throw new ArgumentNullException(nameof(writer));

      if (null != root)
      {
        // Must start the element
        writer.WriteStartElement(root.Name);

        // Write out any children that is available
        foreach (var child in root.Children)
          WriteXmlTree(writer, child);

        // And if there is a "value", we must write it out as well
        if (!string.IsNullOrEmpty(root.Value))
          writer.WriteString(root.Value);

        // Last, don't forget to end the element.
        writer.WriteEndElement();
      } // end of if - valid xml item
    } /* End of Function - WriteXmlTree */


    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /// <summary>
    /// Uses the XML root namespace for configuration files. 
    /// </summary>
    private const string DEFAULT_ROOT_NAME = "configuration";
  } /* End of Class - XmlConfigurationWriter */
} /* End of Namespace - Brogdogg.Configuration.Xml */
/* End of document - XmlConfigurationWriter.cs */