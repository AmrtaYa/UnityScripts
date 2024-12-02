using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVEInfo
{
   public Vector2 invtralRange;//从x到y秒内随机秒数间隔 出一波怪
   public int waveNum;//有几波
   public List<int> enemyNum;//每波有几个怪

   public PVEInfo(int eNum)
   {
      enemyNum = new List<int>(waveNum);
      for (int i = 0; i < waveNum; i++)
      {
         enemyNum.Add(eNum);
      }
   }

   public PVEInfo(List<int> waveEnemyNum)
   {
      enemyNum = waveEnemyNum;
   }
}
