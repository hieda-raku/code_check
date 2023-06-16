using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WeatherStationTransportServer
{
    public partial class fmMain : Form
    {
        //ConcurrentDictionary
        public fmMain()
        {
            InitializeComponent();
          //  System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }
        //ConcurrentDictionary
        public static   ConcurrentQueue<decimal> qAirTemperature60 = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qAirTemperature360  = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qHumidity60   = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qHumidity360 = new ConcurrentQueue<decimal>(); 
        public static ConcurrentQueue<decimal> qAirPressure60  = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qAirPressure360 = new ConcurrentQueue<decimal>();

        public static ConcurrentQueue<decimal> qMinuteRainRAT = new ConcurrentQueue<decimal>();

        public static ConcurrentQueue<decimal> qWindSpeed60  = new ConcurrentQueue<decimal>();

        public static ConcurrentQueue<decimal> qWindSpeed60_10  = new ConcurrentQueue<decimal>();

        public static ConcurrentQueue<decimal> qWindSpeed60_10_6 = new ConcurrentQueue<decimal>();

        public static ConcurrentQueue<decimal> qWindDirection60Sin  = new ConcurrentQueue<decimal>();

        public static ConcurrentQueue<decimal> qWindDirection60  = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qWindDirection60Cos  = new ConcurrentQueue<decimal>();

        public static ConcurrentQueue<decimal> qWindSpeed120 = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qWindDirection360 = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qWindDirection120Sin = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qWindDirection120 = new ConcurrentQueue<decimal>();
        
        public static ConcurrentQueue<decimal> qWindDirection120Cos = new ConcurrentQueue<decimal>();


        //   public static ConcurrentQueue<decimal> qWindSpeed60_10  = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qWindSpeed600 = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<decimal> qWindDirection600  = new ConcurrentQueue<decimal>();
        public static ConcurrentQueue<int> qWindDirectionVAG60_10   = new ConcurrentQueue<int>();

        public static ConcurrentQueue<int> qWindDirectionVAG60_10_6  = new ConcurrentQueue<int>(); 


        public static ConcurrentQueue<decimal> qWindSpeed360 = new ConcurrentQueue<decimal>();


        public static Socket server_client;
        static Thread RThread = null;

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnClose.Enabled = true;
            server_client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;

            //uc为接收数据所使用的UdpClient，不同程序自己手改
            server_client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

            //  byte[] optionInValue = { Convert.ToByte(false) };
            //  byte[] optionOutValue = new byte[4];
            //int  ioControlCode = unchecked((int)0x98000001);
            //  server_client.IOControl(ioControlCode, optionInValue, optionInValue)
            server_client.Bind(new IPEndPoint(IPAddress.Parse(this.LocalIPTxt.Text), int.Parse(this.LocalPortTxt.Text)));
            RThread = new Thread(ReceiveMsg);
            RThread.Start();

            //server_client2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //server_client2.Bind(new IPEndPoint(IPAddress.Parse(this.LocalIPTxt.Text), int.Parse(this.LocalPortTxt.Text)));
            //RThread2 = new Thread(ReceiveMsg2);
            //RThread2.Start();

            //server_client3 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //server_client3.Bind(new IPEndPoint(IPAddress.Parse(this.LocalPortTxt.Text), int.Parse(this.LocalPortTxt.Text)));
            //RThread3 = new Thread(ReceiveMsg3);
            //RThread3.Start();
        }

        public static EndPoint[] endPoints;

        private delegate void PrintRecvMssgDelegate(string s);
        private delegate void PrintRecvMssgDelegate1(ClientData clientData);

        void ReceiveMsg()
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);//任意IP

         

            byte[] buffer = new byte[1024];
            string message = "";



           

            while (true)
            {
                try
                {
                    int length = server_client.ReceiveFrom(buffer, ref point);//获取socket数据
                    message = Encoding.Default.GetString(buffer, 0, length);
                    ClientData clientData = new ClientData();
                    clientData.sData = message;
                    clientData.RemoteEndPoint = point.ToString();
                    Invoke(new PrintRecvMssgDelegate1(PrintRecMsg1), new object[] { clientData });
                }
                catch
                {
                    //
                }
            }

        }
        private void PrintRecMsg1(ClientData clientData)
        {
            //dong.liu
           if(clientData.sData.Contains("#DMGD"))
            {
                //DMGD dong.liu
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00") + ",");
                sb.Append(strDataList4);
                byte[] byteSend = System.Text.Encoding.Default.GetBytes(sb.ToString()+ "\r\n");
                try
                {
                    EndPoint point = new IPEndPoint(IPAddress.Parse(clientData.RemoteEndPoint.Split(':')[0]), int.Parse(clientData.RemoteEndPoint.Split(':')[1]));
                    server_client.SendTo(byteSend, point);
                    Thread.Sleep(800);
                    setstrDataList4("0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0,0,0,0,12.81,24.60;");
                }
                catch
                {

                }

            }
        }

        private void spWind_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //this.Invoke((EventHandler)(delegate

            //{
                string sData = spWind.ReadLine();
                int ilength = sData.Length; //59
                setstrDataLis2(sData);
            //sData ="2022-11-28 15:34:00,0.05,285.63,28.83,0,2.216542e+15,0,1007.06,12.82,24.16,7999.00,0.00,1007.06,0.08,282.10,0.07,288.80;";
            //try
            //{
            ////Thread t = new Thread(() =>
            ////{

            //});
            //t.Start();

            //  }
            //catch
            //{

            // }
            //if (sData.Length >= 4)
            //{
            //    //Thread t = new Thread(() =>
            //    //{







            //                //  tbReceive.Text += DateTime.Now.ToString() + ":" + sData + "\r\n";





            //        }

            //else if (sData[0] == 's')
            //{
            //    tbReceive.Text += DateTime.Now.ToString()+":"+sData+"\r\n";
            //}
            //else if (sData[0] == 'r')
            //{
            //    tbReceive.Text += DateTime.Now.ToString() + ":" + sData + "\r\n";
            //}

            //});
            //t.Start();
            // }

            //  spWind.DiscardInBuffer();//丢弃接收缓冲区数据

            //}));
        }
        double MathPI = Math.PI;
        public static object lockerD3 = new object();//添加一个bai对象作为锁
        public static object lockerD4  = new object();//添加一个bai对象作为锁
        public static string strDataList3 = "----------------------00.00 0.00 00.00 0.00 1000.00 0.00 0.09 272.00 ---2-0.09 272.00 ---10-0.00 0.00 ---60-0.00 0.00 ";
        public static string strDataList4 = "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0,0,0,0,12.81,24.60;";

        public static void setstrDataList3(string value)//线程bai1循环调用此函du数
        {
            lock (lockerD3)//锁
            {

                strDataList3 = value;
            }
        }
        public static void setstrDataList4(string value)//线程bai1循环调用此函du数
        {
            lock (lockerD4)//锁
            {

                strDataList4 = value;
            }
        }
        public void setstrDataList(string data)
        {
            //CPU:JD2022060106L Cuishi Shanghai V6_Enc.CR8
            string[] zdata=data.Split(',');
            if (zdata.Length >= 18&& zdata[0].Length>=17&& zdata[0].Length <= 23)
            {
                DZZ3 dZZ3 = new DZZ3();
                dZZ3.DateTime = Convert.ToDateTime(zdata[0]);
                if (zdata[1] == "NAN")
                {
                    dZZ3.WindSpeed = Convert.ToDecimal("0");
                   // qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                   // qWindSpeed360.Enqueue(dZZ3.WindSpeed);
                }
                else if (zdata[1] != "NAN")
                {
                    dZZ3.WindSpeed = Convert.ToDecimal(zdata[1]);
                 //   qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                   // qWindSpeed360.Enqueue(dZZ3.WindSpeed);
                }
                if (zdata[2] == "NAN")
                {
                    dZZ3.WindDirection = Convert.ToDecimal(30);
                }
                else if(zdata[2] != "NAN")
                {
                    dZZ3.WindDirection = Convert.ToDecimal(zdata[2]);
                }
                dZZ3.WindDirectionSin = Convert.ToDecimal(Math.Sin((double)dZZ3.WindDirection));
                dZZ3.WindDirectionCos = Convert.ToDecimal(Math.Cos((double)dZZ3.WindDirection));

                dZZ3.WorkTemperature = Convert.ToDecimal(zdata[3]);
                dZZ3.WindSensorStatus = Convert.ToDecimal(zdata[4]);
                if (zdata[5].Length > 11)
                {
                    dZZ3.AirTemperature = Convert.ToDecimal(9999.9m);
                 //   qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                   // qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                }
                else  if(zdata[5].Length <9) 
                {
                    dZZ3.AirTemperature = Convert.ToDecimal(zdata[5]);
                  //  qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                  //  qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                }

                 if (Convert.ToDecimal(zdata[6])<30)
                {
                    dZZ3.Humidity = Convert.ToDecimal("-100");
                   // qHumidity60.Enqueue(dZZ3.Humidity);
                   // qHumidity360.Enqueue(dZZ3.Humidity);
                }
                else if (Convert.ToDecimal(zdata[6])>30&& Convert.ToDecimal(zdata[6]) <100)
                {
                    dZZ3.Humidity = Convert.ToDecimal(zdata[6]);
                   // qHumidity60.Enqueue(dZZ3.Humidity);
                   // qHumidity360.Enqueue(dZZ3.Humidity);
                }
                if (zdata[7] == "NAN")
                {
                    dZZ3.AirPressure = Convert.ToDecimal("1000");
                    // qHumidity60.Enqueue(dZZ3.Humidity);
                    // qHumidity360.Enqueue(dZZ3.Humidity);
                }
                else if (zdata[7] != "NAN")
                {
                    dZZ3.AirPressure = Convert.ToDecimal(zdata[7]);
                }
                dZZ3.Voltage = Convert.ToDecimal(zdata[8]);
                dZZ3.WorkTemperature = Convert.ToDecimal(zdata[9]); //机箱温度
              //  dZZ3.WindSensorStatus = Convert.ToDecimal(zdata[9]);
                //if (zdata[7] == "NAN")
                //{
                //    dZZ3.AirPressure = Convert.ToDecimal(1000);
                //   // qAirPressure60.Enqueue(dZZ3.AirPressure);
                //   // qAirPressure360.Enqueue(dZZ3.AirPressure);
                //}
                //else if (zdata[7] != "NAN")
                //{
                //    dZZ3.AirPressure = Convert.ToDecimal(zdata[9]);
                //   // qAirPressure60.Enqueue(dZZ3.AirPressure);
                //   // qAirPressure360.Enqueue(dZZ3.AirPressure);
                //}

                dZZ3.Voltage = Convert.ToDecimal(zdata[8]);
                dZZ3.CrateTemperature = Convert.ToDecimal(zdata[9]);
                if (zdata[10].Length > 10)
                {
                    dZZ3.AirTemperature_AVG1 = Convert.ToDecimal(0);
                }
                else if(zdata[10].Length <= 5)
                {
                    dZZ3.AirTemperature_AVG1 = Convert.ToDecimal(zdata[10]);
                }
                if (zdata[11].Length > 10)
                {
                    dZZ3.HumidityMin_AVG1 = Convert.ToDecimal(0);
                }
                else if (zdata[11].Length <= 5)
                {
                    dZZ3.HumidityMin_AVG1 = Convert.ToDecimal(zdata[11]);
                }
                if (zdata[12].Length > 10)
                {
                    dZZ3.AirPressure_AVG1 = Convert.ToDecimal(0);
                }
                else if (zdata[12].Length <= 5)
                {
                    dZZ3.AirPressure_AVG1 = Convert.ToDecimal(zdata[12]);
                }
                if (zdata[13] == "NAN")
                {
                    dZZ3.WindSpeed2 = Convert.ToDecimal(0);
                }
                else if (zdata[13] != "NAN")
                {
                    dZZ3.WindSpeed2 = Convert.ToDecimal(zdata[15]);
                }
                if (zdata[14] == "NAN")
                {
                    dZZ3.WindDirection2 = Convert.ToDecimal(0);
                }
                else if (zdata[14] != "NAN")
                {
                    dZZ3.WindDirection2 = Convert.ToDecimal(zdata[14]);
                }
                if (zdata[15] == "NAN")
                {
                    dZZ3.WindDirection10 = Convert.ToDecimal(0);
                }
                else if (zdata[15] != "NAN")
                {
                    dZZ3.WindSpeed10 = Convert.ToDecimal(zdata[15]);
                }

                if (zdata[16] == "NAN")
                {
                    dZZ3.WindDirection10 = Convert.ToDecimal(0);
                }
                else if (zdata[16] != "NAN")
                {
                    dZZ3.WindDirection10 = Convert.ToDecimal(zdata[16].Substring(0, 5));
                }
                if (zdata[17] == "NAN")
                {
                    dZZ3.MinuteRainRAT = Convert.ToDecimal(0);
                }
                else if (zdata[17] != "NAN")
                {
                    dZZ3.MinuteRainRAT = Convert.ToDecimal(zdata[17]);
                }
                if (zdata[18] == "NAN")
                {
                    dZZ3.HourRainRAT = Convert.ToDecimal(0);
                }
                else if (zdata[18] != "NAN")
                {
                    dZZ3.HourRainRAT = Convert.ToDecimal(zdata[18].Split(';')[0]);
                }
                

                if (qWindSpeed60.Count <= 60)
                {
                    qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                    qHumidity60.Enqueue(dZZ3.Humidity);
                    qAirPressure60.Enqueue(dZZ3.AirPressure);
                    qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                    qWindDirection60.Enqueue(dZZ3.WindDirection);
                  //  qWindDirection60Sin.Enqueue(dZZ3.WindDirectionSin);
                  //  qWindDirection60Cos.Enqueue(dZZ3.WindDirectionCos);

                }
                else if (qWindSpeed60.Count > 60)
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qAirTemperature60.Count>60 && qAirTemperature60.TryDequeue(out outValue))
                    {
                        decimal[] lAirTemperature60 = new decimal[qAirTemperature60.Count];
                        lAirTemperature60 = qAirTemperature60.ToArray();
                        dZZ3.AirTemperatureAVG60 = lAirTemperature60.Average();
                    }
                    if (qHumidity60.Count > 60 && qHumidity60.TryDequeue(out outValue))
                    {
                        decimal[] lHumidity60 = new decimal[qHumidity60.Count];
                        lHumidity60 = qHumidity60.ToArray();
                        dZZ3.HumidityAVG60 = lHumidity60.Average();
                    }
                    if (qAirPressure60.Count>60&& qAirPressure60.TryDequeue(out outValue))
                    {
                        decimal[] lAirPressure60 = new decimal[qAirPressure60.Count];
                        lAirPressure60 = qAirPressure60.ToArray();
                        dZZ3.AirPressureAVG60 = lAirPressure60.Average();
                    }
                    if (qWindSpeed60.Count > 60 && qWindSpeed60.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed60 = new decimal[qWindSpeed60.Count];
                        lWindSpeed60 = qWindSpeed60.ToArray();
                        dZZ3.WindSpeedVAG60 = lWindSpeed60.Average();
                        #region  方式二 计算风向
                        //if (qWindSpeed60_10.Count <= 10)
                        //{
                        //    qWindSpeed60_10.Enqueue(dZZ3.WindSpeedVAG60);
                        //}
                        //else if (qWindSpeed60_10.Count > 10 && qWindSpeed60_10.TryDequeue(out outValue))
                        //{
                        //    decimal[] lWindSpeed60_10 = new decimal[qWindSpeed60_10.Count];
                        //    lWindSpeed60_10 = qWindSpeed60_10.ToArray();
                        //    dZZ3.WindSpeedVAG60_10 = lWindSpeed60_10.Average();

                        //    if(qWindSpeed60_10_6.Count <= 6)
                        //    {
                        //        qWindSpeed60_10_6.Enqueue(dZZ3.WindSpeedVAG60_10);
                        //    }
                        //    else if(qWindSpeed60_10_6.Count > 6 && qWindSpeed60_10_6.TryDequeue(out outValue))
                        //    {
                        //        decimal[] lWindSpeed60_10_6 = new decimal[qWindSpeed60_10_6.Count];
                        //        lWindSpeed60_10_6 = qWindSpeed60_10_6.ToArray();
                        //        dZZ3.WindSpeedVAG60_10 = lWindSpeed60_10_6.Average();
                        //    }
                        //}
                        #endregion 


                    }
                    #region 方式二计算风向
                    //if (qWindDirection60Sin.Count > 60&&qWindDirection60Cos.Count > 60 && qWindDirection60Sin.TryDequeue(out outValue) && qWindDirection60Sin.TryDequeue(out outValue1))
                    // {
                    //    decimal[] lWindDirection60Sin = new decimal[qWindDirection60Sin.Count];
                    //    lWindDirection60Sin = qWindDirection60Sin.ToArray();

                    //    decimal[] lWindDirection60Cos = new decimal[qWindDirection60Cos.Count];
                    //    lWindDirection60Cos = qWindDirection60Cos.ToArray();


                    //    decimal lSinVga  = lWindDirection60Sin.Average();
                    //    decimal lCosVga  = lWindDirection60Cos.Average();

                    //    double WD = Math.Atan((double)lSinVga / (double)lCosVga);    //  得到 角度 ，以弧度表示

                    //    WD *= 180 / MathPI;    //  转化为 角度
                    //    dZZ3.WindDirectionVAG60 = (int)WD;

                    //    if (qWindDirectionVAG60_10.Count<10)
                    //    {
                    //        qWindDirectionVAG60_10.Enqueue(dZZ3.WindDirectionVAG60);
                    //    }
                    //    else if(qWindSpeed60_10.Count > 60&& qWindSpeed60_10.TryDequeue(out outValue))
                    //    {
                    //        decimal[] lWindSpeed60_10 = new decimal[qWindSpeed60_10.Count];
                    //        lWindSpeed60_10 = qWindSpeed60_10.ToArray();
                    //        dZZ3.WindSpeedVAG60_10 = lWindSpeed60_10.Average();
                    //    }
                    //}
                    #endregion
                    if (qWindDirection60.Count>60&&qWindDirection60.TryPeek(out outValue))
                    {
                        decimal[] lWindDirection60 = new decimal[qWindDirection60.Count];
                        lWindDirection60 = qWindDirection60.ToArray();
                        dZZ3.WindDirectionVAG60 =(int) lWindDirection60.Average();
                    }
                }

                if (qWindSpeed120.Count <= 120)
                {
                    qWindSpeed120.Enqueue(dZZ3.WindSpeed);
                    qWindDirection120.Enqueue(dZZ3.WindDirection);
                    //  qWindDirection120Sin.Enqueue(dZZ3.WindDirectionSin);
                    // qWindDirection120Cos.Enqueue(dZZ3.WindDirectionCos);
                }
                else if (qWindSpeed120.Count > 120)
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qWindSpeed120.Count > 120 && qWindSpeed120.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed120 = new decimal[qWindSpeed120.Count];
                        lWindSpeed120 = qWindSpeed60.ToArray();
                        dZZ3.WindSpeedVAG120 = lWindSpeed120.Average();

                    }
                    #region 2分风向计算
                    //if (qWindDirection120Sin.Count > 120 && qWindDirection120Cos.Count > 120 && qWindDirection120Sin.TryDequeue(out outValue) && qWindDirection120Sin.TryDequeue(out outValue1))
                    // {
                    //     decimal[] lWindDirection120Sin = new decimal[qWindDirection120Sin.Count];
                    //     lWindDirection120Sin = qWindDirection120Sin.ToArray();

                    //     decimal[] lWindDirection120Cos = new decimal[qWindDirection60Cos.Count];
                    //     lWindDirection120Cos = qWindDirection120Cos.ToArray();


                    //     decimal lSinVga = lWindDirection120Sin.Average();
                    //     decimal lCosVga = lWindDirection120Cos.Average();

                    //     double WD = Math.Atan((double)lSinVga / (double)lCosVga);    //  得到 角度 ，以弧度表示

                    //     WD *= 180 / MathPI;    //  转化为 角度
                    //     dZZ3.WindDirectionVAG120 = (int)WD;

                    // }
                    #endregion
                    if (qWindDirection120.Count > 120 && qWindDirection120.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection120 = new decimal[qWindDirection120.Count];
                        lWindDirection120 = qWindDirection120.ToArray();
                        dZZ3.WindDirectionVAG120 = (int)lWindDirection120.Average();

                    }

                }
                if (qWindSpeed600.Count <= 600)
                {

                    qWindDirection600.Enqueue((int)dZZ3.WindDirection);

                    qWindSpeed600.Enqueue(dZZ3.WindSpeed);
                }
                else if (qWindSpeed360.Count > 600)
                {
                    decimal outValue, outValue1;


                    if (qWindSpeed600.Count > 600 && qWindSpeed600.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed600 = new decimal[qWindSpeed600.Count];
                        lWindSpeed600 = qWindSpeed600.ToArray();
                        dZZ3.WindSpeedVAG60_10 = lWindSpeed600.Average();
                    }
                    if (qWindDirection600.Count > 600 && qWindDirection600.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection600 = new decimal[qWindDirection600.Count];
                        lWindDirection600 = qWindDirection360.ToArray();
                        dZZ3.WindDirectionVAG60_10 = (int)lWindDirection600.Average();
                    }
                }

                if (qAirTemperature360.Count <= 900)
                {
                    qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                    qHumidity360.Enqueue(dZZ3.Humidity);
                    qAirPressure360.Enqueue(dZZ3.AirPressure);
                

                }
                else if (qAirTemperature360.Count > 900 )
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qAirTemperature360.Count > 900 && qAirTemperature360.TryDequeue(out outValue))
                    {
                        decimal[] lAirTemperature360 = new decimal[qAirTemperature360.Count];
                        lAirTemperature360 = qAirTemperature360.ToArray();
                        dZZ3.AirTemperatureAVG360 = lAirTemperature360.Average();
                    }
                    if (qHumidity360.Count > 900 && qHumidity360.TryDequeue(out outValue))
                    {
                        decimal[] lHumidity360 = new decimal[qHumidity360.Count];
                        lHumidity360 = qHumidity360.ToArray();
                        dZZ3.HumidityAVG360 = lHumidity360.Average();
                    }
                    if (qAirPressure360.Count > 900 && qAirPressure360.TryDequeue(out outValue))
                    {
                        decimal[] lAirPressure360 = new decimal[qAirPressure360.Count];
                        lAirPressure360 = qAirPressure360.ToArray();
                        dZZ3.AirPressureAVG360 = lAirPressure360.Average();
                    }

                }

                if (qWindSpeed360.Count <= 900)
                {
                 
                    qWindDirection360.Enqueue(dZZ3.WindDirection);

                    qWindSpeed360.Enqueue(dZZ3.WindSpeed);
                }
                else if (qWindSpeed360.Count > 900)
                {
                    decimal outValue;

                   
                    if (qWindSpeed360.Count > 900 && qWindSpeed360.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed360 = new decimal[qWindSpeed360.Count];
                        lWindSpeed360 = qWindSpeed360.ToArray();
                        dZZ3.WindSpeedVAG360 = lWindSpeed360.Average();
                    }
                    if (qWindDirection360.Count > 900 && qWindDirection360.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection360 = new decimal[qWindDirection360.Count];
                        lWindDirection360 = qWindDirection360.ToArray();
                        dZZ3.WindDirectionVAG360 =(int) lWindDirection360.Average();
                    }
                }
               

                StringBuilder sb = new StringBuilder();
                sb.Append("DMGD " + StationID + " " + dZZ3.DateTime.ToString("yyyy-HH-dd HH") + ":" + DateTime.Now.ToString("mm:ss") + " ");
                sb.Append(dZZ3.AirTemperature.ToString("F2") + " " + dZZ3.Humidity.ToString("F2") + " " + dZZ3.AirPressure.ToString("F2") + " ");
                sb.Append(dZZ3.WindSpeed.ToString("F2") + " " + dZZ3.WindDirection.ToString("F2") + " ");
                sb.Append(dZZ3.MinuteRainRAT.ToString("F2") + " " + dZZ3.HourRainRAT.ToString("F2") + " ");
                if (dZZ3.HumidityAVG60 == 0m)
                {
                    sb.Append(strDataList3);
                    Wirter(sb.ToString());
                }
                else
                {
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append("---------------------- ");
                    sb1.Append(dZZ3.AirTemperatureAVG60.ToString("F2") + " " + dZZ3.AirTemperatureAVG360.ToString("F2") + " ");
                    sb1.Append(dZZ3.HumidityAVG60.ToString("F2") + " " + dZZ3.HumidityAVG360.ToString("F2") + " ");
                    sb1.Append(dZZ3.AirPressureAVG60.ToString("F2") + " " + dZZ3.AirPressureAVG360.ToString("F2") + " ");
                    sb1.Append(dZZ3.WindSpeedVAG60.ToString("F2") + " " + dZZ3.WindDirectionVAG60.ToString("F2") + " ");
                    sb1.Append("---2- ");
                    sb1.Append(dZZ3.WindSpeedVAG120.ToString("F2") + " " + dZZ3.WindDirectionVAG120.ToString("F2") + " ");
                    sb1.Append("---10- ");
                    sb1.Append(dZZ3.WindSpeedVAG60_10.ToString("F2") + " " + dZZ3.WindDirectionVAG60_10.ToString("F2") + " ");
                    sb1.Append("---60- ");
                    sb1.Append(dZZ3.WindSpeedVAG360.ToString("F2") + " " + dZZ3.WindDirectionVAG360.ToString("F2") + " ");
                    setstrDataList3(sb1.ToString());
                    Wirter(sb.ToString()+sb1.ToString());
                }
              

            }
        }

        public string IsTemperatureHumidityStatusOK(decimal AirTemperature)
        {
            if(AirTemperature== 9999.9m)
            {
               
               return "error";
            }
            else
            {
                return "0";

            }
        }
        public string IsAirPressureStatusOK(decimal AirPressure)
        {
            if (AirPressure == 1000m)
            {

                return "error";
            }
            else
            {
                return "0";

            }
        }
        public string IsWindStatusOK(decimal WindSpeed)
        {
            if (WindSpeed == 1000m)
            {

                return "error";
            }
            else
            {
                return "0";

            }
        }
        public string AirTemperatureIsOK(decimal AirTemperature)
        {
            if(AirTemperature > 60) 
            {
                return "error";
            }
            else
            {
                return AirTemperature.ToString("F2");
            }
        }
        public string HumidityIsOK(decimal Humidity)
        {
            if (Humidity < 0)
            {
                return "error";
            }
            else
            {
                return Humidity.ToString("F2");
            }
        }
        public string AirPressureIsOK(decimal AirPressure)
        {
            if (AirPressure < 100)
            {
                return "error";
            }
            else
            {
                return AirPressure.ToString("F2");
            }
        }
        public string WindSpeedIsOK(decimal WindSpeed)
        {
            if (WindSpeed < 0m)
            {
                return "error";
            }
            else
            {
                return WindSpeed.ToString("F2");
            }
        }
        public string WindDirectionIsOK(decimal WindDirection)
        {
            if (WindDirection < -10m)
            {
                return "error";
            }
            else
            {
                return WindDirection.ToString("F2");
            }
        }


        public void setstrDataList2(string data)
        {
            //CPU:JD2022060106L Cuishi Shanghai V6_Enc.CR8
            string[] zdata = data.Split(',');
            if (zdata.Length >= 18 && zdata[0].Length >= 17 && zdata[0].Length <= 23)
            {
                DZZ3 dZZ3 = new DZZ3();
                dZZ3.DateTime = Convert.ToDateTime(zdata[0]);
                if (zdata[1] == "NAN")
                {
                    dZZ3.WindSpeed = Convert.ToDecimal("-1");
                    // qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                    // qWindSpeed360.Enqueue(dZZ3.WindSpeed);
                }
                else if (zdata[1] != "NAN")
                {
                    dZZ3.WindSpeed = Convert.ToDecimal(zdata[1]);
                    //   qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                    // qWindSpeed360.Enqueue(dZZ3.WindSpeed);
                }
                if (zdata[2] == "NAN")
                {
                    dZZ3.WindDirection = Convert.ToDecimal(-30);
                }
                else if (zdata[2] != "NAN")
                {
                    dZZ3.WindDirection = Convert.ToDecimal(zdata[2]);
                }
                dZZ3.WindDirectionSin = Convert.ToDecimal(Math.Sin((double)dZZ3.WindDirection));
                dZZ3.WindDirectionCos = Convert.ToDecimal(Math.Cos((double)dZZ3.WindDirection));
                if(zdata[3]=="NAN")
                {
                    dZZ3.WorkTemperature = Convert.ToDecimal(9999.9m);
                }
                else if(zdata[3] != "NAN")
                {
                    dZZ3.WorkTemperature = Convert.ToDecimal(zdata[3]);
                }
                if(zdata[4] == "NAN")
                {
                    dZZ3.WindSensorStatus = Convert.ToDecimal(-9);
                }
                else if(zdata[4] != "NAN")
                {
                    dZZ3.WindSensorStatus = Convert.ToDecimal(zdata[4]);
                }
              //  dZZ3.WorkTemperature = Convert.ToDecimal(zdata[3]);
              //  dZZ3.WindSensorStatus = Convert.ToDecimal(zdata[4]);
                if (zdata[5].Length > 11)
                {
                    dZZ3.AirTemperature = Convert.ToDecimal(9999.9m);
                    //   qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                    // qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                }
                else if (zdata[5].Length < 9)
                {
                    dZZ3.AirTemperature = Convert.ToDecimal(zdata[5]);
                    //  qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                    //  qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                }

                if (Convert.ToDecimal(zdata[6]) < 30)
                {
                    dZZ3.Humidity = Convert.ToDecimal("-100");
                    // qHumidity60.Enqueue(dZZ3.Humidity);
                    // qHumidity360.Enqueue(dZZ3.Humidity);
                }
                else if (Convert.ToDecimal(zdata[6]) > 30 && Convert.ToDecimal(zdata[6]) < 100)
                {
                    dZZ3.Humidity = Convert.ToDecimal(zdata[6]);
                    // qHumidity60.Enqueue(dZZ3.Humidity);
                    // qHumidity360.Enqueue(dZZ3.Humidity);
                }
                if (zdata[7] == "0")
                {
                    dZZ3.AirPressure = Convert.ToDecimal("0");
                    // qHumidity60.Enqueue(dZZ3.Humidity);
                    // qHumidity360.Enqueue(dZZ3.Humidity);
                }
                else if (zdata[7] != "0")
                {
                    dZZ3.AirPressure = Convert.ToDecimal(zdata[7]);
                }
              

                dZZ3.Voltage = Convert.ToDecimal(zdata[8]);
                dZZ3.CrateTemperature = Convert.ToDecimal(zdata[9]);
                if (zdata[10].Length > 10)
                {
                    dZZ3.AirTemperature_AVG1 = Convert.ToDecimal(0);
                }
                else if (zdata[10].Length <= 5)
                {
                    dZZ3.AirTemperature_AVG1 = Convert.ToDecimal(zdata[10]);
                }
                if (zdata[11].Length > 10)
                {
                    dZZ3.HumidityMin_AVG1 = Convert.ToDecimal(0);
                }
                else if (zdata[11].Length <= 5)
                {
                    dZZ3.HumidityMin_AVG1 = Convert.ToDecimal(zdata[11]);
                }
                if (zdata[12].Length > 10)
                {
                    dZZ3.AirPressure_AVG1 = Convert.ToDecimal(0);
                }
                else if (zdata[12].Length <= 5)
                {
                    dZZ3.AirPressure_AVG1 = Convert.ToDecimal(zdata[12]);
                }
                if (zdata[13] == "NAN")
                {
                    dZZ3.WindSpeed2 = Convert.ToDecimal(0);
                }
                else if (zdata[13] != "NAN")
                {
                    dZZ3.WindSpeed2 = Convert.ToDecimal(zdata[15]);
                }
                if (zdata[14] == "NAN")
                {
                    dZZ3.WindDirection2 = Convert.ToDecimal(0);
                }
                else if (zdata[14] != "NAN")
                {
                    dZZ3.WindDirection2 = Convert.ToDecimal(zdata[14]);
                }
                if (zdata[15] == "NAN")
                {
                    dZZ3.WindDirection10 = Convert.ToDecimal(0);
                }
                else if (zdata[15] != "NAN")
                {
                    dZZ3.WindSpeed10 = Convert.ToDecimal(zdata[15]);
                }

                if (zdata[16] == "NAN")
                {
                    dZZ3.WindDirection10 = Convert.ToDecimal(0);
                }
                else if (zdata[16] != "NAN")
                {
                    dZZ3.WindDirection10 = Convert.ToDecimal(zdata[16].Substring(0, 5));
                }
                if (zdata[17] == "NAN")
                {
                    dZZ3.MinuteRainRAT = Convert.ToDecimal(0);
                }
                else if (zdata[17] != "NAN")
                {
                    dZZ3.MinuteRainRAT = Convert.ToDecimal(zdata[17]);
                }
                if (zdata[18] == "NAN")
                {
                    dZZ3.HourRainRAT = Convert.ToDecimal(0);
                }
                else if (zdata[18] != "NAN")
                {
                    dZZ3.HourRainRAT = Convert.ToDecimal(zdata[18].Split(';')[0]);
                }

                if (qWindSpeed60.Count <= 60)
                {
                    qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                    qHumidity60.Enqueue(dZZ3.Humidity);
                    qAirPressure60.Enqueue(dZZ3.AirPressure);
                    qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                    qWindDirection60.Enqueue(dZZ3.WindDirection);
                    //  qWindDirection60Sin.Enqueue(dZZ3.WindDirectionSin);
                    //  qWindDirection60Cos.Enqueue(dZZ3.WindDirectionCos);

                }
                else if (qWindSpeed60.Count > 60)
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qAirTemperature60.Count > 60 && qAirTemperature60.TryDequeue(out outValue))
                    {
                        decimal[] lAirTemperature60 = new decimal[qAirTemperature60.Count];
                        lAirTemperature60 = qAirTemperature60.ToArray();
                        dZZ3.AirTemperatureAVG60 = lAirTemperature60.Average();
                    }
                    if (qHumidity60.Count > 60 && qHumidity60.TryDequeue(out outValue))
                    {
                        decimal[] lHumidity60 = new decimal[qHumidity60.Count];
                        lHumidity60 = qHumidity60.ToArray();
                        dZZ3.HumidityAVG60 = lHumidity60.Average();
                    }
                    if (qAirPressure60.Count > 60 && qAirPressure60.TryDequeue(out outValue))
                    {
                        decimal[] lAirPressure60 = new decimal[qAirPressure60.Count];
                        lAirPressure60 = qAirPressure60.ToArray();
                        dZZ3.AirPressureAVG60 = lAirPressure60.Average();
                    }
                    if (qWindSpeed60.Count > 60 && qWindSpeed60.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed60 = new decimal[qWindSpeed60.Count];
                        lWindSpeed60 = qWindSpeed60.ToArray();
                        dZZ3.WindSpeedVAG60 = lWindSpeed60.Average();
                        #region  方式二 计算风向
                        //if (qWindSpeed60_10.Count <= 10)
                        //{
                        //    qWindSpeed60_10.Enqueue(dZZ3.WindSpeedVAG60);
                        //}
                        //else if (qWindSpeed60_10.Count > 10 && qWindSpeed60_10.TryDequeue(out outValue))
                        //{
                        //    decimal[] lWindSpeed60_10 = new decimal[qWindSpeed60_10.Count];
                        //    lWindSpeed60_10 = qWindSpeed60_10.ToArray();
                        //    dZZ3.WindSpeedVAG60_10 = lWindSpeed60_10.Average();

                        //    if(qWindSpeed60_10_6.Count <= 6)
                        //    {
                        //        qWindSpeed60_10_6.Enqueue(dZZ3.WindSpeedVAG60_10);
                        //    }
                        //    else if(qWindSpeed60_10_6.Count > 6 && qWindSpeed60_10_6.TryDequeue(out outValue))
                        //    {
                        //        decimal[] lWindSpeed60_10_6 = new decimal[qWindSpeed60_10_6.Count];
                        //        lWindSpeed60_10_6 = qWindSpeed60_10_6.ToArray();
                        //        dZZ3.WindSpeedVAG60_10 = lWindSpeed60_10_6.Average();
                        //    }
                        //}
                        #endregion 


                    }
                    #region 方式二计算风向
                    //if (qWindDirection60Sin.Count > 60&&qWindDirection60Cos.Count > 60 && qWindDirection60Sin.TryDequeue(out outValue) && qWindDirection60Sin.TryDequeue(out outValue1))
                    // {
                    //    decimal[] lWindDirection60Sin = new decimal[qWindDirection60Sin.Count];
                    //    lWindDirection60Sin = qWindDirection60Sin.ToArray();

                    //    decimal[] lWindDirection60Cos = new decimal[qWindDirection60Cos.Count];
                    //    lWindDirection60Cos = qWindDirection60Cos.ToArray();


                    //    decimal lSinVga  = lWindDirection60Sin.Average();
                    //    decimal lCosVga  = lWindDirection60Cos.Average();

                    //    double WD = Math.Atan((double)lSinVga / (double)lCosVga);    //  得到 角度 ，以弧度表示

                    //    WD *= 180 / MathPI;    //  转化为 角度
                    //    dZZ3.WindDirectionVAG60 = (int)WD;

                    //    if (qWindDirectionVAG60_10.Count<10)
                    //    {
                    //        qWindDirectionVAG60_10.Enqueue(dZZ3.WindDirectionVAG60);
                    //    }
                    //    else if(qWindSpeed60_10.Count > 60&& qWindSpeed60_10.TryDequeue(out outValue))
                    //    {
                    //        decimal[] lWindSpeed60_10 = new decimal[qWindSpeed60_10.Count];
                    //        lWindSpeed60_10 = qWindSpeed60_10.ToArray();
                    //        dZZ3.WindSpeedVAG60_10 = lWindSpeed60_10.Average();
                    //    }
                    //}
                    #endregion
                    if (qWindDirection60.Count > 60 && qWindDirection60.TryPeek(out outValue))
                    {
                        decimal[] lWindDirection60 = new decimal[qWindDirection60.Count];
                        lWindDirection60 = qWindDirection60.ToArray();
                        dZZ3.WindDirectionVAG60 = (int)lWindDirection60.Average();
                    }
                }

                if (qWindSpeed120.Count <= 120)
                {
                    qWindSpeed120.Enqueue(dZZ3.WindSpeed);
                    qWindDirection120.Enqueue(dZZ3.WindDirection);
                    //  qWindDirection120Sin.Enqueue(dZZ3.WindDirectionSin);
                    // qWindDirection120Cos.Enqueue(dZZ3.WindDirectionCos);
                }
                else if (qWindSpeed120.Count > 120)
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qWindSpeed120.Count > 120 && qWindSpeed120.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed120 = new decimal[qWindSpeed120.Count];
                        lWindSpeed120 = qWindSpeed60.ToArray();
                        dZZ3.WindSpeedVAG120 = lWindSpeed120.Average();

                    }
                    #region 2分风向计算
                    //if (qWindDirection120Sin.Count > 120 && qWindDirection120Cos.Count > 120 && qWindDirection120Sin.TryDequeue(out outValue) && qWindDirection120Sin.TryDequeue(out outValue1))
                    // {
                    //     decimal[] lWindDirection120Sin = new decimal[qWindDirection120Sin.Count];
                    //     lWindDirection120Sin = qWindDirection120Sin.ToArray();

                    //     decimal[] lWindDirection120Cos = new decimal[qWindDirection60Cos.Count];
                    //     lWindDirection120Cos = qWindDirection120Cos.ToArray();


                    //     decimal lSinVga = lWindDirection120Sin.Average();
                    //     decimal lCosVga = lWindDirection120Cos.Average();

                    //     double WD = Math.Atan((double)lSinVga / (double)lCosVga);    //  得到 角度 ，以弧度表示

                    //     WD *= 180 / MathPI;    //  转化为 角度
                    //     dZZ3.WindDirectionVAG120 = (int)WD;

                    // }
                    #endregion
                    if (qWindDirection120.Count > 120 && qWindDirection120.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection120 = new decimal[qWindDirection120.Count];
                        lWindDirection120 = qWindDirection120.ToArray();
                        dZZ3.WindDirectionVAG120 = (int)lWindDirection120.Average();

                    }

                }
                if (qWindSpeed600.Count <= 600)
                {

                    qWindDirection600.Enqueue((int)dZZ3.WindDirection);

                    qWindSpeed600.Enqueue(dZZ3.WindSpeed);
                }
                else if (qWindSpeed360.Count > 600)
                {
                    decimal outValue, outValue1;


                    if (qWindSpeed600.Count > 600 && qWindSpeed600.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed600 = new decimal[qWindSpeed600.Count];
                        lWindSpeed600 = qWindSpeed600.ToArray();
                        dZZ3.WindSpeedVAG60_10 = lWindSpeed600.Average();
                    }
                    if (qWindDirection600.Count > 600 && qWindDirection600.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection600 = new decimal[qWindDirection600.Count];
                        lWindDirection600 = qWindDirection360.ToArray();
                        dZZ3.WindDirectionVAG60_10 = (int)lWindDirection600.Average();
                    }
                }

                if (qAirTemperature360.Count <= 900)
                {
                    qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                    qHumidity360.Enqueue(dZZ3.Humidity);
                    qAirPressure360.Enqueue(dZZ3.AirPressure);


                }
                else if (qAirTemperature360.Count > 900)
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qAirTemperature360.Count > 900 && qAirTemperature360.TryDequeue(out outValue))
                    {
                        decimal[] lAirTemperature360 = new decimal[qAirTemperature360.Count];
                        lAirTemperature360 = qAirTemperature360.ToArray();
                        dZZ3.AirTemperatureAVG360 = lAirTemperature360.Average();
                    }
                    if (qHumidity360.Count > 900 && qHumidity360.TryDequeue(out outValue))
                    {
                        decimal[] lHumidity360 = new decimal[qHumidity360.Count];
                        lHumidity360 = qHumidity360.ToArray();
                        dZZ3.HumidityAVG360 = lHumidity360.Average();
                    }
                    if (qAirPressure360.Count > 900 && qAirPressure360.TryDequeue(out outValue))
                    {
                        decimal[] lAirPressure360 = new decimal[qAirPressure360.Count];
                        lAirPressure360 = qAirPressure360.ToArray();
                        dZZ3.AirPressureAVG360 = lAirPressure360.Average();
                    }

                }

                if (qWindSpeed360.Count <= 900)
                {

                    qWindDirection360.Enqueue(dZZ3.WindDirection);

                    qWindSpeed360.Enqueue(dZZ3.WindSpeed);
                }
                else if (qWindSpeed360.Count > 900)
                {
                    decimal outValue;


                    if (qWindSpeed360.Count > 900 && qWindSpeed360.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed360 = new decimal[qWindSpeed360.Count];
                        lWindSpeed360 = qWindSpeed360.ToArray();
                        dZZ3.WindSpeedVAG360 = lWindSpeed360.Average();
                    }
                    if (qWindDirection360.Count > 900 && qWindDirection360.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection360 = new decimal[qWindDirection360.Count];
                        lWindDirection360 = qWindDirection360.ToArray();
                        dZZ3.WindDirectionVAG360 = (int)lWindDirection360.Average();
                    }
                }


                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",");
                //if(false)
                if (dZZ3.HumidityAVG60 == 0m)
                {
                    sb.Append(strDataList4);
                    Wirter(sb.ToString());
                }
                else
                {
                    StringBuilder sb1 = new StringBuilder();

                    sb1.Append(AirTemperatureIsOK(dZZ3.AirTemperatureAVG60) + "," + HumidityIsOK(dZZ3.HumidityAVG60) + "," + dZZ3.AirPressureAVG60.ToString("F2")+ "," + WindSpeedIsOK(dZZ3.WindSpeedVAG60) + "," + dZZ3.WindDirectionVAG60.ToString("F2") + "," + dZZ3.MinuteRainRAT.ToString("F2") + ",");
                    sb1.Append(WindSpeedIsOK(dZZ3.WindSpeedVAG120)+ "," +dZZ3.WindDirectionVAG120.ToString("F2")+ "," + WindSpeedIsOK(dZZ3.WindSpeedVAG60_10) + "," + dZZ3.WindDirectionVAG60_10.ToString("F2") + ",");
                    sb1.Append(WindSpeedIsOK(dZZ3.WindSpeedVAG360) + "," +dZZ3.WindDirectionVAG360.ToString("F2") + "," + AirTemperatureIsOK(dZZ3.AirTemperatureAVG360) + "," + HumidityIsOK(dZZ3.HumidityAVG360)+ "," + dZZ3.AirPressureAVG360.ToString("F2") + "," + dZZ3.HourRainRAT.ToString("F2") + ",");
                    sb1.Append(IsWindStatusOK(dZZ3.WindSpeed) + "," + IsTemperatureHumidityStatusOK(dZZ3.AirTemperature)+ "," + IsAirPressureStatusOK(dZZ3.AirPressure)+","+dZZ3.RainRATSensorStatus+","+dZZ3.Voltage.ToString("F2")+","+ dZZ3.WorkTemperature.ToString("F2")+";");
                  
                    setstrDataList4(sb1.ToString());
                    Wirter(sb.ToString() + sb1.ToString());
                }


            }
        }


        // dong.liu

        public bool  IsFirstRes=true;
        public void setstrDataLis2(string data)
        {
            //CPU:JD2022060106L Cuishi Shanghai V6_Enc.CR8
            string[] zdata = data.Split(',');
            if (zdata.Length >= 18 && zdata[0].Length >= 17 && zdata[0].Length <= 23)
            {
                DZZ3 dZZ3 = new DZZ3();
                //  string s9 = zdata[0].Substring(0, zdata[0].Length - 3).ToString();
                try
                {
                    dZZ3.DateTime = Convert.ToDateTime(zdata[0]);
                    //  dZZ3.DateTime = Convert.ToDateTime(zdata[0].Substring(0, zdata[0].Length-3).ToString()+":00");

                    dZZ3.WindSpeed = Convert.ToDecimal(zdata[1]);
                    //   qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                    // qWindSpeed360.Enqueue(dZZ3.WindSpeed);


                    dZZ3.WindDirection = Convert.ToDecimal(zdata[2]);


                    dZZ3.WorkTemperature = Convert.ToDecimal(zdata[3]);

                    dZZ3.WindSensorStatus = Convert.ToDecimal(zdata[4]);

                    //  dZZ3.WorkTemperature = Convert.ToDecimal(zdata[3]);
                    //  dZZ3.WindSensorStatus = Convert.ToDecimal(zdata[4]);

                    if (zdata[5].Length < 11)
                    {
                        dZZ3.AirTemperature = Convert.ToDecimal(zdata[5]);
                        //  qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                        //  qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                    }

                    if (Convert.ToDecimal(zdata[6]) < 10)
                    {
                        dZZ3.Humidity = Convert.ToDecimal("-100");
                        // qHumidity60.Enqueue(dZZ3.Humidity);
                        // qHumidity360.Enqueue(dZZ3.Humidity);
                    }
                    else if (Convert.ToDecimal(zdata[6]) > 20 && Convert.ToDecimal(zdata[6]) < 100)
                    {
                        dZZ3.Humidity = Convert.ToDecimal(zdata[6]);
                        // qHumidity60.Enqueue(dZZ3.Humidity);
                        // qHumidity360.Enqueue(dZZ3.Humidity);
                    }
                    if (zdata[7] == "0")
                    {
                        dZZ3.AirPressure = Convert.ToDecimal("0");
                        // qHumidity60.Enqueue(dZZ3.Humidity);
                        // qHumidity360.Enqueue(dZZ3.Humidity);
                    }
                    else if (zdata[7] != "0")
                    {
                        dZZ3.AirPressure = Convert.ToDecimal(zdata[7]);
                    }


                    dZZ3.Voltage = Convert.ToDecimal(zdata[8]);
                    dZZ3.CrateTemperature = Convert.ToDecimal(zdata[9]);
                    if (zdata[10].Length > 10)
                    {
                        dZZ3.AirTemperature_AVG1 = Convert.ToDecimal(0);
                    }
                    else if (zdata[10].Length <= 5)
                    {
                        dZZ3.AirTemperature_AVG1 = Convert.ToDecimal(zdata[10]);
                    }
                    if (zdata[11].Length > 10)
                    {
                        dZZ3.HumidityMin_AVG1 = Convert.ToDecimal(0);
                    }
                    else if (zdata[11].Length <= 5)
                    {
                        dZZ3.HumidityMin_AVG1 = Convert.ToDecimal(zdata[11]);
                    }
                    if (zdata[12].Length > 10)
                    {
                        dZZ3.AirPressure_AVG1 = Convert.ToDecimal(0);
                    }
                    else if (zdata[12].Length <= 5)
                    {
                        dZZ3.AirPressure_AVG1 = Convert.ToDecimal(zdata[12]);
                    }
                    if (zdata[13] == "NAN")
                    {
                        dZZ3.WindSpeed2 = Convert.ToDecimal(0);
                    }
                    else if (zdata[13] != "NAN")
                    {
                        dZZ3.WindSpeed2 = Convert.ToDecimal(zdata[15]);
                    }
                    if (zdata[14] == "NAN")
                    {
                        dZZ3.WindDirection2 = Convert.ToDecimal(0);
                    }
                    else if (zdata[14] != "NAN")
                    {
                        dZZ3.WindDirection2 = Convert.ToDecimal(zdata[14]);
                    }
                    if (zdata[15] == "NAN")
                    {
                        dZZ3.WindDirection10 = Convert.ToDecimal(0);
                    }
                    else if (zdata[15] != "NAN")
                    {
                        dZZ3.WindSpeed10 = Convert.ToDecimal(zdata[15]);
                    }

                    if (zdata[16] == "NAN")
                    {
                        dZZ3.WindDirection10 = Convert.ToDecimal(0);
                    }
                    else if (zdata[16] != "NAN")
                    {
                        dZZ3.WindDirection10 = Convert.ToDecimal(zdata[16].Substring(0, 5));
                    }
                    if (zdata[17] == "NAN")
                    {
                        dZZ3.MinuteRainRAT = Convert.ToDecimal(0);
                    }
                    else if (zdata[17] != "NAN")
                    {
                        dZZ3.MinuteRainRAT = Convert.ToDecimal(zdata[17]);
                    }
                    if (zdata[18] == "NAN")
                    {
                        dZZ3.HourRainRAT = Convert.ToDecimal(0);
                    }
                    else if (zdata[18] != "NAN")
                    {
                        dZZ3.HourRainRAT = Convert.ToDecimal(zdata[18].Split(';')[0]);
                    }
                }
                catch
                {

                }
                if(IsFirstRes==true)
                {
                    for(int  i=0;i<6;i++)
                    {
                        qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                        qHumidity60.Enqueue(dZZ3.Humidity);
                        qAirPressure60.Enqueue(dZZ3.AirPressure);
                        qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                        qWindDirection60.Enqueue(dZZ3.WindDirection);

                      
                    }
                    for(int i=0;i<12;i++)
                    {
                        qWindSpeed120.Enqueue(dZZ3.WindSpeed);
                        qWindDirection120.Enqueue(dZZ3.WindDirection);
                    }
                    for (int i = 0; i < 60; i++)
                    {
                        qWindSpeed600.Enqueue(dZZ3.WindSpeed);
                        qWindDirection600.Enqueue(dZZ3.WindDirection);
                    }
                    for (int i = 0; i < 90; i++)
                    {
                        qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                        qHumidity360.Enqueue(dZZ3.Humidity);
                        qAirPressure360.Enqueue(dZZ3.AirPressure);
                        qWindSpeed360.Enqueue(dZZ3.WindSpeed);
                        qWindDirection360.Enqueue(dZZ3.WindDirection);
                    }
                    IsFirstRes = false;
                }
                if (qWindSpeed60.Count <= 6)
                {
                    qAirTemperature60.Enqueue(dZZ3.AirTemperature);
                    qHumidity60.Enqueue(dZZ3.Humidity);
                    qAirPressure60.Enqueue(dZZ3.AirPressure);
                    qWindSpeed60.Enqueue(dZZ3.WindSpeed);
                    qWindDirection60.Enqueue(dZZ3.WindDirection);
                  

                }
                else if (qWindSpeed60.Count > 6)
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qAirTemperature60.Count > 6 && qAirTemperature60.TryDequeue(out outValue))
                    {
                        decimal[] lAirTemperature60 = new decimal[qAirTemperature60.Count];
                        lAirTemperature60 = qAirTemperature60.ToArray();
                        dZZ3.AirTemperatureAVG60 = lAirTemperature60.Average();
                    }
                    if (qHumidity60.Count > 6 && qHumidity60.TryDequeue(out outValue))
                    {
                        decimal[] lHumidity60 = new decimal[qHumidity60.Count];
                        lHumidity60 = qHumidity60.ToArray();
                        dZZ3.HumidityAVG60 = lHumidity60.Average();
                    }
                    if (qAirPressure60.Count > 6 && qAirPressure60.TryDequeue(out outValue))
                    {
                        decimal[] lAirPressure60 = new decimal[qAirPressure60.Count];
                        lAirPressure60 = qAirPressure60.ToArray();
                        dZZ3.AirPressureAVG60 = lAirPressure60.Average();
                    }
                    if (qWindSpeed60.Count > 6 && qWindSpeed60.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed60 = new decimal[qWindSpeed60.Count];
                        lWindSpeed60 = qWindSpeed60.ToArray();
                        dZZ3.WindSpeedVAG60 = lWindSpeed60.Average();
                      

                    }
               
                    if (qWindDirection60.Count > 6 && qWindDirection60.TryPeek(out outValue))
                    {
                        decimal[] lWindDirection60 = new decimal[qWindDirection60.Count];
                        lWindDirection60 = qWindDirection60.ToArray();
                        dZZ3.WindDirectionVAG60 = (int)lWindDirection60.Average();
                    }
                }

                if (qWindSpeed120.Count <= 12)
                {
                    qWindSpeed120.Enqueue(dZZ3.WindSpeed);
                    qWindDirection120.Enqueue(dZZ3.WindDirection);
                 
                }
                else if (qWindSpeed120.Count > 12)
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qWindSpeed120.Count > 12 && qWindSpeed120.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed120 = new decimal[qWindSpeed120.Count];
                        lWindSpeed120 = qWindSpeed60.ToArray();
                        dZZ3.WindSpeedVAG120 = lWindSpeed120.Average();

                    }
             
                    if (qWindDirection120.Count > 12 && qWindDirection120.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection120 = new decimal[qWindDirection120.Count];
                        lWindDirection120 = qWindDirection120.ToArray();
                        dZZ3.WindDirectionVAG120 = (int)lWindDirection120.Average();

                    }

                }
                if (qWindSpeed600.Count <= 60)
                {

                    qWindDirection600.Enqueue((int)dZZ3.WindDirection);

                    qWindSpeed600.Enqueue(dZZ3.WindSpeed);
                }
                else if (qWindSpeed600.Count > 60)
                {
                    decimal outValue, outValue1;


                    if (qWindSpeed600.Count > 60 && qWindSpeed600.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed600 = new decimal[qWindSpeed600.Count];
                        lWindSpeed600 = qWindSpeed600.ToArray();
                        dZZ3.WindSpeedVAG60_10 = lWindSpeed600.Average();
                    }
                    if (qWindDirection600.Count > 60 && qWindDirection600.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection600 = new decimal[qWindDirection600.Count];
                        lWindDirection600 = qWindDirection360.ToArray();
                        dZZ3.WindDirectionVAG60_10 = (int)lWindDirection600.Average();
                    }
                }

                if (qAirTemperature360.Count <= 90)
                {
                    qAirTemperature360.Enqueue(dZZ3.AirTemperature);
                    qHumidity360.Enqueue(dZZ3.Humidity);
                    qAirPressure360.Enqueue(dZZ3.AirPressure);


                }
                else if (qAirTemperature360.Count > 90)
                {
                    decimal outValue;
                    decimal outValue1;
                    if (qAirTemperature360.Count > 90 && qAirTemperature360.TryDequeue(out outValue))
                    {
                        decimal[] lAirTemperature360 = new decimal[qAirTemperature360.Count];
                        lAirTemperature360 = qAirTemperature360.ToArray();
                        dZZ3.AirTemperatureAVG360 = lAirTemperature360.Average();
                    }
                    if (qHumidity360.Count > 90 && qHumidity360.TryDequeue(out outValue))
                    {
                        decimal[] lHumidity360 = new decimal[qHumidity360.Count];
                        lHumidity360 = qHumidity360.ToArray();
                        dZZ3.HumidityAVG360 = lHumidity360.Average();
                    }
                    if (qAirPressure360.Count > 90 && qAirPressure360.TryDequeue(out outValue))
                    {
                        decimal[] lAirPressure360 = new decimal[qAirPressure360.Count];
                        lAirPressure360 = qAirPressure360.ToArray();
                        dZZ3.AirPressureAVG360 = lAirPressure360.Average();
                    }

                }

                if (qWindSpeed360.Count <= 90)
                {

                    qWindDirection360.Enqueue(dZZ3.WindDirection);

                    qWindSpeed360.Enqueue(dZZ3.WindSpeed);
                }
                else if (qWindSpeed360.Count > 90)
                {
                    decimal outValue;


                    if (qWindSpeed360.Count > 90 && qWindSpeed360.TryDequeue(out outValue))
                    {
                        decimal[] lWindSpeed360 = new decimal[qWindSpeed360.Count];
                        lWindSpeed360 = qWindSpeed360.ToArray();
                        dZZ3.WindSpeedVAG360 = lWindSpeed360.Average();
                    }
                    if (qWindDirection360.Count > 90 && qWindDirection360.TryDequeue(out outValue))
                    {
                        decimal[] lWindDirection360 = new decimal[qWindDirection360.Count];
                        lWindDirection360 = qWindDirection360.ToArray();
                        dZZ3.WindDirectionVAG360 = (int)lWindDirection360.Average();
                    }
                }



                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00") + ",");
                //if(false)
                if (dZZ3.HumidityAVG60 == 0m)
                {
                    sb.Append(strDataList4);
                  
                    if (DateTime.Now.Second <= 20&& DateTime.Now.Second >10)
                    {
                        Send(sb.ToString());
                      
                     

                    }
                    Wirter1(data);
                    Wirter(sb.ToString());


                }
                else
                {
                    StringBuilder sb1 = new StringBuilder();

                    sb1.Append(AirTemperatureIsOK(dZZ3.AirTemperatureAVG60) + "," + HumidityIsOK(dZZ3.HumidityAVG60) + "," + dZZ3.AirPressureAVG60.ToString("F2") + "," + WindSpeedIsOK(dZZ3.WindSpeedVAG60) + "," + dZZ3.WindDirectionVAG60.ToString("F2") + "," + dZZ3.MinuteRainRAT.ToString("F2") + ",");
                    sb1.Append(WindSpeedIsOK(dZZ3.WindSpeedVAG120) + "," + dZZ3.WindDirectionVAG120.ToString("F2") + "," + WindSpeedIsOK(dZZ3.WindSpeedVAG60_10) + "," + dZZ3.WindDirectionVAG60_10.ToString("F2") + ",");
                    sb1.Append(WindSpeedIsOK(dZZ3.WindSpeedVAG360) + "," + dZZ3.WindDirectionVAG360.ToString("F2") + "," + AirTemperatureIsOK(dZZ3.AirTemperatureAVG360) + "," + HumidityIsOK(dZZ3.HumidityAVG360) + "," + dZZ3.AirPressureAVG360.ToString("F2") + "," + dZZ3.HourRainRAT.ToString("F2") + ",");
                    sb1.Append(IsWindStatusOK(dZZ3.WindSpeed) + "," + IsTemperatureHumidityStatusOK(dZZ3.AirTemperature) + "," + IsAirPressureStatusOK(dZZ3.AirPressure) + "," + dZZ3.RainRATSensorStatus + "," + dZZ3.Voltage.ToString("F2") + "," + dZZ3.WorkTemperature.ToString("F2") + ";");

                    setstrDataList4(sb1.ToString());
                  
                    if (DateTime.Now.Second <= 20 && DateTime.Now.Second > 10)
                    {
                        Send(sb.ToString() + sb1.ToString());
                    
                     
                    }
                    Wirter1(data);
                    Wirter(sb.ToString() + sb1.ToString());

                }


            }
        }
        public void Wirter(string TEXT)
        {
           

            string   fullPath1 = saveAddress + DateTime.Now.Year+"\\"+DateTime.Now.Month+"\\"+DateTime.Now.Day + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "Values.Txt";
            if (!File.Exists(fullPath1))
            {
                Folder folder = new Folder();
                string backFileName = saveAddress;
                folder.checkFolder(backFileName);
                
                folder.checkFolder(backFileName + +DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day + "\\");
             
                FileStream fs = File.Create(fullPath1);//创建文件

                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("数据时间,分钟温度(℃),分钟湿度(%),分钟气压(hPa),分钟风速(m/s),分钟风向(°),分钟降雨量(mm),2分风速(m/s),2分风向（°）,10分风速(m/s),10分风向(°),小时风速(m/s),小时风向(°),小时温度(℃),小时湿度(%),小时气压(hPa),小时降雨(mm),风传感器状态(0表正常),温湿度传感器状态(0表正常),气压传感器状态(0表正常),雨传感器状态(0表正常),系统电池电压(V),系统机箱温度(℃),");

                //  sw.WriteLine("采样时间;温度;湿度;气压;风速;风向;气象站点;电池板电压;负载功率;设备温度;蓄电池电流;蓄电池电压;");

                sw.Close();

                fs.Close();


                return;
            }
            else
            {
                  FileStream fs = new FileStream(fullPath1, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //   sw.WriteLine(@"DateTime, sv1, sv2, sv3, sv4, sv5, sv6, sv7, sv8, sv9,sv10 ");
                if (DateTime.Now.Second>12 && DateTime.Now.Second <= 22)
                {
                    sw.WriteLine(TEXT);
                }

                sw.Close();
                fs.Close();
                return;
            }

          
        }


        public void Wirter1(string TEXT)
        {


            string fullPath4 = saveAddress1 + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day+ "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "Values.Txt";
            if (!File.Exists(fullPath4))
            {
                Folder folder = new Folder();
                string backFileName = saveAddress1;
                folder.checkFolder(backFileName);
            
                folder.checkFolder(backFileName + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day + "\\");
             
                FileStream fs = File.Create(fullPath4);//创建文件
             
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("串口报文:");

                sw.Close();
               
                fs.Close();
                return;
            }
            else
            {
                FileStream fs = new FileStream(fullPath4, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
              
                if (DateTime.Now.Second > 12 && DateTime.Now.Second <= 22)
                {
                    sw.WriteLine(TEXT.Trim('\r','\n'));
                }

                sw.Close();
                fs.Close();
                return;
            }


        }

        public void Send(string TEXT)
        {
            try
            {

                EndPoint point = new IPEndPoint(IPAddress.Parse(UDPremoteIPAddress), int.Parse(UDPremoteIPPort));

                byte[] byteSend = System.Text.Encoding.Default.GetBytes(TEXT + "\r\n");

                server_client.SendTo(byteSend, point);


            }
            catch
            {

            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
               
                    spWind.WriteLine("#request\r\n");
                    Thread.Sleep(10000);
               
            });
            t.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Queue q = new Queue(10);
            q.Enqueue(7);
            q.Enqueue(7);
            q.Enqueue(3);
            q.Enqueue(3);
            q.Dequeue();

            ConcurrentQueue<decimal> dQueue = new ConcurrentQueue<decimal>();

            dQueue.Enqueue(7m);
            dQueue.Enqueue(7m);
            dQueue.Enqueue(7m);
            decimal outDecimal;

              

             dQueue.TryDequeue(out outDecimal);

            int i = dQueue.Count;


        }

      
        public static string AlarmThreshold1;
        public static string AlarmThreshold2;
        public static string AlarmThreshold3;
        public static int iLightStatus = 300;
        public static string PicStationID1;

        //
        public static string WindCOM;
        public static int WindBaudRate;
        public static int WindStopBits;
        public static string WindParity;
        public static int WindDataBits;
        public static string WinStata;
        public static string ComIsOpen;

        public static string UDPserverIPAddress;
        public static string UDPserverIPPort = "30001";

        public static string UDPremoteIPAddress;
        public static string UDPremoteIPPort;

        public static string UDPremoteIPAddress2;
        public static string UDPremoteIPPort2;

        public static string UDPremoteIPAddress3;
        public static string UDPremoteIPPort3;

        public static string UDPStata;
        public string saveAddress;

        public string fullPath;
        public static string StationID="";
        public string saveAddress1 = "";

        private void fmMain_Load(object sender, EventArgs e)
        {

            //
         //   this.BackColor = Color.White;
           // this.TransparencyKey = Color.White;
             this.Opacity = 0;  //窗体的透明度为50%  //this.Opacity = 0.5; 
               this.ShowInTaskbar = false;  // //隐藏任务栏区图标

            // 创建存储文件  存储数据
            saveAddress = ConfigurationManager.AppSettings["saveAddress"];
            saveAddress1= ConfigurationManager.AppSettings["saveAddress1"];
            StationID = ConfigurationManager.AppSettings["StationID"];
            Folder folder = new Folder();
            string backFileName = saveAddress;
            folder.checkFolder(backFileName);
            fullPath = backFileName + DateTime.Now.ToString("d");
            folder.checkFolder(backFileName + DateTime.Now.ToString("d"));



            UDPserverIPAddress = LocalIPTxt.Text = ConfigurationManager.AppSettings["UDPserverIPAddress"];//本机IP
            UDPserverIPPort = LocalPortTxt.Text = ConfigurationManager.AppSettings["UDPserverIPPort"];//本机UDP接收端口
            UDPremoteIPAddress = IPTxt.Text = ConfigurationManager.AppSettings["UDPremoteIPAddress1"];//远端ip
            UDPremoteIPPort = PortTxt.Text = ConfigurationManager.AppSettings["UDPremoteIPPort1"];//远端端口

            UDPremoteIPAddress2 = ConfigurationManager.AppSettings["UDPremoteIPAddress2"];//远端ip
            UDPremoteIPPort2 = ConfigurationManager.AppSettings["UDPremoteIPPort2"];//远端端口

            UDPremoteIPAddress3 = ConfigurationManager.AppSettings["UDPremoteIPAddress3"];//远端ip
            UDPremoteIPPort3 = ConfigurationManager.AppSettings["UDPremoteIPPort3"];//远端端口

            UDPStata = ConfigurationManager.AppSettings["UDPStata"];//

            if (UDPStata == "1")
            {
                this.btnStart_Click(null, null);
                //   this.timReadData.


            }

            //初始化  风速串口值
            WindCOM = ConfigurationManager.AppSettings["WindCOM"];
            ComIsOpen = ConfigurationManager.AppSettings["ComIsOpen"];
            WindBaudRate = Convert.ToInt32(ConfigurationManager.AppSettings["WindBaudRate"]);
            WindStopBits = Convert.ToInt32(ConfigurationManager.AppSettings["WindStopBits"]);
            WindParity = ConfigurationManager.AppSettings["WindParity"];
            WindDataBits = Convert.ToInt32(ConfigurationManager.AppSettings["WindDataBits"]);
            WinStata = ConfigurationManager.AppSettings["WinStata"];

      

            //  WSServerTest.StartServer();

            if (WindStopBits == 0)
            {
                spWind.StopBits = System.IO.Ports.StopBits.None;
            }
            else if (WindStopBits == 1)
            {
                spWind.StopBits = System.IO.Ports.StopBits.One;
            }
            else if (WindStopBits == 1.5)
            {
                spWind.StopBits = System.IO.Ports.StopBits.OnePointFive;
            }
            else if (WindStopBits == 2)
            {
                spWind.StopBits = System.IO.Ports.StopBits.Two;
            }
            else
            {
                spWind.StopBits = System.IO.Ports.StopBits.One;
            }

            //  spTem.Parity = 1;
            if (WindParity.CompareTo("无") == 0)
            {
                spWind.Parity = System.IO.Ports.Parity.None;
            }
            else if (WindParity.CompareTo("奇") == 0)
            {
                spWind.Parity = System.IO.Ports.Parity.Odd;
            }
            else if (WindParity.CompareTo("偶") == 0)
            {
                spWind.Parity = System.IO.Ports.Parity.Even;
            }
            else
            {
                spWind.Parity = System.IO.Ports.Parity.None;
            }

            spWind.DataBits = WindDataBits;

            spWind.PortName = WindCOM;

            if (ComIsOpen == "1")
            {
                spWind.Open();
                timReadData.Enabled = true;
            }
        }

        private void btnClrR_Click(object sender, EventArgs e)
        {
            RTxt.Text = "";//清空接收
        }

        private void timReadData_Tick(object sender, EventArgs e)
        {
            if (spWind.IsOpen)
            {
                spWind.WriteLine("#request\r\n");
            }
           
        }

        private void fmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //结束进程
            System.Diagnostics.Process.GetCurrentProcess().Kill(); //关闭窗口后关闭后台进程
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {

                EndPoint point = new IPEndPoint(IPAddress.Parse(UDPremoteIPAddress), int.Parse(UDPremoteIPPort));

                byte[] byteSend = System.Text.Encoding.Default.GetBytes("TEXT" + "\r\n");

                server_client.SendTo(byteSend, point);


            }
            catch
            {

            }
        }

        private void timReadData_Tick_1(object sender, EventArgs e)
        {

        }
    }
}
