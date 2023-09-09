using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySfx : MonoBehaviour
{
    public AudioSource enemyAttackMelee, enemyAttackRanged, enemyDeath1, enemyDeath2, enemyDeath3, enemyHit, enemyNearby, enemySight1, enemySight2, enemySight3;

    public void sfxEnemyAttackMelee() { enemyAttackMelee.Play(); }
    public void sfxEnemyAttackRanged() { enemyAttackRanged.Play(); }
    public void sfxEnemyDeath1() {  enemyDeath1.Play(); }
    public void sfxEnemyDeath2() {  enemyDeath2.Play(); }
    public void sfxEnemyDeath3() {  enemyDeath3.Play(); }
    public void sfxEnemyHit() {  enemyHit.Play(); }
    public void sfxEnemyNearby() {  enemyNearby.Play(); }
    public void sfxEnemySight1() {  enemySight1.Play(); }
    public void sfxEnemySight2() {  enemySight2.Play(); }
    public void sfxEnemySight3() {  enemySight3.Play(); }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
