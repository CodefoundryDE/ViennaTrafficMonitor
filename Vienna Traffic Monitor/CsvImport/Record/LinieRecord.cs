using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using System.CodeDom.Compiler;

namespace ViennaTrafficMonitor.CsvImport.Record {
    [IgnoreFirst(1)]
    [IgnoreEmptyLines()]
    [DelimitedRecord(";")]
    [GeneratedCodeAttribute("CSV to  C#", "")]
    public sealed class LinieRecord {

        private int mId;

        public int Id {
            get { return mId; }
            set { mId = value; }
        }


        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mBezeichnung;

        public String Bezeichnung {
            get { return mBezeichnung; }
            set { mBezeichnung = value; }
        }


        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private int mReihenfolge;

        public int Reihenfolge {
            get { return mReihenfolge; }
            set { mReihenfolge = value; }
        }


        private Boolean mEchtzeit;

        public Boolean Echtzeit {
            get { return mEchtzeit; }
            set { mEchtzeit = value; }
        }


        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mVerkehrsmittel;

        public String Verkehrsmittel {
            get { return mVerkehrsmittel; }
            set { mVerkehrsmittel = value; }
        }


        [FieldQuoted('"', QuoteMode.OptionalForRead, MultilineMode.AllowForRead)]
        private String mStand;

        public String Stand {
            get { return mStand; }
            set { mStand = value; }
        }



    }
}