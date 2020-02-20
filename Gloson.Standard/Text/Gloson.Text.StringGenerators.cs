﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Gloson.Text {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// String Generator
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class StringGenerator {
    #region Public

    /// <summary>
    /// Prefixes
    /// </summary>
    public static IEnumerable<string> Prefixes(string value, bool addEmpty) {
      if (null == value)
        yield break;

      for (int i = 0; i < value.Length; ++i)
        yield return value.Substring(0, i);
    }

    /// <summary>
    /// Prefixes
    /// </summary>
    public static IEnumerable<string> Prefixes(string value) => Prefixes(value, true);

    /// <summary>
    /// Suffixes
    /// </summary>
    public static IEnumerable<string> Suffixes(string value, bool addEmpty) {
      if (null == value)
        yield break;

      for (int i = 0; i < value.Length; ++i)
        yield return value.Substring(i);
    }

    /// <summary>
    /// Suffixes
    /// </summary>
    public static IEnumerable<string> Suffixes(string value) => Suffixes(value, true);

    /// <summary>
    /// Chunks (bi-, tri- grams etc.)
    /// </summary>
    public static IEnumerable<string> Chunks(string value, int chunkSize) {
      if (chunkSize <= 0)
        throw new ArgumentOutOfRangeException(nameof(chunkSize), $"Chunk size must be positive.");

      if (string.IsNullOrEmpty(value))
        yield break;

      for (int i = 0; i < value.Length - chunkSize + 1; ++i)
        yield return value.Substring(i, chunkSize);
    }

    #endregion Public
  }
}