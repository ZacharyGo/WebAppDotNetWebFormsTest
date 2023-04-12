using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TestDBFirstCient;
using WebAppDotNetWebFormsTest.Utilities;

namespace WebAppDotNetWebFormsTest.API
{
    public partial class myDropdown : System.Web.UI.UserControl
    {
        public class MyShopWithDistrictName
        {
            // 組織番号
            public string SSK_NO { get; set; }
            // 適用開始年月日
            public string TYK_YMD { get; set; }
            // 適用終了年月日
            public string TYS_YMD { get; set; }
            // 申込開始年月日
            public string MSM_KNG_H { get; set; }
            // 申込終了年月日
            public string MSM_SYRY_YMD { get; set; }
            // マイショップ名
            public string MISYP_ME { get; set; }
            // ソート用マイショップ名かな
            public string ST_YU_MISYP_ME_KN { get; set; }
            // 行政区コード
            public string GSK_CD { get; set; }
            // 表示用マイショップ郵便番号
            public string HYUZYU_MISYP_YUBN_NO { get; set; }
            // 表示用マイショップ住所
            public string HYUZYU_MISYP_ZYUSY { get; set; }
            // 表示用マイショップ電話番号
            public string HYUZYU_MISYP_DWNO { get; set; }
            // 表示用マイショップURL
            public string HYUZYU_MISYP_URL { get; set; }
            // 独自特典有無
            public string DKZ_TKTN_UM { get; set; }
            // 行政区名称
            public string GSK_MS { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            MyShopDropDownList.Items.Insert(0, new ListItem("hi", "0"));
            MyShopDropDownList.Items.Insert(0, new ListItem("hello", "-1"));
            TGZZZLog.StartLog(); 
            MyShopDropDownList.Items.Clear();

            //DropDownList1.Items.a
            C_KIYK contract;
            string datenow = DateTime.Now.ToString("yyyyMMdd");
            List<TGZZZDba16.Dba16Accept4> myShopList;
            bool areaStorePresenceFlag;
            TGZZZDba16 tGZZZDba16 = new TGZZZDba16(new TohogasDataContext());
            int result;
            using (var context = new TohogasDataContext())
            {

                string customerNumber = Request["customerNumber"];
                // string orgnum = Request["orgnum"];

                //string areastorepresenceflag = Request["areastorepresenceflag"];
                //string postCode2 = Request["postcode2"];
                if (customerNumber == null)
                    return;
                try
                {
                    // お客さま番号より契約番号を取得する。
                    StringBuilder sbSql = new StringBuilder();

                    sbSql.Append("select a.* ");
                    sbSql.Append(" from C_KIYK a");
                    sbSql.Append(" inner join C_KIYK_SYUSI b");
                    sbSql.Append(" on a.KIYK_NO = b.KIYK_NO");
                    sbSql.Append(" where b.SYB_NO = @p0");
                    sbSql.Append(" and a.SBS_SYBT = '01'");
                    sbSql.Append(" and a.TYK_YMD <= @p1");
                    sbSql.Append(" and a.TYS_YMD >= @p1 ");
                    sbSql.Append(" and a.KIYK_KNG_H <=  @p1");
                    sbSql.Append(" and a.KIYK_SYRY_YMD >=  @p1");
                    sbSql.Append(" and a.RNR_SKZY_FLG = '0'");
                    sbSql.Append(" and b.RNR_SKZY_FLG = '0'");
                    sbSql.Append(" order by b.KIYK_NO desc");
                    SqlParameter p0 = new SqlParameter("@p0", System.Data.SqlDbType.Char);
                    SqlParameter p1 = new SqlParameter("@p1", System.Data.SqlDbType.Char);
                    p0.Value = customerNumber;
                    p1.Value = datenow;

                    List<C_KIYK> rec = context.Database.SqlQuery<C_KIYK>
                        (sbSql.ToString()
                        , p0, p1).ToList();
                    if (rec.Count == 0)
                    {
                        // ★NotFound：汎用ログに出力、イベントログに出力。
                        TGZZZLog.WriteEventLog(TGZZZLog.EVENT_LOG_LEVEL_INFO, "Not Found");
                        TGZZZLog.WriteLogFile_INFO("Not Found", "", "");
                        return;
                    } 
                    else
                    {
                        contract = rec.First();
                    }
                    // ★データ取得できた場合、処理を継続する。

                    // 契約番号よりENEDO店舗情報取得共通関数を呼び出す＜共通関数：ENEDO店舗情報取得（契約番号）＞
                    result = tGZZZDba16.AcquisitionOfENEDOStoreInformation(contract.KIYK_NO, DateTime.Now, true, out myShopList, out areaStorePresenceFlag);

                }
                catch (Exception ex)
                {
                    // ★DBアクセス異常：汎用ログに出力、イベントログに出力。
                    TGZZZLog.WriteLogFile_ERR(TGZZZConstants.LOG_ERROR, "", "", ex);
                    TGZZZLog.WriteEventLog_ERR(TGZZZConstants.EVENT_LOG_ERROR);
                }
                finally
                {
                    // 汎用_終了ログを出力する。
                    TGZZZLog.EndLog();
                }

                // If a contract is selected in the target contract list (for subscription), and an area store exists in the selected contract
                // First line = "A nearby shop (" + My Shop management of area stores acquired based on the security service linked to the selected contract M. Store name + ") 【" + District Master Administrative Region name + "】
                // Second line and subsequent lines = My Shop management other than area stores M. Store name + "【" + District Master Administrative Region name + "】" list (sorted by the District Master item)
                // If a contract is selected in the subject contract list (The area store does not exist in the selected contract, or the contracted area store is not registered as My Shop.), 

                // If no contract is selected in the target contract list (for subscription):
                // Blank
                /*postcode1.Text = postCode1;
                postcode2.Text = postCode2;
                Label1.Text = postCode1;
                Label2.Text = postCode2;*/
                
                this.Response.Clear();
                this.Response.ContentType = "application/json; charset=utf-8";
                string jsonout = this.getJson();
                this.Response.Output.Write(jsonout);
                this.Response.End();
            }
        }
       // private string getJson(string address1, string address2, string address3, string prefecturesCd, Dictionary<string, string> messageList, string returnCode)
        private string getJson()
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            // address1
            json.Append("\"address1\":");
            json.Append("\"").Append("address1").Append("\",");
            json.Append("\"returnCode\":");
            json.Append("\"").Append("0").Append("\"");
            json.Append("}");
            return json.ToString();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Console.WriteLine("PostBack");
            }
        }
    }
}