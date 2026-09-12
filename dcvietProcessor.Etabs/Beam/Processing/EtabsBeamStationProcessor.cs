using System;
using dcvietProcessor.Etabs.Beam.Models;

namespace dcvietProcessor.Etabs.Beam.Processing
{
    public sealed class EtabsBeamStationProcessor
    {
        public double GetLength(EtabsBeamInput beam)
        {
            if (beam == null)
                throw new ArgumentNullException(nameof(beam));

            double length = 0.0;

            foreach (var station in beam.Stations)
            {
                if (station == null)
                    continue;

                if (station.Station > length)
                    length = station.Station;
            }

            return length;
        }

        public EtabsBeamRegion GetRegion(
            double station,
            double length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(length),
                    "Beam length must be greater than zero.");

            if (station < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(station),
                    "Station cannot be negative.");

            if (station < length / 4.0)
                return EtabsBeamRegion.Start;

            if (station <= 0.75 * length)
                return EtabsBeamRegion.Middle;

            return EtabsBeamRegion.End;
        }
    }
}