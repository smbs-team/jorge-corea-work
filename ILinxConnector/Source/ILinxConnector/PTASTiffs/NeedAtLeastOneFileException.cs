namespace PTASTiffs
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.Drawing.Imaging;
  using System.IO;
  using System.Linq;

  /// <summary>
  /// Exception thrown when the incoming document has no files contained.
  /// </summary>
  public class NeedAtLeastOneFileException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="NeedAtLeastOneFileException"/> class.
    /// </summary>
    public NeedAtLeastOneFileException()
      : base("Need at least one file.")
    {
    }
  }
}
