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

    /// <summary>
    /// Service Provider
    /// </summary>
    [CLSCompliant(false)]
    public static ServiceProvider Provider => Services.BuildServiceProvider();

    /// <summary>
    /// Create Required Service
    /// </summary>
    public static T GetServiceRequired<T>() => Provider.GetRequiredService<T>();

    /// <summary>
    /// Create Service
    /// </summary>
    public static T GetService<T>() => Provider.GetService<T>();

    /// <summary>
    /// Create Service
    /// </summary>
    /// <typeparam name="T">Service to create</typeparam>
    /// <param name="arguments">Argument to pass</param>
    /// <returns>Created service</returns>
    public static T CreateService<T>(params object[] arguments) {
      return ActivatorUtilities.CreateInstance<T>(Provider, arguments);
    }

    /// <summary>
    /// Create Service
    /// </summary>
    /// <typeparam name="T">Service to create</typeparam>
    /// <param name="arguments">Argument to pass</param>
    /// <returns>Created service</returns>
    public static object CreateService(Type type, params object[] arguments) {
      return ActivatorUtilities.CreateInstance(Provider, type, arguments);
    }

    #endregion Public
  }
}
