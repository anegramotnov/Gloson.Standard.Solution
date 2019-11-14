﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Text;

using Gloson;
using Gloson.Json;

namespace Gloson.Services.Git.Stash {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Stash Controller
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class StashStorage {
    #region Private Data

    private List<StashProject> m_Items = null;

    #endregion Private Data

    #region Algorithm

    private string BuildQuery(string query, int start, int limit) {
      string address = string.IsNullOrWhiteSpace(Connection.Login)
        ? $"{Location.Scheme}://{Location.Host}"
        : $"{Location.Scheme}://{Connection.Login}@{Location.Host}";

      return $"{address}/rest/api/1.0/{query}?start={start}&limit={limit}";
    }

    private HttpWebRequest Request(string query, int start, int limit) {
      HttpWebRequest request = WebRequest.Create(BuildQuery(query, start, limit)) as HttpWebRequest;
      request.Credentials = CredentialCache.DefaultCredentials;

      request.Method = "GET";
      request.ContentType = "application/json";
      request.Accept = "application/json";

      if (Connection.HasPassword) {
        string password = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Connection.Login}:{Connection.Password}"));

        request.Headers.Add(HttpRequestHeader.Authorization, $"Basic {password}");
      }

      return request;
    }

    private IEnumerable<JsonValue> CoreQuery(string query) {
      int start = 0;
      int limit = 1000;

      while (true) {
        HttpWebRequest request = Request(query, start, limit);

        JsonValue root = null;

        using (WebResponse response = request.GetResponse()) {
          using (Stream stream = response.GetResponseStream()) {
            using (StreamReader reader = new StreamReader(stream)) {
              root = JsonValue.Parse(reader.ReadToEnd());
            }
          }
        }

        var array = root?.Value("values") as JsonArray;

        if (null == array) {
          yield return root;
          yield break;
        }

        foreach (JsonValue item in array)
          yield return item;

        if (root.Value("isLastPage") == true)
          break;

        start = root.Value("nextPageStart");
      }
    }

    private void CoreLoadProjects() {
      if (null != m_Items)
        return;

      m_Items = new List<StashProject>();

      foreach (var json in CoreQuery("projects")) 
        m_Items.Add(new StashProject(this, json));
    }

    #endregion Algorithm 

    #region Create

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="connection">Connection</param>
    public StashStorage(LoginPasswordServer connection) {
      if (null == connection)
        throw new ArgumentNullException(nameof(connection));

      if (!Uri.TryCreate(connection.Server, UriKind.RelativeOrAbsolute, out Uri address)) {
        throw new ArgumentException(
          "Connection is not a valid URL",
          nameof(connection));
      }

      Location = address;
      Connection = connection;
    }

    /// <summary>
    /// Nexign
    /// </summary>
    public static StashStorage Nexign(string password) {
      LoginPasswordServer connection = new LoginPasswordServer(
          Environment.UserName,
          password,
        $@"https://{Environment.UserName}@stash.billing.ru"
      );

      return new StashStorage(connection);
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Location
    /// </summary>
    public Uri Location { get; }

    /// <summary>
    /// Connection
    /// </summary>
    public LoginPasswordServer Connection { get; }

    /// <summary>
    /// Query
    /// </summary>
    public IEnumerable<JsonValue> Query(string query) {
      if (null == query)
        throw new ArgumentNullException(nameof(query));
      else if (string.IsNullOrWhiteSpace(query))
        throw new ArgumentException("Empty query", nameof(query));

      return CoreQuery(query);
    }

    /// <summary>
    /// Is connected
    /// </summary>
    public bool IsConnected {
      get {
        try {
          return Query("application-properties").Any();
        }
        catch (WebException) {
          return false;
        }
      }
    }

    /// <summary>
    /// Projects
    /// </summary>
    public IReadOnlyList<StashProject> Projects {
      get {
        CoreLoadProjects();

        return m_Items;
      }
    }

    /// <summary>
    /// To String
    /// </summary>
    public override string ToString() {
      return $"{Connection.Login} at {Location}";
    }

    #endregion Public
  }

}