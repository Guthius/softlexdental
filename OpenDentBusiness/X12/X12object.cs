/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using OpenDentBusiness.X12;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenDentBusiness
{
    /// <summary>
    /// Encapsulates one entire X12 Interchange object, including multiple functional groups and 
    /// transaction sets. It does not care what type of transactions are contained. It just 
    /// stores them. It does not inherit either. It is up to other classes to use this as needed.
    /// </summary>
    public class X12Object
    {
        /// <summary>
        /// External reference to the file corresponding to this X12 object.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The date and time this X12 object was created by the sender. 
        /// Relative to sender's time zone.
        /// </summary>
        public DateTime DateInterchange { get; }

        public X12Separators Separators { get; }

        /// <summary>
        /// A collection of X12FunctionalGroups. GS segments.
        /// </summary>
        public List<X12FunctionalGroup> FunctionalGroups { get; } = new List<X12FunctionalGroup>();

        /// <summary>
        /// All segments for the entiremessage.
        /// </summary>
        public List<X12Segment> Segments { get; } = new List<X12Segment>();

        public static bool IsX12(string messageText)
        {
            if (Parse(messageText) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Parses the specified message and returns the resulting <see cref="X12Object"/>.
        /// If the message is not valid X12 message, null will be returned instead.
        /// </summary>
        public static X12Object Parse(string messageText)
        {
            if (messageText == null || messageText.Length < 106) // Minimum length of 106, because the segment separator is at index 105.
                return null;

            if (messageText.Substring(0, 3) != "ISA")
                return null;

            try
            {
                // Denti-cal sends us 835s, but they also send us "EOB" reports which start with "ISA" and look similar to X12 but are NOT X12.

                return new X12Object(messageText); // Only an X12 object if we can parse it.  Denti-cal "EOB" reports fail this test, as they should.
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Object"/> class.
        /// </summary>
        protected X12Object()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Object"/> class, copying data from the 
        /// specified <see cref="X12Object"/> instance.
        /// </summary>
        public X12Object(X12Object other)
        {
            FilePath = other.FilePath;
            DateInterchange = other.DateInterchange;
            Separators = other.Separators;
            FunctionalGroups = other.FunctionalGroups;
            Segments = other.Segments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X12Object"/> class from the specified raw 
        /// message text.
        /// </summary>
        public X12Object(string messageText)
        {
            messageText = messageText.Replace("\r", "").Replace("\n", "");
            if (messageText.Substring(0, 3) != "ISA")
                throw new Exception("ISA not found");
            
            Separators = new X12Separators
            {
                Element = messageText.Substring(3, 1),
                Subelement = messageText.Substring(104, 1),
                Segment = messageText.Substring(105, 1)
            };

            var rawSegments = messageText.Split(new string[] { Separators.Segment }, StringSplitOptions.None);

            for (int i = 0; i < rawSegments.Length; i++)
            {
                var segment = new X12Segment(rawSegments[i], Separators)
                {
                    SegmentIndex = i
                };

                Segments.Add(segment);

                switch (segment.ID)
                {
                    case "ISA": // Interchange control header begin
                        {
                            try
                            {
                                DateInterchange = DateTime.ParseExact(segment.Get(9) + segment.Get(10), "yyMMddHHmm", CultureInfo.CurrentCulture.DateTimeFormat);
                            }
                            catch
                            {
                                DateInterchange = DateTime.MinValue;
                            }
                        }
                        break;

                    case "GS": // Functional Group Start
                        FunctionalGroups.Add(new X12FunctionalGroup(segment));
                        break;

                    case "ST": // Transaction Start
                        LastFunctionalGroup.Transactions.Add(new X12Transaction(segment));
                        break;

                    case "TA1":
                        // This segment can either replace or supplement any GS segments for any 
                        // ack type (997,999,277).  The TA1 will always be before the first GS 
                        // segment.
                        //
                        // Ignore for now.  We should eventually match TA101 with the ISA13 of the
                        // claim that we sent, so we can report the status to the user using fields
                        // TA104 and TA105. This segment is neither mandated or prohibited 
                        // (see 277.pdf pg. 207).
                        break;

                    case "":
                    case "IEA": // Interchange Control Header End
                    case "GE":  // Functional Group End
                    case "SE":  // Transaction End
                        break;

                    default: // Detail Segment
                        LastTransaction.Segments.Add(segment);
                        break;
                }
            }
        }

        /// <summary>
        /// Example of values returned: 
        /// 004010X097A1 (4010 dental), 
        /// 005010X222A1 (5010 medical), 
        /// 005010X223A2 (5010 institutional), 
        /// 005010X224A2 (5010 dental)
        /// </summary>
        private string GetFormat()
        {
            for (int i = 0; i < Segments.Count; i++)
            {
                if (Segments[i].ID == "GS")
                {
                    return Segments[i].Get(8);
                }
            }
            return "";
        }

        /// <summary>
        /// Returns true if the X12 object is in 4010 format.
        /// </summary>
        public bool IsFormat4010()
        {
            string format = GetFormat();
            if (format.Length >= 6)
            {
                return format.Substring(2, 4) == "4010";
            }
            return false;
        }

        /// <summary>
        /// Returns true if the X12 object is in 5010 format.
        /// </summary>
        public bool IsFormat5010()
        {
            string format = GetFormat();
            if (format.Length >= 6)
            {
                return format.Substring(2, 4) == "5010";
            }
            return false;
        }

        /// <summary>
        /// Returns true if there is a TA1 segment. The TA1 segment is neither mandated or 
        /// prohibited (see 277.pdf pg. 207). The Inmidiata clearinghouse likes to use TA1 segments
        /// to replace the usual acknowledgements (format ISA-TA1-IEA).
        /// </summary>
        public bool IsAckInterchange()
        {
            // If a GS is present, it will get handled elsewhere.
            // A TA1 can be used when there are no GS segments. That implies that it is an interchange ack.
            return
                !Segments.Exists(segment => segment.ID == "GS") && 
                 Segments.Exists(segment => segment.ID == "TA1");
        }

        public bool Is997()
        {
            return 
                FunctionalGroups.Count == 1 && 
                FunctionalGroups[0].Transactions[0].Header.Get(1) == "997";
        }

        public bool Is999()
        {
            return 
                FunctionalGroups.Count == 1 && 
                FunctionalGroups[0].Transactions[0].Header.Get(1) == "999";
        }

        public bool Is271()
        {
            return
                FunctionalGroups.Count == 1 &&
                FunctionalGroups[0].Transactions[0].Header.Get(1) == "271";
        }

        /// <summary>
        /// Gets the last functional group.
        /// </summary>
        private X12FunctionalGroup LastFunctionalGroup => 
            FunctionalGroups[FunctionalGroups.Count - 1];

        /// <summary>
        /// Gets the last transaction.
        /// </summary>
        private X12Transaction LastTransaction => 
            LastFunctionalGroup.Transactions[LastFunctionalGroup.Transactions.Count - 1];

        /// <summary>
        /// Gets the list of unique transaction set identifiers within the X12 object.
        /// </summary>
        public List<string> GetTranSetIds() =>
            FunctionalGroups[0].Transactions.Select(trx => trx.Header.Get(2)).ToList();

        /// <summary>
        /// The startIndex is zero-based. The segmentId is case sensitive.
        /// If arrayElement01Values is specified, then will only return segments where the 
        /// segment.Get(1) returns one of the specified values.
        /// Returns null if no segment is found.
        /// </summary>
        public X12Segment GetNextSegmentById(int startIndex, string segmentId, params string[] validElement01Values)
        {
            for (int i = startIndex; i < Segments.Count; i++)
            {
                var segment = Segments[i];
                if (segment.ID != segmentId)
                {
                    continue;
                }

                if (validElement01Values.Length > 0)
                {
                    var matchingElement01 = false;

                    foreach (var value in validElement01Values)
                    {
                        if (value == segment.Get(1))
                        {
                            matchingElement01 = true;
                            break;
                        }
                    }

                    if (!matchingElement01) continue;
                }

                return segment;
            }

            return null;
        }

        /// <summary>
        /// Returns the number of segments with the specified ID in this <see cref="X12Object"/>.
        /// </summary>
        /// <param name="segmentId">The segment ID.</param>
        /// <returns>The number of segments with the specified ID.</returns>
        public int GetSegmentCountById(string segmentId) => Segments.Count(segment => segment.ID == segmentId);

        /// <summary>
        /// Removes the specified segments and recreates the raw X12 from the remaining segments.
        /// Useful for modifying X12 reports which have been partially processed in order to keep 
        /// track of which parts have not been processed. Certain X12 documents also require the 
        /// segment count as part of the message (ex 5010 SE segment). This function does not 
        /// modify total segment count within the message.
        /// </summary>
        public string ReconstructRaw(List<int> indicesToRemove)
        {
            var indicesToKeep = new bool[Segments.Count];
            for (int i = 0; i < indicesToKeep.Length; i++)
            {
                indicesToKeep[i] = true;
            }

            foreach (var index in indicesToRemove)
            {
                if (index >= 0 && index < indicesToKeep.Length)
                {
                    indicesToKeep[index] = false;
                }
            }

            var stringBuilder = new StringBuilder();

            for (int i = 0; i < indicesToKeep.Length; i++)
            {
                if (indicesToKeep[i])
                {
                    stringBuilder.Append(Segments[i].Raw);
                    stringBuilder.Append(Separators.Segment);
                    stringBuilder.Append("\r\n");
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// "CODE SOURCE 237: Place of Service Codes for Professional Claims" is published by CMS
        /// (Centers for Medicare and Medicaid Services), according to 
        /// https://www.cms.gov/Medicare/Coding/place-of-service-codes/.
        /// 
        /// See code list here:
        /// https://www.cms.gov/Medicare/Medicare-Fee-for-Service-Payment/PhysicianFeeSched/Downloads/Website-POS-database.pdf
        /// </summary>
        public static string GetPlaceService(PlaceOfService place)
        {
            switch (place)
            {
                case PlaceOfService.AmbulatorySurgicalCenter:
                    return "24";
                case PlaceOfService.CustodialCareFacility:
                    return "33";
                case PlaceOfService.EmergencyRoomHospital:
                    return "23";
                case PlaceOfService.FederalHealthCenter:
                    return "50";
                case PlaceOfService.InpatHospital:
                    return "21";
                case PlaceOfService.MilitaryTreatFac:
                    return "26";
                case PlaceOfService.MobileUnit:
                    return "15";
                case PlaceOfService.Office:
                case PlaceOfService.OtherLocation:
                    return "11";
                case PlaceOfService.OutpatHospital:
                    return "22";
                case PlaceOfService.PatientsHome:
                    return "12";
                case PlaceOfService.PublicHealthClinic:
                    return "71";
                case PlaceOfService.RuralHealthClinic:
                    return "72";
                case PlaceOfService.School:
                    return "03";
                case PlaceOfService.SkilledNursFac:
                    return "31";
                case PlaceOfService.Telehealth:
                    return "02";
            }
            return "11";
        }
    }


    #region Segments

    public class X12_ACT : X12Segment
    {
        ///<summary>ACT01</summary>
        public string AccountNumber1;
        ///<summary>ACT06</summary>
        public string AccountNumber2;

        public X12_ACT(X12Segment seg) : base(seg)
        {
            seg.AssertType("ACT");
            AccountNumber1 = seg.Get(1);
            AccountNumber2 = seg.Get(6);
        }
    }

    public class X12_AMT : X12Segment
    {
        ///<summary>AMT01</summary>
        public string AmountQualifierCode;
        ///<summary>AMT02</summary>
        public string MonetaryAmount;

        public X12_AMT(X12Segment seg) : base(seg)
        {
            seg.AssertType("AMT");
            AmountQualifierCode = seg.Get(1);
            MonetaryAmount = seg.Get(2);
        }
    }

    public class X12_BGN : X12Segment
    {
        ///<summary>BGN01</summary>
        public string TransactionSetPurposeCode;
        ///<summary>BGN02</summary>
        public string ReferenceIdentification1;
        ///<summary>BGN03</summary>
        public string DateBgn;
        ///<summary>BGN04</summary>
        public string TimeBgn;
        ///<summary>BGN05</summary>
        public string TimeCode;
        ///<summary>BGN06</summary>
        public string ReferenceIdentifcation2;
        ///<summary>BGN08</summary>
        public string ActionCode;

        public X12_BGN(X12Segment seg) : base(seg)
        {
            seg.AssertType("BGN");
            TransactionSetPurposeCode = seg.Get(1);
            ReferenceIdentification1 = seg.Get(2);
            DateBgn = seg.Get(3);
            TimeBgn = seg.Get(4);
            TimeCode = seg.Get(5);
            ReferenceIdentifcation2 = seg.Get(6);
            ActionCode = seg.Get(8);
        }
    }

    public class X12_COB : X12Segment
    {
        ///<summary>COB01</summary>
        public string PayerResponsibilitySequenceNumberCode;
        ///<summary>COB02</summary>
        public string ReferenceIdentification;
        ///<summary>COB03</summary>
        public string CoordinationOfBenefitsCode;
        ///<summary>COB04</summary>
        public string ServiceTypeCode;

        public X12_COB(X12Segment seg) : base(seg)
        {
            seg.AssertType("COB");
            PayerResponsibilitySequenceNumberCode = seg.Get(1);
            ReferenceIdentification = seg.Get(2);
            CoordinationOfBenefitsCode = seg.Get(3);
            ServiceTypeCode = seg.Get(4);
        }
    }

    public class X12_DMG : X12Segment
    {
        ///<summary>DMG01</summary>
        public string DateTimePeriodFormatQualifier;
        ///<summary>DMG02</summary>
        public string DateTimePeriod;
        ///<summary>DMG03</summary>
        public string GenderCode;
        ///<summary>DMG04</summary>
        public string MaritalStatusCode;
        ///<summary>DMG05</summary>
        public string CompositeRaceOrEthnicityInformation;
        ///<summary>DMG06</summary>
        public string CitizenshipStatusCode;
        ///<summary>DMG10</summary>
        public string CodeListQualifierCode;
        ///<summary>DMG11</summary>
        public string IndustryCode;

        public X12_DMG(X12Segment seg) : base(seg)
        {
            seg.AssertType("DMG");
            DateTimePeriodFormatQualifier = seg.Get(1);
            DateTimePeriod = seg.Get(2);
            GenderCode = seg.Get(3);
            MaritalStatusCode = seg.Get(4);
            CompositeRaceOrEthnicityInformation = seg.Get(5);
            CitizenshipStatusCode = seg.Get(6);
            CodeListQualifierCode = seg.Get(10);
            IndustryCode = seg.Get(11);
        }
    }

    public class X12_DSB : X12Segment
    {
        ///<summary>DSP01</summary>
        public string DisabilityTypeCode;
        ///<summary>DSP07</summary>
        public string ProductServiceIdQualifier;
        ///<summary>DSP08</summary>
        public string MedicalCodeValue;

        public X12_DSB(X12Segment seg) : base(seg)
        {
            seg.AssertType("DSB");
            DisabilityTypeCode = seg.Get(1);
            ProductServiceIdQualifier = seg.Get(7);
            MedicalCodeValue = seg.Get(8);
        }
    }

    public class X12_DTP : X12Segment
    {
        ///<summary>DTP01</summary>
        public string DateTimeQualifier;
        ///<summary>DTP02</summary>
        public string DateTimePeriodFormatQualifier;
        ///<summary>DTP03</summary>
        public string DateTimePeriod;

        public X12_DTP(X12Segment seg) : base(seg)
        {
            seg.AssertType("DTP");
            DateTimeQualifier = seg.Get(1);
            DateTimePeriodFormatQualifier = seg.Get(2);
            DateTimePeriod = seg.Get(3);
        }

        public DateTime DateT()
        {
            if (DateTimePeriodFormatQualifier == "D8")
            {
                return X12Parse.ToDate(DateTimePeriod);
            }
            return DateTime.MinValue;
        }
    }

    public class X12_EC : X12Segment
    {
        ///<summary>EC01</summary>
        public string EmploymentClassCode1;
        ///<summary>EC02</summary>
        public string EmploymentClassCode2;
        ///<summary>EC03</summary>
        public string EmploymentClassCode3;

        public X12_EC(X12Segment seg) : base(seg)
        {
            seg.AssertType("EC");
            EmploymentClassCode1 = seg.Get(1);
            EmploymentClassCode2 = seg.Get(2);
            EmploymentClassCode3 = seg.Get(3);
        }
    }

    public class X12_HD : X12Segment
    {
        ///<summary>HD01</summary>
        public string MaintenanceTypeCode;
        ///<summary>HD03</summary>
        public string InsuranceLineCode;
        ///<summary>HD04</summary>
        public string PlanCoverageDescription;
        ///<summary>HD05</summary>
        public string CoverageLevelCode;
        ///<summary>HD09</summary>
        public string YesNoConditionOrResponseCode;

        public X12_HD(X12Segment seg) : base(seg)
        {
            seg.AssertType("HD");
            MaintenanceTypeCode = seg.Get(1);
            InsuranceLineCode = seg.Get(3);
            PlanCoverageDescription = seg.Get(4);
            CoverageLevelCode = seg.Get(5);
            YesNoConditionOrResponseCode = seg.Get(9);
        }
    }

    public class X12_HLH : X12Segment
    {
        ///<summary>HLH01</summary>
        public string HealthRelatedCode;
        ///<summary>HLH02</summary>
        public string Height;
        ///<summary>HLH03</summary>
        public string Weight;

        public X12_HLH(X12Segment seg) : base(seg)
        {
            seg.AssertType("HLH");
            HealthRelatedCode = seg.Get(1);
            Height = seg.Get(2);
            Weight = seg.Get(3);
        }
    }

    public class X12_ICM : X12Segment
    {
        ///<summary>ICM01</summary>
        public string FrequencyCode;
        ///<summary>ICM02</summary>
        public string MonetaryAmount;
        ///<summary>ICM03</summary>
        public string Quantity;
        ///<summary>ICM04</summary>
        public string LocationIdentifier;
        ///<summary>ICM05</summary>
        public string SalaryGrade;

        public X12_ICM(X12Segment seg) : base(seg)
        {
            seg.AssertType("ICM");
            FrequencyCode = seg.Get(1);
            MonetaryAmount = seg.Get(2);
            Quantity = seg.Get(3);
            LocationIdentifier = seg.Get(4);
            SalaryGrade = seg.Get(5);
        }
    }

    public class X12_IDC : X12Segment
    {
        ///<summary>IDC01</summary>
        public string PlanCoverageDescription;
        ///<summary>IDC02</summary>
        public string IdentificationCardTypeCode;
        ///<summary>IDC03</summary>
        public string Quantity;
        ///<summary>IDC04</summary>
        public string ActionCode;

        public X12_IDC(X12Segment seg) : base(seg)
        {
            seg.AssertType("IDC");
            PlanCoverageDescription = seg.Get(1);
            IdentificationCardTypeCode = seg.Get(2);
            Quantity = seg.Get(3);
            ActionCode = seg.Get(4);
        }
    }

    public class X12_INS : X12Segment
    {
        ///<summary>INS01</summary>
        public string YesNoConditionOrResponseCode1;
        ///<summary>INS02</summary>
        public string IndividualRelationshipCode;
        ///<summary>INS03</summary>
        public string MaintenanceTypeCode;
        ///<summary>INS04</summary>
        public string MaintenanceReasonCode;
        ///<summary>INS05</summary>
        public string BenefitStatusCode;
        ///<summary>INS06</summary>
        public string MedicareStatusCode;
        ///<summary>INS07</summary>
        public string CobraQualifying;
        ///<summary>INS08</summary>
        public string EmploymentStatusCode;
        ///<summary>INS09</summary>
        public string StudentStatusCode;
        ///<summary>INS10</summary>
        public string YesNoConditionOrResponseCode2;
        ///<summary>INS11</summary>
        public string DateTimePeriodFormatQualifier;
        ///<summary>INS12</summary>
        public string DateOfDeath;
        ///<summary>INS13</summary>
        public string ConfidentialityCode;
        ///<summary>INS17</summary>
        public string Number;

        public X12_INS(X12Segment seg) : base(seg)
        {
            seg.AssertType("INS");
            YesNoConditionOrResponseCode1 = seg.Get(1);
            IndividualRelationshipCode = seg.Get(2);
            MaintenanceTypeCode = seg.Get(3);
            MaintenanceReasonCode = seg.Get(4);
            BenefitStatusCode = seg.Get(5);
            MedicareStatusCode = seg.Get(6);
            CobraQualifying = seg.Get(7);
            EmploymentStatusCode = seg.Get(8);
            StudentStatusCode = seg.Get(9);
            YesNoConditionOrResponseCode2 = seg.Get(10);
            DateTimePeriodFormatQualifier = seg.Get(11);
            DateOfDeath = seg.Get(12);
            ConfidentialityCode = seg.Get(13);
            Number = seg.Get(17);
        }
    }

    public class X12_LE : X12Segment
    {
        ///<summary>LE01</summary>
        public string LoopIdentifierCode;

        public X12_LE(X12Segment seg) : base(seg)
        {
            seg.AssertType("LE");
            LoopIdentifierCode = seg.Get(1);
        }
    }

    public class X12_LS : X12Segment
    {
        ///<summary>LS01</summary>
        public string LoopIdentifierCode;

        public X12_LS(X12Segment seg) : base(seg)
        {
            seg.AssertType("LS");
            LoopIdentifierCode = seg.Get(1);
        }
    }

    public class X12_LUI : X12Segment
    {
        ///<summary>LUI01</summary>
        public string IdentificationCodeQualifier;
        ///<summary>LUI02</summary>
        public string IdentificationCode;
        ///<summary>LUI03</summary>
        public string Description;
        ///<summary>LUI04</summary>
        public string UseOfLanguageIndicator;

        public X12_LUI(X12Segment seg) : base(seg)
        {
            seg.AssertType("LUI");
            IdentificationCodeQualifier = seg.Get(1);
            IdentificationCode = seg.Get(2);
            Description = seg.Get(3);
            UseOfLanguageIndicator = seg.Get(4);
        }
    }

    public class X12_LX : X12Segment
    {
        ///<summary>LX01</summary>
        public string AssignedNumber;

        public X12_LX(X12Segment seg) : base(seg)
        {
            seg.AssertType("LX");
            AssignedNumber = seg.Get(1);
        }
    }

    public class X12_N1 : X12Segment
    {
        ///<summary>N101</summary>
        public string EntityIdentifierCode;
        ///<summary>N102</summary>
        public string Name;
        ///<summary>N103</summary>
        public string IdentificationCodeQualifier;
        ///<summary>N104</summary>
        public string IdentificationCode;

        public X12_N1(X12Segment seg, params string[] arrayElement01Values) : base(seg)
        {
            seg.AssertType("N1", arrayElement01Values);
            EntityIdentifierCode = seg.Get(1);
            Name = seg.Get(2);
            IdentificationCodeQualifier = seg.Get(3);
            IdentificationCode = seg.Get(4);
        }
    }

    public class X12_N3 : X12Segment
    {
        ///<summary>N301</summary>
        public string AddressInformation1;
        ///<summary>N302</summary>
        public string AddressInformation2;

        public X12_N3(X12Segment seg) : base(seg)
        {
            seg.AssertType("N3");
            AddressInformation1 = seg.Get(1);
            AddressInformation2 = seg.Get(2);
        }
    }

    public class X12_N4 : X12Segment
    {
        ///<summary>N401</summary>
        public string CityName;
        ///<summary>N402</summary>
        public string StateOrProvinceCode;
        ///<summary>N403</summary>
        public string PostalCode;
        ///<summary>N404</summary>
        public string CountryCode;
        ///<summary>N405</summary>
        public string LocationQualifier;
        ///<summary>N406</summary>
        public string LocationIdentifier;
        ///<summary>N407</summary>
        public string CountrySubdivisionCode;

        public X12_N4(X12Segment seg) : base(seg)
        {
            seg.AssertType("N4");
            CityName = seg.Get(1);
            StateOrProvinceCode = seg.Get(2);
            PostalCode = seg.Get(3);
            CountryCode = seg.Get(4);
            LocationQualifier = seg.Get(5);
            LocationIdentifier = seg.Get(6);
            CountrySubdivisionCode = seg.Get(7);
        }
    }

    public class X12_NM1 : X12Segment
    {
        ///<summary>NM101</summary>
        public string EntityIdentifierCode;
        ///<summary>NM102</summary>
        public string EntityTypeQualifier;
        ///<summary>NM103</summary>
        public string NameLast;
        ///<summary>NM104</summary>
        public string NameFirst;
        ///<summary>NM105</summary>
        public string NameMiddle;
        ///<summary>NM106</summary>
        public string NamePrefix;
        ///<summary>NM107</summary>
        public string NameSuffix;
        ///<summary>NM108</summary>
        public string IdentificationCodeQualifier;
        ///<summary>NM109</summary>
        public string IdentificationCode;

        public X12_NM1(X12Segment seg, params string[] arrayElement01Values) : base(seg)
        {
            seg.AssertType("NM1", arrayElement01Values);
            EntityIdentifierCode = seg.Get(1);
            EntityTypeQualifier = seg.Get(2);
            NameLast = seg.Get(3);
            NameFirst = seg.Get(4);
            NameMiddle = seg.Get(5);
            NamePrefix = seg.Get(6);
            NameSuffix = seg.Get(7);
            IdentificationCodeQualifier = seg.Get(8);
            IdentificationCode = seg.Get(9);
        }
    }

    public class X12_PER : X12Segment
    {
        ///<summary>PER01</summary>
        public string ContactFunctionCode;
        ///<summary>PER03</summary>
        public string CommunicationNumberQualifier1;
        ///<summary>PER04</summary>
        public string CommunicationNumber1;
        ///<summary>PER05</summary>
        public string CommunicationNumberQualifier2;
        ///<summary>PER06</summary>
        public string CommunicationNumber2;
        ///<summary>PER07</summary>
        public string CommunicationNumberQualifier3;
        ///<summary>PER08</summary>
        public string CommunicationNumber3;

        public X12_PER(X12Segment seg, params string[] arrayElement01Values) : base(seg)
        {
            seg.AssertType("PER", arrayElement01Values);
            ContactFunctionCode = seg.Get(1);
            CommunicationNumberQualifier1 = seg.Get(3);
            CommunicationNumber1 = seg.Get(4);
            CommunicationNumberQualifier2 = seg.Get(5);
            CommunicationNumber2 = seg.Get(6);
            CommunicationNumberQualifier3 = seg.Get(7);
            CommunicationNumber3 = seg.Get(8);
        }
    }

    public class X12_PLA : X12Segment
    {
        ///<summary>PLA01</summary>
        public string ActionCode;
        ///<summary>PLA02</summary>
        public string EntityIdentifierCode;
        ///<summary>PLA03</summary>
        public string DatePla;
        ///<summary>PLA04</summary>
        public string TimePla;
        ///<summary>PLA05</summary>
        public string MaintenanceReasonCode;

        public X12_PLA(X12Segment seg) : base(seg)
        {
            seg.AssertType("PLA");
            ActionCode = seg.Get(1);
            EntityIdentifierCode = seg.Get(2);
            DatePla = seg.Get(3);
            TimePla = seg.Get(4);
            MaintenanceReasonCode = seg.Get(5);
        }
    }

    public class X12_REF : X12Segment
    {
        ///<summary>REF01</summary>
        public string ReferenceIdQualifier;
        ///<summary>REF02</summary>
        public string ReferenceId;

        public X12_REF(X12Segment seg, params string[] arrayElement01Values) : base(seg)
        {
            seg.AssertType("REF", arrayElement01Values);
            ReferenceIdQualifier = seg.Get(1);
            ReferenceId = seg.Get(2);
        }
    }

    public class X12_QTY : X12Segment
    {
        ///<summary>QTY01</summary>
        public string QuantityQualifier;
        ///<summary>QTY02</summary>
        public string Quantity;

        public X12_QTY(X12Segment seg) : base(seg)
        {
            seg.AssertType("QTY");
            QuantityQualifier = seg.Get(1);
            Quantity = seg.Get(2);
        }

    }

    #endregion Segments



    ///<summary></summary>
    public enum PlaceOfService
    {
        ///<summary>0. Code 11</summary>
        Office,
        ///<summary>1. Code 12</summary>
        PatientsHome,
        ///<summary>2. Code 21</summary>
        InpatHospital,
        ///<summary>3. Code 22</summary>
        OutpatHospital,
        ///<summary>4. Code 31</summary>
        SkilledNursFac,
        ///<summary>5. Code 33.  In X12, a similar code AdultLivCareFac 35 is mentioned.</summary>
        CustodialCareFacility,
        ///<summary>6. Code 99.  We use 11 for office.</summary>
        OtherLocation,
        ///<summary>7. Code 15</summary>
        MobileUnit,
        ///<summary>8. Code 03</summary>
        School,
        ///<summary>9. Code 26</summary>
        MilitaryTreatFac,
        ///<summary>10. Code 50</summary>
        FederalHealthCenter,
        ///<summary>11. Code 71</summary>
        PublicHealthClinic,
        ///<summary>12. Code 72</summary>
        RuralHealthClinic,
        ///<summary>13. Code 23</summary>
        EmergencyRoomHospital,
        ///<summary>14. Code 24</summary>
        AmbulatorySurgicalCenter,
        ///<summary>15. Code 02.</summary>
        Telehealth,
    }
}