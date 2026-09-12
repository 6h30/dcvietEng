using System;
using System.Collections.Generic;
using System.Linq;

using dcvietProcessor.Design.Beam.RebarSelection.Models;
using dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout;

using BeamLayout =
    dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout.BeamRebarLayout;

namespace dcvietProcessor.Design.Beam.RebarSelection
{
    /// <summary>
    /// Tối ưu bố trí cốt thép dọc toàn dầm.
    ///
    /// Nguyên tắc:
    ///
    /// TOP và BOTTOM được xử lý độc lập.
    ///
    /// Với mỗi phía:
    ///
    ///     AsContinuousTarget
    ///         =
    ///     Min(
    ///         AsLeft,
    ///         AsMiddle,
    ///         AsRight)
    ///
    /// Thép chạy suốt:
    ///
    ///     - ưu tiên số thanh ít nhất;
    ///     - trong cùng số thanh,
    ///       BeamRebarSelector chọn candidate
    ///       có diện tích dư nhỏ nhất.
    ///
    /// Sau khi chọn Continuous:
    ///
    ///     AsAdditional
    ///         =
    ///     Max(
    ///         0,
    ///         AsRequired - AsContinuous)
    ///
    /// và chọn gia cường riêng tại:
    ///
    ///     Left
    ///     Middle
    ///     Right
    ///
    /// RebarSelectionInput vẫn chỉ là DTO
    /// cho một lần selection.
    /// </summary>
    public sealed class BeamRebarLayoutOptimizer
    {
        private readonly BeamRebarSelector _selector;

        public BeamRebarLayoutOptimizer()
        {
            _selector =
                new BeamRebarSelector();
        }

        // =========================================================
        // MAIN
        // =========================================================

        public BeamLayout Optimize(
            double topLeftRequiredArea,
            double topMiddleRequiredArea,
            double topRightRequiredArea,

            double bottomLeftRequiredArea,
            double bottomMiddleRequiredArea,
            double bottomRightRequiredArea,

            RebarSelectionInput continuousSelection,
            RebarSelectionInput additionalSelection)
        {
            if (continuousSelection == null)
            {
                throw new ArgumentNullException(
                    nameof(continuousSelection));
            }

            if (additionalSelection == null)
            {
                throw new ArgumentNullException(
                    nameof(additionalSelection));
            }

            ValidateRequiredArea(
                topLeftRequiredArea,
                nameof(topLeftRequiredArea));

            ValidateRequiredArea(
                topMiddleRequiredArea,
                nameof(topMiddleRequiredArea));

            ValidateRequiredArea(
                topRightRequiredArea,
                nameof(topRightRequiredArea));

            ValidateRequiredArea(
                bottomLeftRequiredArea,
                nameof(bottomLeftRequiredArea));

            ValidateRequiredArea(
                bottomMiddleRequiredArea,
                nameof(bottomMiddleRequiredArea));

            ValidateRequiredArea(
                bottomRightRequiredArea,
                nameof(bottomRightRequiredArea));

            // -----------------------------------------------------
            // TOP
            // -----------------------------------------------------

            RebarSide top =
                OptimizeSide(
                    topLeftRequiredArea,
                    topMiddleRequiredArea,
                    topRightRequiredArea,
                    continuousSelection,
                    additionalSelection);

            // -----------------------------------------------------
            // BOTTOM
            // -----------------------------------------------------

            RebarSide bottom =
                OptimizeSide(
                    bottomLeftRequiredArea,
                    bottomMiddleRequiredArea,
                    bottomRightRequiredArea,
                    continuousSelection,
                    additionalSelection);

            return new BeamLayout(
                top,
                bottom);
        }

        // =========================================================
        // OPTIMIZE ONE SIDE
        // =========================================================

        private RebarSide OptimizeSide(
            double leftRequiredArea,
            double middleRequiredArea,
            double rightRequiredArea,
            RebarSelectionInput continuousSelection,
            RebarSelectionInput additionalSelection)
        {
            // -----------------------------------------------------
            // 1. As chạy suốt mục tiêu
            // -----------------------------------------------------

            double continuousRequiredArea =
                GetContinuousRequiredArea(
                    leftRequiredArea,
                    middleRequiredArea,
                    rightRequiredArea);

            // -----------------------------------------------------
            // 2. Chọn Continuous
            // -----------------------------------------------------

            RebarSelectionResult continuousResult =
                SelectContinuous(
                    continuousRequiredArea,
                    continuousSelection);

            if (continuousResult == null ||
                continuousResult.Candidate == null)
            {
                throw new InvalidOperationException(
                    "Cannot find continuous reinforcement candidate.");
            }

            RebarCandidate continuousCandidate =
                continuousResult.Candidate;

            double continuousArea =
                continuousCandidate.Area;

            // -----------------------------------------------------
            // 3. Continuous Layer 1
            // -----------------------------------------------------

            List<RebarLayer> continuousLayers =
                new List<RebarLayer>();

            continuousLayers.Add(
                CreateLayer(
                    1,
                    continuousCandidate));

            // -----------------------------------------------------
            // 4. As gia cường còn thiếu
            // -----------------------------------------------------

            double leftAdditionalArea =
                GetAdditionalRequiredArea(
                    leftRequiredArea,
                    continuousArea);

            double middleAdditionalArea =
                GetAdditionalRequiredArea(
                    middleRequiredArea,
                    continuousArea);

            double rightAdditionalArea =
                GetAdditionalRequiredArea(
                    rightRequiredArea,
                    continuousArea);

            // -----------------------------------------------------
            // 5. Additional
            // -----------------------------------------------------

            List<RebarLayer> leftLayers =
                SelectAdditionalLayers(
                    leftAdditionalArea,
                    additionalSelection);

            List<RebarLayer> middleLayers =
                SelectAdditionalLayers(
                    middleAdditionalArea,
                    additionalSelection);

            List<RebarLayer> rightLayers =
                SelectAdditionalLayers(
                    rightAdditionalArea,
                    additionalSelection);

            // -----------------------------------------------------
            // 6. RESULT
            // -----------------------------------------------------

            return new RebarSide(
                continuousLayers,
                leftLayers,
                middleLayers,
                rightLayers);
        }

        // =========================================================
        // CONTINUOUS REQUIRED AREA
        // =========================================================

        private static double GetContinuousRequiredArea(
            double leftRequiredArea,
            double middleRequiredArea,
            double rightRequiredArea)
        {
            return Math.Min(
                leftRequiredArea,
                Math.Min(
                    middleRequiredArea,
                    rightRequiredArea));
        }

        // =========================================================
        // SELECT CONTINUOUS
        // =========================================================

        /// <summary>
        /// Chọn thép chạy suốt.
        ///
        /// Ưu tiên tuyệt đối số thanh ít trước.
        ///
        /// Ví dụ:
        ///
        /// MinBarCount = 2
        /// MaxBarCount = 5
        ///
        /// sẽ thử:
        ///
        /// 2 bars
        /// 3 bars
        /// 4 bars
        /// 5 bars
        ///
        /// Ngay khi một BarCount có candidate đủ As,
        /// quá trình dừng.
        /// </summary>
        private RebarSelectionResult SelectContinuous(
            double requiredArea,
            RebarSelectionInput settings)
        {
            double targetArea =
                requiredArea;

            // -----------------------------------------------------
            // Nếu As yêu cầu nhỏ nhất = 0
            //
            // vẫn cần bố trí lượng Continuous tối thiểu
            // theo MinBarCount + MinDiameter.
            // -----------------------------------------------------

            if (targetArea <= 0.0)
            {
                targetArea =
                    GetMinimumContinuousArea(
                        settings);
            }

            // -----------------------------------------------------
            // ƯU TIÊN SỐ THANH ÍT NHẤT
            // -----------------------------------------------------

            for (
                int barCount = settings.MinBarCount;
                barCount <= settings.MaxBarCount;
                barCount++)
            {
                RebarSelectionInput selectionInput =
                    CreateSelectionInput(
                        settings,
                        targetArea,
                        barCount,
                        barCount);

                RebarSelectionResult result =
                    _selector.Select(
                        selectionInput);

                if (result != null &&
                    result.Candidate != null)
                {
                    return result;
                }
            }

            return new RebarSelectionResult(
                false,
                null,
                targetArea);
        }

        // =========================================================
        // ADDITIONAL REQUIRED AREA
        // =========================================================

        private static double GetAdditionalRequiredArea(
            double requiredArea,
            double continuousArea)
        {
            return Math.Max(
                0.0,
                requiredArea - continuousArea);
        }

        // =========================================================
        // SELECT ADDITIONAL
        // =========================================================

        /// <summary>
        /// Chọn thép gia cường cho một region.
        ///
        /// Hiện tại một region sử dụng một layer.
        ///
        /// Nếu không tìm được candidate đủ As
        /// sẽ báo lỗi thay vì tạo layout thiếu thép.
        /// </summary>
        private List<RebarLayer> SelectAdditionalLayers(
            double requiredArea,
            RebarSelectionInput settings)
        {
            List<RebarLayer> layers =
                new List<RebarLayer>();

            if (requiredArea <= 0.0)
            {
                return layers;
            }

            RebarSelectionInput selectionInput =
                CreateSelectionInput(
                    settings,
                    requiredArea,
                    settings.MinBarCount,
                    settings.MaxBarCount);

            RebarSelectionResult result =
                _selector.Select(
                    selectionInput);

            if (result == null ||
                result.Candidate == null)
            {
                throw new InvalidOperationException(
                    "Cannot find additional reinforcement candidate.");
            }

            layers.Add(
                CreateLayer(
                    1,
                    result.Candidate));

            return layers;
        }

        // =========================================================
        // CREATE SELECTION INPUT
        // =========================================================

        /// <summary>
        /// Tạo một RebarSelectionInput mới
        /// từ bộ settings ban đầu.
        ///
        /// Không sửa object input từ bên ngoài.
        /// </summary>
        private static RebarSelectionInput CreateSelectionInput(
            RebarSelectionInput source,
            double requiredArea,
            int minBarCount,
            int maxBarCount)
        {
            return new RebarSelectionInput
            {
                RequiredArea =
                    requiredArea,

                MinDiameter =
                    source.MinDiameter,

                MaxDiameter =
                    source.MaxDiameter,

                MinBarCount =
                    minBarCount,

                MaxBarCount =
                    maxBarCount,

                AllowedDiameters =
                    source.AllowedDiameters
            };
        }

        // =========================================================
        // CREATE LAYER
        // =========================================================

        private static RebarLayer CreateLayer(
            int layerNumber,
            RebarCandidate candidate)
        {
            RebarLayer layer =
                new RebarLayer(
                    layerNumber);

            RebarReinforcement reinforcement =
                new RebarReinforcement(
                    candidate.BarCount,
                    candidate.Diameter);

            layer.Add(
                reinforcement);

            return layer;
        }

        // =========================================================
        // MINIMUM CONTINUOUS AREA
        // =========================================================

        private static double GetMinimumContinuousArea(
            RebarSelectionInput settings)
        {
            double diameter =
                GetMinimumAllowedDiameter(
                    settings);

            return settings.MinBarCount
                * Math.PI
                * diameter
                * diameter
                / 4.0;
        }

        // =========================================================
        // MINIMUM ALLOWED DIAMETER
        // =========================================================

        private static double GetMinimumAllowedDiameter(
            RebarSelectionInput settings)
        {
            if (settings.AllowedDiameters == null ||
                settings.AllowedDiameters.Length == 0)
            {
                return settings.MinDiameter;
            }

            double[] validDiameters =
                settings.AllowedDiameters
                    .Where(
                        x =>
                            x >= settings.MinDiameter
                            &&
                            x <= settings.MaxDiameter)
                    .OrderBy(
                        x => x)
                    .ToArray();

            if (validDiameters.Length == 0)
            {
                throw new InvalidOperationException(
                    "No allowed reinforcement diameter found.");
            }

            return validDiameters[0];
        }

        // =========================================================
        // VALIDATION
        // =========================================================

        private static void ValidateRequiredArea(
            double requiredArea,
            string parameterName)
        {
            if (requiredArea < 0.0)
            {
                throw new ArgumentException(
                    "Required reinforcement area must be >= 0.",
                    parameterName);
            }
        }
    }
}