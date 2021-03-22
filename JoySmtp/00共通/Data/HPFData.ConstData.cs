using System;

namespace JoySmtp.Data
{
    partial class HPFData
    {
        public const string FILE_RECV_Extension2 = ".CSV";
        public const string FILE_RECV_Extension = ".csv";
        public const string FILE_TEMP_Extension = ".tmp";
        public const string FILE_XLSX_Extension = ".xlsx";
        public const string FILE_BACK_Extension = ".bak";

        public const string CRT_STAFF_CD = "SYSTEM";
        public const string LINE_TYPE_1 = "'";
        public const string LINE_TYPE_2 = "";
        public const string USER_ID_ADMIN = "admin";

        // <<メッセージテーブル>>70
        //public const string MESSAGE_COLUMN_MSG_CD = "MSG_CD";
        //public const string MESSAGE_COLUMN_MSG_NM = "MSG_NM";
        public const string MESSAGE_COLUMN_ERR_CD = "NAC_ERR_CD";
        public const string MESSAGE_COLUMN_ERR_NM = "NAC_ERR_NM";

        // <<Resourceテーブル>>
        public const string RESOURCE_COLUMN_PRGID = "PRGID";
        public const string RESOURCE_COLUMN_RESID = "RESID";
        public const string RESOURCE_COLUMN_RES = "RES_JP";

        // <<Sys_ctrlテーブル>>
        public const string SYS_CTRL_COLUMN_SYS_CTRL_KEY = "SYS_CTRL_KEY";
        public const string SYS_CTRL_COLUMN_SETTING_INFO = "SETTING_INFO";


        public const string MST_SEQ_TABLE_NAME = "MST_SEQ";
        public const string MST_SEQ_COLUMN_SEQ_NUM_NOW = "SEQ_NUM_NOW";
        public const string MST_SEQ_COLUMN_SEQ_NUM_MAX = "SEQ_NUM_MAX";
        public const string MST_SEQ_COLUMN_SEQ_LENGH_MAX = "SEQ_LENGH_MAX";
        // Public Const MST_SEQ_COLUMN_SEQ_KBN As String = "SEQ_KBN"
        // Public Const MST_SEQ_COLUMN_NENDO_RESET_FLAG As String = "NENDO_RESET_FLAG"
        // Public Const MST_SEQ_COLUMN_NENDO As String = "NENDO"
        // Public Const MST_SEQ_COLUMN_SEQ_PRE_CHAR As String = "SEQ_PRE_CHAR"
        // Public Const MST_SEQ_COLUMN_SEQ_NAME As String = "SEQ_NAME"
        // Public Const MST_SEQ_COLUMN_VERSION_NO As String = "VERSION_NO"

        public const int M_KAZEI_KAZEI_KBN = 0;
        public const int M_KAZEI_KAZEI_NAME = 1;

        public const int M_DEN_DEN_CD = 0;
        public const int M_DEN_DEN_NAME = 1;

        public const int M_SOKO_SOKO_CD = 0;
        public const int M_SOKO_SOKO_NAME = 1;

        public const int M_TORIHIKISAKI_CD = 0;
        public const int M_TORIHIKISAKI_NAME = 1;

        // <<C_KYOTHU_KBN>>
        // Public Const KYOTU_KBN_TABLE_NAME As String = "C_KYOTHU_KBN"
        public const string KYOTU_KBN_COLUMN_SIKIBETU_KBN = "SHIKIBETSU_KBN";
        public const string KYOTU_KBN_COLUMN_SIKIBETU_NAME = "SHIKIBETSU_NAME";
        public const string KYOTU_KBN_COLUMN_KBN_CODE = "KBN_CODE";
        public const string KYOTU_KBN_COLUMN_KBN_NAME = "KBN_NAME";
        public const string KYOTU_KBN_COLUMN_HYOJI_JUN = "HYOJI_JUN";
        public const string KYOTU_KBN_COLUMN_SONOTA1 = "SONOTA1";
        public const string KYOTU_KBN_COLUMN_HOJO = "HOJO";
        public const string KYOTU_KBN_COLUMN_HOJO_1 = "HOJO_1";
        public const string KYOTU_KBN_COLUMN_HOJO_2 = "HOJO_2";
        public const string KYOTU_KBN_COLUMN_HOJO_3 = "HOJO_3";
        public const string KYOTU_KBN_COLUMN_HOJO_4 = "HOJO_4";

        public const int KYOTU_KBN_INDEX_KBN_CODE = 0;
        public const int KYOTU_KBN_INDEX_KBN_NAME = 1;

        //<<T_DECLARATION_STATUS>>
        public const string SHORI_SHURUI_DECLARATION = "DECLARATION";
        public const string SHORI_SHURUI_NACCS = "NACCS";
        public const string SHORI_SHURUI_PAXTRAX = "PAXTRAX";

        // Public Const LANGUAGE_EN As String = "EN"
        // Public Const LANGUAGE_JP As String = "JP"

        //// 分類区分
        //// Public Const BUNRUI_KBN_1 As String = "1"       '１：大分類
        //// Public Const BUNRUI_KBN_2 As String = "2"       '２：中分類
        //// Public Const BUNRUI_KBN_3 As String = "3"       '３：小分類

        //// 調整区分
        //// Public Const CHOSEI_KBN_0 As String = "0"       '0:出庫
        //// Public Const CHOSEI_KBN_1 As String = "1"       '1:入庫
        //// Public Const CHOSEI_KBN_2 As String = "2"       '2:WMS出庫

        //// 倉庫種類
        //// Public Const SOKO_SHURUI_1 As String = "1"       '１：良品＆振指
        //// Public Const SOKO_SHURUI_2 As String = "2"       '２：不良
        //// Public Const SOKO_SHURUI_3 As String = "3"       '３：入荷不良

        //// 商品状態区分
        //// Public Const SHOHIN_JYOTAI_KBN_0 As String = "0"       '０：良品
        //// Public Const SHOHIN_JYOTAI_KBN_1 As String = "1"       '１：振指
        //// Public Const SHOHIN_JYOTAI_KBN_2 As String = "2"       '２：入荷不良
        //// Public Const SHOHIN_JYOTAI_KBN_3 As String = "3"       '３：不良

        //// 取引区分
        //// Public Const TORIHIKI_KBN_1 As String = "1"         '1：入荷
        //// Public Const TORIHIKI_KBN_2 As String = "2"         '2：移動
        //// Public Const TORIHIKI_KBN_3 As String = "3"         '3：売上
        //// Public Const TORIHIKI_KBN_4 As String = "4"         '4：調整
        //// Public Const TORIHIKI_KBN_5 As String = "5"         '5：棚間移動
        //// Public Const TORIHIKI_KBN_6 As String = "6"         '6：自動棚間移動※
        //// 処理内容
        //// Public Const UKEBARAI_KBN_1 As String = "1"         '1：入荷
        //// Public Const UKEBARAI_KBN_2 As String = "2"         '2：仕入返品
        //// Public Const UKEBARAI_KBN_3 As String = "3"         '3：移動
        //// Public Const UKEBARAI_KBN_4 As String = "4"         '4：WMS出庫
        //// Public Const UKEBARAI_KBN_5 As String = "5"         '5：売上
        //// Public Const UKEBARAI_KBN_6 As String = "6"         '6：売上返品
        //// Public Const UKEBARAI_KBN_7 As String = "7"         '7：在庫調整
        //// Public Const UKEBARAI_KBN_8 As String = "8"         '8：棚卸調整
        //// Public Const UKEBARAI_KBN_9 As String = "9"         '9：棚間移動
        //// Public Const UKEBARAI_KBN_10 As String = "10"         '10：一括棚移動
        //// Public Const UKEBARAI_KBN_11 As String = "11"         '11：自動棚間移動
        //// 調整理由
        //// Public Const CHOSEI_RIYU_01 As String = "1"         '1         廃棄
        //// Public Const CHOSEI_RIYU_02 As String = "2"         '2         経費（本社）
        //// Public Const CHOSEI_RIYU_03 As String = "3"         '3         経費（事業部）
        //// Public Const CHOSEI_RIYU_04 As String = "4"         '4         仕入返品
        //// Public Const CHOSEI_RIYU_05 As String = "5"         '5         在庫移動
        //// 在庫発生区分
        //// Public Const ZAIKO_HASSEI_KBN_1 As String = "1"         '1         入荷
        //// Public Const ZAIKO_HASSEI_KBN_2 As String = "2"         '2         移動返品
        //// Public Const ZAIKO_HASSEI_KBN_3 As String = "3"         '3         売上返品
        //// Public Const ZAIKO_HASSEI_KBN_4 As String = "4"         '4         在庫調整
        //// Public Const ZAIKO_HASSEI_KBN_5 As String = "5"         '5         棚卸調整

        //// 商品状態区分
        //// Public Const SHOHIN_JYOUTAI_KBN_0 As String = "0"
        //// Public Const SHOHIN_JYOUTAI_KBN_1 As String = "1"
        //// Public Const SHOHIN_JYOUTAI_KBN_2 As String = "2"
        //// Public Const SHOHIN_JYOUTAI_KBN_3 As String = "3"

        //// 'ﾕｰｻﾞ区分
        //// Public Const USER_KBN_CD_1 As String = "99"             '99 管理者
        //// Public Const USER_KBN_CD_2 As String = "2"              '2 一般
        //// Public Const USER_KBN_CD_3 As String = "3"              '3 バイト

        //// 棚管理有無ﾌﾗｸﾞ
        //// Public Const TANA_KANRI_UMU_FLAG_0 As String = "0"      '0：棚管理しない
        //// Public Const TANA_KANRI_UMU_FLAG_1 As String = "1"      '1：棚管理する

        //// 'ユーザ区分
        //// Public Const USER_KBN_1 As String = "1"                 '1：管理者
        //// Public Const USER_KBN_2 As String = "2"                 '2：
        //// Public Const USER_KBN_3 As String = "3"                 '3：

        //// 納品先種別
        //// Public Const NOHINSAKI_KIND_TENPOSOKO As String = "1"    '1：店舗・倉庫
        //// Public Const NOHINSAKI_KIND_TOKUISAKI As String = "2"    '2：得意先
        //// Public Const NOHINSAKI_KIND_KOJIN As String = "3"        '3：個人・社販

        //// 出荷指示区分
        //public const string SHIJI_KBN_DC = "0";             // 0：在庫から
        //public const string SHIJI_KBN_TC = "1";              // 1：当日入荷当日出荷

        //// 出荷先種別
        //// Public Const SHUKKASAKI_KIND_REV As String = "0"          '0：なし（逆ﾋﾟｯｷﾝｸﾞ）
        //// Public Const SHUKKASAKI_KIND_TENPOSOKO As String = "1"    '1：店舗・倉庫
        //// Public Const SHUKKASAKI_KIND_TOKUISAKI As String = "2"    '2：得意先

        //// 出荷アンマッチ
        //// Public Const UNMATCH_NASHI As String = "0"                  ''出荷ｱﾝﾏｯﾁ無
        //// Public Const UNMATCH_ARI As String = "1"                    ''出荷ｱﾝﾏｯﾁ有

        //public const string STRING_COMMA = ",";
        //// Public Const STRING_COLON As String = ":"
        //public const string STRING_AT = "@";
        //// Public Const STRING_ASTERISK As String = "*"
        //// Public Const STRING_PER As String = "%"

        //// Public Const SHIMEBI_99 As String = "99"                '99：月末
        //// Public Const SHIMEBI_1 As String = "1"                  '1：日

        //// Public Const TANA_STATUS_99 As String = "99"            '99:削除

        //// Public Const NOHIN_JYOKEN_PATTERN_KBN_A As String = "A"
        //// Public Const NOHIN_JYOKEN_PATTERN_KBN_B As String = "B"

        //public const string CTRL_NAME_PUB_SPREAD = "Spread";

        //// Public Const STRING_CSV_SAPATER As String = ","

        //// キー部
        //public const string CTRL_NAME_PRE_CONTAINER_KEY = "GcCo_c_Key";
        //// 明細部
        //public const string CTRL_NAME_PRE_CONTAINER_DETAIL = "GcCo_c_Detail";

        //public const string CTRL_NAME_PRE_MENU_BUTTON = "GcShapeButton";

        // 復号化キー
        public const string DECRYPT_KEY = "wzgldlllljtzwdcnzhwl";     // 復号化キー

        //// 状態指定有無
        //public const string TORIKOMI_JOTAI_NOTHING = "条件なし";
        //public const string TORIKOMI_JOTAI_SEVERAL = "複数選択";
        //// 承認状態
        //public const string SYOUNIN_JOTAI = "承認待ち";

        //// ADD START F.RICK 2014/04/10
        //// 倉庫の所在のｾﾝﾀｰ外の区分コード
        //// Public Const CENTER_OUTSIDE As String = "99"
        //// ADD END F.RICK 2014/04/10

        //// 得意先検索データ区分 処理区分（0：売掛外,1：得意先）
        //public const string TOKUISAKI_DATA_KBN0 = "0";
        //public const string TOKUISAKI_DATA_KBN1 = "1";


        // コードにゼロを充填する
        public const char FILL_STRING = '0';
        //public const int FILL_TORIHIKISAKI_CD = 8;        // 取引先CD、得意先、発注先
        //public const int FILL_KAIKEI_CD = 6;              // 会計CD 、请求先、仕入先
        //public const int FILL_KIGYO_CD = 5;               // 企業CD、
        //public const int FILL_NOHINSAKI_CD = 10;          // 納品先CD
        //public const int FILL_HANBAI_SENRYAKU_CD = 3;     // 販売戦略CD

        //// 2014/10/16 suzuki add st
        //public const int FILL_HACHU_NO = 8;               // 発注No
        //public const int FILL_HACHU_EDA_NO = 2;           // 発注枝No
        //// 2014/10/16 suzuki add ed

        //// 
        //// ﾋﾟｯｷﾝｸﾞ区分
        //// 
        //public const string PICK_KBN_REV = "0";       // '逆ﾋﾟｯｷﾝｸﾞ
        //public const string PICK_KBN_SNG = "1";       // 'ｼﾝｸﾞﾙﾋﾟｯｷﾝｸﾞ
        //public const string PICK_KBN_TTL = "2";       // 'ﾄｰﾀﾙﾋﾟｯｷﾝｸﾞ

        //// シール出力 ファイル名
        //public const string CSV_NAME = "シール出力.csv";


        //// 'メッセージ画面のタイトル
        //// Public Const MESSAGE_DETAIL As String = "ﾒｯｾｰｼﾞ内容"
        //public const string MESSAGE_DETAIL = "";
        //public const string MESSAGE_CAPTION_ERROR = "入力エラー";
        //public const string MESSAGE_CAPTION_SYSTEMERROR = "システムエラー";
        //public const string MESSAGE_CAPTION_WARNING = "警告";
        //public const string MESSAGE_CAPTION_INFORMATION = "情報";
        //public const string MESSAGE_CAPTION_QUESTION = "確認";

        //public const string MESSAGE_TYPE_ERROR = "E";
        //public const string MESSAGE_TYPE_QUESTION = "Q";
        //public const string MESSAGE_TYPE_WARNING = "W";
        //public const string MESSAGE_TYPE_SYSTEM_ERROR = "S";
        //public const string MESSAGE_TYPE_INFORMATION = "I";

        //// Public Const MODE_ADD As String = "新規"
        //public const string MODE_INST = "登録";
        //// Public Const MODE_UPD As String = "修正"
        //// Public Const MODE_NOE As String = "参照"

        //public const string MODE_DECIDE = "決定";
        //public const string MSG_CHOKUSOU_HENPIN_URIAGE_INST = "直送返品（売上）の登録";
        //public const string MSG_CHOKUSOU_HENPIN_SHIRE_INST = "直送返品（仕入）の登録";
        //public const string MSG_URIAGE_BANGOU = "売上番号";
        //public const string MSG_SHIRE_BANGOU = "仕入番号";
        //public const string MSG_URIAGE_CHOUSEI_MODE_SHONIN = "データを承認";
        //public const string MSG_URIAGE_CHOUSEI_SHONIN = "承認";
        //public const string MSG_URIAGE_CHOUSEI_INST = "登録";
        //public const string MSG_URIAGE_CHOUSEI_UPD = "更新";
        //public const string MSG_URIAGE_MD_MODE_SHONIN = "承認";
        //public const string MSG_URIAGE_MD_SHONIN = "承認";
        //public const string MSG_URIAGE_MD_INST = "登録";
        //public const string MSG_URIAGE_MD_UPD = "更新";

        //public const string MSG_URIAGE_I001 = "更新";
        //public const string MSG_URIAGE_I005_1 = "訂正";
        //public const string MSG_URIAGE_I005_2 = "承認";
        //public const string MSG_URIAGE_I005_3 = "登録";
        //public const string MSG_URIAGE_Q005 = "承認";

        //public const string MSG_URIAGE_CHECK_1 = "変更する数量";
        //public const string MSG_URIAGE_CHECK_2 = "新規した数量";
        //public const string MSG_URIAGE_CHECK_3 = "数量";
        //public const string MSG_URIAGE_CHECK_4 = "在庫数以下";

        //public const string MESSAGE_DELROW = "行を削除";
        //public const string MESSAGE_DEL = "削除";
        //public const string MESSAGE_INS = "登録";
        //public const string MESSAGE_COPY = "コピー";
        //public const string MESSAGE_CANCEL = "キャンセル";
        //public const string MESSAGE_MITUMORI_NO = "見積番号";
        //public const string MESSAGE_HACHU_NO = "受注番号";
        //public const string MESSAGE_URIAGE_NO = "売上番号";
        //public const string MESSAGE_JUCHU = "受注";
        //public const string MSG_SEARCH_JANCD = "検索されたJanコード";
        //public const string MESSAGE_PARAM_MITUMORISHOUKAI = "見積照会";
        //public const string MESSAGE_PARAM_JUCHUSHOUKAI = "受注照会";
        //public const string MESSAGE_PARAM_URIAGESHOUKAI = "売上照会";
        //public const string MESSAGE_PARAM_URIKAKESHOUKAI = "売掛照会";
        //public const string MESSAGE_PARAM_KAIKAKESHOUKAI = "買掛照会";
        //public const string MESSAGE_PARAM_URIKAKEMOTOSHOUKAI = "売掛元帳照会";
        //public const string MESSAGE_PARAM_KAIKAKEMOTOSHOUKAI = "買掛元帳照会";

        //public const string MESSAGE_ZAIKOTYOUSEI = "在庫調整伝票";


        //public const string MSG_JUCHU_DATE_ST = "受注開始日";
        //public const string MSG_JUCHU_DATE_EN = "受注終了日";
        //public const string MSG_HACHU_DATE_ST = "発注開始日";
        //public const string MSG_HACHU_DATE_EN = "発注終了日";

        //public const string MSG_EN = "円";

        //public const string MSG_PW1 = "システムPW1";
        //public const string MSG_PW2 = "システムPW2";

        //public const string MSG_BUNRUI_2 = "中分類";
        //public const string MSG_BUNRUI_3 = "小分類";
        //public const string MSG_BUNRUI_4 = "細分類";

        //public const string MSG_SHOHIN_0 = "該当商品";
        //public const string MSG_SHOHIN_1 = "有形品";
        //public const string MSG_SHOHIN_2 = "無形品";
        //public const string MSG_SHOHIN_3 = "該当商品の仕入先CD";
        //public const string MSG_SHOHIN_4 = "指定仕入先CD";
        //public const string MSG_SHOHIN_5 = "指定有形品の仕入先CD";

        //public const string MSG_DATE_1 = "過去30日～ｼｽﾃﾑ日付";
        //public const string MSG_DATE_2 = "当月度～ｼｽﾃﾑ日付";
        //public const string MSG_DATE_3 = "当月度～翌月度";

        //public const string MSG_ZAIKO_KBN = "在庫区分";
        //public const string MSG_INDANG = "引当可能";

        //public const string DETAIL_MSG = "明細";
        //public const string DETAIL_DATA_MSG = "対象データ";
        //public const string GAITOU_DATA_MSG = "該当データ";
        //public const string SIZE_DATA_MSG = "サイズデータ";
        //public const string COLOR_DATA_MSG = "カラーデータ";
        //public const string SHOHIN_DATA_MSG = "商品データ";
        //public const string SUK_AREA_MSG = "SKUエリアの中に";

        //public const string SHIRE_DATA_MSG = "仕入のデータ";
        //public const string KARI_SHIRE_DATA_MSG = "仮仕入のデータ";

        //public const string SHIRE_TANKA_HANI_MSG = "仕入単価範囲";
        //public const string MSG_SHIRE_SHOUKAI = "仕入照会";
        //public const string MSG_TYOUHYOU_HAKOU = "帳票発行";
        //public const string MSG_JYUTYU_SHOUKAI = "受注処理";
        //public const string MSG_JYUTYU = "受注";
        //public const string MSG_URIKAKESAKI = "売掛先";
        //public const string MSG_KAIKAKESAKI = "買掛先";
        //public const string MSG_SEIKYU_JOUKEN_HENKOU = "請求条件変更";
        //public const string MSG_SHIHARAI_JOUKEN_HENKOU = "支払条件変更";
        //public const string MSG_YUUKOU_NA_SEIKYU_DATA = "有効な請求データ";
        //public const string MSG_YUUKOU_NA_SHIHARAI_DATA = "有効な支払データ";
        //public const string MSG_HENKOU_MAE_SEIKYU_ZANDAKA = "変更前請求残高";
        //public const string MSG_HENKOU_GO_SEIKYU_ZANDAKA = "変更後請求残高";
        //public const string MSG_HENKOU_MAE_SHIHARAI_ZANDAKA = "変更前支払残高";
        //public const string MSG_HENKOU_GO_SHIHARAI_ZANDAKA = "変更後支払残高";

        //public const string MSG_HENSHUU = "編集";


        //public const string DETAIL_DATA_SENTAKU = "選択数";
        //public const string DETAIL_DATA_SHOUNYU = "受注数";
        //public const string DETAIL_DATA_ZAIKOSU = "在庫数";

        //public const string DEITEI_SUU = "訂正数";
        //public const string DEITEI_TANKA = "対象訂正単価";

        //public const string SUURYOU_MSG = "数量";
        //public const string ZAIKOSUURYOU_MSG = "三共在庫数量";
        //public const string YICHI_IJYOU_MSG = "１以上";

        //public const string URIAGE_DENHYOU = "売上照会の伝票を出力";

        //public const string URIAGE_DATA = "売上のデータ";
        //public const string URIAGE_CYOSEI_DATA = "仮売上（伝種：○○調整）のデータ";
        //public const string URIAGE_MD_DATA = "仮売上（伝種：ＭＤ）のデータ";
        //public const string URIAGE_J = "仮売上（状態：計上待、伝種：出荷売上）のデータ";
        //public const string URIAGE_Z = "仮売上（伝種：在庫売上、売上返品）のデータ";

        //public const string KAHEI = "（千円）";
        //public const string DETAIL_ARARI = "粗利";
        //public const string DETAIL_URIAGE_KEIKAKU = "売上計画";

        //public const string SHIZUN_1 = "AW";
        //public const string SHIZUN_2 = "SS";

        //public const string MODE_SPREAD_ADD = "ADD";
        //public const string MODE_SPREAD_UPD = "UPD";
        //public const string MODE_SPREAD_NOE = "NOE";
        //public const string MODE_SPREAD_ADD_END = "END";
        //public const string MODE_SPREAD_CANCEL = "CANCEL";
        //public const string MODE_SPREAD_ADD_CANCEL = "ADD_CANCEL";

        //public const string STRING_PER = "%";
        //public const string STRING_HIN = "$";
        //public const string STRING_HITO = "＝";
        //public const string TANKA_KETEI_KBN_FORMAT = "A9/.";
        //public const string TEL_FAX_FORMAT = "9-";
        //public const string EMAIL_FORMAT = "Aa9.@_-";

        //public const string HAKKOU = "発行";
        //public const string INNSATU = "印刷";
        //public const string JUCHUU = "受注登録";
        //public const string INSTORE = "インストアコードの発番";
        //public const string INSTORE_SYS = "21";
        //public const string INSTORE_USER = "22";

        //public const string HENPINSHO = "返品書";

        //public const string SOURYO_0 = "サービス";
        //public const string MSG_DAY = "日";
        //public const string MSG_GYO = "行";
        //public const string MSG_RETU = "列";

        //public const string NEXT_ROW = @"\n";

        //public const string SPREAD_SENTAKU_FLG_YES = "YES";
        //public const string SPREAD_SENTAKU_FLG_NO = "NO";

        //public const string HACHU_SU = "発注数";
        //public const string NYUKA_TANKA = "入荷単価";
        //public const string NYUKA_SITEI_DATE = "入荷指定日";
        //public const string HENKOUMAE = "変更前";

        //public const string MSG_KAIKEI_SHIWAKE_SHUUKEI = "会計仕訳集計";
        //public const string MSG_SEIKYU_SHIME_SHORI = "請求締処理";
        //public const string MSG_SHIHARAI_SHIME_SHORI = "支払締処理";
        //public const string MSG_URIKAKE_SHIME_SHORI = "売掛締処理";
        //public const string MSG_KAIKAKE_SHIME_SHORI = "買掛締処理";
        //public const string MSG_URIKAKE_SHIME = "売掛締";
        //public const string MSG_KAIKAKE_SHIME = "買掛締";
        //public const string MSG_FOLDER = "フォルダー";

        //public const string MSG_CSV_OUTPUT = "CSV出力";
        //public const string MSG_CSV_FILE = "CSVファイル";
        //public const string MSG_CSV_FILE_FORMAT = ".csv";

        //public const string MSG_HENSYUU_KEKKA_WO_HANEI = "編集結果を反映";

        //public const string TANKA_KETEI_KBN_1 = "1/";
        //public const string TANKA_KETEI_KBN_2 = "2/";
        //public const string TANKA_KETEI_KBN_3 = "3/";
        //public const string TANKA_KETEI_KBN_4 = "4/";
        //public const string TANKA_KETEI_KBN_M = "M/";
        //public const string TANKA_KETEI_KBN_T = "T/";

        //public const string LOGIN_ID_CHECK = "ﾛｸﾞｲﾝID";
        //public const string SHAIN_CD_CHECK = "社員CD";
        //public const string HINBAN_SHOHIN_CHECK = "品番商品";
        //public const string KBN29_NAME1 = "無形品";
        //public const string KBN29_NAME2 = "ｶﾗｰ又はｻｲｽﾞを設定することが";
        //public const string KBN23_NAME1 = "有効開始日";
        //public const string KBN23_NAME2 = "システム日付";

        //public const string INSATU_SITEI = "印刷指定";
        //public const string CSV_PRINT = "シールCSV出力";
        //public const string FILE_PATH = "該当のフォルダー";
        //public const string COMMON_HELP = "Help";


        //// Public Const REPORT_CHOSEI_NO As String = "@CHOSEI_NO"
        //// Public Const REPORT_DEN_NO As String = "@DenNo"
        //// Public Const REPORT_CENTER_CDLIST As String = "@CENTER_CDLIST"
        //// Public Const REPORT_SOUKO_CDLIST As String = "@SOUKO_CDLIST"
        //// Public Const REPORT_JIGYOBU_CDLIST As String = "@JIGYOBU_CDLIST"
        //// Public Const REPORT_JANCODELIST As String = "@JANCODELIST"

        //// 見積書発行用
        //public const string REPORT_MITSUMORISHO = "見積書";
        //public const string REPORT_MITSUMORISHO_TITLE_1 = "御　見　積　書";
        //public const string REPORT_MITSUMORISHO_TITLE_2 = "御　見　積　書（仮）";
        //public const string REPORT_MITSUMORISHO_TITLE_3 = "御　見　積　書（承認用）";
        //public const string REPORT_MITSUMORISHO_YUKOKIGEN = "見積日より{0}日";
        //public const string REPORT_MITSUMORISHO_KOTEIMOJI_SORIGOUKEI = "粗利合計";

        //public const string REPORT_SYURUI_1 = "1";        // 1の場合、見積書
        //public const string REPORT_SYURUI_2 = "2";        // 2の場合、見　積　書（仮）

        //// 見積種別が物件の場合の印字
        //public const string REPORT_MITSUMORISHO_INJI = "※物件単位の御見積りになりますので明細単位での発注は再見積りとなります";
        //public const string REPORT_MITSUMORISHO_MARK_PERCENT = "%";
        //public const string REPORT_MITSUMORISHO_KOTEIMOJI = "以下余白";
        //public const string REPORT_MITSUMORISHO_KOTEIMOJI_SHIIRE = "仕入({0})";
        //public const string REPORT_MITSUMORISHO_KOTEIMOJI_SORI = "粗利({0})";
        //// セルの背景色
        //public const string REPORT_MITSUMORISHO_BACKCOLOR_SHOUHIN = "247,150,70";
        //public const string REPORT_MITSUMORISHO_BACKCOLOR_SONOTA = "255,255,0";
        //// ピクチャの拡張子
        //public const string REPORT_MITSUMORISHO_IMAGE_EXTENSION = ".jpg";
        //// 固定文字列
        //public const string REPORT_MITSUMORISHO_KOTEIMOJIRETSU_SHOUHINGOUKEI = "商品合計";
        //public const string REPORT_MITSUMORISHO_KOTEIMOJIRETSU_SOURYOUGOUKEI = "送料合計";
        //public const string REPORT_MITSUMORISHO_KOTEIMOJIRETSU_MITSUMORIGOUKEI = "見積合計";

        //// 見積依頼書
        //public const string REPORT_MITUMORIIRAISHO_TITLE = "書";
        //public const string REPORT_MITUMORIIRAISHO_KOURI_TANKA = @"\";

        //// 発注書発行用
        //public const string REPORT_HACHUSHO_TITLE_1 = "発注書";
        //public const string REPORT_HACHUSHO_TITLE_2 = "直送発注書";
        //public const string REPORT_HACHUSHO_HEADER_TEISEI = "訂正版";
        //public const string REPORT_HACHUSHO_YUBIN = "〒";
        //public const string REPORT_HACHUSHO_TEL = "Tel：";

        //// メーカー返品申請書
        //public const string REPORT_HENPIN_SHINSEISHO_TITLE = "返品申請書";

        //// 返品受入書
        //public const string REPORT_HENPIN_UKEIRESHO_TITLE = "返品受入書";
        //public const string REPORT_HENPIN_UKEIRESHO_MARK_ASTERISK = "＊";

        //// 入荷差異リスト
        //public const string REPORT_NYUKA_SAI_LIST_TITLE = "入荷差異リスト";
        //public const string REPORT_NYUKA_SAI_LIST_SEIGOU = "+";

        //// 入荷明細表
        //public const string REPORT_NYUKA_MEISAI_HYOU_TITLE = "【入荷明細表】";
        //public const string REPORT_NYUKA_MEISAI_HYOU_MARK = "－";

        //// 在庫集約表
        //public const string REPORT_ZAIKO_SYUYAKU_TITLE = "【在庫集約表】";
        //public const string REPORT_ZAIKO_SYUYAKU_MARK_ASTERISK = "＊";

        //// 入荷検品表
        //public const string REPORT_NYUKA_KENPIN_HYOU_TITLE = "【店別入荷検品表】";
        //public const string REPORT_NYUKA_KENPIN_HYOU_TITLE1 = "【総量入荷検品表】";
        //public const string REPORT_NYUKA_KENPIN_HYOU_HEADER_TITLE = "【 当日出荷 】";
        //public const string REPORT_NYUKA_KENPIN_HYOU_HEADER_TITLE1 = "【 翌日以降出荷 】";
        //public const string REPORT_NYUKA_KENPIN_HYOU_HYOJI_JUN = "1";
        //public const int REPORT_NYUKA_KENPIN_HYOU_FILL_BIT = 9;
        //public const string REPORT_NYUKA_KENPIN_HYOU_FILL_HEAD = "07";
        //public const string REPORT_NYUKA_KENPIN_HYOU_FILL_FOOT = "0002";

        //// 入金一覧表
        //public const string REPORT_NYUKIN_SHI = "締";
        //public const string REPORT_NYUKIN_NEN = "年";
        //public const string REPORT_NYUKIN_GETU = "月";
        //public const string REPORT_NYUKIN_BI = "日";
        //public const string REPORT_NYUKIN_NENDO = "月度";
        //public const string REPORT_NYUKIN_SANSHI = "三共締";

        //// 支払一覧表
        //public const string REPORT_SIHARAI_TITLE = "支払一覧表";
        //public const string REPORT_SIHARAI_UTIWAKE_TITLE = "支払内訳一覧表";

        //// 支払チェックリスト
        //public const string REPORT_SIHARAI_CHECK_TITLE = "支払チェックリスト";

        //// 入金チェックリスト
        //public const string REPORT_NYUKIN_CHECK_TITLE = "入金チェックリスト";

        //// 入金一覧表
        //public const string REPORT_NYUKIN_TITLE = "入金一覧表";
        //public const string REPORT_NYUKIN_UTIWAKE_TITLE = "入金内訳一覧表";
        //public const string REPORT_NYUKINBUSHO_NAME = "部署：";

        //// 買掛残高一覧表
        //public const string REPORT_KAIKAKE_HYOU_TITLE = "買掛残高一覧表";
        //// 買掛元帳
        //public const string REPORT_KAIKAKE_TITLE = "買掛元帳";
        //// 売掛元帳
        //public const string REPORT_URIKAKE_TITLE = "売掛元帳";
        //// 売掛残高一覧表
        //public const string REPORT_URIKAKE_HYOU_TITLE = "売掛残高一覧表";

        //// 請求書発行用
        //public const string REPORT_SEIKYUUSHO = "請求書";
        //public const string REPORT_ONTYUU = "御中";
        //public const string REPORT_MEISAISHO = "明細書";
        //public const string REPORT_SAMABUN = "様分";
        //public const string REPORT_TOIAWASE = "≪問合せ先≫";
        //public const string REPORT_YMD = "締　下記の通り御請求申し上げます。";
        //public const string REPORT_SHIME = "　締";
        //public const string REPORT_SEIKYU_NO = "No.";
        //public const string REPORT_SEIYU_PHONE = "ＴＥＬ　";
        //public const string REPORT_SEIKYU_YUBIN = "〒";
        //public const string REPORT_GINKOU = "【取引銀行】";

        //// 受取手形明細表
        //public const string REPORT_SAKUJYO_TITLE = "受取手形明細表（期日別）";
        //public const string REPORT_SAKUJYO_TITLE1 = "受取手形明細表（売掛先別）";
        //public const string REPORT_SAKUJYO_KINITUKEI = "< 期日計 >";
        //public const string REPORT_SAKUJYO_KINITUKEI1 = "< 売掛先計 >";
        //public const string REPORT_SAKUJYO_KINITUKEI_SIGN = "枚)";
        //public const string REPORT_SAKUJYO_KINGAKU_SUM = "< 合計 >";

        //// 支払手形明細表
        //public const string REPORT_SIHARAI_SAKUJYO_TITLE = "支払手形明細書（期日別）";
        //public const string REPORT_SIHARAI_SAKUJYO_TITLE1 = "支払手形明細書（買掛先別）";
        //public const string REPORT_SIHARAI_SAKUJYO_KINITUKEI = "< 期日計 >";
        //public const string REPORT_SIHARAI_SAKUJYO_KINITUKEI1 = "< 買掛先計 >";
        //public const string REPORT_SIHARAI_SAKUJYO_KINITUKEI_SIGN = "枚)";
        //public const string REPORT_SIHARAI_SAKUJYO_KINGAKU_SUM = "< 合計 >";


        //// 回収予定実績表
        //public const string REPORT_KAISYU_TITLE = "回収予定実績表";
        //public const string REPORT_HOSI = "*";
        //public const string REPORT_BUSHO_NAME = "部署　　：";
        //public const string REPORT_TANTO_NAME = "営業担当：";



        //// バッチユーザーコード
        //public const string SYS_CTRL_CD_SYSTEM = "AT_Cd_System";
        //// Public Const SYS_CTRL_CD_SYSTEM_TEMP As String = "Cd_System_Temp"

        //// 各種マスタ取込の受信フォルダ各種
        //public const string SYS_CTRL_PATH_MST_RECV = "Path_MstRecv";
        //public const string SYS_CTRL_PATH_MST_RECVBACK = "Path_MstRecvBack";
        //public const string SYS_CTRL_PATH_MST_RECVTEMP = "Path_MstRecvTemp";

        //// RAA連携の受信フォルダ各種
        //public const string SYS_CTRL_PATH_RAA_RECV = "Path_RaaRecv";
        //public const string SYS_CTRL_PATH_RAA_RECVBACK = "Path_RaaRecvBack";
        //public const string SYS_CTRL_PATH_RAA_RECVTEMP = "Path_RaaRecvTemp";

        //// RAA連携の送信フォルダ各種
        //public const string SYS_CTRL_PATH_RAA_SEND = "Path_RaaSend";
        //public const string SYS_CTRL_PATH_RAA_SENDBACK = "Path_RaaSendBack";


        //// INDEX選択の検索件数
        //public const string SYS_CTRL_INDEX_SENTAKU = "IndexSentakuSelMaxcount";


        //// タイトルバーの規定値
        //public const string SYS_CTRL_APPNAME = "AppName";
        //// アクティーブのタブの背景色
        //public const string SYS_CTRL_TABSELBACKCOLOR = "TabSelBackColor";
        //// アクティーブタブのフォント色
        //public const string SYS_CTRL_TABSELFONTCOLOR = "TabSelFontColor";

        //// アクティーブのタブの背景色
        //public const string SYS_CTRL_TABBACKCOLOR = "TabBackColor";
        //// アクティーブタブのフォント色
        //public const string SYS_CTRL_TABFONTCOLOR = "TabFontColor";
        //public const string SYS_CTRL_MENUBTNBACKCOLOR = "MenuBtnBackColor";

        //public const string SYS_CTRL_LABELFONTCOLORBLUE = "LabelFontBlue";
        //public const string SYS_CTRL_MAXFAVOURITEBTNCOUNT = "MaxFavouriteBtnCount";

        //// 日付コントロールの最小日付
        //public const string SYS_CTRL_SYSTEM_MIN_DATE = "SystemMinDate";
        //// 入力項目の背景色
        //public const string SYS_CTRL_ENTBACKCOLOR = "EntBackColor";
        //// 入力項目の前景
        //public const string SYS_CTRL_ENTFORECOLOR = "EntForeColor";
        //// ｴﾗｰ時の背景色
        //public const string SYS_CTRL_ERRBACKCOLOR = "ErrBackColor";
        //// 表示項目の背景色Start
        //public const string SYS_CTRL_READBACKCOLOR_S = "ReadBackColor_S";
        //// 表示項目の背景色End
        //public const string SYS_CTRL_READBACKCOLOR_E = "ReadBackColor_E";
        //// 表示項目の前景色
        //public const string SYS_CTRL_READFORECOLOR = "ReadForeColor";
        //// 見出し項目の背景色Start
        //public const string SYS_CTRL_VIEWBACKCOLOR_S = "ViewBackColor_S";
        //// 見出し項目の背景色End
        //public const string SYS_CTRL_VIEWBACKCOLOR_E = "ViewBackColor_E";
        //// 見出し項目の前景色
        //public const string SYS_CTRL_VIEWFORECOLOR = "ViewForeColor";
        //// タイトルの背景色Start
        //public const string SYS_CTRL_TITLEBACKCOLOR_S = "TitleBackColor_S";
        //// タイトルの背景色End
        //public const string SYS_CTRL_TITLEBACKCOLOR_E = "TitleBackColor_E";
        //// タイトルの前景色
        //public const string SYS_CTRL_TITLEFORECOLOR = "TitleForeColor";
        //// フォーカス時の背景色
        //public const string SYS_CTRL_FOCUSBACKCOLOR = "FocusBackColor";
        //// フォーカス時の前景色
        //public const string SYS_CTRL_FOCUSFORECOLOR = "FocusForeColor";
        //// キー部の背景色Start
        //public const string SYS_CTRL_KEYAREABACKCOLOR_S = "KeyAreaBackColor_S";
        //// キー部の背景色End
        //public const string SYS_CTRL_KEYAREABACKCOLOR_E = "KeyAreaBackColor_E";
        //// 明細部の背景色Start
        //public const string SYS_CTRL_DETAILAREABACKCOLOR_S = "DetailAreaBackColor_S";
        //// 明細部の背景色End
        //public const string SYS_CTRL_DETAILAREABACKCOLOR_E = "DetailAreaBackColor_E";
        //// フォーカス時の背景色［スプレッド用］
        //public const string SYS_CTRL_SPR_FOCUSBACKCOLOR = "SprFocusBackColor";
        //// フォーカス時の前景色 ［スプレッド用］
        //public const string SYS_CTRL_SPR_FOCUSFORECOLOR = "SprFocusForeColor";
        //// Popup検索ラベルの前景色
        //public const string SYS_CTRL_LINKFORECOLOR = "LinkForeColor";
        //// 項目がディセーブル時の背景色
        //public const string SYS_CTRL_DISABACKCOLOR = "DisaBackColor";
        //// 項目がディセーブル時の前景色
        //public const string SYS_CTRL_DISAFORECOLOR = "DisaForeColor";
        //// ﾌｫｰﾑの背景色Start
        //public const string SYS_CTRL_FORMBACKCOLOR_S = "FormBackColor_S";
        //// ﾌｫｰﾑの背景色End
        //public const string SYS_CTRL_FORMBACKCOLOR_E = "FormBackColor_E";
        //// 出荷コントロール：引当できないﾋﾟｯｷﾝｸﾞ
        //public const string SYS_CTRL_HIKIATEFUKACOLOR = "SyukaIntounaiColor";
        //// 出荷コントロール：取り消されたﾋﾟｯｷﾝｸﾞ
        //public const string SYS_CTRL_SYUKACANCELCOLOR = "SyukaCancelColor";
        //// 出荷コントロール：ﾋﾟｯｷﾝｸﾞとﾊﾟｯｷﾝｸﾞのｱﾝﾏｯﾁない
        //public const string SYS_CTRL_SYUKAANMATINAICOLOR = "SyukaAnmatiNaiColor";
        //// 出荷コントロール：ﾋﾟｯｷﾝｸﾞとﾊﾟｯｷﾝｸﾞのｱﾝﾏｯﾁ有り
        //public const string SYS_CTRL_SYUKAANMATIARICOLOR = "SyukaAnmatiAriColor";
        //// '靴のｱｲﾃﾑｺｰﾄﾞ
        //// Public Const SYS_CTRL_AITEMU As String = "AITEMU"

        //// 'エラーラベルの背景色(E)Start
        //// Public Const SYS_CTRL_EFORMBACKCOLOR_S As String = "EFormBackColor_S"
        //// 'エラーラベルの背景色(E)End
        //// Public Const SYS_CTRL_EFORMBACKCOLOR_E As String = "EFormBackColor_E"

        //// Public Const SYS_CTRL_QFORMBACKCOLOR_S As String = "QFormBackColor_S"                   'エラーラベルの背景色(Q)Start
        //// Public Const SYS_CTRL_QFORMBACKCOLOR_E As String = "QFormBackColor_E"                   'エラーラベルの背景色(Q)End
        //// Public Const SYS_CTRL_WFORMBACKCOLOR_S As String = "WFormBackColor_S"                   'エラーラベルの背景色(W)Start
        //// Public Const SYS_CTRL_WFORMBACKCOLOR_E As String = "WFormBackColor_E"                   'エラーラベルの背景色(W)End
        //// Public Const SYS_CTRL_IFORMBACKCOLOR_S As String = "IFormBackColor_S"                   'エラーラベルの背景色(I)Start
        //// Public Const SYS_CTRL_IFORMBACKCOLOR_E As String = "IFormBackColor_E"                   'エラーラベルの背景色(I)End

        //// 'INDEX検索の検索件数
        //// Public Const SYS_CTRL_INDEXSELMAXCOUNT As String = "IndexSelMaxcount"

        //// 'カラー検索の検索件数
        //// Public Const SYS_CTRL_COLORSELMAXCOUNT As String = "ColorSelMaxcount"

        //// 'サイズ検索の検索件数
        //// Public Const SYS_CTRL_SIZESELMAXCOUNT As String = "SizeSelMaxcount"

        //// 'ブランド検索の検索件数
        //// Public Const SYS_CTRL_BRANDSELMAXCOUNT As String = "BrandSelMaxcount"

        //// 'メーカー検索の検索件数
        //// Public Const SYS_CTRL_MAKERSELMAXCOUNT As String = "MakerSelMaxcount"

        //// 'ロケーション検索の検索件数
        //// Public Const SYS_CTRL_LOCATIONSELMAXCOUNT As String = "LocationSelMaxcount"

        //// '企業検索の検索件数
        //// Public Const SYS_CTRL_KIGYOSELMAXCOUNT As String = "KigyoSelMaxcount"

        //// '在庫区分検索の検索件数
        //// Public Const SYS_CTRL_ZAIKOKBNSELMAXCOUNT As String = "ZaikokbnSelMaxcount"

        //// '仕入先検索の検索件数
        //// Public Const SYS_CTRL_SHIRESAKISELMAXCOUNT As String = "ShiresakiSelMaxcount"

        //// '社員検索の検索件数
        //// Public Const SYS_CTRL_SHAINSELMAXCOUNT As String = "ShainSelMaxcount"

        //// '所有権検索の検索件数
        //// Public Const SYS_CTRL_SHOYUKENSELMAXCOUNT As String = "ShoyukenSelMaxcount"

        //// '商品分類検索の検索件数
        //// Public Const SYS_CTRL_SHOHIN_BUNRUISELMAXCOUNT As String = "Shohin_BunrulSelMaxcount"

        //// '請求先検索の検索件数
        //// Public Const SYS_CTRL_KAIKEISELMAXCOUNT As String = "KaikeiSelMaxcount"

        //// '倉庫検索の検索件数
        //// Public Const SYS_CTRL_SOKOSELMAXCOUNT As String = "SokoSelMaxcount"

        //// '展示会検索の検索件数
        //// Public Const SYS_CTRL_TENJIKAISELMAXCOUNT As String = "TenjikaiSelMaxcount"

        //// '得意先検索の検索件数
        //// Public Const SYS_CTRL_TOKUISAKISELMAXCOUNT As String = "TokuisakiSelMaxcount"

        //// '納品先検索の検索件数
        //// Public Const SYS_CTRL_NOHINSAKISELMAXCOUNT As String = "NohinsakiSelMaxcount"

        //// '発注先検索の検索件数
        //// Public Const SYS_CTRL_HACHUSAKISELMAXCOUNT As String = "HachusakiSelMaxcount"

        //// '販売戦略検索の検索件数
        //// Public Const SYS_CTRL_HANBAI_SENRYAKUSELMAXCOUNT As String = "Hanbai_SenryakuSelMaxcount"

        //// '品番検索の検索件数
        //// Public Const SYS_CTRL_HINBANSELMAXCOUNT As String = "HinbanSelMaxcount"

        //// '部署検索の検索件数
        //// Public Const SYS_CTRL_BUSHOSELMAXCOUNT As String = "BushoSelMaxcount"

        //// '見積照会検索の検索件数
        //// Public Const SYS_CTRL_MITUMORISHOUKAISELMAXCOUNT As String = "MitumorishoukaiSelMaxcount"

        //// '受払照会検索の検索件数
        //// Public Const SYS_CTRL_UKENARASHOUKAISELMAXCOUNT As String = "UkenaraShoukaiSelMaxcount"

        //// '受入照会検索の検索件数
        //// Public Const SYS_CTRL_UKEIRESYOUKASELMAXCOUNT As String = "UkeiresyoukaiSelMaxcount"

        //// '発注照会検索の検索件数
        //// Public Const SYS_CTRL_HACHUSHOUKAISELMAXCOUNT As String = "HarchushoukaiSelMaxcount"

        //// '売上照会検索の検索件数
        //// Public Const SYS_CTRL_URIAGESHOUKAISELMAXCOUNT As String = "UriageSyokaiMaxcount"

        //// '売掛照会検索の検索件数
        //// Public Const SYS_CTRL_URIKAKESHOUKAISELMAXCOUNT As String = "UrikakeshoukaiSelMaxcount"

        //// '買掛照会検索の検索件数
        //// Public Const SYS_CTRL_KAIKAKESHOUKAISELMAXCOUNT As String = "KaikakeshoukaiSelMaxcount"

        //// '売掛元帳照会の検索件数
        //// Public Const SYS_CTRL_URIKAKEMOTOSHOUKAISELMAXCOUNT As String = "UrikakemotoshoukaiSelMaxcount"

        //// '買掛元帳照会の検索件数
        //// Public Const SYS_CTRL_KAIKAKEMOTOSHOUKAISELMAXCOUNT As String = "KaikakemotoshoukaiSelMaxcount"

        //// '仕入照会の検索件数
        //// Public Const SYS_CTRL_SHIRESHOUKAISELMAXCOUNT As String = "ShireshoukaiSelMaxcount"

        //// '入金照会の検索件数
        //// Public Const SYS_CTRL_NYUKINSHOUKAISELMAXCOUNT As String = "NyukinshoukaiSelMaxcount"
        //// '支払照会の検索件数
        //// Public Const SYS_CTRL_SHIHARAISHOUKAISELMAXCOUNT As String = "ShiharaishoukaiSelMaxcount"

        //// '受取手形明細照会の検索件数
        //// Public Const SYS_CTRL_UKETORITEGATASHOUKAIMAXCOUNT As String = "UketoritegataShoukaiMaxcount"
        //// '支払手形明細照会の検索件数
        //// Public Const SYS_CTRL_SHIHARAITEGATASHOUKAIMAXCOUNT As String = "ShiharaitegataShoukaiMaxcount"

        //// '回収予定実績照会の検索件数
        //// Public Const SYS_CTRL_KAISYUYOTEISHOUKAIMAXCOUNT As String = "KaisyuyoteiShoukaiMaxcount"


        //// '入出力管理の検索件数
        //// Public Const SYS_CTRL_NYUSYUSOUKOKENRIMAXCOUNT As String = "NyusyusoukokenriSelMaxcount"

        //// 'MD選択の検索件数
        //// Public Const SYS_CTRL_URIAGE_SENTAKU As String = "UriageSentakuSelMaxcount"

        //// '在庫選択の検索件数
        //// Public Const SYS_CTRL_ZAIKO_SENTAKU As String = "ZaikoSentakuSelMaxcount"

        //// 'カラーサイズ選択の検索件数
        //// Public Const SYS_CTRL_COLOR_SIZE_SENTAKU As String = "ColorSizeSentakuSelMaxcount"

        //// 'マトリックス選択の検索件数
        //// Public Const SYS_CTRL_MATRIX_SENTAKU As String = "MatrixSentakuSelMaxcount"

        //// 'INDEX選択の検索件数
        //// Public Const SYS_CTRL_INDEX_SENTAKU As String = "IndexSentakuSelMaxcount"

        //// '受注変更履歴の検索件数
        //// Public Const SYS_CTRL_HENCOU_RIREKI As String = "HencouRirekiSelMaxcount"

        //// '入荷宣言の検索件数
        //// Public Const SYS_CTRL_NYUKASANGENMAXCOUNT As String = "NyukaSangenSelMaxcount"

        //// '入荷コントロールの検索件数
        //// Public Const SYS_CTRL_NYUKAKONTORORUMAXCOUNT As String = "NyukaKonToRoruMaxcount"

        //// '入荷入力の検索件数
        //// Public Const SYS_CTRL_NYUKANYURYOKUMAXCOUNT As String = "NyukaNuryokuMaxcount"

        //// '在庫照会_倉庫別の検索件数
        //// Public Const SYS_CTRL_ZAIKOSHOKAI_SOKO_MAXCOUNT As String = "ZaikoShokaiSokoMaxcount"
        //// '在庫照会_商品別の検索件数
        //// Public Const SYS_CTRL_ZAIKOSHOKAI_SHOHIN_MAXCOUNT As String = "ZaikoShokaiShohinMaxcount"
        //// *********************************************************************
        //// スプレッド
        //// *********************************************************************
        //// 列のヘッダ
        //public const string SYS_CTRL_SPR_COL_HEADER_BACKCOLOR_S = "SprColHeaderBackColor_S";
        //public const string SYS_CTRL_SPR_COL_HEADER_BACKCOLOR_E = "SprColHeaderBackColor_E";
        //public const string SYS_CTRL_SPR_COL_HEADER_FORECOLOR = "SprColHeaderForeColor";
        //// 行のヘッダ
        //public const string SYS_CTRL_SPR_ROW_HEADER_BACKCOLOR_S = "SprRowHeaderBackColor_S";
        //public const string SYS_CTRL_SPR_ROW_HEADER_BACKCOLOR_E = "SprRowHeaderBackColor_E";
        //public const string SYS_CTRL_SPR_ROW_HEADER_FORECOLOR = "SprRowHeaderForeColor";
        //// コーナー
        //public const string SYS_CTRL_SPR_CORNER_HEADER_BACKCOLOR_S = "SprCornerHeaderBackColor_S";
        //public const string SYS_CTRL_SPR_CORNER_HEADER_BACKCOLOR_E = "SprCornerHeaderBackColor_E";
        //public const string SYS_CTRL_SPR_CORNER_HEADER_FORECOLOR = "SprCornerHeaderForeColor";
        //// 編集のセル
        //public const string SYS_CTRL_SPR_FOCUSE_BACKCOLOR = "SprFocusBackColor";
        //public const string SYS_CTRL_SPR_FOCUSE_FORECOLOR = "SprFocusForeColor";

        //public const string SYS_CTRL_SPR_READ_BACKCOLOR = "SprReadBackColor";
        //public const string SYS_CTRL_SPR_READ_FORECOLOR = "SprReadForeColor";

        //// 'パスワードの有効期間
        //// Public Const SYS_CTRL_PASSWORDEFFECTIVEDAYS As String = "PasswordEffectiveDays"

        //// '・状態『未着手』で行の色
        //// Public Const SYS_CTRL_SPREAD_ROWCOLOR_WILL As String = "SprBackColor_Will"
        //// '・状態『実施中・完了』で行の色
        //// Public Const SYS_CTRL_SPREAD_ROWCOLOR_FINISH As String = "SprBackColor_Finish"

        //public const string SYS_CTRL_CODE_CAN_INPUT_CHAR = "CodeCanInputChar";
        //// '・出荷指示取込パス
        //// Public Const SYS_CTRL_SYUKKASIJIPATH As String = "SyukkaSijiPath"

        //// Public Const SYS_CTRL_CSVFILEPATH As String = "CsvFilePath"
        //// Public Const SYS_CTRL_CSVFILENAME As String = "CsvFileName"

        //// Public Const SYS_CTRL_TORIKOMIDATE As String = "TorikomiDate"

        //// Public Const SYS_CTRL_SHIRESOURYO As String = "ShireSouryo"

        //// 'EOS出力フォルダ
        //// Public Const SYS_CTRL_EOSSENCSVFILEPATH As String = "EosSenCsvFilePath"

        //// 'お知らせメッセージの長さ
        //// Public Const SYS_CTRL_OSHIRASEMSGLEN As String = "OshiraseMsgLen"

        //// Public Const SYS_CTRL_TAGEXEFILEPATH As String = "TagExeFilePath"

        //// Public Const SYS_CTRL_TAGEXEBATPATH As String = "TagExeBatPath"

        //// Public Const SYS_CTRL_TAGEXEFILENAME As String = "TagExeFileName"
        //// '進行表取込パス
        //// Public Const SYS_CTRL_IDDPI01FILENAME As String = "IDDPI01FileName"
        //// '進行表取込ファイル名称
        //// Public Const SYS_CTRL_IDDPI01PATH As String = "IDDPI01Path"


        //// '棚番
        //// Public Const SYS_CTRL_DEFAULTTMPLOC_DENTEISEI As String = "DefaultTmpLoc_DenTeisei"
        //// Public Const SYS_CTRL_DEFAULTTMPLOC_UKEIRE As String = "DefaultTmpLoc_Ukeire"
        //// Public Const SYS_CTRL_DEFAULTTMPLOC_SIIRENYUKA As String = "DefaultTmpLoc_SiireNyuka"
        //// Public Const SYS_CTRL_DEFAULTTMPLOC_IDONYUKO As String = "DefaultTmpLoc_IdoNyuko"
        //// Public Const SYS_CTRL_DEFAULTTMPLOC_PACKING As String = "DefaultTmpLoc_Packing"
        //// Public Const SYS_CTRL_DEFAULTTMPLOC_SHUKACHUSHI As String = "DefaultTmpLoc_ShukkaChushi"

        //// '伝票種別CD
        //// Public Const SYS_CTRL_HENPINSHOSHUBETU As String = "HenpinShoShubetu"

        //// 'ADD START F.RICK 2014/2/6
        //// 'お知らせのデフォルトの有効期限（日）
        //// Public Const SYS_CTRL_NOTIFY_YUKO_DAYS As String = "NotifyYukoDays"

        //// ローカルに帳票を出力する時のデイレクトリ
        //public const string SYS_CTRL_LOCAL_REPORT_SAVE_PATH = "LocalReportSavePath";

        //// 'ADD END F.RICK 2014/2/6

        //// '印鑑画像の保存パス
        //// Public Const SYS_CTRL_INKAN As String = "Inkan"
        //// キャンセルデータのグレー色
        //public const string SYS_CTRL_CANCELDATACORLOR = "CancelDataColor";
        //// '赤色太字
        //// Public Const SYS_CTRL_TANKA_COLOR As String = "TankaColor"
        //// 期首月
        //public const string SYS_CTRL_KISHUGETUDO = "Kishugetudo";
        //// 売掛締日
        //public const string SYS_CTRL_URIKAKESHIMEBI = "UrikakeShimebi";
        //// 買掛締日
        //public const string SYS_CTRL_KAIKAKESHIMEBI = "KaikakeShimebi";
        //// '入金
        //// Public Const SYS_CTRL_NYUKIN As String = "Nyukin"
        //// '支払
        //// Public Const SYS_CTRL_SHIHARAI As String = "Shiharai"
        //// '発注
        //// Public Const SYS_CTRL_HACHU As String = "hachu"
        //// '売上
        //// Public Const SYS_CTRL_URIAGE As String = "uriage"
        //// '商品
        //// Public Const SYS_CTRL_SYOUHIN As String = "syouhin"

        //// '仕訳区分
        //// Public Const SYS_CTRL_SHIWAKEKBN_URIAGE As String = "ShiwakeKBN-Uriage"
        //// Public Const SYS_CTRL_SHIWAKEKBN_SHIRE As String = "ShiwakeKBN-Shiire"
        //// Public Const SYS_CTRL_SHIWAKEKBN_ZAIKO As String = "ShiwakeKBN-Zaiko"
        //// Public Const SYS_CTRL_SHIWAKEKBN_ZAIKOCHOSEI As String = "ShiwakeKBN-Zaikochosei"
        //// Public Const SYS_CTRL_SHIWAKEKBN_NYUKIN As String = "ShiwakeKBN-Nyukin"
        //// Public Const SYS_CTRL_SHIWAKEKBN_SHIHARAI As String = "ShiwakeKBN-Shiharai"
        //// Public Const SYS_CTRL_SHIWAKEKBN_SHOHIZEI_U As String = "ShiwakeKBN-Shohizei_U"
        //// Public Const SYS_CTRL_SHIWAKEKBN_SHOHIZEI_K As String = "ShiwakeKBN-Shohizei_K"

        //// '仕訳部門コード
        //// Public Const SYS_CTRL_SHIWAKEKBN_BUMON_CD As String = "ShiwakeBumonCd"


        //// 'JoyPlusアプリケーションｓｖ1
        //// Public Const SYS_CTRL_JOYPlUSAPSERVERIP1 = "JoyPlusApServerIP1"
        //// '三共伝（納品書）
        //// Public Const SYS_CTRL_DFNOHINSHO = "DfNohinSho"
        //// '三共伝（返品書）
        //// Public Const SYS_CTRL_DFHENPINSHO = "DfHenpinSho"
        //// '三共伝（消費税）
        //// Public Const SYS_CTRL_DFSHOHIZEISHO = "DfShohizeiSho"
        //// 伝発発行コマンド出力共有フォルダ
        //public const string SYS_CTRL_DENPATUCTLFOLDER1 = "DenpatuCtlFolder1";
        //public const string SYS_CTRL_DENPATUCTLFOLDER2 = "DenpatuCtlFolder2";

        //// '出荷予定日算出値
        //// Public Const SYS_CTRL_CHOKUHACHUSHUKABI As String = "ChokuHachuShukabi"

        //// '卸単価倍率
        //// Public Const SYS_CTRL_OROSHITANKABAIRITSU As String = "OroshiTankaBairitsu"

        //// '自社郵便番号
        //// Public Const SYS_CTRL_COMPANY_POSTAL As String = "Company_Postal"
        //// '自社住所
        //// Public Const SYS_CTRL_COMPANY_ADDR As String = "Company_Addr"

        //// '出荷期間(Add 2014/08/23 bph)
        //// Public Const SYS_CTRL_SHUKAKIGEN As String = "ShukaKigen"

        //// '印鑑拡張子
        //// Public Const SYS_CTRL_INKAN_EXTENSION As String = "Inkan_Extension"

        //// '請求書に印字する取引銀行（銀行名）
        //// Public Const SYS_CTRL_COMPANY_BANK_NAME As String = "Company_Bank_Name"

        //// '請求書に印字する取引銀行（支店名）
        //// Public Const SYS_CTRL_COMPANY_BANK_BRANCH As String = "Company_Bank_Branch"

        //// '請求書に印字する取引銀行（口座）
        //// Public Const SYS_CTRL_COMPANY_BANK_ACCOUNT As String = "Company_Bank_Account"

        //// Public Const SYS_CTRL_DEFAULTTMPLOC_ZAIKOCHOSEI As String = "DefaultTmpLoc_ZaikoChosei"

        //// Public Const SYS_CTRL_GENKO_KAZEI_KBN As String = "GenkoKazeiKubun"

        //// '入金仕訳 
        //// Public Const SYS_CTRL_NYUKIN_SHIWAKE As String = "NyukinShiwake"
        //// '支払仕訳 
        //// Public Const SYS_CTRL_SHIHARAI_SHIWAKE As String = "ShiharaiShiwake"


        //// '手形情報有無 
        //// Public Const TEGATA_JYOHO_UMU_0 As String = "0"          '無 
        //// Public Const TEGATA_JYOHO_UMU_1 As String = "1"          '有 

        //// '商品CSV分割
        //// Public Const SHOHIN_CSV_BUNKATU As String = "CsvBunkatuPath"

        //// '----------------
        //// '日付チェック
        //// '----------------
        //// '見積
        //// Public Const SYS_CTRL_DATE_MITUMORI_DATE As String = "date_mitumori_date"
        //// '受注
        //// Public Const SYS_CTRL_DATE_JUCHU_DATE As String = "date_juchu_date"
        //// Public Const SYS_CTRL_DATE_JUCHU_SHUKAYOTEI As String = "date_juchu_shukayotei"
        //// Public Const SYS_CTRL_DATE_JUCHU_OKURIJO As String = "date_juchu_okurijo"
        //// Public Const SYS_CTRL_DATE_JUCHU_URIAGEYOTEI As String = "date_juchu_uriageyotei"
        //// Public Const SYS_CTRL_DATE_JUCHU_KIBONOKI As String = "date_juchu_kibonoki"
        //// Public Const SYS_CTRL_DATE_JUCHU_SHOKUDASHI As String = "date_juchu_shokudashi"
        //// '発注
        //// Public Const SYS_CTRL_DATE_HACHU_NYUKA_SHITEI As String = "date_hachu_nyuka_shitei"
        //// Public Const SYS_CTRL_DATE_HACHU_NYUKA_YOTEI As String = "date_hachu_nyuka_yotei"
        //// Public Const SYS_CTRL_DATE_HACHU_SHUKA_YOTEI As String = "date_hachu_shuka_yotei"
        //// Public Const SYS_CTRL_DATE_HACHU_YOTEINOKI As String = "date_hachu_yoteinoki"
        //// '入荷
        //// Public Const SYS_CTRL_DATE_NYUKA_DENDATE As String = "date_nyuka_dendate"
        //// Public Const SYS_CTRL_DATE_NYUKA_DATE As String = "date_nyuka_date"
        //// '受入
        //// Public Const SYS_CTRL_DATE_UKEIRE_YOTEI As String = "date_ukeire_yotei"
        //// Public Const SYS_CTRL_DATE_UKEIRE_URIAGE_DATE As String = "date_ukeire_uriage_date"
        //// Public Const SYS_CTRL_DATE_UKEIRE_UKETUKE As String = "date_ukeire_uketuke"
        //// Public Const SYS_CTRL_DATE_UKEIRE_SHUKA As String = "date_ukeire_shuka"
        //// '売上
        //// Public Const SYS_CTRL_DATE_URIAGE_CHOKUSO As String = "date_uriage_chokuso"
        //// Public Const SYS_CTRL_DATE_URIAGE_CHOKUSO_DEN As String = "date_uriage_chokuso_den"
        //// Public Const SYS_CTRL_DATE_URIAGE_ZAIKO As String = "date_uriage_zaiko"
        //// Public Const SYS_CTRL_DATE_URIAGE_SHUKA As String = "date_uriage_shuka"
        //// Public Const SYS_CTRL_DATE_URIAGE_CHOSEI As String = "date_uriage_chosei"
        //// Public Const SYS_CTRL_DATE_URIAGE_MD As String = "date_uriage_md"
        //// Public Const SYS_CTRL_DATE_URIAGE_CHOKUSO_HENPIN As String = "date_uriage_chokuso_henpin"
        //// Public Const SYS_CTRL_DATE_URIAGE_CHOKUSO_HENPIN_DEN As String = "date_uriage_chokuso_henpin_den"
        //// Public Const SYS_CTRL_DATE_URIAGE_HENPIN As String = "date_uriage_henpin"
        //// '仕入
        //// Public Const SYS_CTRL_DATE_SHIRE_CHOSEI As String = "date_shire_chosei"
        //// Public Const SYS_CTRL_DATE_SHIRE_HENPIN As String = "date_shire_henpin"
        //// Public Const SYS_CTRL_DATE_SHIRE_HENPIN_SHUKA As String = "date_shire_henpin_shuka"
        //// Public Const SYS_CTRL_DATE_SHIRE_CHOSEI_DEN As String = "date_shire_chosei_den"
        //// Public Const SYS_CTRL_DATE_SHIRE_HENPIN_DEN As String = "date_shire_henpin_den"
        //// '移動
        //// Public Const SYS_CTRL_DATE_IDO_SHUKA_YOTEI As String = "date_ido_shuka_yotei"
        //// Public Const SYS_CTRL_DATE_IDO_NYUKA_YOTEI As String = "date_ido_nyuka_yotei"
        //// '入金
        //// Public Const SYS_CTRL_DATE_NYUKIN As String = "date_nyukin"
        //// '支払
        //// Public Const SYS_CTRL_DATE_SHIHARAI As String = "date_shiharai"

        //// '消費税入力（売掛・買掛）
        //// Public Const SYS_CTRL_DATE_SHOHIZEI_U_KEIJO As String = "date_shohizei_u_keijo"
        //// Public Const SYS_CTRL_DATE_SHOHIZEI_U_KANREN_H As String = "date_shohizei_u_kanren_h"
        //// Public Const SYS_CTRL_DATE_SHOHIZEI_U_KANREN_D As String = "date_shohizei_u_kanren_d"
        //// Public Const SYS_CTRL_DATE_SHOHIZEI_K_KEIJO As String = "date_shohizei_k_keijo"
        //// Public Const SYS_CTRL_DATE_SHOHIZEI_K_KANREN_H As String = "date_shohizei_k_kanren_h"
        //// Public Const SYS_CTRL_DATE_SHOHIZEI_K_KANREN_D As String = "date_shohizei_k_kanren_d"




        //public const int FUNCTIONKEY_F1 = 1;
        //public const int FUNCTIONKEY_F2 = 2;
        //public const int FUNCTIONKEY_F3 = 3;
        //public const int FUNCTIONKEY_F4 = 4;
        //public const int FUNCTIONKEY_F5 = 5;
        //public const int FUNCTIONKEY_F6 = 6;
        //public const int FUNCTIONKEY_F7 = 7;
        //public const int FUNCTIONKEY_F8 = 8;
        //public const int FUNCTIONKEY_F9 = 9;
        //public const int FUNCTIONKEY_F10 = 10;
        //public const int FUNCTIONKEY_F11 = 11;
        //public const int FUNCTIONKEY_F12 = 12;


        //public const string RESOURCE_PRGID_COMMON = "COMMON";

        //// ファクションキー
        //public const int RESOURCE_COMMON_F1_CLOSE = 1;
        //public const int RESOURCE_COMMON_F2_CLEAR = 34;
        //public const int RESOURCE_COMMON_F3_INQUIRY = 3;
        //public const int RESOURCE_COMMON_F4_OUTPUT = 36;
        //public const int RESOURCE_COMMON_F5_READER = 26;
        //public const int RESOURCE_COMMON_F12_SEARCH = 4;
        //public const int RESOURCE_COMMON_F12_RUN = 5;
        //public const int RESOURCE_COMMON_F3_ADDLINE = 6;
        //public const int RESOURCE_COMMON_F4_DELLINE = 7;
        //public const int RESOURCE_COMMON_F9_NEW = 8;
        //public const int RESOURCE_COMMON_F10_UPD = 9;
        //public const int RESOURCE_COMMON_F10_SEARCH = 4;
        //public const int RESOURCE_COMMON_F11_RETURN = 35;
        //public const int RESOURCE_COMMON_F11_DEL = 10;
        //public const int RESOURCE_COMMON_F2_RESET = 11;
        //public const int RESOURCE_COMMON_F12_NEW = 12;
        //public const int RESOURCE_COMMON_F12_UPD = 13;
        //public const int RESOURCE_COMMON_F12_DEL = 14;
        //public const int RESOURCE_COMMON_F7_PRE = 15;
        //public const int RESOURCE_COMMON_F8_NEXT = 16;
        //public const int RESOURCE_COMMON_F3_INSERTLINE = 17;
        //public const int RESOURCE_COMMON_F6_ALLDEL = 18;
        //public const int RESOURCE_COMMON_F8_IMPORT = 19;
        //public const int RESOURCE_COMMON_F9_FORMAT = 20;
        //public const int RESOURCE_COMMON_F10_OUTPUT = 21;
        //public const int RESOURCE_COMMON_F12_DECIDE = 22;
        //public const int RESOURCE_COMMON_F7_AFTER_PAGE = 23;
        //public const int RESOURCE_COMMON_F8_NEXT_PAGE = 24;
        //public const int RESOURCE_COMMON_F12_ENTER = 25;
        //public const int RESOURCE_COMMON_F4_DEL = 27;
        //public const int RESOURCE_COMMON_F6_IMPORT = 28;
        //public const int RESOURCE_COMMON_F7_OUTPUT = 29;
        //public const int RESOURCE_COMMON_F8_INQUIRY = 30;
        //public const int RESOURCE_COMMON_F9_DIFF = 31;
        //public const int RESOURCE_COMMON_F10_SETTLEMENT = 32;
        //public const int RESOURCE_COMMON_F8_DETAILS = 33;
        //public const int RESOURCE_COMMON_F10_UPDLINE = 37;
        //public const int RESOURCE_COMMON_F12_REC = 38;
        //public const int RESOURCE_COMMON_F9_APPROVAL = 39;
        //public const int RESOURCE_COMMON_F11_REJECTION = 40;
        //public const int RESOURCE_COMMON_F4_AFTER_ROW = 41;
        //public const int RESOURCE_COMMON_F5_NEXT_ROW = 42;
        //public const int RESOURCE_COMMON_F2_SHOHIN_HOJYO = 43;
        //public const int RESOURCE_COMMON_F3_TANKA_ENKOU_RIREKI = 44;
        //public const int RESOURCE_COMMON_F3_CSV_OUTPUT = 111;
        //public const int RESOURCE_COMMON_F6_SHEET_OUTPUT = 112;

        //public const int RESOURCE_COMMON_F5_TANKASYONIN = 180;
        //public const int RESOURCE_COMMON_F6_KIKANSYONIN = 181;
        //public const int RESOURCE_COMMON_F7_TANKAKYAKA = 182;
        //public const int RESOURCE_COMMON_F8_KIKANKYAKA = 183;
        //public const int RESOURCE_COMMON_F7_INSTORE = 184;

        //public const int RESOURCE_COMMON_F7_JUCHUU = 46;
        //public const int RESOURCE_COMMON_F10_COPY = 47;
        //public const int RESOURCE_COMMON_F10_UPD_MODE = 48;
        //public const int RESOURCE_COMMON_F11_CANCEL = 49;
        //public const int RESOURCE_COMMON_F10_NO_NEW = 50;
        //public const int RESOURCE_COMMON_F3_ORDERS_INQ = 51;
        //public const int RESOURCE_COMMON_F4_ALL_CLEAR = 52;
        //public const int RESOURCE_COMMON_F5_ALL_SELECT = 53;

        //public const int RESOURCE_COMMON_F4_JUCHUU = 54;
        //public const int RESOURCE_COMMON_F5_COPY = 55;
        //public const int RESOURCE_COMMON_F6_UPD_MODE = 56;
        //public const int RESOURCE_COMMON_F7_CANCEL = 57;
        //public const int RESOURCE_COMMON_F8_NO_NEW = 58;
        //public const int RESOURCE_COMMON_F9_ORDERS_INQ = 59;
        //public const int RESOURCE_COMMON_F12_ALL_CLEAR = 60;
        //public const int RESOURCE_COMMON_F6_NYUKA_MEISHI = 61;
        //public const int RESOURCE_COMMON_F9_SAIHYO_HATU = 62;
        //public const int RESOURCE_COMMON_F6_NYUKA_MEISAI_SYUSEI = 63;
        //public const int RESOURCE_COMMON_F6_SHIRESAKI_CD = 64;

        //public const int RESOURCE_COMMON_F5_COLORSIZE = 108;
        //public const int RESOURCE_COMMON_TTL_NEW = 101;
        //public const int RESOURCE_COMMON_TTL_UPD = 102;
        //public const int RESOURCE_COMMON_TTL_DEL = 103;
        //public const int RESOURCE_COMMON_TTL_DEL_AGAIN = 104;
        //public const int RESOURCE_COMMON_F8_PUBLISH = 105;
        //public const int RESOURCE_COMMON_F4_ALL_SELECT = 106;
        //public const int RESOURCE_COMMON_F5_ALL_REMOVE = 107;
        //public const int RESOURCE_COMMON_F5_Edit = 109;
        //public const int RESOURCE_COMMON_F8_INNSATU = 110;
        //public const int RESOURCE_COMMON_F5_HACHU_SYOUKAI = 152;
        //public const int RESOURCE_COMMON_F4_MDSELECT = 65;
        //public const int RESOURCE_COMMON_F4_URIAGESELECT = 66;
        //public const int RESOURCE_COMMON_F5_HACHU_SEARCH = 179;

        //// 在庫選択
        //public const int RESOURCE_COMMON_STOCK_EDIT = 163;
        //public const int RESOURCE_COMMON_STOCK_SHOKAI = 172;

        //// CSV取込(受注)
        //public const int RESOURCE_COMMON_JAN_IMPORT = 173;
        //public const int RESOURCE_COMMON_HINBAN_IMPORT = 174;
        //public const int RESOURCE_COMMON_F11_PASTE = 175;

        //public const int RESOURCE_COMMON_F9_ZAIKOSYUYAKU = 176;
        //public const int RESOURCE_COMMON_F8_WARITUKE_UKE = 177;
        //public const int RESOURCE_COMMON_F10_WARITUKE_HARAI = 178;
        //public const int RESOURCE_COMMON_F12_HIKIATEYOYAKU = 185;
        //public const int RESOURCE_COMMON_F10_SANSHO_MODE = 186;
        //public const int RESOURCE_COMMON_F10_GYO_SANSHO = 187;
        //public const int RESOURCE_COMMON_F5_NYUKAUPD = 188;

        //public const int RESOURCE_COMMON_LBL_INS_USER_ID = 201;
        //public const int RESOURCE_COMMON_LBL_INS_DATE_TIME = 202;
        //public const int RESOURCE_COMMON_LBL_UPD_USER_ID = 203;
        //public const int RESOURCE_COMMON_LBL_UPD_DATE_TIME = 204;
        //public const int RESOURCE_COMMON_LBL_INS_TANMATUSID = 205;
        //public const int RESOURCE_COMMON_LBL_UPD_TANMATUSID = 206;

        //public const int RESOURCE_COMMON_ITEM_BRAND = 301;            // 商品　ﾌﾞﾗﾝﾄﾞ
        //public const int RESOURCE_COMMON_ITEM_CODE = 302;             // 商品　品番
        //public const int RESOURCE_COMMON_ITEM_NAME = 303;             // 商品　品名
        //public const int RESOURCE_COMMON_ITEM_SEASON = 304;           // 商品　ｼｰｽﾞﾝ
        //public const int RESOURCE_COMMON_ITEM_COLOR = 305;            // 商品　色コード
        //public const int RESOURCE_COMMON_ITEM_COLOR_NAME = 306;       // 商品　色名
        //public const int RESOURCE_COMMON_ITEM_SIZE = 307;             // 商品　ｻｲｽﾞ
        //public const int RESOURCE_COMMON_ITEM_SIZE_NAME = 308;        // 商品　ｻｲｽﾞ名
        //public const int RESOURCE_COMMON_ITEM_OLD_PRICE = 309;        // 商品　上代単価
        //public const int RESOURCE_COMMON_ITEM_OLD_MONERY = 310;       // 商品　上代金額
        //public const int RESOURCE_COMMON_ITEM_NEW_PRICE = 311;        // 商品　下代単価
        //public const int RESOURCE_COMMON_ITEM_NEW_MONERY = 312;       // 商品　下代金額
        //public const int RESOURCE_COMMON_ITEM_BRAND_NAME = 313;       // 商品　ﾌﾞﾗﾝﾄﾞ名
        //public const int RESOURCE_COMMON_ITEM_SEASON_NAME = 314;      // 商品　ｼｰｽﾞﾝ名
        //public const int RESOURCE_COMMON_ITEM_PRODUCT = 315;          // 商品　ｱｲﾃﾑ
        //public const int RESOURCE_COMMON_ITEM_PRODUCT_NAME = 316;     // 商品　ｱｲﾃﾑ名
        //public const int RESOURCE_COMMON_ITEM_SOKO = 317;             // 商品　出荷元倉庫
        //public const int RESOURCE_COMMON_ITEM_SOKO_NAME = 318;        // 商品　出荷元倉庫名


        //public const int RESOURCE_COMMON_SEL_ALL_BTN = 351;
        //public const int RESOURCE_COMMON_SEL_SEARCH_TITLE = 352;
        //public const int RESOURCE_COMMON_SEL_SEARCH_COUNT = 353;
        //public const int RESOURCE_COMMON_SEL_INFO = 354;

        //public const int RESOURCE_COMMON_DELIVERED = 400;                     // 納品　納品先
        //public const int RESOURCE_COMMON_DELIVERED_CODE = 401;                // 納品　納品先ｺｰﾄﾞ
        //public const int RESOURCE_COMMON_DELIVERED_NAME = 402;                // 納品　納品先名称
        //public const int RESOURCE_COMMON_DELIVERED_WAREHOUSE = 403;           // 納品　出荷元倉庫
        //public const int RESOURCE_COMMON_BILLINGDESTINATION = 404;            // 納品  請求先
        //public const int RESOURCE_COMMON_BILLINGDESTINATION_CODE = 405;       // 納品　請求先ｺｰﾄﾞ
        //public const int RESOURCE_COMMON_BILLINGDESTINATION_NAME = 406;       // 納品　請求先名称
        //public const int RESOURCE_COMMON_ADDRESS = 407;                       // 納品　住所
        //public const int RESOURCE_COMMON_PERSONNEL = 408;                     // 納品  担当者
        //public const int RESOURCE_COMMON_DEPARTMENT = 409;                    // 納品　部門
        //public const int RESOURCE_COMMON_TEL_NUM = 410;                       // 納品　電話番号
        //public const int RESOURCE_COMMON_FAX = 411;                           // 納品　ＦＡＸ
        //public const int RESOURCE_COMMON_FAX_NUM = 412;                       // 納品　FAX番号
        //public const int RESOURCE_COMMON_YUUBIN_NUM = 413;                    // 納品　郵便番号
        //public const int RESOURCE_COMMON_AGGREGATESDELIVERED = 414;           // 納品  集約納品先
        //public const int RESOURCE_COMMON_AGGREGATESDELIVERED_CODE = 415;      // 納品　集約納品先ｺｰﾄﾞ
        //public const int RESOURCE_COMMON_AGGREGATESDELIVERED_NAME = 416;      // 納品　集約納品先名称
        //public const int RESOURCE_COMMON_BOX_DETAIL = 417;                    // 納品　箱明細
        //public const int RESOURCE_COMMON_DEDICATED_TAG = 418;                 // 納品  専ﾀｸﾞ
        //public const int RESOURCE_COMMON_DEDICATED_TRANSFER = 419;            // 納品　専伝
        //public const int RESOURCE_COMMON_VALUE_WITH = 420;                    // 納品　値付
        //public const int RESOURCE_COMMON_QUALITY_TAG = 421;                   // 納品　品質ﾀｸﾞ
        //public const int RESOURCE_COMMON_INSPECTED_REPORT = 422;              // 納品　検品報告
        //public const int RESOURCE_COMMON_MAKER = 423;                         // 作成者
        //public const int RESOURCE_COMMON_MAKE_DATE = 424;                     // 作成日付
        //public const int RESOURCE_COMMON_UPDATER = 425;                       // 更新者
        //public const int RESOURCE_COMMON_UPDATE_DATE = 426;                   // 更新日付
        //public const int RESOURCE_COMMON_CSV_PRINT = 427;                     // CSV出力
        //public const int RESOURCE_COMMON_PART_SELECT = 428;                   // 部分選択

        //public const int RESOURCE_COMMON_PRINT_SHEET = 501;
        //public const int RESOURCE_COMMON_PRINT_PAGE = 502;
        //public const int RESOURCE_COMMON_PRINT_BARCODE = 503;
        //public const int RESOURCE_COMMON_PRINT_HEADER = 504;
        //public const int RESOURCE_COMMON_PRINT_FOOTER = 505;

        //// <<Resourceテーブル>>ﾕｰｻﾞｰIDの取得
        //public const string RESOURCE_COMMON_PRG_ID = "SCLPS01";
        //public const int RESOURCE_COMMON_USER_ID = 1;
        //public const string RESOURCE_COMMON_SOKOE05 = "RTSLE05";
        //public const int RESOURCE_COMMON_SOKO_IDE05 = 13;
        //public const string RESOURCE_COMMON_SOKOE07 = "RTSLE07";
        //public const int RESOURCE_COMMON_SOKO_IDE07 = 21;
        //public const string RESOURCE_COMMON_RTTRE02 = "RTTRE02";
        //public const int RESOURCE_COMMON_RTTRE02IDE02 = 10;


        //public const string SEQ_WMS_IDO_KBN = "2";                                // WMS側での移動伝票№末尾：2
        //public const string SEQ_WMS_NOHINSHO_KBN = "3";                           // WMS側での納品書№末尾：2
        //public const string SEQ_WMS_UKEIRE_NO = "000";                            // 受入№末尾：000

        //public const string SEQ_KBN_SHIP_BCD = "SHIP_BCD";                        // ShippingボードＢＣＤ
        //public const string SEQ_KBN_NAME = "NAME";                                // ネームプレートＢＣＤ
        //public const string SEQ_KBN_TANA_BCD = "TANA_BCD";                        // 棚番ＢＣＤ
        //public const string SEQ_KBN_SHUK = "SHUK";                                // 出荷No(出荷カンバン用)ＢＣＤ
        //public const string SEQ_KBN_PICK = "PICK";                                // ﾋﾟｯｷﾝｸﾞﾘｽﾄＮｏＢＣＤ
        //public const string SEQ_KBN_NOHIN = "NOHIN";                              // 納品書ＢＣＤ
        //public const string SEQ_KBN_FRYO = "FRYO";                                // 不良カードＢＣＤ
        //public const string SEQ_KBN_FRYO_SR = "FRYO_SR";                          // 不良カードＢＣＤ（ｽｷｬﾝ省略）
        //public const string SEQ_KBN_HENPIN = "HENPIN";                            // 返品管理NoＢＣＤ
        //public const string SEQ_KBN_WMS_IDO = "WMS_IDO";                          // ＷＭＳ移動出庫伝票NoＢＣＤ
        //public const string SEQ_KBN_NH_JK_PTN = "NH_JK_PTN";                      // 納品条件ﾊﾟﾀﾝｺｰﾄﾞ
        //public const string SEQ_KBN_ZAIKOTS = "ZAIKOTS";                          // 在庫調整伝票NoＢＣＤ
        //public const string SEQ_KBN_TANAOROSHI = "TANAOROSHI";                    // 棚卸管理No
        //// Public Const SEQ_KBN_UKEBARAI As String = "UKEBARAI"                        '受払管理No
        //public const string SEQ_KBN_SHIPK = "SHIPK";                              // Shipping管理№
        //// 追加部分
        //public const string SEQ_KBN_MITUMORI = "MITUMORI";                        // 見積№
        //public const string SEQ_KBN_TOIAWASE = "TOIAWASE";                        // 依頼№
        //public const string SEQ_KBN_HINBAN = "HINBAN";                            // 品番
        //public const string SEQ_KBN_CARD = "CARD";                                // 顧客カードNo
        //public const string SEQ_KBN_UKEBARAI = "UKEBARAI";                        // 受払管理No
        //public const string SEQ_KBN_URIAGE = "URIAGE";                            // 売上No
        //public const string SEQ_KBN_SHIRE = "SHIRE";                              // 仕入No
        //public const string SEQ_KBN_SHIP = "SHIP";                                // 入荷管理No
        //public const string SEQ_KBN_IDO = "IDO";                                  // 移動伝票No
        //public const string SEQ_KBN_TOKUTEI = "TOKUTEI";                          // 特定カード
        //public const string SEQ_KBN_UKEIRE = "UKEIRE";                            // 受入No
        //public const string SEQ_KBN_EDI = "EDI";                                  // EDI管理No
        //public const string SEQ_KBN_OSHIRASE = "OSHIRASE";                        // お知らせID　 
        //public const string SEQ_KBN_URIKAKE = "URIKAKE";                          // 売掛　 
        //public const string SEQ_KBN_KAIKAKE = "KAIKAKE";                          // 買掛　 
        //public const string SEQ_KBN_NYUKIN = "NYUKIN";                            // 入金　 
        //public const string SEQ_KBN_SHIHARAI = "SHIHARAI";                        // 支払
        //public const string SEQ_KBN_SHIWAKE = "SHIWAKE";                          // 仕訳伝票
        //public const string SEQ_KBN_SEIKYU = "SEIKYU";                            // 請求
        //public const string SEQ_KBN_SHIHARAIS = "SHIHARAIS";                      // 支払締　 
        //public const string SEQ_KBN_ARZANDAKA = "ARZANDAKA";                      // 売掛残高　 
        //public const string SEQ_KBN_APZANDAKA = "APZANDAKA";                      // 買掛残高　
        //public const string SEQ_KBN_MSHIWAKE = "MSHIWAKE";                        // 明細仕訳　 
        //public const string SEQ_KBN_CHOSEINO = "CHOSEINO";                        // 調整伝票No　
        //public const string SEQ_KBN_MDKANRENNO = "MDKANRENNO";                    // MD関連№　   
        //public const string SEQ_KBN_JUCHU = "JUCHU";                              // 受注No
        //public const string SEQ_KBN_TENJIKAI = "TENJIKAI";                        // 展示会
        //public const string SEQ_KBN_SHOHIZEI_URI = "SHOHIZEI_U";                // 消費税(売掛)
        //public const string SEQ_KBN_SHOHIZEI_KAI = "SHOHIZEI_K";                // 消費税(買掛)


        //public const string SEQ_PRE_CHAR_UKEIRE_NO = "09";                        // 受入No識別番号：09
        //// 2014/9/3 suzuki add st
        //public const string SEQ_PRE_CHAR_NYUKAQC_NO = "01";                       // 入荷検品No識別番号：01
        //// 2014/9/3 suzuki add ed

        //// 2014/10/28 suzuki add st
        //public const string SEQ_PRE_CHAR_SHUKA_MEISAI_NO = "20";                       // 出荷明細書No識別番号：01
        //// 2014/10/28 suzuki add ed
        //public const string SEQ_PRE_CHAR_SHIP_BCD = "901";                        // ShippingボードＢＣＤ
        //public const string SEQ_PRE_CHAR_NAME = "902";                            // ネームプレートＢＣＤ
        //public const string SEQ_PRE_CHAR_TANA_BCD = "903";                        // 棚版ＢＣＤ
        //public const string SEQ_PRE_CHAR_SHUK = "904";                            // 出荷No(出荷カンバン用)ＢＣＤ
        //public const string SEQ_PRE_CHAR_PICK = "905";                            // ﾋﾟｯｷﾝｸﾞﾘｽﾄＮｏＢＣＤ
        //public const string SEQ_PRE_CHAR_R_PICK = "06";                          // 逆ﾋﾟｯｷﾝｸﾞﾘｽﾄＮｏＢＣＤ
        //public const string SEQ_PRE_CHAR_NOHIN = "907";                           // 納品書ＢＣＤ
        //public const string SEQ_PRE_CHAR_FURYO = "908";                            // 不良カードＢＣＤ
        //public const string SEQ_PRE_CHAR_HENPIN = "909";                          // 返品管理NoＢＣＤ
        //public const string SEQ_PRE_CHAR_WMS_IDO = "910";                         // ＷＭＳ移動出庫伝票NoＢＣＤ
        //public const string SEQ_PRE_CHAR_ZAIKOTS = "911";                         // 在庫調整伝票NoＢＣＤ
        //public const string SEQ_PRE_CHAR_TOTALPICK = "912";                       // ﾄｰﾀﾙﾋﾟｯｷﾝｸﾞﾘｽﾄＮｏＢＣＤ
        //public const string SEQ_PRE_CHAR_R_WMS = "917";                           // （WMS）逆ﾋﾟｯｷﾝｸﾞﾘｽﾄ№のﾊﾞｰｺｰﾄﾞ№

        //public const string SEQ_NAME_SHIP_BCD = "ShippingボードＢＣＤ";           // ShippingボードBCD
        //public const string SEQ_NAME_NAME = "ネームプレートＢＣＤ";               // ネームプレートＢＣＤ
        //public const string SEQ_NAME_TANA_BCD = "棚番ＢＣＤ";                     // 棚番ＢＣＤ
        //public const string SEQ_NAME_SHUK = "出荷No(出荷カンバン用)ＢＣＤ";       // 出荷No(出荷カンバン用)ＢＣＤ
        //public const string SEQ_NAME_PICK = "ﾋﾟｯｷﾝｸﾞﾘｽﾄ№ＢＣＤ";                 // ﾋﾟｯｷﾝｸﾞﾘｽﾄＮｏＢＣＤ
        //public const string SEQ_NAME_NOHIN = "納品書ＢＣＤ";                      // 納品書ＢＣＤ
        //public const string SEQ_NAME_FRYO = "不良カードＢＣＤ";                   // 不良カードＢＣＤ
        //public const string SEQ_NAME_HENPIN = "返品管理№ＢＣＤ";                 // 返品管理NoＢＣＤ
        //public const string SEQ_NAME_WMS_IDO = "ＷＭＳ移動出庫伝票№ＢＣＤ";      // ＷＭＳ移動出庫伝票NoＢＣＤ
        //public const string SEQ_NAME_NH_JK_PTN = "納品条件ﾊﾟﾀﾝｺｰﾄﾞ";              // 納品条件ﾊﾟﾀﾝｺｰﾄﾞ
        //public const string SEQ_NAME_ZAIKOTS = "在庫調整伝票№ＢＣＤ";            // 在庫調整伝票NoＢＣＤ
        //public const string SEQ_NAME_TANAOROSHI = "棚卸管理№";                   // 棚卸管理No
        //public const string SEQ_NAME_UKEBARAI = "受払管理№";                     // 受払管理No
        //public const string SEQ_NAME_SHIPK = "Shipping管理№";                    // Shipping管理№

        //// 追加部分
        //public const string SEQ_NAME_MITUMORI = "見積№";                        // 見積№
        //public const string SEQ_NAME_TOIAWASE = "依頼№";                        // 依頼№
        //public const string SEQ_NAME_HINBAN = "品番";                            // 品番
        //public const string SEQ_NAME_CARD = "顧客カードNo";                      // 顧客カードNo
        //// Public Const SEQ_NAME_UKEBARAI As String = "受払管理No"                    '受払管理No
        //public const string SEQ_NAME_URIAGE = "売上No";                          // 売上No
        //public const string SEQ_NAME_SHIRE = "仕入No";                           // 仕入No
        //public const string SEQ_NAME_SHIP = "入荷管理No";                        // 入荷管理No
        //public const string SEQ_NAME_IDO = "移動伝票No";                         // 移動伝票No
        //public const string SEQ_NAME_TOKUTEI = "特定カード";                     // 特定カード
        //public const string SEQ_NAME_EDI = "EDI管理No";                          // EDI管理No
        //public const string SEQ_NAME_UKEIRE = "受入No";                          // 受入No
        //public const string SEQ_NAME_OSHIRASE = "お知らせID";                    // お知らせID
        //public const string SEQ_NAME_URIKAKE = "売掛";                           // 売掛　 
        //public const string SEQ_NAME_KAIKAKE = "買掛";                           // 買掛　 
        //public const string SEQ_NAME_NYUKIN = "入金";                            // 入金　 
        //public const string SEQ_NAME_SHIHARAI = "支払";                          // 支払
        //public const string SEQ_NAME_SHIWAKE = "仕訳伝票";                       // 仕訳伝票
        //public const string SEQ_NAME_SEIKYU = "請求";                            // 請求
        //public const string SEQ_NAME_SHIHARAIS = "支払締";                       // 支払締　 
        //public const string SEQ_NAME_ARZANDAKA = "売掛残高";                     // 売掛残高　 
        //public const string SEQ_NAME_APZANDAKA = "買掛残高";                     // 買掛残高　
        //public const string SEQ_NAME_MSHIWAKE = "明細仕訳";                      // 明細仕訳　 
        //public const string SEQ_NAME_CHOSEINO = "調整伝票No";                    // 調整伝票No
        //public const string SEQ_NAME_MDKANRENNO = "MD関連№";                    // MD関連№
        //public const string SEQ_NAME_TENJIKAI = "展示会CD";                        // 展示会CD
        //public const string SEQ_NAME_SHOHIZEI_U = "売掛消費税No";                        // 売掛消費税No
        //public const string SEQ_NAME_SHOHIZEI_K = "買掛消費税No";                        // 買掛消費税No



        //public const string BUTTONTEXT_F1 = "F1";
        //public const string BUTTONTEXT_F2 = "F2";
        //public const string BUTTONTEXT_F3 = "F3";
        //public const string BUTTONTEXT_F4 = "F4";
        //public const string BUTTONTEXT_F5 = "F5";
        //public const string BUTTONTEXT_F6 = "F6";
        //public const string BUTTONTEXT_F7 = "F7";
        //public const string BUTTONTEXT_F8 = "F8";
        //public const string BUTTONTEXT_F9 = "F9";
        //public const string BUTTONTEXT_F10 = "F10";
        //public const string BUTTONTEXT_F11 = "F11";
        //public const string BUTTONTEXT_F12 = "F12";

        public const string LOG_LEVEL_ALL = "ALL";
        public const string LOG_LEVEL_DEBUG = "DEBUG";
        public const string LOG_LEVEL_INFO = "INFO";
        public const string LOG_LEVEL_ERROR = "ERROR";
        public const string LOG_LEVEL_WARNING = "WARNING";

        public const string LOGINUSERINFO_KEY = "INFO_KEY";
        public const string LOGINUSERINFO_VALUE = "INFO_VALUE";

        //public const string LOGINUSERINFO_KEY_SHAIN_CD = "SHAIN_CD";
        //public const string LOGINUSERINFO_KEY_SHAIN_BCD = "SHAIN_BCD";
        //public const string LOGINUSERINFO_KEY_SHAIN_NAME_KN = "SHAIN_NAME_KN";
        //public const string LOGINUSERINFO_KEY_SHAIN_NAME_RK = "SHAIN_NAME_RK";
        //public const string LOGINUSERINFO_KEY_SHAIN_NAME_RK_KN = "SHAIN_NAME_RK_KN";
        //public const string LOGINUSERINFO_KEY_LOGIN_PW = "LOGIN_PW";
        //public const string LOGINUSERINFO_KEY_PW_UPDATE = "PW_UPDATE";
        //public const string LOGINUSERINFO_KEY_WEB_ID = "WEB_ID";
        //public const string LOGINUSERINFO_KEY_WEB_PW1 = "WEB_PW1";
        //public const string LOGINUSERINFO_KEY_WEB_PW2 = "WEB_PW2";
        //public const string LOGINUSERINFO_KEY_WEB_FLG = "WEB_FLG";
        //public const string LOGINUSERINFO_KEY_KENGEN_GP_ID = "KENGEN_GP_ID";
        //public const string LOGINUSERINFO_KEY_YUKO_START = "YUKO_START";
        //public const string LOGINUSERINFO_KEY_YUKO_END = "YUKO_END";
        //public const string LOGINUSERINFO_KEY_ZEN_BUMON_FLG = "ZEN_BUMON_FLG";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_01 = "KENMU_BUSHO_01";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_02 = "KENMU_BUSHO_02";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_03 = "KENMU_BUSHO_03";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_04 = "KENMU_BUSHO_04";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_05 = "KENMU_BUSHO_05";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_06 = "KENMU_BUSHO_06";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_07 = "KENMU_BUSHO_07";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_08 = "KENMU_BUSHO_08";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_09 = "KENMU_BUSHO_09";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_10 = "KENMU_BUSHO_10";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO_11 = "KENMU_BUSHO_11";
        //public const string LOGINUSERINFO_KEY_KENMU_BUSHO = "KENMU_BUSHO";
        //public const string LOGINUSERINFO_KEY_CRT_SHAIN_CD = "CRT_SHAIN_CD";
        //public const string LOGINUSERINFO_KEY_CRT_DATE = "CRT_DATE";
        //public const string LOGINUSERINFO_KEY_CRT_TANMATU_ID = "CRT_TANMATU_ID";
        //public const string LOGINUSERINFO_KEY_UPD_SHAIN_CD = "UPD_SHAIN_CD";
        //public const string LOGINUSERINFO_KEY_UPD_DATE = "UPD_DATE";
        //public const string LOGINUSERINFO_KEY_UPD_TANMATU_ID = "UPD_TANMATU_ID";
        //public const string LOGINUSERINFO_KEY_VERSION_NO = "VERSION_NO";
        //public const string LOGINUSERINFO_KEY_MUKO_FLG = "MUKO_FLG";
        //public const string LOGINUSERINFO_KEY_DELETE_FLG = "DELETE_FLG";
        //public const string LOGINUSERINFO_KEY_HAITA_FLG = "HAITA_FLG";
        //public const string LOGINUSERINFO_KEY_HAITA_NICHIJI = "HAITA_NICHIJI";
        //public const string LOGINUSERINFO_KEY_HAITA_USER = "HAITA_USER";

        //public const string LOGINUSERINFO_KEY_ID = "LOGIN_ID";
        //public const string LOGINUSERINFO_KEY_NAME = "SHAIN_NAME";
        //public const string LOGINUSERINFO_KEY_SHAIN_KBN = "SHAIN_KBN";
        //public const string LOGINUSERINFO_KEY_PW_HENKO_FLG = "PWLMT_FLG";
        //public const string LOGINUSERINFO_KEY_BUSHO_CD = "BUSHO_CD";
        //public const string LOGINUSERINFO_KEY_TANMATSUID = "TANMATSU_ID";
        //public const string LOGINUSERINFO_KEY_HAVE_NOTYFY = "HAVE_NOTYFY";

        //public const string LOGINUSERINFO_KEY_LOGINFLG = "LOGINFLG";
        //public const string LOGINUSERINFO_KEY_LOGIN_TANMATU_ID = "LOGIN_TANMATU_ID";
        //public const string LOGINUSERINFO_KEY_LOGINDT = "LOGINDT";
        //public const string LOGINUSERINFO_KEY_OSHIRASE_NEW_FLG = "OSHIRASE_NEW_FLG";

        //public const string LOGINUSERINFO_KEY_SOKO_CD = "SOKO_CD";
        //public const string LOGINUSERINFO_KEY_SOKO_NAME = "SOKO_NAME";

        //// T_LOG ログ情報最大長さ
        //public const int LOGJYOHOMAX = 8000;

        //public const string FORM_ID_100 = "100";
        //public const string FORM_ID_101 = "101";
        //public const string FORM_ID_102 = "102";
        //public const string FORM_ID_103 = "103";
        //public const string FORM_ID_110 = "110";
        //public const string FORM_ID_111 = "111";
        //public const string FORM_ID_112 = "112";
        //public const string FORM_ID_115 = "115";
        //public const string FORM_ID_120 = "120";
        //public const string FORM_ID_121 = "121";
        //public const string FORM_ID_130 = "130";
        //public const string FORM_ID_131 = "131";
        //public const string FORM_ID_132 = "132";
        //public const string FORM_ID_140 = "140";
        //public const string FORM_ID_200 = "200";
        //public const string FORM_ID_201 = "201";
        //public const string FORM_ID_202 = "202";
        //public const string FORM_ID_203 = "203";
        //public const string FORM_ID_204 = "204";
        //public const string FORM_ID_210 = "210";
        //public const string FORM_ID_211 = "211";
        //public const string FORM_ID_220 = "220";
        //public const string FORM_ID_221 = "221";
        //public const string FORM_ID_300 = "300";
        //public const string FORM_ID_301 = "301";
        //public const string FORM_ID_302 = "302";
        //public const string FORM_ID_303 = "303";
        //public const string FORM_ID_304 = "304";
        //public const string FORM_ID_310 = "310";
        //public const string FORM_ID_311 = "311";
        //public const string FORM_ID_312 = "312";
        //public const string FORM_ID_320 = "320";
        //public const string FORM_ID_321 = "321";
        //public const string FORM_ID_322 = "322";
        //public const string FORM_ID_330 = "330";
        //public const string FORM_ID_331 = "331";
        //public const string FORM_ID_400 = "400";
        //public const string FORM_ID_401 = "401";
        //public const string FORM_ID_402 = "402";
        //public const string FORM_ID_403 = "403";
        //public const string FORM_ID_404 = "404";
        //public const string FORM_ID_405 = "405";
        //public const string FORM_ID_410 = "410";
        //public const string FORM_ID_411 = "411";
        //public const string FORM_ID_412 = "412";
        //public const string FORM_ID_500 = "500";
        //public const string FORM_ID_501 = "501";
        //public const string FORM_ID_502 = "502";
        //public const string FORM_ID_503 = "503";
        //public const string FORM_ID_504 = "504";
        //public const string FORM_ID_505 = "505";
        //public const string FORM_ID_510 = "510";
        //public const string FORM_ID_511 = "511";
        //public const string FORM_ID_520 = "520";
        //public const string FORM_ID_521 = "521";
        //public const string FORM_ID_522 = "522";
        //public const string FORM_ID_523 = "523";
        //public const string FORM_ID_524 = "524";
        //public const string FORM_ID_525 = "525";
        //public const string FORM_ID_530 = "530";
        //public const string FORM_ID_531 = "531";
        //public const string FORM_ID_512 = "512";
        //public const string FORM_ID_532 = "532";
        //public const string FORM_ID_514 = "514";
        //public const string FORM_ID_515 = "515";
        //public const string FORM_ID_534 = "534";
        //public const string FORM_ID_535 = "535";

        //public const string FORM_ID_600 = "600";
        //public const string FORM_ID_610 = "610";
        //public const string FORM_ID_611 = "611";
        //public const string FORM_ID_612 = "612";
        //public const string FORM_ID_613 = "613";
        //public const string FORM_ID_614 = "614";
        //public const string FORM_ID_615 = "615";
        //public const string FORM_ID_601 = "601";
        //public const string FORM_ID_616 = "616";
        //public const string FORM_ID_618 = "618";
        //public const string FORM_ID_620 = "620";
        //public const string FORM_ID_621 = "621";
        //public const string FORM_ID_622 = "622";
        //public const string FORM_ID_677 = "677";
        //public const string FORM_ID_688 = "688";
        //public const string FORM_ID_699 = "699";
        //public const string FORM_ID_150 = "150";
        //public const string FORM_ID_151 = "151";

        //public const string FORM_ID_602 = "602";
        //public const string FORM_ID_603 = "603";
        //public const string FORM_ID_617 = "617";
        //public const string FORM_ID_631 = "631";
        //public const string FORM_ID_632 = "632";
        //public const string FORM_ID_633 = "633";
        //public const string FORM_ID_630 = "630";

        //public const string FORM_ID_SC_NF_S01 = "SC_NF_S01";

        //public const string SYS_CTRL_WEB_NYUKOSINTYOKU = "WebNyukoSintyoku";
        //public const string SYS_CTRL_WEB_FURISISITEI = "WebFurisisitei";


        //public const string LOGINFORM_NAME = "SC_LP_S01";

        public const string CONST_ENCODING = "Shift-JIS";
        public const string FILE_EXT_NAME_XLSX = "xlsx";
        public const string FILE_JPG = "画像ﾌｧｲﾙ(*.jpg;*.jpeg;*.bmp)|*.jpg;*.jpeg;*.bmp";
        public const string FILE_EXT_NAME_CSV = "csv";
        public const string FILE_FILTER_XLSX = "Excelﾌｧｲﾙ(*.xlsx;*.xls)|*.xlsx;*.xls";
        public const string FILE_FILTER_CSV = "Csvﾌｧｲﾙ(*.csv)|*.csv";
        // Public Const FILE_FILTER_CSV_SAPATER As String = "	"
        public const string FILE_FILTER_CSV_SAPATER = ",";
        public const string FILE_FILTER_CSV_QUOTE = "\"";
        public const string FILE_VBLF = "%CR$";
        // CSV拡張子
        public const string FILE_CSV_SUFFIX = ".csv";


        public const string SIKIBETU_KBN_001 = "001";     // 進捗ｽﾃｰﾀｽ
        public const string SIKIBETU_KBN_002 = "002";     // INVOICE差異有無
        public const string SIKIBETU_KBN_003 = "003";     // 不良有無
        public const string SIKIBETU_KBN_004 = "004";     // Shipping№のチェック有無
        public const string SIKIBETU_KBN_005 = "005";     // 商品状態区分
        public const string SIKIBETU_KBN_006 = "006";     // 職出状態区分
        public const string SIKIBETU_KBN_007 = "007";     // 伝票タイプ
        public const string SIKIBETU_KBN_008 = "008";     // 納品条件有無
        public const string SIKIBETU_KBN_009 = "009";     // 指示区分
        public const string SIKIBETU_KBN_010 = "010";     // 出荷先種別
        public const string SIKIBETU_KBN_011 = "011";     // 特約店管理FLG
        public const string SIKIBETU_KBN_012 = "012";     // 取込種別
        public const string SIKIBETU_KBN_013 = "013";     // 印刷区分
        public const string SIKIBETU_KBN_014 = "014";     // 調整先区分
        public const string SIKIBETU_KBN_015 = "015";     // 調整区分
        public const string SIKIBETU_KBN_016 = "016";     // 実作業有無
        public const string SIKIBETU_KBN_017 = "017";     // 棚間移動作業区分
        public const string SIKIBETU_KBN_018 = "018";     // 移動区分
        public const string SIKIBETU_KBN_019 = "019";     // 棚タイプ
        public const string SIKIBETU_KBN_020 = "020";     // 棚状態
        public const string SIKIBETU_KBN_021 = "021";     // 棚卸対象区分
        public const string SIKIBETU_KBN_022 = "022";     // 外税内税区分
        public const string SIKIBETU_KBN_023 = "023";     // 課税区分
        public const string SIKIBETU_KBN_024 = "024";     // 商品分類区分
        public const string SIKIBETU_KBN_025 = "025";     // ﾕｰｻﾞ区分
        public const string SIKIBETU_KBN_026 = "026";     // ﾊﾟｽﾜｰﾄﾞ有効期間
        public const string SIKIBETU_KBN_027 = "027";     // ﾛｸﾞｲﾝﾌﾗｸﾞ
        public const string SIKIBETU_KBN_028 = "028";     // 倉庫種類
        public const string SIKIBETU_KBN_029 = "029";     // 倉庫所在
        public const string SIKIBETU_KBN_030 = "030";     // 送り状印刷
        public const string SIKIBETU_KBN_031 = "031";     // 納品条件ﾊﾟﾀﾝ区分
        public const string SIKIBETU_KBN_032 = "032";     // 調整理由
        public const string SIKIBETU_KBN_34_1 = "034-1";  // 配達指定日（ヤマト）
        public const string SIKIBETU_KBN_34_2 = "034-2";  // 配達指定日（佐川）
        public const string SIKIBETU_KBN_34_3 = "034-3";  // 配達指定日（浪速）
        public const string SIKIBETU_KBN_35_1 = "035-1";  // 配達時間帯指定（ヤマト）
        public const string SIKIBETU_KBN_35_2 = "035-2";  // 配達時間帯指定（佐川）
        public const string SIKIBETU_KBN_36_1 = "036-1";  // 便種類（ヤマト）
        public const string SIKIBETU_KBN_36_2 = "036-2";  // 便種類（佐川）
        public const string SIKIBETU_KBN_37_1 = "037-1";  // 配達品名（ヤマト）
        public const string SIKIBETU_KBN_37_2 = "037-2";  // 配達品名（佐川）
        public const string SIKIBETU_KBN_38_1 = "038-01";  // 個数口枠の印字（ヤマト）
        public const string SIKIBETU_KBN_39_1 = "039-01";  // 送り状出荷日(ヤマト）
        public const string SIKIBETU_KBN_39_2 = "039-02";  // 送り状出荷日(佐川）
        public const string SIKIBETU_KBN_39_3 = "039-03";  // 送り状出荷日(浪速）
        public const string SIKIBETU_KBN_040 = "040";     // 区切り文字
        public const string SIKIBETU_KBN_041 = "041";     // 原産国
        public const string SIKIBETU_KBN_042 = "042";     // 素材
        public const string SIKIBETU_KBN_043 = "043";     // コメント
        public const string SIKIBETU_KBN_044 = "044";     // 取引区分
        public const string SIKIBETU_KBN_045 = "045";     // 処理内容
        public const string SIKIBETU_KBN_046 = "046";     // 在庫発生区分
        public const string SIKIBETU_KBN_047 = "047";     // 項目ｿｰﾄ設定
        public const string SIKIBETU_KBN_048 = "048";     // タグ種類
        public const string SIKIBETU_KBN_049 = "049";     // 上代区分
        public const string SIKIBETU_KBN_050 = "050";     // 上代印字区分
        public const string SIKIBETU_KBN_051 = "051";     // 区切有無区分
        public const string SIKIBETU_KBN_052 = "052";     // アイテム
        public const string SIKIBETU_KBN_053 = "053";     // 状態
        public const string SIKIBETU_KBN_054 = "054";     // 発注区分
        public const string SIKIBETU_KBN_055 = "055";     // 納品先種別
        public const string SIKIBETU_KBN_056 = "056";     // 納品先種別２
        public const string SIKIBETU_KBN_057 = "057";     // 締日
        public const string SIKIBETU_KBN_058 = "058";     // 入出力
        public const string SIKIBETU_KBN_059 = "059";     // 取込状態区分
        public const string SIKIBETU_KBN_060 = "060";     // 取込形式
        public const string SIKIBETU_KBN_061 = "061";     // 休日区分
        public const string SIKIBETU_KBN_062 = "062";     // ﾒﾆｭｰID
        public const string SIKIBETU_KBN_063 = "063";     // 承認区分
        public const string SIKIBETU_KBN_064 = "064";     // 売買契約書区分
        public const string SIKIBETU_KBN_065 = "065";     // 保証預り金区分
        public const string SIKIBETU_KBN_066 = "066";     // 不動産担保区分
        public const string SIKIBETU_KBN_067 = "067";     // 店舗形態区分
        public const string SIKIBETU_KBN_068 = "068";     // 建物区分
        public const string SIKIBETU_KBN_069 = "069";     // 土地区分
        public const string SIKIBETU_KBN_070 = "070";     // 送金手数料区分
        public const string SIKIBETU_KBN_071 = "071";     // 評価ランク
        public const string SIKIBETU_KBN_072 = "072";     // 照合FLG
        public const string SIKIBETU_KBN_073 = "073";     // 支払方法
        public const string SIKIBETU_KBN_074 = "074";     // 消費税計算区分
        public const string SIKIBETU_KBN_075 = "075";     // 消費税端数区分
        public const string SIKIBETU_KBN_076 = "076";     // 単価端数区分
        public const string SIKIBETU_KBN_077 = "077";     // 会計外FLG
        public const string SIKIBETU_KBN_078 = "078";     // 発注先FLG
        public const string SIKIBETU_KBN_079 = "079";     // 返品先FLG
        public const string SIKIBETU_KBN_080 = "080";     // 職出先FLG
        public const string SIKIBETU_KBN_081 = "081";     // 配送方法区分
        public const string SIKIBETU_KBN_082 = "082";     // 発注方法区分
        public const string SIKIBETU_KBN_083 = "083";     // WEB接続FLG
        public const string SIKIBETU_KBN_084 = "084";     // WEB-DL区分1
        public const string SIKIBETU_KBN_085 = "085";     // 分類検索FLG
        public const string SIKIBETU_KBN_086 = "086";     // ｻｲｽﾞ区分
        public const string SIKIBETU_KBN_087 = "087";     // ﾃｳﾁFLG
        public const string SIKIBETU_KBN_088 = "088";     // 単価不定FLG
        public const string SIKIBETU_KBN_089 = "089";     // 無形品FLG
        public const string SIKIBETU_KBN_090 = "090";     // 値引不可区分
        public const string SIKIBETU_KBN_091 = "091";     // 商品分類区分
        public const string SIKIBETU_KBN_092 = "092";     // 単価区分
        public const string SIKIBETU_KBN_093 = "093";     // 値引区分
        public const string SIKIBETU_KBN_094 = "094";     // 無効FLG
        public const string SIKIBETU_KBN_095 = "095";     // 取消FLG
        public const string SIKIBETU_KBN_096 = "096";     // 削除FLG
        public const string SIKIBETU_KBN_097 = "097";     // 倉庫区分
        public const string SIKIBETU_KBN_098 = "098";     // 引当可能FLG
        public const string SIKIBETU_KBN_099 = "099";     // WEB公開FLG
        public const string SIKIBETU_KBN_100 = "100";     // 自動引当可能FLG
        public const string SIKIBETU_KBN_101 = "101";     // 伝票区分
        public const string SIKIBETU_KBN_102 = "102";     // 科目CD(借方)
        public const string SIKIBETU_KBN_103 = "103";     // 補助CD(借方)
        public const string SIKIBETU_KBN_104 = "104";     // 科目CD(貸方)
        public const string SIKIBETU_KBN_105 = "105";     // 補助CD(貸方)
        public const string SIKIBETU_KBN_106 = "106";     // 受払区分
        public const string SIKIBETU_KBN_107 = "107";     // 与信ランク
        public const string SIKIBETU_KBN_108 = "108";     // 曜日区分
        public const string SIKIBETU_KBN_109 = "109";     // 単位
        public const string SIKIBETU_KBN_110 = "110";     // 卸決定
        public const string SIKIBETU_KBN_111 = "111";     // 仕入決定
        public const string SIKIBETU_KBN_113 = "113";     // 社員区分
        public const string SIKIBETU_KBN_114 = "114";     // 全部門FLG
        public const string SIKIBETU_KBN_115 = "115";     // 休日FLG
        public const string SIKIBETU_KBN_116 = "116";     // データ区分
        public const string SIKIBETU_KBN_117 = "117";     // WEB-DL区分2
        public const string SIKIBETU_KBN_118 = "118";     // 承認不要FLG
        public const string SIKIBETU_KBN_119 = "119";     // 仕入先担当敬称
        public const string SIKIBETU_KBN_120 = "120";     // 依頼種別
        public const string SIKIBETU_KBN_121 = "121";     // 見積種別
        public const string SIKIBETU_KBN_122 = "122";     // 見積状態
        public const string SIKIBETU_KBN_123 = "123";     // 承認状態
        // Public Const SIKIBETU_KBN_124 As String = "124"     '見積取引区分    ******この区分は削除しました。
        public const string SIKIBETU_KBN_125 = "125";     // 見積有効期限
        public const string SIKIBETU_KBN_127 = "127";     // 直発注先FLG
        public const string SIKIBETU_KBN_128 = "128";     // 入荷方法区分
        public const string SIKIBETU_KBN_129 = "129";     // 在庫売上可能FLG
        public const string SIKIBETU_KBN_130 = "130";     // PW変更FLG
        public const string SIKIBETU_KBN_131 = "131";     // 発行状態
        public const string SIKIBETU_KBN_132 = "132";     // 計画分類区分
        public const string SIKIBETU_KBN_133 = "133";     // 新旧FLG
        public const string SIKIBETU_KBN_134 = "134";     // 計画種別
        public const string SIKIBETU_KBN_135 = "135";     // 所有権区分
        public const string SIKIBETU_KBN_136 = "136";     // WEB-DL区分3
        public const string SIKIBETU_KBN_137 = "137";     // 在庫区分CD
        public const string SIKIBETU_KBN_138 = "138";     // 操作モード
        public const string SIKIBETU_KBN_139 = "139";     // 納品書区分
        public const string SIKIBETU_KBN_140 = "140";     // カード区分
        public const string SIKIBETU_KBN_141 = "141";     // SKUFLG
        public const string SIKIBETU_KBN_144 = "144";     // 商品区分
        public const string SIKIBETU_KBN_147 = "147";     // 受入区分
        public const string SIKIBETU_KBN_149 = "149";     // 集計区分
        public const string SIKIBETU_KBN_150 = "150";     // 集計区分
        public const string SIKIBETU_KBN_175 = "175";     // お知らせ区分
        public const string SIKIBETU_KBN_176 = "176";     // お知らせ対象区分
        public const string SIKIBETU_KBN_151 = "151";     // 発注単位区分
        public const string SIKIBETU_KBN_152 = "152";     // 出荷単位区分
        public const string SIKIBETU_KBN_153 = "153";     // 小売単位区分
        public const string SIKIBETU_KBN_154 = "154";     // 予備項目
        public const string SIKIBETU_KBN_155 = "155";     // 受注明細承認区分
        public const string SIKIBETU_KBN_157 = "157";     // 受付予定
        public const string SIKIBETU_KBN_158 = "158";     // 受入種別
        public const string SIKIBETU_KBN_159 = "159";     // 特殊条件CD1
        public const string SIKIBETU_KBN_160 = "160";     // 特殊条件CD2
        public const string SIKIBETU_KBN_161 = "161";     // 特殊条件CD3
        public const string SIKIBETU_KBN_162 = "162";     // 特殊条件CD4
        public const string SIKIBETU_KBN_163 = "163";     // 特殊条件CD5
        public const string SIKIBETU_KBN_164 = "164";     // 特殊条件CD6
        public const string SIKIBETU_KBN_165 = "165";     // 特殊条件CD7
        public const string SIKIBETU_KBN_166 = "166";     // 特殊条件CD8
        public const string SIKIBETU_KBN_167 = "167";     // 特殊条件CD9
        public const string SIKIBETU_KBN_168 = "168";     // 特殊条件CD10
        public const string SIKIBETU_KBN_169 = "169";     // 受入状態
        public const string SIKIBETU_KBN_170 = "170";     // 伝票発注状態
        public const string SIKIBETU_KBN_171 = "171";     // 発注方法
        public const string SIKIBETU_KBN_172 = "172";     // 発注区分
        public const string SIKIBETU_KBN_173 = "173";     // 発注種別
        public const string SIKIBETU_KBN_174 = "174";     // 直送フラグ
        public const string SIKIBETU_KBN_177 = "177";     // 画面機能区分
        public const string SIKIBETU_KBN_179 = "179";     // 識別区分
        public const string SIKIBETU_KBN_191 = "191";     // 売上状態
        public const string SIKIBETU_KBN_192 = "192";     // 売上種別
        public const string SIKIBETU_KBN_193 = "193";     // 売上区分
        public const string SIKIBETU_KBN_194 = "194";     // 承認状態
        public const string SIKIBETU_KBN_195 = "195";     // 集計区分
        public const string SIKIBETU_KBN_196 = "196";     // Web回答状態
        public const string SIKIBETU_KBN_197 = "197";     // 回答状況区分
        public const string SIKIBETU_KBN_198 = "198";     // 予定納期区分
        public const string SIKIBETU_KBN_199 = "199";     // 発注明細状態
        // Public Const SIKIBETU_KBN_200 As String = "200"     '見積明細承認区分
        public const string SIKIBETU_KBN_201 = "201";     // 売上直送フラグ
        public const string SIKIBETU_KBN_202 = "202";     // 出荷指示区分
        public const string SIKIBETU_KBN_203 = "203";     // 返品理由
        public const string SIKIBETU_KBN_204 = "204";     // 仕入種別
        public const string SIKIBETU_KBN_205 = "205";     // 仕入区分
        public const string SIKIBETU_KBN_206 = "206";     // 仕入状態
        public const string SIKIBETU_KBN_207 = "207";     // 価格絞込
        public const string SIKIBETU_KBN_210 = "210";     // 売上訂正申請先区分
        public const string SIKIBETU_KBN_211 = "211";     // 仕入集計区分
        public const string SIKIBETU_KBN_213 = "213";     // 移動区分
        public const string SIKIBETU_KBN_214 = "214";     // 移動方法
        public const string SIKIBETU_KBN_215 = "215";     // 移動状態
        public const string SIKIBETU_KBN_218 = "218";     // 受注明細状態区分
        //public const string SIKIBETU_KBN_223 = "223";     // EDI印刷FLG
        public const string SIKIBETU_KBN_224 = "224";     // 締処理区分
        public const string SIKIBETU_KBN_225 = "225";     // 発行区分
        public const string SIKIBETU_KBN_226 = "226";     // 請求先伝票種別
        public const string SIKIBETU_KBN_228 = "228";     // お知らせメッセージ区分
        public const string SIKIBETU_KBN_232 = "232";     // 在庫調整理由
        public const string SIKIBETU_KBN_233 = "233";     // CSV用パス
        public const string SIKIBETU_KBN_234 = "234";     // 手形種類
        public const string SIKIBETU_KBN_235 = "235";     // 決済区分
        public const string SIKIBETU_KBN_236 = "236";     // 印刷順
        public const string SIKIBETU_KBN_237 = "237";     // 回収状態
        public const string SIKIBETU_KBN_238 = "238";     // 印刷順（支払）
        public const string SIKIBETU_KBN_239 = "239";     // 回収区分
        public const string SIKIBETU_KBN_252 = "252";     // 代表者FLG
        public const string SIKIBETU_KBN_257 = "257";     // 表示条件（倉庫別在庫照会）
        public const string SIKIBETU_KBN_259 = "259";     // 在庫調整可能FLG
        public const string SIKIBETU_KBN_260 = "260";     // 移動可能FLG
        public const string SIKIBETU_KBN_261 = "261";     // 仕入返品可能FLG
        public const string SIKIBETU_KBN_262 = "262";     // 受注引当状況
        public const string SIKIBETU_KBN_263 = "263";     // 受注納品指定
        public const string SIKIBETU_KBN_264 = "264";     // 返品申請書適用
        public const string SIKIBETU_KBN_265 = "265";     // 仕訳パターン区分
        public const string SIKIBETU_KBN_266 = "266";     // 在庫仕訳FLG
        public const string SIKIBETU_KBN_267 = "267";     // 入金支払種別
        public const string SIKIBETU_KBN_268 = "268";     // 売掛種別
        public const string SIKIBETU_KBN_269 = "269";     // 買掛種別
        public const string SIKIBETU_KBN_270 = "270";     // 仮伝FLG
        public const string SIKIBETU_KBN_272 = "272";     // 箱管理対象仕入先


        public const string SIKIBETU_KBN_NACCS_OMIT_ERR = "220";     // NACCS用ERR除外項目一覧
        public const string SIKIBETU_KBN_NACCS_ERR = "222";     // NACCS用ERR大項目
        public const string SIKIBETU_KBN_NACCS_ERR_SUB = "223";     // NACCS用ERR小項目

        public const string TEST_FLG_SHORI_SHIKIBETSU = "ShoriShikibetsuFlg";

        //取込状態
        public struct TORIKOMI_JOTAI {
            public const string MISHORI = "1";           //未処理
            public const string SHINKOKU_MACHI = "2";    //NACCS申告待
            public const string PAX_ERROR = "3";         //PAXエラー
            public const string KYOKA_MACHI = "4";       //NACCS許可待
            public const string NACCS_ERROR = "5";      //NACCSエラー
            public const string KYOKA_ZUMI = "6";       //輸入許可済
        }; 


        //public struct E_MIC_RESULT
        //{
        //    public static string 正常 = "1";
        //    public static string 異常 = "2";
        //    public static string 失敗 = "3";
        //    public static string 未処理 = "9";
        //}

        //public struct E_MIC_SHOKAI_KBN
        //{
        //    public static string 未登録 = "0";
        //    public static string 未送信 = "1";
        //    public static string 問合中 = "2";
        //    public static string 異常 = "3";
        //    public static string 正常 = "4";
        //    public static string 問合失敗 = "5";
        //}

    }
}
