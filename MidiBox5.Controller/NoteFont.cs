using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;
using System.Drawing;
using System.Drawing.Text;
namespace MidiBox5.Controller
{
    public static class NoteFont
    {
        public static void Load()
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(Environment.CurrentDirectory + "\\Polihymnia.ttf");

            FontStyles.MusicFont = new Font(pfc.Families[0], 20);
            FontStyles.GraceNoteFont = new Font(pfc.Families[0], 18);
            FontStyles.StaffFont = new Font(pfc.Families[0], 23);
            /*
        public static Font GraceNoteFont = new Font("Polihymnia", 18);
        public static Font StaffFont = new Font("Polihymnia", 23);
        public static Font LyricFont = new Font("Times New Roman", 8);
        public static Font LyricFontBold = new Font("Times New Roman", 10, FontStyle.Bold);
        public static Font MiscArticulationFont = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
        public static Font DirectionFont = new Font("Microsoft Sans Serif", 9, FontStyle.Italic | FontStyle.Bold);
        public static Font TrillFont = new Font("Times New Roman", 9, FontStyle.Italic | FontStyle.Bold);
        public static Font TimeSignatureFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);*/
        }
    }
}
