﻿using System;

namespace Gloson.UI.CommandLine {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Command Line Type
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public enum CommandLineType {
    None = 0,
    Boolean = 1,
    Integer = 2,
    FloatingPoint = 3,
    Date = 4,
    Time = 5,
    DateTime = 6,
    String = 7,

    File = 8,
    Directory = 9,
    FileOrDirectory = 10,
    ExistingFile = 11,
    ExistingDirectory = 12,
    ExistingFileOrDirectory = 13
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Command Line Type Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class CommandLineTypeExtensions {
    #region Public

    /// <summary>
    /// Wants / Has parameter
    /// </summary>
    public static bool WantsValue(this CommandLineType kind) {
      return kind != CommandLineType.None;
    }

    #endregion Public
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Command Line Descriptions Options
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  [Flags]
  public enum CommandLineDescriptorsOptions {
    None = 0,
    CaseSensitive = 1,
    OptionalPrefix = 2,
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Special Descriptors
  /// </summary>
  // 
  //-------------------------------------------------------------------------------------------------------------------

  public enum CommandLineDescriptorKind {
    /// <summary>
    /// Standard Command Line Argument
    /// </summary>
    None = 0,
    /// <summary>
    /// Standard Command Line Argument
    /// </summary>
    Standard = None,
    /// <summary>
    /// Help Command Line Argument
    /// </summary>
    Help = 1
  }

}
