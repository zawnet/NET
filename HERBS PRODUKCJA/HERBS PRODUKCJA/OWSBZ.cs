//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HERBS_PRODUKCJA
{
    using System;
    using System.Collections.Generic;
    
    public partial class OWSBZ
    {
        public int id { get; set; }
        public int id_mg { get; set; }
        public string producent { get; set; }
        public string nawozenie1 { get; set; }
        public string nawozenie2 { get; set; }
        public string inne { get; set; }
    
        public virtual MG MG { get; set; }
    }
}
