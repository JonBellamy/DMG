﻿using System;
namespace DMG
{
    public class TileMap
    {
        IGpu gpu;
        IMemoryReader memory;
        readonly ushort vramOffset;

        // The Game Boy contains two 32x32 tile background maps in VRAM at addresses 9800h-9BFFh and 9C00h-9FFFh.
        // Each can be used either to display "normal" background, or "window" background.
        public TileMap(IGpu gpu, IMemoryReader memory, ushort vramOffset)
        {
            if(vramOffset != 0x9800 && vramOffset != 0x9C00)
            {
                throw new ArgumentException("Background map must be located at cofrrect address");
            }
            this.vramOffset = vramOffset;

            this.gpu = gpu;
            this.memory = memory;
        }


        byte GetTileIndex(byte index)
        {
            return memory.ReadByte((ushort)(vramOffset + index));
        }


        // Which tile occupies the x,y in the 256x256 screen space?
        byte TileIndexFromXY(byte x, byte y)
        {
            byte tileX = (byte) (x / 8);
            byte tileY = (byte) (y / 8);
            return memory.ReadByte((ushort)(vramOffset + (tileY * 32) + tileX));
        }


        public Tile TileFromXY(byte x, byte y)
        {
            byte tileIndex = TileIndexFromXY(x, y);
          
            ushort vramPointer;
            if (gpu.MemoryRegisters.LCDC.BgAndWindowTileAddressingMode == 0)
            {
                sbyte signedTileIndex = (sbyte)tileIndex;
                // The "8800 method" uses $9000 as its base pointer and uses a signed addressing
                vramPointer = (ushort)(0x9000 + (short)(signedTileIndex * 16));
            }
            else
            {
                // The "8000 method" uses $8000 as its base pointer and uses an unsigned addressing,
                vramPointer = (ushort)(0x8000 + (ushort)(tileIndex * 16));
            }

            return gpu.GetTileByVRamAdrress(vramPointer);
        }
    }
}