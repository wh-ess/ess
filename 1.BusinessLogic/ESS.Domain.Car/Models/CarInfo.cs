using System;

namespace ESS.Domain.Car.Models
{
    public class CarInfo
    {
        public int Id { get; set; }
        public string No { get; set; }
        public string PlateNo { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string EngineNo { get; set; }
        public string BaseNo { get; set; }
        public string OwnerName { get; set; }
        public int OwenerSex { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime OperateStartDate { get; set; }
        public DateTime OperateNextDate { get; set; }
        public string OperatNo { get; set; }
        public string SurchargeNo { get; set; }
        public DateTime AnnualVerificationDate { get; set; }
        public DateTime NextAnnualVerificationDate { get; set; }
        public DateTime InsurenceDate { get; set; }
        public DateTime NextInsurenceDate { get; set; }
        public int InsurenceDuration { get; set; }
        public string TaximeterNo { get; set; }
        public string CarPic { get; set; }
        public string OwnerPic { get; set; }
        public string SafeNote { get; set; }
        public string ViolationNote { get; set; }
        public string Note { get; set; }
    }
}