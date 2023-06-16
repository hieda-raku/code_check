using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherStationTransportServer
{
   public class DZZ3
    {
        public DateTime DateTime { set; get; } = Convert.ToDateTime("2000-01-01 00:00:00"); 
        public decimal WindSpeed { set; get; } = 0m;

        public decimal WindDirection { set; get; } = 0m;

        public decimal WindDirectionSin { set; get; } = 0m;

        public decimal WindDirectionCos { set; get; } = 0m;

     

        public decimal AirTemperature { set; get; } = 99.9m;

        public decimal Humidity { set; get; } = -20m;

        public decimal AirPressure { set; get; } = 0m;

        public decimal Voltage { set; get; } =12.5m;

        public decimal WorkTemperature { set; get; } = 0m;

        public decimal WindSensorStatus { set; get; } = 0m;

        public decimal TemperatureHumiditySensorStatus  { set; get; } = 0m;

        public decimal RainRATSensorStatus { set; get; } = 0m;
        public decimal AirPressureSensorStatus { set; get; } = 0m;

        public decimal CrateTemperature { set; get; } = 0m;
        public decimal AirTemperature_AVG1  { set; get; } = 0m;

        public decimal HumidityMin_AVG1 { set; get; } = 0m;

        public decimal AirPressure_AVG1 { set; get; } = 0m;

        public decimal WindDirection2 { set; get; } = 0m;

        public decimal WindSpeed2  { set; get; } = 0m;


        public decimal WindDirection10 { set; get; } = 0m;


        public decimal WindSpeed10 { set; get; } = 0m;

        public decimal MinuteRainRAT { set; get; } = 0m;

        public decimal HourRainRAT { set; get; } = 0m;


        //

        public decimal WindSpeedVAG60 { set; get; } = 0m;

        public decimal WindSpeedVAG60_10 { set; get; } = 0m;

        public decimal WindSpeedVAG60_10_6  { set; get; } = 0m;

        public decimal WindSpeedVAG120 { set; get; } = 0m;

        public int WindDirectionVAG60 { set; get; } = 0;

        public int WindDirectionVAG60_10 { set; get; } = 0;

        public decimal WindDirectionVAG60_10_6  { set; get; } = 0m;
        public int WindDirectionVAG120 { set; get; } = 0; 

        public decimal WindSpeedVAG360 { set; get; } = 0m;

        public int WindDirectionVAG360 { set; get; } = 0;

        //
        public decimal HumidityAVG60 { set; get; } = 0m;
        public decimal   HumidityAVG360 { set; get; } =0m;

        //
        public decimal  AirPressureAVG60 { set; get; } = 0m;

        public decimal AirPressureAVG360 { set; get; } = 0m;

        //
        public decimal AirTemperatureAVG60 { set; get; } = 0m;

        public decimal AirTemperatureAVG360 { set; get; } = 0m;
    }
}
