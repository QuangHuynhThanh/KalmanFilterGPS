using System;
using System.Collections.Generic;
using GpsKalMan.KalManGPS;
using NUnit.Framework;

namespace GpsKalMan
{
    public class Tests
    {
        
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var gpsTracks = new List<GpsTrack>();
            gpsTracks.Add(new GpsTrack(
                new GpsModel
                {
                    Lat = 10.7884861,
                    Long = 106.6995996,
                    Alt = 0,
                    RecordTime = new DateTime(2021, 6, 5, 9, 0, 0)
                },
                new GpsModel
                {
                    Lat = 10.7905096,
                    Long = 106.7015201,
                    Alt = 0,
                    RecordTime = new DateTime(2021, 6, 5, 9, 15, 0)
                }, 100
            ));
            
            var kalmanLib = new KalManTestDataProcessing(gpsTracks);
            
            kalmanLib.GenerateNoise(RandomMode.RandomNumber);
            kalmanLib.RunKalMan();
            var error = kalmanLib.CalculateError();
            kalmanLib.SaveData("Data1.txt");
            Assert.Pass();
        }
    }
}