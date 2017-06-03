using EduSpot.Entity.Tables.AngkutJual;
using EduSpot.Entity.Tables.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esdm.Web.Areas.AngkutJual.Models
{
    public class IupOpAngkutJualListViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string NPWP { get; set; }
        public string SkNumber { get; set; }
        public string StatusIzin { get; set; }
        public string LetterNumber { get; set; }
        public string AdditionalInformation { get; set; }
        public string WarningDuration { get; set; }
        public string WarningType { get; set; }
        public string NoHp { get; set; }
        public string Email { get; set; }

    }
    public class IupOpAngkutJualListTab2ViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string SkNumber { get; set; }
        public Nullable<DateTime> SkDate { get; set; }
        public Nullable<DateTime> SkEndDate { get; set; }
        public string SkDuration { get; set; }
    }
    public class IupOpAngkutJualListBKPM
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string SkNumber { get; set; }
        public string LetterNumber { get; set; }
        public Nullable<DateTime> LetterDate { get; set; }
        public Nullable<DateTime> BKPMAcceptanceDate { get; set; }
        public Nullable<DateTime> EvaluatorAcceptanceDate { get; set; }
        public string LetterType { get; set; }
        public string Status { get; set; }
        public string CompanyID { get; set; }
        public string AdditionalInformation { get; set; }
    }

    public class IupOpAngkutJualListDetailPeringatan
    {
        public string ID { get; set; }
        public string CompanyID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string SkNumber { get; set; }
        public string SkDate { get; set; }
        public Nullable<DateTime> SkDate2 { get; set; }
        public string WarningDuration { get; set; }

        public string Email { get; set; }
        public string NoHp { get; set; }

        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Annual { get; set; }
        public string Rkab { get; set; }

    }
}