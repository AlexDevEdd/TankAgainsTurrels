using System;

public interface IDamageable<T> where T : struct
{ 
    public void Damage(T damageAmount);
}

