
/// <summary>
/// プロジェクト：お客さまポータルサイト
/// 機能        ：共通処理
/// クラス名    ：DB基底
/// 作成日      ：2015/11/25
/// Copyright 2015 FUJITSU LIMITED
/// </summary>


namespace WebAppDotNetWebFormsTest.Utilities
{
    /// <summary>
    /// DB基底
    /// </summary>
    public class TGZZZDba
    {
        /// <summary>
        /// データコンテキスト
        /// </summary>
        protected TohogasDataContext context;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">データコンテキスト</param>
        public TGZZZDba(TohogasDataContext context)
        {
            this.context = context;
        }
    }
}
