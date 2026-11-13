using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokoHex.Parsing
{
    public static class GameBuildingDumper
    {
        public static void DumpExteriorBuildingData(string path, string destination)
        {
            Directory.CreateDirectory(destination);

            string archiveFolder = GameItemDumper.GetArchiveFolder(path);

            using var archive = new GameArchive(archiveFolder);

            byte[] entry40 = archive.Read(40);
            byte[] entry41 = archive.Read(41);
            byte[] entry51 = archive.Read(51);

            const int TypeCount = 256;

            byte[] output = new byte[8 + TypeCount * 2];

            // File identifier.
            System.Text.Encoding.ASCII.GetBytes("PKOEXT01")
                .CopyTo(output, 0);

            for (int type = 0; type < TypeCount; type++)
            {
                int typeOffset = 0x40 + type * 0x5C + 0x32;

                if (typeOffset + 2 > entry51.Length)
                    break;

                ushort variation = System.Buffers.Binary.BinaryPrimitives
                    .ReadUInt16LittleEndian(entry51.AsSpan(typeOffset, 2));

                int variationOffset = 0x40 + variation * 0x10 + 0x04;

                if (variationOffset + 2 > entry41.Length)
                    continue;

                ushort basePart = System.Buffers.Binary.BinaryPrimitives
                    .ReadUInt16LittleEndian(entry41.AsSpan(variationOffset, 2));

                int basePartOffset = 0x40 + basePart * 0x50 + 0x06;

                if (basePartOffset + 2 > entry40.Length)
                    continue;

                ushort itemId = System.Buffers.Binary.BinaryPrimitives
                    .ReadUInt16LittleEndian(entry40.AsSpan(basePartOffset, 2));

                System.Buffers.Binary.BinaryPrimitives
                    .WriteUInt16LittleEndian(
                        output.AsSpan(8 + type * 2, 2),
                        itemId);
            }

            File.WriteAllBytes(
                Path.Combine(destination, "exterior_buildings.bin"),
                output);
        }
    }
}
