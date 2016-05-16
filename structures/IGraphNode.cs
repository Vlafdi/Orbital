using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital.structures {
    interface IGraphNode<T> {
        double GetDistance(T target);
    }
}
