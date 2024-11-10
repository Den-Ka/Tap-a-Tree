using System;
using UnityEngine;

namespace Tap_a_Tree
{
    public interface IHealth
    {
        event Action HealthChanged;
        event Action Died; 
        
        float MaxHealth { get; }
        float CurrentHealth { get; }
    }
}