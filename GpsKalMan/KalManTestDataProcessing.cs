using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GpsKalMan.KalManGPS;

namespace GpsKalMan
{
    public class KalManTestDataProcessing
    {
        private Random random = new Random();

        public List<GpsModel> InputLocations;
        public List<GpsModel> TestLocations;
        public List<GpsModel> KalmanLocations;

        public KalManTestDataProcessing(List<GpsTrack> tracks)
        {
            InputLocations = new List<GpsModel>();
            if (tracks?.Any() != true)
                return;

            tracks.ForEach(x => InputLocations.AddRange(x.Proceed()));
        }
        
        public void GenerateNoise(RandomMode randomMode)
        {
            TestLocations = new List<GpsModel>();
            switch (randomMode)
            {
                case RandomMode.RandomNumber:
                    InputLocations.ForEach(x => TestLocations.Add(GpsRandom(x)));
                    break;
                case RandomMode.Sin:
                    break;
            }
        }

        public void RunKalMan()
        {
            KalmanLocations = new List<GpsModel>();
            //Proceed TestLocations list and put it to KalmanLocations list
            for (var i = 0; i < TestLocations.Count; i++)
            {
                if (i == 0)
                {
                    KalmanLocations.Add(TestLocations[i]);
                }
                else
                {
                    var result = TestLocations[i];
                    KalmanLocations.Add(result);   
                }
            }
        }

        public double CalculateError()
        {
            return KalmanLocations
                .Select((x, i) => CalculateDistance(x, InputLocations[i]))
                .Sum();
        }

        private double CalculateDistance(GpsModel point1, GpsModel point2)
        {
            return Math.Sqrt(
                Math.Pow(point1.Lat-point2.Lat,2) +
                Math.Pow(point1.Long-point2.Long,2) +
                Math.Pow(point1.Alt-point2.Alt,2)
            );
        }

        private GpsModel GpsRandom(GpsModel val)
        {
            return new GpsModel
            {
                Lat = val.Lat + 0.0001 * (0.5 - random.NextDouble()),
                Long = val.Long + 0.0001 * (0.5 - random.NextDouble()),
                Alt = val.Alt + 0.0001 * (0.5 - random.NextDouble()),
                RecordTime = val.RecordTime
            };
        }
        
        public void SaveData(string fileName)
        {
            TextWriter tw = new StreamWriter(fileName);
            var header =
                "InputLat,InputLong,InputAlt,InputTime,TestLat,TestLong,TestAlt,TestTime,KalManLat,KalManLong,KalmanAlt,KalManTime";
            tw.WriteLine(header);
            for (var i =0; i< InputLocations.Count;i++)
            {
                var row =
                    $"{InputLocations[i].Lat},{InputLocations[i].Long},{InputLocations[i].Alt},{InputLocations[i].RecordTime}," +
                    $"{TestLocations[i].Lat},{TestLocations[i].Long},{TestLocations[i].Alt},{TestLocations[i].RecordTime}," +
                    $"{KalmanLocations[i].Lat},{KalmanLocations[i].Long},{KalmanLocations[i].Alt},{KalmanLocations[i].RecordTime}" +
                    "\n";
                tw.WriteLine(row);
            }
            tw.Close();
        }
    }
}