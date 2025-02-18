using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Insprctor: Enemy")]

    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float showDamageDuration = 0.1f;
    public float powerUpDropChance = 1f;

    [Header("Set in Dynamically: Enemy")]
    public Color[] originalColor;
    public Material[] materials;
    public bool showDamage = false;
    public float damageDoneTime;
    public bool notifideOFDestruction = false;

    protected BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        materials = Utils.GetAllMaterials(gameObject);
        originalColor = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++) 
        {
            originalColor[i] = materials[i].color;
        }
    }

    public Vector3 pos 
    {
        get 
        {
            return (this.transform.position);
        }

        set 
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();

        if (showDamage && Time.time > damageDoneTime) 
        {
            UnShowDamage();
        }

        if (bndCheck != null && bndCheck.offDown) 
        {
             Destroy(gameObject);   
        }
    }

    public virtual void Move() 
    {
        Vector3 temPos = pos;
        temPos.y -= speed * Time.deltaTime;
        pos = temPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO  = coll.gameObject;
        switch (otherGO.tag) 
        {
            case "ProjectileHero": 
                Projectile p = otherGO.GetComponent<Projectile>();

                if (!bndCheck.isOnScreen) 
                {
                    Destroy(otherGO);
                    break;
                }
                ShowDamage();
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0) 
                {
                    if (!notifideOFDestruction) 
                    {
                        Main.S.ShipDestroyed(this);
                    }

                    notifideOFDestruction = true;
                    Destroy(this.gameObject);
                }

                Destroy(otherGO);
                break;
                
            default:
                print("Enemy hit by non - ProjectileHero:" +  otherGO.name); 
                break;
        }
    }

    void ShowDamage() 
    {
        foreach (Material m in materials) 
        {
            m.color = Color.red;
        }

        showDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage() 
    {
        for (int i = 0; i < materials.Length; i ++) 
        {
            materials[i].color = originalColor[i];
        }

        showDamage = false; 
    }
}   
