using System.Collections;
using UnityEngine;

namespace Scenes
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}