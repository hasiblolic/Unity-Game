using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraExecution
{
    Update,
    FixedUpdate,
    LateUpdate
}

public enum AttackInputs
{
    rb,
    rt,
    lb,
    lt,
    none,
}

public enum ScalingValues
{
    S,
    A,
    B,
    C,
    D,
    E,
    NONE
}

public enum DamageType
{
    Bleed,
    Elemental,
    Fire,
    Lightning,
    Magic,
    Poison,

    // Swords, Straightswords, Greatswords, Axes, Greataxes,
    Regular,
    // Curved Swords, Katanas, Daggers, Halberds,
    Slash,
    // Hammers, Greathammers, Clubs,
    Strike,
    // Spears, (Thrusting Swords - Estocs / Rapiers), Bows, Crossbows
    Thrust,
    Toxic,
}