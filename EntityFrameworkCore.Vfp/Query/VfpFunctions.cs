using System;

namespace EntityFrameworkCore.Vfp.Query {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This is not an executable class.")]
    public static class VfpFunctions {
        private const string NotSupportedExceptionMessage = "This function can only be invoked from LINQ.";

        public static int? Ascii(string string1) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Chr(int? number) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static int? At(string cSearchExpression, string cExpressionSearched) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static int? At(string cSearchExpression, string cExpressionSearched, int? nOccurrence) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static long? At(string cSearchExpression, string cExpressionSearched, long? nOccurrence) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static int? Atc(string cSearchExpression, string cExpressionSearched) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static int? Atc(string cSearchExpression, string cExpressionSearched, int? nOccurrence) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static long? Atc(string cSearchExpression, string cExpressionSearched, long? nOccurrence) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Strtran(string cSearched, string cExpressionSought, string cReplacement) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Strtran(string cSearched, string cExpressionSought, string cReplacement, int? nStartOccurrence) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Strtran(string cSearched, string cExpressionSought, string cReplacement, int? nStartOccurrence, int? nNumberOfOccurrences) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Strtran(string cSearched, string cExpressionSought, string cReplacement, int? nStartOccurrence, int? nNumberOfOccurrences, int? nFlags) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Replicate(string strTarget, int? count) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string AllTrim(string str) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Space(int? arg1) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Str(double? number) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Str(decimal? number) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Str(double? number, int? length) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Str(decimal? number, int? length) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Str(double? number, int? length, int? decimalPlaces) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Str(decimal? number, int? length, int? decimalPlaces) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Stuff(string strInput, int? start, int? length, string strReplacement) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Substr(string str, long? start, long? length) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Acos(double? arg1) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Acos(decimal? arg1) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Asin(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Asin(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Atan(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Atan(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Atn2(double? arg1, double? arg2) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Atn2(decimal? arg1, decimal? arg2) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Cos(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Cos(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static decimal? Rtod(decimal? arg1) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Rtod(double? arg1) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Exp(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Exp(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Log(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Log(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Log10(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Log10(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static int? Dtor(int? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static long? Dtor(long? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static decimal? Dtor(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Dtor(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Rand(int? seed) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static int? Sign(int? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static long? Sign(long? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static decimal? Sign(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Sign(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Sin(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Sin(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Sqrt(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Sqrt(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Tan(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static double? Tan(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string Cdow(DateTime? date) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static string CMonth(DateTime? date) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static int? Sec(DateTime? date) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? IsDigit(string arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? Empty(string arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? Empty(decimal? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? Empty(DateTime? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? Empty(bool? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? Empty(double? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? Empty(int? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? Empty(long? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
        public static bool? Empty(float? arg) => throw new NotSupportedException(NotSupportedExceptionMessage);
    }
}
