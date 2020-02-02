using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Linq;

namespace JT808.Gateway.Traffic
{
    public interface IJT808Traffic
    {
        long Get(string key);
        long Increment(string terminalNo, string field, int len);
        List<(string,long)> GetAll();
    }

    public class JT808TrafficDefault : IJT808Traffic
    {
        private ConcurrentDictionary<string, long> dict = new ConcurrentDictionary<string, long>();

        public long Get(string key)
        {
            long value;
            dict.TryGetValue(key, out value);
            return value;
        }

        public List<(string, long)> GetAll()
        {
            return dict.Select(s => (s.Key, s.Value)).ToList();
        }

        public long Increment(string terminalNo, string field, int len)
        {
            return dict.AddOrUpdate($"{terminalNo}_{field}", len, (id, count) => count + len);
        }
    }
}
