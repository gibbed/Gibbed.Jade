using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Gibbed.Helpers;

namespace Gibbed.Jade.FileFormats
{
	[StructLayout(LayoutKind.Sequential)]
	public class BigFileHeader
	{
		public UInt32 Magic;
		public UInt32 Unknown01;
		public UInt32 Unknown02;
		public UInt32 Unknown03;
		public UInt32 Unknown04;
		public UInt32 Unknown05;
		public UInt32 Unknown06;
		public UInt32 Unknown07;
		public UInt32 Unknown08;
		public UInt32 BlockCount;
		public UInt32 Unknown10;

		public void Swap()
		{
			this.Unknown01 = this.Unknown01.Swap();
			this.Unknown02 = this.Unknown02.Swap();
			this.Unknown03 = this.Unknown03.Swap();
			this.Unknown04 = this.Unknown04.Swap();
			this.Unknown05 = this.Unknown05.Swap();
			this.Unknown06 = this.Unknown06.Swap();
			this.Unknown07 = this.Unknown07.Swap();
			this.Unknown08 = this.Unknown08.Swap();
			this.BlockCount = this.BlockCount.Swap();
			this.Unknown10 = this.Unknown10.Swap();
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public class BigBlockHeader
	{
		public UInt32 Count;
		public UInt32 Unknown1;
		public UInt32 Offset;
		public UInt32 Unknown3;
		public UInt32 Unknown4;
		public UInt32 Unknown5;

		public void Swap()
		{
			this.Count = this.Count.Swap();
			this.Unknown1 = this.Unknown1.Swap();
			this.Offset = this.Offset.Swap();
			this.Unknown3 = this.Unknown3.Swap();
			this.Unknown4 = this.Unknown4.Swap();
			this.Unknown5 = this.Unknown5.Swap();
		}
	}

	public class BigEntry
	{
	}

	public class BigFile
	{
		public void Test()
		{
			Stream input = File.OpenRead("T:\\Games\\Singleplayer\\Ubisoft\\Warrior Within\\prince.bf");
			this.Read(input);
			input.Close();
		}

		public Dictionary<UInt32, BigEntry> Entries;

		public void Read(Stream input)
		{
			BigFileHeader header = input.ReadStructure<BigFileHeader>();

			if (BitConverter.IsLittleEndian == false)
			{
				header.Swap();
			}

			List<BigBlockHeader> blocks = new List<BigBlockHeader>();
			for (int i = 0; i < header.BlockCount; i++)
			{
				BigBlockHeader block = input.ReadStructure<BigBlockHeader>();

				if (BitConverter.IsLittleEndian == false)
				{
					block.Swap();
				}

				blocks.Add(block);
			}
			
			foreach (BigBlockHeader block in blocks)
			{
				if (block.Count > 0)
				{
					input.Seek(block.Offset, SeekOrigin.Begin);

					byte[] entries = new byte[block.Count * 8];
					input.Read(entries, 0, entries.Length);

					for (int i = 0; i < blocks[i].Count; i++)
					{
						UInt32 offset = BitConverter.ToUInt32(entries, (i * 8) + 0);
						UInt32 hash = BitConverter.ToUInt32(entries, (i * 8) + 4);

						input.Seek(offset, SeekOrigin.Begin);

					}
				}
			}
		}
	}
}
