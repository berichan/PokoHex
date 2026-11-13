using PokoHex.Core;
using PokoHex.Core.Strings;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PokoHex.Core.Strings
{
    public sealed class ExteriorBuildingData
    {
        private static ushort[] HouseTypes = [ 15, 19, 23, 27, 76 ];
        public static Dictionary<ushort, string> InteriorTypes = new Dictionary<ushort, string>();
        private static void InitBuildings()
        {
            foreach (var i in (ushort[])[11, 58, 59]) InteriorTypes[i] = "Area Gate";
            foreach (var i in (ushort[])[61, 62, 67]) InteriorTypes[i] = "Area Gate";
            foreach (var i in (ushort[])[60, 63])     InteriorTypes[i] = "Area Gate";
            foreach (var i in (ushort[])[64, 65, 66]) InteriorTypes[i] = "Area Gate";
        }

        private readonly ushort[] _itemIds;

        private ExteriorBuildingData(ushort[] itemIds)
        {
            _itemIds = itemIds;
            InitBuildings();
        }

        public static ExteriorBuildingData Load()
        {
            byte[] data = ResourceUtil.GetBinaryResource("exterior_buildings.bin");
            if (data.Length != 8 + 256 * 2 || Encoding.ASCII.GetString(data, 0, 8) != "PKOEXT01")
                throw new InvalidDataException("Invalid exterior building data file.");

            var itemIds = new ushort[256];
            for (int i = 0; i < itemIds.Length; i++)
                itemIds[i] = BinaryPrimitives.ReadUInt16LittleEndian(data.AsSpan(8 + i * 2, 2));

            return new ExteriorBuildingData(itemIds);
        }

        public int GetItemId(ushort type)
        {
            if ((uint)type >= (uint)_itemIds.Length)
                throw new ArgumentOutOfRangeException(nameof(type));
            return _itemIds[type];
        }

        private ushort PreviousBuildingType;
        public string GetItemName(ushort type)
        {
            int itemId = GetItemId(type);

            if (itemId >= GameInfo.Strings.itemlist.Length)
                throw new InvalidDataException($"Invalid building");

            string name = GameInfo.Strings.itemlist[itemId];

            if (HouseTypes.Contains(type))
            {
                name = $"{name} (1F)";
            }
            if (name == "(Empty)")
            {
                if (HouseTypes.Contains(PreviousBuildingType))
                {
                    name = $"{GetItemName(PreviousBuildingType)[..^5]} (2F)";
                    PreviousBuildingType = 0;
                }
                else if (InteriorTypes.ContainsKey(type))
                    name = InteriorTypes[type];
            }

            PreviousBuildingType = type;
            return name;
        }
    }
}
