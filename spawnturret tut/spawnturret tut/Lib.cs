using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PS3Lib;

namespace spawnturret_tut
{
    class Lib
    {
        private static PS3API PS3 = new PS3API();
        public static void GetMemoryR(uint Address, ref byte[] Bytes)
        {
            PS3.GetMemory(Address, Bytes);
        }
        private static void GetMem(uint offset, byte[] buffer, SelectAPI API)
        {
            if (API == SelectAPI.ControlConsole)
            {
                GetMemoryR(offset, ref buffer);
            }
            else if (API == SelectAPI.TargetManager)
            {
                GetMemoryR(offset, ref buffer);
            }
        }
        public static void SetMemory(UInt32 Address, Byte[] bytes)
        {
            PS3.SetMemory(Address, bytes);
        }
        public static Byte[] GetMem(UInt32 Address, Int32 Length)
        {
            Byte[] buff = new Byte[Length];
            GetMemoryR(Address,ref buff);
            return buff;
        }
        public static int ReadInt(uint Offset)
        {
            byte[] buffer = new byte[4];
            GetMemoryR(Offset, ref buffer);
            Array.Reverse(buffer);
            int Value = BitConverter.ToInt32(buffer, 0);
            return Value;
        }
        public static float[] ReadFloatLength(uint Offset, int Length)
        {
            byte[] buffer = new byte[Length * 4];
            GetMemoryR(Offset, ref buffer);
            Lib.ReverseBytes(buffer);
            float[] Array = new float[Length];
            for (int i = 0; i < Length; i++)
            {
                Array[i] = BitConverter.ToSingle(buffer, (Length - 1 - i) * 4);
            }
            return Array;
        }
        public static Byte[] SetMem(UInt32 Address, Int32 Length)
        {
            Byte[] bytes = new Byte[Length];
            SetMemory(Address, bytes);
            return bytes;
        }
        public static float ReadFloat(UInt32 offset)
        {
            byte[] myBuffer = Lib.GetMem(offset, 4);
            Array.Reverse(myBuffer, 0, 4);
            return BitConverter.ToSingle(myBuffer, 0);
        }
        public static void WriteFloat(UInt32 offset, float input)
        {
            byte[] array = new byte[4];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            Array.Reverse(array, 0, 4);
            Lib.SetMemory(offset, array);
        }
       
        public static Byte ReadByte(UInt32 address)
        {
            return Lib.GetMem(address, 1)[0];
        }

        public static Byte[] ReadBytes(UInt32 address, Int32 length)
        {
            return Lib.GetMem(address, length);
        }

        public static Int32 ReadInt32(UInt32 address)
        {
            Byte[] memory = Lib.GetMem(address, 4);
            Array.Reverse(memory, 0, 4);
            return BitConverter.ToInt32(memory, 0);
        }
                             
        public static float ReadSingle(UInt32 address)
        {
            Byte[] memory = Lib.GetMem(address, 4);
            Array.Reverse(memory, 0, 4);
            return BitConverter.ToSingle(memory, 0);
        }

        public static float[] ReadSingle(UInt32 address, Int32 length)
        {
            Byte[] memory = Lib.GetMem(address, length * 4);
            ReverseBytes(memory);
            float[] numArray = new float[length];
            for (Int32 i = 0; i < length; i++)
            {
                numArray[i] = BitConverter.ToSingle(memory, ((length - 1) - i) * 4);
            }
            return numArray;
        }

        public static string ReadString(UInt32 address)
        {
            Int32 length = 40;
            Int32 num2 = 0;
            string source = "";
            do
            {
                Byte[] memory = Lib.GetMem(address + ((UInt32)num2), length);
                source = source + Encoding.UTF8.GetString(memory);
                num2 += length;
            }
            while (!source.Contains<char>('\0'));
            Int32 inPS3 = source.IndexOf('\0');
            string str2 = source.Substring(0, inPS3);
            source = string.Empty;
            return str2;
        }

        public static Byte[] ReverseBytes(Byte[] toReverse)
        {
            Array.Reverse(toReverse);
            return toReverse;
        }

        public static void WriteByte(UInt32 address, Byte input)
        {
            Lib.SetMemory(address, new Byte[] { input });
        }

        public static void WriteBytes(UInt32 address, Byte[] input)
        {
            Lib.SetMemory(address, input);
        }

        public static bool WriteBytesToggle(uint Offset, Byte[] On, Byte[] Off)
        {
            bool flag = ReadByte(Offset) == On[0];
            WriteBytes(Offset, !flag ? On : Off);
            return flag;
        }

        public static void WriteInt16(UInt32 address, short input)
        {
            Byte[] array = new Byte[2];
            ReverseBytes(BitConverter.GetBytes(input)).CopyTo(array, 0);
            Lib.SetMemory(address, array);
        }

        public static void WriteInt32(UInt32 address, Int32 input)
        {
            Byte[] array = new Byte[4];
            ReverseBytes(BitConverter.GetBytes(input)).CopyTo(array, 0);
            Lib.SetMemory(address, array);
        }
      
        public static void WriteSingle(UInt32 address, float input)
        {
            Byte[] array = new Byte[4];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            Array.Reverse(array, 0, 4);
            Lib.SetMemory(address, array);
        }

        public static void WriteSingle(UInt32 address, float[] input)
        {
            Int32 length = input.Length;
            Byte[] array = new Byte[length * 4];
            for (Int32 i = 0; i < length; i++)
            {
                ReverseBytes(BitConverter.GetBytes(input[i])).CopyTo(array, (Int32)(i * 4));
            }
            Lib.SetMemory(address, array);
        }

        public static void WriteString(UInt32 address, String input)
        {
            Byte[] Bytes = Encoding.UTF8.GetBytes(input);
            Array.Resize<byte>(ref Bytes, Bytes.Length + 1);
            Lib.SetMemory(address, Bytes);
        }

        public static void WriteUInt16(UInt32 address, ushort input)
        {
            Byte[] array = new Byte[2];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            Array.Reverse(array, 0, 2);
            Lib.SetMemory(address, array);
        }
                
        public static void WriteUInt32(UInt32 address, UInt32 input)
        {
            Byte[] array = new Byte[4];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            Array.Reverse(array, 0, 4);
            Lib.SetMemory(address, array);
        }

    }

}
