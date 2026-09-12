using dcvietProcessor.Design.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcvietProcessor.Design.Common.Material
{
    public sealed class ConcreteMaterialRepository
    {
        private readonly Dictionary<string, ConcreteMaterialInput> _materials;

        public ConcreteMaterialRepository()
        {
            _materials =
                new Dictionary<string, ConcreteMaterialInput>(
                    StringComparer.OrdinalIgnoreCase);

            RegisterMaterials();
        }

        public ConcreteMaterialInput Get(
            string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                throw new ArgumentException(
                    "Concrete grade is required.",
                    nameof(grade));

            string key =
                grade.Trim().ToUpperInvariant();

            ConcreteMaterialInput material;

            if (!_materials.TryGetValue(
                key,
                out material))
            {
                throw new ArgumentException(
                    "Unsupported concrete grade: "
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
            // TCVN 5574:2018
            //
            // Đơn vị:
            // Rb     : MPa
            // RbSer  : MPa
            // Rbt    : MPa
            // RbtSer : MPa
            // Eb     : MPa
            // =====================================================

            Add(
                "B30",
                17.0,
                22.0,
                1.15,
                1.75,
                32500.0);

            Add(
                "B35",
                19.5,
                25.5,
                1.30,
                1.95,
                34500.0);

            Add(
                "B40",
                22.0,
                29.0,
                1.40,
                2.10,
                36000.0);

            Add(
                "B45",
                25.0,
                32.0,
                1.50,
                2.20,
                37000.0);

            Add(
                "B50",
                27.5,
                36.0,
                1.60,
                2.45,
                38000.0);

            Add(
                "B55",
                30.0,
                39.0,
                1.70,
                2.55,
                39000.0);

            Add(
                "B60",
                33.0,
                43.0,
                1.80,
                2.70,
                39500.0);

            Add(
                "B70",
                40.0,
                52.0,
                1.90,
                2.90,
                40500.0);

            Add(
                "B80",
                45.0,
                58.0,
                2.10,
                3.10,
                41500.0);

            Add(
                "B90",
                50.0,
                64.0,
                2.20,
                3.30,
                42000.0);

            Add(
                "B100",
                55.0,
                70.0,
                2.30,
                3.50,
                42500.0);
        }

        private void Add(
            string grade,
            double rb,
            double rbSer,
            double rbt,
            double rbtSer,
            double eb)
        {
            _materials.Add(
                grade,
                new ConcreteMaterialInput
                {
                    Grade = grade,
                    Rb = rb,
                    RbSer = rbSer,
                    Rbt = rbt,
                    RbtSer = rbtSer,
                    Eb = eb
                });
        }

        private ConcreteMaterialInput Clone(
            ConcreteMaterialInput source)
        {
            return new ConcreteMaterialInput
            {
                Grade = source.Grade,
                Rb = source.Rb,
                RbSer = source.RbSer,
                Rbt = source.Rbt,
                RbtSer = source.RbtSer,
                Eb = source.Eb
            };
        }

        public IReadOnlyList<ConcreteMaterialInput> GetAll()
        {
            return _materials.Values
                .OrderBy(x => GetGradeNumber(x.Grade))
                .Select(Clone)
                .ToList();
        }

        private static int GetGradeNumber(string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
                return 0;

            string numberText =
                grade.Trim().TrimStart('B', 'b');

            int gradeNumber;

            return int.TryParse(
                numberText,
                out gradeNumber)
                ? gradeNumber
                : 0;
        }
    }
}
