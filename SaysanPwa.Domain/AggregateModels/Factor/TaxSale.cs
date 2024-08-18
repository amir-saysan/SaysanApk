using SaysanPwa.Domain.SeedWorker;

namespace SaysanPwa.Domain.AggregateModels.Factor;

public class TaxSale : Entity, IAggregateRoot
{
	//---------------------------------فاکتور فروش----------------------------
	public long ID_tbl_FF { get; set; }//آیدی فاکتور
	public long N_F { get; set; }//شماره فاکتور
	public long ID_tbl_TarafHesab { get; set; }
	public long ID_tbl_Bzy { get; set; }
	public decimal Tkh_N_F { get; set; }
	public decimal Tkh_A_F { get; set; }//جمع تخفیفات
	public decimal JM_F { get; set; }//جمع مالیات
	public decimal JAv_F { get; set; }
	public decimal J_Hz_F { get; set; }//جمع هزینه
	public decimal J_F { get; set; }//جمع فاکتور
	public decimal BG_F { get; set; }//باقیمانده فاکتور
	public decimal J_Tedad_Asl_F { get; set; }//جمع تعداد اصلی
	public decimal J_Tedad_Fare_F { get; set; }
	public string Dc_F { get; set; }//شرح
	public string Dt_F { get; set; }//تاریخ
	public string Dt_C_F { get; set; }
	public string Tm_C_F { get; set; }
	public long ID_tbl_DA { get; set; }//شماره دریافت
	public long ID_tbl_S1 { get; set; }


	//---------------------------------فاکتور فروش----------------------------
	public string Name_TarafHesab { get; set; }//نام طرفحساب
	public string Name_Bzy { get; set; }//نام بازاریاب
	public string Username { get; set; }//کاربر سیستم

	//---------------------------------سامانه مودیان----------------------------
	public long ID_tbl_Samane_Moadayan { get; set; }//آیدی
	public string Date_Send { get; set; }//تاریخ ارسال
	public string Time_Send { get; set; }//ساعت ارسال
	public int Type_Sorathesab { get; set; }//نوع صورتحساب
	public string Type_Sorathesab1 { get; set; }//نوع صورتحساب به فارسی
	public int Temp_Sorathesab { get; set; }//قالب صورتحساب
	public string Temp_Sorathesab1 { get; set; }//قالب صورتحساب به فارسی
	public int Subject_Sorathesab { get; set; }//موضوع صورتحساب
	public string Subject_Sorathesab1 { get; set; }//موضوع صورتحساب به فارسی
	public int State_Sorathesab { get; set; }//وضعیت ارسال
	public string State_Sorathesab1 { get; set; }//وضعیت ارسال به فارسی
	public string Serial_Sorathesab { get; set; }//سریال صورتحساب
	public string Sstid { get; set; }//شماره منحصر به فرد مالیاتی
	public string Sstid1 { get; set; }//شماره منحصر به فرد مالیاتی مرجع
	public string Uid { get; set; }//

	//----------------------------------------فراخوانی در ارسال----------------------------
	public string Taxid { get; set; }                     //شماره منحصر بفرد مالیاتیtbl_Samane_Moadayan.Sstid

	public string Indatim { get; set; }                     //تاریخ و زمان صدور صورتحساب میلادی صورتحسابDt_F
	public string Indati2m { get; set; }                     //تاریخ و زمان ایجاد صورتحساب میلادی صورتحسابDt_F
															 //public string Tm_C_F { get; set; }                     //زمان ایجاد فاکتور

	public string Inno { get; set; }                        //سریال صورتحساب
	public string Tinb { get; set; }                        //شماره اقتصادی خریدارtbl_TarafHesab.CodeEgtesad_TarafHesab
	public string Tins { get; set; }                        //شماره اقتصادی فروشندهtbl_Comp.Code_E_Comp
	public int Tob { get; set; }                            //نوع شخص خریدار: 1-حقیقی 2-حقوقی 3-مشارکت مدنی 4-اتباع غیر ایرانی 5-مصرف کننده نهاییType_TarafHesab_Samane_Moadyan
															//'' Bpn,                                               //شماره گذرنامه خریدار-کاربرد در الگوی بلیط هواپیما
															//0 Crn,                                                //شناسه یکتای ثبت قرارداد فروشنده
															//0 Sbc,                                                //کد شعبه فروشنده
															//0 Bbc,                                                //کد شعبه خریدار
	public string Bid { get; set; }                         //شناسه ملی / کد ملی(شناسه مشارکت مدنی) - کد فراگیر خریدارtbl_TarafHesab.CodeMelli_TarafHesab
	public string Bpc { get; set; }                         //کد پستی خریدارtbl_TarafHesab.CodePosti_Asli
															//1 Inp,                                                //الگوی صورتحساب: 1-فروش 2-فروش ارزی 3-صورتحساب طلا،جواهر،پلاتین 4-قرارداد پیمانکار 5-قبوض خدماتی 6-بلیط هواپیما
															//0 Scc,                                                //کد گمرک محل اضهار فروشنده
	public decimal Tprdis { get; set; }                     //مجموع مبلغ قبل از کسر تخفیفsum(Tedad* Fi_Bed_Haz) over(partition by tbl_F_Aglm.ID_tbl_F, tbl_F_Aglm.Type_tbl_F)

	public decimal Tdis { get; set; }                       //مجموع تخفیفاتTkh_A_F

	public decimal Tadis { get; set; }                      //مجموع مبلغ پس از کسر تخفیفsum(Tedad* Fi_Bed_Haz)over(partition by tbl_F_Aglm.ID_tbl_F, tbl_F_Aglm.Type_tbl_F) - Tkh_A_F

	public decimal Tvam { get; set; }                       //مجموع مالیات بر ارزش افزودهJM_F+JAv_F
															//0 Todam,									            //مجموع سایر مالیات عوارض و وجوه قانونی
															//1 Setm,										        //روش تسویه: 1-نقد 2-نسیه 3-نقد/نسیه
	public decimal Tbill { get; set; }                      //مجموع صورتحسابJ_F


	//public string Sstid { get; set; }                       //شناسه کالا/خدمتtbl_Kala.Shenase_Kala
	public string Sstt { get; set; }                        //شرح کالا/خدمتtbl_Kala.Desc_Shenase_Kala
	public string Mu { get; set; }                          //واحد کالا خدمتtblVahede_Kala_Moadyan.Vahede_Code_Moadyan
	public decimal Am { get; set; }                         //تعداد/مقدارtbl_F_Aglm.Tedad
	public decimal Fee { get; set; }                        //مبلغ واحدtbl_F_Aglm.Fi_Bed_Haz
															//0 Cfee,                                               //میزان ارز

	public decimal Prdis { get; set; }                      //مبلغ قبل از تخفیفtbl_F_Aglm.Fi_Bed_Haz* tbl_F_Aglm.Tedad

	public decimal Dis { get; set; }                        //مبلغ تخفیفtbl_F_Aglm.M_T_Radf_K

	public decimal Adis { get; set; }                       //مبلغ بعد از تخفیفFi_Ba_Takh    (tbl_F_Aglm.Fi_Bed_Haz* tbl_F_Aglm.Tedad)-tbl_F_Aglm.M_T_Radf_K

	public decimal Vra { get; set; }                        //نرخ مالیات بر ارزش افزودهMA_Radf_K
	public decimal Vam { get; set; }                        //مبلغ مالیات بر ارزش افزودهtbl_F_Aglm.M_AV_Radf_K
															//'' Odt,                                              //موضوع سایر مالیات و عوارض
															//0 Odr,                                                //نرخ سایر مالیات و عوارض
															//0 Odam,                                               //مبلغ سایر مالیات و عوارض
															//'' Olt,                                               //موضوع سایر وجوه قانونی
															//0 Olr,                                                //نرخ سایر وجوه قانونی
															//0 Olam,                                               //مبلغ سایر وجوه قانونی
															//0 Consfee,                                            //اجرت ساخت
															//0 Spro,                                               //سود فروشنده
															//0 Bros,                                               //حق العمل
															//0 Tcpbs,                                              //جمع کل اجرت،حق العمل،سود

	public decimal Tsstam { get; set; }//مبلغ کل کالا/خدمتtbl_F_Aglm.M_KHLS     ((Tedad* Fi_Bed_Haz)+M_AV_Radf_K)-M_T_Radf_K
									   //0 Cop,										//سهم نقدی از پرداخت
									   //0 Bsrn,										//شناسه یکتای ثبت قرارداد حق العمل کاری
									   //0 Vop,										//سهم ارزش افزوده از پرداخت

	//'' cut,										//نوع ارز
	//0 iinn,										//شماره سوئیچ پرداخت
	//0 acn,										//شماره پذیرنده فروشگاهی
	//0 trmn,										//شماره پایانه

	public decimal cap { get; set; }                //مبلغ پرداخت نقدیJ_F
													//0 insp,										//مبلغ پرداختی نسیه
													//0 trn,										//شماره پیگیری
													//'' pcn,										//شماره کارت پرداخت کننده صورتحساب
	public string pid { get; set; }                 //شناسه ملی/کد ملی(شناسه مشارکت مدنی)- کد فراگیر: پرداخت کننده صورتحسابtbl_TarafHesab.CodeMelli_TarafHesab
													//0 pdt,										//تاریخ و زمان پرداخت صورتحساب
													//0 tvop,										//مجموع سهم مالیات بر ارزش افزوده از پرداخت

	//1 ft,										    //نوع پرواز: 1-داخلی 2-خارجی
	//0 tax17,									    //مالیات موضوع ماده 17
	//0 dpvb										//عدم ‍پرداخت مالیات بر ارزش افزوده خریدار: 1-عدم پرداخت 2-پرداخت
}