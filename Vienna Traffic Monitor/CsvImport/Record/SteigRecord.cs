using FileHelpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.CsvImport.Parser {
    [IgnoreFirst(1)]
    [IgnoreEmptyLines()]
    [DelimitedRecord(";")]
    [GeneratedCodeAttribute("CSV to  C#", "")]
    public sealed class SteigRecord {

        private int mId;
        public int Id {
            get { return mId; }
            set { mId = value; }
        }

        private int mLinienId;
        public int LinienId {
            get { return mLinienId; }
            set { mLinienId = value; }
        }

        private int mHaltestellenId;
        public int HaltestellenId {
            get { return mHaltestellenId; }
            set { mHaltestellenId = value; }
        }

        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mRichtung;
        public String Richtung {
            get { return mRichtung; }
            set { mRichtung = value; }
        }

        private int mReihenfolge;
        public int Reihenfolge {
            get { return mReihenfolge; }
            set { mReihenfolge = value; }
        }

        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        [FieldNullValue(0)]
        private int mRbl;
        public int Rbl {
            get { return mRbl; }
            set { mRbl = value; }
        }

        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        [FieldNullValue(0)]
        private int mBereich;
        public int Bereich {
            get { return mBereich; }
            set { mBereich = value; }
        }

        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mName;
        public String Name {
            get { return mName; }
            set { mName = value; }
        }

        private Double mXKoord;
        public Double XKoord {
            get { return mXKoord; }
            set { mXKoord = value; }
        }

        private Double mYKoord;
        public Double YKoord {
            get { return mYKoord; }
            set { mYKoord = value; }
        }

        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mStand;
        public String Stand {
            get { return mStand; }
            set { mStand = value; }
        }

    }
}
