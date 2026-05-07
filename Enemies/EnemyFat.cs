using Godot;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
public partial class EnemyFat : Enemy
{
    /* public EnemyFat(int damage, int speed, int bounty, int life) : base(damage, speed, bounty, life)
     {
     }*/

    public override void _Ready()
    {
        base._Ready();
        this.Damage = 2;
        this.Speed = 50;
        this.Bounty = 5;
        this.Life = 100;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}
