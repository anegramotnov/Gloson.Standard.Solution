﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gloson.Text.Parsing {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Token description via regular expressions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  class TokenDescriptionRegex : TokenDescription {
    #region Private Data

    // Start expression
    private string m_StartPattern;
    // Stop expression
    private string m_StopPattern;
    // Entire expression
    private string m_EntirePattern;

    // Start Regex
    private Regex m_StartRegex;
    // Stop Regex
    private Regex m_StopRegex;
    // Entire Regex
    private Regex m_EntireRegex;

    #endregion Private Data

    #region Algorithm

    private void CoreUpdate() {
      RegexOptions options = RegexOptions.Singleline | RegexOptions.CultureInvariant;

      if ((Options & TokenDescriptionOptions.IgnoreCase) == TokenDescriptionOptions.IgnoreCase)
        options = RegexOptions.IgnoreCase;

      if (!string.IsNullOrEmpty(m_StartPattern))
        m_StartRegex = new Regex(m_StartPattern, options);

      if (!string.IsNullOrEmpty(m_StopPattern))
        m_StopRegex = new Regex(m_StopPattern, options);

      if (!string.IsNullOrEmpty(m_EntirePattern))
        m_EntireRegex = new Regex(m_EntirePattern, options);
    }

    /// <summary>
    /// Match entire
    /// </summary>
    protected override Tuple<int, int> CoreMatchEntire(string source, int checkAt, IReadOnlyList<Token> context) {
      if (null == m_EntireRegex)
        return new Tuple<int, int>(-1, -1);

      var match = m_EntireRegex.Match(source, checkAt, source.Length - checkAt);

      if (!match.Success)
        return new Tuple<int, int>(-1, -1);
      else if (match.Index != checkAt)
        return new Tuple<int, int>(-1, -1);

      return new Tuple<int, int>(checkAt, checkAt + match.Value.Length);
    }

    /// <summary>
    /// Match start
    /// </summary>
    protected override Tuple<int, int> CoreMatchStart(string source, int checkAt, IReadOnlyList<Token> context) {
      if (null == m_StartRegex)
        return new Tuple<int, int>(-1, -1);

      var match = m_StartRegex.Match(source, checkAt, source.Length - checkAt);

      if (!match.Success)
        return new Tuple<int, int>(-1, -1);
      else if (match.Index != checkAt)
        return new Tuple<int, int>(-1, -1);

      return new Tuple<int, int>(checkAt, checkAt + match.Value.Length);
    }

    /// <summary>
    /// Match stop
    /// </summary>
    protected override Tuple<int, int> CoreMatchStop(string source, int startAt, string prefix, IReadOnlyList<Token> context) {
      if (null == m_StopRegex)
        return new Tuple<int, int>(-1, -1);

      var match = m_StopRegex.Match(source, startAt);

      if (!match.Success)
        return new Tuple<int, int>(-1, -1);

      return new Tuple<int, int>(match.Index, match.Index + match.Value.Length);
    }

    /// <summary>
    /// Is in Start/Stop mode 
    /// </summary>
    protected override bool CoreIsStartStopMode() {
      return m_EntireRegex == null;
    }

    #endregion Algorithm

    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    public TokenDescriptionRegex(string startPattern, string stopPattern, int priority, TokenDescriptionOptions options)
      : base(priority, options) {

      if (string.IsNullOrEmpty(startPattern))
        throw new ArgumentNullException("startPattern", "Start Pattern must not be null or empty.");
      else if (string.IsNullOrEmpty(stopPattern))
        throw new ArgumentNullException("stopPattern", "Stop Pattern must not be null or empty.");

      m_StartPattern = startPattern;
      m_StopPattern = stopPattern;

      CoreUpdate();
    }

    /// <summary>
    /// Standard constructor
    /// </summary>
    public TokenDescriptionRegex(string entirePattern, int priority, TokenDescriptionOptions options)
      : base(priority, options) {

      if (string.IsNullOrEmpty(entirePattern))
        throw new ArgumentNullException("entirePattern", "Entire Pattern must not be null or empty.");

      m_EntirePattern = entirePattern;

      CoreUpdate();
    }

    #endregion Create
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Token description via Func
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  class TokenDescriptionFunc : TokenDescription {
    #region Private Data

    // Start match
    private Locator m_StartMatch;

    // Stop match
    private LocatorStop m_StopMatch;

    // Entire match
    private Locator m_EntireMatch;

    #endregion Private Data

    #region Algorithm

    /// <summary>
    /// Match entire
    /// </summary>
    protected override Tuple<int, int> CoreMatchEntire(string source, int checkAt, IReadOnlyList<Token> context) {
      if (null == m_EntireMatch)
        return new Tuple<int, int>(-1, -1);

      return m_EntireMatch(source, checkAt);
    }

    /// <summary>
    /// Match start
    /// </summary>
    protected override Tuple<int, int> CoreMatchStart(string source, int checkAt, IReadOnlyList<Token> context) {
      if (null == m_StartMatch)
        return new Tuple<int, int>(-1, -1);

      return m_StartMatch(source, checkAt);
    }

    /// <summary>
    /// Match stop
    /// </summary>
    protected override Tuple<int, int> CoreMatchStop(string source, int startAt, string prefix, IReadOnlyList<Token> context) {
      if (null == m_StopMatch)
        return new Tuple<int, int>(-1, -1);

      return m_StopMatch(source, startAt, prefix);
    }

    /// <summary>
    /// Is in Start/Stop mode 
    /// </summary>
    protected override bool CoreIsStartStopMode() {
      return m_EntireMatch == null;
    }

    #endregion Algorithm

    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    public TokenDescriptionFunc(Locator startLocator, LocatorStop stopLocator, int priority, TokenDescriptionOptions options)
      : base(priority, options) {

      if (object.ReferenceEquals(null, startLocator))
        throw new ArgumentNullException("startLocator", "Start locator must not be null.");
      else if (object.ReferenceEquals(null, stopLocator))
        throw new ArgumentNullException("stopLocator", "Stop locator must not be null.");

      m_StartMatch = startLocator;
      m_StopMatch = stopLocator;
    }

    /// <summary>
    /// Standard constructor
    /// </summary>
    public TokenDescriptionFunc(Locator entireLocator, int priority, TokenDescriptionOptions options)
      : base(priority, options) {

      if (object.ReferenceEquals(null, entireLocator))
        throw new ArgumentNullException("entireLocator", "Entire locator must not be null.");

      m_EntireMatch = entireLocator;
    }

    #endregion Create

    #region Public
    #endregion Public
  }

}