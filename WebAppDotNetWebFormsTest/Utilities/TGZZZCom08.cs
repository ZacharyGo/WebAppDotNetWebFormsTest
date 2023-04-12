/// <summary>
/// プロジェクト:お客さまポータルサイト
/// 機能        :共通処理
/// クラス名    :文字型チェック
/// 作成日      :2015/11/27
/// Copyright 2015 FUJITSU LIMITED
/// </summary>

using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using TestDBFirstCient;
using Microsoft.VisualBasic;

namespace TestDBFirstCient.Utilities
{
    /// <summary>
    /// 文字型チェック
    /// </summary>
    public static class TGZZZCom08
    {
        /// <summary>
        /// 文字型チェック
        /// </summary>
        /// <param name="paramStr">対象文字列</param>
        /// <param name="paramType">文字型ID</param>
        /// <returns>正常／異常／パラメーター属性不正</returns>
        public static int CheckStringType(string paramStr, string paramType)
        {
            try
            {
                // TGZZZLog.StartLog();

                string regexPattern = "";
                string patternName = "";
                string tmpErrLogMsg = "";
                
                // 正規表現とログメッセージを設定する
                switch (paramType)
                {
                    // 半角数字
                    case TGZZZConstants.CHECK_STRING_TYPE_HALFNUM:
                        // 文字型チェック(半角数字)を取得する
                        patternName = "CheckStringHalfNum";
                        tmpErrLogMsg = "文字列に半角数字以外の文字が含まれています。";
                        break;
                    // 半角英数字
                    case TGZZZConstants.CHECK_STRING_TYPE_HALFALPHNUM:
                        // 文字型チェック(半角英数字)を取得する
                        patternName = "CheckStringHalfAlphNum";
                        tmpErrLogMsg = "文字列に半角英数字以外の文字が含まれています。";
                        break;
                    // 半角英数字記号
                    case TGZZZConstants.CHECK_STRING_TYPE_HALFALPHNUMSYMBOL:
                        // 文字型チェック(半角英数字記号)を取得する
                        patternName = "CheckStringHalfSymbol";
                        tmpErrLogMsg = "文字列に半角英数字記号以外の文字が含まれています。";
                        break;
                    // 半角
                    case TGZZZConstants.CHECK_STRING_TYPE_HALFALL:
                        // 半角チェックは正規表現は使用しない
                        Encoding sjisEncHalf = Encoding.GetEncoding("Shift_JIS");
                        if (sjisEncHalf.GetByteCount(paramStr) != paramStr.Length)
                        {
                            TGZZZLog.WriteLogFile_ERR(String.Format("文字列は半角以外の文字が含まれています。対象文字列 = [{0}]", paramStr), "", "");
                            return TGZZZConstants.ERR_PRMATR;
                        }
                        return TGZZZConstants.SUCCEESS;
                    // 全角
                    case TGZZZConstants.CHECK_STRING_TYPE_FULLALL:
                        // 全角チェックは正規表現は使用しない
                        Encoding sjisEncFull = Encoding.GetEncoding("Shift_JIS");
                        if (sjisEncFull.GetByteCount(paramStr) != paramStr.Length * 2)
                        {
                            TGZZZLog.WriteLogFile_ERR(String.Format("文字列は全角以外の文字が含まれています。対象文字列 = [{0}]", paramStr), "", "");
                            return TGZZZConstants.ERR_PRMATR;
                        }
                        return TGZZZConstants.SUCCEESS;
                    // 全角カナ
                    case TGZZZConstants.CHECK_STRING_TYPE_FULLKANA:
                        // 文字型チェック(全角カナ)を取得する
                        patternName = "CheckStringFullKana";
                        tmpErrLogMsg = "文字列に全角カナ以外の文字が含まれています。";
                        break;
                    // 整数値
                    case TGZZZConstants.CHECK_STRING_TYPE_INT32:
                        // 整数値チェックは正規表現は使用せず変換可能かを判定する(SmarP用)
                        int retNumber;
                        if (!Int32.TryParse(paramStr, out retNumber))
                        {
                            TGZZZLog.WriteLogFile_ERR(String.Format("文字列は整数値に変換できない文字が含まれています。対象文字列 = [{0}]", paramStr), "", "");
                            return TGZZZConstants.ERR_PRMATR;
                        }
                        return TGZZZConstants.SUCCEESS;
                    // 半角大文字英数字
                    case TGZZZConstants.CHECK_STRING_TYPE_HALFUPPERALPHNUM:
                        // 文字型チェック(半角大文字英数字)を取得する
                        patternName = "CheckStringHalfUpperAlphNum";
                        tmpErrLogMsg = "文字列に半角英(大文字)数字以外の文字が含まれています。";
                        break;
                    default:
                        TGZZZLog.WriteLogFile_ERR(String.Format("文字型IDが不正です。文字型ID = [{0}]", paramType), "", "");
                        return TGZZZConstants.ABNORMAL;
                }
                return TGZZZConstants.SUCCEESS;

                // web.Configのapp.Settingsから各正規表現を取得する
                regexPattern = ConfigurationManager.AppSettings[patternName];
                if (regexPattern == null || regexPattern == "")
                {
                    TGZZZLog.WriteLogFile_ERR(String.Format(TGZZZConstants.LOG_WEBCONF_ERR, patternName), "", "");
                    return TGZZZConstants.ABNORMAL;
                }

                // 正規表現によるチェック
                Regex chkRegex = new Regex(regexPattern);
                if (!chkRegex.IsMatch(paramStr))
                {
                    TGZZZLog.WriteLogFile_ERR(String.Format(tmpErrLogMsg + "対象文字列 = [{0}]", paramStr), "", "");
                    return TGZZZConstants.ERR_PRMATR;
                }
                return TGZZZConstants.SUCCEESS;
            }
            catch (Exception e)
            {
                TGZZZLog.WriteLogFile_ERR(TGZZZConstants.LOG_ERROR, "", "", e);
                return TGZZZConstants.ABNORMAL;
            }
            finally
            {
                // TGZZZLog.EndLog();
            }
        }

        /// <summary>
        /// 住所自動変換（半角英数字、半角スペース、半角マイナス、半角中点を全角文字に変換する）
        /// </summary>
        /// <param name="str">住所(町域名等・番地)または住所(建物名・部屋番号)（</param>
        /// <param name="emStr">住所自動変換後の住所(町域名等・番地)または住所(建物名・部屋番号)（</param>
        /// <returns>正常／異常</returns>
        public static int AddressAutoConversion (string str,out string emStr)
        {
            emStr = null;
            try
            {
                TGZZZLog.StartLog();

                string regexStr = ""; /*正規表現の式を格納するための変数*/

                // web.Configのapp.Settingsから正規表現を取得する
                regexStr = ConfigurationManager.AppSettings["AddressConversionFormat"];
                if (String.IsNullOrEmpty(regexStr))
                {
                    TGZZZLog.WriteLogFile_ERR(String.Format(TGZZZConstants.LOG_WEBCONF_ERR, "AddressConversionFormat"), "", "");
                    return TGZZZConstants.ABNORMAL;
                }
                // 正規表現によるチェック
                Regex re = new Regex(ConfigurationManager.AppSettings["AddressConversionFormat"]);
                emStr = re.Replace(str, myReplacer);
                return TGZZZConstants.SUCCEESS;
            }
            catch (Exception e)
            {
                TGZZZLog.WriteLogFile_ERR(TGZZZConstants.LOG_ERROR, "", "", e);
                return TGZZZConstants.ABNORMAL;
            }
            finally
            {
                TGZZZLog.EndLog();
            }
        }

        /// <summary>
        /// 正規表現に一致した文字列を全角に変換する
        /// </summary>
        /// <param name="m">正規表現に一致した文字列</param>
        ///<returns>対象文字を全角に変換した文字列</returns>
        private static string myReplacer(Match m)
        {
            try
            {
                return Strings.StrConv(m.Value, VbStrConv.Wide, 0);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
