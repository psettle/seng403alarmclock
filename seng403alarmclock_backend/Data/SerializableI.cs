using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace seng403alarmclock_backend.Data {
    public interface SerializableI {
        string ToXML();
        void FromXML(string xml);
    }
}
