using System;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Statex
{
    public class TvActionResult
    {
        private static readonly TvActionResult _ok = new TvActionResult(isOk: true);

        private readonly List<Exception> _errors;

        public IEnumerable<Exception> Errors => _isOk ? Enumerable.Empty<Exception>() : _errors;
        private readonly bool _isOk;

        private TvActionResult(bool isOk)
        {
            _isOk = isOk;

            if (!_isOk)
            {
                _errors = new List<Exception>();
            }
        }

        public static TvActionResult OK => _ok;
        public static TvActionResult Failed() => new TvActionResult(isOk: false);

        public void AddError(Exception ex)
        {
            if (_isOk)
            {
                throw new InvalidOperationException("Can't add error to a succesful result!");
            }

            _errors.Add(ex);
        }

    }
}
