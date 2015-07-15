using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semiodesk.VirtuosoInstrumentation.Configuration
{

    public class Striping
    {
        /// <summary>
        /// Segment<number> = <size>, <stripe file name> [, <stripe file name> .. ]
        /// <number> must be ordered from 1 upwards; The <size> is the size of the segment which is equally divided across all stripes comprising the segment.
        ///
        /// This section is only effective when Striping = 1 is set in the [Database] section. When striping is enabled the Virtuoso database spans multiple segments where each segment can have multiple stripes.
        /// Striping can be used to distribute the database across multiple locations of a file system for performance. Segmentation can be used for expansion or dealing with file size limitations. To allow for database growth when a file system is near capacity or near the OS file size limitation, new segments can be added on the same or other file systems.
        /// Striping only has a potential performance benefit when striped across multiple devices. Striping on the same file system is needless and unlikely to alter performance, however, multiple segments do provide convenience and flexibility as described above. Striping across partitions on the same device is likely to reduce performance by causing high unnecessary seek times on the physical disk.
        /// Database segments are pre-allocated as defined. This can reduce the potential for file fragmentation which could also provide some performance benefit.
        /// Virtuoso striping alone does not allow for any fault tolerance. This is best handled at the I/O layer or by the operating system. File system RAID with fault-tolerant striping defined should be used to host the Virtuoso files if striping based protection is desired.
        /// The segments are numbered, their segment <number> must be specified in order starting with segment1.
        /// The <size> is the total size of the segment which is that will be divided equally across all stripes comprising the segment. Its specification can be in gigabytes (g), megabytes (m), kilobytes (k) or in database blocks (b) the default.
        /// </summary>
        public List<string> Segments { get; set; }
    }
}
