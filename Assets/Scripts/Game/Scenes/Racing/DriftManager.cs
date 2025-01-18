using System.Collections;
using UnityEngine;

public class DriftManager
{
    private int _score;
    private Coroutine _driftScoreCalculator;

    private const float _scoreDelay = 0.01f;

    private bool _isInDrift;

    public void Subscribe()
    {
        CarEvents.OnDrift += Drift;
    }

    public void UnSubscribe()
    {
        CarEvents.OnDrift -= Drift;
    }

    private void Drift(bool state)
    {
        if(state && !_isInDrift)
        {
            _isInDrift = true;
            _driftScoreCalculator = CoroutineServices.instance.StartRoutine(DriftCalculator());
        }
        else if(!state && _isInDrift) 
        {
            _isInDrift = false;

            CoroutineServices.instance.StopRoutine(_driftScoreCalculator);

            ResourcesManager.Instance.ModifyResource(ResourceTypes.TotalPoints, _score);
            GameEvents.PointsUpdate(_score);

            _score = 0;
            GameEvents.DriftScoreUpdate(_score, false);
        }
    }

    private IEnumerator DriftCalculator()
    {
        while (true)
        {
            _score += 1;
            GameEvents.DriftScoreUpdate(_score, true);
            yield return new WaitForSeconds(_scoreDelay);
        }
    }
}