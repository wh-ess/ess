using System;

namespace ESS.Domain.Common.Contact.Domain
{
    public class GeographicBoundary
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        //缩写
        public string Abbreviation { get; set; }
        public string InternetRegionCode { get; set; }
    }

    public class PostalCode : GeographicBoundary
    {
        
    }
    /// <summary>
    ///  郡，县
    /// </summary>
    public class County : GeographicBoundary
    {

    }
    

    public class Province : GeographicBoundary
    {
        
    }

    public class City : GeographicBoundary
    {
        
    }

    /// <summary>
    /// 区域
    /// </summary>
    public class Territory : GeographicBoundary
    {
        
    }

    /// <summary>
    /// 县
    /// </summary>
    public class Prefecture : GeographicBoundary
    {
        
    }

    

    public class State : GeographicBoundary
    {
        
    }

    /// <summary>
    /// 行政区，州
    /// </summary>
    public class Canton : GeographicBoundary
    {
        
    }

    public class Region : GeographicBoundary
    {
        
    }

    public class SubDivision : GeographicBoundary
    {
        
    }

    /// <summary>
    /// 大陆，洲，陆地
    /// </summary>
    public class Continent : GeographicBoundary
    {
        
    }
    public class Country : GeographicBoundary
    {

    }
}

