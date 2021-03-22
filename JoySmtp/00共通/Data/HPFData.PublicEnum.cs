namespace JoySmtp.Data
{
    partial class Data
    {
        /// <summary>
        ///         ''' 画面の種類
        ///         ''' </summary>
        ///         ''' <remarks></remarks>
        public enum FormType : int
        {
            Menu = 1            // メニュー
,
            Master = 2          // マスタ画面
,
            Trans = 3           // 機能画面
        }

        /// <summary>
        ///         ''' 画面のモード
        ///         ''' </summary>
        ///         ''' <remarks></remarks>
        public enum FormModes : int
        {
            None = 0            // なし
,
            List = 1            // 一覧
,
            Insert = 2          // 追加
,
            Update = 3          // 変更
,
            Delete = 4          // 削除
        }

        /// <summary>
        ///         ''' 画面の処理ステップ
        ///         ''' </summary>
        ///         ''' <remarks></remarks>
        public enum FormStep : int
        {
            None = 0,
            Init = 1,
            KeyEditing = 2,
            DetailEditing = 3
        }
    }

    /// <summary>
    ///     ''' CloseSystem：終了（F1）ボタンで、システムを終了します
    ///     ''' CloseForm：ログイン画面へ（F2）ボタンで、当画面を閉じる、ログイン画面へ
    ///     ''' Updated：パスワードの更新処理で、ログイン画面へ
    ///     ''' </summary>
    public enum CloseType : int
    {
        None = 0,
        CloseSystem = 1,
        CloseForm = 2,
        UpdatedPWD = 3
    }

    public enum DeleteFlag : int
    {
        NotDelet = 0,
        Deleted = 1
    }

    public enum DataExistType : int
    {
        NotExist = -1,
        NotDelete = 0,
        Deleted = 1
    }

    public enum FromEditMode : int
    {
        None = 0            // 参照モード
,
        Add_Item = 1        // 新規モード
,
        Update_Item = 2     // 修正モード
,
        Delete_Item = 3     // 削除モード
,
        Interrupt_Item = 4  // 割込モード
,
        KeyNoInput_Item = 5 // 得意先決定前
,
        DoJuchu_Item = 6    // 受注処理
    }

    public enum FromUriageMode : int
    {
        Menu = 0            // メニュー
,
        SyouNinn = 1        // 売上承認（一般）画面
,
        SyouKai = 2         // 売上照会画面
,
        SyouKaiGari = 3     // 売上照会画面（仮）
,
        Hattyuu = 4         // 発注入力画面
    }

    public enum FileOutputStatus : int
    {
        Success = 0,
        Cancel = 1,
        Faild = 2,
        FileFound = 3,
        FileNotFound = 4,
        DirNotFound = 5,
        DataNotFound = 6,
        FileNameOutOfSize = 7
    }

    // エクセル計算タイプ
    public enum ExcelCalcModels : int
    {
        None = 0,
        Sum = 1,
        SubTraction = 2
    }

    // エクセルの数字形式の表示形式
    public enum NumberFormatLocalModels : int
    {
        None = 0,
        Text = 1
    }

    public enum HasuuEtcKbn : int
    {
        Raise = 0           // 切り上げ
,
        Ommit = 1           // 切捨て
,
        FourFive = 2        // 四捨五入
,
        Devalued = 3        // 切り下げ
    }


    public enum DateType : int
    {
        None = 0                            // 定義なし
,
        SeirekiHukoLong = 1                 // 西暦のyyyy/MM/DD HH:mm:ss
,
        SeirekiHukoShort = 2                // 西暦のyyyy/M/D H:m:s
,
        SeirekiKanji_Long = 3               // 西暦のyyyy年MM月DD日　HH時mm分ss秒
,
        SeirekiKanji_Short = 4              // 西暦のyyyy年M月D日　HH時m分s秒
    }
}
