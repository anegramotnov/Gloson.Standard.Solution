﻿// https://en.wikipedia.org/wiki/Tajik_alphabet

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Gloson.Text.NaturalLanguages.Library {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Tajik / English (Library of Congress)
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class TajikToEnglishAlaAc : StandardTransliteration {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    public TajikToEnglishAlaAc() : base(
      "AlaAc",
       CultureInfo.GetCultureInfo("Tj"),
       CultureInfo.GetCultureInfo("En"),
       new (string, string)[] {
         ("а", "a"),
         ("б", "b"),
         ("в", "v"),
         ("г", "g"),
         ("ғ", "ḡ"),
         ("д", "d"),
         ("е", "e"),
         ("ё", "ë"),
         ("ж", "ž"),
         ("з", "z"),
         ("и", "i"),
         ("ӣ", "ī"),
         ("й", "j"),
         ("к", "k"),
         ("қ", "ķ"),
         ("л", "l"),
         ("м", "m"),
         ("н", "n"),
         ("о", "o"),
         ("п", "p"),
         ("р", "r"),
         ("с", "s"),
         ("т", "t"),
         ("у", "u"),
         ("ӯ", "ū"),
         ("ф", "f"),
         ("х", "h"),
         ("ҳ", "x"),
         ("ч", "č"),
         ("ҷ", "ç"),
         ("ш", "š"),
         ("ъ", "'"),
         ("э", "è"),
         ("ю", "ju"),
         ("я", "ja"),
       }) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Instance
    /// </summary>
    public static TajikToEnglishAlaAc Instance { get; } = new TajikToEnglishAlaAc();

    #endregion Public
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Tajik / English (Iso 9)
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class TajikToEnglishIso9 : StandardTransliteration {
    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    public TajikToEnglishIso9() : base(
      "Iso9",
       CultureInfo.GetCultureInfo("Tj"),
       CultureInfo.GetCultureInfo("En"),
       new (string, string)[] {
         ("а", "a"),
         ("б", "b"),
         ("в", "v"),
         ("г", "g"),
         ("ғ", "ġ"),
         ("д", "d"),
         ("е", "e"),
         ("ё", "ë"),
         ("ж", "ž"),
         ("з", "z"),
         ("и", "i"),
         ("ӣ", "ī"),
         ("й", "j"),
         ("к", "k"),
         ("қ", "ķ"),
         ("л", "l"),
         ("м", "m"),
         ("н", "n"),
         ("о", "o"),
         ("п", "p"),
         ("р", "r"),
         ("с", "s"),
         ("т", "t"),
         ("у", "u"),
         ("ӯ", "ū"),
         ("ф", "f"),
         ("х", "h"),
         ("ҳ", "ḩ"),
         ("ч", "č"),
         ("ҷ", "ç"),
         ("ш", "š"),
         ("ъ", "'"),
         ("э", "è"),
         ("ю", "û"),
         ("я", "â"),
       }) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Instance
    /// </summary>
    public static TajikToEnglishIso9 Instance { get; } = new TajikToEnglishIso9();

    #endregion Public
  }

}
