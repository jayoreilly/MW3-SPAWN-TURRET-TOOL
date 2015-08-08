using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PS3Lib;

using System.Threading;
namespace spawnturret_tut
{

    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        private static PS3API PS3 = new PS3API();

        public Form1()
        {
            InitializeComponent();
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try   
            {
                            PS3.ConnectTarget();
                            PS3.AttachProcess();
                            if (PS3.AttachProcess())
                            label27.Text = PS3.GetCurrentAPIName() + " Connected / Attached";
                            RPC.Enable();
            }
            catch
            {
                label27.Text = "Failed To Connect and Attach";
                
            } 
        }

        private void button14_Click(object sender, EventArgs e)
        {
             //Copy to Clipboard/ auto code func.
            Int32 Client = 0;
            {
                Single[] Origin = Lib.ReadSingle(Offsets.Funcs.G_Client((Int32)Client) + 0x1C, 3); Origin[2] += 50;
                Single[] Angles = Lib.ReadSingle(Offsets.Funcs.G_Client((Int32)Client) + 0x158, 3);
                string a = "Spawn_Turrent.OnValues(" + @"""sentry_minigun_mp""," + @"""weapon_minigun""," + (Int32)Origin[0] + ", " + (Int32)Origin[1] + ", " + (Int32)Origin[2] + ", " + (Int32)Angles[0] + ", " + (Int32)Angles[1] + ", " + (Int32)Angles[2] + ");";
                string b = "Your Pos : X: " + (Int32)Origin[0] + "  Y: " + (Int32)Origin[1] + "  Z: " + (Int32)Origin[2];
                string c = "Your Angles : Pitch: " + (Int32)Angles[0] + "  Yaw: " + (Int32)Angles[1];
                label22.Text = b;
                label24.Text = c;
                Clipboard.SetText(a);
                MessageBox.Show(a, "Copied to Clipboard");
            }
        }

        private void POS_Tick(object sender, EventArgs e) 
        {
            Int32 Client = 0;
            {   // Position In Real Time.
                Single[] Origin = Lib.ReadSingle(Offsets.Funcs.G_Client((Int32)Client) + 0x1C, 3); Origin[2] += 50;
                Single[] Angles = Lib.ReadSingle(Offsets.Funcs.G_Client((Int32)Client) + 0x158, 3);
                string a = "Your Pos : X: " + (Int32)Origin[0] + "  Y: " + (Int32)Origin[1] + "  Z: " + (Int32)Origin[2];
                string b = "Your Angles : Pitch: " + (Int32)Angles[0] + "  Yaw: " + (Int32)Angles[1];
                label22.Text = a;
                label24.Text = b;
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {//Send To Numeric's
            Int32 Client = 0;
            {
                Single[] Origin = Lib.ReadSingle(Offsets.Funcs.G_Client((Int32)Client) + 0x1C, 3); Origin[2] += 50;
                Single[] Angles = Lib.ReadSingle(Offsets.Funcs.G_Client((Int32)Client) + 0x158, 3);
                X.Value = (Int32)Origin[0]; Y.Value = (Int32)Origin[1]; Z.Value = (Int32)Origin[2]; Pitch.Value = (Int32)Angles[0]; Yaw.Value = (Int32)Angles[1]; Roll.Value = (Int32)Angles[2];
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            //Spawns Turrent On Values
            if (Lib.ReadInt(0xFCA41D + ((Int32)0 * 0x280)) > 0) // Cheacks If Client Is Dead or Not // Credit to xCSBKx
            {
                Int32 Client = 0;
                Spawn_Turrent.iPrintln((Int32)Client, "^:Turret Spawned On Values");
               Spawn_Turrent.OnValues("sentry_minigun_mp", "weapon_minigun", (Int32)X.Value, (Int32)Y.Value, (Int32)Z.Value, (Int32)Pitch.Value, (Int32)Yaw.Value, (Int32)Roll.Value);
                //Test Auto Code Function "DOME" = Spawn_Turrent.OnValues("sentry_minigun_mp", "weapon_minigun", -313, 297, -350, -1, -153, 0); //working
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (button15.Text == "Pos In RealTime [ OFF ]")
            {
                POS.Start();
                button15.Text = "Pos In RealTime [ ON ]";

            }
            else if (button15.Text == "Pos In RealTime [ ON ]")
            {
                POS.Stop();
                button15.Text = "Pos In RealTime [ OFF ]";
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //single Client
            Spawn_Turrent.iPrintln((Int32)numericUpDown3.Value, "^:Turret Spawned On your Position. ^1KILL KILL KILL");
            Spawn_Turrent.OnAnglesToForward((Int32)numericUpDown3.Value);
            System.Threading.Thread.Sleep(3000);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //Every Client
            for (Int32 i = 0; i < 18; i++)
            {
                if (Lib.ReadInt(0xFCA41D + ((uint)i * 0x280)) > 0) // Cheacks If Client Is Dead or Not // Credit to xCSBKx
                {
                    Spawn_Turrent.iPrintln((Int32)i, "^:Turret Spawned On your Position. ^1KILL KILL KILL");
                    Spawn_Turrent.OnAnglesToForward((Int32)i);
                }
            }
        }

        
    }
}
