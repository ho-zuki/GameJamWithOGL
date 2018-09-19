using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Sho
{
    interface ISpawnedByPlayer
    {
        Vector2 Offset { get; }

        Transform transform { get; }
    }
}