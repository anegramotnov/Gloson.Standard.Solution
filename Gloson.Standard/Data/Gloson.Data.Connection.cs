﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gloson.Data {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// DB Connection Dialog
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface IRdbmsConnectionDialog {
    /// <summary>
    /// Connection string or null if not connected
    /// </summary>
    string ConnectionString(IDbConnection connection);
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Connection
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class RdbmsConnection {
    #region Private Data

    private static String s_ConnectionString = "";

    #endregion Private Data

    #region Public

    /// <summary>
    /// Register Connection
    /// </summary>
    /// <param name="connectionType">Connection Type</param>
    /// <param name="connectionString">Connection String</param>
    public static void Register(Type connectionType, string connectionString) {
      if (null == connectionType)
        throw new ArgumentNullException(nameof(connectionType));
      else if (connectionType.IsAbstract || connectionType.IsInterface)
        throw new ArgumentException($"Class {connectionType.Name} must not be abstract", nameof(connectionType));
      else if (!connectionType.GetInterfaces().Any(itf => typeof(IDbConnection) == itf))
        throw new ArgumentException($"Class {connectionType.Name} must implement {typeof(IDbConnection).Name}", nameof(connectionType));

      if (string.IsNullOrWhiteSpace(connectionString)) {
        Register(connectionType);

        return;
      }

      var items = Dependencies.Services.RemoveAll(typeof(IDbConnection));
        
      ServiceDescriptor descriptor = new ServiceDescriptor(
        typeof(IDbConnection),
        (provider) => {
          IDbConnection result = Activator.CreateInstance(connectionType, connectionString) as IDbConnection;

          result.Open();

          return result;
        },
        ServiceLifetime.Transient);

      Dependencies.Services.Add(descriptor);

      Interlocked.Exchange(ref s_ConnectionString, connectionString);
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <param name="connectionType">Connection Type</param>
    public static void Register(Type connectionType) {
      if (null == connectionType)
        throw new ArgumentNullException(nameof(connectionType));
      else if (connectionType.IsAbstract || connectionType.IsInterface)
        throw new ArgumentException($"Class {connectionType.Name} must not be abstract", nameof(connectionType));
      else if (!connectionType.GetInterfaces().Any(itf => typeof(IDbConnection) == itf))
        throw new ArgumentException($"Class {connectionType.Name} must implement {typeof(IDbConnection).Name}", nameof(connectionType));
      
      var items = Dependencies.Services.RemoveAll(typeof(IDbConnection));

      ServiceDescriptor descriptor = new ServiceDescriptor(
        typeof(IDbConnection),
        (provider) => {
          IDbConnection result = Activator.CreateInstance(connectionType) as IDbConnection;

          var dialog = Dependencies.Provider.GetService<IRdbmsConnectionDialog>();

          if (dialog != null) {
            string connectionString = dialog.ConnectionString(result);

            if (!string.IsNullOrWhiteSpace(connectionString)) {
              Register(connectionType, connectionString);

              result.ConnectionString = connectionString;

              result.Open(); 
            }
          }

          return result;
        },
        ServiceLifetime.Transient);

      Dependencies.Services.Add(descriptor);

      Interlocked.Exchange(ref s_ConnectionString, "");
    }

    /// <summary>
    /// Create Connection
    /// </summary>
    public static IDbConnection Create() {
      return Dependencies.Provider.GetRequiredService<IDbConnection>();
    }

    /// <summary>
    /// Connection string
    /// </summary>
    public static string ConnectionString => s_ConnectionString;

    #endregion Public
  }

}
