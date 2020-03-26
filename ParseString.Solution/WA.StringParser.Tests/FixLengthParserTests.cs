using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace WA.StringParser.Tests
{
    [TestFixture]
    public class FixLengthParserTests
    {
	    [TestCase(null)]
	    [TestCase("")]
	    [TestCase(" ")]
		public void ParseSingleLine_空字串_傳回空集合(string source)
		{
			int[] columnLengths = {7,3 };
			var result = FixLengthParser.ParseSingleLine(source, columnLengths);

			result.Should().BeEmpty();
		}

		[TestCase("ab")]
		[TestCase("abc")]
		[TestCase("abcd")]
		public void ParseSingleLine_純英文_文字長度不拘都能解析(string source)
		{
			int[] columnLengths = { 3, 5 };
			int expectedColumns = columnLengths.Length;
			var result = FixLengthParser.ParseSingleLine(source, columnLengths);

			result.Should().HaveCount(c => c == expectedColumns);
		}

		[Test]
		public void ParseSingleLine_純英文_驗證可解析成2欄()
		{
			string source = "abcd";
			int[] columnLengths = { 3, 5 };
			
			var result = FixLengthParser.ParseSingleLine(source, columnLengths);

			result[0].Should().Be("abc");
			result[1].Should().Be("d");

		}

		[Test]
		public void ParseSingleLine_純中文_驗證可解析成2欄()
		{
			string source = "王小明李小華";
			int[] columnLengths = { 6, 4 };

			var result = FixLengthParser.ParseSingleLine(source, columnLengths);

			result[0].Should().Be("王小明");
			result[1].Should().Be("李小");

		}

		[Test]
		public void ParseSingleLine_中英文混合_驗證可解析成2欄()
		{
			string source = "王abc123";
			int[] columnLengths = { 6, 4 };

			var result = FixLengthParser.ParseSingleLine(source, columnLengths);

			result[0].Should().Be("王abc1");
			result[1].Should().Be("23");
		}

		[Test]
		public void ParseBig5File_WhenCalled()
		{
			string filePath = @"D:\temp\big5.txt";
			int[] columnLengths = { 7, 3 };
			var result = FixLengthParser.ParseBig5File(filePath, columnLengths);

			result.Should().HaveCount(c => c == 2);

			result[0][0].Should().Be("中文eng");
			result[0][1].Should().Be("123");

			result[1][0].Should().Be("abcd   ");
			result[1][1].Should().Be("456");
		}
	}
}
