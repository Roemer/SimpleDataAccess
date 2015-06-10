using System;

namespace SimpleDataAccess.Mapping
{
    public class MappingField
    {
        /// <summary>
        /// Index of the field in the entity
        /// </summary>
        public int FieldIndex { get; set; }
        /// <summary>
        /// Database name of the field
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Database type of the field
        /// </summary>
        public FieldType FieldType { get; set; }
        /// <summary>
        /// Index in the primary key of this field
        /// </summary>
        public int PrimaryIndex { get; set; }
        /// <summary>
        /// Flag to indicate if this field is an identity
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// Flag to indicate if this field is nullable
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// The length of the field (if any)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Simple check if this field is part (or equals) the primary key
        /// </summary>
        public bool IsPrimary { get { return PrimaryIndex >= 0; } }

        public MappingField Primary(bool isPrimary)
        {
            PrimaryIndex = isPrimary ? 0 : -1;
            return this;
        }

        public MappingField Identity(bool isIdentity)
        {
            IsIdentity = isIdentity;
            return this;
        }

        public MappingField Nullable(bool isNullable)
        {
            IsNullable = isNullable;
            return this;
        }

        public MappingField SetLength(int length)
        {
            Length = length;
            return this;
        }

        public override string ToString()
        {
            return String.Format("{0} [{1}]", FieldName, FieldType);
        }
    }
}
