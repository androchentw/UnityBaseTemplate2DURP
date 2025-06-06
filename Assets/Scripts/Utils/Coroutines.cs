using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
  /// <summary>
  ///  The Coroutines class provides static methods for managing coroutines
  ///  without the need for a MonoBehaviour component attached to a game object.
  ///  This is useful if you have a ScriptableObject that needs to run a coroutine.
  /// </summary>
  public static class Coroutines
  {
    private static MonoBehaviour _coroutineRunner;

    public static bool isInitialized => _coroutineRunner != null;

    public static void Initialize(MonoBehaviour runner)
    {
      _coroutineRunner = runner;
    }

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
      if (_coroutineRunner == null)
      {
        throw new InvalidOperationException("CoroutineRunner is not initialized.");
      }

      return _coroutineRunner.StartCoroutine(coroutine);
    }

    public static void StopCoroutine(Coroutine coroutine)
    {
      if (_coroutineRunner != null)
      {
        _coroutineRunner.StopCoroutine(coroutine);
      }
    }

    public static void StopCoroutine(ref Coroutine coroutine)
    {
      if (_coroutineRunner != null && coroutine != null)
      {
        _coroutineRunner.StopCoroutine(coroutine);
        coroutine = null;
      }
    }
  }
}
