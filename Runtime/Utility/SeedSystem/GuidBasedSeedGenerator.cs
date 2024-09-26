using System;
using UnityEngine;

namespace Studio23.SS2.SaveSystem.Utilities
{
    [CreateAssetMenu(fileName = "Guid Seed Gen", menuName = "Studio-23/SaveSystem/SeedGenerator/Guid", order = 1)]
    public class GuidBasedSeedGenerator : SeedGeneratorBase
    {
        
        public override string GenerateSeed()
        {
            return Guid.NewGuid().ToString();
        }


       
    }
}