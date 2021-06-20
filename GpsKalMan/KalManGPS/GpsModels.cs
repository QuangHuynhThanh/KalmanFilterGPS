using System;
using System.Collections.Generic;

namespace GpsKalMan.KalManGPS
{
    public enum RandomMode
    {
        RandomNumber = 0,
        Sin = 1
    }

    public class GpsModel
    {
        public double Lat = 0.0;
        public double Long = 0.0;
        public double Alt;
        public DateTime RecordTime;
    }

    public class GpsTrack
    {
        private GpsModel Start;
        private GpsModel End;
        private int NumnberOfRecord;
        private List<GpsModel> ResultTrack;

        public GpsTrack(GpsModel start, GpsModel end, int numnberOfRecord)
        {
            Start = start;
            End = end;
            NumnberOfRecord = numnberOfRecord;
        }

        public List<GpsModel> Proceed()
        {
            ResultTrack = new List<GpsModel>();
            var diffLat = End.Lat - Start.Lat;
            var diffLong = End.Long - Start.Long;
            var diffAlt = End.Alt - Start.Alt;
            var diffRecordTime = (End.RecordTime - Start.RecordTime)/NumnberOfRecord;

            for (var i = 0; i < NumnberOfRecord; i++)
            {
                ResultTrack.Add(new GpsModel
                {
                    Lat = Start.Lat + diffLat,
                    Long = Start.Long + diffLong,
                    Alt = Start.Alt + diffAlt,
                    RecordTime = Start.RecordTime + diffRecordTime * i
                });
            }

            return ResultTrack;
        }

    }

}