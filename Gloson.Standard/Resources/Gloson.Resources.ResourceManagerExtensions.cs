﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;

namespace Gloson.Resources {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Resource Manager Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class ResourceManagerExtensions {
    #region Private Data

    private static readonly Lazy<FieldInfo> s_MainAssembly = new Lazy<FieldInfo>(() => 
      typeof(ResourceManager).GetField("MainAssembly", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
    
    #endregion Private Data

    #region Public

    /// <summary>
    /// Resources
    /// </summary>
    public static IEnumerable<KeyValuePair<string, object>> EnumerateResources(this ResourceManager manager, 
                                                                                    CultureInfo culture) {

      if (null == manager)
        throw new ArgumentNullException();

      if (null == culture)
        culture = CultureInfo.CurrentUICulture;

      using (ResourceSet rs = manager.GetResourceSet(culture, true, true)) {
        foreach (DictionaryEntry entry in rs) {
          yield return new KeyValuePair<string, object>(entry.Key as String, entry.Value);
        }
      }
    }

    /// <summary>
    /// Resources
    /// </summary>
    public static IEnumerable<KeyValuePair<string, object>> EnumerateResources(this ResourceManager manager) =>
      EnumerateResources(manager, null);

    /// <summary>
    /// Resources Main Assembly
    /// </summary>
    public static Assembly ResourceMainAssembly(this ResourceManager manager) {
      if (null == manager)
        throw new ArgumentNullException();

      return s_MainAssembly.Value.GetValue(manager) as Assembly;
    }

    #endregion Public
  }
}
