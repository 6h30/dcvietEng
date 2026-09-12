using dcvietProcessor.Design.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.Common.Material
{
    public sealed class SteelMaterialRepository
    {
        private readonly Dictionary<string, SteelMaterialInput> _materials;

        public SteelMaterialRepository()
        {
            _materials =
                new Dictionary<string, SteelMaterialInput>(
                    StringComparer.OrdinalIgnoreCase);

            RegisterMaterials();
        }

        public SteelMaterialInput Get(
            string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                throw new ArgumentException(
                    "Steel grade is required.",
                    nameof(grade));

            string key =
                grade.Trim().ToUpperInvariant();

            SteelMaterialInput material;

            if (!_materials.TryGetValue(
                key,
                out material))
            {
                throw new ArgumentException(
                    "Unsupported steel grade: "
                    + grade);
            }

            return Clone(material);
        }

        public bool Contains(
            string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                return false;

            return _materials.ContainsKey(
                grade.Trim().ToUpperInvariant());
        }

        private void RegisterMaterials()
        {
            // =====================================================
            // TCVN reinforcement steel
            //
            // Đơn vị:
            // Rs  : MPa
            // Rsc : MPa
            // Es  : MPa
            // =====================================================

            Add(
                "CB240-T",
                225.0,
                225.0,
                200000.0);

            Add(
                "CB300-V",
                280.0,
                280.0,
                200000.0);

            Add(
                "CB400-V",
                365.0,
                365.0,
                200000.0);

            Add(
                "CB500-V",
                435.0,
                435.0,
                200000.0);
        }

        private void Add(
            string grade,
            double rs,
            double rsc,
            double es)
        {
            _materials.Add(
                grade,
                new SteelMaterialInput
                {
                    Grade = grade,
                    Rs = rs,
                    Rsc = rsc,
                    Es = es
                });
        }

        private SteelMaterialInput Clone(
            SteelMaterialInput source)
        {
            return new SteelMaterialInput
            {
                Grade = source.Grade,
                Rs = source.Rs,
                Rsc = source.Rsc,
                Es = source.Es
            };
        }
    }
}
