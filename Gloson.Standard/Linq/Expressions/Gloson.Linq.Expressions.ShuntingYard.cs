﻿using System;
using System.Collections.Generic;
using System.Text;

using Gloson.Text;

namespace Gloson.Linq.Expressions {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Token Kind
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public enum ExpressionTokenKind {
    None = 0,
    Delimiter = 1,
    BraceOpen = 2,
    BraceClose = 3,
    WhiteSpace = 4,

    Constant = 5,
    Variable = 6,
    Operator = 7,
    Function = 8,
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Token to operate with
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface IExpressionToken {
    /// <summary>
    /// Priority 
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Kind
    /// </summary>
    ExpressionTokenKind Kind { get; }

    /// <summary>
    /// Name
    /// </summary>
    string Name { get; }
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Expression Token
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public class ExpressionToken : IExpressionToken, IEquatable<IExpressionToken> {
    #region Create

    /// <summary>
    /// Standard Constructor
    /// </summary>
    public ExpressionToken(string name, int priority, ExpressionTokenKind kind) {
      Name = name;
      Priority = priority;
      Kind = kind;
    }

    #endregion Create

    #region Public

    /// <summary>
    /// ToString (Name)
    /// </summary>
    public override string ToString() => Name;

    #endregion Public

    #region IExpressionToken

    /// <summary>
    /// Priority 
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Kind
    /// </summary>
    public ExpressionTokenKind Kind { get; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    #endregion IExpressionToken

    #region IEquatable<ExpressionToken>

    /// <summary>
    /// Equals
    /// </summary>
    public bool Equals(IExpressionToken other) {
      if (ReferenceEquals(this, other))
        return true;
      else if (ReferenceEquals(null, other))
        return false;

      return string.Equals(Name, other.Name) &&
             Kind == other.Kind &&
             Priority == other.Priority;
    }

    /// <summary>
    /// Equals
    /// </summary>
    public override bool Equals(object obj) => Equals(obj as IExpressionToken);

    /// <summary>
    /// HashCode
    /// </summary>
    public override int GetHashCode() {
      unchecked {
        return (null == Name ? 0 : Name.GetHashCode()) ^
               ((int)Kind << 4) ^
                Priority;
      }
    }

    #endregion IEquatable<ExpressionToken>
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Shunting Yard
  /// </summary>
  /// https://ru.wikipedia.org/wiki/%D0%90%D0%BB%D0%B3%D0%BE%D1%80%D0%B8%D1%82%D0%BC_%D1%81%D0%BE%D1%80%D1%82%D0%B8%D1%80%D0%BE%D0%B2%D0%BE%D1%87%D0%BD%D0%BE%D0%B9_%D1%81%D1%82%D0%B0%D0%BD%D1%86%D0%B8%D0%B8
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class ShuntingYard<T> where T : IExpressionToken {
    #region public

    /// <summary>
    /// Process
    /// </summary>
    /// <param name="source">Original parsed text</param>
    /// <returns>Reversed Polish Order</returns>
    public static IEnumerable<T> Process(IEnumerable<T> source) {
      if (null == source)
        throw new ArgumentNullException(nameof(source));

      Stack<T> funcs = new Stack<T>();

      foreach (T token in source) {
        if (token.Kind == ExpressionTokenKind.Constant || token.Kind == ExpressionTokenKind.Variable)
          yield return token;
        else if (token.Kind == ExpressionTokenKind.Function || token.Kind == ExpressionTokenKind.BraceOpen)
          funcs.Push(token);
        else if (token.Kind == ExpressionTokenKind.Delimiter) {
          while (true) {
            if (funcs.Count <= 0)
              throw new ArgumentException($"Either delimiter or opening parenthesis missed.");

            if (funcs.Peek().Kind == ExpressionTokenKind.BraceOpen)
              break;

            yield return funcs.Pop();
          }
        }
        else if (token.Kind == ExpressionTokenKind.BraceClose) {
          while (true) {
            if (funcs.Count <= 0)
              throw new ArgumentException($"Opening parenthesis missed.");

            if (funcs.Peek().Kind == ExpressionTokenKind.BraceOpen) {
              if (!string.Equals(token.Name, funcs.Peek().Name.ParenthesesReversed()))
                throw new ArgumentException($"Mismatch parentheses for {token.Name}");

              funcs.Pop();

              if (funcs.Count > 0 && funcs.Peek().Kind == ExpressionTokenKind.Function)
                yield return funcs.Pop();

              break;
            }

            yield return funcs.Pop();
          }
        }
        else if (token.Kind == ExpressionTokenKind.Operator) {
          while (funcs.Count > 0)
            if (funcs.Peek().Kind == ExpressionTokenKind.Operator && funcs.Peek().Priority >= token.Priority)
              yield return funcs.Pop();
            else
              break;

          funcs.Push(token);
        }
        else if (token.Kind == ExpressionTokenKind.None || token.Kind == ExpressionTokenKind.WhiteSpace)
          continue;
        else
          throw new ArgumentException($"Unknown token {token.Name} with {token.Kind} kind");
      }

      // Tail
      while (funcs.Count > 0) {
        if (funcs.Peek().Kind == ExpressionTokenKind.BraceOpen)
          throw new ArgumentException($"Closing parenthesis missed.");
        else if (funcs.Peek().Kind == ExpressionTokenKind.BraceClose)
          throw new ArgumentException($"Opeing parenthesis missed.");

        yield return funcs.Pop();
      }
    }

    #endregion public
  }

}
