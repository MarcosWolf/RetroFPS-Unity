using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects instance;


    public AudioSource playerHit, playerDeath;
    
    public AudioSource impMelee, impRanged, impSight1, impSight2, impHit, impDeath1, impDeath2;

    public AudioSource smgFire, smgSlide, smgReload1, smgReload2;
    public AudioSource shotgunFire, shotgunPump, shotgunReload, shotgunEmpty;
    public AudioSource casingDrop, shellDrop;

    void Awake()
    {
        instance = this;
    }

    // Player
    public void sfxPlayerHit() { playerHit.Play(); }
    public void sfxPlayerDeath() { playerDeath.Play(); }
    
    // Monster
    // Imp
    public void sfxImpRanged() { impRanged.Play(); }
    public void sfxImpHit() { impHit.Play(); }
    public void sfxImpDeath1() { impDeath1.Play(); }
    public void sfxImpDeath2() { impDeath2.Play(); }

    // Weapons
    // Generic
    public void sfxCasingDrop() { casingDrop.Play(); }
    public void sfxShellDrop() { shellDrop.Play(); }

    // Smg
    public void sfxSmgFire() { smgFire.Play(); }
    public void sfxSmgSlide() { smgSlide.Play(); }
    public void sfxSmgReload1() { smgReload1.Play(); }
    public void sfxSmgReload2() { smgReload2.Play(); }

    // Shotgun
    public void sfxShotgunFire() { shotgunFire.Play(); }
    public void sfxShotgunPump() { shotgunPump.Play(); }
    public void sfxShotgunReload() { shotgunReload.Play(); }
    public void sfxShotgunEmpty() { shotgunEmpty.Play(); }
}
