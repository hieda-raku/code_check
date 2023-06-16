using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStationTransportServer 
{
  public  class ClientData
    {
        /// <summary>
        /// 固定站的 数据 data 和 ip
        /// </summary>
        public  byte[] data { set; get; }  
        public  string RemoteEndPoint  { set; get; } 

        public string  sData { set; get; } 

    }
}
