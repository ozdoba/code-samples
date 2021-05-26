using System;
using AutoMapper;
using Payroll.Application.Common.Mappings;
using Payroll.Domain.Employees;

namespace Payroll.Application.Employees.Queries.ListIdDocuments
{
    public class IdDocumentType : IMapFrom<IdDocument>
    {
        public CategoryType IdType { get; set; }
        /// <summary>
        /// The number of the id document
        /// </summary>
        public string DocumentNumber { get; set; }
        /// <summary>
        /// The issuing authority of the document
        /// </summary>
        public string IssuedBy { get; set; }
        /// <summary>
        /// Where the document was issued
        /// </summary>
        public string IssuedAt { get; set; }
        /// <summary>
        /// When the document was issued
        /// </summary>
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// When does the document expire
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IdDocument, IdDocumentType>()
                .ForMember(
                    dest => dest.IdType,
                    opt => opt.MapFrom(
                        src => Enum.Parse<CategoryType>(src.IdType)));
        }
    }
}