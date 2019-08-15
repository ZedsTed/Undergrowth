using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Eventually turn this into an SO.
/// </summary>
public class GameConstants : SingletonDontCreate<GameConstants>
{
    public float GrowthMultiplier = 0.01f;

    public float WaterDrainMultiplier = 0.05f;

    public float EnergyProductionMultiplier = 0.05f;

    public float NotificationRingSpeed = 5f;
}
