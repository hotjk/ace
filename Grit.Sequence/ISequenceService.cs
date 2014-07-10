using System;

namespace Grit.Sequence
{
    public interface ISequenceService
    {
        int Next(int sequence, int step);
    }
}
