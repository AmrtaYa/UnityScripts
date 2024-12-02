using System;
using System.Collections;
using System.Collections.Generic;
using GJC.Helper;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// 寻敌算法存放
/// </summary>
public class SeekFactory : SharpSington<SeekFactory>, ICanInit
{
   private Dictionary<SeekType, Seek> seeksDir;

   public void Init()
   {
      seeksDir = new Dictionary<SeekType, Seek>(10);
      InitDir();
   }

   private void InitDir()
   {
      var seekT = Enum.GetNames(typeof(SeekType));
      foreach (var VARIABLE in seekT)
      {
         Type t = Type.GetType(VARIABLE+"Seek");
         var instance = Activator.CreateInstance(t);
         seeksDir.Add(Enum.Parse<SeekType>(VARIABLE),(Seek)instance);
      }
   }

   public Seek GetSeek(SeekType seekType)
   {
      return seeksDir[seekType];
   }
}

public interface Seek
{
   public bool FindEnemy(Unit finder,ExData data);
}

public struct ExData
{
   
}

public class BoxSeek : Seek
{
   public bool FindEnemy(Unit finder,ExData data)
   {
      
      
      return false;
   }
}
public class RaySeek : Seek
{
   public bool FindEnemy(Unit finder,ExData data)
   {
      Ray2D ray = new Ray2D(finder.entity.position,finder.entity.transform.right);
      return false;
   }
}
public enum SeekType
{
   Box,
   Ray,
  // Circle,
}