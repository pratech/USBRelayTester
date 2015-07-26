using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace USBRelayTester
{
    public partial class USBRElayTester : Form
    {
        
        delegate void SetOutputTextCallback(string text);
        private SerialPort serialPort;
        
        string[] availablePorts = SerialPort.GetPortNames();
        byte ctrlCmdStartByte = 0xFF;
        byte ctrlCmdOnByte = 0x01;
        byte ctrlCmdOffByte = 0x00;
        byte[] bank1StatusBytes = { 0xAA, 0x00, 0x00 };
        byte[] bank2StatusBytes = { 0xAA, 0xFF, 0x00 };
        byte[] readBuffer = new byte[3000];
        byte[] completeMessage = new byte[3000];
        
        Exception invalidMsgException = new Exception("Invalid Message Received");
        public USBRElayTester()
        {
            InitializeComponent();
        }
        


        public void OpenSerialPort(string PortName)
        {
            serialPort = new SerialPort(PortName, 9600, Parity.None, 8, StopBits.One);
            //serialPort.ReceivedBytesThreshold = 10;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.Open();
                    enableRelayButtons();
                    btnConnect.Text = "DISCONNECT";
                    getStatus(bank1StatusBytes);
                    Thread.Sleep(2000);
                    getStatus(bank2StatusBytes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Connecting to Selected COM Port");
                    
                }
            }
        }
        public void CloseSerialPort(SerialPort serialPort)
        {
            try
            {
                serialPort.Close();
                btnConnect.Text = "CONNECT";
                disableRelayButtons();
            }
            catch (Exception ex)
            {

            }
        }

        
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
               
                SerialPort tempSerialPort = sender as SerialPort;
                Thread.Sleep(500);
                int nOfBytes = tempSerialPort.BytesToRead;

                serialPort.Read(readBuffer, 0, nOfBytes);
                if (nOfBytes == 113)
                {
                    
                    byte[] statusBuffer = new byte[50];
                    for (int i = 0; i < 50; i++)
                    {
                        statusBuffer[i] = readBuffer[48 + i];
                       
                    }
                    resetUIAfterGetStatus(statusBuffer);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        
        private void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {

        }

        private void USBRElayTester_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < availablePorts.Count(); i++)
            {
                cbComPort.Items.Add(availablePorts[i]);
            }
            disableRelayButtons();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "CONNECT")
                OpenSerialPort(cbComPort.SelectedItem.ToString());
            else
                CloseSerialPort(serialPort);
        }

        private void btnRly1_Click_1(object sender, EventArgs e)
        {

            toggleRelay(ref btnRly1, ref lblRly1,0x01);

        }

       


        private void btnAll1On_Click(object sender, EventArgs e)
        {
            byte[] bValue = { ctrlCmdStartByte, 0x00, ctrlCmdOnByte };
            serialPort.Write(bValue,0,3);
            //Thread.Sleep(5000);
            //getStatus(bank1StatusBytes);
            //Thread.Sleep(1000);
            //getStatus(bank2StatusBytes);
            ChangeButton(btnRly1, "OFF");
            ChangeLabel(lblRly1, Color.Green);
            ChangeButton(btnRly2, "OFF");
            ChangeLabel(lblRly2, Color.Green);
            ChangeButton(btnRly3, "OFF");
            ChangeLabel(lblRly3, Color.Green);
            ChangeButton(btnRly4, "OFF");
            ChangeLabel(lblRly4, Color.Green);
            ChangeButton(btnRly5, "OFF");
            ChangeLabel(lblRly5, Color.Green);
            ChangeButton(btnRly6, "OFF");
            ChangeLabel(lblRly6, Color.Green);
            ChangeButton(btnRly7, "OFF");
            ChangeLabel(lblRly7, Color.Green);
            ChangeButton(btnRly8, "OFF");
            ChangeLabel(lblRly8, Color.Green);
           
        }

        private void btnAll1Off_Click(object sender, EventArgs e)
        {
            byte[] bValue = { ctrlCmdStartByte, 0x00, ctrlCmdOffByte };
            serialPort.Write(bValue, 0, 3);
            Thread.Sleep(5000);
            //getStatus(bank1StatusBytes);
            Thread.Sleep(1000);
            //getStatus(bank2StatusBytes);
            ChangeButton(btnRly1, "ON");
            ChangeLabel(lblRly1, Color.Red);
            ChangeButton(btnRly2, "ON");
            ChangeLabel(lblRly2, Color.Red);
            ChangeButton(btnRly3, "ON");
            ChangeLabel(lblRly3, Color.Red);
            ChangeButton(btnRly4, "ON");
            ChangeLabel(lblRly4, Color.Red);
            ChangeButton(btnRly5, "ON");
            ChangeLabel(lblRly5, Color.Red);
            ChangeButton(btnRly6, "ON");
            ChangeLabel(lblRly6, Color.Red);
            ChangeButton(btnRly7, "ON");
            ChangeLabel(lblRly7, Color.Red);
            ChangeButton(btnRly8, "ON");
            ChangeLabel(lblRly8, Color.Red);
        }

        private void btnAll2On_Click(object sender, EventArgs e)
        {
            byte[] bValue = { ctrlCmdStartByte, 0xFF, ctrlCmdOnByte };
            serialPort.Write(bValue,0,3);
            //getStatus(bank1StatusBytes);
            //Thread.Sleep(1000);
            //getStatus(bank2StatusBytes);
            //Thread.Sleep(1000);
            ChangeButton(btnRly9, "OFF");
            ChangeLabel(lblRly9, Color.Green);
            ChangeButton(btnRly10, "OFF");
            ChangeLabel(lblRly10, Color.Green);
            ChangeButton(btnRly11, "OFF");
            ChangeLabel(lblRly11, Color.Green);
            ChangeButton(btnRly12, "OFF");
            ChangeLabel(lblRly12, Color.Green);
            ChangeButton(btnRly13, "OFF");
            ChangeLabel(lblRly13, Color.Green);
            ChangeButton(btnRly14, "OFF");
            ChangeLabel(lblRly14, Color.Green);
            ChangeButton(btnRly15, "OFF");
            ChangeLabel(lblRly15, Color.Green);
            ChangeButton(btnRly16, "OFF");
            ChangeLabel(lblRly16, Color.Green);

        }

        private void btnAll2Off_Click(object sender, EventArgs e)
        {
            byte[] bValue = { ctrlCmdStartByte, 0xFF, ctrlCmdOffByte };
            serialPort.Write(bValue, 0, 3);
            //getStatus(bank1StatusBytes);
            //Thread.Sleep(1000);
            //getStatus(bank2StatusBytes);
            //Thread.Sleep(1000);
            ChangeButton(btnRly9, "ON");
            ChangeLabel(lblRly9, Color.Red);
            ChangeButton(btnRly10, "ON");
            ChangeLabel(lblRly10, Color.Red);
            ChangeButton(btnRly11, "ON");
            ChangeLabel(lblRly11, Color.Red);
            ChangeButton(btnRly12, "ON");
            ChangeLabel(lblRly12, Color.Red);
            ChangeButton(btnRly13, "ON");
            ChangeLabel(lblRly13, Color.Red);
            ChangeButton(btnRly14, "ON");
            ChangeLabel(lblRly14, Color.Red);
            ChangeButton(btnRly15, "ON");
            ChangeLabel(lblRly15, Color.Red);
            ChangeButton(btnRly16, "ON");
            ChangeLabel(lblRly16, Color.Red);
        }
        private void toggleRelay(ref Button button, ref Label label, byte relayNumber)
        {
            
            if (button.Text == "ON")
            {
                byte[] bValue = { ctrlCmdStartByte, relayNumber, ctrlCmdOnByte };
                serialPort.Write(bValue,0,3);
                ChangeButton(button, "OFF");
                ChangeLabel(label, Color.Green);
                //button.Text = "OFF";
                //label.ForeColor = Color.Green;

            }
            else
            {
                byte[] bValue = { ctrlCmdStartByte, relayNumber, ctrlCmdOffByte };
                serialPort.Write(bValue, 0, 3);
                ChangeButton(button, "ON");
                ChangeLabel(label, Color.Red);
                //button.Text = "ON";
                //label.ForeColor = Color.Red;
            }

        }
        private void getStatus(byte[] x)
        {
            serialPort.Write(x, 0, x.Length);
            Thread.Sleep(1000);
        }

        private void resetUIAfterGetStatus(byte[] receivedBytes)
        {
            
            string s = Encoding.ASCII.GetString(receivedBytes); 
            string[] rcvdString=s.Split('\0');
            int nOfBytes = receivedBytes.Length;
            if (rcvdString[0] == "0x55")
                {
                    if (rcvdString[1] == "0x00")
                    {
                        if (rcvdString[2] == "0x00")
                        {
                            ChangeButton(btnRly1, "ON");
                            ChangeLabel(lblRly1, Color.Red);
                           
                        }
                        else if (rcvdString[2] == "0x01")
                        {
                            ChangeButton(btnRly1, "OFF");
                            ChangeLabel(lblRly1, Color.Green);

                        }
                        else
                            throw invalidMsgException;
                            if (rcvdString[3] == "0x00")
                            {
                                ChangeButton(btnRly2, "ON");
                                ChangeLabel(lblRly2, Color.Red);

                            }
                            else if (rcvdString[3] == "0x01")
                            {
                                ChangeButton(btnRly2, "OFF");
                                ChangeLabel(lblRly2, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[4] == "0x00")
                            {
                                ChangeButton(btnRly3, "ON");
                                ChangeLabel(lblRly3, Color.Red);

                            }
                            else if (rcvdString[4] == "0x01")
                            {
                                ChangeButton(btnRly3, "OFF");
                                ChangeLabel(lblRly3, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[5] == "0x00")
                            {
                                ChangeButton(btnRly4, "ON");
                                ChangeLabel(lblRly4, Color.Red);

                            }
                            else if (rcvdString[5] == "0x01")
                            {
                                ChangeButton(btnRly4, "OFF");
                                ChangeLabel(lblRly4, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[6] == "0x00")
                            {
                                ChangeButton(btnRly5, "ON");
                                ChangeLabel(lblRly5, Color.Red);

                            }
                            else if (rcvdString[6] == "0x01")
                            {
                                ChangeButton(btnRly5, "OFF");
                                ChangeLabel(lblRly5, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[7] == "0x00")
                            {
                                ChangeButton(btnRly6, "ON");
                                ChangeLabel(lblRly6, Color.Red);

                            }
                            else if (rcvdString[7] == "0x01")
                            {
                                ChangeButton(btnRly6, "OFF");
                                ChangeLabel(lblRly6, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[8] == "0x00")
                            {
                                ChangeButton(btnRly7, "ON");
                                ChangeLabel(lblRly7, Color.Red);

                            }
                            else if (rcvdString[8] == "0x01")
                            {
                                ChangeButton(btnRly7, "OFF");
                                ChangeLabel(lblRly7, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[9] == "0x00")
                            {
                                ChangeButton(btnRly8, "ON");
                                ChangeLabel(lblRly8, Color.Red);

                            }
                            else if (rcvdString[9] == "0x01")
                            {
                                ChangeButton(btnRly8, "OFF");
                                ChangeLabel(lblRly8, Color.Green);

                            }
                            else
                                throw invalidMsgException;

                        }
                        else if (rcvdString[1] == "0xFF")
                        {
                            if (rcvdString[2] == "0x00")
                            {
                                ChangeButton(btnRly9, "ON");
                                ChangeLabel(lblRly9, Color.Red);

                            }
                            else if (rcvdString[2] == "0x01")
                            {
                                ChangeButton(btnRly9, "OFF");
                                ChangeLabel(lblRly9, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[3] == "0x00")
                            {
                                ChangeButton(btnRly10, "ON");
                                ChangeLabel(lblRly10, Color.Red);

                            }
                            else if (rcvdString[3] == "0x01")
                            {
                                ChangeButton(btnRly10, "OFF");
                                ChangeLabel(lblRly10, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[4] == "0x00")
                            {
                                ChangeButton(btnRly11, "ON");
                                ChangeLabel(lblRly11, Color.Red);

                            }
                            else if (rcvdString[4] == "0x01")
                            {
                                ChangeButton(btnRly11, "OFF");
                                ChangeLabel(lblRly11, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[5] == "0x00")
                            {
                                ChangeButton(btnRly12, "ON");
                                ChangeLabel(lblRly12, Color.Red);

                            }
                            else if (rcvdString[5] == "0x01")
                            {
                                ChangeButton(btnRly12, "OFF");
                                ChangeLabel(lblRly12, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[6] == "0x00")
                            {
                                ChangeButton(btnRly13, "ON");
                                ChangeLabel(lblRly13, Color.Red);

                            }
                            else if (rcvdString[6] == "0x01")
                            {
                                ChangeButton(btnRly13, "OFF");
                                ChangeLabel(lblRly13, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[7] == "0x00")
                            {
                                ChangeButton(btnRly14, "ON");
                                ChangeLabel(lblRly14, Color.Red);

                            }
                            else if (rcvdString[7] == "0x01")
                            {
                                ChangeButton(btnRly14, "OFF");
                                ChangeLabel(lblRly14, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[8] == "0x00")
                            {
                                ChangeButton(btnRly15, "ON");
                                ChangeLabel(lblRly15, Color.Red);

                            }
                            else if (rcvdString[8] == "0x01")
                            {
                                ChangeButton(btnRly15, "OFF");
                                ChangeLabel(lblRly15, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                            if (rcvdString[9] == "0x00")
                            {
                                ChangeButton(btnRly16, "ON");
                                ChangeLabel(lblRly16, Color.Red);

                            }
                            else if (rcvdString[9] == "0x01")
                            {
                                ChangeButton(btnRly16, "OFF");
                                ChangeLabel(lblRly16, Color.Green);

                            }
                            else
                                throw invalidMsgException;
                        }
            }
           
           
        }
        private void disableRelayButtons()
        {
            btnRly1.Enabled = false;
            btnRly2.Enabled = false;
            btnRly3.Enabled = false;
            btnRly4.Enabled = false;
            btnRly5.Enabled = false;
            btnRly6.Enabled = false;
            btnRly7.Enabled = false;
            btnRly8.Enabled = false;
            btnAll1Off.Enabled = false;
            btnAll1On.Enabled = false;
            btnRly9.Enabled = false;
            btnRly10.Enabled = false;
            btnRly11.Enabled = false;
            btnRly12.Enabled = false;
            btnRly13.Enabled = false;
            btnRly14.Enabled = false;
            btnRly15.Enabled = false;
            btnRly16.Enabled = false;
            btnAll2Off.Enabled = false;
            btnAll2On.Enabled = false;

        }

        private void enableRelayButtons()
        {
            btnRly1.Enabled = true;
            btnRly2.Enabled = true;
            btnRly3.Enabled = true;
            btnRly4.Enabled = true;
            btnRly5.Enabled = true;
            btnRly6.Enabled = true;
            btnRly7.Enabled = true;
            btnRly8.Enabled = true;
            btnAll1Off.Enabled = true;
            btnAll1On.Enabled = true;
            btnRly9.Enabled = true;
            btnRly10.Enabled = true;
            btnRly11.Enabled = true;
            btnRly12.Enabled = true;
            btnRly13.Enabled = true;
            btnRly14.Enabled = true;
            btnRly15.Enabled = true;
            btnRly16.Enabled = true;
            btnAll2Off.Enabled = true;
            btnAll2On.Enabled = true;

        }
        
        delegate void ChangeButtonDelegate(Button btn, string text);
        private void ChangeButton(Button button,string text)
        {
            if (button.InvokeRequired)
            {
                ChangeButtonDelegate del = new ChangeButtonDelegate(ChangeButton);
                button.Invoke(del, button,text);
            }
            else
            {
                
                button.Text = text;
                
            }
           
          
           
        }
        
        delegate void ChangeLabelDelegate(Label label,Color color);
        private void ChangeLabel(Label label,Color color)
        {
            if (label.InvokeRequired)
            {
                ChangeLabelDelegate del = new ChangeLabelDelegate(ChangeLabel);
                label.Invoke(del, label,color);
            }
            else
            {
                label.ForeColor=color;
            }
            
        }
       

        private void btnRly2_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly2, ref lblRly2, 0x02);
        }

        private void btnRly3_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly3, ref lblRly3, 0x03);
        }

        private void btnRly4_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly4, ref lblRly4, 0x04);
        }

        private void btnRly5_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly5, ref lblRly5, 0x05);
        }

        private void btnRly6_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly6, ref lblRly6, 0x06);
        }

        private void btnRly7_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly7, ref lblRly7, 0x07);
        }

        private void btnRly8_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly8, ref lblRly8, 0x08);
        }

        private void btnRly9_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly9, ref lblRly9, 0x09);
        }

        private void btnRly10_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly10, ref lblRly10, 0x10);
        }

        private void btnRly11_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly11, ref lblRly11, 0x11);
        }

        private void btnRly12_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly12, ref lblRly12, 0x12);
        }

        private void btnRly13_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly13, ref lblRly13, 0x13);
        }

        private void btnRly14_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly14, ref lblRly14, 0x14);
        }

        private void btnRly15_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly15, ref lblRly15, 0x15);
        }

        private void btnRly16_Click(object sender, EventArgs e)
        {
            toggleRelay(ref btnRly16, ref lblRly16, 0x16);
        }

        private void USBRElayTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(serialPort.IsOpen)
            serialPort.Close();
        }

       
    }
}
