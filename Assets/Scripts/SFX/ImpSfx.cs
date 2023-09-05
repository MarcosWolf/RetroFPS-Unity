using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSfx : MonoBehaviour
{
    public AudioSource impMelee, impRanged, impSight1, impSight2, impHit, impDeath1, impDeath2;

    public void sfxImpRanged() { impRanged.Play(); }
    public void sfxImpHit() { impHit.Play(); }
    public void sfxImpDeath1() { impDeath1.Play(); }
    public void sfxImpDeath2() { impDeath2.Play(); }
}
