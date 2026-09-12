using System;
using dcvietProcessor.Etabs.Beam.Combination;
using dcvietProcessor.Etabs.Beam.Models;

namespace dcvietProcessor.Etabs.Beam.Processing
{
    /// <summary>
    /// Processes the ETABS force results of a single beam.
    ///
    /// Responsibilities:
    /// - Determine beam length from stations.
    /// - Classify stations into Start / Middle / End regions.
    /// - Forward forces to the appropriate force processors.
    /// - Assemble the processed result for the beam.
    ///
    /// This class does NOT:
    /// - Read ETABS API.
    /// - Read Excel.
    /// - Write Excel.
    /// - Perform reinforcement design.
    /// - Perform code checks.
    /// </summary>
    public sealed class EtabsBeamResultProcessor
    {
        private readonly EtabsBeamStationProcessor _stationProcessor;

        public EtabsBeamResultProcessor(
            EtabsBeamStationProcessor stationProcessor)
        {
            _stationProcessor =
                stationProcessor
                ?? throw new ArgumentNullException(
                    nameof(stationProcessor));
        }

        /// <summary>
        /// Processes one ETABS beam.
        /// </summary>
        public EtabsBeamProcessedResult Process(
            EtabsBeamInput beam)
        {
            if (beam == null)
                throw new ArgumentNullException(nameof(beam));

            if (beam.Stations == null ||
                beam.Stations.Count == 0)
            {
                return null;
            }

            // =====================================================
            // 1. DETERMINE BEAM LENGTH
            // =====================================================

            double length =
                _stationProcessor.GetLength(beam);

            if (length <= 0.0)
                return null;

            // =====================================================
            // 2. CREATE FORCE SET PROCESSOR
            //
            // This processor handles:
            //     M3
            //     V2
            //     T
            // =====================================================

            var forceSetProcessor =
                new EtabsBeamForceSetProcessor();

            // =====================================================
            // 3. PROCESS ALL STATIONS
            // =====================================================

            foreach (var station in beam.Stations)
            {
                if (station == null)
                    continue;

                // -------------------------------------------------
                // Ignore stations outside the physical beam length
                // -------------------------------------------------

                if (station.Station < 0.0)
                    continue;

                if (station.Station > length)
                    continue;

                // -------------------------------------------------
                // Determine region
                //
                // Start:
                //     station < L / 4
                //
                // Middle:
                //     L / 4 <= station <= 3L / 4
                //
                // End:
                //     station > 3L / 4
                // -------------------------------------------------

                EtabsBeamRegion region =
                    _stationProcessor.GetRegion(
                        station.Station,
                        length);

                // -------------------------------------------------
                // Process forces at this station
                // -------------------------------------------------

                if (station.Forces == null)
                    continue;

                foreach (var force in station.Forces)
                {
                    if (force == null)
                        continue;

                    forceSetProcessor.Process(
                        region,
                        force);
                }
            }

            // =====================================================
            // 4. BUILD FINAL BEAM RESULT
            // =====================================================

            return new EtabsBeamProcessedResult
            {
                Label = beam.Label,

                Story = beam.Story,

                Width = beam.Width,

                Height = beam.Height,

                Length = length,

                Start =
                    forceSetProcessor.CreateResult(
                        EtabsBeamRegion.Start),

                Middle =
                    forceSetProcessor.CreateResult(
                        EtabsBeamRegion.Middle),

                End =
                    forceSetProcessor.CreateResult(
                        EtabsBeamRegion.End),

                AdditionalData =
                    beam.AdditionalData
            };
        }
    }
}