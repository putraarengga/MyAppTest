using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyApp
{
    public partial class Form1 : Form
    {
        bool addDollars = false;
        String frontNumber = null, backNumber = null;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            // remove leading zero
            txbResult.Text = ParseWholeNumber(txbNumber.Text.TrimStart(new Char[] { '0' }));
        }

        String ParseWholeNumber(string str)
        {
            String word = null, tmpString=null;
            int charsCount = 0;
            int index = 0;
            int blockCount = 0;
            int firstBlockLength = 0;

            // normalize format
            if (str.Contains('.'))
            {
                frontNumber = str.Split('.')[0];
                backNumber = str.Split('.')[1];

                if (frontNumber == "")
                    frontNumber = "0";
                if (backNumber.Count() < 2)
                    backNumber += "0";
            }
            else
            {
                frontNumber = str;
                backNumber = "00";
            }

            // blockCount is all number divided by 3 number eg. 10,000 is 1 blocks
            charsCount = frontNumber.Count();
            blockCount = charsCount / 3;            
            
            // get first block length eg. 10000 is 10  equal 2 numbers
            // if result = 0 firstBlockLength is 3 eg. 100000  is 100 equal 3 numbers
            if (charsCount % 3 == 0)
            {
                firstBlockLength = 3;
                blockCount -= 1;
            }
            else
                firstBlockLength = charsCount % 3;

            // Process Dollars
            // get numbers in front number with length is firstblockLength
            tmpString = frontNumber.Substring(0, firstBlockLength);

            // loop every block
            while (index <= blockCount)
            {
                // Dont add "DOLLARS" if frontnumber is 0
                if(frontNumber != "0")
                    addDollars = true;

                // Parse Block Number
                word += ParseBlock(tmpString, blockCount - index);
                
                // Set Up next Block Number
                if (index < blockCount)
                {
                    tmpString = str.Substring(firstBlockLength + (index * 3), 3);
                }
                index++;
            }

            // Process Cents
            // value -1 to add word "CENTS" 
            tmpString = backNumber;
            word += ParseBlock(tmpString, -1);
            return word;
        }

        String AddShortScaleNumbers(int illion)
        {
            String word = null;
            switch (illion)
            {
                case -1: word = " CENTS "; break;
                case 0: if(addDollars) word = " DOLLARS "; break;
                case 1:     word = " THOUSAND, "; break;
                case 2:     word = " MILLION, "; break;
                case 3:     word = " BILLION, "; break;
                case 4:     word = " TRILLION, "; break;
                case 5:     word = " QUADRILLION, "; break;
                case 6:     word = " QUINTILLION, "; break;
                case 7:     word = " SEXTILLION, "; break;
                case 8:     word = " SEPTILLION, "; break;
                case 9:     word = " OCTILLION, "; break;
                case 10:    word = " NONILLION, "; break;
                case 11:    word = " DECIILLION, "; break;
                case 12:    word = " UNDECILLION, "; break;
                case 13:    word = " DUODECILLION, "; break;
                case 14:    word = " TREDECILLION, "; break;
                case 15:    word = " QUATTUORDECILLION, "; break;
                case 16:    word = " QUINDECILLION, "; break;
                case 17:    word = " SEXDECILLION, "; break;
                case 18:    word = " SEPTDECILLION, "; break;
                case 19:    word = " OCTODECILLION, "; break;
                case 20:    word = " NOVEMDECILLION, "; break;
                case 21:    word = " VIGINTILLION, "; break;
            }
            //word += Environment.NewLine;
            return word;
        }

        // Parse Block Number 
        String ParseBlock(string str, int shortScaleNumber)
        {
            string word = null;
            // Parse Hundred first
            if (str.Count() == 3)
            {
                word = ParseHundred(str.Substring(0, 1));
                

                if (str.Substring(1, 2) != "00")
                {
                    // hundreds value not 0
                    if (word != null)
                        word += " AND ";

                    if (str.Substring(1, 1) == "1")
                        word += ParseTeen(str.Substring(1, 2));
                    else if (str.Substring(1, 1) == "0")
                        word += ParseOne(str.Substring(2, 1));
                    else
                    {
                        word += ParseTens(str.Substring(1, 1));
                        word += ParseOne(str.Substring(2, 1));
                    }
                }
            }
            else if (str.Count() == 2)
            {
                if (str.Substring(0, 2) != "00")
                {
                    if (shortScaleNumber == -1 && frontNumber != "0" )
                        word += " AND ";

                    if (str.Substring(0, 1) == "0")
                        word += ParseOne(str.Substring(1, 1));
                    else if (str.Substring(0, 1) == "1")
                        word += ParseTeen(str.Substring(0, 2));
                    else
                    {
                        word += ParseTens(str.Substring(0, 1));
                        word += ParseOne(str.Substring(1, 1));
                    }
                }
            }
            else if (str.Count() == 1)
            {
                word += ParseOne(str);
            }

            // Assign Dollar, Dollars, Cent or Cents
            if (frontNumber == "1" && shortScaleNumber == 0)
                word += " DOLLAR ";
            else if (backNumber == "01" && shortScaleNumber == -1)
                word += " CENT";
            else
            {
                if (word != null || shortScaleNumber == 0)
                    word += AddShortScaleNumbers(shortScaleNumber);
            }

            return word;
        }

        String ParseHundred(string str)
        {
            int numb = Convert.ToInt32(str);
            String word = null;
            if (numb > 0)
                word = ParseOne(str) + " HUNDRED";
            return word ;
        }

        String ParseOne(string str)
        {
            int numb = Convert.ToInt32(str);
            String word = null;
            switch(numb)
            {
                case 1:
                    word = "ONE"; break;
                case 2:
                    word = "TWO"; break;
                case 3:
                    word = "THREE"; break;
                case 4:
                    word = "FOUR"; break;
                case 5:
                    word = "FIVE"; break;
                case 6:
                    word = "SIX"; break;
                case 7:
                    word = "SEVEN"; break;
                case 8:
                    word = "EIGHT"; break;
                case 9:
                    word = "NINE"; break;
            }

            return word;
        }
        String ParseTeen(String str)
        {
            int numb = Convert.ToInt32(str);
            String word = null;
            switch (numb)
            {
                case 10:
                    word = "TEN"; break;
                case 11:
                    word = "ELEVEN"; break;
                case 12:
                    word = "TWELVE"; break;
                case 13:
                    word = "THIRTEEN"; break;
                case 14:
                    word = "FOURTEEN"; break;
                case 15:
                    word = "FIFTEEN"; break;
                case 16:
                    word = "SIXTEEN"; break;
                case 17:
                    word = "SEVENTEEN"; break;
                case 18:
                    word = "EIGHTEEN"; break;
                case 19:
                    word = "NINETEEN"; break;
            }
            return word;
        }


        String ParseTens(String str)
        {
            int numb = Convert.ToInt32(str);
            String word = null;
            switch(numb)
            {
                case 2:
                    word = "TWENTY-"; break;
                case 3:
                    word = "THIRTY-"; break;
                case 4:
                    word = "FORTY-"; break;
                case 5:
                    word = "FIFTY-"; break;
                case 6:
                    word = "SIXTY-"; break;
                case 7:
                    word = "SEVENTY-"; break;
                case 8:
                    word = "EIGHTY-"; break;
                case 9:
                    word = "NINETY-"; break;
            }
            return word;
        }

        // Only Accepts Numbers
        private void txbNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) 
            {
                e.Handled = true;
            }
        }
        
    }
}
