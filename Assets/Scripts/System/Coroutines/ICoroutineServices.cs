using System.Collections;
using UnityEngine;

public interface ICoroutineServices 
{
    Coroutine StartRoutine(IEnumerator enumerator);
    void StopRoutine(Coroutine routine);
}
