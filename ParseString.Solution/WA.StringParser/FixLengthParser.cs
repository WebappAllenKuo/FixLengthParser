using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WA.StringParser
{
    /// <summary>
    /// 將一行big5編碼的文字, 用固定長度切割
    /// </summary>
    public static class FixLengthParser
    {
        /// <summary>
        /// 解析 Big5編碼的純文字檔案, 並逐行依指定長度解析
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="columnLengths"></param>
        /// <returns></returns>
	    public static List<string[]> ParseBig5File(string filePath, int[] columnLengths)
	    {
		    if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
		    if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException(nameof(filePath)); 
		    
		    if (columnLengths == null) throw new ArgumentNullException(nameof(columnLengths));
            if (columnLengths.Length == 0) throw new ArgumentOutOfRangeException(nameof(columnLengths));
            if(columnLengths.Any(item => item == 0)) throw new ArgumentOutOfRangeException($"{nameof(columnLengths)} 裡的長度不可以小於等於零");
            
            var result = new List<string[]>();
            foreach (var singleLine in System.IO.File.ReadAllLines(filePath))
            {
	         result.Add(ParseSingleLine(singleLine, columnLengths));   
            }

            return result;
	    }

        /// <summary>
        /// 將一段 Big5編碼的字串,根據各欄位bytes數量拆解成多個欄位
        /// </summary>
        /// <param name="columnLengths"></param>
        /// <returns></returns>
	    public static string[] ParseSingleLine(string source, int[] columnLengths)
	    {
            if(string.IsNullOrWhiteSpace(source)) return new string[0];

            byte[] bytes = Encoding.GetEncoding(950).GetBytes(source);

            var result = new string[columnLengths.Length];
            int index = 0;
            int startIndex = 0;
            
            foreach (var columnLength in columnLengths)
            {
                if(startIndex > bytes.Length-1)break;
                
	            int endIndex =Math.Min( startIndex + columnLength, bytes.Length)-1;
	            string columnValue = Encoding.GetEncoding(950).GetString(bytes, startIndex, endIndex - startIndex + 1);

	            result[index] = columnValue;

	            startIndex += columnLength;
	            index++;
            }

            return result;
	    }
    }
}
