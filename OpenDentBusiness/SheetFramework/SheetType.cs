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

namespace OpenDentBusiness
{
    /// <Summary>
    /// Different types of sheets that can be used.
    /// </Summary>
    public enum SheetTypeEnum
    {
        ///<Summary>0-Requires SheetParameter for PatNum. Does not get saved to db.</Summary>
        LabelPatient,

        ///<Summary>1-Requires SheetParameter for CarrierNum. Does not get saved to db.</Summary>
        LabelCarrier,

        ///<Summary>2-Requires SheetParameter for ReferralNum. Does not get saved to db.</Summary>
        LabelReferral,

        ///<Summary>3-Requires SheetParameters for PatNum,ReferralNum.</Summary>
        ReferralSlip,

        ///<Summary>4-Requires SheetParameter for AptNum. Does not get saved to db.</Summary>
        LabelAppointment,

        ///<Summary>5-Requires SheetParameter for RxNum.</Summary>
        Rx,

        ///<summary>6-Requires SheetParameter for PatNum.</summary>
        Consent,

        ///<summary>7-Requires SheetParameter for PatNum.</summary>
        PatientLetter,

        ///<summary>8-Requires SheetParameters for PatNum,ReferralNum.</summary>
        ReferralLetter,

        ///<Summary>9-Requires SheetParameter for PatNum.</Summary>
        PatientForm,

        ///<Summary>10-Requires SheetParameter for AptNum.  Does not get saved to db.</Summary>
        RoutingSlip,

        ///<Summary>11-Requires SheetParameter for PatNum.</Summary>
        MedicalHistory,

        ///<Summary>12-Requires SheetParameter for PatNum, LabCaseNum.</Summary>
        LabSlip,

        ///<Summary>13-Requires SheetParameter for PatNum.</Summary>
        ExamSheet,

        ///<summary>14-Requires SheetParameter for DepositNum.</summary>
        DepositSlip,

        ///<summary>15-Requires SheetParameter for PatNum.</summary>
        Statement,

        ///<summary>16-Requires SheetParameters for PatNum,MedLab,MedLabResult.</summary>
        MedLabResults,

        ///<summary>17-Requires SheetParameters for PatNum,TreatmentPlan.</summary>
        TreatmentPlan,

        ///<summary>18-Requires SheetParameter for ScreenNum.  
        ///Optional SheetParameter for PatNum if screening is associated to a patient.</summary>
        Screening,

        ///<summary>19-Used for Payment Plans to Sheets.</summary>
        PaymentPlan,

        ///<summary>20-Requires SheetParameters for ListRxSheet and ListRxNums.</summary>
        RxMulti,

        ///<summary>21</summary>
        ERA,

        ///<summary>22</summary>
        ERAGridHeader,

        ///<summary>23</summary>
        RxInstruction,

        ///<summary>24-Defines the layout of a patient specific dashboard sheet.  Not directly user editable.  Each sheetfielddef linked to this sheet 
        ///type further links a PatientDashboardWidget type sheet to this PatientDashboard sheet, allowing users to place various 
        ///PatientDashboardWidgets on their personal PatientDashboard.</summary>
        PatientDashboard,

        ///<summary>25-Defines the layout and elements of a reusable dashboard element that can be placed on a PatientDashboard.  Editable from 
        ///Dashboard Setup with Setup permissions.</summary>
        PatientDashboardWidget,

        ///<summary>26</summary>
        [SheetLayout(true, SheetFieldLayoutMode.TreatPlan, SheetFieldLayoutMode.Ecw, SheetFieldLayoutMode.EcwTreatPlan, SheetFieldLayoutMode.Orion, SheetFieldLayoutMode.OrionTreatPlan, SheetFieldLayoutMode.MedicalPractice, SheetFieldLayoutMode.MedicalPracticeTreatPlan)]
        ChartModule,
    }
}
