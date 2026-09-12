using System;
using System.Collections.Generic;
using System.Linq;

namespace dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout
{
    /// <summary>
    /// Bố trí cốt thép dọc của một phía dầm:
    /// TOP hoặc BOTTOM.
    ///
    /// Mỗi phía gồm 4 vùng:
    ///
    /// Continuous
    ///     ├── Layer 1
    ///     ├── Layer 2
    ///     └── Layer N
    ///
    /// Left
    ///     ├── Layer 1
    ///     ├── Layer 2
    ///     └── Layer N
    ///
    /// Middle
    ///     ├── Layer 1
    ///     ├── Layer 2
    ///     └── Layer N
    ///
    /// Right
    ///     ├── Layer 1
    ///     ├── Layer 2
    ///     └── Layer N
    ///
    /// Lưu ý:
    /// - Continuous là thép chạy suốt.
    /// - Left / Middle / Right là thép gia cường
    ///   tại từng vùng theo chiều dài dầm.
    /// </summary>
    public sealed class RebarSide
    {
        private readonly List<RebarLayer> _continuous;
        private readonly List<RebarLayer> _left;
        private readonly List<RebarLayer> _middle;
        private readonly List<RebarLayer> _right;

        // =========================================================
        // LAYERS
        // =========================================================

        public IReadOnlyList<RebarLayer> Continuous
        {
            get { return _continuous.AsReadOnly(); }
        }

        public IReadOnlyList<RebarLayer> Left
        {
            get { return _left.AsReadOnly(); }
        }

        public IReadOnlyList<RebarLayer> Middle
        {
            get { return _middle.AsReadOnly(); }
        }

        public IReadOnlyList<RebarLayer> Right
        {
            get { return _right.AsReadOnly(); }
        }

        // =========================================================
        // AREA THEO REGION
        // =========================================================

        public double ContinuousArea
        {
            get
            {
                return GetRegionArea(
                    RebarRegion.Continuous);
            }
        }

        public double LeftArea
        {
            get
            {
                return GetRegionArea(
                    RebarRegion.Left);
            }
        }

        public double MiddleArea
        {
            get
            {
                return GetRegionArea(
                    RebarRegion.Middle);
            }
        }

        public double RightArea
        {
            get
            {
                return GetRegionArea(
                    RebarRegion.Right);
            }
        }

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public RebarSide(
            IEnumerable<RebarLayer> continuous,
            IEnumerable<RebarLayer> left = null,
            IEnumerable<RebarLayer> middle = null,
            IEnumerable<RebarLayer> right = null)
        {
            _continuous =
                ToLayerList(continuous);

            _left =
                ToLayerList(left);

            _middle =
                ToLayerList(middle);

            _right =
                ToLayerList(right);
        }

        // =========================================================
        // GET LAYERS
        // =========================================================

        /// <summary>
        /// Lấy toàn bộ layer của một region.
        /// </summary>
        public IReadOnlyList<RebarLayer> GetLayers(
            RebarRegion region)
        {
            switch (region)
            {
                case RebarRegion.Continuous:
                    return Continuous;

                case RebarRegion.Left:
                    return Left;

                case RebarRegion.Middle:
                    return Middle;

                case RebarRegion.Right:
                    return Right;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(region));
            }
        }

        /// <summary>
        /// Lấy một layer cụ thể trong một region.
        /// </summary>
        public RebarLayer GetLayer(
            RebarRegion region,
            int layerNumber)
        {
            if (layerNumber <= 0)
                throw new ArgumentException(
                    "LayerNumber must be > 0.",
                    nameof(layerNumber));

            return GetLayers(region)
                .FirstOrDefault(
                    x => x.LayerNumber == layerNumber);
        }

        // =========================================================
        // AREA
        // =========================================================

        /// <summary>
        /// Diện tích riêng của một region.
        ///
        /// Continuous:
        ///     chỉ thép chạy suốt.
        ///
        /// Left / Middle / Right:
        ///     chỉ thép gia cường của region đó.
        /// </summary>
        public double GetRegionArea(
            RebarRegion region)
        {
            return GetLayers(region)
                .Sum(x => x.TotalArea);
        }

        /// <summary>
        /// Tổng diện tích thép tại một vị trí dọc dầm.
        ///
        /// Ví dụ:
        ///
        /// Left =
        /// Continuous + Left
        ///
        /// Middle =
        /// Continuous + Middle
        ///
        /// Right =
        /// Continuous + Right
        /// </summary>
        public double GetSectionArea(
            RebarRegion region)
        {
            if (region == RebarRegion.Continuous)
                return ContinuousArea;

            return ContinuousArea
                + GetRegionArea(region);
        }

        // =========================================================
        // CHECK
        // =========================================================

        public bool HasLayers(
            RebarRegion region)
        {
            return GetLayers(region).Count > 0;
        }

        public bool HasContinuous
        {
            get
            {
                return _continuous.Count > 0;
            }
        }

        public bool HasReinforcement
        {
            get
            {
                return _left.Count > 0
                    || _middle.Count > 0
                    || _right.Count > 0;
            }
        }

        // =========================================================
        // PRIVATE
        // =========================================================

        private static List<RebarLayer> ToLayerList(
            IEnumerable<RebarLayer> layers)
        {
            if (layers == null)
                return new List<RebarLayer>();

            return layers
                .Where(x => x != null)
                .OrderBy(x => x.LayerNumber)
                .ToList();
        }
    }
}