using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HoneboneExtentions
{
    public static void Visualize(this Ray ray,float range)
    {
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
    }
    public static Vector3 ToVector3(this Vector2 vector) { return new Vector3(vector.x, vector.y, 0); }

    public static bool CheckRaycastHit(this RaycastHit2D hit,string tagName)
    {
        return hit.collider != null && hit.collider.CompareTag(tagName);
    }
    
    public static bool Dice(this float chance)
    {
        return Random.Range(0f, 100f) < chance;
    }
    public static bool Dice(this int chance)
    {
        return Random.Range(0f, 100f) < chance;
    }

    public static string ToStr(this TwoPlayerManager.WandColor color)
    {
        Dictionary<TwoPlayerManager.WandColor, string> dic = new Dictionary<TwoPlayerManager.WandColor, string>() {
            {TwoPlayerManager.WandColor.White,"White" },{TwoPlayerManager.WandColor.Orange,"Orange" },{TwoPlayerManager.WandColor.Yellow,"Yellow" },
            {TwoPlayerManager.WandColor.Green,"Green" },{TwoPlayerManager.WandColor.Blue,"Blue" },{TwoPlayerManager.WandColor.Purple,"Purple" },
            {TwoPlayerManager.WandColor.Red,"Red" },{TwoPlayerManager.WandColor.Black,"Black" },{TwoPlayerManager.WandColor.Other,"Other" }

        };
        return dic[color];
    }
}
