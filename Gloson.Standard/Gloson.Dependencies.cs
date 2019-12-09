﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gloson {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Dependencies
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class Dependencies {
    #region Private Data
    #endregion Private Data

    #region Algorithm
    #endregion Algorithm

    #region Public

    /// <summary>
    /// Services
    /// </summary>
    [CLSCompliant(false)]
    public static IServiceCollection Services { get; } = new ServiceCollection();

    #endregion Public
  }
}
