using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects instance;

    public AudioSource playerHit, playerDeath;

    public AudioSource smgFire, smgSlide, smgReload1, smgReload2;
    public AudioSource shotgunFire, shotgunPump, shotgunReload;
    public AudioSource casingDrop, shellDrop, weaponEmpty, swapWeapon;

    void Awake()
    {
       instance = this;
    }

    // Player
    public void sfxPlayerHit() { playerHit.Play(); }
    public void sfxPlayerDeath() { playerDeath.Play(); }
    
    // Monster
    // Imp

    // Weapons
    // Generic
    public void sfxCasingDrop() { casingDrop.Play(); }
    public void sfxShellDrop() { shellDrop.Play(); }
    public void sfxWeaponEmpty() { weaponEmpty.Play(); }
    public void sfxSwapWeapon() { swapWeapon.Play(); }

    // Smg
    public void sfxSmgFire() { smgFire.Play(); }
    public void sfxSmgSlide() { smgSlide.Play(); }
    public void sfxSmgReload1() { smgReload1.Play(); }
    public void sfxSmgReload2() { smgReload2.Play(); }

    // Shotgun
    public void sfxShotgunFire() { shotgunFire.Play(); }
    public void sfxShotgunPump() { shotgunPump.Play(); }
    public void sfxShotgunReload() { shotgunReload.Play(); }
}
