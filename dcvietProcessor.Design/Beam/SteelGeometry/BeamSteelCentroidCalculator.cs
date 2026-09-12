using System;
using System.Collections.Generic;
using System.Linq;

using dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout;

namespace dcvietProcessor.Design.Beam.SteelGeometry
{
    /// <summary>
    /// Tính khoảng cách từ mép bê tông
    /// đến trọng tâm cốt thép của một phía dầm
    /// tại một region cụ thể.
    ///
    /// Ví dụ:
    ///
    /// TOP - Left
    ///     = Continuous + Left
    ///
    /// BOTTOM - Middle
    ///     = Continuous + Middle
    ///
    /// Công thức:
    ///
    ///     a = Σ(Ai * zi) / ΣAi
    ///
    /// Trong đó:
    ///
    ///     Ai : diện tích nhóm thép
    ///     zi : khoảng cách từ mép bê tông
    ///          đến tâm nhóm thép
    ///
    /// Đơn vị:
    ///
    ///     mm
    ///     mm2
    /// </summary>
    public sealed class BeamSteelCentroidCalculator
    {
        public double Calculate(
            RebarSide side,
            RebarRegion region,
            double concreteCover,
            params double[] layerSpacings)
        {
            if (side == null)
            {
                throw new ArgumentNullException(
                    nameof(side));
            }

            IReadOnlyList<RebarLayer> continuousLayers =
                side.GetLayers(
                    RebarRegion.Continuous);

            IReadOnlyList<RebarLayer> additionalLayers =
                region == RebarRegion.Continuous
                    ? new List<RebarLayer>()
                    : side.GetLayers(region);

            return CalculateCentroid(
                continuousLayers,
                additionalLayers,
                concreteCover,
                layerSpacings);
        }

        // =========================================================
        // CALCULATE
        // =========================================================

        private static double CalculateCentroid(
            IReadOnlyList<RebarLayer> continuousLayers,
            IReadOnlyList<RebarLayer> additionalLayers,
            double concreteCover,
            double[] layerSpacings)
        {
            int maximumLayerNumber =
                GetMaximumLayerNumber(
                    continuousLayers,
                    additionalLayers);

            if (maximumLayerNumber <= 0)
            {
                throw new InvalidOperationException(
                    "No reinforcement found.");
            }

            double totalArea =
                0.0;

            double firstMoment =
                0.0;

            // Khoảng cách từ mép bê tông
            // đến mép ngoài layer hiện tại.
            double layerBase =
                concreteCover;

            // =====================================================
            // Duyệt theo LayerNumber thực tế:
            //
            // Layer 1
            // Layer 2
            // Layer 3
            // ...
            //
            // Không phụ thuộc việc layer đó
            // có reinforcement hay không.
            // =====================================================

            for (
                int layerNumber = 1;
                layerNumber <= maximumLayerNumber;
                layerNumber++)
            {
                RebarLayer continuousLayer =
                    FindLayer(
                        continuousLayers,
                        layerNumber);

                RebarLayer additionalLayer =
                    FindLayer(
                        additionalLayers,
                        layerNumber);

                // -------------------------------------------------
                // Continuous
                // -------------------------------------------------

                AddLayerToCentroid(
                    continuousLayer,
                    layerBase,
                    ref totalArea,
                    ref firstMoment);

                // -------------------------------------------------
                // Additional
                // -------------------------------------------------

                AddLayerToCentroid(
                    additionalLayer,
                    layerBase,
                    ref totalArea,
                    ref firstMoment);

                // -------------------------------------------------
                // Tính vị trí layer kế tiếp.
                //
                // VBA:
                //
                // Max(
                //     d_ChaySuot,
                //     d_GiaCuong
                // )
                // + spacing
                // -------------------------------------------------

                if (layerNumber < maximumLayerNumber)
                {
                    double maximumDiameter =
                        GetMaximumDiameter(
                            continuousLayer,
                            additionalLayer);

                    double spacing =
                        GetLayerSpacing(
                            layerNumber,
                            layerSpacings);

                    layerBase +=
                        maximumDiameter
                        + spacing;
                }
            }

            if (totalArea <= 0.0)
            {
                throw new InvalidOperationException(
                    "Total reinforcement area must be greater than zero.");
            }

            return
                firstMoment
                / totalArea;
        }

        // =========================================================
        // LAYER CONTRIBUTION
        // =========================================================

        private static void AddLayerToCentroid(
            RebarLayer layer,
            double layerBase,
            ref double totalArea,
            ref double firstMoment)
        {
            if (layer == null)
            {
                return;
            }

            foreach (
                RebarReinforcement reinforcement
                in layer.Reinforcements)
            {
                double area =
                    reinforcement.Area;

                double z =
                    layerBase
                    + reinforcement.Diameter
                    / 2.0;

                totalArea +=
                    area;

                firstMoment +=
                    area * z;
            }
        }

        // =========================================================
        // MAXIMUM LAYER NUMBER
        // =========================================================

        private static int GetMaximumLayerNumber(
            IReadOnlyList<RebarLayer> continuousLayers,
            IReadOnlyList<RebarLayer> additionalLayers)
        {
            int continuousMaximum =
                GetMaximumLayerNumber(
                    continuousLayers);

            int additionalMaximum =
                GetMaximumLayerNumber(
                    additionalLayers);

            return Math.Max(
                continuousMaximum,
                additionalMaximum);
        }

        private static int GetMaximumLayerNumber(
            IReadOnlyList<RebarLayer> layers)
        {
            if (layers == null ||
                layers.Count == 0)
            {
                return 0;
            }

            return layers.Max(
                x => x.LayerNumber);
        }

        // =========================================================
        // MAXIMUM DIAMETER
        // =========================================================

        private static double GetMaximumDiameter(
            RebarLayer continuousLayer,
            RebarLayer additionalLayer)
        {
            double continuousDiameter =
                GetMaximumDiameter(
                    continuousLayer);

            double additionalDiameter =
                GetMaximumDiameter(
                    additionalLayer);

            return Math.Max(
                continuousDiameter,
                additionalDiameter);
        }

        private static double GetMaximumDiameter(
            RebarLayer layer)
        {
            if (layer == null ||
                layer.Reinforcements.Count == 0)
            {
                return 0.0;
            }

            return layer.Reinforcements.Max(
                x => x.Diameter);
        }

        // =========================================================
        // FIND LAYER
        // =========================================================

        private static RebarLayer FindLayer(
            IReadOnlyList<RebarLayer> layers,
            int layerNumber)
        {
            if (layers == null)
            {
                return null;
            }

            return layers.FirstOrDefault(
                x =>
                    x.LayerNumber
                    == layerNumber);
        }

        // =========================================================
        // SPACING
        // =========================================================

        private static double GetLayerSpacing(
            int currentLayerNumber,
            double[] layerSpacings)
        {
            if (layerSpacings == null)
            {
                return 0.0;
            }

            int index =
                currentLayerNumber - 1;

            if (index < 0 ||
                index >= layerSpacings.Length)
            {
                return 0.0;
            }

            return layerSpacings[index];
        }
    }
}