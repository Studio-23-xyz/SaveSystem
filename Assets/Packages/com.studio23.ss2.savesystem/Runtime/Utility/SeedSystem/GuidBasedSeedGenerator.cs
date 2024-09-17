
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Studio23.SS2.SaveSystem.Utilities
{
    [CreateAssetMenu(fileName = "Guid Seed Gen", menuName = "Studio-23/SaveSystem/SeedGenerator/Guid", order = 1)]
    public class GuidBasedSeedGenerator : SeedGeneratorBase
    {
        
        public override string GenerateSeed()
        {
            Guid guid = Guid.NewGuid();
            string newSeed = guid.ToString();
            int tempSeed = GetNumericSeedFromGuid(guid);
            Debug.Log($"Temp seed {tempSeed} ");
            Random.InitState(tempSeed);
            return newSeed;
        }


        private int GetNumericSeedFromGuid(Guid guid)
        {
            byte[] guidBytes = guid.ToByteArray();
            // Convert part of the GUID to an integer (using first 4 bytes for simplicity)
            int numericSeed = BitConverter.ToInt32(guidBytes, 0);
            return numericSeed;
        }
    }
}