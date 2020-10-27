/******************************************************************************
 * File: FileStreamProvider.cs
 */
using System.IO;

namespace Brogdogg.Configuration.Xml
{
  /************************** IFileStreamProvider ****************************/
  /// <summary>
  /// Represents a service capable of getting a writable stream for a file.
  /// </summary>
  public interface IFileStreamProvider
  {

    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /*----------------------- GetWritableStream -----------------------------*/
    /// <summary>
    /// Get a stream suitable for writing to for the specified path.
    /// </summary>
    /// <param name="path">
    /// Path of the file resource to get the stream for.
    /// </param>
    /// <returns>A <see cref="Stream"/> suitable for writing.</returns>
    Stream GetWritableStream(string path);
  } /* End of Interface - IFileStreamProvider */


  /************************** FileStreamProvider *****************************/
  /// <inheritdoc/>
  public class FileStreamProvider : IFileStreamProvider
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- GetWritableStream -----------------------------*/
    /// <inheritdoc/>
    public Stream GetWritableStream(string path)
      => new FileStream(path, FileMode.Truncate, FileAccess.Write);
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - FileStreamProvider */


} /* End of Namespace - Brogdogg.Configuration.Xml */
/* End of document - FileStreamProvider.cs */