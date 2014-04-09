using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViennaTrafficMonitor.CsvImport.Record
{
    [IgnoreFirst(1)]
    [IgnoreEmptyLines()]
    [DelimitedRecord(";")]
    public sealed class HaltestelleRecord
    {

        private int mId;

        public int Id
        {
            get { return mId; }
            set { mId = value; }
        }


        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mTyp;

        public String Typ
        {
            get { return mTyp; }
            set { mTyp = value; }
        }


        private int mDiva;

        public int Diva
        {
            get { return mDiva; }
            set { mDiva = value; }
        }


        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mName;

        public String Name
        {
            get { return mName; }
            set { mName = value; }
        }


        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mGemeinde;

        public String Gemeinde
        {
            get { return mGemeinde; }
            set { mGemeinde = value; }
        }


        private int mGemeindeId;

        public int GemeindeId
        {
            get { return mGemeindeId; }
            set { mGemeindeId = value; }
        }


        private Double mXKoord;

        public Double XKoord
        {
            get { return mXKoord; }
            set { mXKoord = value; }
        }


        private Double mYKoord;

        public Double YKoord
        {
            get { return mYKoord; }
            set { mYKoord = value; }
        }


        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mStand;

        public String Stand
        {
            get { return mStand; }
            set { mStand = value; }
        }
    }
}
