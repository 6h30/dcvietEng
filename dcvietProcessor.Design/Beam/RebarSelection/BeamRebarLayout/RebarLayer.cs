using System;
using System.Collections.Generic;
using System.Linq;

namespace dcvietProcessor.Design.Beam.RebarSelection.BeamRebarLayout
{
    public sealed class RebarLayer
    {
        private readonly List<RebarReinforcement> _reinforcements;

        public int LayerNumber { get; }

        public IReadOnlyList<RebarReinforcement> Reinforcements
        {
            get { return _reinforcements.AsReadOnly(); }
        }

        public double TotalArea
        {
            get
            {
                return _reinforcements.Sum(
                    x => x.Area);
            }
        }

        public RebarLayer(int layerNumber)
        {
            if (layerNumber <= 0)
                throw new ArgumentException(
                    "LayerNumber must be > 0.");

            LayerNumber = layerNumber;

            _reinforcements =
                new List<RebarReinforcement>();
        }

        public void Add(
            RebarReinforcement reinforcement)
        {
            if (reinforcement == null)
                throw new ArgumentNullException(
                    nameof(reinforcement));

            _reinforcements.Add(reinforcement);
        }
    }
}