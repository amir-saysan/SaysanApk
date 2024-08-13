namespace SaysanPwa.Application.DTOs.Factor
{
    public record GetSaleFactorDto
    {
        public long ID_tbl_TarafHesab { get; set; }
        public long ID_tbl_Partner_Branch { get; set; }
        public long ID_tbl_Bzy { get; set; }
        public decimal Tkh_N_F { get; set; }
        public decimal Tkh_A_F { get; set; }
        public decimal JM_F { get; set; }
        public decimal JAv_F { get; set; }
        public decimal J_F { get; set; }
        public decimal BG_F { get; set; } = 0;
        public decimal J_Tedad_Asl_F { get; set; }
        public decimal J_Tedad_Fare_F { get; set; }
        public string Dc_F { get; set; } = "";
        public decimal Prsnt_Bzy { get; set; } = 0;
        public decimal Pdsh_Bzy { get; set; } = 0;
        public string Dt_F { get; set; }
        public string Name_TarafHesab { get; set; }
        public string ChelPhone_TarafHesab { get; set; }
        public string Dt_C_F { get; set; } //Dt_F;
        public string Tm_C_F { get; set; }
        public long ID_tbl_S1 { get; set; } = 0;
        public int ID_tbl_SalMaly { get; set; }
        public int ID_tbl_Users { get; set; }


        public long N_F { get; set; }
        public long ID_tbl_FF { get; set; }
        public decimal J_Hz_F { get; set; } = 0;



        public string Code_TarafHesab { get; set; }//کد مشتری
        public string Name_Bzy { get; set; }//نام بازاریاب
        public string Name_Kala { get; set; }//نام کالا
        public string BarCode_Kala { get; set; }//بارکد کالا
        public long Code_Kala { get; set; }//کد کالا
        public string Shenase_Kala { get; set; }//شناسه مالیاتی کالا
        public string Vahede_Name { get; set; }//واحد کالا
        public string Name_Anbar { get; set; }//نام انبار
        public string Code_Anbar { get; set; }//کد انبار
        public decimal Tedad { get; set; }//تعداد
        public decimal Fi { get; set; }//فی
        public decimal Mablagh { get; set; }//مبلغ کل
        public string Name_Branch { get; set; }//نام شعبه


        // ------------- For set fiscal year information to sale factor.

        public string FiscalYearBeginDate { get; set; } = string.Empty;
        public string FiscalYearEndDate { get; set; } = string.Empty;
        public string FiscalYearTitle { get; set; } = string.Empty;

        public IEnumerable<GetSaleFactorDto> Items { get; set; } = new List<GetSaleFactorDto>();
    }

}
