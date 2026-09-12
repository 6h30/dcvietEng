using dcvietProcessor.Design.Common.Material;
using dcvietProcessor.Design.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace dcvietUI.ViewModels.Material
{
    public sealed class MaterialViewModel :
        INotifyPropertyChanged
    {
        private readonly ConcreteMaterialRepository
            _concreteRepository;

        private ConcreteMaterialInput
            _selectedConcrete;

        public MaterialViewModel()
        {
            _concreteRepository =
                new ConcreteMaterialRepository();

            ConcreteGrades =
                _concreteRepository.GetAll();

            ProjectMaterial =
                new ProjectDesignMaterialInput();

            SelectedConcrete =
                FindConcrete("B40")
                ?? ConcreteGrades.FirstOrDefault();
        }

        public IReadOnlyList<ConcreteMaterialInput>
            ConcreteGrades
        { get; }

        /// <summary>
        /// DTO lưu lựa chọn vật liệu của dự án.
        /// Đối tượng này sẽ được truyền sang Design.
        /// </summary>
        public ProjectDesignMaterialInput
            ProjectMaterial
        { get; }

        public ConcreteMaterialInput SelectedConcrete
        {
            get
            {
                return _selectedConcrete;
            }

            set
            {
                if (ReferenceEquals(
                    _selectedConcrete,
                    value))
                {
                    return;
                }

                _selectedConcrete = value;

                ProjectMaterial.ConcreteGrade =
                    _selectedConcrete?.Grade;

                OnPropertyChanged();
                OnPropertyChanged(
                    nameof(ConcreteSummary));
            }
        }

        public string ConcreteSummary
        {
            get
            {
                if (SelectedConcrete == null)
                    return string.Empty;

                return string.Format(
                    "{0} | Rb={1:F1} MPa | Rbt={2:F2} MPa",
                    SelectedConcrete.Grade,
                    SelectedConcrete.Rb,
                    SelectedConcrete.Rbt);
            }
        }

        private ConcreteMaterialInput FindConcrete(
            string grade)
        {
            return ConcreteGrades.FirstOrDefault(
                x => string.Equals(
                    x.Grade,
                    grade,
                    StringComparison.OrdinalIgnoreCase));
        }

        public event PropertyChangedEventHandler
            PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName]
            string propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(
                    propertyName));
        }
    }
}