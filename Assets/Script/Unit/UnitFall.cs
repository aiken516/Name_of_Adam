using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnitFall : MonoBehaviour
{
    [SerializeField] private int _maxCount;
    [SerializeField, ReadOnly] private int _currentCount;
    private bool _isEdified;

    [Header("타락 이벤트")]
    public UnityEvent UnitFallEvent;

    public void Init(int maxCount)
    {
        _maxCount = maxCount;
        _currentCount = 0;
    }

    public void ChangeFall(int value)
    {
        if (_isEdified) return;

        _currentCount += value;

        if (_currentCount <= 0)
        {
            _currentCount = 0;
        }
        else if (_currentCount >= _maxCount)
        {
            UnitFallEvent.Invoke();
            _currentCount = 0;
        }

        // Add Fall Change Event (EX. UI)
        Debug.Log("FALL : " + value + ", CurFALL ; " + _currentCount);
    }

    public void Editfy()
    {
        _isEdified = true;
    }

    public int GetCurrentFallCount()
    {
        return _currentCount;
    }
}