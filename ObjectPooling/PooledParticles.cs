﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledParticles : PoolObject
{
    void OnParticleSystemStopped()
    {
        ReturnSelf();
    }
}
