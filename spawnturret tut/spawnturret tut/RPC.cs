﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace spawnturret_tut
{
    public class RPC
    {
        #region Vezah's FPS RPC
        public static uint func_address = 0x0277208; //FPS Address 1.24
        /*
         * MW3 FPS RPC by VezahHFH!
         * This function get written at the FPS Offset!
         *
             lis r28, 0x1005
             lwz r12, 0x48(r28)
             cmpwi r12, 0
             beq loc_277290
             lwz r3, 0x00(r28)
             lwz r4, 0x04(r28)
             lwz r5, 0x08(r28)
             lwz r6, 0x0C(r28)
             lwz r7, 0x10(r28)
             lwz r8, 0x14(r28)
             lwz r9, 0x18(r28)
             lwz r10, 0x1C(r28)
             lwz r11, 0x20(r28)
             lfs f1, 0x24(r28)
             lfs f2, 0x28(r28)
             lfs f3, 0x2C(r28)
             lfs f4, 0x30(r28)
             lfs f5, 0x34(r28)
             lfs f6, 0x38(r28)
             lfs f7, 0x3C(r28)
             lfs f8, 0x40(r28)
             lfs f9, 0x44(r28)
             mtctr r12
             bctrl
             li r4, 0
             stw r4, 0x48(r28)
             stw r3, 0x4C(r28)
             stfs f1, 0x50(r28)
             b loc_277290
         */
        public static uint GetFuncReturn()
        {
            byte[] ret = new byte[4];
            Lib.GetMemoryR(0x114AE64, ref ret);
            Array.Reverse(ret);
            return BitConverter.ToUInt32(ret, 0);
        }
        public static void Enable()
        {
            byte[] CheckRPC = new byte[1];
            Lib.GetMemoryR(0x27720C, ref CheckRPC);
            if (CheckRPC[0] == 0x80)
            {
                byte[] WritePPC = new byte[] {0x3F,0x80,0x10,0x05,0x81,0x9C,0x00,0x48,0x2C,0x0C,0x00,0x00,0x41,0x82,0x00,0x78,
                                        0x80,0x7C,0x00,0x00,0x80,0x9C,0x00,0x04,0x80,0xBC,0x00,0x08,0x80,0xDC,0x00,0x0C,
                                        0x80,0xFC,0x00,0x10,0x81,0x1C,0x00,0x14,0x81,0x3C,0x00,0x18,0x81,0x5C,0x00,0x1C,
                                        0x81,0x7C,0x00,0x20,0xC0,0x3C,0x00,0x24,0xC0,0x5C,0x00,0x28,0xC0,0x7C,0x00,0x2C,
                                        0xC0,0x9C,0x00,0x30,0xC0,0xBC,0x00,0x34,0xC0,0xDC,0x00,0x38,0xC0,0xFC,0x00,0x3C,
                                        0xC1,0x1C,0x00,0x40,0xC1,0x3C,0x00,0x44,0x7D,0x89,0x03,0xA6,0x4E,0x80,0x04,0x21,
                                        0x38,0x80,0x00,0x00,0x90,0x9C,0x00,0x48,0x90,0x7C,0x00,0x4C,0xD0,0x3C,0x00,0x50,
                                        0x48,0x00,0x00,0x14};
                Lib.SetMemory(func_address,  new byte [] { 0x41 });
                Lib.SetMemory(func_address + 4, WritePPC);
                Lib.SetMemory(func_address, new byte[] { 0x40 });
                Thread.Sleep(10);
                RPC.DestroyAll();


            }
            else if (CheckRPC[0] == 0x3F)
            {
                MessageBox.Show("RPC Is Already Enabled\nWe Don't Want You To Freeze Now Do We ", "Saved Your Ass :P");
            }
        }
        public static Int32 Call(UInt32 address, params Object[] parameters)
        {
            Int32 length = parameters.Length;
            Int32 index = 0;
            UInt32 count = 0;
            UInt32 Strings = 0;
            UInt32 Single = 0;
            UInt32 Array = 0;
            while (index < length)
            {
                if (parameters[index] is Int32)
                {
                    Lib.WriteInt32(0x10050000 + (count * 4), (Int32)parameters[index]);
                    count++;
                }
                else if (parameters[index] is UInt32)
                {
                    Lib.WriteUInt32(0x10050000 + (count * 4), (UInt32)parameters[index]);
                    count++;
                }
                else if (parameters[index] is Int16)
                {
                    Lib.WriteInt16(0x10050000 + (count * 4), (Int16)parameters[index]);
                    count++;
                }
                else if (parameters[index] is UInt16)
                {
                    Lib.WriteUInt16(0x10050000 + (count * 4), (UInt16)parameters[index]);
                    count++;
                }
                else if (parameters[index] is Byte)
                {
                    Lib.WriteByte(0x10050000 + (count * 4), (Byte)parameters[index]);
                    count++;
                } //Should work now :D let me try
                else
                {
                    UInt32 pointer;
                    if (parameters[index] is String)
                    {
                        pointer = 0x10052000 + (Strings * 0x400);
                        Lib.WriteString(pointer, Convert.ToString(parameters[index]));
                        Lib.WriteUInt32(0x10050000 + (count * 4), pointer);
                        count++;
                        Strings++;
                    }
                    else if (parameters[index] is Single)
                    {
                        WriteSingle(0x10050024 + (Single * 4), (Single)parameters[index]);
                        Single++;
                    }
                    else if (parameters[index] is Single[])
                    {
                        Single[] Args = (Single[])parameters[index];
                        pointer = 0x10051000 + Array * 4;
                        WriteSingle(pointer, Args);
                        Lib.WriteUInt32(0x10050000 + count * 4, pointer);
                        count++;
                        Array += (UInt32)Args.Length;
                    }

                }
                index++;
            }
            Lib.WriteUInt32(0x10050048, address);
            Thread.Sleep(20);
            return Lib.ReadInt32(0x1005004c);
        }
        private static void WriteSingle(uint address, float input)
        {
            byte[] array = new byte[4];
            BitConverter.GetBytes(input).CopyTo(array, 0);
            Array.Reverse(array, 0, 4);
            Lib.SetMemory(address, array);
        }
        private static byte[] ReverseBytes(byte[] inArray)
        {
            Array.Reverse(inArray);
            return inArray;
        }
        private static void WriteSingle(uint address, float[] input)
        {
            int length = input.Length;
            byte[] array = new byte[length * 4];
            for (int i = 0; i < length; i++)
            {
                ReverseBytes(BitConverter.GetBytes(input[i])).CopyTo(array, (int)(i * 4));
            }
            Lib.SetMemory(address, array);
        }
        public static void DestroyAll()
        {
            Byte[] clear = new Byte[0xB4 * 1024];
            Lib.SetMemory(0xF0E10C, clear);
        }
        
        public static Single[] ReadSingle(uint address, int length)
        {
            byte[] mem = Lib.ReadBytes(address, length * 4);
            Array.Reverse(mem);
            float[] numArray = new float[length];
            for (int index = 0; index < length; ++index)
                numArray[index] = BitConverter.ToSingle(mem, (length - 1 - index) * 4);
            return numArray;
        }
        #endregion
    }
}
           