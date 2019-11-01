﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gloson.Linq {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Combinatorics
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class EnumerableExtensions {
    #region Public

    /// <summary>
    /// Permutations (factorial)
    /// e.g. {A, B, C} -> {[A, B, C], [A, C, B], ... [C, B, A] }
    /// </summary>
    public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source) {
      if (null == source)
        throw new ArgumentNullException("source");

      T[] items = source.ToArray();

      if (items.Length <= 1) {
        yield return items;

        yield break;
      }

      for (int i = 0; i < items.Length; ++i) {
        foreach (T[] record in Permutations<T>(items.Skip(1))) {
          T[] result = new T[items.Length];

          result[i] = items[0];

          for (int j = 0; j < i; ++j)
            result[j] = record[j];

          for (int j = i + 1; j < result.Length; ++j)
            result[j] = record[j - 1];

          yield return result;
        }
      }
    }

    /// <summary>
    /// Combinations (2**N)
    /// e.g. {A, B, C} -> {[], [A], [B], [A, B], [C], [A, C], ..., [A, B, C]}
    /// </summary>
    public static IEnumerable<T[]> SubSets<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer) {
      if (null == source)
        throw new ArgumentNullException("source");

      T[] items = source.Distinct(comparer).ToArray();

      for (int i = 0; i < 1 << items.Length; ++i) {
        List<T> result = new List<T>(items.Length);

        for (int v = i, j = 0; v > 0; v /= 2, ++j)
          if ((v & 1) != 0)
            result.Add(items[j]);

        yield return result.ToArray();
      }
    }

    /// <summary>
    /// Combinations (2**N)
    /// e.g. {A, B, C} -> {[], [A], [B], [A, B], [C], [A, C], ..., [A, B, C]}
    /// </summary>
    public static IEnumerable<T[]> SubSets<T>(this IEnumerable<T> source) {
      if (null == source)
        throw new ArgumentNullException("source");

      T[] items = source.Distinct().ToArray();

      for (int i = 0; i < 1 << items.Length; ++i) {
        List<T> result = new List<T>(items.Length);

        for (int v = i, j = 0; v > 0; v /= 2, ++j)
          if ((v & 1) != 0)
            result.Add(items[j]);

        yield return result.ToArray();
      }
    }

    /// <summary>
    /// Number Combinations
    /// {A, B, C}, 4 -> {A, A, A, A}, {A, A, A, B}, {A, A, A, C}, {A, A, B, A}, ... , {C, C, C, C}
    /// </summary>
    public static IEnumerable<T[]> OrderedWithReplacement<T>(this IEnumerable<T> source, int size) {
      if (null == source)
        throw new ArgumentNullException("source");
      else if (size < 0)
        throw new ArgumentOutOfRangeException("size");

      T[] alphabet = source.ToArray();

      if (alphabet.Length <= 0)
        yield break;
      else if (size == 0)
        yield break;

      int[] indexes = new int[size];

      do {
        yield return indexes
          .Select(i => alphabet[i])
          .ToArray();

        for (int i = indexes.Length - 1; i >= 0; --i)
          if (indexes[i] >= alphabet.Length - 1)
            indexes[i] = 0;
          else {
            indexes[i] += 1;

            break;
          }
      }
      while (!indexes.All(index => index == 0));
    }

    /// <summary>
    /// Number Combinations
    /// {A, B, C}, 4 -> {A, A, A, A}, {A, A, A, B}, {A, A, A, C}, {A, A, B, B}, {A, A, B, C},... , {C, C, C, C}
    /// </summary>
    public static IEnumerable<T[]> UnOrderedWithReplacement<T>(this IEnumerable<T> source, int size) {
      if (null == source)
        throw new ArgumentNullException("source");
      else if (size < 0)
        throw new ArgumentOutOfRangeException("size");

      T[] alphabet = source.ToArray();

      if (alphabet.Length <= 0)
        yield break;
      else if (size == 0)
        yield break;

      int[] indexes = new int[size];

      do {
        yield return indexes
          .Select(i => alphabet[i])
          .ToArray();

        for (int i = indexes.Length - 1; i >= 0; --i)
          if (indexes[i] >= alphabet.Length - 1)
            indexes[i] = 0;
          else {
            indexes[i] += 1;

            for (int j = i + 1; j < indexes.Length; ++j)
              indexes[j] = indexes[i];

            break;
          }
      }
      while (!indexes.All(index => index == 0));
    }

    /// <summary>
    /// Assignments
    /// e.g. {A, B, C}, {1, 2} -> {[(A, 1) (B, 2)], [(A, 1) (C, 2)], ..., [(B, 2) (C, 1)]}
    /// </summary>
    public static IEnumerable<Tuple<T1, T2>[]> Assignments<T1, T2>(this IEnumerable<T1> source, IEnumerable<T2> target) {
      if (null == source)
        throw new ArgumentNullException("source");
      else if (null == target)
        throw new ArgumentNullException("target");

      T1[] sources = source.ToArray();
      T2[] targets = target.ToArray();

      if (sources.Length <= 0 || targets.Length <= 0)
        yield break;

      if (sources.Length == 1) {
        foreach (var t in targets)
          yield return (new Tuple<T1, T2>[] { new Tuple<T1, T2>(sources[0], t) });

        yield break;
      }

      // Not assigned
      if (sources.Length > targets.Length) {
        var reversed = Assignments(targets, sources);

        foreach (var record in reversed)
          yield return record
            .Select(item => new Tuple<T1, T2>(item.Item2, item.Item1))
            .ToArray();

        yield break;
      }

      for (int i = 0; i < targets.Length; ++i) {
        var tar = targets.Where((v, index) => index != i);

        foreach (var record in Assignments(sources.Skip(1), tar)) {
          List<Tuple<T1, T2>> result = new List<Tuple<T1, T2>>(Math.Min(sources.Length, targets.Length));

          result.Add(new Tuple<T1, T2>(sources[0], targets[i]));
          result.AddRange(record);

          yield return result.ToArray();
        }
      }
    }

    #endregion Public
  }

}