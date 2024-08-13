namespace SaysanPwa.Application.DTOs.Factor
{
    public record GetServiceSaleFactorDto
    {
        public string Type_tbl_F { get; set; } = "tbl_FF_KHed";
        public string Name_TarafHesab { get; set; }
        public long N_FF_KHed { get; set; }
        public long ID_tbl_TarafHesab_Moj { get; set; } = 0;
        public decimal Pors_Mbg { get; set; } = 0;
        public bool rdo1 { get; set; } = false;
        public bool rdo2 { get; set; } = true;
        public decimal Tkh_N_FF_KHed { get; set; }
        public decimal Tkh_K_FF_KHed { get; set; } = 0;
        public decimal Tkh_A_FF_KHed { get; set; }
        public decimal JM_FF_KHed { get; set; }
        public decimal JAv_FF_KHed { get; set; }
        public decimal J_Hz_FF_KHed { get; set; } = 0;
        public decimal J_FF_KHed { get; set; }
        public decimal BG_FF_KHed { get; set; }
        public long J_Tedad_Asli_FF_KHed { get; set; }
        public long J_Tedad_Farei_FF_KHed { get; set; } = 0;
        public string Dc_FF_KHed { get; set; }
        public string Dt_FF_KHed { get; set; }
        public string Dt_C_FF_KHed { get; set; }
        public string Tm_C_FF_KHed { get; set; }
        public bool Conred_FF_KHed { get; set; } = false;
        public long ID_tbl_DA { get; set; } = 0;
        public long ID_tbl_S1 { get; set; }
        public string IS_PR { get; set; } = "چاپ نشده";
        public int ID_tbl_SalMaly { get; set; }
        public int ID_tbl_Users { get; set; }
        public string Dt_Up { get; set; } = "";
        public string Tm_Up { get; set; } = "";
        public int ID_tbl_Users_Up { get; set; } = 0;



        public long ID_tbl_FF_KHed { get; set; }
        public decimal JM_F { get; set; }//مالیات
        public string Code_TarafHesab { get; set; }//کد مشتری
        public string ChelPhone_TarafHesab { get; set; }//تلفن مشتری
        public string Name_TarafHesab_Moj_Rdf { get; set; }//نام مجری
        public string Code_TarafHesab_Moj_Rdf { get; set; }//کد مجری
        public string Name_Bzy { get; set; }//نام بازاریاب
        public string Name_Khedmat { get; set; }//نام خدمت
        public long Cde_Khedmat { get; set; }//کد خدمت
        public string Shenase_Khedmat { get; set; }//شناسه مالیاتی خدمت
        public decimal Tedad { get; set; }//تعداد
        public decimal Fi { get; set; }//فی
        public decimal Mablagh { get; set; }//مبلغ کل
        public string Name_Branch { get; set; }//نام شعبه

        public long Code_Kala { get; set; }//کد کالا
        public string Shenase_Kala { get; set; }//شناسه مالیاتی کالا
        public string Vahede_Name { get; set; }//واحد کالا
        public string Name_Anbar { get; set; }//نام انبار
        public string Code_Anbar { get; set; }//کد انبار
       
        public string Name_Kala { get; set; }//نام کالا
        public string BarCode_Kala { get; set; }//بارکد کالا


       



        // ------------- For init fiscal year information to sale factor.

        public string FiscalYearBeginDate { get; set; } = string.Empty;
        public string FiscalYearEndDate { get; set; } = string.Empty;
        public string FiscalYearTitle { get; set; } = string.Empty;

        public long ID_tbl_TarafHesab { get; set; }

        public IEnumerable<AddSaleServiceFactorItemDto> Items { get; set; } = new List<AddSaleServiceFactorItemDto>();
    }
}
