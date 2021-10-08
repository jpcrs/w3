using System;

namespace w3.Hotkeys;

public static class StringExtensions
{
    public static LineSplitEnumerator SplitLines(this string str)
    {
        return new LineSplitEnumerator(str.AsSpan());
    }

    // Must be a ref struct as it contains a ReadOnlySpan<char>
    public ref struct LineSplitEnumerator
    {
        private ReadOnlySpan<char> _str;
        public ReadOnlySpan<char> Current { get; private set; }
        
        public LineSplitEnumerator(ReadOnlySpan<char> str)
        {
            _str = str;
            Current = default;
        }

        public LineSplitEnumerator GetEnumerator() => this;

        public bool MoveNext()
        {
            var span = _str;
            if (span.Length == 0) // Reach the end of the string
                return false;

            var index = span.IndexOfAny('\r', '\n');
            if (index == -1) // The string is composed of only one line
            {
                _str = ReadOnlySpan<char>.Empty; // The remaining string is an empty string
                Current = span;
                return true;
            }

            if (index < span.Length - 1 && span[index] == '\r')
            {
                var next = span[index + 1];
                if (next == '\n')
                {
                    Current = span.Slice(0, index);
                    _str = span.Slice(index + 2);
                    return true;
                }
            }

            Current = span.Slice(0, index);
            _str = span.Slice(index + 1);
            return true;
        }
    }
}
